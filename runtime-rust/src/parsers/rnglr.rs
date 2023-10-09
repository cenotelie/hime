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

use alloc::collections::VecDeque;
use alloc::string::ToString;
use alloc::vec::Vec;

use super::{
    get_op_code_base, get_op_code_tree_action, read_table_u16, read_u16, read_u32, ContextProvider,
    LRAction, LRColumnMap, LRContexts, LRExpected, LRProduction, Parser, Symbol, TreeAction,
    LR_ACTION_CODE_ACCEPT, LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_SHIFT,
    LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE, LR_OP_CODE_BASE_ADD_VIRTUAL,
    LR_OP_CODE_BASE_SEMANTIC_ACTION, TREE_ACTION_DROP, TREE_ACTION_PROMOTE,
    TREE_ACTION_REPLACE_BY_CHILDREN, TREE_ACTION_REPLACE_BY_EPSILON,
};
use crate::ast::{AstCell, AstImpl, TableElemRef, TableType};
use crate::errors::ParseErrorUnexpectedToken;
use crate::lexers::{Lexer, TokenKernel, DEFAULT_CONTEXT};
use crate::sppf::{SppfImpl, SppfImplNode, SppfImplNodeRef};
use crate::symbols::{SemanticBody, SemanticElement, SemanticElementTrait, SID_EPSILON};
use crate::utils::biglist::BigList;
use crate::utils::OwnOrMut;

/// Represents a cell in a RNGLR parse table
#[derive(Copy, Clone)]
struct RNGLRAutomatonCell {
    /// The number of actions in this cell
    count: u32,
    /// Index of the cell's data
    index: u32,
}

/// Represents the RNGLR parsing table and productions
#[derive(Clone)]
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
    nullables: Vec<u16>,
}

