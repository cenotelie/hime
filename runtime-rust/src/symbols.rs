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

use std::fmt::Display;
use std::fmt::Error;
use std::fmt::Formatter;

use super::text::interface::TextContext;
use super::text::interface::TextPosition;
use super::text::interface::TextSpan;

/// The possible types of symbol
pub enum SymbolType
{
    /// A token, i.e. a piece of text matched by a lexer
    Token,
    /// A variable defined in the original grammar
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
pub struct Symbol {
    /// The symbol's unique identifier
    pub id: u32,
    /// The symbol's name
    pub name: &'static str
}

/// Implementation of `Clone` for `Symbol`
impl Clone for Symbol {
    fn clone(&self) -> Self {
        Symbol {
            id: self.id,
            name: self.name
        }
    }
}

/// Implementation of `Copy` for `Symbol`
impl Copy for Symbol {}

/// Implementation of `Display` for `Symbol`
impl Display for Symbol {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "{}", self.name)
    }
}

/// Represents an element of parsing data
pub trait SemanticElement {
    /// Gets the type of symbol this element represents
    fn get_symbol_type(&self) -> SymbolType;

    /// Gets the position in the input text of this element
    fn get_position(&self) -> TextPosition;

    /// Gets the span in the input text of this element
    fn get_span(&self) -> TextSpan;

    /// Gets the context of this element in the input
    fn get_context(&self) -> TextContext;

    /// Gets the grammar symbol associated to this element
    fn get_symbol(&self) -> Symbol;

    /// Gets the value of this element, if any
    fn get_value(&self) -> Option<String>;
}