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

use std::fmt::Display;
use std::fmt::Error;
use std::fmt::Formatter;

use super::text::TextContext;
use super::text::TextPosition;
use super::text::TextSpan;
use super::tokens::Token;

/// The possible types of symbol
#[derive(Copy, Clone, Eq, PartialEq)]
pub enum SymbolType {
    /// A terminal symbol, defined in the original grammar
    Terminal,
    /// A variable symbol, defined in the original grammar
    Variable,
    /// A virtual symbol, defined in the original grammar
    Virtual
}

/// Symbol ID for inexistant symbol
pub const SID_NOTHING: u32 = 0;
/// Symbol ID of the Epsilon terminal
pub const SID_EPSILON: u32 = 1;
/// Symbol ID of the Dollar terminal
pub const SID_DOLLAR: u32 = 2;

/// Represents a grammar symbol (terminal, variable or virtual)
#[derive(Copy, Clone, Eq, PartialEq)]
pub struct Symbol {
    /// The symbol's unique identifier
    pub id: u32,
    /// The symbol's name
    pub name: &'static str
}

/// Implementation of `Display` for `Symbol`
impl Display for Symbol {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "{}", self.name)
    }
}

/// A trait for a parsing element
pub trait SemanticElementTrait {
    /// Gets the position in the input text of this element
    fn get_position(&self) -> Option<TextPosition>;

    /// Gets the span in the input text of this element
    fn get_span(&self) -> Option<TextSpan>;

    /// Gets the context of this element in the input
    fn get_context(&self) -> Option<TextContext>;

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol;

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<String>;
}

/// Represents an element of parsing data
pub enum SemanticElement<'a> {
    /// A token, i.e. a piece of text matched by a lexer
    Token(Token<'a>),
    /// A terminal symbol, defined in the original grammar
    Terminal(Symbol),
    /// A variable symbol defined in the original grammar
    Variable(Symbol),
    /// A virtual symbol, defined in the original grammar
    Virtual(Symbol)
}

impl<'a> SemanticElementTrait for SemanticElement<'a> {
    fn get_position(&self) -> Option<TextPosition> {
        match self {
            &SemanticElement::Token(ref token) => token.get_position(),
            &SemanticElement::Terminal(ref _symbol) => None,
            &SemanticElement::Variable(ref _symbol) => None,
            &SemanticElement::Virtual(ref _symbol) => None
        }
    }

    fn get_span(&self) -> Option<TextSpan> {
        match self {
            &SemanticElement::Token(ref token) => token.get_span(),
            &SemanticElement::Terminal(ref _symbol) => None,
            &SemanticElement::Variable(ref _symbol) => None,
            &SemanticElement::Virtual(ref _symbol) => None
        }
    }

    fn get_context(&self) -> Option<TextContext> {
        match self {
            &SemanticElement::Token(ref token) => token.get_context(),
            &SemanticElement::Terminal(ref _symbol) => None,
            &SemanticElement::Variable(ref _symbol) => None,
            &SemanticElement::Virtual(ref _symbol) => None
        }
    }

    fn get_symbol(&self) -> Symbol {
        match self {
            &SemanticElement::Token(ref token) => token.get_symbol(),
            &SemanticElement::Terminal(ref symbol) => *symbol,
            &SemanticElement::Variable(ref symbol) => *symbol,
            &SemanticElement::Virtual(ref symbol) => *symbol
        }
    }

    fn get_value(&self) -> Option<String> {
        match self {
            &SemanticElement::Token(ref token) => token.get_value(),
            &SemanticElement::Terminal(ref _symbol) => None,
            &SemanticElement::Variable(ref _symbol) => None,
            &SemanticElement::Virtual(ref _symbol) => None
        }
    }
}

impl<'a> SemanticElement<'a> {
    /// Gets the type of the associated symbol
    pub fn get_symbol_type(&self) -> SymbolType {
        match self {
            &SemanticElement::Token(ref _token) => SymbolType::Terminal,
            &SemanticElement::Terminal(ref _symbol) => SymbolType::Terminal,
            &SemanticElement::Variable(ref _symbol) => SymbolType::Variable,
            &SemanticElement::Virtual(ref _symbol) => SymbolType::Virtual
        }
    }
}

/// Represents the semantic body of a rule being reduced
pub trait SemanticBody {
    /// Gets the symbol at the i-th index
    fn get_element_at(&self, index: usize) -> SemanticElement;

    /// Gets the length of this body
    fn length(&self) -> usize;
}

/// Delegate for a user-defined semantic action
pub type SemanticAction = FnMut(Symbol, &SemanticBody);
