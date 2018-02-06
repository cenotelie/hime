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

/// Represents a state in a Non-deterministic Finite Automaton
pub struct NFAState<'s> {
    /// This state's id
    pub id: StateId,
    /// List of the items on this state
    pub items: Vec<&'s FinalItem>,
    /// The transitions from this state
    pub transitions: Vec<Transition>,
    /// The watermark of this state, if any
    pub mark: isize
}

impl<'s> PartialEq for NFAState<'s> {
    fn eq(&self, other: &NFAState<'s>) -> bool {
        self.id == other.id
    }
}

impl<'s> Eq for NFAState<'s> {}

impl<'s> NFAState<'s> {
    /// Creates a new state
    pub fn new(id: StateId) -> NFAState<'s> {
        NFAState {
            id,
            items: Vec::<&'s FinalItem>::new(),
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
    pub fn add_items(&mut self, state: &NFAState<'s>) {
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
pub struct NFA<'s> {
    /// The list of states in this automaton
    pub states: Vec<NFAState<'s>>,
    /// The identifier of the exit state, if any
    pub exit: Option<StateId>
}

impl<'s> NFA<'s> {
    /// Creates a new empty NFA
    pub fn new_empty() -> NFA<'s> {
        NFA {
            states: Vec::<NFAState<'s>>::new(),
            exit: None
        }
    }

    /// Creates a minimal NFA with an entry and an exit state
    pub fn new_minimal() -> NFA<'s> {
        let mut states = Vec::<NFAState<'s>>::new();
        states.push(NFAState::new(0));
        states.push(NFAState::new(1));
        NFA {
            states,
            exit: Some(1)
        }
    }

    /// Initializes this automaton as a copy of the given DFA
    /// This automaton will not have an exit state
    pub fn from_dfa(dfa: &DFA<'s>) -> NFA<'s> {
        let mut states = Vec::<NFAState<'s>>::new();
        for dfa_state in dfa.states.iter() {
            let mut nfa_state = NFAState::new(dfa_state.id);
            dfa_state
                .items
                .iter()
                .for_each(|&item| nfa_state.items.push(item));
            dfa_state
                .transitions
                .iter()
                .for_each(|&transition| nfa_state.transitions.push(transition));
            states.push(nfa_state);
        }
        NFA { states, exit: None }
    }

    /// Adds a new state to this automaton
    pub fn add_state(&mut self) -> &mut NFAState<'s> {
        let id = self.states.len();
        self.states.push(NFAState::new(id));
        &mut self.states[id]
    }

    /// Clones this automaton
    pub fn clone_with_finals(&self) -> NFA<'s> {
        self.clone(true)
    }

    /// Clones this automaton
    pub fn clone(&self, keep_finals: bool) -> NFA<'s> {
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
        result.exit = self.exit;
        result
    }

    /// Inserts all the states of the given automaton into this one
    /// This does not make a copy of the states, this directly includes them
    pub fn include(&mut self, nfa: NFA<'s>) {
        nfa.states
            .into_iter()
            .for_each(|state| self.states.push(state));
    }
}

/// Represents a set of states in a Non-deterministic Finite Automaton
/// A state can only appear once in a set
pub struct NFAStateSet<'n, 's: 'n> {
    /// The parent NFA
    nfa: &'n NFA<'s>,
    /// The identifiers of the states in this set
    states: Vec<StateId>
}

impl<'n, 's: 'n> PartialEq for NFAStateSet<'n, 's> {
    fn eq(&self, other: &NFAStateSet<'n, 's>) -> bool {
        (self.nfa as *const NFA<'s>) == (other.nfa as *const NFA<'s>)
            && self.states.len() == other.states.len()
            && self.states
                .iter()
                .fold(true, |acc, &id| acc && other.states.contains(&id))
    }
}

impl<'n, 's: 'n> Eq for NFAStateSet<'n, 's> {}

impl<'n, 's: 'n> NFAStateSet<'n, 's> {
    /// Creates a new set
    pub fn new(nfa: &'n NFA<'s>) -> NFAStateSet<'n, 's> {
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
    pub fn get_transitions(&self) -> Vec<(CharSpan, NFAStateSet<'n, 's>)> {
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

        let mut result = Vec::<(CharSpan, NFAStateSet<'n, 's>)>::new();
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
    pub fn get_finals(&self) -> Vec<&'s FinalItem> {
        let mut finals = Vec::<&'s FinalItem>::new();
        self.states.iter().for_each(|&id| {
            self.nfa.states[id]
                .items
                .iter()
                .for_each(|&item| finals.push(item))
        });
        finals
    }
}