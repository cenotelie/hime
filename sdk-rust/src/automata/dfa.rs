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

//! Module for the DFA automata

use super::FinalItem;
use super::StateId;
use super::Transition;
use super::nfa::NFA;
use super::nfa::NFAStateSet;
use super::super::CharSpan;
use std::collections::HashMap;

/// Represents a state in a Deterministic Finite Automaton
#[derive(Clone)]
pub struct DFAState {
    /// This state's id
    pub id: StateId,
    /// List of the items on this state
    pub items: Vec<FinalItem>,
    /// The transitions from this state
    pub transitions: Vec<Transition>
}

impl PartialEq for DFAState {
    fn eq(&self, other: &DFAState) -> bool {
        self.id == other.id
    }
}

impl Eq for DFAState {}

impl DFAState {
    /// Initialize this state
    pub fn new(id: StateId) -> DFAState {
        DFAState {
            id,
            items: Vec::<FinalItem>::new(),
            transitions: Vec::<Transition>::new()
        }
    }

    /// Gets whether this is a final state
    pub fn is_final(&self) -> bool {
        !self.items.is_empty()
    }

    /// Gets whether this is the entry state
    pub fn is_entry(&self) -> bool {
        self.id == 0
    }

    /// Checks whether this state has the same final items as the specified one
    pub fn same_items(&self, state: &DFAState) -> bool {
        if self.items.len() != state.items.len() {
            return false;
        }
        for item in self.items.iter() {
            let mut found = false;
            for item2 in state.items.iter() {
                if ((*item) as *const FinalItem) == ((*item2) as *const FinalItem) {
                    found = true;
                }
            }
            if !found {
                return false;
            }
        }
        true
    }

    /// Adds to this state the same items as the specified one
    pub fn add_items(&mut self, state: &DFAState) {
        for item in state.items.iter() {
            self.items.push(*item);
        }
    }

    /// Determines whether this state has the specified transition
    pub fn has_transition(&self, key: CharSpan) -> bool {
        for transition in self.transitions.iter() {
            if transition.key == key {
                return true;
            }
        }
        false
    }

    /// Gets the identifier of the state by a transition
    pub fn get_next_by(&self, key: CharSpan) -> Option<StateId> {
        for transition in self.transitions.iter() {
            if transition.key == key {
                return Some(transition.next);
            }
        }
        None
    }

    /// Repacks all the transitions from this state to remove
    /// overlaps between the transitions' values
    pub fn repack_transitions(&mut self) {
        let mut inverse: HashMap<StateId, Vec<CharSpan>> = HashMap::new();
        for transition in self.transitions.iter() {
            if !inverse.contains_key(&transition.next) {
                inverse.insert(transition.next, Vec::<CharSpan>::new());
            }
            inverse
                .get_mut(&transition.next)
                .unwrap()
                .push(transition.key);
        }
        self.transitions.clear();
        for (child, keys) in inverse.iter_mut() {
            keys.sort();
            for i in 0..keys.len() {
                let mut k1 = keys[i];
                let mut j = i;
                while j < keys.len() {
                    let k2 = keys[j];
                    if k2.begin == k1.end + 1 {
                        k1 = CharSpan {
                            begin: k1.begin,
                            end: k2.end
                        };
                        keys[i] = k1;
                        keys.remove(j);
                        j -= 1;
                    }
                    j += 1;
                }
            }
            for key in keys {
                self.transitions.push(Transition {
                    key: *key,
                    next: *child
                });
            }
        }
    }
}

/// Represents a Deterministic Finite-state Automaton
#[derive(Clone)]
pub struct DFA {
    /// The list of states in this automaton
    pub states: Vec<DFAState>
}

impl DFA {
    /// Creates a new empty DFA
    pub fn new() -> DFA {
        DFA {
            states: Vec::<DFAState>::new()
        }
    }