impl RNGLRAutomaton {
    /// Initializes a new automaton from the given binary data
    #[must_use]
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
        let mut contexts = Vec::with_capacity(states_count);
        let mut index = 14 + columns_count * 2;
        for _i in 0..states_count {
            let mut context = LRContexts::new();
            let count = read_u16(data, index);
            index += 2;
            for _j in 0..count {
                context.add(read_u16(data, index), read_u16(data, index + 2));
                index += 4;
            }
            contexts.push(context);
        }
        // read the automaton cells
        let mut cells = Vec::with_capacity(columns_count * states_count);
        for _i in 0..(columns_count * states_count) {
            cells.push(RNGLRAutomatonCell {
                count: u32::from(read_u16(data, index)),
                index: read_u32(data, index + 2),
            });
            index += 6;
        }
        // read the actions table for the automaton
        let table = read_table_u16(data, index, actions_count * 2);
        index += actions_count * 4;
        // read the production table
        let mut productions = Vec::with_capacity(productions_count);
        for _i in 0..productions_count {
            let production = LRProduction::new(data, &mut index);
            productions.push(production);
        }
        // read the nullables table
        let nullables = read_table_u16(data, index, nullables_count);
        // index += nullables_count * 2;
        // assert_eq!(index, data.len());
        RNGLRAutomaton {
            axiom: axiom_index,
            columns_count,
            states_count,
            columns_map,
            contexts,
            cells,
            table,
            productions,
            nullables,
        }
    }

    /// Gets the index of the axiom
    #[must_use]
    pub fn get_axiom(&self) -> usize {
        self.axiom
    }

    /// Gets the number of states in this automaton
    #[must_use]
    pub fn get_states_count(&self) -> usize {
        self.states_count
    }

    /// Gets the number of columns in the LR table
    #[must_use]
    pub fn get_columns_count(&self) -> usize {
        self.columns_count
    }

    /// Gets the symbol's identifier for a column
    #[must_use]
    pub fn get_sid_for_column(&self, column: usize) -> u32 {
        self.columns_map.get_id_at(column)
    }

    /// Gets the contexts opened by the specified state
    #[must_use]
    pub fn get_contexts(&self, state: u32) -> &LRContexts {
        &self.contexts[state as usize]
    }

    /// Gets the number of GLR actions for the given state and symbol identifier
    #[must_use]
    pub fn get_actions_count(&self, state: u32, identifier: u32) -> usize {
        let column = self.columns_map.get(identifier) as usize;
        self.get_actions_count_at(state, column)
    }

    /// Gets the number of GLR actions for the given state and column
    #[must_use]
    pub fn get_actions_count_at(&self, state: u32, column: usize) -> usize {
        self.cells[state as usize * self.columns_count + column].count as usize
    }

    /// Gets the i-th GLR action for the given state and sid
    #[must_use]
    pub fn get_action(&self, state: u32, identifier: u32, index: usize) -> LRAction {
        let column = self.columns_map.get(identifier) as usize;
        self.get_action_at(state, column, index)
    }

    /// Gets the i-th GLR action for the given state and column
    #[must_use]
    pub fn get_action_at(&self, state: u32, column: usize, index: usize) -> LRAction {
        let cell = self.cells[state as usize * self.columns_count + column];
        LRAction {
            table: &self.table,
            offset: (cell.index as usize + index) * 2,
        }
    }

    /// Gets the number of productions
    #[must_use]
    pub fn get_productions_count(&self) -> usize {
        self.productions.len()
    }

    /// Gets the i-th production
    #[must_use]
    pub fn get_production(&self, index: usize) -> &LRProduction {
        &self.productions[index]
    }

    /// Gets the production for the nullable variable with the given index
    #[must_use]
    pub fn get_nullable_production(&self, index: usize) -> Option<&LRProduction> {
        match self.nullables.get(index) {
            None | Some(0xFFFF) => None,
            Some(&prod_index) => Some(&self.productions[prod_index as usize]),
        }
    }

    /// Determine whether the given state is the accepting state
    #[must_use]
    pub fn is_accepting_state(&self, state: u32) -> bool {
        let cell = self.cells[state as usize * self.columns_count];
        if cell.count == 1 {
            self.table[(cell.index as usize) * 2] == LR_ACTION_CODE_ACCEPT
        } else {
            false
        }
    }

    /// Gets the expected terminals for the specified state
    #[must_use]
    pub fn get_expected<'s>(&self, state: u32, terminals: &[Symbol<'s>]) -> LRExpected<'s> {
        let mut expected = LRExpected::new();
        for (column, terminal) in terminals.iter().enumerate() {
            let cell = self.cells[state as usize * self.columns_count + column];
            for i in 0..cell.count as usize {
                let action = LRAction {
                    table: &self.table,
                    offset: (cell.index as usize + i) * 2,
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

/// Represents a label for a GSS edge
#[derive(Debug, Default, Copy, Clone)]
struct GSSLabel {
    /// The identifier of the SPPF node in this edge
    sppf_node: SppfImplNodeRef,
    /// The symbol identifier of the original symbol on the SPPF node
    symbol_id: u32,
}

/// Represents an edge in a Graph-Structured Stack
#[derive(Debug, Default, Copy, Clone)]
struct GSSEdge {
    /// The index of the node from which this edge starts
    from: u32,
    /// The index of the node to which this edge arrives to
    to: u32,
    /// The label for this edge
    label: GSSLabel,
}

/// Represents a generation in a Graph-Structured Stack
/// Because GSS nodes and edges are always created sequentially,
/// a generation basically describes a span in a buffer of GSS nodes or edges
#[derive(Debug, Default, Copy, Clone)]
struct GSSGeneration {
    /// The start index of this generation in the list of nodes
    start: usize,
    /// The number of nodes in this generation
    count: usize,
}

/// Represents a path in a GSS
#[derive(Debug, Clone)]
struct GSSPath {
    /// The final target of this path
    last_node: usize,
    /// The generation containing the final target of this path
    generation: usize,
    /// The labels on this GSS path
    labels: Vec<GSSLabel>,
}

impl GSSPath {
    /// Initializes this path
    pub fn new(last_node: usize, generation: usize, length: usize) -> GSSPath {
        GSSPath {
            last_node,
            generation,
            labels: if length == 0 {
                Vec::new()
            } else {
                Vec::with_capacity(length)
            },
        }
    }

    /// Initializes this path
    pub fn new_length0(last_node: usize, generation: usize) -> GSSPath {
        GSSPath {
            last_node,
            generation,
            labels: Vec::new(),
        }
    }

    /// Initializes this path
    pub fn from(
        previous: &GSSPath,
        last_node: usize,
        generation: usize,
        label: GSSLabel,
    ) -> GSSPath {
        let mut result = GSSPath {
            last_node,
            generation,
            labels: previous.labels.clone(),
        };
        result.labels.push(label);
        result
    }

    /// Pushes the next label
    pub fn push(&mut self, last_node: usize, generation: usize, label: GSSLabel) {
        self.last_node = last_node;
        self.generation = generation;
        self.labels.push(label);
    }
}

/// Represents Graph-Structured Stacks for GLR parsers
#[allow(clippy::upper_case_acronyms)]
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
    current_generation: usize,
}

impl GSS {
    /// Initializes the GSS
    pub fn new() -> GSS {
        GSS {
            node_labels: BigList::default(),
            node_generations: BigList::default(),
            edges: BigList::default(),
            edges_generations: BigList::default(),
            current_generation: 0,
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
        (data.start..(data.start + data.count)).find(|&i| self.node_labels[i] == state)
    }

    /// Gets the corresponding edge, if any
    pub fn get_edge(&self, generation: usize, from: usize, to: usize) -> Option<GSSEdge> {
        let data = self.edges_generations[generation];
        for i in data.start..(data.start + data.count) {
            let edge = self.edges[i];
            if edge.from as usize == from && edge.to as usize == to {
                return Some(edge);
            }
        }
        None
    }

    /// Opens a new generation in this GSS
    pub fn create_generation(&mut self) -> usize {
        self.node_generations.push(GSSGeneration {
            start: self.node_labels.len(),
            count: 0,
        });
        self.edges_generations.push(GSSGeneration {
            start: self.edges.len(),
            count: 0,
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

    /// Creates a new edge in the GSS
    pub fn create_edge(&mut self, from: usize, to: usize, label: GSSLabel) {
        self.edges.push(GSSEdge {
            from: from as u32,
            to: to as u32,
            label,
        });
        self.edges_generations[self.current_generation].count += 1;
    }

    /// Retrieve the generation of the given node in this GSS
    fn get_generation_of(&self, node: usize) -> usize {
        for i in (0..=self.current_generation).rev() {
            let data = self.node_generations[i];
            if node >= data.start && node < data.start + data.count {
                return i;
            }
        }
        panic!("Node not found");
    }

    /// Gets all paths in the GSS starting at the given node and with the given length
    pub fn get_paths(&self, from: usize, length: usize) -> Vec<GSSPath> {
        let mut paths = Vec::new();
        if length == 0 {
            // 0-length path, simply return a single path with the 'from' node
            paths.push(GSSPath::new_length0(from, 0));
            return paths;
        }

        // Initialize the first path
        paths.push(GSSPath::new(from, self.get_generation_of(from), length));
        // For the remaining hops
        for _i in 0..length {
            // Insertion index for the compaction process
            let mut m = 0;
            let total = paths.len();
            for p in 0..total {
                // for all paths
                let last = paths[p].last_node;
                let gen_index = paths[p].generation;
                // Look for new additional paths from last
                let gen = self.edges_generations[gen_index];
                let mut first_edge: Option<GSSEdge> = None;
                for e in gen.start..(gen.start + gen.count) {
                    let edge = self.edges[e];
                    if edge.from as usize == last {
                        match first_edge {
                            None => {
                                // This is the first edge
                                first_edge = Some(edge);
                            }
                            Some(_x) => {
                                // Not the first edge
                                // Clone and extend the new path
                                let new_path = GSSPath::from(
                                    &paths[p],
                                    edge.to as usize,
                                    self.get_generation_of(edge.to as usize),
                                    edge.label,
                                );
                                paths.push(new_path);
                            }
                        }
                    }
                }
                // Check whether there was at least one edge
                match first_edge {
                    None => {}
                    Some(edge) => {
                        // Continue the current path
                        if m != p {
                            // swap m and p paths
                            paths.swap(m, p);
                        }
                        paths[m].push(
                            edge.to as usize,
                            self.get_generation_of(edge.to as usize),
                            edge.label,
                        );
                        // goto next
                        m += 1;
                    }
                }
            }
            // inspected all current paths
            if m != total {
                // if some previous paths have been removed
                // => compact the list if needed
                for p in total..paths.len() {
                    paths.swap(m, p);
                    m += 1;
                }
                // truncates the paths to those inserted
                // m is now the exact number of paths
                paths.truncate(m);
            }
        }
        paths
    }
}

/// Represents the epsilon node
const EPSILON: GSSLabel = GSSLabel {
    sppf_node: SppfImplNodeRef::new(0xFFFF_FFFF),
    symbol_id: SID_EPSILON,
};

/// The data about a reduction for a SPPF
struct SPPFReduction {
    /// The adjacency cache for the reduction
    cache: Vec<SppfImplNodeRef>,
    /// The actions for the reduction
    actions: Vec<TreeAction>,
    /// The stack of semantic objects for the reduction
    stack: Vec<GSSLabel>,
    /// The number of items popped from the stack
    pop_count: usize,
}

/// Represents a structure that helps build a Shared Packed Parse Forest (SPPF)
/// A SPPF is a compact representation of multiple variants of an AST at once.
struct SPPFBuilder<'s, 't, 'a, 'l> {
    /// Lexer associated to this parser
    lexer: &'l mut Lexer<'s, 't, 'a>,
    /// The table of variables
    variables: &'a [Symbol<'s>],
    /// The table of virtuals
    virtuals: &'a [Symbol<'s>],
    /// The SPPF front to build the SPPF
    sppf: OwnOrMut<'a, SppfImpl>,
    /// The data of the current reductions
    reduction: Option<SPPFReduction>,
    /// The AST being built, if any
    ast: Option<&'a mut AstImpl>,
}

impl<'s, 't, 'a, 'l> SemanticBody for SPPFBuilder<'s, 't, 'a, 'l> {
    fn get_element_at(&self, index: usize) -> SemanticElement {
        let reduction = self.reduction.as_ref().expect("Not in a reduction");
        let reference = reduction.cache[index];
        let node = self.sppf.get_node(reference).as_normal();
        let label = node.versions[0].label;
        match label.table_type() {
            TableType::Token => {
                SemanticElement::Token(self.lexer.get_data().repository.get_token(label.index()))
            }
            TableType::Variable => SemanticElement::Variable(self.variables[label.index()]),
            TableType::Virtual => SemanticElement::Virtual(self.virtuals[label.index()]),
            TableType::None => {
                SemanticElement::Terminal(self.lexer.get_data().repository.terminals[0])
            }
        }
    }

    fn length(&self) -> usize {
        self.reduction
            .as_ref()
            .expect("Not in a reduction")
            .cache
            .len()
    }
}

impl<'s, 't, 'a, 'l> SPPFBuilder<'s, 't, 'a, 'l> {
    /// Initializes the builder targeting an AST
    pub fn new_ast(
        lexer: &'l mut Lexer<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        ast: &'a mut AstImpl,
    ) -> SPPFBuilder<'s, 't, 'a, 'l> {
        SPPFBuilder {
            lexer,
            variables,
            virtuals,
            sppf: OwnOrMut::Owned(SppfImpl::default()),
            reduction: None,
            ast: Some(ast),
        }
    }

    /// Initializes the builder targeting an SPPF
    pub fn new_sppf(
        lexer: &'l mut Lexer<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        sppf: &'a mut SppfImpl,
    ) -> SPPFBuilder<'s, 't, 'a, 'l> {
        SPPFBuilder {
            lexer,
            variables,
            virtuals,
            sppf: OwnOrMut::MutRef(sppf),
            reduction: None,
            ast: None,
        }
    }

    /// Creates a single node in the result SPPF an returns it
    pub fn get_single_node(&mut self, symbol: TableElemRef) -> SppfImplNodeRef {
        self.sppf.new_normal_node(symbol)
    }

    /// Prepares for the forthcoming reduction operations
    pub fn reduction_prepare(&mut self, first: GSSLabel, path: &GSSPath, length: usize) {
        let mut stack = Vec::new();
        if length > 0 {
            if length > 1 {
                for i in 0..(length - 1) {
                    stack.push(path.labels[length - 2 - i]);
                }
            }
            stack.push(first);
        }
        self.reduction = Some(SPPFReduction {
            cache: Vec::with_capacity(length),
            actions: Vec::with_capacity(length),
            stack,
            pop_count: 0,
        });
    }

    /// Adds the specified GSS label to the reduction cache with the given tree action
    fn reduction_add_to_cache(
        reduction: &mut SPPFReduction,
        sppf: &SppfImpl,
        sppf_node_ref: SppfImplNodeRef,
        action: TreeAction,
    ) {
        if action == TREE_ACTION_DROP {
            return;
        }
        let node = sppf.get_node(sppf_node_ref);
        match node {
            SppfImplNode::Normal(_) => {
                // this is a simple reference to an existing SPPF node
                SPPFBuilder::reduction_add_to_cache_node(reduction, sppf_node_ref, action);
            }
            SppfImplNode::Replaceable(replaceable) => {
                // this is replaceable sub-tree
                for (&node_ref, &action) in replaceable.children.iter().zip(&replaceable.actions) {
                    SPPFBuilder::reduction_add_to_cache(reduction, sppf, node_ref, action);
                }
            }
        }
    }

    /// Adds the specified GSS label to the reduction cache with the given tree action
    fn reduction_add_to_cache_node(
        reduction: &mut SPPFReduction,
        sppf_node_ref: SppfImplNodeRef,
        action: TreeAction,
    ) {
        // add the node in the cache
        reduction.cache.push(sppf_node_ref);
        reduction.actions.push(action);
    }

    /// During a reduction, pops the top symbol from the stack and gives it a tree action
    pub fn reduction_pop(&mut self, action: TreeAction) {
        let reduction = self.reduction.as_mut().expect("Not in a reduction");
        let label = reduction.stack[reduction.pop_count];
        reduction.pop_count += 1;
        SPPFBuilder::reduction_add_to_cache(reduction, &self.sppf, label.sppf_node, action);
    }

    /// During a reduction, inserts a virtual symbol
    pub fn reduction_add_virtual(&mut self, index: usize, action: TreeAction) {
        let reduction = self.reduction.as_mut().expect("Not in a reduction");
        let node_id = self
            .sppf
            .new_normal_node(TableElemRef::new(TableType::Virtual, index));
        reduction.cache.push(node_id);
        reduction.actions.push(action);
    }

    /// During a reduction, inserts the sub-tree of a nullable variable
    pub fn reduction_add_nullable(&mut self, nullable: SppfImplNodeRef, action: TreeAction) {
        let reduction = self.reduction.as_mut().expect("Not in a reduction");
        SPPFBuilder::reduction_add_to_cache(reduction, &self.sppf, nullable, action);
    }

    /// Finalizes the reduction operation
    pub fn reduce(
        &mut self,
        variable_index: usize,
        head_action: TreeAction,
        target: Option<SppfImplNodeRef>,
    ) -> SppfImplNodeRef {
        if head_action == TREE_ACTION_REPLACE_BY_CHILDREN {
            self.reduce_replaceable(variable_index)
        } else {
            self.reduce_normal(variable_index, head_action, target)
        }
    }

    /// Executes the reduction as a normal reduction
    pub fn reduce_normal(
        &mut self,
        variable_index: usize,
        head_action: TreeAction,
        target: Option<SppfImplNodeRef>,
    ) -> SppfImplNodeRef {
        let reduction: &mut SPPFReduction = self.reduction.as_mut().expect("Not in a reduction");
        let mut promoted: Option<SppfImplNodeRef> = None;

        let mut b = 0;
        let mut e = 0;
        while e < reduction.cache.len() {
            if reduction.actions[e] == TREE_ACTION_PROMOTE {
                let current = self.sppf.get_node_mut(reduction.cache[e]).as_normal_mut();
                if promoted.is_some() {
                    // not the first promotion, insert the old promoted and the remaining head
                    current.insert_head(&reduction.cache[(b - 1)..e]);
                } else {
                    assert_eq!(b, 0);
                    // capture the current head
                    current.insert_head(&reduction.cache[..e]);
                }

                // promote
                promoted = Some(reduction.cache[e]);
                // advance
                b = e + 1;
            }
            e += 1;
        }

        if let Some(promoted) = promoted {
            // capture the tail
            let current = self.sppf.get_node_mut(promoted).as_normal_mut();
            current.add_tail(&reduction.cache[b..]);
            promoted
        } else {
            assert_eq!(b, 0);
            let original_label = if head_action == TREE_ACTION_REPLACE_BY_EPSILON {
                TableElemRef::new(TableType::None, 0)
            } else {
                TableElemRef::new(TableType::Variable, variable_index)
            };
            if let Some(target) = target {
                self.sppf
                    .get_node_mut(target)
                    .as_normal_mut()
                    .new_version(original_label, &reduction.cache);
                target
            } else {
                self.sppf
                    .new_normal_node_with_children(original_label, &reduction.cache)
            }
        }
    }

    /// Executes the reduction as the reduction of a replaceable variable
    pub fn reduce_replaceable(&mut self, variable_index: usize) -> SppfImplNodeRef {
        let reduction = self.reduction.as_mut().expect("Not in a reduction");
        let label = TableElemRef::new(TableType::Variable, variable_index);
        self.sppf
            .new_replaceable_node(label, &reduction.cache, &reduction.actions)
    }

    /// Finalizes the parse tree
    pub fn commit_root(&mut self, root: SppfImplNodeRef) {
        self.sppf.store_root(root);
        if let Some(ast) = self.ast.as_mut() {
            let sppf = &self.sppf;
            let cell_root = SPPFBuilder::build_final_ast(sppf, root, ast);
            ast.store_root(cell_root);
        }
    }

    /// Builds thSe final AST for the specified SPPF node reference
    fn build_final_ast(
        sppf: &SppfImpl,
        sppf_node_ref: SppfImplNodeRef,
        result: &mut AstImpl,
    ) -> AstCell {
        let node = sppf.get_node(sppf_node_ref).as_normal();
        let version = &node.versions[0];
        if version.children.is_empty() {
            AstCell {
                label: version.label,
                first: 0,
                count: 0,
            }
        } else {
            let mut buffer = Vec::with_capacity(version.children.len());
            for child in &version.children {
                buffer.push(SPPFBuilder::build_final_ast(sppf, child, result));
            }
            let first = result.store(&buffer, 0, buffer.len());
            AstCell {
                label: version.label,
                first: first as u32,
                count: version.children.len() as u32,
            }
        }
    }
}

/// Represents a reduction operation to be performed
/// For reduction of length 0, the node is the GSS node on which it is applied,
/// the first label then is epsilon
/// For others, the node is the SECOND GSS node on the path, not the head.
/// The first label is then the label on the transition from the head.
#[derive(Debug, Copy, Clone)]
struct RNGLRReduction {
    /// The GSS node to reduce from
    node: usize,
    /// The LR production for the reduction
    production: usize,
    /// The first label in the GSS
    first: GSSLabel,
}

/// Represents a shift operation to be performed
#[derive(Copy, Clone)]
struct RNGLRShift {
    /// GSS node to shift from
    from: usize,
    /// The target RNGLR state
    to: usize,
}

struct RNGLRParserData<'s, 'a> {
    /// The parser's automaton
    automaton: RNGLRAutomaton,
    /// The GSS for this parser
    gss: GSS,
    /// The next token
    next_token: Option<TokenKernel>,
    /// The queue of reduction operations
    reductions: VecDeque<RNGLRReduction>,
    /// The queue of shift operations
    shifts: VecDeque<RNGLRShift>,
    /// The grammar variables
    variables: &'a [Symbol<'s>],
    /// The semantic actions
    actions: &'a mut dyn FnMut(usize, Symbol, &dyn SemanticBody),
}

impl<'s, 'a> ContextProvider for RNGLRParserData<'s, 'a> {
    /// Gets the priority of the specified context required by the specified terminal
    /// The priority is an unsigned integer. The lesser the value the higher the priority.
    /// The absence of value represents the unavailability of the required context.
    #[allow(
        clippy::cognitive_complexity,
        clippy::too_many_lines,
        clippy::cast_possible_truncation
    )]
    fn get_context_priority(
        &self,
        token_count: usize,
        context: u16,
        terminal_id: u32,
    ) -> Option<usize> {
        // the default context is always active
        if context == DEFAULT_CONTEXT {
            return Some(usize::MAX);
        }
        if token_count == 0 {
            // this is the first token, does it open the context?
            let contexts = self.automaton.get_contexts(0);
            return if contexts.opens(terminal_id, context) {
                Some(0)
            } else {
                None
            };
        }

        // try to only look at stack heads that expect the terminal
        let mut queue = Vec::new();
        let mut productions = Vec::new();
        let mut distances = Vec::new();
        let mut found_on_previous_shift = false;
        for shift in &self.shifts {
            let count = self
                .automaton
                .get_actions_count(shift.to as u32, terminal_id);
            for i in 0..count {
                let action = self.automaton.get_action(shift.to as u32, terminal_id, i);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    // does the context opens with the terminal?
                    let contexts = self.automaton.get_contexts(shift.to as u32);
                    if contexts.opens(terminal_id, context) {
                        return Some(0);
                    }
                    // looking at the immediate history, does the context opens from the shift just before?
                    let contexts2 = self
                        .automaton
                        .get_contexts(self.gss.get_represented_state(shift.from));
                    if contexts2.opens(self.get_next_token_id(), context) {
                        // found the context opening on the previous shift (and was not immediately closed by a reduction)
                        found_on_previous_shift = true;
                        break;
                    }
                    // no, enqueue
                    if !queue.contains(&shift.from) {
                        queue.push(shift.from);
                        productions.push(0xFFFF_FFFF);
                        distances.push(2);
                    }
                } else if action.get_code() == LR_ACTION_CODE_REDUCE {
                    let production = self.automaton.get_production(action.get_data() as usize);
                    // looking at the immediate history, does the context opens from the shift just before?
                    let contexts = self
                        .automaton
                        .get_contexts(self.gss.get_represented_state(shift.from));
                    if contexts.opens(self.get_next_token_id(), context)
                        && production.reduction_length == 0
                    {
                        // the reduction does not close the context
                        found_on_previous_shift = true;
                        break;
                    }
                    // no, enqueue
                    if !queue.contains(&shift.from) {
                        queue.push(shift.from);
                        productions.push(action.get_data() as usize);
                        distances.push(2);
                    }
                }
            }
        }
        if found_on_previous_shift {
            return Some(1);
        }
        if queue.is_empty() {
            // the track is empty, the terminal is unexpected
            return None;
        }
        // explore the current GSS to find the specified context
        let mut i = 0;
        while i < queue.len() {
            let from = queue[i];
            let distance = distances[i];
            let production = productions[i];
            i += 1;
            let paths = self.gss.get_paths(from, 1);
            for path in &paths {
                let last_node = path.last_node;
                let symbol_id = path.labels[0].symbol_id;
                let contexts = self
                    .automaton
                    .get_contexts(self.gss.get_represented_state(last_node));
                // was the context opened on this transition?
                if contexts.opens(symbol_id, context) {
                    if production == 0xFFFF_FFFF {
                        return Some(distance);
                    }
                    let production_data = self.automaton.get_production(production);
                    if production_data.reduction_length < distance {
                        return Some(distance);
                    }
                }
                // no, enqueue
                if !queue.contains(&last_node) {
                    queue.push(last_node);
                    productions.push(production);
                    distances.push(distance + 1);
                }
            }
        }
        // at this point, the requested context is not yet open
        // can it be open by a token with the specified terminal ID?
        // queue of GLR states to inspect:
        let mut queue_gss_heads = Vec::new(); // the related GSS head
        let mut queue_vstack = Vec::<Vec<u32>>::new(); // the virtual stack
        for shift in &self.shifts {
            let count = self
                .automaton
                .get_actions_count(shift.to as u32, terminal_id);
            if count > 0 {
                // enqueue the info, top GSS stack node and target GLR state
                queue_gss_heads.push(shift.from);
                queue_vstack.push(alloc::vec![shift.to as u32]);
            }
        }
        // now, close the queue
        i = 0;
        while i < queue_gss_heads.len() {
            let head = queue_vstack[i][queue_vstack[i].len() - 1];
            let gss_node = queue_gss_heads[i];
            let count = self.automaton.get_actions_count(head, terminal_id);
            if count == 0 {
                i += 1;
                continue;
            }
            for j in 0..count {
                let action = self.automaton.get_action(head, terminal_id, j);
                if action.get_code() != LR_ACTION_CODE_REDUCE {
                    continue;
                }
                // execute the reduction
                let production = self.automaton.get_production(action.get_data() as usize);
                if production.reduction_length == 0 {
                    // 0-length reduction => start from the current head
                    let mut virtual_stack = queue_vstack[i].clone();
                    let next = self.get_next_by_var(head, self.variables[production.head].id);
                    virtual_stack.push(next.unwrap());
                    // enqueue
                    queue_gss_heads.push(gss_node);
                    queue_vstack.push(virtual_stack);
                } else if production.reduction_length < queue_vstack[i].len() {
                    // we are still the virtual stack
                    let mut virtual_stack =
                        Vec::with_capacity(queue_vstack[i].len() - production.reduction_length + 1);
                    for k in 0..(queue_vstack[i].len() - production.reduction_length) {
                        virtual_stack.push(queue_vstack[i][k]);
                    }
                    let next = self.get_next_by_var(head, self.variables[production.head].id);
                    virtual_stack.push(next.unwrap());
                    // enqueue
                    queue_gss_heads.push(gss_node);
                    queue_vstack.push(virtual_stack);
                } else {
                    // we reach the GSS
                    let paths = self.gss.get_paths(
                        gss_node,
                        production.reduction_length - queue_vstack[i].len(),
                    );
                    for path in &paths {
                        // get the target GLR state
                        let next = self.get_next_by_var(
                            self.gss.get_represented_state(path.last_node),
                            self.variables[production.head].id,
                        );
                        // enqueue the info, top GSS stack node and target GLR state
                        queue_gss_heads.push(path.last_node);
                        queue_vstack.push(alloc::vec![next.unwrap()]);
                    }
                }
            }
            i += 1;
        }
        for virtual_stack in queue_vstack {
            let state = virtual_stack[virtual_stack.len() - 1];
            let count = self.automaton.get_actions_count(state, terminal_id);
            for i in 0..count {
                let action = self.automaton.get_action(state, terminal_id, i);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    let contexts = self.automaton.get_contexts(state);
                    if contexts.opens(terminal_id, context) {
                        // the context opens here
                        return Some(0);
                    }
                }
            }
        }
        // the context is still unavailable
        None
    }
}

