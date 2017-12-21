/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 ******************************************************************************/

//! Module for parsers' automata

use super::LRActionCode;
use super::LROpCode;
use super::TreeAction;
use super::LR_ACTION_CODE_REDUCE;
use super::LR_ACTION_CODE_SHIFT;
use super::TREE_ACTION_NONE;
use super::TREE_ACTION_REPLACE;
use super::super::symbols::Symbol;
use super::super::utils::bin::*;

/// A cell in a column map for non-cached identifiers
#[derive(Copy, Clone)]
struct LRColumnMapCell {
    /// The symbol identifier
    identifier: u16,
    /// The column in the LR table
    column: u16
}

/// Represent a map from symbols' IDs to the index of their corresponding column in an LR table.
/// It is optimized for IDs from 0x0000 to 0x01FF (the first 512 symbols) with hope they are the most frequent.
struct LRColumnMap {
    /// The cache for Ids from 0x0000 to 0x01FF
    cache: [u16; 512],
    /// Mapping for the other symbols
    others: Vec<LRColumnMapCell>
}

impl LRColumnMap {
    /// Creates and loads a new column map
    pub fn new(data: &[u8], offset: usize, column_count: usize) -> LRColumnMap {
        let mut result = LRColumnMap {
            cache: [0; 512],
            others: Vec::<LRColumnMapCell>::new()
        };
        for i in 0..column_count {
            let index = offset + i * 2;
            let identifier = read_u16(data, index);
            result.add(i as u16, identifier);
        }
        result
    }

    /// Adds a new mapping to the map
    fn add(&mut self, column: u16, identifier: u16) {
        if identifier <= 0x1FF {
            self.cache[identifier as usize] = column;
        } else {
            self.others.push(LRColumnMapCell { identifier, column });
        }
    }

    /// Gets the column for the specified symbol identifier
    pub fn get(&self, identifier: u32) -> u16 {
        if identifier <= 0x01FF {
            self.cache[identifier as usize]
        } else {
            for i in 0..self.others.len() {
                if self.others[i].identifier as u32 == identifier {
                    return self.others[i].column;
                }
            }
            return 0;
        }
    }
}

/// Represents a context opening by a transition from a state
#[derive(Copy, Clone)]
struct LRContextOpening {
    /// The identifier of the symbol on the transition
    identifier: u16,
    /// The context being opened
    context: u16
}

/// Represents the contexts opening by transitions from a state

pub struct LRContexts {
    /// The contexts, if any
    openings: Option<Vec<LRContextOpening>>
}

impl LRContexts {
    /// Initializes empty contexts
    pub fn new() -> LRContexts {
        LRContexts { openings: None }
    }

    /// Registers a new context
    pub fn add(&mut self, identifier: u16, context: u16) {
        match self.openings {
            None => {
                let mut content = Vec::<LRContextOpening>::new();
                content.push(LRContextOpening { identifier, context });
                self.openings = Some(content);
            },
            Some(ref mut data) => {
                data.push(LRContextOpening { identifier, context });
            }
        }
    }

    /// Gets whether the specified context opens by a transition using the specified terminal ID
    pub fn opens(&self, terminal_id: u32, context: u16) -> bool {
        match self.openings {
            None => false,
            Some(ref data) => {
                for x in data.iter() {
                    if x.identifier as u32 == terminal_id && x.context == context {
                        return true;
                    }
                }
                false
            }
        }
    }
}

/// Represents a LR action in a LR parse table
#[derive(Copy, Clone)]
pub struct LRAction<'a> {
    /// The automaton table
    table: &'a [u16],
    /// The offset of this state within the table
    offset: usize
}

impl<'a> LRAction<'a> {
    /// Gets the action code
    pub fn get_code(&self) -> LRActionCode {
        self.table[self.offset]
    }

    /// Gets the data associated with the action
    /// If the code is Reduce, it is the index of the LRProduction
	/// If the code is Shift, it is the index of the next state
    pub fn get_data(&self) -> u16 {
        self.table[self.offset + 1]
    }
}

/// Represents a rule's production in a LR parser
/// The binary representation of a LR Production is as follow:
/// --- header
/// u16: head's index
/// u8: 1=replace, 0=nothing
/// u8: reduction length
/// u8: bytecode length in number of op-code
/// --- production's bytecode
/// array of LROpCode
pub struct LRProduction {
    /// Index of the rule's head in the parser's array of variables
    head: usize,
    /// Action of the rule's head (replace or not)
    head_action: TreeAction,
    /// Size of the rule's body by only counting terminals and variables
    reduction_length: usize,
    /// Bytecode for the rule's production
    bytecode: Vec<LROpCode>
}