    /// Initializes this DFA as equivalent to the given NFA
    pub fn from_nfa<'n>(nfa: &'n NFA) -> DFA {
        let mut dfa = DFA::new();
        let mut sets = Vec::<NFAStateSet<'n>>::new();
        // Create the first NFA set, add the entry and close it
        let mut initial = NFAStateSet::new(nfa);
        initial.add(0);
        initial.close();
        dfa.add_state();

        let mut i = 0;
        while i < sets.len() {
            sets[i]
                .get_finals()
                .iter()
                .for_each(|&item| dfa.states[i].items.push(item));
            let transitions = sets[i].get_transitions();
            for (key, child) in transitions.into_iter() {
                let maybe_index = sets.iter()
                    .enumerate()
                    .find(|&(_i, set)| *set == child)
                    .map(|(i, _set)| i);
                match maybe_index {
                    None => {
                        // the child does not exist
                        let id = dfa.states.len();
                        sets.push(child);
                        dfa.add_state();
                        dfa.states[i].transitions.push(Transition { key, next: id });
                    }
                    Some(index) => {
                        // the child already exist
                        dfa.states[i]
                            .transitions
                            .push(Transition { key, next: index });
                    }
                }
            }
            i += 1;
        }
        dfa
    }

    /// Creates a new state in this DFA
    pub fn add_state(&mut self) -> &mut DFAState {
        let id = self.states.len();
        self.states.push(DFAState::new(id));
        &mut self.states[id - 1]
    }

    /// Repacks the transitions of all the states in this automaton
    pub fn repack_transitions(&mut self) {
        for state in self.states.iter_mut() {
            state.repack_transitions();
        }
    }

    /// Gets the minimal DFA equivalent to this one
    pub fn minimize(&self) -> DFA {
        let mut current = DFAPartition::new(self);
        partition_dfa_into(self, &mut current);
        let mut new_partition = current.refine();
        while current.groups.len() != new_partition.groups.len() {
            current = new_partition;
            new_partition = current.refine();
        }
        new_partition.into_dfa()
    }

    /// Prunes all the unreachable states from this automaton
    pub fn prune(&self) -> DFA {
        let inverse = DFAInverse::new(self);
        let finals = inverse.close_by_antecedents(&inverse.finals);
        let entry_index = finals
            .iter()
            .enumerate()
            .find(|&(_i, state)| state.is_entry())
            .unwrap()
            .0;
        let mut result = DFA::new();
        // re-create the states
        // re-create the entry state
        {
            let new_state = result.add_state();
            new_state.add_items(&finals[entry_index]);
        }
        for state in finals.iter() {
            if !state.is_entry() {
                let new_state = result.add_state();
                new_state.add_items(state);
            }
        }
        // re-create the links
        for (i, state) in finals.iter().enumerate() {
            let from_index = if state.is_entry() {
                0
            } else if i < entry_index {
                i + 1
            } else {
                i
            };
            let from = &mut result.states[from_index];
            for transition in state.transitions.iter() {
                let old_target = &self.states[transition.next];
                let old_data = finals
                    .iter()
                    .enumerate()
                    .find(|&(_i, state)| state.id == old_target.id);
                match old_data {
                    None => {}
                    Some((j, _s)) => {
                        let target_index = if old_target.is_entry() {
                            0
                        } else if j < entry_index {
                            j + 1
                        } else {
                            j
                        };
                        from.transitions.push(Transition {
                            key: transition.key,
                            next: target_index
                        });
                    }
                }
            }
        }
        result
    }
}

/// Represents a group of DFA states within a partition
struct DFAStateGroup<'d> {
    /// The states in this group
    states: Vec<&'d DFAState>
}

/// Represents a partition of a DFA
/// This is used to compute minimal DFAs
struct DFAPartition<'d> {
    /// The parent DFA
    dfa: &'d DFA,
    /// The groups in this partition
    groups: Vec<DFAStateGroup<'d>>
}

