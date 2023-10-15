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

//! Module for the definition of lexical and syntactic errors

use core::fmt::{Display, Formatter};
use alloc::string::String;
use alloc::vec::Vec;

use serde::Serialize;

use crate::symbols::Symbol;
use crate::text::{TextPosition, Utf16C};

/// Common trait for data about an error
pub trait ParseErrorDataTrait: Display {
    /// Gets the error's position in the input
    #[must_use]
    fn get_position(&self) -> TextPosition;

    /// Gets the error's length in the input (in number of characters)
    #[must_use]
    fn get_length(&self) -> usize;
}

/// Represents the unexpected of the input text while more characters were expected
#[derive(Debug, Clone, Serialize)]
pub struct ParseErrorEndOfInput {
    /// The error's position in the input text
    position: TextPosition,
}

impl ParseErrorDataTrait for ParseErrorEndOfInput {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        self.position
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        0
    }
}

impl Display for ParseErrorEndOfInput {
    fn fmt(&self, f: &mut Formatter<'_>) -> core::fmt::Result {
        write!(f, "Unexpected end of input")
    }
}

impl ParseErrorEndOfInput {
    /// Creates a new error
    #[must_use]
    pub fn new(position: TextPosition) -> ParseErrorEndOfInput {
        ParseErrorEndOfInput { position }
    }
}

/// Represents an unexpected character error in the input stream of a lexer
#[derive(Debug, Clone, Serialize)]
pub struct ParseErrorUnexpectedChar {
    /// The error's position in the input text
    position: TextPosition,
    /// The unexpected character
    unexpected: char,
}

impl ParseErrorDataTrait for ParseErrorUnexpectedChar {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        self.position
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        self.unexpected.len_utf8()
    }
}

impl Display for ParseErrorUnexpectedChar {
    fn fmt(&self, f: &mut Formatter<'_>) -> core::fmt::Result {
        write!(
            f,
            "Unexpected character '{}' (U+{:X})",
            self.unexpected, self.unexpected as u32
        )
    }
}

impl ParseErrorUnexpectedChar {
    /// Creates a new error
    #[must_use]
    pub fn new(position: TextPosition, unexpected: char) -> ParseErrorUnexpectedChar {
        ParseErrorUnexpectedChar {
            position,
            unexpected,
        }
    }
}

/// Represents an incorrect encoding sequence error in the input of a lexer
/// This kind of error cannot really be produced by the Rust runtime
#[derive(Debug, Clone, Serialize)]
pub struct ParseErrorIncorrectEncodingSequence {
    /// The error's position in the input text
    position: TextPosition,
    /// The precise error type
    #[serde(rename = "missingHigh")]
    missing_high: bool,
    /// The incorrect sequence
    sequence: Utf16C,
}

impl ParseErrorDataTrait for ParseErrorIncorrectEncodingSequence {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        self.position
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        1
    }
}

impl Display for ParseErrorIncorrectEncodingSequence {
    fn fmt(&self, f: &mut Formatter<'_>) -> core::fmt::Result {
        write!(f, "Incorrect encoding sequence: [")?;
        if self.missing_high {
            write!(f, "<missing> 0x{:X}", self.sequence)?;
        } else {
            write!(f, "0x{:X} <missing>", self.sequence)?;
        }
        write!(f, "]")?;
        Ok(())
    }
}

impl ParseErrorIncorrectEncodingSequence {
    /// Initializes this error
    #[must_use]
    pub fn new(
        position: TextPosition,
        missing_high: bool,
        sequence: Utf16C,
    ) -> ParseErrorIncorrectEncodingSequence {
        ParseErrorIncorrectEncodingSequence {
            position,
            missing_high,
            sequence,
        }
    }
}

/// Represents an unexpected token error in a parser
#[derive(Debug, Clone, Serialize)]
pub struct ParseErrorUnexpectedToken<'s> {
    /// The error's position in the input text
    position: TextPosition,
    /// The error's length in the input
    length: usize,
    /// The value for the unexpected token
    value: String,
    /// The terminal symbol for the unexpected token
    terminal: Symbol<'s>,
    /// The identifier of the current states
    #[cfg(feature = "debug")]
    state_ids: Vec<u32>,
    /// The expected terminals
    expected: Vec<Symbol<'s>>,
}

impl<'s> ParseErrorDataTrait for ParseErrorUnexpectedToken<'s> {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        self.position
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        self.length
    }
}

impl<'s> Display for ParseErrorUnexpectedToken<'s> {
    fn fmt(&self, f: &mut Formatter<'_>) -> core::fmt::Result {
        write!(f, "Unexpected token \"{}\"", self.value)?;
        #[cfg(feature = "debug")]
        {
            write!(
                f,
                " at state(s) {}",
                self.state_ids
                    .iter()
                    .map(alloc::string::ToString::to_string)
                    .collect::<Vec<_>>()
                    .join(",")
            )?;
        }
        if !self.expected.is_empty() {
            write!(f, "; expected: ")?;
            for (i, x) in self.expected.iter().enumerate() {
                if i != 0 {
                    write!(f, ", ")?;
                }
                write!(f, "{}", x.name)?;
            }
        }
        Ok(())
    }
}

