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

use crate::errors::{Error, Errors};
use crate::grammars::{
    Grammar, Rule, RuleChoice, SymbolRef, Terminal, TerminalRef, TerminalSet, GENERATED_AXIOM
};
use crate::ParsingMethod;
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
        &grammar
            .variables
            .iter()
            .find(|v| v.id == self.variable)
            .unwrap()
            .rules[self.index]
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
            items,
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

impl State {
    /// Builds reductions for this state
    pub fn build_reductions_lr0(&mut self, id: usize, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        let mut reduce_index = None;
        for (index, item) in self.items.iter().enumerate() {
            if item.get_action(grammar) != LR_ACTION_CODE_REDUCE {
                continue;
            }
            if !self.children.is_empty() {
                // shift/reduce conflict
                conflicts.raise_shift_reduce(
                    self,
                    id,
                    grammar,
                    item.clone(),
                    TerminalRef::NullTerminal
                );
            }
            if let Some(previous_index) = reduce_index {
                // reduce/reduce conflict
                let previous: &Item = &self.items[previous_index];
                conflicts.raise_reduce_reduce(
                    id,
                    previous.clone(),
                    item.clone(),
                    TerminalRef::NullTerminal
                );
            } else {
                reduce_index = Some(index);
                self.reductions.push(Reduction {
                    lookahead: TerminalRef::NullTerminal,
                    rule: item.rule,
                    length: item.position
                });
            }
        }
        conflicts
    }

    /// Builds reductions for this state
    pub fn build_reductions_lr1(&mut self, id: usize, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        let mut reductions: HashMap<TerminalRef, usize> = HashMap::new();
        for (index, item) in self.items.iter().enumerate() {
            if item.get_action(grammar) != LR_ACTION_CODE_REDUCE {
                continue;
            }
            for lookahead in item.lookaheads.content.iter() {
                let symbol_ref: SymbolRef = (*lookahead).into();
                if self.children.contains_key(&symbol_ref) {
                    // There is already a shift action for the lookahead => conflict
                    conflicts.raise_shift_reduce(self, id, grammar, item.clone(), *lookahead);
                } else if let Some(previous_index) = reductions.get(lookahead) {
                    // There is already a reduction action for the lookahead => conflict
                    let previous: &Item = &self.items[*previous_index];
                    conflicts.raise_reduce_reduce(id, previous.clone(), item.clone(), *lookahead);
                } else {
                    // no conflict
                    reductions.insert(*lookahead, index);
                    self.reductions.push(Reduction {
                        lookahead: *lookahead,
                        rule: item.rule,
                        length: item.position
                    });
                }
            }
        }
        conflicts
    }

    /// Builds reductions for this state
    pub fn build_reductions_rnglr1(&mut self, id: usize, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        let mut reductions: HashMap<TerminalRef, usize> = HashMap::new();
        for (index, item) in self.items.iter().enumerate() {
            let rule = item.rule.get_rule_in(grammar);
            if item.get_action(grammar) == LR_ACTION_CODE_SHIFT
                && !rule.body.choices[item.position]
                    .firsts
                    .content
                    .contains(&TerminalRef::Epsilon)
            {
                // item is shift action and is not nullable after the dot
                continue;
            }
            for lookahead in item.lookaheads.content.iter() {
                let symbol_ref: SymbolRef = (*lookahead).into();
                if self.children.contains_key(&symbol_ref) {
                    // There is already a shift action for the lookahead => conflict
                    conflicts.raise_shift_reduce(self, id, grammar, item.clone(), *lookahead);
                } else if let Some(previous_index) = reductions.get(lookahead) {
                    // There is already a reduction action for the lookahead => conflict
                    let previous: &Item = &self.items[*previous_index];
                    conflicts.raise_reduce_reduce(id, previous.clone(), item.clone(), *lookahead);
                } else {
                    // no conflict
                    reductions.insert(*lookahead, index);
                    self.reductions.push(Reduction {
                        lookahead: *lookahead,
                        rule: item.rule,
                        length: item.position
                    });
                }
            }
        }
        conflicts
    }
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

    /// Builds the reductions for this graph
    pub fn build_reductions_lr0(&mut self, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        for (index, state) in self.states.iter_mut().enumerate() {
            conflicts.aggregate(state.build_reductions_lr0(index, grammar));
        }
        conflicts
    }

