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

//! Definition of lexical and syntactic errors for Hime

use text::utf16::Utf16C;
use text::interface::TextPosition;

/// Specifies the type of error
pub enum ParseErrorType {
    /// Lexical error occurring when the end of input has been encountered while more characters were expected
    UnexpectedEndOfInput,
    /// Lexical error occurring when an unexpected character is encountered in the input preventing to match tokens
    UnexpectedChar,
    /// Syntactic error occurring when an unexpected token is encountered by the parser
    UnexpectedToken
}

/// Represents an error in a parser
pub trait ParseError {
    /// Gets the error's type
    fn get_type(&self) -> ParseErrorType;

    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition;

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize;

    /// Gets the error's message
    fn get_message(&self) -> String;
}

/// Represents the unexpected of the input text while more characters were expected
pub struct ParseErrorEndOfInput {
    /// The error's position in the input text
    position: TextPosition
}

impl ParseError for ParseErrorEndOfInput {
    /// Gets the error's type
    fn get_type(&self) -> ParseErrorType {
        ParseErrorType::UnexpectedEndOfInput
    }

    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        *(&self.position)
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

/// Represents an unexpected character error in the input stream of a lexer
pub struct ParseErrorUnexpectedChar {
    /// The error's position in the input text
    position: TextPosition,
    /// The unexpected character
    unexpected: [Utf16C; 2]
}

impl ParseError for ParseErrorUnexpectedChar {
    /// Gets the error's type
    fn get_type(&self) -> ParseErrorType {
        ParseErrorType::UnexpectedChar
    }

    /// Gets the error's position in the input
    fn get_position(&self) -> TextPosition {
        *(&self.position)
    }

    /// Gets the error's length in the input (in number of characters)
    fn get_length(&self) -> usize {
        if self.unexpected[1] == 0x00 { 1 } else { 2 }
    }

    /// Gets the error's message
    fn get_message(&self) -> String {
        let mut result = String::new();
        result.push_str("Unexpected character '");
        if self.unexpected[1] == 0x00 {
            result.push_str(&String::from_utf16(&self.unexpected[0..1]).unwrap());
            result.push_str("' (U+");
            result.push_str(&format!("{:X}", self.unexpected[0]));
        } else {
            let lead = self.unexpected[0] as u32;
            let trail = self.unexpected[1] as u32;
            let cp = ((trail - 0xDC00) | ((lead - 0xD800) << 10)) + 0x10000;
            result.push_str(&String::from_utf16(&self.unexpected).unwrap());
            result.push_str("' (U+");
            result.push_str(&format!("{:X}", cp));
        }
        result.push_str(")");
        result
    }
}