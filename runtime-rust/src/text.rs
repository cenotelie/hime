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

//! Module for text-handling APIs

use alloc::borrow::Cow;
use alloc::fmt::{Display, Error, Formatter};
use alloc::string::{String, ToString};
use alloc::vec::Vec;
use core::cmp::{Ord, Ordering};
// use alloc::io::Read;
use core::str::Chars;

use serde::{Deserialize, Serialize};

/// `Utf16C` represents a single UTF-16 code unit.
/// A UTF-16 code unit is always represented as a 16 bits unsigned integer.
/// UTF-16 code units may not represent by themselves valid Unicode code points (characters).
/// A Unicode code point (a character) is a 32-bits unsigned integer in the ranges:
/// U+0000 to U+D7FF and U+E000 to U+FFFF and U+10000 to U+10FFFF.
/// Unicode code points in the range U+D800 to U+DFFF are reserved and cannot be used.
/// UTF-16 can be used to encode a single Unicode code point in either one or two UTF-16 code units.
///
/// See [UTF-16](https://en.wikipedia.org/wiki/UTF-16) for more details.
pub type Utf16C = u16;

/// Represents a span of text in an input as a starting index and length
#[derive(Debug, Default, Copy, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct TextSpan {
    /// The starting index
    pub index: usize,
    /// The length
    pub length: usize,
}

impl PartialOrd for TextSpan {
    fn partial_cmp(&self, other: &TextSpan) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for TextSpan {
    fn cmp(&self, other: &TextSpan) -> Ordering {
        match self.index.cmp(&other.index) {
            Ordering::Equal => self.length.cmp(&other.length),
            o => o,
        }
    }
}

/// Implementation of `Display` for `TextSpan`
impl Display for TextSpan {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "@{}+{}", self.index, self.length)
    }
}

/// Represents a position in term of line and column in a text input
#[derive(Debug, Default, Copy, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct TextPosition {
    /// The 1-base line number
    pub line: usize,
    /// The 1-base column number
    pub column: usize,
}

impl PartialOrd for TextPosition {
    fn partial_cmp(&self, other: &TextPosition) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

impl Ord for TextPosition {
    fn cmp(&self, other: &TextPosition) -> Ordering {
        match self.line.cmp(&other.line) {
            Ordering::Equal => self.column.cmp(&other.column),
            o => o,
        }
    }
}

/// Implementation of `Display` for `TextPosition`
impl Display for TextPosition {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "({}, {})", self.line, self.column)
    }
}

/// Represents the context description of a position in a piece of text.
/// A context is composed of two pieces of text, the line content and the pointer.
/// For example, given the piece of text:
///
/// ```hime
/// public Struct Context
/// ```
///
/// A context pointing to the second word will look like:
///
/// ```hime
/// content = "public Struct Context"
/// pointer = "       ^^^^^^"
/// ```
#[derive(Debug, Default, Clone, PartialEq, Eq)]
pub struct TextContext<'a> {
    /// The text content being represented
    pub content: &'a str,
    /// The pointer textual representation
    pub pointer: String,
}

/// Represents the input of parser with some metadata for line endings
/// All line numbers and column numbers are 1-based.
/// Indices in the content are 0-based.
#[derive(Debug, Clone)]
pub struct Text<'a> {
    /// The full content of the input
    content: Cow<'a, str>,
    /// Cache of the starting indices of each line within the text
    lines: Vec<usize>,
}

