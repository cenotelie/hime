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

use std::cmp::Ordering;
use std::collections::HashMap;

use crate::grammars::{TerminalRef, TerminalSet};
use crate::{CharSpan, CHARSPAN_INVALID};

/// Represents the value epsilon on NFA transitions
pub const EPSILON: CharSpan = CHARSPAN_INVALID;

/// Represents a marker for the final state of an automaton
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum FinalItem {
    /// Represents a fake marker of a final state in an automaton
    Dummy,
    /// A terminal symbol in a grammar
    Terminal(usize, usize),
}

impl FinalItem {
    /// Gets the sid for the referenced item
    ///
    /// # Panics
    ///
    /// Panic when the final item does not reference a terminal
    #[must_use]
    pub fn sid(self) -> usize {
        if let FinalItem::Terminal(id, _) = self {
            id
        } else {
            panic!("This final item does not reference a terminal")
        }
    }

    /// Gets the priority of this item
    #[must_use]
    pub fn priority(self) -> usize {
        match self {
            FinalItem::Dummy => 0,
            FinalItem::Terminal(id, _) => id,
        }
    }

    /// Get the final item from the terminal reference
    ///
    /// # Panics
    ///
    /// Panic when the terminal cannot be turned into a DFA final item
    #[must_use]
    pub fn from(terminal: TerminalRef, context: usize) -> FinalItem {
        match terminal {
            TerminalRef::Dummy => FinalItem::Dummy,
            TerminalRef::Terminal(id) => FinalItem::Terminal(id, context),
            _ => panic!("Cannot turn this terminal into a DFA final item"),
        }
    }
}

impl Ord for FinalItem {
    fn cmp(&self, other: &FinalItem) -> Ordering {
        other.priority().cmp(&self.priority())
    }
}

impl PartialOrd for FinalItem {
    fn partial_cmp(&self, other: &FinalItem) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl From<FinalItem> for TerminalRef {
    fn from(final_item: FinalItem) -> Self {
        match final_item {
            FinalItem::Dummy => TerminalRef::Dummy,
            FinalItem::Terminal(id, _) => TerminalRef::Terminal(id),
        }
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
    pub items: Vec<FinalItem>,
}

impl DFAState {
    /// Initializes this state
    #[must_use]
    pub fn new(id: usize) -> DFAState {
        DFAState {
            id,
            transitions: HashMap::new(),
            items: Vec::new(),
        }
    }

    /// Gets whether this state is final (i.e. it is marked with final items)
    #[must_use]
    pub fn is_final(&self) -> bool {
        !self.items.is_empty()
    }

    /// Determines if the two states have the same marks
    #[must_use]
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
        for item in items {
            self.do_add_item(*item);
        }
        self.items.sort();
    }

    /// Clears all markers for this states making it non-final
    pub fn clear_items(&mut self) {
        self.items.clear();
    }

    /// Gets the child state by the specified transition
    #[must_use]
    pub fn get_child_by(&self, value: CharSpan) -> Option<usize> {
        self.transitions.get(&value).copied()
    }

    /// Determines whether this state has the specified transition
    #[must_use]
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
        for (value, next) in &self.transitions {
            inverse.entry(*next).or_insert_with(Vec::new).push(*value);
        }
        self.transitions.clear();
        for (next, mut values) in inverse {
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
                            end: k2.end.max(k1.end),
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
            for value in values {
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
    pub states: Vec<DFAState>,
}

struct DFAInverse {
    /// The reachable states
    reachable: Vec<usize>,
    /// The inverse graph data
    inverses: HashMap<usize, Vec<usize>>,
    /// The final states
    finals: Vec<usize>,
}

/// Represents a group of DFA states within a partition
struct DFAStateGroup<'a> {
    /// The states in this group
    states: Vec<&'a DFAState>,
}

/// Represents a partition of a DFA
/// This is used to compute minimal DFAs
struct DFAPartition<'a> {
    /// The groups in this partition
    groups: Vec<DFAStateGroup<'a>>,
}

impl DFA {
    /// Initializes this dfa as equivalent to the given nfa
    #[must_use]
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
            // normalize transitions
            nfa_sets[i].normalize(&mut nfa);
            // Get the transitions for the set
            let transitions = nfa_sets[i].get_transitions(&nfa);
            // For each transition
            for (value, child_set) in transitions {
                if let Some((index, _)) = nfa_sets.iter_mut().enumerate().find(|(_, set)| *set == &child_set) {
                    // An existing equivalent set is already present
                    states[i].add_transition(value, index);
                } else {
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
            // Add finals
            states[i].add_items(&nfa_sets[i].get_finals(&nfa));
            i += 1;
        }
        DFA { states }
    }

