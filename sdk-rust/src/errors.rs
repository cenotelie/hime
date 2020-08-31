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

//! Module for the management of errors in the SDK

use crate::grammars::TerminalRef;
use crate::lr::{Conflict, ContextError};
use crate::{InputReference, LoadedData};
use std::io;

/// Represents an error where a token is used by cannot be produced by the lexer
#[derive(Debug, Clone)]
pub struct UnmatchableTokenError {
    /// The problematic terminal
    pub terminal: TerminalRef,
    /// The terminals that override the problematic one
    pub overriders: Vec<TerminalRef>
}

/// The global error type
#[derive(Debug)]
pub enum Error {
    /// An IO error (file not found, etc.)
    Io(io::Error),
    /// A simple message
    Msg(String),
    /// Parsing error
    Parsing(InputReference, String),
    /// The target grammar was not specified
    GrammarNotSpecified,
    /// The specified grammar was not found
    GrammarNotFound(String),
    /// The value for the option is invalid
    /// (grammar_index, option_name, valid_options)
    InvalidOption(usize, String, Vec<String>),
    /// The grammar's axiom has not been specified in the options
    /// (grammar_index)
    AxiomNotSpecified(usize),
    /// The grammar's axiom is not defined (does not exist)
    /// (grammar_index)
    AxiomNotDefined(usize),
    /// The separator token specified by a grammar is not defined
    /// (grammar_index)
    SeparatorNotDefined(usize),
    /// The separator token is contextual
    /// (grammar_index, separator)
    SeparatorIsContextual(usize, TerminalRef),
    /// The separator token cannot be matched, it may be overriden by others
    /// (grammar_index, separator, overriders)
    SeparatorCannotBeMatched(usize, UnmatchableTokenError),
    /// The template rule could not be found
    TemplateRuleNotFound(InputReference, String),
    /// When instantiating a template rule, the wrong number of arguments were supplied (expected, supplied)
    TemplateRuleWrongNumberOfArgs(InputReference, usize, usize),
    /// The specifiec symbol was not found
    SymbolNotFound(InputReference, String),
    /// Invalid character span
    InvalidCharacterSpan(InputReference),
    /// The unicode block is not known
    UnknownUnicodeBlock(InputReference, String),
    /// The unicode category is not known
    UnknownUnicodeCategory(InputReference, String),
    /// A unicode character not in plane 0 was used in a character class, which is not supported
    UnsupportedNonPlane0InCharacterClass(InputReference, usize),
    /// The specified value is not a valid unicode code point
    InvalidCodePoint(InputReference, u32),
    /// A terminal override a previous definition
    OverridingPreviousTerminal(InputReference, String),
    /// The inherited grammar cannot be found
    GrammarNotDefined(InputReference, String),
    /// A conflict in a grammar
    LrConflict(usize, Conflict),
    /// A contextual terminal is used outside of its context
    TerminalOutsideContext(usize, ContextError),
    /// A terminal is used by the parser but cannot be produced by the lexer
    TerminalCannotBeMatched(usize, UnmatchableTokenError),
    /// A terminal matches the empty string
    /// (grammar_index, terminal)
    TerminalMatchesEmpty(usize, TerminalRef)
}

impl From<io::Error> for Error {
    fn from(err: io::Error) -> Self {
        Error::Io(err)
    }
}

/// A collection of errors
#[derive(Debug)]
pub struct Errors {
    /// The associated data
    pub data: LoadedData,
    /// The errors
    pub errors: Vec<Error>
}

impl Errors {
    /// Encapsulate the errors
    pub fn from(data: LoadedData, errors: Vec<Error>) -> Errors {
        Errors { data, errors }
    }
}