impl<'s> ParseErrorUnexpectedToken<'s> {
    /// Initializes this error
    #[must_use]
    pub fn new(
        position: TextPosition,
        length: usize,
        value: String,
        terminal: Symbol<'s>,
        #[cfg(feature = "debug")] state_ids: Vec<u32>,
        expected: Vec<Symbol<'s>>,
    ) -> ParseErrorUnexpectedToken<'s> {
        ParseErrorUnexpectedToken {
            position,
            length,
            value,
            terminal,
            #[cfg(feature = "debug")]
            state_ids,
            expected,
        }
    }
}

/// Represents a lexical or syntactic error
#[derive(Debug, Clone, Serialize)]
#[serde(tag = "type")]
pub enum ParseError<'s> {
    /// Lexical error occurring when the end of input has been encountered while more characters were expected
    UnexpectedEndOfInput(ParseErrorEndOfInput),
    /// Lexical error occurring when an unexpected character is encountered in the input preventing to match tokens
    UnexpectedChar(ParseErrorUnexpectedChar),
    /// Syntactic error occurring when an unexpected token is encountered by the parser
    UnexpectedToken(ParseErrorUnexpectedToken<'s>),
    /// Lexical error occurring when the low surrogate encoding point is missing in a UTF-16 encoding sequence with an expected high and low surrogate pair
    IncorrectUTF16NoLowSurrogate(ParseErrorIncorrectEncodingSequence),
    /// Lexical error occurring when the high surrogate encoding point is missing in a UTF-16 encoding sequence with an expected high and low surrogate pair
    IncorrectUTF16NoHighSurrogate(ParseErrorIncorrectEncodingSequence),
}

impl<'s> ParseErrorDataTrait for ParseError<'s> {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.get_position(),
            ParseError::UnexpectedChar(x) => x.get_position(),
            ParseError::UnexpectedToken(x) => x.get_position(),
            ParseError::IncorrectUTF16NoLowSurrogate(x)
            | ParseError::IncorrectUTF16NoHighSurrogate(x) => x.get_position(),
        }
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.get_length(),
            ParseError::UnexpectedChar(x) => x.get_length(),
            ParseError::UnexpectedToken(x) => x.get_length(),
            ParseError::IncorrectUTF16NoLowSurrogate(x)
            | ParseError::IncorrectUTF16NoHighSurrogate(x) => x.get_length(),
        }
    }
}

impl<'s> Display for ParseError<'s> {
    fn fmt(&self, f: &mut Formatter<'_>) -> core::fmt::Result {
        // write!(f, "@{} ", self.get_position())?;
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.fmt(f),
            ParseError::UnexpectedChar(x) => x.fmt(f),
            ParseError::UnexpectedToken(x) => x.fmt(f),
            ParseError::IncorrectUTF16NoLowSurrogate(x)
            | ParseError::IncorrectUTF16NoHighSurrogate(x) => x.fmt(f),
        }
    }
}

#[cfg(feature = "std")]
impl<'s> std::error::Error for ParseError<'s> {}

/// Represents an entity that can handle lexical and syntactic errors
#[derive(Debug, Default, Clone)]
pub struct ParseErrors<'s> {
    /// The overall errors
    pub errors: Vec<ParseError<'s>>,
}

impl<'s> ParseErrors<'s> {
    /// Handles the end-of-input error
    pub fn push_error_eoi(&mut self, error: ParseErrorEndOfInput) {
        self.errors.push(ParseError::UnexpectedEndOfInput(error));
    }

    /// Handles the unexpected character error
    pub fn push_error_unexpected_char(&mut self, error: ParseErrorUnexpectedChar) {
        self.errors.push(ParseError::UnexpectedChar(error));
    }

    /// Handles the unexpected token error
    pub fn push_error_unexpected_token(&mut self, error: ParseErrorUnexpectedToken<'s>) {
        self.errors.push(ParseError::UnexpectedToken(error));
    }

    /// Handles the incorrect encoding sequence error
    pub fn push_error_no_low_utf16_surrogate(
        &mut self,
        error: ParseErrorIncorrectEncodingSequence,
    ) {
        self.errors
            .push(ParseError::IncorrectUTF16NoLowSurrogate(error));
    }

    /// Handles the incorrect encoding sequence error
    pub fn push_error_no_high_utf16_surrogate(
        &mut self,
        error: ParseErrorIncorrectEncodingSequence,
    ) {
        self.errors
            .push(ParseError::IncorrectUTF16NoHighSurrogate(error));
    }
}
