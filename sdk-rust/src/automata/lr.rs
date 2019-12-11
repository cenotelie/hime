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

//! LR automata

/// Represents a reference to a grammar symbol
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum SymbolRef {
    /// Represents a fake terminal, used as a marker by LR-related algorithms
    Dummy,
    /// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
    Epsilon,
    /// Represents the dollar symbol in a grammar, i.e. the marker of end of input
    Dollar,
    /// Represents the absence of terminal, used as a marker by LR-related algorithms
    NullTerminal,
    /// A terminal in a grammar
    Terminal(usize),
    /// A variable in a grammar
    Variable(usize),
    /// A virtual symbol in a grammar
    Virtual(usize),
    /// An action symbol in a grammar
    Action(usize)
}

/// Represents a transition in a LR automaton
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct Transition {
    /// The label on this transition
    pub label: SymbolRef,
    /// The transition's target
    pub target: usize
}

impl Transition {
    /// Creates a new transition
    pub fn new(label: SymbolRef, target: usize) -> Transition {
        Transition { label, target }
    }
}

/// Represents a reduction in a LR automaton
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct Reduction {
    /// The lookahead to reduce on
    pub lookahead: SymbolRef,
    /// The reduced variable
    pub head: usize,
    /// The reduction's length
    pub length: usize
}

impl Reduction {
    /// Create a new reduction
    pub fn new(lookahead: SymbolRef, head: usize, length: usize) -> Reduction {
        Reduction {
            lookahead,
            head,
            length
        }
    }
}

/// Represents a state in an LR automaton
#[derive(Debug, Clone)]
pub struct State {
    /// The state's identifier
    pub id: usize,
    /// The transitions from this state
    pub transitions: Vec<Transition>,
    /// The reductions in this state
    pub reductions: Vec<Reduction>,
    /// Whether this state is an accepting state
    pub is_accept: bool
}

impl PartialEq for State {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for State {}

impl State {
    /// Create a new state
    pub fn new(id: usize) -> State {
        State {
            id,
            transitions: Vec::new(),
            reductions: Vec::new(),
            is_accept: false
        }
    }
}

/// Represents an LR automaton
#[derive(Debug, Clone, Default)]
pub struct Automaton {
    /// The states
    pub states: Vec<State>
}

impl Automaton {
    /// Gets the number of states in this automaton
    pub fn len(&self) -> usize {
        self.states.len()
    }

    /// Gets whether the DFA has no state
    pub fn is_empty(&self) -> bool {
        self.states.is_empty()
    }

    /// Creates a new state in this automaton
    pub fn add_state(&mut self) -> &mut State {
        let index = self.states.len();
        self.states.push(State::new(index));
        &mut self.states[index]
    }
}