impl LRProduction {
    /// Creates and loads a production
    pub fn new(data: &[u8], index: &mut usize) -> LRProduction {
        let head = read_u16(data, *index) as usize;
        *index += 2;
        let head_action = if data[*index] == 1 { TREE_ACTION_REPLACE } else { TREE_ACTION_NONE };
        *index += 1;
        let reduction_length = data[*index] as usize;
        *index += 1;
        let bytecode_length = data[*index] as usize;
        *index += 1;
        let mut bytecode = Vec::<LROpCode>::with_capacity(bytecode_length);
        for i in 0..bytecode_length {
            bytecode.push(read_u16(data, *index + i * 2));
        }
        *index += bytecode_length * 2;
        LRProduction {
            head,
            head_action,
            reduction_length,
            bytecode
        }
    }
}

/// Container for the expected terminals for a LR state
pub struct LRExpected {
    /// The terminals expected for shift actions
    pub shifts: Vec<Symbol>,
    /// The terminals expected for reduction actions
    pub reductions: Vec<Symbol>
}

impl LRExpected {
    /// Initializes this container
    pub fn new() -> LRExpected {
        LRExpected {
            shifts: Vec::<Symbol>::new(),
            reductions: Vec::<Symbol>::new()
        }
    }

    /// Adds the specified terminal as expected on a shift action
    /// If the terminal is already added to the reduction collection it is removed from it.
    pub fn add_unique_shift(&mut self, terminal: Symbol) {
        let position = self.reductions.iter().position(|x| *x == terminal);
        if position.is_some() {
            self.reductions.remove(position.unwrap());
        }
        if !self.shifts.contains(&terminal) {
            self.shifts.push(terminal);
        }
    }

    /// Adds the specified terminal as expected on a reduction action
    /// If the terminal is in the shift collection, nothing happens.
    pub fn add_unique_reduction(&mut self, terminal: Symbol) {
        if !self.shifts.contains(&terminal) && !self.reductions.contains(&terminal) {
            self.reductions.push(terminal);
        }
    }
}

/// Represents the LR(k) parsing table and productions
pub struct LRkAutomaton {
    /// The number of columns in the LR table
    columns_count: usize,
    /// The number of states in the LR table
    states_count: usize,
    /// Map of symbol ID to column index in the LR table
    columns_map: LRColumnMap,
    /// The contexts information
    contexts: Vec<LRContexts>,
    /// The LR table
    table: Vec<u16>,
    /// The table of LR productions
    productions: Vec<LRProduction>
}

impl LRkAutomaton {
    /// Initializes a new automaton from the given binary data
    pub fn new(data: &[u8]) -> LRkAutomaton {
        let columns_count = read_u16(data, 0) as usize;
        let states_count = read_u16(data, 2) as usize;
        let productions_count = read_u16(data, 4) as usize;
        let columns_map = LRColumnMap::new(data, 6, columns_count);
        let mut contexts = Vec::<LRContexts>::with_capacity(states_count);
        let mut index = 6 + columns_count * 2;
        for _i in 0..states_count {
            let mut context = LRContexts::new();
            let count = read_u16(data, index);
            index += 2;
            for _j in 0..count {
                context.add(read_u16(data, index), read_u16(data, index + 2));
                index += 4
            }
            contexts.push(context);
        }
        let table = read_table_u16(data, index, states_count * columns_count * 2);
        index += states_count * columns_count * 4;
        let mut productions = Vec::<LRProduction>::with_capacity(productions_count);
        for _i in 0..productions_count {
            let production = LRProduction::new(data, &mut index);
            productions.push(production);
        }
        LRkAutomaton {
            columns_count,
            states_count,
            columns_map,
            contexts,
            table,
            productions
        }
    }

    /// Gets the number of states in this automaton
    pub fn get_states_count(&self) -> usize {
        self.states_count
    }

    /// Gets the contexts opened by the specified state
    pub fn get_contexts(&self, state: u32) -> &LRContexts {
        &self.contexts[state as usize]
    }

    /// Gets the LR(k) action for the given state and sid
    pub fn get_action(&self, state: u32, identifier: u32) -> LRAction {
        let column = self.columns_map.get(identifier) as usize;
        LRAction {
            table: &self.table,
            offset: state as usize * self.columns_count + column
        }
    }

    /// Gets the i-th production
    pub fn get_production(&self, index: usize) -> &LRProduction {
        &self.productions[index]
    }

    /// Gets the expected terminals for the specified state
    pub fn get_expected(&self, state: u32, terminals: &'static Vec<Symbol>) -> LRExpected {
        let mut expected = LRExpected::new();
        let mut offset = self.columns_count * state as usize * 2;
        for terminal in terminals.iter() {
            let action = self.table[offset];
            if action == LR_ACTION_CODE_SHIFT {
                expected.shifts.push(*terminal);
            } else if action == LR_ACTION_CODE_REDUCE {
                expected.reductions.push(*terminal);
            }
            offset += 2;
        }
        expected
    }
}