/// Initializes this partition as the first partition of the given DFA
/// The first partition is according to final, non-final states
fn partition_dfa_into<'d: 'p, 'p>(dfa: &'d DFA, partition: &'p mut DFAPartition<'d>) {
    // Partition the DFA states between final and non-finals
    let has_non_finals = dfa.states
        .iter()
        .fold(false, |acc, ref state| acc || !state.is_final());
    if has_non_finals {
        partition.groups.push(DFAStateGroup::new());
    }
    for state in dfa.states.iter() {
        if state.is_final() {
            let mut added = false;
            for i in 1..partition.groups.len() {
                let group = &mut partition.groups[i];
                // If the current state and the group have same finals set : group found
                if state.same_items(group.representative()) {
                    // Add the set to the group
                    group.states.push(state);
                    added = true;
                    break;
                }
            }
            // No previous group match
            // => Create a new group from the current state
            if !added {
                let count = partition.groups.len();
                partition.groups.push(DFAStateGroup::new());
                partition.groups[count].states.push(state);
            }
        } else {
            // add the state to the non-final group (always at index 0)
            partition.groups[0].states.push(state);
        }
    }
}

impl<'d> DFAStateGroup<'d> {
    /// Initializes a new group
    pub fn new() -> DFAStateGroup<'d> {
        DFAStateGroup {
            states: Vec::<&'d DFAState>::new()
        }
    }

    /// Gets the representative state of this group
    pub fn representative(&self) -> &'d DFAState {
        self.states[0]
    }

    /// Gets whether this is the entry group (contains the entry state)
    pub fn is_entry(&self) -> bool {
        for state in self.states.iter() {
            if state.is_entry() {
                return true;
            }
        }
        false
    }
}

impl<'d> DFAPartition<'d> {
    /// Initializes this partition
    pub fn new(dfa: &'d DFA) -> DFAPartition<'d> {
        DFAPartition {
            dfa,
            groups: Vec::<DFAStateGroup<'d>>::new()
        }
    }

    /// Refines this partition into another one
    pub fn refine(&self) -> DFAPartition<'d> {
        let mut new_partition = DFAPartition::new(self.dfa);
        // For each group in the current partition
        // Split the group and add the resulting groups to the new partition
        for group in self.groups.iter() {
            let mut temp = DFAPartition::new(self.dfa);
            for state in group.states.iter() {
                temp.add_state(*state, self);
            }
            for group in temp.groups.into_iter() {
                new_partition.groups.push(group);
            }
        }
        new_partition
    }

    /// Adds a state to this partition, coming from the given old partition
    pub fn add_state(&mut self, state: &'d DFAState, old_partition: &DFAPartition<'d>) {
        let mut added = false;
        // For each group in the resulting groups set
        for group in self.groups.iter_mut() {
            // If the current state can be in this resulting group according to the old partition :
            if old_partition.in_same_group(group.representative(), state) {
                // Add the state to the group
                group.states.push(state);
                added = true;
            }
        }
        // The state cannot be in any groups : create a new group
        if !added {
            let count = self.groups.len();
            self.groups.push(DFAStateGroup::new());
            self.groups[count].states.push(state);
        }
    }

    /// Determines whether two states should be in the same group
    fn in_same_group(&self, state1: &'d DFAState, state2: &'d DFAState) -> bool {
        if state1.transitions.len() != state2.transitions.len() {
            return false;
        }
        // For each transition from state 1
        for transition in state1.transitions.iter() {
            // If state 2 does not have a transition with the same value : not same group
            if !state2.has_transition(transition.key) {
                return false;
            }
            // Here State1 and State2 have both a transition of the same value
            // If the target of these transitions are in the same group
            // in the old partition : same transition
            let group1 =
                self.get_group_of(&self.dfa.states[state1.get_next_by(transition.key).unwrap()]);
            let group2 =
                self.get_group_of(&self.dfa.states[state2.get_next_by(transition.key).unwrap()]);
            let same = match group1 {
                None => match group2 {
                    None => true,
                    Some(_g) => false
                },
                Some(g1) => match group2 {
                    None => false,
                    Some(g2) => (g1 as *const DFAStateGroup) == (g2 as *const DFAStateGroup)
                }
            };
            if !same {
                return false;
            }
        }
        true
    }