impl<'s, 'a> RNGLRParserData<'s, 'a> {
    /// Gets the terminal's identifier for the next token
    fn get_next_token_id(&self) -> u32 {
        match self.next_token.as_ref() {
            None => SID_EPSILON,
            Some(kernel) => kernel.terminal_id,
        }
    }

    /// Checks whether the specified terminal is indeed expected for a reduction
    /// This check is required because in the case of a base LALR graph,
    /// some terminals expected for reduction in the automaton are coming from other paths.
    fn check_is_expected(&self, gss_node: usize, terminal: Symbol) -> bool {
        // queue of GLR states to inspect:
        let mut queue_gss_heads = Vec::new(); // the related GSS head
        let mut queue_vstack = Vec::<Vec<u32>>::new(); // the virtual stack

        // first reduction
        {
            let count = self
                .automaton
                .get_actions_count(self.gss.get_represented_state(gss_node), terminal.id);
            for j in 0..count {
                let action = self.automaton.get_action(
                    self.gss.get_represented_state(gss_node),
                    terminal.id,
                    j,
                );
                if action.get_code() != LR_ACTION_CODE_REDUCE {
                    continue;
                }
                let production = self.automaton.get_production(action.get_data() as usize);
                let paths = self.gss.get_paths(gss_node, production.reduction_length);
                for path in &paths {
                    // get the target GLR state
                    let next = self.get_next_by_var(
                        self.gss.get_represented_state(path.last_node),
                        self.variables[production.head].id,
                    );
                    // enqueue the info, top GSS stack node and target GLR state
                    queue_gss_heads.push(path.last_node);
                    queue_vstack.push(alloc::vec![next.unwrap()]);
                }
            }
        }

        // now, close the queue
        let mut i = 0;
        while i < queue_gss_heads.len() {
            let head = queue_vstack[i][queue_vstack[i].len() - 1];
            let gss_node = queue_gss_heads[i];
            let count = self.automaton.get_actions_count(head, terminal.id);
            if count == 0 {
                i += 1;
                continue;
            }
            for j in 0..count {
                let action = self.automaton.get_action(head, terminal.id, j);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    // yep, the terminal was expected
                    return true;
                }
                if action.get_code() != LR_ACTION_CODE_REDUCE {
                    continue;
                }
                // execute the reduction
                let production = self.automaton.get_production(action.get_data() as usize);
                if production.reduction_length == 0 {
                    // 0-length reduction => start from the current head
                    let mut virtual_stack = queue_vstack[i].clone();
                    let next = self.get_next_by_var(head, self.variables[production.head].id);
                    virtual_stack.push(next.unwrap());
                    // enqueue
                    queue_gss_heads.push(gss_node);
                    queue_vstack.push(virtual_stack);
                } else if production.reduction_length < queue_vstack[i].len() {
                    // we are still the virtual stack
                    let mut virtual_stack =
                        Vec::with_capacity(queue_vstack[i].len() - production.reduction_length + 1);
                    for k in 0..(queue_vstack[i].len() - production.reduction_length) {
                        virtual_stack.push(queue_vstack[i][k]);
                    }
                    let next = self.get_next_by_var(head, self.variables[production.head].id);
                    virtual_stack.push(next.unwrap());
                    // enqueue
                    queue_gss_heads.push(gss_node);
                    queue_vstack.push(virtual_stack);
                } else {
                    // we reach the GSS
                    let paths = self.gss.get_paths(
                        gss_node,
                        production.reduction_length - queue_vstack[i].len(),
                    );
                    for path in &paths {
                        // get the target GLR state
                        let next = self.get_next_by_var(
                            self.gss.get_represented_state(path.last_node),
                            self.variables[production.head].id,
                        );
                        // enqueue the info, top GSS stack node and target GLR state
                        queue_gss_heads.push(path.last_node);
                        queue_vstack.push(alloc::vec![next.unwrap()]);
                    }
                }
            }
            i += 1;
        }
        // nope, that was a pathological case in a LALR graph
        false
    }

    /// Gets the next RNGLR state by a shift with the given variable ID
    fn get_next_by_var(&self, state: u32, variable_id: u32) -> Option<u32> {
        let count = self.automaton.get_actions_count(state, variable_id);
        for i in 0..count {
            let action = self.automaton.get_action(state, variable_id, i);
            if action.get_code() == LR_ACTION_CODE_SHIFT {
                return Some(u32::from(action.get_data()));
            }
        }
        None
    }

    /// Executes a shift operation
    fn parse_shift(&mut self, generation: usize, label: GSSLabel, shift: RNGLRShift) {
        let w = self.gss.find_node(generation, shift.to as u32);
        if let Some(w) = w {
            // A node for the target state is already in the GSS
            self.gss.create_edge(w, shift.from, label);
            // Look for the new reductions at this state
            let count = self
                .automaton
                .get_actions_count(shift.to as u32, self.get_next_token_id());
            for i in 0..count {
                let action =
                    self.automaton
                        .get_action(shift.to as u32, self.get_next_token_id(), i);
                if action.get_code() == LR_ACTION_CODE_REDUCE {
                    let production = self.automaton.get_production(action.get_data() as usize);
                    // length 0 reduction are not considered here because they already exist at this point
                    if production.reduction_length > 0 {
                        self.reductions.push_back(RNGLRReduction {
                            node: shift.from,
                            production: action.get_data() as usize,
                            first: label,
                        });
                    }
                }
            }
        } else {
            // Create the new corresponding node in the GSS
            let w = self.gss.create_node(shift.to as u32);
            self.gss.create_edge(w, shift.from, label);
            // Look for all the reductions and shifts at this state
            let count = self
                .automaton
                .get_actions_count(shift.to as u32, self.get_next_token_id());
            for i in 0..count {
                let action =
                    self.automaton
                        .get_action(shift.to as u32, self.get_next_token_id(), i);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    self.shifts.push_back(RNGLRShift {
                        from: w,
                        to: action.get_data() as usize,
                    });
                } else if action.get_code() == LR_ACTION_CODE_REDUCE {
                    let production = self.automaton.get_production(action.get_data() as usize);
                    if production.reduction_length == 0 {
                        // Length 0 => reduce from the head
                        self.reductions.push_back(RNGLRReduction {
                            node: w,
                            production: action.get_data() as usize,
                            first: EPSILON,
                        });
                    } else {
                        // reduce from the second node on the path
                        self.reductions.push_back(RNGLRReduction {
                            node: shift.from,
                            production: action.get_data() as usize,
                            first: label,
                        });
                    }
                }
            }
        }
    }
}

