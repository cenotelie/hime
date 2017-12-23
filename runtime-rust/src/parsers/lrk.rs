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

//! Module for LR(k) parsers

use super::*;
use super::subtree::SubTree;
use super::super::ast::Ast;
use super::super::ast::TableElemRef;
use super::super::ast::TableType;
use super::super::lexers::Lexer;
use super::super::symbols::SemanticBody;
use super::super::symbols::SemanticElement;

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

const ESTIMATION_BIAS: usize = 5;

/// the data about a reduction
struct LRkAstReduction {
    /// The length of the reduction
    pub length: usize,
    /// The sub-tree build-up cache
    pub cache: SubTree,
    /// The number of items popped from the stack
    pub pop_count: usize
}

impl LRkAstReduction {
    /// Turns this reduction data into a subtree
    pub fn into_subtree(self) -> SubTree {
        self.cache
    }
}

/// Represents the builder of Parse Trees for LR(k) parsers
struct LRkAstBuilder<'a> {
    /// The stack of semantic objects
    stack: Vec<SubTree>,
    /// The AST being built
    result: Ast<'a>,
    /// The reduction handle represented as the indices of the sub-trees in the cache
    handle: Vec<usize>,
    /// The data of the current reduction
    reduction: Option<LRkAstReduction>
}

impl<'a> SemanticBody for LRkAstBuilder<'a> {
    fn length(&self) -> usize {
        self.handle.len()
    }

    fn get_element_at(&self, index: usize) -> SemanticElement {
        match self.reduction {
            None => panic!("Not in a reduction"),
            Some(ref data) => {
                let label = data.cache.get_label_at(self.handle[index]);
                self.result.get_semantic_element_for_label(label)
            }
        }
    }
}

impl<'a> LRkAstBuilder<'a> {
    /// Initializes the builder with the given stack size
    pub fn new(result: Ast<'a>) -> LRkAstBuilder {
        LRkAstBuilder {
            stack: Vec::<SubTree>::new(),
            result,
            handle: Vec::<usize>::new(),
            reduction: None
        }
    }

    /// Push a token onto the stack
    pub fn push_token(&mut self, index: usize) {
        let mut single = SubTree::new(1);
        single.push(TableElemRef::new(TableType::Token, index), TREE_ACTION_NONE);
        self.stack.push(single);
    }

    /// Prepares for the forthcoming reduction operations
    pub fn reduction_prepare(&mut self, variable_index: usize, length: usize, action: TreeAction) {
        let mut estimation = ESTIMATION_BIAS;
        for i in 0..length {
            estimation += self.stack[self.stack.len() - length + i].get_size();
        }
        let mut cache = SubTree::new(estimation);
        cache.setup_root(
            TableElemRef::new(TableType::Variable, variable_index),
            action
        );
        self.reduction = Some(LRkAstReduction {
            length,
            cache,
            pop_count: 0
        });
    }

    /// During a reduction, insert the given sub-tree
    fn reduction_add_sub(
        reduction: &mut LRkAstReduction,
        handle: &mut Vec<usize>,
        sub: &SubTree,
        action: TreeAction
    ) {
        if sub.get_action_at(0) == TREE_ACTION_REPLACE {
            let children_count = sub.get_children_count_at(0);
            let mut cache_index = reduction.cache.get_size();
            // copy the children to the cache
            sub.copy_children_to(&mut reduction.cache);
            // setup the handle
            let mut sub_index = 1;
            for _i in 0..children_count {
                let size = sub.get_children_count_at(sub_index) + 1;
                handle.push(cache_index);
                cache_index += size;
                sub_index += size;
            }
        } else if action == TREE_ACTION_DROP {
            return;
        } else {
            let cache_index = reduction.cache.get_size();
            // copy the complete sub-tree to the cache
            sub.copy_to(&mut reduction.cache);
            handle.push(cache_index);
            if action != TREE_ACTION_NONE {
                reduction.cache.set_action_at(cache_index, action);
            }
        }
    }

    /// During a redution, pops the top symbol from the stack and gives it a tree action
    pub fn reduction_pop(&mut self, action: TreeAction) {
        match self.reduction {
            None => panic!("Not in a reduction"),
            Some(ref mut reduction) => {
                let sub = &self.stack[self.stack.len() - reduction.length + reduction.pop_count];
                LRkAstBuilder::reduction_add_sub(reduction, &mut self.handle, &sub, action);
                reduction.pop_count += 1;
            }
        }
    }

    /// During a reduction, inserts a virtual symbol
    pub fn reduction_add_virtual(&mut self, index: usize, action: TreeAction) {
        if action != TREE_ACTION_DROP {
            match self.reduction {
                None => panic!("Not in a reduction"),
                Some(ref mut reduction) => {
                    let cache_index = reduction.cache.get_size();
                    reduction
                        .cache
                        .push(TableElemRef::new(TableType::Virtual, index), action);
                    self.handle.push(cache_index);
                }
            }
        }
    }

    /// Finalizes the reduction operation
    pub fn reduce(&mut self) {
        let stack_size = self.stack.len();
        match self.reduction {
            None => panic!("Not in a reduction"),
            Some(ref mut reduction) => {
                if reduction.cache.get_action_at(0) == TREE_ACTION_REPLACE {
                    reduction.cache.set_children_count_at(0, self.handle.len());
                } else {
                    LRkAstBuilder::reduce_tree(reduction, &self.handle, &mut self.result);
                }
                // Put it on the stack
                self.stack.truncate(stack_size - reduction.length);
            }
        }
        let result = ::std::mem::replace(&mut self.reduction, None)
            .unwrap()
            .into_subtree();
        self.handle.clear();
        self.stack.push(result);
    }

    /// Applies the promotion tree actions to the cache and commits to the final AST
    pub fn reduce_tree(reduction: &mut LRkAstReduction, handle: &Vec<usize>, result: &mut Ast) {
        // promotion data
        let mut promotion = false;
        let mut insertion = 1;
        for i in 0..handle.len() {
            match reduction.cache.get_action_at(handle[i]) {
                TREE_ACTION_PROMOTE => {
                    if promotion {
                        // This is not the first promotion
                        // Commit the previously promoted node's children
                        reduction.cache.set_children_count_at(0, insertion - 1);
                        reduction.cache.commit_children_of(0, result);
                        // Re-put the previously promoted node in the cache
                        reduction.cache.move_node(0, 1);
                        insertion = 2;
                    }
                    promotion = true;
                    // Save the new promoted node
                    reduction.cache.move_node(handle[i], 0);
                    // Repack the children on the left if any
                    let nb = reduction.cache.get_children_count_at(0);
                    reduction.cache.move_range(handle[i] + 1, insertion, nb);
                    insertion += nb;
                }
                _ => {
                    // Commit the children if any
                    reduction.cache.commit_children_of(handle[i], result);
                    // Repack the sub-root on the left
                    if insertion != handle[i] {
                        reduction.cache.move_node(handle[i], insertion);
                    }
                    insertion += 1;
                }
            }
        }
        // finalize the sub-tree data
        reduction.cache.set_children_count_at(0, insertion - 1);
    }
}

/// Represents a base for all LR(k) parsers
pub struct LRkParser<'a> {
    /// Lexer associated to this parser
    lexer: &'a mut Lexer<'a>
}
