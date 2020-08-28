/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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

//! Module for SDK utilities

use hime_redist::symbols::Symbol;

/// Represents complete data for a parser
#[derive(Clone, Default)]
pub struct InMemoryParser<'a> {
    /// The name of the original grammar
    pub name: &'a str,
    /// The expected terminals
    pub terminals: Vec<Symbol<'a>>,
    /// The variables
    pub variables: Vec<Symbol<'a>>,
    /// The virtuals
    pub virtuals: Vec<Symbol<'a>>,
    /// The identifier of the separator terminal, if any
    pub separator: u32,
    /// The lexer's automaton
    pub lexer_automaton: Vec<u8>,
    /// Whether the lexer is context-sensitive
    pub lexer_is_context_sensitive: bool,
    /// The parser's automaton
    pub parser_automaton: Vec<u8>,
    /// Whether the parser is a RNGLR parser
    pub parser_is_rnglr: bool
}

impl<'a> InMemoryParser<'a> {
    /// Creates a new in-memory parser data
    pub fn new(name: &'a str) -> InMemoryParser<'a> {
        InMemoryParser {
            name,
            terminals: Vec::new(),
            variables: Vec::new(),
            virtuals: Vec::new(),
            separator: 0,
            lexer_automaton: Vec::new(),
            lexer_is_context_sensitive: false,
            parser_automaton: Vec::new(),
            parser_is_rnglr: false
        }
    }
}