    /// Builds the reductions for this graph
    pub fn build_reductions_lr1(&mut self, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        for (index, state) in self.states.iter_mut().enumerate() {
            conflicts.aggregate(state.build_reductions_lr1(index, grammar));
        }
        conflicts
    }

    /// Builds the reductions for this graph
    pub fn build_reductions_rnglr1(&mut self, grammar: &Grammar) -> Conflicts {
        let mut conflicts = Conflicts::default();
        for (index, state) in self.states.iter_mut().enumerate() {
            conflicts.aggregate(state.build_reductions_rnglr1(index, grammar));
        }
        conflicts
    }

    /// Gets the inverse graph
    pub fn inverse(&self) -> InverseGraph {
        InverseGraph::from(self)
    }
}

/// An inverse LR graph
#[derive(Debug, Clone, Default)]
pub struct InverseGraph(HashMap<usize, HashMap<SymbolRef, Vec<usize>>>);

/// Queue element for exploring paths in the LR graph
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
struct PNode {
    /// The associated LR state
    state: usize,
    /// The transition to investigate
    transition: Option<SymbolRef>,
    /// The next element
    next: Option<usize>
}

impl PNode {
    /// Creates a new path element
    fn new(state: usize, transition: Option<SymbolRef>, next: Option<usize>) -> PNode {
        PNode {
            state,
            transition,
            next
        }
    }
}

/// The element of a path in a LR
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct PathElem {
    /// The LR state at this step
    pub state: usize,
    /// The symbol to use as a transition
    pub transition: Option<SymbolRef>
}

/// A path in a LR graph
#[derive(Debug, Default, Clone)]
pub struct Path(pub Vec<PathElem>);

impl Path {
    /// Gets the corresponding input phrase
    pub fn get_phrase(&self, grammar: &Grammar) -> Phrase {
        let mut phrase = Phrase::default();
        for elem in self.0.iter() {
            match elem.transition {
                Some(SymbolRef::Variable(id)) => {
                    let mut stack = Vec::new();
                    phrase.build_input(grammar, id, &mut stack);
                }
                Some(SymbolRef::Terminal(id)) => {
                    // easy, just add it to the sample
                    phrase.append(TerminalRef::Terminal(id));
                }
                _ => { /* ignore */ }
            }
        }
        phrase
    }
}

impl InverseGraph {
    /// Builds the inverse graph
    pub fn from(graph: &Graph) -> InverseGraph {
        let mut transitions = HashMap::new();
        for (id, state) in graph.states.iter().enumerate() {
            for (terminal, child) in state.children.iter() {
                transitions
                    .entry(*child)
                    .or_insert_with(HashMap::new)
                    .entry(*terminal)
                    .or_insert_with(Vec::new)
                    .push(id);
            }
        }
        InverseGraph(transitions)
    }

    /// Gets all the paths from state 0 to the specified one
    pub fn get_paths_to(&self, target: usize) -> Vec<Path> {
        let mut elements: Vec<PNode> = vec![PNode::new(target, None, None)];
        let mut visited: HashMap<usize, Vec<SymbolRef>> = HashMap::new();
        let mut queue: Vec<usize> = vec![0];
        let mut goals: Vec<usize> = Vec::new();
        while !queue.is_empty() {
            let current = queue.remove(0);
            if let Some(transitions) = self.0.get(&elements[current].state) {
                for (symbol, antecedents) in transitions.iter() {
                    for previous in antecedents {
                        let visited_with = visited.entry(*previous).or_insert_with(Vec::new);
                        if visited_with.contains(symbol) {
                            continue;
                        }
                        visited_with.push(*symbol);
                        let index = elements.len();
                        elements.push(PNode::new(*previous, Some(*symbol), Some(current)));
                        if *previous == 0 {
                            goals.push(index);
                        } else {
                            queue.push(index);
                        }
                    }
                }
            }
        }
        goals
            .into_iter()
            .map(|goal| {
                let mut parts = Vec::new();
                let mut current = Some(goal);
                while let Some(current_id) = current {
                    let node = &elements[current_id];
                    parts.push(PathElem {
                        state: node.state,
                        transition: node.transition
                    });
                    current = node.next;
                }
                Path(parts)
            })
            .collect()
    }

    /// Gets possible inputs that allows for reaching the specified state from state 0
    pub fn get_inputs_for(&self, state: usize, grammar: &Grammar) -> Vec<Phrase> {
        self.get_paths_to(state)
            .into_iter()
            .map(|path| path.get_phrase(grammar))
            .collect()
    }
}