    /// Gets the group of the given state in this partition
    fn get_group_of(&self, state: &'d DFAState) -> Option<&DFAStateGroup<'d>> {
        self.groups
            .iter()
            .find(|&group| group.states.contains(&state))
    }

    /// Gets the new dfa represented by this partition
    pub fn into_dfa(self) -> DFA {
        let mut dfa = DFA::new();
        // get the index of the group with the entry state
        let entry_index = self.groups
            .iter()
            .enumerate()
            .find(|&(_i, group)| group.is_entry())
            .unwrap()
            .0;
        // for the entry group, add the resulting entry state
        {
            let target_state = dfa.add_state();
            target_state.add_items(self.groups[entry_index].representative());
        }
        // for all other groups, add the corresponding new state
        for group in self.groups.iter() {
            if !group.is_entry() {
                let target_state = dfa.add_state();
                target_state.add_items(group.representative());
            }
        }
        // re-link the new states
        for (i, group) in self.groups.iter().enumerate() {
            let from_index = if group.is_entry() {
                0
            } else if i < entry_index {
                i + 1
            } else {
                i
            };
            let from = &mut dfa.states[from_index];
            for transition in group.representative().transitions.iter() {
                let j = self.groups
                    .iter()
                    .enumerate()
                    .find(|&(_j, group)| group.states.contains(&&self.dfa.states[transition.next]))
                    .unwrap()
                    .0;
                let to_index = if self.groups[j].is_entry() {
                    0
                } else if j < entry_index {
                    j + 1
                } else {
                    j
                };
                from.transitions.push(Transition {
                    key: transition.key,
                    next: to_index
                });
            }
        }
        dfa
    }
}

/// Represents the inverse graph of a DFA
struct DFAInverse<'d> {
    /// The final states
    finals: Vec<&'d DFAState>,
    /// The reachable states
    reachable: Vec<&'d DFAState>,
    /// The inverse graph data
    inverse: HashMap<StateId, Vec<&'d DFAState>>
}

impl<'d> DFAInverse<'d> {
    /// Builds this inverse graph from the specified DFA
    pub fn new(dfa: &'d DFA) -> DFAInverse<'d> {
        let mut inverse = DFAInverse {
            finals: Vec::<&'d DFAState>::new(),
            reachable: Vec::<&'d DFAState>::new(),
            inverse: HashMap::<StateId, Vec<&'d DFAState>>::new()
        };
        // transitive closure of the first state
        inverse.reachable.push(&dfa.states[0]);
        inverse.inverse.insert(0, Vec::<&'d DFAState>::new());
        let mut i = 0;
        while i < inverse.reachable.len() {
            let current = inverse.reachable[i];
            for transition in current.transitions.iter() {
                let next_id = transition.next;
                if !inverse.inverse.contains_key(&next_id) {
                    inverse.reachable.push(&dfa.states[next_id]);
                    inverse.inverse.insert(next_id, Vec::<&'d DFAState>::new());
                }
                inverse.inverse.get_mut(&next_id).unwrap().push(current);
            }
            if current.is_final() {
                inverse.finals.push(current);
            }
            i += 1;
        }
        inverse
    }

    /// Closes the specified list of states through inverse transitions
    pub fn close_by_antecedents(&self, states: &Vec<&'d DFAState>) -> Vec<&'d DFAState> {
        let mut result = Vec::<&'d DFAState>::new();
        // transitive closure of the final states by their antecedents
        // final states are all reachable
        for state in states.iter() {
            result.push(state);
        }
        let mut i = 0;
        while i < result.len() {
            let state = result[i];
            if !self.inverse.contains_key(&state.id) {
                continue;
            }
            for antecedent in self.inverse.get(&state.id).unwrap().iter() {
                if !result.contains(antecedent) && self.reachable.contains(antecedent) {
                    // this antecedent is reachable and not yet in the closure
                    // => add it to the closure
                    result.push(antecedent);
                }
            }
            i += 1;
        }
        result
    }
}