impl<'a> Text<'a> {
    /// Transforms into an owned static version of the data
    #[must_use]
    pub fn into_static(self) -> Text<'static> {
        Text {
            content: Cow::Owned(self.content.to_string()),
            lines: self.lines,
        }
    }

    /// Initializes this text
    #[allow(clippy::should_implement_trait)]
    #[must_use]
    pub fn from_str(content: &'a str) -> Text<'a> {
        let lines = find_lines_in(content.char_indices());
        Text {
            content: Cow::Borrowed(content),
            lines,
        }
    }

    /// Initializes this text
    #[must_use]
    pub fn from_string(content: String) -> Text<'static> {
        let lines = find_lines_in(content.char_indices());
        Text {
            content: Cow::Owned(content),
            lines,
        }
    }

    /// Initializes this text from a UTF-8 stream
    ///
    /// # Errors
    ///
    /// Return an error when reading the input fails.
    #[cfg(feature = "std")]
    pub fn from_utf8_stream(
        input: &mut dyn std::io::Read,
    ) -> Result<Text<'static>, std::io::Error> {
        let mut content = String::new();
        input.read_to_string(&mut content)?;
        let lines = find_lines_in(content.char_indices());
        Ok(Text {
            content: Cow::Owned(content),
            lines,
        })
    }

    /// Gets the number of lines
    #[must_use]
    pub fn get_line_count(&self) -> usize {
        self.lines.len()
    }

    /// Gets whether the text is empty
    #[must_use]
    pub fn is_empty(&self) -> bool {
        self.content.is_empty()
    }

    /// Gets the size in number of characters
    #[must_use]
    pub fn len(&self) -> usize {
        self.content.len()
    }

    /// Gets whether the specified index is after the end of the text represented by this object
    #[must_use]
    pub fn is_end(&self, index: usize) -> bool {
        index >= self.content.len()
    }

    /// Gets the character at the specified index
    ///
    /// # Panics
    ///
    /// Panic when index is at or beyond the end of the content
    #[must_use]
    pub fn at(&self, index: usize) -> char {
        self.content[index..].chars().next().unwrap()
    }

    /// Gets the substring beginning at the given index with the given length
    #[must_use]
    pub fn get_value(&self, index: usize, length: usize) -> &str {
        &self.content[index..(index + length)]
    }

    /// Get the substring corresponding to the specified span
    #[must_use]
    pub fn get_value_for(&self, span: TextSpan) -> &str {
        self.get_value(span.index, span.length)
    }

    /// Get the substring corresponding to the text at the specified position and the given length
    #[must_use]
    pub fn get_value_at(&self, position: TextPosition, length: usize) -> &str {
        let start = self.get_index_at(position);
        &self.content[start..(start + length)]
    }

    /// Gets the index within the content of the specified position
    #[must_use]
    pub fn get_index_at(&self, position: TextPosition) -> usize {
        let from_line = &self.content[self.lines[position.line - 1]..];
        let in_line_offset = from_line
            .char_indices()
            .take(position.column - 1)
            .last()
            .map(|(offset, c)| offset + c.len_utf8())
            .unwrap_or_default();
        self.lines[position.line - 1] + in_line_offset
    }

    /// Gets the starting index of the i-th line
    #[must_use]
    pub fn get_line_index(&self, line: usize) -> usize {
        self.lines[line - 1]
    }

    /// Gets the length of the i-th line
    #[must_use]
    pub fn get_line_length(&self, line: usize) -> usize {
        if line == self.lines.len() {
            self.content.len() - self.lines[line - 1]
        } else {
            self.lines[line] - self.lines[line - 1]
        }
    }

    /// Gets the string content of the i-th line
    #[must_use]
    pub fn get_line_content(&self, line: usize) -> &str {
        self.get_value(self.get_line_index(line), self.get_line_length(line))
    }

    /// Gets the position at the given index
    #[must_use]
    pub fn get_position_at(&self, index: usize) -> TextPosition {
        let line = find_line_at(&self.lines, index);
        let nb_chars = self.content[self.lines[line]..index].chars().count();
        TextPosition {
            line: line + 1,
            column: nb_chars + 1,
        }
    }

    /// Gets the position for a starting position and a length
    #[must_use]
    pub fn get_position_for(&self, position: TextPosition, length: usize) -> TextPosition {
        let index = self.lines[position.line - 1] + position.column - 1 + length;
        self.get_position_at(index)
    }

    /// Gets the context description for the current text at the specified position
    #[must_use]
    pub fn get_context_at(&self, position: TextPosition) -> TextContext {
        self.get_context_for(position, 1)
    }

    /// Gets the context description for the current text at the specified position
    #[must_use]
    pub fn get_context_for(&self, position: TextPosition, length: usize) -> TextContext {
        // gather the data for the line
        let mut line_content = self.get_line_content(position.line);
        // remove the line ending
        line_content = line_content.trim_end_matches(is_line_ending_char);
        // remove the heading white space
        let mut removed_heading = 0;
        loop {
            match line_content.chars().next() {
                None => break,
                Some(c) => {
                    if c.is_whitespace() {
                        line_content = &line_content[c.len_utf8()..];
                        removed_heading += 1;
                    } else {
                        break;
                    }
                }
            }
        }
        let in_line_offset = line_content
            .char_indices()
            .take(position.column - 1 - removed_heading)
            .last()
            .map(|(offset, c)| offset + c.len_utf8())
            .unwrap_or_default();
        let pointer_count = line_content[in_line_offset..]
            .char_indices()
            .take_while(|&(offset, _)| offset < length)
            .count()
            .max(1);
        let pointer_blank_count = position.column - 1 - removed_heading;
        // build the pointer
        let mut pointer = String::with_capacity(pointer_count + pointer_blank_count);
        for c in line_content.chars().take(pointer_blank_count) {
            pointer.push(if c == '\t' { '\t' } else { ' ' });
        }
        for _ in 0..pointer_count {
            pointer.push('^');
        }
        // return the output
        TextContext {
            content: line_content,
            pointer,
        }
    }

    /// Gets the context description for the current text at the specified span
    #[must_use]
    pub fn get_context_of(&self, span: TextSpan) -> TextContext {
        let position = self.get_position_at(span.index);
        self.get_context_for(position, span.length)
    }

    /// Gets an iterator over the UTF-16 codepoints starting at a location
    #[must_use]
    pub fn iter_utf16_from(&self, from: usize) -> Utf16Iter {
        Utf16Iter {
            inner: self.content[from..].chars(),
            next_cp: None,
        }
    }
}