/// Represents a phrase that can be produced by grammar.
/// It is essentially a list of terminals
#[derive(Debug, Default, Clone, Eq)]
pub struct Phrase(pub Vec<TerminalRef>);

impl PartialEq for Phrase {
    fn eq(&self, other: &Phrase) -> bool {
        self.0.len() == other.0.len() && self.0.iter().zip(other.0.iter()).all(|(x, y)| x == y)
    }
}

impl Phrase {
    /// Appends a terminal to this phrase
    pub fn append(&mut self, terminal: TerminalRef) {
        self.0.push(terminal);
    }

    /// Builds the input by decomposing the given variable
    /// This methods recursively triggers the production of encoutered variables to arrive to the terminal symbols.
    /// The methods also tries do not go into an infinite loop by keeping track of the rule definitions that are currently used.
    /// If a rule definition has already been used (is in the stack) the method avoids it
    fn build_input(&mut self, grammar: &Grammar, variable: usize, stack: &mut Vec<RuleRef>) {
        let variable = grammar.get_variable(variable).unwrap();
        // if the variable to decompose is nullable (epsilon is in the FIRSTS state), stop here
        if variable.firsts.content.contains(&TerminalRef::Epsilon) {
            return;
        }
        let rule_index = match (0..(variable.rules.len())).find(|index| {
            // if it is not in the stack of rule definition
            !stack.contains(&RuleRef::new(variable.id, *index))
        }) {
            Some(index) => index,
            // if all identified rule definitions are in the stack (currently in use)
            None => 0
        };

        let elements = stack.clone();
        // push the rule definition to use onto the stack
        stack.push(RuleRef::new(variable.id, rule_index));
        // walk the rule definition to build the sample
        for element in variable.rules[rule_index].body.choices[0].parts.iter() {
            match element.symbol {
                SymbolRef::Variable(id) => {
                    // TODO: cleanup this code, this is really not a nice patch!!
                    // TODO: this is surely ineficient, but I don't understand the algorithm to do better yet
                    // The idea is to try and avoid infinite recursive call by checking the symbol was not already processed before
                    // This code checks whether the variable in this part is not already in one of the rule definition currently in the stack
                    let found = elements.iter().any(|rule_ref| {
                        rule_ref.get_rule_in(grammar).body.choices[0]
                            .parts
                            .iter()
                            .any(|part| part.symbol == element.symbol)
                    });
                    // if part.Symbol is not the same as another part.symbol found in a previous definition
                    if !found {
                        self.build_input(grammar, id, stack);
                    }
                    // TODO: what do we do if the variable was found?
                }
                SymbolRef::Terminal(id) => {
                    // easy, just add it to the sample
                    self.append(TerminalRef::Terminal(id));
                }
                _ => { /* should not happen, ignore */ }
            }
        }
        stack.pop();
    }
}

/// The kinds of LR conflicts
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
pub enum ConflictKind {
    /// Conflict between a shift action and a reduce action
    ShiftReduce,
    /// Conflict between two reduce actions
    ReduceReduce
}

/// A conflict between items
#[derive(Debug, Clone, Eq)]
pub struct Conflict {
    /// The state raising the conflict
    pub state: usize,
    /// The kind of conflict
    pub kind: ConflictKind,
    /// The items in the conflict
    pub items: Vec<Item>,
    /// The terminal that poses the conflict
    pub lookahead: TerminalRef
}

impl PartialEq for Conflict {
    fn eq(&self, other: &Conflict) -> bool {
        self.state == other.state && self.kind == other.kind && self.lookahead == other.lookahead
    }
}

/// A set of conflicts
#[derive(Debug, Default, Clone)]
pub struct Conflicts(Vec<Conflict>);

impl Conflicts {
    /// Raise a shift/reduce conflict
    pub fn raise_shift_reduce(
        &mut self,
        state: &State,
        state_id: usize,
        grammar: &Grammar,
        reducing: Item,
        lookahead: TerminalRef
    ) {
        // look for previous conflict
        for previous in self.0.iter_mut() {
            if previous.kind == ConflictKind::ShiftReduce && previous.lookahead == lookahead {
                // Previous conflict
                previous.items.push(reducing);
                return;
            }
        }
        // No previous conflict was found
        let mut items: Vec<Item> = state
            .items
            .iter()
            .filter(|item| item.get_next_symbol(grammar) == Some(lookahead.into()))
            .cloned()
            .collect();
        items.push(reducing);
        self.0.push(Conflict {
            state: state_id,
            kind: ConflictKind::ShiftReduce,
            items,
            lookahead
        });
    }

