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

/// Represents a symbol in a grammar
pub trait Symbol {
    /// Gets the unique identifier (within a grammar) of this symbol
    fn id(&self) -> usize;
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
    fn id(&self) -> usize {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
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
    fn id(&self) -> usize {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}

/// Represents a terminal symbol in a grammar
pub struct Terminal {
    /// The unique identifier (within a grammar) of this symbol
    id: usize,
    /// The name of this symbol
    name: String,
    /// The inline value of this terminal
    value: String,
    /// The context of this terminal
    context: usize
}

impl Symbol for Terminal {
    fn id(&self) -> usize {
        self.id
    }

    fn name(&self) -> &str {
        &self.name
    }
}
