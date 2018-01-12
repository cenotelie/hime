/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

//! Module for RNGLR parsers

use std::usize;

use super::*;
use super::subtree::SubTree;
use super::super::ast::Ast;
use super::super::ast::TableElemRef;
use super::super::ast::TableType;
use super::super::errors::ParseErrorUnexpectedToken;
use super::super::lexers::DEFAULT_CONTEXT;
use super::super::lexers::Lexer;
use super::super::lexers::TokenKernel;
use super::super::symbols::SemanticBody;
use super::super::symbols::SemanticElement;
use super::super::symbols::SemanticElementTrait;
use super::super::utils::biglist::BigList;

/// Represents a cell in a RNGLR parse table
#[derive(Copy, Clone)]
struct RNGLRAutomatonCell {
    /// The number of actions in this cell
    count: u32,
    /// Index of the cell's data
    index: u32
}

/// Represents the RNGLR parsing table and productions
pub struct RNGLRAutomaton {
    /// Index of the axiom variable
    axiom: usize,
    /// The number of columns in the LR table
    columns_count: usize,
    /// The number of states in the LR table
    states_count: usize,
    /// Map of symbol ID to column index in the LR table
    columns_map: LRColumnMap,
    /// The contexts information
    contexts: Vec<LRContexts>,
    /// The RNGLR table
    cells: Vec<RNGLRAutomatonCell>,
    /// The LR action table
    table: Vec<u16>,
    /// The table of LR productions
    productions: Vec<LRProduction>,
    /// The table of nullable variables
    nullables: Vec<u16>
}

impl RNGLRAutomaton {
    /// Initializes a new automaton from the given binary data
    pub fn new(data: &[u8]) -> RNGLRAutomaton {
        // read basic counters
        let axiom_index = read_u16(data, 0) as usize;
        let columns_count = read_u16(data, 2) as usize;
        let states_count = read_u16(data, 4) as usize;
        let actions_count = read_u32(data, 6) as usize;
        let productions_count = read_u16(data, 10) as usize;
        let nullables_count = read_u16(data, 12) as usize;
        // reads the column map
        let columns_map = LRColumnMap::new(data, 14, columns_count);
        // read the contexts table
        let mut contexts = Vec::<LRContexts>::with_capacity(states_count);
        let mut index = 14 + columns_count * 2;
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
        // read the automaton cells
        let mut cells = Vec::<RNGLRAutomatonCell>::with_capacity(columns_count * states_count);
        for _i in 0..(columns_count * states_count) {
            cells.push(RNGLRAutomatonCell {
                count: read_u16(data, index) as u32,
                index: read_u32(data, index + 2)
            });
            index += 6;
        }
        // read the actions table for the automaton
        let table = read_table_u16(data, index, actions_count * 2);
        index += actions_count * 4;
        // read the production table
        let mut productions = Vec::<LRProduction>::with_capacity(productions_count);
        for _i in 0..productions_count {
            let production = LRProduction::new(data, &mut index);
            productions.push(production);
        }
        // read the nullables table
        let nullables = read_table_u16(data, index, nullables_count);
        index += nullables_count * 2;
        assert_eq!(index, data.len());
        RNGLRAutomaton {
            axiom: axiom_index,
            columns_count,
            states_count,
            columns_map,
            contexts,
            cells,
            table,
            productions,
            nullables
        }
    }

    /// Gets the index of the axiom
    pub fn get_axiom(&self) -> usize {
        self.axiom
    }

    /// Gets the number of states in this automaton
    pub fn get_states_count(&self) -> usize {
        self.states_count
    }

    /// Gets the contexts opened by the specified state
    pub fn get_contexts(&self, state: u32) -> &LRContexts {
        &self.contexts[state as usize]
    }

    /// Gets the number of GLR actions for the given state and symbol identifier
    pub fn get_actions_count(&self, state: u32, identifier: u32) -> usize {
        let column = self.columns_map.get(identifier) as usize;
        self.cells[state as usize * self.columns_count + column].count as usize
    }

    /// Gets the i-th GLR action for the given state and sid
    pub fn get_action(&self, state: u32, identifier: u32, index: usize) -> LRAction {
        let column = self.columns_map.get(identifier) as usize;
        let cell = self.cells[state as usize * self.columns_count + column];
        LRAction {
            table: &self.table,
            offset: (cell.index as usize + index) * 2
        }
    }

    /// Gets the i-th production
    pub fn get_production(&self, index: usize) -> &LRProduction {
        &self.productions[index]
    }

    /// Gets the production for the nullable variable with the given index
    pub fn get_nullable_production(&self, index: usize) -> Option<&LRProduction> {
        match self.nullables[index] {
            0xFFFF => None,
            prod_index => Some(&self.productions[prod_index as usize])
        }
    }

    /// Determine whether the given state is the accepting state
    pub fn is_accepting_state(&self, state: u32) -> bool {
        let cell = self.cells[state as usize * self.columns_count];
        if cell.count != 1 {
            false
        } else {
            self.table[(cell.index as usize) * 2] == LR_ACTION_CODE_ACCEPT
        }
    }

    /// Gets the expected terminals for the specified state
    pub fn get_expected(&self, state: u32, terminals: &'static [Symbol]) -> LRExpected {
        let mut expected = LRExpected::new();
        for (column, terminal) in terminals.iter().enumerate() {
            let cell = self.cells[state as usize * self.columns_count + column];
            for i in 0..cell.count as usize {
                let action = LRAction {
                    table: &self.table,
                    offset: (cell.index as usize + i) * 2
                };
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    expected.add_unique_shift(*terminal);
                } else if action.get_code() == LR_ACTION_CODE_REDUCE {
                    expected.add_unique_reduction(*terminal);
                }
            }
        }
        expected
    }
}