    /// Raise a reduce/reduce conflict
    pub fn raise_reduce_reduce(
        &mut self,
        state_id: usize,
        previous: Item,
        reducing: Item,
        lookahead: TerminalRef
    ) {
        // look for previous conflict
        for previous in self.0.iter_mut() {
            if previous.kind == ConflictKind::ReduceReduce && previous.lookahead == lookahead {
                // Previous conflict
                previous.items.push(reducing);
                return;
            }
        }
        // No previous conflict was found
        self.0.push(Conflict {
            state: state_id,
            kind: ConflictKind::ReduceReduce,
            items: vec![previous, reducing],
            lookahead
        });
    }

    /// Aggregate other conflicts into this collection
    pub fn aggregate(&mut self, mut other: Conflicts) {
        self.0.append(&mut other.0);
    }
}

/// Represents an error where a contextual terminal is expected but its context cannot be available at this point
#[derive(Debug, Clone, Eq)]
pub struct ContextError {
    /// The state raising the error
    pub state: usize,
    /// The state's items that requires the terminal
    pub items: Vec<Item>,
    /// The problematic contextual terminal
    pub terminal: TerminalRef,
    /// The problematic phrases
    pub phrases: Vec<Phrase>
}

impl PartialEq for ContextError {
    fn eq(&self, other: &ContextError) -> bool {
        self.terminal == other.terminal
            && self.items.len() == other.items.len()
            && self.items.iter().all(|item| other.items.contains(item))
    }
}

/// Gets the LR(0) graph
fn get_graph_lr0(grammar: &Grammar) -> Graph {
    // Create the base LR(0) graph
    let axiom = grammar.get_variable_for_name(GENERATED_AXIOM).unwrap();
    let item = Item {
        rule: RuleRef::new(axiom.id, 0),
        position: 0,
        lookaheads: TerminalSet::default()
    };
    let kernel = StateKernel { items: vec![item] };
    let state0 = kernel.into_state(grammar, LookaheadMode::LR0);
    Graph::from(state0, grammar, LookaheadMode::LR0)
}

/// Builds a LR(0) graph
pub fn build_graph_lr0(grammar: &Grammar) -> (Graph, Conflicts) {
    let mut graph = get_graph_lr0(grammar);
    let conflicts = graph.build_reductions_lr0(grammar);
    (graph, conflicts)
}

/// Gets the LR(1) graph
fn get_graph_lr1(grammar: &Grammar) -> Graph {
    // Create the base LR(0) graph
    let axiom = grammar.get_variable_for_name(GENERATED_AXIOM).unwrap();
    let item = Item {
        rule: RuleRef::new(axiom.id, 0),
        position: 0,
        lookaheads: TerminalSet::default()
    };
    let kernel = StateKernel { items: vec![item] };
    let state0 = kernel.into_state(grammar, LookaheadMode::LR1);
    Graph::from(state0, grammar, LookaheadMode::LR1)
}

/// Builds a LR(1) graph
pub fn build_graph_lr1(grammar: &Grammar) -> (Graph, Conflicts) {
    let mut graph = get_graph_lr1(grammar);
    let conflicts = graph.build_reductions_lr1(grammar);
    (graph, conflicts)
}

/// Builds a RNGLR(1) graph
pub fn build_graph_rnglr1(grammar: &Grammar) -> (Graph, Conflicts) {
    let mut graph = get_graph_lr1(grammar);
    let conflicts = graph.build_reductions_rnglr1(grammar);
    (graph, conflicts)
}

/// Builds the kernels for a LALR(1) graph
fn build_graph_lalr1_kernels(graph0: &Graph) -> Vec<StateKernel> {
    // copy kernel without the lookaheads
    let mut kernels: Vec<StateKernel> = graph0
        .states
        .iter()
        .map(|state| StateKernel {
            items: state
                .kernel
                .items
                .iter()
                .map(|item| Item {
                    rule: item.rule,
                    position: item.position,
                    lookaheads: TerminalSet::default()
                })
                .collect()
        })
        .collect();
    // set epsilon as lookahead on all items in kernel 0
    for item in kernels[0].items.iter_mut() {
        item.lookaheads.add(TerminalRef::Epsilon);
    }
    kernels
}