    /// Gets the number of states in this automaton
    #[must_use]
    pub fn len(&self) -> usize {
        self.states.len()
    }

    /// Gets whether the DFA has no state
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.states.is_empty()
    }

    /// Initializes a DFA with a list of existing states
    #[must_use]
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
        for state in &mut self.states {
            state.repack_transitions();
        }
    }

    /// Gets a prune version of this DFA
    pub fn prune(&mut self) {
        let inverse = DFAInverse::new(self);
        let mut finals = inverse.close_by_antecedents(&inverse.finals);
        finals.sort_unstable();
        if finals.len() == self.states.len() {
            // no change
            return;
        }

        // prune transitions
        for index in &finals {
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
    #[must_use]
    pub fn minimize(&self) -> DFA {
        let mut current = DFAPartition::from_dfa(self);
        let mut new_partition = current.refine(self);
        while new_partition.groups.len() != current.groups.len() {
            current = new_partition;
            new_partition = current.refine(self);
        }
        DFA {
            states: current.into_dfa_states(self),
        }
    }

    /// Gets the expected terminals in the DFA
    #[must_use]
    pub fn get_expected(&self) -> TerminalSet {
        let mut expected = TerminalSet::default();
        expected.add(TerminalRef::Epsilon);
        expected.add(TerminalRef::Dollar);
        for state in &self.states {
            let mut contexts = Vec::new();
            for item in &state.items {
                if let FinalItem::Terminal(id, context) = item {
                    if !contexts.contains(context) {
                        contexts.push(*context);
                        expected.add(TerminalRef::Terminal(*id));
                    }
                }
            }
        }
        expected.sort();
        expected
    }

    /// Gets all the terminals that override the given one in final states
    #[must_use]
    pub fn get_overriders(&self, terminal: TerminalRef, context: usize) -> Vec<TerminalRef> {
        let mut overriders = TerminalSet::default();
        let terminal_final = FinalItem::from(terminal, context);
        for state in &self.states {
            if state.items.contains(&terminal_final) {
                // separator is final of this state
                for item in &state.items {
                    if item == &terminal_final {
                        break;
                    }
                    if let FinalItem::Terminal(id, c) = item {
                        if *c == context {
                            // this final item has more priority than the separator
                            overriders.add(TerminalRef::Terminal(*id));
                        }
                    }
                }
            }
        }
        overriders.content
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
                inverses.entry(*next).or_insert_with(Vec::new).push(current.id);
            }
            if current.is_final() {
                finals.push(current.id);
            }
            i += 1;
        }
        DFAInverse {
            reachable,
            inverses,
            finals,
        }
    }

    /// Closes the specified list of states through inverse transitions
    fn close_by_antecedents(&self, states: &[usize]) -> Vec<usize> {
        let mut result = Vec::new();
        for state in states {
            result.push(*state);
        }
        // transitive closure of the final states by their antecedents
        // final states are all reachable
        let mut len = result.len();
        let mut i = 0;
        while i < len {
            let state = result[i];
            if let Some(antecedents) = self.inverses.get(&state) {
                for antecedent in antecedents {
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
        DFAStateGroup { states: vec![state] }
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
        for state in &dfa.states {
            if state.is_final() {
                // the state is final
                // Look for a corresponding group in the existing ones
                match groups
                    .iter_mut()
                    .find(|g: &&mut DFAStateGroup| state.same_finals(g.states[0]))
                {
                    None => groups.push(DFAStateGroup::new(state)),
                    Some(group) => group.states.push(state),
                }
            } else {
                // the state is not final
                if let Some(group) = non_finals.as_mut() {
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
        for (key, n1) in &s1.transitions {
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
        self.groups.iter().position(|g| g.states.contains(&state)).unwrap()
    }

    /// Splits the given partition with this group
    fn refine(&self, dfa: &DFA) -> DFAPartition<'a> {
        let mut new_partition = DFAPartition::new();
        // For each group in the current partition
        // Split the group and add the resulting groups to the new partition
        for group in &self.groups {
            let mut temp_partition = self.split(dfa, group);
            new_partition.groups.append(&mut temp_partition.groups);
        }
        new_partition
    }

    /// Splits this partition with the group
    fn split(&self, dfa: &DFA, group: &DFAStateGroup<'a>) -> DFAPartition<'a> {
        let mut result = DFAPartition::new();
        for state in &group.states {
            result.add_state(dfa, state, self);
        }
        result
    }

    /// Gets the new dfa states produced by this partition
    fn into_dfa_states(mut self, dfa: &DFA) -> Vec<DFAState> {
        // move froup with state 0 as first group
        let first_group = self.groups.iter().position(|g| g.states.iter().any(|s| s.id == 0)).unwrap();
        self.groups.swap(0, first_group);
        // For each group in the partition
        // Create the corresponding state and add the finals
        let mut states: Vec<DFAState> = self
            .groups
            .iter()
            .enumerate()
            .map(|(index, group)| {
                let mut state = DFAState::new(index);
                // Add the terminal from the group to the new state
                for item in &group.states[0].items {
                    state.items.push(*item);
                }
                state.items.sort();
                state
            })
            .collect();
        // Do linkage
        for (index, group) in self.groups.iter().enumerate() {
            for (key, next) in &group.states[0].transitions {
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
    pub next: usize,
}

/// An effect for a bound for a set of NFA transitions
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum NFATransitionBoundEffect {
    /// Starts a range of transition to a next state
    Start { tid: usize, next: usize },
    /// Ends a range of transition to a next state
    End { tid: usize },
    /// Start of a range for another state
    OtherStart,
    /// End of a range for another state
    OtherEnd,
}

/// A bound for NFA transitions
#[derive(Debug, Clone)]
pub struct NFATransitionBound {
    /// The character value, included
    value: u16,
    /// The effects on the bounds
    effects: Vec<NFATransitionBoundEffect>,
}

impl NFATransitionBound {
    /// Counts the number of starts and ends in the effects    
    #[must_use]
    pub fn count_starts_ends(&self) -> (usize, usize) {
        self.effects.iter().fold((0, 0), |(starts, ends), effect| match effect {
            NFATransitionBoundEffect::Start { tid: _, next: _ } | NFATransitionBoundEffect::OtherStart => (starts + 1, ends),
            NFATransitionBoundEffect::End { tid: _ } | NFATransitionBoundEffect::OtherEnd => (starts, ends + 1),
        })
    }
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
    pub mark: i32,
}

impl NFAState {
    /// Initializes this state
    #[must_use]
    pub fn new(id: usize) -> NFAState {
        NFAState {
            id,
            transitions: Vec::new(),
            items: Vec::new(),
            mark: 0,
        }
    }

    /// Gets whether this state is final (i.e. it is marked with final items)
    #[must_use]
    pub fn is_final(&self) -> bool {
        !self.items.is_empty()
    }

    /// Determines if the two states have the same marks
    #[must_use]
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
        for item in items {
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

    /// Fill a mpa of bounds with data from transitions from this state
    fn fill_bounds_map(&self, map: &mut Vec<NFATransitionBound>) {
        for transition in &self.transitions {
            if transition.value != EPSILON {
                Self::insert_sorted_in(map, transition.value.begin, NFATransitionBoundEffect::OtherStart);
                Self::insert_sorted_in(map, transition.value.end, NFATransitionBoundEffect::OtherEnd);
            }
        }
    }

    /// Insert a bound in a sorted map of transition bounds
    fn insert_sorted_in(map: &mut Vec<NFATransitionBound>, value: u16, effect: NFATransitionBoundEffect) {
        let mut index = 0;
        while index < map.len() {
            if map[index].value == value {
                map[index].effects.push(effect);
                return;
            }
            if map[index].value > value {
                map.insert(
                    index,
                    NFATransitionBound {
                        value,
                        effects: vec![effect],
                    },
                );
                return;
            }
            index += 1;
        }
        map.push(NFATransitionBound {
            value,
            effects: vec![effect],
        });
    }

    /// Normalize the transitions in this state
    fn normalize(&mut self, map: &[NFATransitionBound]) {
        let mut map = map.to_vec();
        let mut transitions = Vec::new();
        for (index, transition) in self.transitions.iter().enumerate() {
            if transition.value == EPSILON || transition.value.len() == 1 {
                transitions.push(*transition);
            } else {
                Self::insert_sorted_in(
                    &mut map,
                    transition.value.begin,
                    NFATransitionBoundEffect::Start {
                        tid: index,
                        next: transition.next,
                    },
                );
                Self::insert_sorted_in(&mut map, transition.value.end, NFATransitionBoundEffect::End { tid: index });
            }
        }
        let mut current_start = 0;
        let mut current_nexts = Vec::new();
        for bound in map {
            let (starts, ends) = bound.count_starts_ends();

            // end all ongoing ranges
            for &(_tid, next) in &current_nexts {
                if starts == 0 || current_start != bound.value {
                    transitions.push(NFATransition {
                        value: CharSpan::new(current_start, if starts == 0 { bound.value } else { bound.value - 1 }),
                        next,
                    });
                }
            }
            let ongoings = current_nexts.iter().map(|&(tid, _)| tid).collect::<Vec<_>>();

            // apply effects
            for effect in bound.effects {
                match effect {
                    NFATransitionBoundEffect::OtherStart | NFATransitionBoundEffect::OtherEnd => {}
                    NFATransitionBoundEffect::Start { tid, next } => {
                        current_nexts.push((tid, next));
                        if ends > 0 {
                            // there are ends, we will re-start from +1
                            // add a single transition
                            transitions.push(NFATransition {
                                value: CharSpan::new(bound.value, bound.value),
                                next,
                            });
                        }
                    }
                    NFATransitionBoundEffect::End { tid } => {
                        let index = current_nexts.iter().position(|&(id, _next)| id == tid).unwrap();
                        let (_tid, next) = current_nexts.swap_remove(index);
                        if starts > 0 {
                            // there are starts, we ends ongoing before
                            // add a single transition
                            transitions.push(NFATransition {
                                value: CharSpan::new(bound.value, bound.value),
                                next,
                            });
                        }
                    }
                }
            }

            // all surviving spans, use this bound
            if starts > 0 && ends > 0 {
                for &(tid, next) in &current_nexts {
                    if ongoings.contains(&tid) {
                        transitions.push(NFATransition {
                            value: CharSpan::new(bound.value, bound.value),
                            next,
                        });
                    }
                }
            }

            // starts from this bounds if all starts (no ends) effect, else use the next
            current_start = if ends == 0 || bound.value == u16::MAX {
                bound.value
            } else {
                bound.value + 1
            };
        }

        if cfg!(debug_assertions) {
            Self::assert_equivalent_transitions(&self.transitions, &transitions);
        }
        self.transitions = transitions;
        assert!(current_nexts.is_empty());
    }

    /// Gets all the possible nexts given transactions and an input
    #[must_use]
    fn get_nexts_by(transitions: &[NFATransition], c: u16) -> Vec<usize> {
        transitions
            .iter()
            .filter_map(|transition| {
                if transition.value.contains(c) {
                    Some(transition.next)
                } else {
                    None
                }
            })
            .collect()
    }

    /// Checks that two sets of transitions are equivalent
    fn assert_equivalent_transitions(left: &[NFATransition], right: &[NFATransition]) {
        for left in left {
            if left.value == EPSILON {
                assert!(
                    right.iter().any(|r| r.value == EPSILON && r.next == left.next),
                    "missing transition on EPSILON to {}",
                    left.next
                );
            } else {
                for v in left.value.begin..=left.value.end {
                    assert!(
                        Self::get_nexts_by(right, v).contains(&left.next),
                        "missing transition on {} to {}",
                        v,
                        left.next
                    );
                }
            }
        }

        for right in right {
            if right.value == EPSILON {
                assert!(
                    left.iter().any(|l| l.value == EPSILON && l.next == right.next),
                    "spurious on EPSILON to {}",
                    right.next
                );
            } else {
                for v in right.value.begin..=right.value.end {
                    assert!(
                        Self::get_nexts_by(left, v).contains(&right.next),
                        "spurious transition on {} to {}",
                        v,
                        right.next
                    );
                }
            }
        }
    }
}

impl PartialEq for NFAState {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for NFAState {}

#[cfg(test)]
mod tests_nfa_normalize {
    use super::{NFAState, NFATransition};
    use crate::finite::{NFATransitionBound, NFATransitionBoundEffect};
    use crate::CharSpan;

    #[test]
    fn test_no_overlap_start_0() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        state.add_transition(CharSpan::new(11, 20), 2);
        state.add_transition(CharSpan::new(30, 40), 3);
        let mut map = Vec::new();
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 10, 11, 20, 30, 40]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(11, 20)
                },
                NFATransition {
                    next: 3,
                    value: CharSpan::new(30, 40)
                },
            ]
        );
    }

    #[test]
    fn test_no_overlap_start_1() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(1, 10), 1);
        state.add_transition(CharSpan::new(11, 20), 2);
        state.add_transition(CharSpan::new(30, 40), 3);
        let mut map = Vec::new();
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![1, 10, 11, 20, 30, 40]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(1, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(11, 20)
                },
                NFATransition {
                    next: 3,
                    value: CharSpan::new(30, 40)
                },
            ]
        );
    }

    #[test]
    fn test_no_overlap_ends_max() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        state.add_transition(CharSpan::new(11, 20), 2);
        state.add_transition(CharSpan::new(30, 0xFFFF), 3);
        let mut map = Vec::new();
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 10, 11, 20, 30, 0xFFFF]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(11, 20)
                },
                NFATransition {
                    next: 3,
                    value: CharSpan::new(30, 0xFFFF)
                },
            ]
        );
    }

    #[test]
    fn test_overlap_1() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        state.add_transition(CharSpan::new(5, 15), 2);
        let mut map = Vec::new();
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 5, 10, 15]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 4)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(5, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(5, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(11, 15)
                },
            ]
        );
    }

    #[test]
    fn test_overlap_1_other() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        let mut map = vec![
            NFATransitionBound {
                value: 5,
                effects: vec![NFATransitionBoundEffect::OtherStart],
            },
            NFATransitionBound {
                value: 15,
                effects: vec![NFATransitionBoundEffect::OtherEnd],
            },
        ];
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 5, 10, 15]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 4)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(5, 10)
                },
            ]
        );
    }

    #[test]
    fn test_overlap_start_end_bound() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        state.add_transition(CharSpan::new(10, 15), 2);
        let mut map = Vec::new();
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 10, 15]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 9)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(10, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(10, 10)
                },
                NFATransition {
                    next: 2,
                    value: CharSpan::new(11, 15)
                },
            ]
        );
    }

    #[test]
    fn test_overlap_start_end_bound_other() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        let mut map = vec![
            NFATransitionBound {
                value: 10,
                effects: vec![NFATransitionBoundEffect::OtherStart],
            },
            NFATransitionBound {
                value: 15,
                effects: vec![NFATransitionBoundEffect::OtherEnd],
            },
        ];
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 10, 15]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 9)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(10, 10)
                },
            ]
        );
    }

    #[test]
    fn test_overlap_start_end_bound_other_2() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        let mut map = vec![NFATransitionBound {
            value: 5,
            effects: vec![NFATransitionBoundEffect::OtherStart, NFATransitionBoundEffect::OtherEnd],
        }];
        state.fill_bounds_map(&mut map);
        let bounds = map.iter().map(|b| b.value).collect::<Vec<_>>();
        assert_eq!(bounds, vec![0, 5, 10]);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 4)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(5, 5)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(6, 10)
                },
            ]
        );
    }

    #[test]
    fn test_no_overlap_consecutive() {
        let mut state = NFAState::new(0);
        state.add_transition(CharSpan::new(0, 10), 1);
        // other ranges are: 2-3, 3-4, 4-5
        let mut map = vec![
            NFATransitionBound {
                value: 2,
                effects: vec![NFATransitionBoundEffect::OtherStart],
            },
            NFATransitionBound {
                value: 3,
                effects: vec![NFATransitionBoundEffect::OtherStart, NFATransitionBoundEffect::OtherEnd],
            },
            NFATransitionBound {
                value: 4,
                effects: vec![NFATransitionBoundEffect::OtherStart, NFATransitionBoundEffect::OtherEnd],
            },
            NFATransitionBound {
                value: 5,
                effects: vec![NFATransitionBoundEffect::OtherEnd],
            },
        ];
        state.fill_bounds_map(&mut map);
        state.normalize(&map);
        assert_eq!(
            state.transitions,
            vec![
                NFATransition {
                    next: 1,
                    value: CharSpan::new(0, 1)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(2, 2)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(3, 3)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(4, 4)
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(5, 5),
                },
                NFATransition {
                    next: 1,
                    value: CharSpan::new(6, 10)
                },
            ]
        );
    }
}

