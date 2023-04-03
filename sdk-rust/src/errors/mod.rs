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

// #[cfg(feature = "print_errors")]
pub mod print;

use std::fmt::{Display, Formatter};
use std::io;

use crate::grammars::{TerminalRef, OPTION_AXIOM, OPTION_SEPARATOR};
use crate::lr::{Conflict, ConflictKind, ContextError};
use crate::{InputReference, LoadedData};

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
    UnsupportedNonPlane0InCharacterClass(InputReference, char),
    /// The specified value is not a valid unicode code point
    InvalidCodePoint(InputReference, u32),
    /// A terminal override a previous definition
    OverridingPreviousTerminal(InputReference, String, InputReference),
    /// The inherited grammar cannot be found
    GrammarNotDefined(InputReference, String),
    /// A conflict in a grammar
    LrConflict(usize, Box<Conflict>),
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

impl Display for Error {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match self {
            Self::Io(e) => e.fmt(f),
            Self::Msg(msg) => write!(f, "{}", msg),
            Self::Parsing(_input, msg) => write!(f, "{}", msg),
            Self::GrammarNotSpecified => write!(f, "The target grammar was not specified"),
            Self::GrammarNotFound(name) => write!(f, "Cannot find grammar `{}`", name),
            Self::InvalidOption(_grammar_index, name, _valid) => {
                write!(f, "Invalid value for grammar option `{}`", name)
            }
            Self::AxiomNotSpecified(_grammar_index) => {
                write!(f, "Grammar axiom has not been specified")
            }
            Error::AxiomNotDefined(_grammar_index) => write!(f, "Grammar axiom is not defined"),
            Error::SeparatorNotDefined(_grammar_index) => {
                write!(f, "Grammar separator token is not defined",)
            }
            Error::SeparatorIsContextual(_grammar_index, _terminal_ref) => {
                write!(f, "Grammar separator token is only defined for a context",)
            }
            Error::SeparatorCannotBeMatched(_grammar_index, _error) => {
                write!(f, "Token is expected but can never be matched")
            }
            Self::TemplateRuleNotFound(_input, name) => {
                write!(f, "Cannot find template rule `{}`", name)
            }
            Self::TemplateRuleWrongNumberOfArgs(_input, expected, provided) => write!(
                f,
                "Template expected {} arguments, {} given",
                expected, provided
            ),
            Self::SymbolNotFound(_input, name) => write!(f, "Cannot find symbol `{}`", name),
            Self::InvalidCharacterSpan(_input) => {
                write!(f, "Invalid character span, swap left and right bounds")
            }
            Self::UnknownUnicodeBlock(_input, name) => {
                write!(f, "Unknown unicode block `{}`", name)
            }
            Self::UnknownUnicodeCategory(_input, name) => {
                write!(f, "Unknown unicode category `{}`", name)
            }
            Self::UnsupportedNonPlane0InCharacterClass(_input, c) => write!(
                f,
                "Unsupported non-plane 0 Unicode character {} (U+{:04X}) in character class",
                *c,
                u32::from(*c)
            ),
            Self::InvalidCodePoint(_input, c) => write!(
                f,
                "The value U+{:0X} is not a supported unicode code point",
                c
            ),
            Self::OverridingPreviousTerminal(_input, name, _previous) => {
                write!(f, "Overriding the previous definition of `{}`", name)
            }
            Self::GrammarNotDefined(_input, name) => {
                write!(f, "Grammar `{}` is not defined", name)
            }
            Self::LrConflict(_grammar_index, conflict) => {
                write!(
                    f,
                    "{} conflict, cannot decide what to do",
                    match conflict.kind {
                        ConflictKind::ShiftReduce => "Shift/Reduce",
                        ConflictKind::ReduceReduce => "Reduce/Reduce"
                    }
                )
            }
            Self::TerminalOutsideContext(_grammar_index, _error) => {
                write!(f, "Contextual terminal is expected outside its context")
            }
            Self::TerminalCannotBeMatched(_grammar_index, _error) => {
                write!(f, "Token is expected but can never be matched",)
            }
            Self::TerminalMatchesEmpty(_grammar_index, _terminal_ref) => {
                write!(f, "Terminal matches empty string, which is not allowed",)
            }
        }
    }
}

impl std::error::Error for Error {
    fn source(&self) -> Option<&(dyn std::error::Error + 'static)> {
        match self {
            Self::Io(e) => Some(e),
            _ => None
        }
    }
}

impl Error {
    /// Transform into this error into one with its context
    pub fn with_context<'context, 'error, 't>(
        &'error self,
        context: &'context LoadedData<'t>
    ) -> ContextualizedError<'context, 'error, 't> {
        ContextualizedError {
            context,
            error: self
        }
    }
}

/// An error associated to its contextual data
#[derive(Debug)]
pub struct ContextualizedError<'context, 'error, 't> {
    /// The contextual data
    pub context: &'context LoadedData<'t>,
    /// The error itself
    pub error: &'error Error
}

