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

//! Module for the NFA automata

use super::FinalItem;
use super::StateId;
use super::Transition;
use super::super::CHAR_SPAN_NULL;
use super::super::CharSpan;

use super::dfa::DFA;

/// The epsilon transition label in an NFA
pub const EPSILON: CharSpan = CHAR_SPAN_NULL;

/// The neutral mark for a NFA state (the state is not marked)
pub const MARK_NEUTRAL: isize = 0;

/// The identifier of the entry state in an NFA
pub const NFA_ENTRY: StateId = 0;

/// The identifier of the exit state in an NFA
pub const NFA_EXIT: StateId = 1;

/// The single dummy item, a fake marker of a final state in an automaton
const DUMMY: FinalItem = 0xFFFFFFFF;

/// Represents a state in a Non-deterministic Finite Automaton
#[derive(Clone)]
pub struct NFAState {
    /// This state's id
    pub id: StateId,
    /// List of the items on this state
    pub items: Vec<FinalItem>,
    /// The transitions from this state
    pub transitions: Vec<Transition>,
    /// The watermark of this state, if any
    pub mark: isize
}

impl PartialEq for NFAState {
    fn eq(&self, other: &NFAState) -> bool {
        self.id == other.id
    }
}

impl Eq for NFAState {}

impl NFAState {
    /// Creates a new state
    pub fn new(id: StateId) -> NFAState {
        NFAState {
            id,
            items: Vec::<FinalItem>::new(),
            transitions: Vec::<Transition>::new(),
            mark: MARK_NEUTRAL
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

    /// Adds to this state the same items as the specified one
    pub fn add_items(&mut self, state: &NFAState) {
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
}

/// Represents a Non-deterministic Finite-state Automaton
#[derive(Clone)]
pub struct NFA {
    /// The list of states in this automaton
    pub states: Vec<NFAState>
}

impl NFA {
    /// Creates a new empty NFA
    pub fn new_empty() -> NFA {
        NFA {
            states: Vec::<NFAState>::new()
        }
    }

    /// Creates a minimal NFA with an entry and an exit state
    pub fn new_minimal() -> NFA {
        let mut states = Vec::<NFAState>::new();
        states.push(NFAState::new(NFA_ENTRY));
        states.push(NFAState::new(NFA_EXIT));
        NFA { states }
    }

    /// Creates an automaton that represents makes the given sub-automaton optional
    pub fn new_optional(sub: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let sub_first = result.include_nfa(sub);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: NFA_EXIT
        });
        result.states[sub_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result
    }

    /// Creates an automaton that repeats the sub-automaton zero or more times
    pub fn new_repeat_zero_or_more(sub: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let sub_first = result.include_nfa(sub);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: NFA_EXIT
        });
        result.states[sub_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result.states[NFA_EXIT].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        result
    }

    /// Creates an automaton that repeats the sub-automaton one or more times
    pub fn new_repeat_one_or_more(sub: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let sub_first = result.include_nfa(sub);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        result.states[sub_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result.states[NFA_EXIT].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        result
    }

    /// Creates an automaton that repeats the sub-automaton a number of times
    /// in the given range [min, max]
    pub fn new_repeat_range(sub: &NFA, min: usize, max: usize) -> NFA {
        let mut result = NFA::new_minimal();
        let mut last = NFA_ENTRY;
        for _i in 0..min {
            let sub_first = result.include_nfa(sub);
            result.states[last].transitions.push(Transition {
                key: EPSILON,
                next: sub_first + NFA_ENTRY
            });
            last = sub_first + NFA_EXIT;
        }
        for _i in min..max {
            let optional_entry = result.add_state().id;
            let optional_exit = result.add_state().id;
            let sub_first = result.include_nfa(sub);
            result.states[optional_entry].transitions.push(Transition {
                key: EPSILON,
                next: sub_first + NFA_ENTRY
            });
            result.states[optional_entry].transitions.push(Transition {
                key: EPSILON,
                next: optional_exit
            });
            result.states[sub_first + NFA_EXIT]
                .transitions
                .push(Transition {
                    key: EPSILON,
                    next: optional_exit
                });
            result.states[last].transitions.push(Transition {
                key: EPSILON,
                next: optional_entry
            });
            last = optional_exit;
        }
        result.states[last].transitions.push(Transition {
            key: EPSILON,
            next: NFA_EXIT
        });
        if min == 0 {
            result.states[NFA_ENTRY].transitions.push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        }
        result
    }

    /// Creates an automaton that is the union of the two sub-automaton
    pub fn new_union(left: &NFA, right: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let left_first = result.include_nfa(left);
        let right_first = result.include_nfa(right);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: left_first + NFA_ENTRY
        });
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: right_first + NFA_ENTRY
        });
        result.states[left_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result.states[right_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result
    }

    /// Creates an automaton that concatenates the two sub-automaton
    pub fn new_concatenation(left: &NFA, right: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let left_first = result.include_nfa(left);
        let right_first = result.include_nfa(right);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: left_first + NFA_ENTRY
        });
        result.states[left_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: right_first + NFA_ENTRY
            });
        result.states[right_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: NFA_EXIT
            });
        result
    }

    /// Creates an automaton that is the difference between the left and right sub-automata
    pub fn new_difference(left: &NFA, right: &NFA) -> NFA {
        let mut result = NFA::new_minimal();
        let positive = result.add_state().id;
        let negative = result.add_state().id;
        result.states[positive].mark = 1;
        result.states[negative].mark = -1;
        let left_first = result.include_nfa(left);
        let right_first = result.include_nfa(right);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: left_first + NFA_ENTRY
        });
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: right_first + NFA_ENTRY
        });
        result.states[left_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: positive
            });
        result.states[right_first + NFA_EXIT]
            .transitions
            .push(Transition {
                key: EPSILON,
                next: negative
            });
        result.states[positive].transitions.push(Transition {
            key: EPSILON,
            next: NFA_EXIT
        });
        result.states[NFA_EXIT].items.push(DUMMY);

        let mut dfa = DFA::from_nfa(&result);
        dfa = dfa.prune();

        let mut result = NFA::new_minimal();
        let sub_first = result.include_dfa(&dfa);
        result.states[NFA_ENTRY].transitions.push(Transition {
            key: EPSILON,
            next: sub_first + NFA_ENTRY
        });
        let sub_exit = sub_first
            + dfa.states
                .iter()
                .enumerate()
                .find(|&(_i, state)| state.items.contains(&DUMMY))
                .map(|(i, _state)| i)
                .unwrap();
        result.states[sub_exit].items.clear();
        result.states[sub_exit].transitions.push(Transition {
            key: EPSILON,
            next: NFA_EXIT
        });
        result
    }

    /// Adds a new state to this automaton
    pub fn add_state(&mut self) -> &mut NFAState {
        let id = self.states.len();
        self.states.push(NFAState::new(id));
        &mut self.states[id]
    }

    /// Clones this automaton
    pub fn clone_with_finals(&self) -> NFA {
        self.clone(true)
    }

    /// Clones this automaton
    pub fn clone(&self, keep_finals: bool) -> NFA {
        let mut result = NFA::new_empty();
        for original in self.states.iter() {
            let target = result.add_state();
            target.mark = original.mark;
            if keep_finals {
                original
                    .items
                    .iter()
                    .for_each(|&item| target.items.push(item));
            }
            original
                .transitions
                .iter()
                .for_each(|&transition| target.transitions.push(transition));
        }
        result
    }

    /// Inserts all the states of the given automaton into this one
    /// This does not make a copy of the states, this directly includes them
    pub fn include_nfa(&mut self, nfa: &NFA) -> usize {
        let shift = self.states.len();
        // copy the states
        for state in nfa.states.iter() {
            let target = self.add_state();
            target.mark = state.mark;
            state.items.iter().for_each(|&item| target.items.push(item));
            state.transitions.iter().for_each(|&transition| {
                target.transitions.push(Transition {
                    key: transition.key,
                    next: transition.next + shift
                })
            });
        }
        shift
    }

    /// Inserts all the states of the given automaton into this one
    /// This does not make a copy of the states, this directly includes them
    pub fn include_dfa(&mut self, dfa: &DFA) -> usize {
        let shift = self.states.len();
        // copy the states
        for state in dfa.states.iter() {
            let target = self.add_state();
            state.items.iter().for_each(|&item| target.items.push(item));
            state.transitions.iter().for_each(|&transition| {
                target.transitions.push(Transition {
                    key: transition.key,
                    next: transition.next + shift
                })
            });
        }
        shift
    }
}