/// Represents a base for all RNGLR parsers
pub struct RNGLRParser<'s, 't, 'a, 'l> {
    /// The parser's data
    data: RNGLRParserData<'s, 'a>,
    /// The AST builder
    builder: SPPFBuilder<'s, 't, 'a, 'l>,
    /// The sub-trees for the constant nullable variables
    nullables: Vec<usize>,
}

impl<'s, 't, 'a, 'l> RNGLRParser<'s, 't, 'a, 'l> {
    /// Initializes a new instance of the parser
    pub fn new_with_ast(
        lexer: &'l mut Lexer<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        automaton: RNGLRAutomaton,
        ast: &'a mut AstImpl,
        actions: &'a mut dyn FnMut(usize, Symbol, &dyn SemanticBody),
    ) -> RNGLRParser<'s, 't, 'a, 'l> {
        let mut parser = RNGLRParser {
            data: RNGLRParserData {
                automaton,
                gss: GSS::new(),
                next_token: None,
                reductions: VecDeque::new(),
                shifts: VecDeque::new(),
                variables,
                actions,
            },
            builder: SPPFBuilder::new_ast(lexer, variables, virtuals, ast),
            nullables: alloc::vec![0xFFFF_FFFF ; variables.len()],
        };
        RNGLRParser::build_nullables(
            &mut parser.builder,
            &mut parser.data.actions,
            &mut parser.nullables,
            &parser.data.automaton,
            parser.data.variables,
        );
        parser
    }

