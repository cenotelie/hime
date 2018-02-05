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

use super::CharSpan;

/// Represents a marker for the final state of an automaton
pub trait FinalItem {
    /// Gets the priority of this item
    fn priority(&self) -> isize;
}

/// Represents a fake marker of a final state in an automaton
pub struct DummyItem {}

impl FinalItem for DummyItem {
    fn priority(&self) -> isize {
        -1
    }
}

/// The single dummy item, a fake marker of a final state in an automaton
pub const DUMMY: DummyItem = DummyItem {};

/// Defines the type of state's identifier
type StateId = usize;

#[derive(Copy, Clone, Eq, PartialEq)]
pub struct Transition {
    /// The key on this transition
    pub key: CharSpan,
    /// The identifier of the next state by this transition
    pub next: StateId
}