/// Represents a set of states in a Non-deterministic Finite Automaton
/// A state can only appear once in a set
pub struct NFAStateSet<'n> {
    /// The parent NFA
    nfa: &'n NFA,
    /// The identifiers of the states in this set
    states: Vec<StateId>
}

impl<'n> PartialEq for NFAStateSet<'n> {
    fn eq(&self, other: &NFAStateSet<'n>) -> bool {
        (self.nfa as *const NFA) == (other.nfa as *const NFA)
            && self.states.len() == other.states.len()
            && self.states
                .iter()
                .fold(true, |acc, &id| acc && other.states.contains(&id))
    }
}

impl<'n> Eq for NFAStateSet<'n> {}

impl<'n> NFAStateSet<'n> {
    /// Creates a new set
    pub fn new(nfa: &'n NFA) -> NFAStateSet<'n> {
        NFAStateSet {
            nfa,
            states: Vec::<StateId>::new()
        }
    }

    /// Adds the given state in this set if it is not already present
    pub fn add(&mut self, state: StateId) {
        if !self.states.contains(&state) {
            self.states.push(state);
        }
    }

    /// Closes this set by transitively adding to it all reachable state by the epsilon transition
    /// This looks for the watermark of states
    pub fn close(&mut self) {
        // Close the set
        let mut i = 0;
        while i < self.states.len() {
            let id = self.states[i];
            for transition in self.nfa.states[id].transitions.iter() {
                if transition.key == EPSILON {
                    self.add(transition.next);
                }
            }
            i += 1;
        }
        // Look for a positive and a negative node
        let positive = self.states
            .iter()
            .enumerate()
            .find(|&(_i, &id)| self.nfa.states[id].mark > 0)
            .map(|(i, _id)| i);
        let negative = self.states
            .iter()
            .enumerate()
            .find(|&(_i, &id)| self.nfa.states[id].mark < 0)
            .map(|(i, _id)| i);
        // With both negative and positive states
        // remove the states immediately reached with epsilon from the positive state
        if positive.is_some() && negative.is_some() {
            for transition in self.nfa.states[positive.unwrap()].transitions.iter() {
                if transition.key == EPSILON {
                    let index = self.states
                        .iter()
                        .enumerate()
                        .find(|&(_i, &id)| id == transition.next)
                        .unwrap()
                        .0;
                    self.states.remove(index);
                }
            }
        }
    }

    /// Gets transitions from this set to other sets
    pub fn get_transitions(&self) -> Vec<(CharSpan, NFAStateSet<'n>)> {
        let mut transitions = Vec::<Transition>::new();
        self.states.iter().for_each(|&id| {
            self.nfa.states[id]
                .transitions
                .iter()
                .for_each(|&transition| transitions.push(transition))
        });
        // normalize the original transitions
        if !transitions.is_empty() {
            transitions = super::normalize(transitions);
        }
        transitions.sort();

        let mut result = Vec::<(CharSpan, NFAStateSet<'n>)>::new();
        let mut i = 0;
        while i < transitions.len() {
            let key = transitions[i].key;
            let mut child = NFAStateSet::new(self.nfa);
            child.add(transitions[i].next);
            i += 1;
            while i < transitions.len() && transitions[i].key == key {
                child.add(transitions[i].next);
                i += 1;
            }
            child.close();
            result.push((key, child));
        }
        result
    }

    /// Gets all the final markers of all the states in this set
    pub fn get_finals(&self) -> Vec<FinalItem> {
        let mut finals = Vec::<FinalItem>::new();
        self.states.iter().for_each(|&id| {
            self.nfa.states[id]
                .items
                .iter()
                .for_each(|&item| finals.push(item))
        });
        finals
    }
}