/// An iterator over UTF-16 code points in the input text
/// This iterator yields a tuple (CP, length), where:
/// * CP is a UTF-16 codepoint
/// * length is the CP in the input
pub struct Utf16Iter<'a> {
    /// The inner iterator over chars
    inner: Chars<'a>,
    /// The next codepoint, if any
    next_cp: Option<(Utf16C, usize)>,
}

impl<'a> Iterator for Utf16Iter<'a> {
    type Item = (Utf16C, usize);

    fn next(&mut self) -> Option<Self::Item> {
        match self.next_cp.take() {
            Some(r) => Some(r),
            None => match self.inner.next() {
                None => None,
                Some(c) => {
                    let length = c.len_utf8();
                    let mut encoded = [0_u16; 2];
                    c.encode_utf16(&mut encoded);
                    if encoded[1] == 0 {
                        Some((encoded[0], length))
                    } else {
                        // sequence
                        self.next_cp = Some((encoded[1], length));
                        Some((encoded[0], 0))
                    }
                }
            },
        }
    }
}

/// Determines whether [c1, c2] form a line ending sequence
/// Recognized sequences are:
/// [U+000D, U+000A] (this is Windows-style \r \n)
/// [U+????, U+000A] (this is unix style \n)
/// [U+000D, U+????] (this is `MacOS` style \r, without \n after)
/// Others:
/// [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
fn is_line_ending(c1: char, c2: char) -> bool {
    (c2 == '\u{000B}'
        || c2 == '\u{000C}'
        || c2 == '\u{0085}'
        || c2 == '\u{2028}'
        || c2 == '\u{2029}')
        || (c1 == '\u{000D}' || c2 == '\u{000A}')
}

/// Determines whether the character is part of a line ending sequence
fn is_line_ending_char(c: char) -> bool {
    (c == '\u{000B}' || c == '\u{000C}' || c == '\u{0085}' || c == '\u{2028}' || c == '\u{2029}')
        || c == '\u{000D}'
        || c == '\u{000A}'
}

/// Finds all the lines in this content
fn find_lines_in<T: Iterator<Item = (usize, char)>>(iterator: T) -> Vec<usize> {
    let mut result = Vec::new();
    let mut c1: char;
    let mut c2: char = '\0';
    result.push(0);
    for (offset, x) in iterator {
        c1 = c2;
        c2 = x;
        if is_line_ending(c1, c2) {
            result.push(if c1 == '\u{000D}' && c2 != '\u{000A}' {
                offset
            } else {
                offset + x.len_utf8()
            });
        }
    }
    result
}