/// Represents a Non-deterministic Finite Automaton
#[derive(Debug, Clone)]
pub struct NFA {
    /// The list of all the states in this automaton
    pub states: Vec<NFAState>,
    /// The entry set for this automaton
    pub entry: usize,
    /// The exit state for this automaton
    pub exit: usize,
}

impl NFA {
    /// Creates and initializes a minimal automaton with an entry state and a separate exit state
    #[must_use]
    pub fn new_minimal() -> NFA {
        NFA {
            states: vec![NFAState::new(0), NFAState::new(1)],
            entry: 0,
            exit: 1,
        }
    }

    /// Create an optional NFA
    #[must_use]
    pub fn new_optional(sub: &NFA) -> NFA {
        let mut result = sub.clone();
        result.bind_optional_of(sub.entry, sub.exit);
        result
    }

    /// Create an optional NFA
    #[must_use]
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
    #[must_use]
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
    #[must_use]
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
    #[must_use]
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
    #[must_use]
    pub fn into_union_with(self, other: &NFA) -> NFA {
        let mut result = self;
        let left_entry = result.entry;
        let left_exit = result.exit;
        let (right_entry, right_exit) = result.insert_sub_nfa(other);
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, left_entry);
        result.add_transition(entry, EPSILON, right_entry);
        result.add_transition(left_exit, EPSILON, exit);
        result.add_transition(right_exit, EPSILON, exit);
        result
    }

    /// Creates an automaton that concatenates the two sub-automaton
    #[must_use]
    pub fn into_concatenation(self, other: &NFA) -> NFA {
        let mut result = self;
        let left_entry = result.entry;
        let left_exit = result.exit;
        let (right_entry, right_exit) = result.insert_sub_nfa(other);
        let (entry, exit) = result.add_entry_exit();
        result.add_transition(entry, EPSILON, left_entry);
        result.add_transition(left_exit, EPSILON, right_entry);
        result.add_transition(right_exit, EPSILON, exit);
        result
    }

    /// Creates an automaton that is the difference between the left and right sub-automata
    #[allow(clippy::similar_names)]
    #[must_use]
    pub fn into_difference(self, other: &NFA) -> NFA {
        let mut nfa = self;
        let left_entry = nfa.entry;
        let left_exit = nfa.exit;
        let (right_entry, right_exit) = nfa.insert_sub_nfa(other);
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
        for state in &mut nfa.states {
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
    #[must_use]
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
                            next: *next,
                        })
                        .collect(),
                    mark: 0,
                })
                .collect(),
            entry: 0,
            exit: usize::MAX,
        }
    }

    /// Gets the number of states in this automaton
    #[must_use]
    pub fn len(&self) -> usize {
        self.states.len()
    }

    /// Gets whether the NFA has no state
    #[must_use]
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
    #[must_use]
    pub fn clone_no_finals(&self) -> NFA {
        NFA {
            states: self
                .states
                .iter()
                .map(|state| NFAState {
                    id: state.id,
                    items: Vec::new(),
                    transitions: state.transitions.clone(),
                    mark: 0,
                })
                .collect(),
            entry: self.entry,
            exit: self.exit,
        }
    }

    /// Inserts all the states of the given automaton into this one
    pub fn insert_sub_nfa(&mut self, nfa: &NFA) -> (usize, usize) {
        let offset = self.states.len();
        for state in &nfa.states {
            self.states.push(NFAState {
                id: state.id + offset,
                items: state.items.clone(),
                mark: state.mark,
                transitions: state
                    .transitions
                    .iter()
                    .map(|t| NFATransition {
                        value: t.value,
                        next: t.next + offset,
                    })
                    .collect(),
            });
        }
        (nfa.entry + offset, nfa.exit + offset)
    }
}

