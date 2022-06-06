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

use std::fmt::{Display, Error, Formatter};

use serde_derive::Serialize;

use crate::symbols::Symbol;
use crate::text::{TextPosition, Utf16C};

/// Common trait for data about an error
pub trait ParseErrorDataTrait {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition;

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize;

    /// Gets the error's message
    fn get_message(&self) -> String;
}

/// Represents the unexpected of the input text while more characters were expected
#[derive(Debug, Clone, Serialize)]
pub struct ParseErrorEndOfInput {
    /// The error's position in the input text
    position: TextPosition
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

    /// Gets the error's message
    fn get_message(&self) -> String {
        String::from("Unexpected end of input")
    }
}

impl ParseErrorEndOfInput {
    /// Creates a new error
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
    unexpected: char
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

    /// Gets the error's message
    fn get_message(&self) -> String {
        let mut result = String::new();
        result.push_str("Unexpected character '");
        result.push(self.unexpected);
        result.push_str("' (U+");
        result.push_str(&format!("{:X}", self.unexpected as u32));
        result.push(')');
        result
    }
}

impl ParseErrorUnexpectedChar {
    /// Creates a new error
    pub fn new(position: TextPosition, unexpected: char) -> ParseErrorUnexpectedChar {
        ParseErrorUnexpectedChar {
            position,
            unexpected
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
    sequence: Utf16C
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

    /// Gets the error's message
    fn get_message(&self) -> String {
        let mut result = String::new();
        result.push_str("Incorrect encoding sequence: [");
        if self.missing_high {
            result.push_str("<missing> ");
            result.push_str("0x");
            result.push_str(&format!("{:X}", self.sequence));
        } else {
            result.push_str("0x");
            result.push_str(&format!("{:X}", self.sequence));
            result.push_str(" <missing>");
        }
        result.push(']');
        result
    }
}

impl ParseErrorIncorrectEncodingSequence {
    /// Initializes this error
    pub fn new(
        position: TextPosition,
        missing_high: bool,
        sequence: Utf16C
    ) -> ParseErrorIncorrectEncodingSequence {
        ParseErrorIncorrectEncodingSequence {
            position,
            missing_high,
            sequence
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
    /// The expected terminals
    expected: Vec<Symbol<'s>>
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

    /// Gets the error's message
    fn get_message(&self) -> String {
        let mut result = String::new();
        result.push_str("Unexpected token \"");
        result.push_str(&self.value);
        result.push('"');
        if !self.expected.is_empty() {
            result.push_str("; expected: ");
            for (i, x) in self.expected.iter().enumerate() {
                if i != 0 {
                    result.push_str(", ");
                }
                result.push_str(x.name);
            }
        }
        result
    }
}

impl<'s> ParseErrorUnexpectedToken<'s> {
    /// Initializes this error
    pub fn new(
        position: TextPosition,
        length: usize,
        value: String,
        terminal: Symbol<'s>,
        expected: Vec<Symbol<'s>>
    ) -> ParseErrorUnexpectedToken<'s> {
        ParseErrorUnexpectedToken {
            position,
            length,
            value,
            terminal,
            expected
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
    IncorrectUTF16NoHighSurrogate(ParseErrorIncorrectEncodingSequence)
}

impl<'s> ParseErrorDataTrait for ParseError<'s> {
    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.get_position(),
            ParseError::UnexpectedChar(x) => x.get_position(),
            ParseError::UnexpectedToken(x) => x.get_position(),
            ParseError::IncorrectUTF16NoLowSurrogate(x) => x.get_position(),
            ParseError::IncorrectUTF16NoHighSurrogate(x) => x.get_position()
        }
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.get_length(),
            ParseError::UnexpectedChar(x) => x.get_length(),
            ParseError::UnexpectedToken(x) => x.get_length(),
            ParseError::IncorrectUTF16NoLowSurrogate(x) => x.get_length(),
            ParseError::IncorrectUTF16NoHighSurrogate(x) => x.get_length()
        }
    }

    /// Gets the error's message
    fn get_message(&self) -> String {
        match self {
            ParseError::UnexpectedEndOfInput(x) => x.get_message(),
            ParseError::UnexpectedChar(x) => x.get_message(),
            ParseError::UnexpectedToken(x) => x.get_message(),
            ParseError::IncorrectUTF16NoLowSurrogate(x) => x.get_message(),
            ParseError::IncorrectUTF16NoHighSurrogate(x) => x.get_message()
        }
    }
}

impl<'s> Display for ParseError<'s> {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "@{} {}", self.get_position(), self.get_message())
    }
}

impl<'s> std::error::Error for ParseError<'s> {}

/// Represents an entity that can handle lexical and syntactic errors
#[derive(Default, Clone)]
pub struct ParseErrors<'s> {
    /// The overall errors
    pub errors: Vec<ParseError<'s>>
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
        error: ParseErrorIncorrectEncodingSequence
    ) {
        self.errors
            .push(ParseError::IncorrectUTF16NoLowSurrogate(error));
    }

    /// Handles the incorrect encoding sequence error
    pub fn push_error_no_high_utf16_surrogate(
        &mut self,
        error: ParseErrorIncorrectEncodingSequence
    ) {
        self.errors
            .push(ParseError::IncorrectUTF16NoHighSurrogate(error));
    }
}