impl<'context, 'error, 't> Display for ContextualizedError<'context, 'error, 't> {
    fn fmt(&self, f: &mut Formatter<'_>) -> std::fmt::Result {
        match &self.error {
            Error::Io(err) => err.fmt(f),
            Error::Msg(msg) => write!(f, "{}", msg),
            Error::Parsing(_input, msg) => write!(f, "{}", msg),
            Error::GrammarNotSpecified => write!(f, "The target grammar was not specified"),
            Error::GrammarNotFound(name) => write!(f, "Cannot find grammar `{}`", name),
            Error::InvalidOption(_grammar_index, name, _valid) => {
                write!(f, "Invalid value for grammar option `{}`", name)
            }
            Error::AxiomNotSpecified(_grammar_index) => {
                write!(f, "Grammar axiom has not been specified")
            }
            Error::AxiomNotDefined(grammar_index) => {
                let option = self.context.grammars[*grammar_index]
                    .get_option(OPTION_AXIOM)
                    .unwrap();
                write!(f, "Grammar axiom `{}` is not defined", &option.value)
            }
            Error::SeparatorNotDefined(grammar_index) => {
                let option = self.context.grammars[*grammar_index]
                    .get_option(OPTION_SEPARATOR)
                    .unwrap();
                write!(
                    f,
                    "Grammar separator token `{}` is not defined",
                    &option.value
                )
            }
            Error::SeparatorIsContextual(grammar_index, terminal_ref) => {
                let separator = self.context.grammars[*grammar_index]
                    .get_terminal(terminal_ref.sid())
                    .unwrap();
                write!(
                    f,
                    "Grammar separator token `{}` is only defined for context `{}`",
                    &separator.name,
                    &self.context.grammars[*grammar_index].contexts[separator.context]
                )
            }
            Error::SeparatorCannotBeMatched(grammar_index, error) => {
                let terminal = self.context.grammars[*grammar_index]
                    .get_terminal(error.terminal.sid())
                    .unwrap();
                write!(
                    f,
                    "Token `{}` is expected but can never be matched",
                    &terminal.value
                )
            }
            Error::TemplateRuleNotFound(_input, name) => {
                write!(f, "Cannot find template rule `{}`", name)
            }
            Error::TemplateRuleWrongNumberOfArgs(_input, expected, provided) => write!(
                f,
                "Template expected {} arguments, {} given",
                expected, provided
            ),
            Error::SymbolNotFound(_input, name) => write!(f, "Cannot find symbol `{}`", name),
            Error::InvalidCharacterSpan(_input) => {
                write!(f, "Invalid character span, swap left and right bounds")
            }
            Error::UnknownUnicodeBlock(_input, name) => {
                write!(f, "Unknown unicode block `{}`", name)
            }
            Error::UnknownUnicodeCategory(_input, name) => {
                write!(f, "Unknown unicode category `{}`", name)
            }
            Error::UnsupportedNonPlane0InCharacterClass(_input, c) => write!(
                f,
                "Unsupported non-plane 0 Unicode character {} (U+{:04X}) in character class",
                *c,
                u32::from(*c)
            ),
            Error::InvalidCodePoint(_input, c) => write!(
                f,
                "The value U+{:0X} is not a supported unicode code point",
                c
            ),
            Error::OverridingPreviousTerminal(_input, name, _previous) => {
                write!(f, "Overriding the previous definition of `{}`", name)
            }
            Error::GrammarNotDefined(_input, name) => {
                write!(f, "Grammar `{}` is not defined", name)
            }
            Error::LrConflict(grammar_index, conflict) => {
                let grammar = &self.context.grammars[*grammar_index];
                let terminal = grammar.get_symbol_value(conflict.lookahead.terminal.into());
                write!(
                    f,
                    "{} conflict, cannot decide what to do facing `{}`",
                    match conflict.kind {
                        ConflictKind::ShiftReduce => "Shift/Reduce",
                        ConflictKind::ReduceReduce => "Reduce/Reduce"
                    },
                    terminal
                )
            }
            Error::TerminalOutsideContext(grammar_index, error) => {
                let grammar = &self.context.grammars[*grammar_index];
                let terminal = grammar.get_symbol_value(error.terminal.into());
                write!(
                    f,
                    "Contextual terminal `{}` is expected outside its context",
                    terminal
                )
            }
            Error::TerminalCannotBeMatched(grammar_index, error) => {
                let terminal = self.context.grammars[*grammar_index]
                    .get_terminal(error.terminal.sid())
                    .unwrap();
                write!(
                    f,
                    "Token `{}` is expected but can never be matched",
                    &terminal.value
                )
            }
            Error::TerminalMatchesEmpty(grammar_index, terminal_ref) => {
                let terminal = self.context.grammars[*grammar_index]
                    .get_terminal(terminal_ref.sid())
                    .unwrap();
                write!(
                    f,
                    "Terminal `{}` matches empty string, which is not allowed",
                    &terminal.name
                )
            }
        }
    }
}

impl<'context, 'error, 't> std::error::Error for ContextualizedError<'context, 'error, 't> {
    fn source(&self) -> Option<&(dyn std::error::Error + 'static)> {
        self.error.source()
    }
}

/// A collection of errors
#[derive(Debug)]
pub struct Errors<'t> {
    /// The associated context
    pub context: LoadedData<'t>,
    /// The errors
    pub errors: Vec<Error>
}

impl<'t> Errors<'t> {
    /// Encapsulate the errors
    pub fn from(context: LoadedData, errors: Vec<Error>) -> Errors {
        Errors { context, errors }
    }

    /// Transforms into an owned static version of the data
    pub fn into_static(self) -> Errors<'static> {
        errors_into_static(self)
    }
}

impl<'t> Display for Errors<'t> {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{} errors", self.errors.len())
    }
}

impl<'t> std::error::Error for Errors<'t> {
    fn source(&self) -> Option<&(dyn std::error::Error + 'static)> {
        if self.errors.is_empty() {
            None
        } else {
            Some(&self.errors[0])
        }
    }
}

/// Transforms into an owned static version of the data
fn errors_into_static<'t>(errors: Errors<'t>) -> Errors<'static> {
    Errors {
        context: errors.context.into_static(),
        errors: errors.errors
    }
}
