/*******************************************************************************
 * Copyright (c) 2019 Association Cénotélie (cenotelie.fr)
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

//! Finite automata

use crate::grammars::{TerminalRef, TerminalSet};
use crate::{CharSpan, CHARSPAN_INVALID};
use std::cmp::Ordering;
use std::collections::HashMap;

/// Represents the value epsilon on NFA transitions
pub const EPSILON: CharSpan = CHARSPAN_INVALID;

/// Represents a marker for the final state of an automaton
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum FinalItem {
    /// Represents a fake marker of a final state in an automaton
    Dummy,
    /// A terminal symbol in a grammar
    Terminal(usize)
}

impl FinalItem {
    /// Gets the priority of this item
    pub fn priority(self) -> usize {
        match self {
            FinalItem::Dummy => 0,
            FinalItem::Terminal(id) => id
        }
    }
}

impl Ord for FinalItem {
    fn cmp(&self, other: &FinalItem) -> Ordering {
        self.priority().cmp(&other.priority())
    }
}

impl PartialOrd for FinalItem {
    fn partial_cmp(&self, other: &FinalItem) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

/// Represents a state in a Deterministic Finite Automaton
#[derive(Debug, Clone)]
pub struct DFAState {
    /// This state's id
    pub id: usize,
    /// The transitions from this state
    pub transitions: HashMap<CharSpan, usize>,
    /// List of the items on this state
    pub items: Vec<FinalItem>
}

impl DFAState {
    /// Initializes this state
    pub fn new(id: usize) -> DFAState {
        DFAState {
            id,
            transitions: HashMap::new(),
            items: Vec::new()
        }
    }

    /// Gets whether this state is final (i.e. it is marked with final items)
    pub fn is_final(&self) -> bool {
        !self.items.is_empty()
    }

    /// Gets the final with the most priority on this state
    pub fn get_final(&self) -> Option<FinalItem> {
        self.items.last().copied()
    }

    /// Determines if the two states have the same marks
    pub fn same_finals(&self, other: &DFAState) -> bool {
        if self.items.len() != other.items.len() {
            return false;
        }
        self.items.iter().all(|item| other.items.contains(item))
    }

    /// Adds a new item making this state a final state
    fn do_add_item(&mut self, item: FinalItem) {
        if !self.items.contains(&item) {
            self.items.push(item);
        }
    }

    /// Adds a new item making this state a final state
    pub fn add_item(&mut self, item: FinalItem) {
        self.do_add_item(item);
        self.items.sort();
    }

    /// Adds new items making this state a final state
    pub fn add_items(&mut self, items: &[FinalItem]) {
        for item in items.iter() {
            self.do_add_item(*item);
        }
        self.items.sort();
    }

    /// Clears all markers for this states making it non-final
    pub fn clear_items(&mut self) {
        self.items.clear();
    }

    /// Gets the child state by the specified transition
    pub fn get_child_by(&self, value: CharSpan) -> Option<usize> {
        self.transitions.get(&value).copied()
    }

    /// Determines whether this state has the specified transition
    pub fn has_transition(&self, value: CharSpan) -> bool {
        self.transitions.contains_key(&value)
    }

    /// Adds a transition from this state
    pub fn add_transition(&mut self, span: CharSpan, next: usize) {
        self.transitions.insert(span, next);
    }

    /// Removes a transition from this state
    pub fn remove_transition(&mut self, span: CharSpan) {
        self.transitions.remove(&span);
    }

    /// Removes all the transitions from this state
    pub fn clear_transitions(&mut self) {
        self.transitions.clear();
    }

    /// Repacks all the transitions from this state to remove overlaps between the transitions' values
    pub fn repack_transitions(&mut self) {
        let mut inverse = HashMap::new();
        for (value, next) in self.transitions.iter() {
            inverse.entry(*next).or_insert_with(Vec::new).push(*value);
        }
        self.transitions.clear();
        for (next, mut values) in inverse.into_iter() {
            values.sort();
            let mut len = values.len();
            let mut i = 0;
            while i < len {
                let mut k1 = values[i];
                let mut j = i + 1;
                while j < len {
                    let k2 = values[j];
                    if k2.begin <= k1.end + 1 {
                        k1 = CharSpan {
                            begin: k1.begin,
                            end: k2.end.max(k1.end)
                        };
                        values[i] = k1;
                        values.remove(j);
                        len -= 1;
                    } else {
                        j += 1;
                    }
                }
                i += 1;
            }
            for value in values.into_iter() {
                self.transitions.insert(value, next);
            }
        }
    }
}

impl PartialEq for DFAState {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for DFAState {}

/// Represents a Deterministic Finite-state Automaton
#[derive(Debug, Clone, Default)]
pub struct DFA {
    /// The list of states in this automaton
    pub states: Vec<DFAState>
}

struct DFAInverse {
    /// The reachable states
    reachable: Vec<usize>,
    /// The inverse graph data
    inverses: HashMap<usize, Vec<usize>>,
    /// The final states
    finals: Vec<usize>
}

/// Represents a group of DFA states within a partition
struct DFAStateGroup<'a> {
    /// The states in this group
    states: Vec<&'a DFAState>
}

/// Represents a partition of a DFA
/// This is used to compute minimal DFAs
struct DFAPartition<'a> {
    /// The groups in this partition
    groups: Vec<DFAStateGroup<'a>>
}

impl DFA {
    /// Initializes this dfa as equivalent to the given nfa
    pub fn from_nfa(mut nfa: NFA) -> DFA {
        // Create the first NFA set, add the entry and close it
        let mut nfa_init = NFAStateSet::new();
        nfa_init.add_unique(nfa.entry);
        nfa_init.close_with_marks(&nfa);
        let mut nfa_sets = vec![nfa_init];
        // Create the initial state for the DFA
        let mut states = vec![DFAState::new(0)];
        // For each set in the list of the NFA states
        let mut i = 0;
        while i < nfa_sets.len() {
            // println!("at state {:?}", &nfa_sets[i].states);
            // normalize transitions
            nfa_sets[i].normalize(&mut nfa);
            // Get the transitions for the set
            let transitions = nfa_sets[i].get_transitions(&nfa);
            // For each transition
            for (value, child_set) in transitions.into_iter() {
                match nfa_sets
                    .iter_mut()
                    .enumerate()
                    .find(|(_, set)| *set == &child_set)
                {
                    Some((index, _)) => {
                        // An existing equivalent set is already present
                        states[i].add_transition(value, index);
                    }
                    None => {
                        // The child is not already present
                        let id = states.len();
                        // Add to the sets list
                        nfa_sets.push(child_set);
                        // Create the corresponding DFA state
                        states.push(DFAState::new(id));
                        // Setup transition
                        states[i].add_transition(value, id);
                    }
                }
            }
            // Add finals
            states[i].add_items(&nfa_sets[i].get_finals(&nfa));
            i += 1;
        }
        DFA { states }
    }

    /// Gets the number of states in this automaton
    pub fn len(&self) -> usize {
        self.states.len()
    }

    /// Gets whether the DFA has no state
    pub fn is_empty(&self) -> bool {
        self.states.is_empty()
    }

    /// Initializes a DFA with a list of existing states
    pub fn from_states(states: Vec<DFAState>) -> DFA {
        DFA { states }
    }

    /// Creates a new state in this DFA
    pub fn add_state(&mut self) -> &mut DFAState {
        let index = self.states.len();
        self.states.push(DFAState::new(index));
        &mut self.states[index]
    }

    /// Repacks the transitions of all the states in this automaton
    pub fn repack_transitions(&mut self) {
        for state in self.states.iter_mut() {
            state.repack_transitions();
        }
    }

    /// Gets a prune version of this DFA
    pub fn prune(&mut self) {
        let inverse = DFAInverse::new(self);
        let mut finals = inverse.close_by_antecedents(&inverse.finals);
        finals.sort();
        if finals.len() == self.states.len() {
            // no change
            return;
        }

        // prune transitions
        for index in finals.iter() {
            let state = &mut self.states[*index];
            state
                .transitions
                .retain(|_, next| match finals.iter().position(|f| f == next) {
                    None => false,
                    Some(new_index) => {
                        // rebind to the new index of the state
                        *next = new_index;
                        true
                    }
                });
        }

        // prune states
        self.states.retain(|s| finals.contains(&s.id));
        // re-label the states
        for (index, state) in self.states.iter_mut().enumerate() {
            state.id = index;
        }
    }

    /// Gets the minimal automaton equivalent to this ine
    pub fn minimize(&self) -> DFA {
        let mut current = DFAPartition::from_dfa(self);
        let mut new_partition = current.refine(self);
        while new_partition.groups.len() != current.groups.len() {
            current = new_partition;
            new_partition = current.refine(self);
        }
        DFA {
            states: current.into_dfa_states(self)
        }
    }

    /// Gets the expected terminals in the DFA
    pub fn get_expected(&self) -> TerminalSet {
        let mut expected = TerminalSet::default();
        expected.add(TerminalRef::Epsilon);
        expected.add(TerminalRef::Dollar);
        for state in self.states.iter() {
            match state.get_final() {
                Some(FinalItem::Terminal(id)) => {
                    expected.add(TerminalRef::Terminal(id));
                }
                _ => {}
            }
        }
        expected.sort();
        expected
    }
}

impl DFAInverse {
    /// Builds this inverse graph from the specified DFA
    fn new(dfa: &DFA) -> DFAInverse {
        let mut reachable = Vec::new();
        let mut inverses = HashMap::new();
        let mut finals = Vec::new();
        reachable.push(0);
        let mut len = 1;
        let mut i = 0;
        while i < len {
            let current = &dfa.states[reachable[i]];
            for next in current.transitions.values() {
                if !reachable.contains(next) {
                    reachable.push(*next);
                    len += 1;
                }
                inverses
                    .entry(*next)
                    .or_insert_with(Vec::new)
                    .push(current.id);
            }
            if current.is_final() {
                finals.push(current.id);
            }
            i += 1;
        }
        DFAInverse {
            reachable,
            inverses,
            finals
        }
    }

    /// Closes the specified list of states through inverse transitions
    fn close_by_antecedents(&self, states: &[usize]) -> Vec<usize> {
        let mut result = Vec::new();
        for state in states.iter() {
            result.push(*state);
        }
        // transitive closure of the final states by their antecedents
        // final states are all reachable
        let mut len = result.len();
        let mut i = 0;
        while i < len {
            let state = result[i];
            if let Some(antecedents) = self.inverses.get(&state) {
                for antecedent in antecedents.iter() {
                    if !result.contains(antecedent) && self.reachable.contains(antecedent) {
                        // this antecedent is reachable and not yet in the closure
                        // => add it to the closure
                        result.push(*antecedent);
                        len += 1;
                    }
                }
            }
            i += 1;
        }
        result
    }
}

impl<'a> DFAStateGroup<'a> {
    /// Initializes this group with a representative state
    fn new(state: &'a DFAState) -> DFAStateGroup {
        DFAStateGroup {
            states: vec![state]
        }
    }
}

impl<'a> DFAPartition<'a> {
    /// Initializes this partition
    fn new() -> DFAPartition<'a> {
        DFAPartition { groups: Vec::new() }
    }

    /// Initializes this partition as the first partition of the given DFA
    /// The first partition is according to final, non-final states
    fn from_dfa(dfa: &'a DFA) -> DFAPartition<'a> {
        let mut groups = Vec::new();
        // Partition the DFA states between final and non-finals
        let mut non_finals: Option<DFAStateGroup<'a>> = None;
        // For each state in the DFA
        for state in dfa.states.iter() {
            if state.is_final() {
                // the state is final
                // Look for a corresponding group in the existing ones
                match groups
                    .iter_mut()
                    .find(|g: &&mut DFAStateGroup| state.same_finals(g.states[0]))
                {
                    None => groups.push(DFAStateGroup::new(state)),
                    Some(ref mut group) => group.states.push(state)
                }
            } else {
                // the state is not final
                if let Some(ref mut group) = &mut non_finals {
                    group.states.push(state);
                } else {
                    non_finals = Some(DFAStateGroup::new(state));
                }
            }
        }
        if let Some(group) = non_finals {
            groups.insert(0, group);
        }
        DFAPartition { groups }
    }

    /// Adds a state to this
    fn add_state(&mut self, dfa: &DFA, state: &'a DFAState, old: &DFAPartition) {
        if let Some(group) = self
            .groups
            .iter_mut()
            .find(|group| old.in_same_group(dfa, group.states[0], state))
        {
            group.states.push(state);
        } else {
            self.groups.push(DFAStateGroup::new(state));
        }
    }

    /// Determines whether two states should be in the same group
    fn in_same_group(&self, dfa: &DFA, s1: &DFAState, s2: &DFAState) -> bool {
        if s1.transitions.len() != s2.transitions.len() {
            return false;
        }
        // For each transition from state 1
        for (key, n1) in s1.transitions.iter() {
            // If state 2 does not have a transition with the same value : not same group
            match s2.transitions.get(key) {
                None => {
                    return false;
                }
                Some(n2) => {
                    // Here s1 and s2 have both a transition of the same value
                    // If the target of these transitions are in the same group in the old partition : same transition
                    let g1 = self.group_of(&dfa.states[*n1]);
                    let g2 = self.group_of(&dfa.states[*n2]);
                    if g1 != g2 {
                        return false;
                    }
                }
            }
        }
        true
    }

    /// Gets the group of the given state in this partition
    fn group_of(&self, state: &DFAState) -> usize {
        self.groups
            .iter()
            .position(|g| g.states.contains(&state))
            .unwrap()
    }

    /// Splits the given partition with this group
    fn refine(&self, dfa: &DFA) -> DFAPartition<'a> {
        let mut new_partition = DFAPartition::new();
        // For each group in the current partition
        // Split the group and add the resulting groups to the new partition
        for group in self.groups.iter() {
            let mut temp_partition = self.split(dfa, group);
            new_partition.groups.append(&mut temp_partition.groups);
        }
        let first_group = new_partition
            .groups
            .iter()
            .position(|g| g.states.iter().any(|s| s.id == 0))
            .unwrap();
        new_partition.groups.swap(0, first_group);
        new_partition
    }

    /// Splits this partition with the group
    fn split(&self, dfa: &DFA, group: &DFAStateGroup<'a>) -> DFAPartition<'a> {
        let mut result = DFAPartition::new();
        for state in group.states.iter() {
            result.add_state(dfa, state, self);
        }
        result
    }

    /// Gets the new dfa states produced by this partition
    fn into_dfa_states(self, dfa: &DFA) -> Vec<DFAState> {
        // For each group in the partition
        // Create the corresponding state and add the finals
        let mut states: Vec<DFAState> = self
            .groups
            .iter()
            .enumerate()
            .map(|(index, group)| {
                let mut state = DFAState::new(index);
                // Add the terminal from the group to the new state
                for item in group.states[0].items.iter() {
                    state.items.push(*item);
                }
                state.items.sort();
                state
            })
            .collect();
        // Do linkage
        for (index, group) in self.groups.iter().enumerate() {
            for (key, next) in group.states[0].transitions.iter() {
                let next_group = self.group_of(&dfa.states[*next]);
                states[index].transitions.insert(*key, next_group);
            }
        }
        states
    }
}

/// Represents a transition in a Non-deterministic Finite Automaton
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct NFATransition {
    /// The value on this transition
    pub value: CharSpan,
    /// The next state by this transition
    pub next: usize
}

/// Represents a state in a Non-deterministic Finite Automaton
#[derive(Debug, Clone)]
pub struct NFAState {
    /// This state's id
    pub id: usize,
    /// The transitions from this state
    pub transitions: Vec<NFATransition>,
    /// List of the items on this state
    pub items: Vec<FinalItem>,
    /// The watermark of this state
    pub mark: i32
}

impl NFAState {
    /// Initializes this state
    pub fn new(id: usize) -> NFAState {
        NFAState {
            id,
            transitions: Vec::new(),
            items: Vec::new(),
            mark: 0
        }
    }

    /// Gets whether this state is final (i.e. it is marked with final items)
    pub fn is_final(&self) -> bool {
        !self.items.is_empty()
    }

    /// Determines if the two states have the same marks
    pub fn same_finals(&self, other: &NFAState) -> bool {
        if self.items.len() != other.items.len() {
            return false;
        }
        self.items.iter().all(|item| other.items.contains(item))
    }

    /// Adds a new item making this state a final state
    pub fn add_item(&mut self, item: FinalItem) {
        if !self.items.contains(&item) {
            self.items.push(item);
        }
    }

    /// Adds new items making this state a final state
    pub fn add_items(&mut self, items: &[FinalItem]) {
        for item in items.iter() {
            self.add_item(*item);
        }
    }

    /// Clears all markers for this states making it non-final
    pub fn clear_items(&mut self) {
        self.items.clear();
    }

    /// Adds a transition from this state to the given state on the given value
    pub fn add_transition(&mut self, value: CharSpan, next: usize) {
        self.transitions.push(NFATransition { value, next });
    }

    /// Removes all transitions starting from this state
    pub fn clear_transitions(&mut self) {
        self.transitions.clear();
    }

    /// Normalize the transitions in this state
    pub fn normalize_self(&mut self) -> bool {
        let mut modified = false;
        let mut i = 0;
        while i < self.transitions.len() {
            let mut j = i + 1;
            while j < self.transitions.len() {
                if self.normalize_split(i, self.transitions[j].value) {
                    modified = true;
                }
                j += 1;
            }
            i += 1;
        }
        modified
    }

    /// Normalize the transitions in this state with another state
    pub fn normalize_with_other(&mut self, others: Vec<NFATransition>) -> bool {
        let mut modified = false;
        let mut i = 0;
        while i < self.transitions.len() {
            for transition in others.iter() {
                if self.normalize_split(i, transition.value) {
                    modified = true;
                }
            }
            i += 1;
        }
        modified
    }

    /// Normalize a specific transition for a specific state in this set
    pub fn normalize_split(&mut self, t: usize, splitter: CharSpan) -> bool {
        let transition = self.transitions[t];
        if transition.value == EPSILON || transition.value == splitter {
            return false;
        }
        let intersection = transition.value.intersect(splitter);
        if intersection == CHARSPAN_INVALID {
            return false;
        }
        let (part1, part2) = transition.value.split(intersection);
        self.transitions[t].value = intersection;
        if part1 != CHARSPAN_INVALID {
            self.add_transition(part1, transition.next);
        }
        if part2 != CHARSPAN_INVALID {
            self.add_transition(part2, transition.next);
        }
        true
    }
}

impl PartialEq for NFAState {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for NFAState {}

/// Represents a Non-deterministic Finite Automaton
#[derive(Debug, Clone)]
pub struct NFA {
    /// The list of all the states in this automaton
    pub states: Vec<NFAState>,
    /// The entry set for this automaton
    pub entry: usize,
    /// The exit state for this automaton
    pub exit: usize
}

impl NFA {
    /// Creates and initializes a minimal automaton with an entry state and a separate exit state
    pub fn new_minimal() -> NFA {
        NFA {
            states: vec![NFAState::new(0), NFAState::new(1)],
            entry: 0,
            exit: 1
        }
    }

    /// Create an optional NFA
    pub fn new_optional(sub: &NFA) -> NFA {
        let mut result = sub.clone();
        result.bind_optional_of(sub.entry, sub.exit);
        result
    }

    /// Create an optional NFA
    pub fn into_optional(self) -> NFA {
        let mut result = self;
        result.bind_optional_of(result.entry, result.exit);
        result
    }

    /// Makes the binding for wrapping a sub NFA into an optional
    fn bind_optional_of(&mut self, sub_entry: usize, sub_exit: usize) {
        let (entry, exit) = self.add_entry_exit();
        self.add_transition(entry, EPSILON, sub_entry);
        self.add_transition(entry, EPSILON, exit);
        self.add_transition(sub_exit, EPSILON, exit);
    }

    /// Creates an automaton that repeats the sub-automaton zero or more times
    pub fn into_zero_or_more(self) -> NFA {
        let mut result = self;
        let sub_entry = result.entry;
        let sub_exit = result.exit;
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, sub_entry);
        result.add_transition(entry, EPSILON, exit);
        result.add_transition(sub_exit, EPSILON, exit);
        result.add_transition(exit, EPSILON, sub_entry);
        result
    }

    /// Creates an automaton that repeats the sub-automaton one or more times
    pub fn into_one_or_more(self) -> NFA {
        let mut result = self;
        let sub_entry = result.entry;
        let sub_exit = result.exit;
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, sub_entry);
        result.add_transition(sub_exit, EPSILON, exit);
        result.add_transition(exit, EPSILON, sub_entry);
        result
    }

    /// Creates an automaton that repeats the sub-automaton a number of times in the given range [min, max]
    pub fn into_repeat_range(self, min: usize, max: usize) -> NFA {
        let mut result = NFA::new_minimal();
        let mut last = 0;
        for _ in 0..min {
            let (sub_entry, sub_exit) = result.insert_sub_nfa(&self);
            result.add_transition(last, EPSILON, sub_entry);
            last = sub_exit;
        }
        let optional = self.into_optional();
        for _ in min..max {
            let (sub_entry, sub_exit) = result.insert_sub_nfa(&optional);
            result.add_transition(last, EPSILON, sub_entry);
            last = sub_exit;
        }
        result.add_transition(last, EPSILON, result.exit);
        if min == 0 {
            result.add_transition(result.entry, EPSILON, result.exit);
        }
        result
    }

    /// Creates an automaton that is the union of the two sub-automaton
    pub fn into_union_with(self, other: NFA) -> NFA {
        let mut result = self;
        let left_entry = result.entry;
        let left_exit = result.exit;
        let (right_entry, right_exit) = result.insert_sub_nfa(&other);
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, left_entry);
        result.add_transition(entry, EPSILON, right_entry);
        result.add_transition(left_exit, EPSILON, exit);
        result.add_transition(right_exit, EPSILON, exit);
        result
    }

    /// Creates an automaton that concatenates the two sub-automaton
    pub fn into_concatenation(self, other: NFA) -> NFA {
        let mut result = self;
        let left_entry = result.entry;
        let left_exit = result.exit;
        let (right_entry, right_exit) = result.insert_sub_nfa(&other);
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, left_entry);
        result.add_transition(left_exit, EPSILON, right_entry);
        result.add_transition(right_exit, EPSILON, exit);
        result
    }

    /// Creates an automaton that is the difference between the left and right sub-automata
    pub fn into_difference(self, other: NFA) -> NFA {
        let mut nfa = self;
        let left_entry = nfa.entry;
        let left_exit = nfa.exit;
        let (right_entry, right_exit) = nfa.insert_sub_nfa(&other);
        let (entry, exit) = nfa.add_entry_exit();
        let exit_positive = nfa.add_state().id;
        let exit_negative = nfa.add_state().id;
        nfa.states[exit_positive].mark = 1;
        nfa.states[exit_negative].mark = -1;
        nfa.add_transition(entry, EPSILON, left_entry);
        nfa.add_transition(entry, EPSILON, right_entry);
        nfa.add_transition(left_exit, EPSILON, exit_positive);
        nfa.add_transition(right_exit, EPSILON, exit_negative);
        nfa.add_transition(exit_positive, EPSILON, exit);
        nfa.states[exit].items.push(FinalItem::Dummy);

        let mut dfa = DFA::from_nfa(nfa);
        dfa.prune();
        let mut nfa = NFA::from_dfa(&dfa);
        nfa.exit = nfa.add_state().id;
        for state in nfa.states.iter_mut() {
            if state.items.contains(&FinalItem::Dummy) {
                state.clear_items();
                state.add_transition(EPSILON, nfa.exit);
            }
        }
        nfa
    }

    /// Adds entry and exit states
    fn add_entry_exit(&mut self) -> (usize, usize) {
        self.entry = self.add_state().id;
        self.exit = self.add_state().id;
        (self.entry, self.exit)
    }

    /// Initializes this automaton as a copy of the given DFA
    /// This automaton will not have an exit state
    pub fn from_dfa(dfa: &DFA) -> NFA {
        NFA {
            states: dfa
                .states
                .iter()
                .map(|state| NFAState {
                    id: state.id,
                    items: state.items.clone(),
                    transitions: state
                        .transitions
                        .iter()
                        .map(|(value, next)| NFATransition {
                            value: *value,
                            next: *next
                        })
                        .collect(),
                    mark: 0
                })
                .collect(),
            entry: 0,
            exit: std::usize::MAX
        }
    }

    /// Gets the number of states in this automaton
    pub fn len(&self) -> usize {
        self.states.len()
    }

    /// Gets whether the NFA has no state
    pub fn is_empty(&self) -> bool {
        self.states.is_empty()
    }

    /// Creates a new state in this DFA
    pub fn add_state(&mut self) -> &mut NFAState {
        let index = self.states.len();
        self.states.push(NFAState::new(index));
        &mut self.states[index]
    }

    /// Adds a transition to the NFA
    pub fn add_transition(&mut self, from: usize, value: CharSpan, to: usize) {
        self.states[from].add_transition(value, to);
    }

    /// Clone this automaton without the final items
    pub fn clone_no_finals(&self) -> NFA {
        NFA {
            states: self
                .states
                .iter()
                .map(|state| NFAState {
                    id: state.id,
                    items: Vec::new(),
                    transitions: state.transitions.clone(),
                    mark: 0
                })
                .collect(),
            entry: self.entry,
            exit: self.exit
        }
    }

    /// Inserts all the states of the given automaton into this one
    pub fn insert_sub_nfa(&mut self, nfa: &NFA) -> (usize, usize) {
        let offset = self.states.len();
        for state in nfa.states.iter() {
            self.states.push(NFAState {
                id: state.id + offset,
                items: state.items.clone(),
                mark: state.mark,
                transitions: state
                    .transitions
                    .iter()
                    .map(|t| NFATransition {
                        value: t.value,
                        next: t.next + offset
                    })
                    .collect()
            });
        }
        (nfa.entry + offset, nfa.exit + offset)
    }
}

/// Represents a set of states in a Non-deterministic Finite Automaton
/// A state can only appear once in a set
struct NFAStateSet {
    /// The backend storage for the states in this set
    states: Vec<usize>
}

impl NFAStateSet {
    /// Initializes this set
    fn new() -> NFAStateSet {
        NFAStateSet { states: Vec::new() }
    }

    /// Adds the given state in this set if it is not already present
    fn add_unique(&mut self, state: usize) {
        if !self.states.contains(&state) {
            self.states.push(state);
        }
    }

    /// Closes this set by transitively adding to it all reachable state by the epsilon transition
    fn close_normal(&mut self, nfa: &NFA) {
        let mut i = 0;
        while i < self.states.len() {
            for transition in nfa.states[self.states[i]].transitions.iter() {
                if transition.value == EPSILON {
                    self.add_unique(nfa.states[transition.next].id);
                }
            }
            i += 1;
        }
    }

    /// Closes this set by transitively adding to it all reachable state by the epsilon transition
    /// This looks for the watermark of states
    fn close_with_marks(&mut self, nfa: &NFA) {
        // Close the set
        self.close_normal(nfa);
        // Look for a positive and a negative node
        let mut state_positive = None;
        let mut state_negative = None;
        for state in self.states.iter() {
            match nfa.states[*state].mark.cmp(&0) {
                Ordering::Greater => state_positive = Some(*state),
                Ordering::Less => state_negative = Some(*state),
                _ => ()
            }
        }
        // With both negative and positive states
        // remove the states immediately reached with epsilon from the positive state
        if let (Some(state_positive), Some(_)) = (state_positive, state_negative) {
            self.states.retain(|s| {
                nfa.states[state_positive]
                    .transitions
                    .iter()
                    .all(|t| t.next != *s)
            });
        }
    }

    /// Gets all the final markers of all the states in this set
    fn get_finals(&self, nfa: &NFA) -> Vec<FinalItem> {
        let mut result = Vec::new();
        for state in self.states.iter() {
            for fi in nfa.states[*state].items.iter() {
                result.push(*fi);
            }
        }
        result
    }

    /// Builds transitions from this set to other sets
    fn get_transitions(&self, nfa: &NFA) -> HashMap<CharSpan, NFAStateSet> {
        let mut transitions = HashMap::new();
        for state in self.states.iter() {
            for transition in nfa.states[*state].transitions.iter() {
                if transition.value == EPSILON {
                    // If this is an ε-transition : pass
                    continue;
                }
                // Add the transition's target to set's transitions dictionnary
                transitions
                    .entry(transition.value)
                    .or_insert_with(NFAStateSet::new)
                    .add_unique(nfa.states[transition.next].id);
            }
        }
        // Close all children
        for (_, set) in transitions.iter_mut() {
            set.close_with_marks(nfa);
        }
        transitions
    }

    /// Normalize this set
    fn normalize(&self, nfa: &mut NFA) {
        while self.normalize_once(nfa) {}
    }

    /// Normalize this set
    fn normalize_once(&self, nfa: &mut NFA) -> bool {
        let mut modified = false;
        let nb_states = self.states.len();
        let mut s1 = 0;
        while s1 < nb_states {
            if nfa.states[self.states[s1]].normalize_self() {
                modified = true;
            }
            let mut s2 = s1 + 1;
            while s2 < nb_states {
                let transitions = nfa.states[self.states[s2]].transitions.clone();
                if nfa.states[self.states[s1]].normalize_with_other(transitions) {
                    modified = true;
                }
                let transitions = nfa.states[self.states[s1]].transitions.clone();
                if nfa.states[self.states[s2]].normalize_with_other(transitions) {
                    modified = true;
                }
                s2 += 1;
            }
            s1 += 1;
        }
        modified
    }
}

impl PartialEq for NFAStateSet {
    fn eq(&self, other: &Self) -> bool {
        if self.states.len() != other.states.len() {
            return false;
        }
        self.states.iter().all(|s| other.states.contains(s))
    }
}

impl Eq for NFAStateSet {}
