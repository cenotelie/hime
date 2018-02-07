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

//! Module for the grammar rules

use super::SymbolId;
use super::symbols::SymbolReference;

use std::collections::HashMap;
use std::ops::Index;

/// Represents a tree action
#[derive(Copy, Clone, Eq, PartialEq)]
pub enum TreeAction {
    /// Keep the node as is
    None,
    /// Promote the node, i.e. replace its parent with it and insert its children where it was
    Promote,
    /// Drop the node and all its descendants
    Drop
}

/// Represents an element in the body of a grammar rule
#[derive(Copy, Clone, Eq, PartialEq)]
pub struct RuleBodyElement {
    /// The symbol of this element
    symbol: SymbolReference,
    /// The action applied on this element
    action: TreeAction
}

impl RuleBodyElement {
    /// Creates a new element
    pub fn new(symbol: SymbolReference, action: TreeAction) -> RuleBodyElement {
        RuleBodyElement { symbol, action }
    }

    /// Clones with an updated identifier
    pub fn clone_with_ids(&self, map: &HashMap<SymbolId, SymbolId>) -> RuleBodyElement {
        RuleBodyElement {
            symbol: self.symbol.clone_with_ids(map),
            action: self.action
        }
    }
}

/// Represents the body of a grammar rule
#[derive(Clone)]
pub struct RuleBody {
    /// The elements in this body
    parts: Vec<RuleBodyElement>
}

impl PartialEq for RuleBody {
    fn eq(&self, other: &RuleBody) -> bool {
        if self.parts.len() != other.parts.len() {
            return false;
        }
        for i in 0..self.parts.len() {
            if self.parts[i] != other.parts[i] {
                return false;
            }
        }
        true
    }
}

impl Eq for RuleBody {}

impl RuleBody {
    /// Creates a new choice
    pub fn new() -> RuleBody {
        RuleBody {
            parts: Vec::<RuleBodyElement>::new()
        }
    }

    /// Creates a new choice
    pub fn new_single(first: RuleBodyElement) -> RuleBody {
        let mut parts = Vec::<RuleBodyElement>::new();
        parts.push(first);
        RuleBody { parts }
    }

    /// Clones with an updated identifier
    pub fn clone_with_ids(&self, map: &HashMap<SymbolId, SymbolId>) -> RuleBody {
        RuleBody {
            parts: self.parts
                .iter()
                .map(|ref part| part.clone_with_ids(map))
                .collect()
        }
    }

    /// Gets the length of this body
    pub fn len(&self) -> usize {
        self.parts.len()
    }

    /// Computes the choices for this rule body
    pub fn get_choices(&self) -> Vec<RuleBody> {
        let mut choices = Vec::<RuleBody>::new();
        for part in self.parts.iter() {
            match part.symbol {
                SymbolReference::Action(_id) => {}
                SymbolReference::Virtual(_id) => {}
                SymbolReference::Variable(_id) => {
                    // Append the symbol to all the choices definition
                    for choice in choices.iter_mut() {
                        choice.parts.push(*part);
                    }
                    // Create a new choice with only the symbol
                    choices.push(RuleBody::new_single(*part));
                }
                SymbolReference::Terminal(_id) => {
                    // Append the symbol to all the choices definition
                    for choice in choices.iter_mut() {
                        choice.parts.push(*part);
                    }
                    // Create a new choice with only the symbol
                    choices.push(RuleBody::new_single(*part));
                }
            }
        }
        // Create a new empty choice
        choices.push(RuleBody::new());
        choices
    }
}

impl Index<usize> for RuleBody {
    type Output = RuleBodyElement;
    fn index(&self, index: usize) -> &RuleBodyElement {
        &self.parts[index]
    }
}

/// Represents a grammar rule
#[derive(Clone)]
pub struct Rule {
    /// The rule's head variable
    head: SymbolId,
    /// The rule's body
    body: RuleBody,
    /// Whether this rule has been generated
    generated: bool,
    /// The lexical context pushed by this rule
    context: usize
}

impl Rule {
    /// Creates a new rule
    pub fn new(head: SymbolId, body: RuleBody, generated: bool, context: usize) -> Rule {
        Rule {
            head,
            body,
            generated,
            context
        }
    }

    /// Clones with an updated identifier
    pub fn clone_with_ids(&self, map: &HashMap<SymbolId, SymbolId>) -> Rule {
        Rule {
            head: *map.get(&self.head).unwrap(),
            body: self.body.clone_with_ids(map),
            generated: self.generated,
            context: self.context
        }
    }
}

impl PartialEq for Rule {
    fn eq(&self, other: &Rule) -> bool {
        self.head == other.head && self.body.eq(&other.body)
    }
}

impl Eq for Rule {}
