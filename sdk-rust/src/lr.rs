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

use crate::grammars::{Grammar, Rule, RuleChoice, SymbolRef, TerminalRef, TerminalSet};
use hime_redist::parsers::{LRActionCode, LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_SHIFT};
use std::collections::HashMap;

/// A reference to a grammar rule
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub struct RuleRef {
    /// The identifier of the variable
    pub variable: usize,
    /// The index of the rule for the variable
    pub index: usize
}

impl RuleRef {
    /// Creates a new rule reference
    pub fn new(variable: usize, index: usize) -> RuleRef {
        RuleRef { variable, index }
    }

    /// Gets the referenced rule in the grammar
    pub fn get_rule_in<'s, 'g>(&'s self, grammar: &'g Grammar) -> &'g Rule {
        &grammar.variables[self.variable].rules[self.index]
    }
}

/// The lookahead mode for LR items
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub enum LookaheadMode {
    /// LR(0) item (no lookahead)
    LR0,
    /// LR(1) item (exactly one lookahead)
    LR1,
    /// LALR(1) item (multiple lookahead)
    LALR1
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

impl Item {
    /// Gets the action for this item
    pub fn get_action(&self, grammar: &Grammar) -> LRActionCode {
        let rule = self.rule.get_rule_in(grammar);
        if self.position >= rule.body.choices[0].parts.len() {
            LR_ACTION_CODE_REDUCE
        } else {
            LR_ACTION_CODE_SHIFT
        }
    }

    /// Gets the symbol following the dot in this item
    pub fn get_next_symbol(&self, grammar: &Grammar) -> Option<SymbolRef> {
        let rule = self.rule.get_rule_in(grammar);
        if self.position >= rule.body.choices[0].parts.len() {
            None
        } else {
            Some(rule.body.choices[0].parts[self.position].symbol)
        }
    }

    /// Gets rule choice following the dot in this item
    pub fn get_next_choice<'s, 'g>(&'s self, grammar: &'g Grammar) -> Option<&'g RuleChoice> {
        let rule = self.rule.get_rule_in(grammar);
        if self.position >= rule.body.choices[0].parts.len() {
            None
        } else {
            Some(&rule.body.choices[self.position + 1])
        }
    }

    /// Gets the child of this item
    /// The child item is undefined if the action is REDUCE
    pub fn get_child(&self) -> Item {
        Item {
            rule: self.rule,
            position: self.position + 1,
            lookaheads: self.lookaheads.clone()
        }
    }

    /// Gets the context opened by this item
    pub fn get_opened_context(&self, grammar: &Grammar) -> Option<usize> {
        if self.position > 0 {
            // not at the beginning
            return None;
        }
        let rule = self.rule.get_rule_in(grammar);
        if self.position < rule.body.choices[0].parts.len() && rule.context != 0 {
            // this is a shift to a symbol with a context
            Some(rule.context)
        } else {
            None
        }
    }

    /// Closes this item into the given closure
    pub fn close_to(&self, grammar: &Grammar, closure: &mut Vec<Item>, mode: LookaheadMode) {
        if let Some(SymbolRef::Variable(sid)) = self.get_next_symbol(grammar) {
            // Here the item is of the form [Var -> alpha . next beta]
            // next is a variable
            // Firsts is a copy of the Firsts set for beta (next choice)
            // Firsts will contains symbols that may follow Next
            // Firsts will therefore be the lookahead for child items
            let mut firsts = self.get_next_choice(grammar).unwrap().firsts.clone();
            // If beta is nullifiable (contains ε) :
            if let Some(eps_index) = firsts
                .content
                .iter()
                .position(|x| *x == TerminalRef::Epsilon)
            {
                // Remove ε
                firsts.content.remove(eps_index);
                // Add the item's lookaheads
                firsts.add_others(&self.lookaheads);
            }
            let variable = grammar.get_variable(sid).unwrap();
            // For each rule that has Next as a head variable :
            for index in 0..variable.rules.len() {
                match mode {
                    LookaheadMode::LR0 => {
                        let candidate = Item {
                            rule: RuleRef::new(sid, index),
                            position: 0,
                            lookaheads: firsts.clone()
                        };
                        if !closure.contains(&candidate) {
                            closure.push(candidate);
                        }
                    }
                    LookaheadMode::LR1 => {
                        for terminal in firsts.clone().content.into_iter() {
                            let candidate = Item {
                                rule: RuleRef::new(sid, index),
                                position: 0,
                                lookaheads: TerminalSet::single(terminal)
                            };
                            if !closure.contains(&candidate) {
                                closure.push(candidate);
                            }
                        }
                    }
                    LookaheadMode::LALR1 => {
                        let candidate = Item {
                            rule: RuleRef::new(sid, index),
                            position: 0,
                            lookaheads: firsts.clone()
                        };
                        if let Some(other) =
                            closure.iter_mut().find(|item| item.same_base(&candidate))
                        {
                            other.lookaheads.add_others(&candidate.lookaheads);
                        } else {
                            closure.push(candidate);
                        }
                    }
                }
            }
        }
    }

    /// Gets whether the two items have the same base
    pub fn same_base(&self, other: &Item) -> bool {
        self.rule == other.rule && self.position == other.position
    }
}

