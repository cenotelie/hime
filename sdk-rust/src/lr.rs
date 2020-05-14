/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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

//! Module for LR automata

use crate::grammars::{Grammar, SymbolRef, TerminalRef, TerminalSet};
use std::collections::HashMap;

/// A reference to a grammar rule
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub struct RuleRef {
    /// The identifier of the variable
    pub variable: usize,
    /// The index of the rule for the variable
    pub index: usize
}

/// Represents a base LR item
#[derive(Debug, Clone, PartialEq, Eq)]
pub struct Item {
    /// The grammar rule for the item
    pub rule: RuleRef,
    /// The position in the grammar rule
    pub position: usize,
    /// The lookaheads for this item
    pub lookaheads: TerminalSet
}

/// Represents the kernel of a LR state
#[derive(Debug, Clone, Eq)]
pub struct StateKernel {
    /// The items in this kernel
    pub items: Vec<Item>
}

impl PartialEq for StateKernel {
    fn eq(&self, other: &StateKernel) -> bool {
        self.items.len() == other.items.len()
            && self.items.iter().all(|item| other.items.contains(item))
    }
}

/// Represents a reduction action in a LR state
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub struct Reduction {
    /// The lookahead to reduce on
    pub lookahead: TerminalRef,
    /// The rule to reduce with
    pub rule: RuleRef,
    /// The length of the reduction for RNGLR parsers
    pub length: usize
}

/// Represents a LR state
#[derive(Debug, Clone)]
pub struct State {
    /// The state's kernel
    pub kernel: StateKernel,
    /// The state's item
    pub items: Vec<Item>,
    /// The state's children (transitions)
    pub children: HashMap<SymbolRef, usize>,
    /// The contexts opening by transitions from this state
    pub opening_contexts: HashMap<TerminalRef, Vec<usize>>,
    /// The reductions on this state
    pub reductions: Vec<Reduction>
}

/// Represents a LR graph
#[derive(Debug, Clone, Default)]
pub struct Graph {
    /// The states in this graph
    pub states: Vec<State>
}

impl Graph {
    /// Initializes a graph from the given state
    pub fn from(state: State, grammar: &Grammar) -> Graph {
        let mut graph = Graph::default();
        graph.states.push(state);
        let mut i = 0;
        while i < graph.states.len() {
            graph.build_at_state(grammar, i);
            i += 1;
        }
        graph
    }

    /// Build this graph at the given state
    fn build_at_state(&mut self, grammar: &Grammar, state_id: usize) {}

    /// Determines whether the given state (as a kernel) is already in this graph
    pub fn get_state_for(&self, kernel: &StateKernel) -> Option<usize> {
        self.states.iter().position(|state| &state.kernel == kernel)
    }

    /// Adds a state to this graph
    pub fn add_state(&mut self, state: State) -> usize {
        let index = self.states.len();
        self.states.push(state);
        index
    }
}