/// Represents an edge in a Graph-Structured Stack
#[derive(Copy, Clone)]
struct GSSEdge {
    /// The index of the node from which this edge starts
    from: usize,
    /// The index of the node to which this edge arrives to
    to: usize,
    /// The identifier of the SPPF node that serve as label for this edge
    label: usize
}

/// Represents a generation in a Graph-Structured Stack
/// Because GSS nodes and edges are always created sequentially,
/// a generation basically describes a span in a buffer of GSS nodes or edges
#[derive(Copy, Clone)]
struct GSSGeneration {
    /// The start index of this generation in the list of nodes
    start: usize,
    /// The number of nodes in this generation
    count: usize
}

/// Represents a path in a GSS
#[derive(Clone)]
struct GSSPath {
    /// The final target of this path
    last_node: usize,
    /// The generation containing the final target of this path
    generation: usize,
    /// The labels on this GSS path
    labels: Option<Vec<usize>>
}

impl GSSPath {
    /// Initializes this path
    pub fn new(length: usize) -> GSSPath {
        GSSPath {
            last_node: 0,
            generation: 0,
            labels: if length == 0 {
                None
            } else {
                Some(Vec::<usize>::with_capacity(length))
            }
        }
    }
}

/// Represents Graph-Structured Stacks for GLR parsers
struct GSS {
    /// The label (GLR state) on the GSS node for the given index
    node_labels: BigList<u32>,
    /// The generations of nodes in this GSS
    node_generations: BigList<GSSGeneration>,
    /// The edges in this GSS
    edges: BigList<GSSEdge>,
    /// The generations for the edges
    edges_generations: BigList<GSSGeneration>,
    /// Index of the current generation
    current_generation: usize
}

impl GSS {
    /// Initializes the GSS
    pub fn new() -> GSS {
        GSS {
            node_labels: BigList::<u32>::new(0),
            node_generations: BigList::<GSSGeneration>::new(GSSGeneration { start: 0, count: 0 }),
            edges: BigList::<GSSEdge>::new(GSSEdge {
                from: 0,
                to: 0,
                label: 0
            }),
            edges_generations: BigList::<GSSGeneration>::new(GSSGeneration { start: 0, count: 0 }),
            current_generation: 0
        }
    }

    /// Gets the data of the current generation
    pub fn get_current_generation(&self) -> GSSGeneration {
        self.node_generations[self.current_generation]
    }

    /// Gets the data of the specified generation of nodes
    pub fn get_generation(&self, generation: usize) -> GSSGeneration {
        self.node_generations[generation]
    }

    /// Gets the GLR state represented by the specified node
    pub fn get_represented_state(&self, node: usize) -> u32 {
        self.node_labels[node]
    }

    /// Finds in the given generation a node representing the given GLR state
    pub fn find_node(&self, generation: usize, state: u32) -> Option<usize> {
        let data = self.node_generations[generation];
        for i in data.start..(data.start + data.count) {
            if self.node_labels[i] == state {
                return Some(i);
            }
        }
        None
    }

    /// Determines whether this instance has the required edge
    pub fn has_edge(&self, generation: usize, from: usize, to: usize) -> bool {
        let data = self.edges_generations[generation];
        for i in data.start..(data.start + data.count) {
            let edge = self.edges[i];
            if edge.from == from && edge.to == to {
                return true;
            }
        }
        false
    }

    /// Opens a new generation in this GSS
    pub fn create_generation(&mut self) -> usize {
        self.node_generations.push(GSSGeneration {
            start: self.node_labels.len(),
            count: 0
        });
        self.edges_generations.push(GSSGeneration {
            start: self.edges.len(),
            count: 0
        });
        if self.node_generations.len() == 1 {
            // this is the first generation
            self.current_generation = 0;
        } else {
            self.current_generation += 1;
        }
        self.current_generation
    }

    /// Creates a new node in the GSS
    pub fn create_node(&mut self, state: u32) -> usize {
        let node = self.node_labels.push(state);
        self.node_generations[self.current_generation].count += 1;
        node
    }
}

/// Represents the builder of Parse Trees for RNGLR parsers
struct RNGLRAstBuilder<'l> {
    /// Lexer associated to this parser
    lexer: &'l mut Lexer<'l>,
    /// The AST being built
    result: Ast<'l>
}

impl<'l> RNGLRAstBuilder<'l> {
    /// Initializes the builder with the given stack size
    pub fn new(lexer: &'l mut Lexer<'l>, result: Ast<'l>) -> RNGLRAstBuilder<'l> {
        RNGLRAstBuilder { lexer, result }
    }
}

struct RNGLRParserData<'a> {
    /// The parser's automaton
    automaton: RNGLRAutomaton,
    /// The semantic actions
    actions: &'a mut FnMut(usize, Symbol, &SemanticBody)
}

/// Represents a base for all RNGLR parsers
pub struct RNGLRParser<'l, 'a: 'l> {
    /// The parser's data
    data: RNGLRParserData<'a>,
    /// The AST builder
    builder: RNGLRAstBuilder<'l>
}

impl<'l, 'a: 'l> RNGLRParser<'l, 'a> {
    /// Initializes a new instance of the parser
    pub fn new(
        lexer: &'l mut Lexer<'l>,
        automaton: RNGLRAutomaton,
        ast: Ast<'l>,
        actions: &'a mut FnMut(usize, Symbol, &SemanticBody)
    ) -> RNGLRParser<'l, 'a> {
        RNGLRParser {
            data: RNGLRParserData { automaton, actions },
            builder: RNGLRAstBuilder::new(lexer, ast)
        }
    }
}

impl<'l, 'a> Parser for RNGLRParser<'l, 'a> {
    fn parse(&mut self) {}
}
