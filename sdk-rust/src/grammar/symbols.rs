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

//! Module for the grammar symbols

use std::cmp::Ordering;
use std::hash::Hash;
use std::hash::Hasher;

use super::SymbolId;
use super::super::automata::nfa::NFA;
use super::super::automata::nfa::NFA_EXIT;

/// Represents a symbol in a grammar
pub trait Symbol {
    /// Gets the unique identifier (within a grammar) of this symbol
    fn id(&self) -> SymbolId;
    /// Gets the name of this symbol
    fn name(&self) -> &str;
}

impl PartialEq for Symbol {
    fn eq(&self, other: &Symbol) -> bool {
        self.id() == other.id()
    }
}

impl Eq for Symbol {}

impl PartialOrd for Symbol {
    fn partial_cmp(&self, other: &Symbol) -> Option<Ordering> {
        let other = other.id();
        self.id().partial_cmp(&other)
    }
}

impl Ord for Symbol {
    fn cmp(&self, other: &Symbol) -> Ordering {
        let other = other.id();
        self.id().cmp(&other)
    }
}

impl Hash for Symbol {
    fn hash<H: Hasher>(&self, state: &mut H) {
        self.id().hash(state);
    }
}

/// Represents a symbol for a semantic action in a grammar
pub struct Action {
    /// The unique identifier (within a grammar) of this symbol
    id: usize,
    /// The name of this symbol
    name: String
}

impl Symbol for Action {
    fn id(&self) -> SymbolId {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}

impl Action {
    /// Creates a new action
    pub fn new(id: SymbolId, name: String) -> Action {
        Action { id, name }
    }

    /// Clones this symbol with a new identifier
    pub fn clone_with_id(&self, id: SymbolId) -> Self {
        Action {
            id,
            name: self.name.clone()
        }
    }
}

/// Represents a virtual symbol in a grammar
pub struct Virtual {
    /// The unique identifier (within a grammar) of this symbol
    id: usize,
    /// The name of this symbol
    name: String
}

impl Symbol for Virtual {
    fn id(&self) -> SymbolId {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}

impl Virtual {
    /// Creates a new virtual
    pub fn new(id: SymbolId, name: String) -> Virtual {
        Virtual { id, name }
    }

    /// Clones this symbol with a new identifier
    pub fn clone_with_id(&self, id: SymbolId) -> Self {
        Virtual {
            id,
            name: self.name.clone()
        }
    }
}

/// Represents a terminal symbol in a grammar
#[derive(Clone)]
pub struct Terminal {
    /// The unique identifier (within a grammar) of this symbol
    id: usize,
    /// The name of this symbol
    name: String,
    /// The inline value of this terminal
    value: String,
    /// The context of this terminal
    context: usize,
    /// The NFA that is used to match this terminal
    nfa: Option<NFA>
}

impl Symbol for Terminal {
    fn id(&self) -> SymbolId {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}

impl Terminal {
    /// Creates a new terminal
    pub fn new(id: SymbolId, name: String, value: String, context: usize, nfa: NFA) -> Terminal {
        Terminal {
            id,
            name,
            value,
            context,
            nfa: Some(nfa)
        }
    }

    /// Gets the terminal's value
    pub fn value(&self) -> &str {
        &self.value
    }

    /// Gets the context for this terminal
    pub fn context(&self) -> usize {
        self.context
    }

    /// Clones this symbol with a new identifier
    pub fn clone_with_id(&self, id: SymbolId) -> Self {
        let mut result = Terminal {
            id,
            name: self.name.clone(),
            value: self.value.clone(),
            context: self.context,
            nfa: self.nfa.clone()
        };
        if result.nfa.is_some() {
            let exit_state = &mut result.nfa.as_mut().unwrap().states[NFA_EXIT];
            exit_state.items.clear();
            exit_state.items.push(id);
        }
        result
    }
}

/// Represents the absence of terminal, used as a marker by LR-related algorithms
lazy_static! {
    pub static ref TERMINAL_NULL: Terminal = Terminal {
        id: 0,
        name: "#".to_owned(),
        value: "#".to_owned(),
        context: 0,
        nfa: None
    };
}

/// Represents a fake terminal, used as a marker by LR-related algorithms
lazy_static! {
    pub static ref TERMINAL_DUMMY: Terminal = Terminal {
        id: 0,
        name: "#".to_owned(),
        value: "#".to_owned(),
        context: 0,
        nfa: None
    };
}

/// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
lazy_static! {
    pub static ref TERMINAL_EPSILON: Terminal = Terminal {
        id: 1,
        name: "ε".to_owned(),
        value: "ε".to_owned(),
        context: 0,
        nfa: None
    };
}

/// Represents the dollar symbol in a grammar, i.e. the marker of end of input
lazy_static! {
    pub static ref TERMINAL_DOLLAR: Terminal = Terminal {
        id: 2,
        name: "$".to_owned(),
        value: "$".to_owned(),
        context: 0,
        nfa: None
    };
}

/// Represents a variable in a grammar
pub struct Variable {
    /// The unique identifier (within a grammar) of this symbol
    id: usize,
    /// The name of this symbol
    name: String
}

impl Symbol for Variable {
    fn id(&self) -> SymbolId {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}

impl Variable {
    /// Creates a new variable
    pub fn new(id: SymbolId, name: String) -> Variable {
        Variable { id, name }
    }

    /// Clones this symbol with a new identifier
    pub fn clone_with_id(&self, id: SymbolId) -> Self {
        Variable {
            id,
            name: self.name.clone()
        }
    }
}

/// A reference to a grammar symbol
#[derive(Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
pub enum SymbolReference {
    /// A reference to a terminal
    Terminal(SymbolId),
    /// A reference to a variable
    Variable(SymbolId),
    /// A reference to a virtual
    Virtual(SymbolId),
    /// A reference to an action
    Action(SymbolId)
}

impl SymbolReference {
    /// Get the identifier of the symbol
    pub fn id(&self) -> SymbolId {
        match self {
            &SymbolReference::Terminal(ref id) => *id,
            &SymbolReference::Variable(ref id) => *id,
            &SymbolReference::Virtual(ref id) => *id,
            &SymbolReference::Action(ref id) => *id
        }
    }
}