/// Item in a propagation table
#[derive(Debug, Copy, Clone, PartialEq, Eq)]
struct Propagation {
    from_state: usize,
    from_item: usize,
    to_state: usize,
    to_item: usize
}

/// Builds the propagation table for a LALR(1) graph
fn build_graph_lalr1_propagation_table(
    graph0: &Graph,
    grammar: &Grammar,
    kernels: &mut Vec<StateKernel>
) -> Vec<Propagation> {
    let mut propagation = Vec::new();
    for i in 0..kernels.len() {
        // For each LALR(1) item in the kernel
        // Only the kernel needs to be examined as the other items will be discovered and treated
        // with the dummy closures
        for item_id in 0..(kernels[i].items.len()) {
            if kernels[i].items[item_id].get_action(grammar) == LR_ACTION_CODE_REDUCE {
                // If item is of the form [A -> alpha .]
                // => The closure will only contain the item itself
                // => Cannot be used to generate or propagate lookaheads
                continue;
            }
            // Item here is of the form [A -> alpha . beta]
            // Create the corresponding dummy item : [A -> alpha . beta, dummy]
            // This item is used to detect lookahead propagation
            let dummy_state = StateKernel {
                items: vec![Item {
                    rule: kernels[i].items[item_id].rule,
                    position: kernels[i].items[item_id].position,
                    lookaheads: TerminalSet::single(TerminalRef::Dummy)
                }]
            }
            .into_state(grammar, LookaheadMode::LALR1);
            // For each item in the closure of the dummy item
            for dummy_item in dummy_state.items.iter() {
                if let Some(next_symbol) = dummy_item.get_next_symbol(grammar) {
                    // not a reduction
                    let dummy_child = dummy_item.get_child();
                    // Get the child item in the child LALR(1) kernel
                    let child_state = *graph0.states[i].children.get(&next_symbol).unwrap();
                    let child_item = kernels[child_state]
                        .items
                        .iter()
                        .position(|candidate| candidate.same_base(&dummy_child))
                        .unwrap();
                    // If the lookaheads of the item in the dummy set contains the dummy terminal
                    if dummy_item.lookaheads.content.contains(&TerminalRef::Dummy) {
                        // => Propagation from the parent item to the child
                        propagation.push(Propagation {
                            from_state: i,
                            from_item: item_id,
                            to_state: child_state,
                            to_item: child_item
                        });
                    } else {
                        // => Spontaneous generation of lookaheads
                        for lookahead in dummy_item.lookaheads.content.iter() {
                            kernels[child_state].items[child_item]
                                .lookaheads
                                .add(*lookahead);
                        }
                    }
                }
            }
        }
    }
    propagation
}

/// Executes the propagation for a LALR(1) graph
fn build_graph_lalr1_propagate(kernels: &mut Vec<StateKernel>, table: &[Propagation]) {
    let mut modifications = 1;
    while modifications != 0 {
        modifications = 0;
        for propagation in table.iter() {
            let before = kernels[propagation.to_state].items[propagation.to_item]
                .lookaheads
                .content
                .len();
            let others = kernels[propagation.from_state].items[propagation.from_item]
                .lookaheads
                .clone();
            kernels[propagation.to_state].items[propagation.to_item]
                .lookaheads
                .add_others(&others);
            let after = kernels[propagation.to_state].items[propagation.to_item]
                .lookaheads
                .content
                .len();
            modifications += after - before;
        }
    }
}

/// Builds the complete LALR(1) graph
fn build_graph_lalr1_graph(kernels: Vec<StateKernel>, graph0: &Graph, grammar: &Grammar) -> Graph {
    // Build states
    let mut states: Vec<State> = kernels
        .into_iter()
        .map(|kernel| kernel.into_state(grammar, LookaheadMode::LALR1))
        .collect();
    // Link for each LALR(1) set
    for (state0, state1) in graph0.states.iter().zip(states.iter_mut()) {
        state1.children = state0.children.clone();
        state1.opening_contexts = state0.opening_contexts.clone();
    }
    Graph { states }
}

/// Gets the LALR(1) graph
fn get_graph_lalr1(grammar: &Grammar) -> Graph {
    let graph0 = get_graph_lr0(grammar);
    let mut kernels = build_graph_lalr1_kernels(&graph0);
    let propagation = build_graph_lalr1_propagation_table(&graph0, grammar, &mut kernels);
    build_graph_lalr1_propagate(&mut kernels, &propagation);
    build_graph_lalr1_graph(kernels, &graph0, grammar)
}

