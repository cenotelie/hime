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

//! Module for parsers API

pub mod subtree;
pub mod lrk;

use super::lexers::ContextProvider;
use super::symbols::Symbol;
use super::utils::bin::*;

/// The maximum number of errors
pub const MAX_ERROR_COUNT: usize = 100;

/// Represents a tree action
pub type TreeAction = u16;

/// Keep the node as is
pub const TREE_ACTION_NONE: TreeAction = 0;
/// Replace the node with its children
pub const TREE_ACTION_REPLACE: TreeAction = 1;
/// Drop the node and all its descendants
pub const TREE_ACTION_DROP: TreeAction = 2;
/// Promote the node, i.e. replace its parent with it and insert its children where it was
pub const TREE_ACTION_PROMOTE: TreeAction = 3;

/// Represent an op-code for a LR production
/// An op-code can be either an instruction or raw data
pub type LROpCode = u16;

/// Pop an AST from the stack
pub const LR_OP_CODE_BASE_POP_STACK: LROpCode = 0;
/// Add a virtual symbol
pub const LR_OP_CODE_BASE_ADD_VIRTUAL: LROpCode = 1 << 2;
/// Execute a semantic action
pub const LR_OP_CODE_BASE_SEMANTIC_ACTION: LROpCode = 1 << 3;
/// Add a null variable
/// This can be found only in RNGLR productions
pub const LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE: LROpCode = 1 << 4;

/// Gets the base LR op-code
pub fn get_op_code_base(op_code: LROpCode) -> LROpCode {
    op_code & 0xFFFC
}

/// Gets the tree action encoded in the specified LR op-code
pub fn get_op_code_tree_action(op_code: LROpCode) -> TreeAction {
    op_code & 0x0003
}

/// Represents an action in a LR parser
pub type LRActionCode = u16;

/// No possible action => Error
pub const LR_ACTION_CODE_NONE: LRActionCode = 0;
/// Apply a reduction
pub const LR_ACTION_CODE_REDUCE: LRActionCode = 1;
/// Shift to another state
pub const LR_ACTION_CODE_SHIFT: LRActionCode = 2;
/// Accept the input
pub const LR_ACTION_CODE_ACCEPT: LRActionCode = 3;

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
                content.push(LRContextOpening {
                    identifier,
                    context
                });
                self.openings = Some(content);
            }
            Some(ref mut data) => {
                data.push(LRContextOpening {
                    identifier,
                    context
                });
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
    pub head: usize,
    /// Action of the rule's head (replace or not)
    pub head_action: TreeAction,
    /// Size of the rule's body by only counting terminals and variables
    pub reduction_length: usize,
    /// Bytecode for the rule's production
    pub bytecode: Vec<LROpCode>
}

impl LRProduction {
    /// Creates and loads a production
    pub fn new(data: &[u8], index: &mut usize) -> LRProduction {
        let head = read_u16(data, *index) as usize;
        *index += 2;
        let head_action = if data[*index] == 1 {
            TREE_ACTION_REPLACE
        } else {
            TREE_ACTION_NONE
        };
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

pub trait Parser {
    /// Parses the input
    fn parse(&mut self);
}
