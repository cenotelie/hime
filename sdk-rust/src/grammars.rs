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

//! Library for grammars

use crate::automata::fa::NFA;
use crate::automata::lr::SymbolRef;
use hime_redist::parsers::{TreeAction, TREE_ACTION_NONE};
use std::cmp::Ordering;

/// Represents a symbol in a grammar
pub trait Symbol {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize;

    /// Gets the name of this symbol
    fn get_name(&self) -> &str;
}

/// Represents a terminal symbol in a grammar
#[derive(Debug, Clone)]
pub struct Terminal {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String,
    /// The inline value of this terminal
    pub value: String,
    /// The NFA that is used to match this terminal
    pub nfa: NFA,
    /// The context of this terminal
    pub context: usize
}

impl Terminal {
    /// Gets the priority of this terminal
    pub fn priority(&self) -> usize {
        self.id
    }
}

impl Symbol for Terminal {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Terminal {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Terminal {}

impl Ord for Terminal {
    fn cmp(&self, other: &Terminal) -> Ordering {
        self.id.cmp(&other.id)
    }
}

impl PartialOrd for Terminal {
    fn partial_cmp(&self, other: &Terminal) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

/// Represents a reference to a terminal-like
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub enum TerminalRef {
    /// Represents a fake terminal, used as a marker by LR-related algorithms
    Dummy,
    /// Represents the epsilon symbol in a grammar, i.e. a terminal with an empty value
    Epsilon,
    /// Represents the dollar symbol in a grammar, i.e. the marker of end of input
    Dollar,
    /// Represents the absence of terminal, used as a marker by LR-related algorithms
    NullTerminal,
    /// A terminal in a grammar
    Terminal(usize)
}

impl TerminalRef {
    /// Gets the terminal priority
    pub fn priority(self) -> usize {
        match self {
            TerminalRef::Dummy => 0,
            TerminalRef::Epsilon => 1,
            TerminalRef::Dollar => 2,
            TerminalRef::NullTerminal => 0,
            TerminalRef::Terminal(id) => id
        }
    }
}

impl Ord for TerminalRef {
    fn cmp(&self, other: &TerminalRef) -> Ordering {
        self.priority().cmp(&other.priority())
    }
}

impl PartialOrd for TerminalRef {
    fn partial_cmp(&self, other: &TerminalRef) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

/// Represents a set of unique terminals (sorted by ID)
#[derive(Debug, Clone, Default)]
pub struct TerminalSet {
    /// The backing content
    pub content: Vec<TerminalRef>
}

impl TerminalSet {
    /// Gets the number of states in this automaton
    pub fn len(&self) -> usize {
        self.content.len()
    }

    /// Gets whether the NFA has no state
    pub fn is_empty(&self) -> bool {
        self.content.is_empty()
    }

    /// Adds a new terminal
    fn do_add(&mut self, item: TerminalRef) {
        if !self.content.contains(&item) {
            self.content.push(item);
        }
    }

    /// Adds a new terminal
    pub fn add(&mut self, item: TerminalRef) {
        self.do_add(item);
        self.content.sort();
    }

    /// Adds new terminals
    pub fn add_items(&mut self, items: &[TerminalRef]) {
        for item in items.iter() {
            self.do_add(*item);
        }
        self.content.sort();
    }

    /// Removes all items from this collection
    pub fn clear(&mut self) {
        self.content.clear();
    }
}

/// Represents a virtual symbol in a grammar
#[derive(Debug, Clone)]
pub struct Virtual {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String
}

impl Symbol for Virtual {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Virtual {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Virtual {}

/// Represents a symbol for a semantic action in a grammar
#[derive(Debug, Clone)]
pub struct Action {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String
}

impl Symbol for Action {
    /// Gets the unique indentifier (within a grammar) of this symbol
    fn get_id(&self) -> usize {
        self.id
    }