/// Represents the kernel of a LR state
#[derive(Debug, Clone, Eq, Default)]
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

impl StateKernel {
    /// Gets the closure of this kernel
    pub fn into_state(self, grammar: &Grammar, mode: LookaheadMode) -> State {
        let mut items = self.items.clone();
        let mut i = 0;
        while i < items.len() {
            items[i].clone().close_to(grammar, &mut items, mode);
            i += 1;
        }
        State {
            kernel: self,
            items: items,
            children: HashMap::new(),
            opening_contexts: HashMap::new(),
            reductions: Vec::new()
        }
    }

    /// Adds an item to the kernel
    pub fn add_item(&mut self, item: Item) {
        if !self.items.contains(&item) {
            self.items.push(item);
        }
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
    pub fn from(state: State, grammar: &Grammar, mode: LookaheadMode) -> Graph {
        let mut graph = Graph::default();
        graph.states.push(state);
        let mut i = 0;
        while i < graph.states.len() {
            graph.build_at_state(grammar, i, mode);
            i += 1;
        }
        graph
    }

    /// Build this graph at the given state
    fn build_at_state(&mut self, grammar: &Grammar, state_id: usize, mode: LookaheadMode) {
        // Shift dictionnary for the current set
        let mut shifts: HashMap<SymbolRef, StateKernel> = HashMap::new();
        // Build the children kernels from the shift actions
        for item in self.states[state_id].items.iter() {
            if let Some(next) = item.get_next_symbol(grammar) {
                shifts
                    .entry(next)
                    .or_insert_with(StateKernel::default)
                    .add_item(item.get_child());
            }
        }
        // Close the children and add them to the graph
        for (next, kernel) in shifts.into_iter() {
            let child_index = match self.get_state_for(&kernel) {
                Some(child_index) => child_index,
                None => self.add_state(kernel.into_state(grammar, mode))
            };
            self.states[state_id].children.insert(next, child_index);
        }
        // Build the context data
        let state = &mut self.states[state_id];
        for item in state.items.iter() {
            if let Some(context) = item.get_opened_context(grammar) {
                let mut opening_terminals = TerminalSet::default();
                match item.get_next_symbol(grammar) {
                    Some(SymbolRef::Virtual(sid)) => {
                        let variable = &grammar.get_variable(sid).unwrap();
                        opening_terminals.add_others(&variable.firsts);
                    }
                    Some(SymbolRef::Epsilon) => {
                        opening_terminals.add(TerminalRef::Epsilon);
                    }
                    Some(SymbolRef::Dollar) => {
                        opening_terminals.add(TerminalRef::Dollar);
                    }
                    Some(SymbolRef::Dummy) => {
                        opening_terminals.add(TerminalRef::Dummy);
                    }
                    Some(SymbolRef::NullTerminal) => {
                        opening_terminals.add(TerminalRef::NullTerminal);
                    }
                    Some(SymbolRef::Terminal(sid)) => {
                        opening_terminals.add(TerminalRef::Terminal(sid));
                    }
                    _ => {}
                }
                for terminal in opening_terminals.content.into_iter() {
                    let contexts = state.opening_contexts.entry(terminal).or_default();
                    if !contexts.contains(&context) {
                        contexts.push(context);
                    }
                }
            }
        }
    }

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
