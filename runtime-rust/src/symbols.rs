/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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

//! Module for the definition of grammar symbols

use alloc::fmt::{Display, Error, Formatter};

use serde::{Deserialize, Serialize};

use crate::text::{TextContext, TextPosition, TextSpan};
use crate::tokens::Token;

/// The possible types of symbol
#[derive(Copy, Clone, Eq, PartialEq)]
pub enum SymbolType {
    /// A terminal symbol, defined in the original grammar
    Terminal,
    /// A variable symbol, defined in the original grammar
    Variable,
    /// A virtual symbol, defined in the original grammar
    Virtual,
}

/// Symbol ID for inexistant symbol
pub const SID_NOTHING: u32 = 0;
/// Symbol ID of the Epsilon terminal
pub const SID_EPSILON: u32 = 1;
/// Symbol ID of the Dollar terminal
pub const SID_DOLLAR: u32 = 2;

/// Represents a grammar symbol (terminal, variable or virtual)
#[derive(Debug, Copy, Clone, Eq, PartialEq, Serialize, Deserialize)]
pub struct Symbol<'a> {
    /// The symbol's unique identifier
    pub id: u32,
    /// The symbol's name
    pub name: &'a str,
}

/// Implementation of `Display` for `Symbol`
impl<'a> Display for Symbol<'a> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "{}", self.name)
    }
}

/// A trait for a parsing element
pub trait SemanticElementTrait<'s, 'a> {
    /// Gets the position in the input text of this element
    #[must_use]
    fn get_position(&self) -> Option<TextPosition>;

    /// Gets the span in the input text of this element
    #[must_use]
    fn get_span(&self) -> Option<TextSpan>;

    /// Gets the context of this element in the input
    #[must_use]
    fn get_context(&self) -> Option<TextContext<'a>>;

    /// Gets the grammar symbol associated to this element
    #[must_use]
    fn get_symbol(&self) -> Symbol<'s>;

    /// Gets the value of this element, if any
    #[must_use]
    fn get_value(&self) -> Option<&'a str>;
}

/// Represents an element of parsing data
pub enum SemanticElement<'s, 't, 'a> {
    /// A token, i.e. a piece of text matched by a lexer
    Token(Token<'s, 't, 'a>),
    /// A terminal symbol, defined in the original grammar
    Terminal(Symbol<'s>),
    /// A variable symbol defined in the original grammar
    Variable(Symbol<'s>),
    /// A virtual symbol, defined in the original grammar
    Virtual(Symbol<'s>),
}

impl<'s, 't, 'a> SemanticElementTrait<'s, 'a> for SemanticElement<'s, 't, 'a> {
    fn get_position(&self) -> Option<TextPosition> {
        match self {
            SemanticElement::Token(token) => token.get_position(),
            SemanticElement::Terminal(_symbol) => None,
            SemanticElement::Variable(_symbol) => None,
            SemanticElement::Virtual(_symbol) => None,
        }
    }

    fn get_span(&self) -> Option<TextSpan> {
        match self {
            SemanticElement::Token(token) => token.get_span(),
            SemanticElement::Terminal(_symbol) => None,
            SemanticElement::Variable(_symbol) => None,
            SemanticElement::Virtual(_symbol) => None,
        }
    }

    fn get_context(&self) -> Option<TextContext<'a>> {
        match self {
            SemanticElement::Token(token) => token.get_context(),
            SemanticElement::Terminal(_symbol) => None,
            SemanticElement::Variable(_symbol) => None,
            SemanticElement::Virtual(_symbol) => None,
        }
    }

    fn get_symbol(&self) -> Symbol<'s> {
        match self {
            SemanticElement::Token(token) => token.get_symbol(),
            SemanticElement::Terminal(symbol)
            | SemanticElement::Variable(symbol)
            | SemanticElement::Virtual(symbol) => *symbol,
        }
    }

    fn get_value(&self) -> Option<&'a str> {
        match self {
            SemanticElement::Token(token) => token.get_value(),
            SemanticElement::Terminal(_symbol) => None,
            SemanticElement::Variable(_symbol) => None,
            SemanticElement::Virtual(_symbol) => None,
        }
    }
}

impl<'s, 't, 'a> SemanticElement<'s, 't, 'a> {
    /// Gets the type of the associated symbol
    #[must_use]
    pub fn get_symbol_type(&self) -> SymbolType {
        match self {
            SemanticElement::Token(_token) => SymbolType::Terminal,
            SemanticElement::Terminal(_symbol) => SymbolType::Terminal,
            SemanticElement::Variable(_symbol) => SymbolType::Variable,
            SemanticElement::Virtual(_symbol) => SymbolType::Virtual,
        }
    }
}

/// Represents the semantic body of a rule being reduced
pub trait SemanticBody {
    /// Gets the symbol at the i-th index
    #[must_use]
    fn get_element_at(&self, index: usize) -> SemanticElement;

    /// Gets the length of this body
    #[must_use]
    fn length(&self) -> usize;
}

/// Delegate for a user-defined semantic action
pub type SemanticAction = dyn FnMut(Symbol, &dyn SemanticBody);
