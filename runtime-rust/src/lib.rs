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

pub mod utils;
pub mod lexer;

/// Represents a UTF-16 encoding point
type Utf16C = u16;

/// Represents a span of text in an input as a starting index and length
pub struct TextSpan {
    /// The starting index
    index: usize,
    /// The length
    length: usize
}

/// Implementation of `Clone` for `TextSpan`
impl ::std::clone::Clone for TextSpan {
    fn clone(&self) -> Self {
        TextSpan {
            index: self.index,
            length: self.length
        }
    }
}

/// Implementation of `Copy` for `TextSpan`
impl ::std::marker::Copy for TextSpan {}

/// Implementation of `Display` for `TextSpan`
impl ::std::fmt::Display for TextSpan {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> Result<(), ::std::fmt::Error> {
        write!(f, "@{}+{}", self.index, self.length)
    }
}

/// Represents a position in term of line and column in a text input
pub struct TextPosition {
    /// The line number
    line: usize,
    /// The column number
    column: usize
}

/// Implementation of `Clone` for `TextPosition`
impl ::std::clone::Clone for TextPosition {
    fn clone(&self) -> Self {
        TextPosition {
            line: self.line,
            column: self.column
        }
    }
}

/// Implementation of `Copy` for `TextPosition`
impl ::std::marker::Copy for TextPosition {}

/// Implementation of `Display` for `TextPosition`
impl ::std::fmt::Display for TextPosition {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> Result<(), ::std::fmt::Error> {
        write!(f, "({}, {})", self.line, self.column)
    }
}

/// Represents the context description of a position in a piece of text.
/// A context is composed of two pieces of text, the line content and the pointer.
/// For example, given the piece of text:
/// "public Struct Context"
/// A context pointing to the second word will look like:
/// content = "public Struct Context"
/// pointer = "       ^"
pub struct TextContext {
    /// The text content being represented
    content: String,
    /// The pointer textual representation
    pointer: String
}


/// Represents the input of parser with some metadata for line endings
/// All line numbers and column numbers are 1-based.
/// Indices in the content are 0-based.
pub trait Text {
    /// Gets the number of lines
    fn get_line_count(&self) -> usize;

    /// Gets the size in number of characters
    fn get_size(&self) -> usize;

    /// Gets whether the specified index is after the end of the text represented by this object
    fn is_end(&self, index: usize) -> bool;

    /// Gets the character at the specified index
    fn get_at(&self, index: usize) -> Utf16C;

    /// Gets the substring beginning at the given index with the given length
    fn get_value(&self, index: usize, length: usize) -> String;

    /// Get the substring corresponding to the specified span
    fn get_value_for(&self, span: TextSpan) -> String {
        self.get_value(span.index, span.length)
    }

    /// Gets the starting index of the i-th line
    fn get_line_index(&self, line: usize) -> usize;

    /// Gets the length of the i-th line
    fn get_line_length(&self, line: usize) -> usize;

    /// Gets the string content of the i-th line
    fn get_line_content(&self, line: usize) -> String {
        self.get_value(self.get_line_index(line), self.get_line_length(line))
    }

    /// Gets the position at the given index
    fn get_position_at(&self, index: usize) -> TextPosition;

    /// Gets the context description for the current text at the specified position
    fn get_context_at(&self, position: TextPosition) -> TextContext {
        self.get_context_for(position, 1)
    }

    /// Gets the context description for the current text at the specified position
    fn get_context_for(&self, position: TextPosition, length: usize) -> TextContext;

    /// Gets the context description for the current text at the specified span
    fn get_context_of(&self, span: TextSpan) -> TextContext {
        let position = self.get_position_at(span.index);
        return self.get_context_for(position, span.length);
    }
}

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
struct ParseErrorEndOfInput {
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
struct ParseErrorUnexpectedChar {
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
    id: u32,
    /// The symbol's name
    name: &'static str
}

impl Symbol {
    /// Gets the symbol's unique identifier
    fn get_id(&self) -> u32 {
        self.id
    }

    /// Gets the symbol's name
    fn get_name(&self) -> &str {
        self.name
    }
}

/// Implementation of `Display` for `Symbol`
impl ::std::fmt::Display for Symbol {
    fn fmt(&self, f: &mut ::std::fmt::Formatter) -> Result<(), ::std::fmt::Error> {
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

/// Represents the metadata of a token
struct TokenRepositoryCell {
    terminal: usize,
    span: TextSpan
}

/// A repository of matched tokens
struct TokenRepository {}

pub struct Token {}