    /// Initializes a new instance of the parser
    pub fn new_with_sppf(
        lexer: &'l mut Lexer<'s, 't, 'a>,
        variables: &'a [Symbol<'s>],
        virtuals: &'a [Symbol<'s>],
        automaton: RNGLRAutomaton,
        sppf: &'a mut SppfImpl,
        actions: &'a mut dyn FnMut(usize, Symbol, &dyn SemanticBody),
    ) -> RNGLRParser<'s, 't, 'a, 'l> {
        let mut parser = RNGLRParser {
            data: RNGLRParserData {
                automaton,
                gss: GSS::new(),
                next_token: None,
                reductions: VecDeque::new(),
                shifts: VecDeque::new(),
                variables,
                actions,
            },
            builder: SPPFBuilder::new_sppf(lexer, variables, virtuals, sppf),
            nullables: alloc::vec![0xFFFF_FFFF ; variables.len()],
        };
        RNGLRParser::build_nullables(
            &mut parser.builder,
            &mut parser.data.actions,
            &mut parser.nullables,
            &parser.data.automaton,
            parser.data.variables,
        );
        parser
    }

    /// Builds the constant sub-trees of nullable variables
    fn build_nullables(
        builder: &mut SPPFBuilder<'s, 't, 'a, 'l>,
        actions: &mut dyn FnMut(usize, Symbol, &dyn SemanticBody),
        nullables: &mut [usize],
        automaton: &RNGLRAutomaton,
        variables: &[Symbol],
    ) {
        // Get the dependency table
        let mut dependencies = RNGLRParser::build_nullables_dependencies(automaton, variables);
        // Solve and build
        let mut remaining = 1;
        while remaining > 0 {
            remaining = 0;
            let mut resolved = 0;
            for i in 0..variables.len() {
                let production = automaton.get_nullable_production(i);
                if production.is_none() || nullables[i] != 0xFFFF_FFFF {
                    // not nullable, or already resolved
                    continue;
                }
                let can_resolve = dependencies[i].is_empty()
                    || dependencies[i].iter().all(|&d| nullables[d] != 0xFFFF_FFFF);
                if can_resolve {
                    let path = GSSPath::new(0, 0, 0);
                    nullables[i] = RNGLRParser::build_sppf(
                        builder,
                        actions,
                        nullables,
                        production.unwrap(),
                        EPSILON,
                        &path,
                        None,
                    )
                    .node_id();
                    dependencies[i].clear();
                    resolved += 1;
                } else {
                    remaining += 1;
                }
            }
            // There is dependency cycle ...
            // That should not be possible ...
            assert!(
                !(resolved == 0 && remaining > 0),
                "Failed to initialize the parser, found a cycle in the nullable variables"
            );
        }
    }

    /// Builds the dependency table between nullable variables
    fn build_nullables_dependencies(
        automaton: &RNGLRAutomaton,
        variables: &[Symbol],
    ) -> Vec<Vec<usize>> {
        variables
            .iter()
            .enumerate()
            .map(|(i, _variable)| {
                automaton
                    .get_nullable_production(i)
                    .map(RNGLRParser::build_nullable_dependencies_for)
                    .unwrap_or_default()
            })
            .collect()
    }

    /// Gets the dependencies on nullable variables
    fn build_nullable_dependencies_for(production: &LRProduction) -> Vec<usize> {
        let mut result = Vec::new();
        let mut i = 0;
        while i < production.bytecode.len() {
            let op_code = production.bytecode[i];
            i += 1;
            match get_op_code_base(op_code) {
                LR_OP_CODE_BASE_SEMANTIC_ACTION | LR_OP_CODE_BASE_ADD_VIRTUAL => {
                    i += 1;
                }
                LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE => {
                    result.push(production.bytecode[i] as usize);
                    i += 1;
                }
                _ => {
                    break;
                }
            }
        }
        result
    }

    /// Builds the SPPF
    fn build_sppf(
        builder: &mut SPPFBuilder<'s, 't, 'a, 'l>,
        actions: &mut dyn FnMut(usize, Symbol, &dyn SemanticBody),
        nullables: &[usize],
        production: &LRProduction,
        first: GSSLabel,
        path: &GSSPath,
        target: Option<SppfImplNodeRef>,
    ) -> SppfImplNodeRef {
        let variable = builder.variables[production.head];
        builder.reduction_prepare(first, path, production.reduction_length);
        let mut i = 0;
        while i < production.bytecode.len() {
            let op_code = production.bytecode[i];
            i += 1;
            match get_op_code_base(op_code) {
                LR_OP_CODE_BASE_SEMANTIC_ACTION => {
                    let index = production.bytecode[i] as usize;
                    i += 1;
                    actions(index, variable, builder);
                }
                LR_OP_CODE_BASE_ADD_VIRTUAL => {
                    let index = production.bytecode[i] as usize;
                    i += 1;
                    builder.reduction_add_virtual(index, get_op_code_tree_action(op_code));
                }
                LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE => {
                    let index = production.bytecode[i] as usize;
                    i += 1;
                    builder.reduction_add_nullable(
                        SppfImplNodeRef::new_usize(nullables[index]),
                        get_op_code_tree_action(op_code),
                    );
                }
                _ => {
                    builder.reduction_pop(get_op_code_tree_action(op_code));
                }
            }
        }
        builder.reduce(production.head, production.head_action, target)
    }

    /// Gets the next token in the kernel
    fn get_next_token(&mut self) {
        let next_token = {
            let data = &self.data;
            self.builder.lexer.get_next_token(data)
        };
        self.data.next_token = next_token;
    }

    /// Executes the reduction operations from the given GSS generation
    fn parse_reductions(&mut self, generation: usize) {
        while !self.data.reductions.is_empty() {
            let reduction = self.data.reductions.pop_front().unwrap();
            self.parse_reduction(generation, reduction);
        }
    }

    /// Executes a reduction operation for all found path
    fn parse_reduction(&mut self, generation: usize, reduction: RNGLRReduction) {
        let paths = {
            let production = self.data.automaton.get_production(reduction.production);
            if production.reduction_length == 0 {
                self.data.gss.get_paths(reduction.node, 0)
            } else {
                // The given GSS node is the second on the path, so start from it with length - 1
                self.data
                    .gss
                    .get_paths(reduction.node, production.reduction_length - 1)
            }
        };
        for path in &paths {
            self.parse_reduction_path(generation, reduction, path);
        }
    }

    /// Executes a reduction operation for a given path
    fn parse_reduction_path(
        &mut self,
        generation: usize,
        reduction: RNGLRReduction,
        path: &GSSPath,
    ) {
        let production = self.data.automaton.get_production(reduction.production);
        // Get the rule's head
        let head = self.data.variables[production.head];
        // Get the target state by transition on the rule's head
        let to = self
            .data
            .get_next_by_var(self.data.gss.get_represented_state(path.last_node), head.id)
            .unwrap();
        // Find a node for the target state in the GSS
        let w = self.data.gss.find_node(generation, to);
        // Do we have to create a GSS edge?
        let previous_edge_label = w.and_then(|w| {
            self.data
                .gss
                .get_edge(generation, w, path.last_node)
                .map(|edge| edge.label)
        });
        let sppf_node =
            if self.data.automaton.nullables[production.head] as usize == reduction.production {
                // nullable production, use the nullable node
                SppfImplNodeRef::new_usize(self.nullables[production.head])
            } else {
                RNGLRParser::build_sppf(
                    &mut self.builder,
                    &mut self.data.actions,
                    &self.nullables,
                    production,
                    reduction.first,
                    path,
                    previous_edge_label
                        .as_ref()
                        .map(|previous| previous.sppf_node),
                )
            };
        let label = previous_edge_label.unwrap_or(GSSLabel {
            sppf_node,
            symbol_id: head.id,
        });

        if let Some(w) = w {
            // A node for the target state is already in the GSS
            if previous_edge_label.is_none() {
                // But the new edge does not exist
                self.data.gss.create_edge(w, path.last_node, label);
                // Look for the new reductions at this state
                if production.reduction_length != 0 {
                    let count = self
                        .data
                        .automaton
                        .get_actions_count(to, self.data.get_next_token_id());
                    for i in 0..count {
                        let action =
                            self.data
                                .automaton
                                .get_action(to, self.data.get_next_token_id(), i);
                        if action.get_code() == LR_ACTION_CODE_REDUCE {
                            let new_production = self
                                .data
                                .automaton
                                .get_production(action.get_data() as usize);
                            // length 0 reduction are not considered here because they already exist at this point
                            if new_production.reduction_length > 0 {
                                self.data.reductions.push_back(RNGLRReduction {
                                    node: path.last_node,
                                    production: action.get_data() as usize,
                                    first: label,
                                });
                            }
                        }
                    }
                }
            }
        } else {
            // Create the new corresponding node in the GSS
            let w = self.data.gss.create_node(to);
            self.data.gss.create_edge(w, path.last_node, label);
            // Look for all the reductions and shifts at this state
            let count = self
                .data
                .automaton
                .get_actions_count(to, self.data.get_next_token_id());
            for i in 0..count {
                let action = self
                    .data
                    .automaton
                    .get_action(to, self.data.get_next_token_id(), i);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    self.data.shifts.push_back(RNGLRShift {
                        from: w,
                        to: action.get_data() as usize,
                    });
                } else if action.get_code() == LR_ACTION_CODE_REDUCE {
                    let new_production = self
                        .data
                        .automaton
                        .get_production(action.get_data() as usize);
                    if new_production.reduction_length == 0 {
                        self.data.reductions.push_back(RNGLRReduction {
                            node: w,
                            production: action.get_data() as usize,
                            first: EPSILON,
                        });
                    } else {
                        self.data.reductions.push_back(RNGLRReduction {
                            node: path.last_node,
                            production: action.get_data() as usize,
                            first: label,
                        });
                    }
                }
            }
        }
    }

    /// Executes the shift operations for the given token
    fn parse_shifts(&mut self, old_token: TokenKernel) -> usize {
        // Create next generation
        let new_gen = self.data.gss.create_generation();
        // Create the GSS label to be used for the transitions
        let symbol = TableElemRef::new(TableType::Token, old_token.index as usize);
        let sppf_node = self.builder.get_single_node(symbol);
        let label = GSSLabel {
            sppf_node,
            symbol_id: old_token.terminal_id,
        };
        // Execute all shifts in the queue at this point
        let count = self.data.shifts.len();
        for _i in 0..count {
            let shift = self.data.shifts.pop_front().unwrap();
            self.data.parse_shift(new_gen, label, shift);
        }
        new_gen
    }

    /// Builds the unexpected token error
    fn build_error(&self, kernel: TokenKernel, stem: usize) -> ParseErrorUnexpectedToken<'s> {
        let token = self
            .builder
            .lexer
            .get_data()
            .repository
            .get_token(kernel.index as usize);
        let mut my_states = Vec::new();
        let mut my_expected = Vec::new();
        let generation_data = self.data.gss.get_current_generation();
        for i in 0..generation_data.count {
            let state = self
                .data
                .gss
                .get_represented_state(generation_data.start + i);
            my_states.push(state);
            let expected_on_head = self
                .data
                .automaton
                .get_expected(state, self.builder.lexer.get_data().repository.terminals);
            // register the terminals for shift actions
            for symbol in &expected_on_head.shifts {
                if !my_expected.contains(symbol) {
                    my_expected.push(*symbol);
                }
            }
            if i < stem {
                // the state was in the stem, also look for reductions
                for symbol in &expected_on_head.reductions {
                    if !my_expected.contains(symbol)
                        && self
                            .data
                            .check_is_expected(generation_data.start + i, *symbol)
                    {
                        my_expected.push(*symbol);
                    }
                }
            }
        }
        ParseErrorUnexpectedToken::new(
            token.get_position().unwrap(),
            token.get_span().unwrap().length,
            token.get_value().unwrap().to_string(),
            token.get_symbol(),
            #[cfg(feature = "debug")]
            my_states,
            my_expected,
        )
    }
}