/// Represents a set of states in a Non-deterministic Finite Automaton
/// A state can only appear once in a set
struct NFAStateSet {
    /// The backend storage for the states in this set
    states: Vec<usize>,
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
            for transition in &nfa.states[self.states[i]].transitions {
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
        for state in &self.states {
            match nfa.states[*state].mark.cmp(&0) {
                Ordering::Greater => state_positive = Some(*state),
                Ordering::Less => state_negative = Some(*state),
                Ordering::Equal => (),
            }
        }
        // With both negative and positive states
        // remove the states immediately reached with epsilon from the positive state
        if let (Some(state_positive), Some(_)) = (state_positive, state_negative) {
            self.states.retain(|s| {
                // all transitions from the positive state are
                // not to s with an EPSION
                nfa.states[state_positive]
                    .transitions
                    .iter()
                    .all(|t| t.value != EPSILON || t.next != *s)
            });
        }
    }

    /// Gets all the final markers of all the states in this set
    fn get_finals(&self, nfa: &NFA) -> Vec<FinalItem> {
        let mut result = Vec::new();
        for state in &self.states {
            for fi in &nfa.states[*state].items {
                result.push(*fi);
            }
        }
        result
    }

    /// Builds transitions from this set to other sets
    fn get_transitions(&self, nfa: &NFA) -> Vec<(CharSpan, NFAStateSet)> {
        let mut transitions = HashMap::new();
        for state in &self.states {
            for transition in &nfa.states[*state].transitions {
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
        for set in transitions.values_mut() {
            set.close_with_marks(nfa);
        }
        let mut transitions: Vec<(CharSpan, NFAStateSet)> = transitions.into_iter().collect();
        transitions.sort_by_key(|(span, _)| span.begin);
        transitions
    }

    /// Normalize this set so that no transition overlap with another from the set
    fn normalize(&self, nfa: &mut NFA) {
        let mut map = Vec::new();
        for &index in &self.states {
            nfa.states[index].fill_bounds_map(&mut map);
        }
        for &index in &self.states {
            nfa.states[index].normalize(&map);
        }
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