/// Finds the index of the line at the given input index in the content
fn find_line_at(lines: &[usize], index: usize) -> usize {
    for (i, line) in lines.iter().enumerate().skip(1) {
        if index < *line {
            return i - 1;
        }
    }
    lines.len() - 1
}

#[test]
fn test_text_lines() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.lines.len(), 2);
    assert_eq!(text.lines[0], 0);
    assert_eq!(text.lines[1], 8);
}

#[test]
fn test_text_at() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.at(0), 't');
    assert_eq!(text.at(8), 'a');
}

#[test]
fn test_text_get_value() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.get_value(0, 3), "thi");
    assert_eq!(text.get_value(8, 2), "a ");
}

#[test]
fn test_text_get_value_at() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(
        text.get_value_at(TextPosition { line: 1, column: 1 }, 3),
        "thi"
    );
    assert_eq!(
        text.get_value_at(TextPosition { line: 2, column: 3 }, 3),
        "new"
    );
}

#[test]
fn test_text_get_value_at_2() {
    let text = Text::from_str("नमस्ते\nЗдравствуйте");
    assert_eq!(
        text.get_value_at(TextPosition { line: 1, column: 1 }, 6),
        "नम"
    );
    assert_eq!(
        text.get_value_at(TextPosition { line: 2, column: 3 }, 4),
        "ра"
    );
}

#[test]
fn test_text_get_line_index() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.get_line_index(1), 0);
    assert_eq!(text.get_line_index(2), 8);
}

#[test]
fn test_text_get_line_length() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.get_line_length(1), 8);
    assert_eq!(text.get_line_length(2), 10);
}

#[test]
fn test_text_get_line_content() {
    let text = Text::from_str("this is\na new line");
    assert_eq!(text.get_line_content(1), "this is\n");
    assert_eq!(text.get_line_content(2), "a new line");
}

#[test]
fn test_text_get_position_at() {
    let text = Text::from_str("this is\na new line");
    for i in 0..8 {
        assert_eq!(
            text.get_position_at(i),
            TextPosition {
                line: 1,
                column: i + 1
            }
        );
    }
    for i in 8..text.content.len() {
        assert_eq!(
            text.get_position_at(i),
            TextPosition {
                line: 2,
                column: i + 1 - 8
            }
        );
    }
}

#[test]
fn test_text_get_context_for() {
    let text = Text::from_str("नमस्ते\nЗдравствуйте");
    assert_eq!(
        text.get_context_for(TextPosition { line: 1, column: 2 }, 6),
        TextContext {
            content: "नमस्ते",
            pointer: String::from(" ^^")
        }
    );
    assert_eq!(
        text.get_context_for(TextPosition { line: 2, column: 3 }, 6),
        TextContext {
            content: "Здравствуйте",
            pointer: String::from("  ^^^")
        }
    );
}

#[test]
fn test_text_get_context_for_on_empty_last() {
    let text = Text::from_str("x\n");
    assert_eq!(
        text.get_context_for(TextPosition { line: 2, column: 1 }, 0),
        TextContext {
            content: "",
            pointer: String::from("^")
        }
    );
}

#[test]
fn test_text_get_context_for_no_leading() {
    let text = Text::from_str("x\n\n  xxyx\nx");
    assert_eq!(
        text.get_context_for(TextPosition { line: 3, column: 5 }, 1),
        TextContext {
            content: "xxyx",
            pointer: String::from("  ^")
        }
    );
}

#[test]
fn test_text_get_context_for_inner_tab() {
    let text = Text::from_str("x\n\nx\txyx\nx");
    assert_eq!(
        text.get_context_for(TextPosition { line: 3, column: 4 }, 1),
        TextContext {
            content: "x\txyx",
            pointer: String::from(" \t ^")
        }
    );
}

#[test]
fn test_text_get_context_for_no_leading_tab() {
    let text = Text::from_str("x\n\n\txxyx\nx");
    assert_eq!(
        text.get_context_for(TextPosition { line: 3, column: 4 }, 1),
        TextContext {
            content: "xxyx",
            pointer: String::from("  ^")
        }
    );
}