impl<'s, 't, 'a, 'l> Parser for RNGLRParser<'s, 't, 'a, 'l> {
    fn parse(&mut self) {
        let mut generation = self.data.gss.create_generation();
        let state0 = self.data.gss.create_node(0);
        self.get_next_token();

        // bootstrap the shifts and reductions queues
        {
            let count = self
                .data
                .automaton
                .get_actions_count(0, self.data.get_next_token_id());
            for i in 0..count {
                let action = self
                    .data
                    .automaton
                    .get_action(0, self.data.get_next_token_id(), i);
                if action.get_code() == LR_ACTION_CODE_SHIFT {
                    self.data.shifts.push_back(RNGLRShift {
                        from: state0,
                        to: action.get_data() as usize,
                    });
                } else if action.get_code() == LR_ACTION_CODE_REDUCE {
                    self.data.reductions.push_back(RNGLRReduction {
                        node: state0,
                        production: action.get_data() as usize,
                        first: EPSILON,
                    });
                }
            }
        }

        // Wait for ε token
        while self.data.get_next_token_id() != SID_EPSILON {
            // the stem length (initial number of nodes in the generation before reductions)
            let stem = self.data.gss.get_generation(generation).count;
            // apply all reduction actions
            self.parse_reductions(generation);
            // no scheduled shift actions?
            if self.data.shifts.is_empty() {
                // this is an error
                let error = self.build_error(self.data.next_token.unwrap(), stem);
                self.builder
                    .lexer
                    .get_data_mut()
                    .errors
                    .push_error_unexpected_token(error);
                // TODO: try to recover here
                return;
            }
            // look for the next next-token
            let old_token = self.data.next_token.unwrap();
            self.get_next_token();
            // apply the scheduled shift actions
            generation = self.parse_shifts(old_token);
        }

        let generation_data = self.data.gss.get_generation(generation);
        for i in generation_data.start..(generation_data.start + generation_data.count) {
            let state = self.data.gss.get_represented_state(i);
            if self.data.automaton.is_accepting_state(state) {
                // Has reduction _Axiom_ -> axiom $ . on ε
                let paths = self.data.gss.get_paths(i, 2);
                let root = paths[0].labels[1];
                self.builder.commit_root(root.sppf_node);
            }
        }
        // At end of input but was still waiting for tokens
    }
}
