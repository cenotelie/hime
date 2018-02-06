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

//! Module for the automata

pub mod dfa;
pub mod nfa;

use std::cmp::Ordering;

use super::CharSpan;

/// Represents a marker for the final state of an automaton
pub trait FinalItem {
    /// Gets the priority of this item
    fn priority(&self) -> usize;
}

/// Defines the type of state's identifier
type StateId = usize;

#[derive(Copy, Clone, Eq, PartialEq, Ord)]
pub struct Transition {
    /// The key on this transition
    pub key: CharSpan,
    /// The identifier of the next state by this transition
    pub next: StateId
}

impl PartialOrd for Transition {
    fn partial_cmp(&self, other: &Transition) -> Option<Ordering> {
        self.key.begin.partial_cmp(&other.key.begin)
    }
}

/// Normalizes a set of transitions, i.e. no intersecting transitions excepting equal ones
pub fn normalize(transitions: Vec<Transition>) -> Vec<Transition> {
    let mut result = Vec::<Transition>::new();
    for transition in transitions.into_iter() {
        push_normalized(&mut result, transition);
    }
    result
}

/// Pushes a transition in a vector of normalized transitions
fn push_normalized(transitions: &mut Vec<Transition>, transition: Transition) {
    let l = transitions.len();
    for i in 0..l {
        let candidate = transitions[i];
        if transition.key == candidate.key {
            // same span, continue
            continue;
        }
        let maybe_intersect = CharSpan::intersect(transition.key, candidate.key);
        if maybe_intersect.is_none() {
            // no intersection, continue
            continue;
        }
        let splitter = maybe_intersect.unwrap();
        // split the candidate currently in the vector
        let (left, right) = CharSpan::split(candidate.key, splitter);
        transitions[i] = Transition {
            key: splitter,
            next: candidate.next
        };
        if left.is_some() {
            transitions.push(Transition {
                key: left.unwrap(),
                next: candidate.next
            });
        }
        if right.is_some() {
            transitions.push(Transition {
                key: right.unwrap(),
                next: candidate.next
            });
        }
        // split the new transaction
        let (left, right) = CharSpan::split(transition.key, splitter);
        // the splitter should be safe to add
        transitions.push(Transition {
            key: splitter,
            next: transition.next
        });
        // now treat the rest
        if left.is_some() {
            push_normalized(
                transitions,
                Transition {
                    key: left.unwrap(),
                    next: transition.next
                }
            );
        }
        if right.is_some() {
            push_normalized(
                transitions,
                Transition {
                    key: right.unwrap(),
                    next: transition.next
                }
            );
        }
        // stop here because the incoming transition was decomposed
        return;
    }
    // no intersection => add as is
    transitions.push(transition);
}