/// Builds a LALR(1) graph
pub fn build_graph_lalr1(grammar: &Grammar) -> (Graph, Conflicts) {
    let mut graph = get_graph_lalr1(grammar);
    let conflicts = graph.build_reductions_lr1(grammar);
    (graph, conflicts)
}

/// Builds a RNGLALR(1) graph
pub fn build_graph_rnglalr1(grammar: &Grammar) -> (Graph, Conflicts) {
    let mut graph = get_graph_lalr1(grammar);
    let conflicts = graph.build_reductions_rnglr1(grammar);
    (graph, conflicts)
}

/// Find the potential context errors in the graph
fn find_context_errors(
    graph: &Graph,
    inverse: &InverseGraph,
    grammar: &Grammar
) -> Vec<ContextError> {
    let mut errors = Vec::new();
    for (from_state, state) in graph.states.iter().enumerate() {
        for (symbol, to_state) in state.children.iter() {
            if let SymbolRef::Terminal(tid) = *symbol {
                let terminal = grammar.get_terminal(tid).unwrap();
                if terminal.context == 0 {
                    continue;
                }
                // this is a contextual terminal, can we reach this state without the right context being available
                find_context_errors_in(
                    graph,
                    inverse,
                    grammar,
                    &mut errors,
                    from_state,
                    *to_state,
                    terminal
                )
            }
        }
    }
    errors
}

/// Find the potential context errors in the graph at a state
fn find_context_errors_in(
    graph: &Graph,
    inverse: &InverseGraph,
    grammar: &Grammar,
    errors: &mut Vec<ContextError>,
    from_state: usize,
    to_state: usize,
    terminal: &Terminal
) {
    let mut paths = inverse.get_paths_to(from_state);
    for path in paths.iter_mut() {
        path.0.push(PathElem {
            state: to_state,
            transition: Some(SymbolRef::Terminal(terminal.id))
        });
    }
    paths.retain(|path| {
        let mut found = false;
        for i in 0..(path.0.len() - 1) {
            for item in graph.states[path.0[i].state].items.iter() {
                if item.position == 0 && item.rule.get_rule_in(grammar).context == terminal.context
                {
                    // this is the opening of a context only if we are not going to the next state using the associated variable
                    let child_by_var = graph.states[to_state]
                        .children
                        .get(&SymbolRef::Variable(item.rule.variable));
                    found |= child_by_var.is_none() || child_by_var != Some(&path.0[i + 1].state);
                    break;
                }
            }
            if found {
                break;
            }
        }
        for item in graph.states[to_state].items.iter() {
            if item.position == 0 && item.rule.get_rule_in(grammar).context == terminal.context {
                found = true;
                break;
            }
        }
        !found
    });
    if !paths.is_empty() {
        let items: Vec<Item> = graph.states[from_state]
            .items
            .iter()
            .filter(|item| item.get_next_symbol(grammar) == Some(SymbolRef::Terminal(terminal.id)))
            .cloned()
            .collect();
        errors.push(ContextError {
            state: from_state,
            items,
            terminal: TerminalRef::Terminal(terminal.id),
            phrases: paths
                .into_iter()
                .map(|path| path.get_phrase(grammar))
                .collect()
        });
    }
}

/// Build the specified grammar
pub fn build_graph(grammar: &Grammar, method: ParsingMethod) -> Result<Graph, Errors> {
    let (graph, conflicts) = match method {
        ParsingMethod::LR0 => build_graph_lr0(grammar),
        ParsingMethod::LR1 => build_graph_lr1(grammar),
        ParsingMethod::LALR1 => build_graph_lalr1(grammar),
        ParsingMethod::RNGLR1 => build_graph_rnglr1(grammar),
        ParsingMethod::RNGLALR1 => build_graph_rnglalr1(grammar)
    };
    let inverse = graph.inverse();
    let mut errors = Errors::from(
        conflicts
            .0
            .into_iter()
            .map(|conflict| {
                let phrases = inverse.get_inputs_for(conflict.state, grammar);
                Error::LrConflict(conflict, phrases)
            })
            .collect()
    );
    for error in find_context_errors(&graph, &inverse, grammar).into_iter() {
        errors.errors.push(Error::TerminalOutsideContext(error));
    }
    if errors.errors.is_empty() {
        return Ok(graph);
    }
    Err(errors)
}