    /// Gets the name of this symbol
    fn get_name(&self) -> &str {
        &self.name
    }
}

impl PartialEq for Action {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Action {}

/// Represents a variable in a grammar
#[derive(Debug, Clone)]
pub struct Variable {
    /// The unique indentifier (within a grammar) of this symbol
    pub id: usize,
    /// The name of this symbol
    pub name: String,
    /// The FIRSTS set for this variable
    pub firsts: TerminalSet,
    /// The FOLLOWERS set for this variable
    pub followers: TerminalSet
}

impl PartialEq for Variable {
    fn eq(&self, other: &Self) -> bool {
        self.id == other.id
    }
}

impl Eq for Variable {}

/// Represents an element in the body of a grammar rule
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct RuleBodyElement {
    /// The symbol of this element
    pub symbol: SymbolRef,
    /// The action applied on this element
    pub action: TreeAction
}

impl RuleBodyElement {
    /// Creates a new body element
    pub fn new(symbol: SymbolRef, action: TreeAction) -> RuleBodyElement {
        RuleBodyElement { symbol, action }
    }
}

/// Represents a choice in a rule, i.e. the remainder of a rule's body
#[derive(Debug, Clone, Default)]
pub struct RuleChoice {
    /// The elements in this body
    pub parts: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet
}

impl RuleChoice {
    /// Creates a new choice from a single symbol
    pub fn new(symbol: SymbolRef) -> RuleChoice {
        RuleChoice {
            parts: vec![RuleBodyElement::new(symbol, TREE_ACTION_NONE)],
            firsts: TerminalSet::default()
        }
    }

    /// Initializes this rule body from parts
    pub fn from_parts(parts: &[RuleBodyElement]) -> RuleBody {
        RuleBody {
            parts: parts.to_vec(),
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    pub fn len(&self) -> usize {
        self.parts.len()
    }

    /// Gets wether the rule choice is empty
    pub fn is_empty(&self) -> bool {
        self.parts.is_empty()
    }

    /// Appends a single symbol to the choice
    pub fn append_symbol(&mut self, symbol: SymbolRef) {
        self.parts
            .push(RuleBodyElement::new(symbol, TREE_ACTION_NONE));
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in other.parts.iter() {
            self.parts.push(*element);
        }
    }
}

impl PartialEq for RuleChoice {
    fn eq(&self, other: &Self) -> bool {
        self.parts.len() == other.parts.len()
            && self
                .parts
                .iter()
                .zip(other.parts.iter())
                .all(|(e1, e2)| e1 == e2)
    }
}

impl Eq for RuleChoice {}

/// Represents the body of a grammar rule
#[derive(Debug, Clone, Default)]
pub struct RuleBody {
    /// The elements in this body
    pub parts: Vec<RuleBodyElement>,
    /// The FIRSTS set of terminals
    pub firsts: TerminalSet,
    /// The choices in this body
    pub choices: Vec<RuleChoice>
}

impl RuleBody {
    /// Initializes this rule body
    pub fn new(symbol: SymbolRef) -> RuleBody {
        RuleBody {
            parts: vec![RuleBodyElement::new(symbol, TREE_ACTION_NONE)],
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Initializes this rule body from parts
    pub fn from_parts(parts: &[RuleBodyElement]) -> RuleBody {
        RuleBody {
            parts: parts.to_vec(),
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Produces the concatenation of the left and right bodies
    pub fn concatenate(left: &RuleBody, right: &RuleBody) -> RuleBody {
        let mut parts = Vec::new();
        for element in left.parts.iter() {
            parts.push(*element);
        }
        for element in right.parts.iter() {
            parts.push(*element);
        }
        RuleBody {
            parts,
            firsts: TerminalSet::default(),
            choices: Vec::new()
        }
    }

    /// Gets the length of the rule choice
    pub fn len(&self) -> usize {
        self.parts.len()
    }

    /// Gets wether the rule choice is empty
    pub fn is_empty(&self) -> bool {
        self.parts.is_empty()
    }

    /// Appends a single symbol to the choice
    pub fn append_symbol(&mut self, symbol: SymbolRef) {
        self.parts
            .push(RuleBodyElement::new(symbol, TREE_ACTION_NONE));
    }

    /// Appends the content of another choice to this one
    pub fn append_choice(&mut self, other: &RuleChoice) {
        for element in other.parts.iter() {
            self.parts.push(*element);
        }
    }

    /// Applies the given action to all elements in this body
    pub fn apply_action(&mut self, action: TreeAction) {
        for element in self.parts.iter_mut() {
            element.action = action;
        }
    }
}

impl PartialEq for RuleBody {
    fn eq(&self, other: &Self) -> bool {
        self.parts.len() == other.parts.len()
            && self
                .parts
                .iter()
                .zip(other.parts.iter())
                .all(|(e1, e2)| e1 == e2)
    }
}

impl Eq for RuleBody {}
