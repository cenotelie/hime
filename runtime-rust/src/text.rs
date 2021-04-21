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

use std::cmp::{Ord, Ordering};
use std::fmt::{Display, Error, Formatter};
use std::io::{BufReader, Read};
use std::result::Result;

use serde::{Deserialize, Serialize};

use crate::utils::biglist::BigList;

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
#[derive(Debug, Copy, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct TextSpan {
    /// The starting index
    pub index: usize,
    /// The length
    pub length: usize
}

impl PartialOrd for TextSpan {
    fn partial_cmp(&self, other: &TextSpan) -> Option<Ordering> {
        match self.index.cmp(&other.index) {
            Ordering::Greater => Some(Ordering::Greater),
            Ordering::Less => Some(Ordering::Less),
            Ordering::Equal => Some(self.length.cmp(&other.length))
        }
    }
}

impl Ord for TextSpan {
    fn cmp(&self, other: &TextSpan) -> Ordering {
        self.partial_cmp(other).unwrap()
    }
}

/// Implementation of `Display` for `TextSpan`
impl Display for TextSpan {
    fn fmt(&self, f: &mut Formatter) -> Result<(), Error> {
        write!(f, "@{}+{}", self.index, self.length)
    }
}

/// Represents a position in term of line and column in a text input
#[derive(Debug, Copy, Clone, PartialEq, Eq, Serialize, Deserialize)]
pub struct TextPosition {
    /// The line number
    pub line: usize,
    /// The column number
    pub column: usize
}

impl PartialOrd for TextPosition {
    fn partial_cmp(&self, other: &TextPosition) -> Option<Ordering> {
        match self.line.cmp(&other.line) {
            Ordering::Greater => Some(Ordering::Greater),
            Ordering::Less => Some(Ordering::Less),
            Ordering::Equal => Some(self.column.cmp(&other.column))
        }
    }
}

impl Ord for TextPosition {
    fn cmp(&self, other: &TextPosition) -> Ordering {
        self.partial_cmp(other).unwrap()
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
#[derive(Debug, Clone)]
pub struct TextContext {
    /// The text content being represented
    pub content: String,
    /// The pointer textual representation
    pub pointer: String
}

/// Represents the input of parser with some metadata for line endings
/// All line numbers and column numbers are 1-based.
/// Indices in the content are 0-based.
#[derive(Debug, Clone)]
pub struct Text {
    /// The full content of the input
    content: BigList<Utf16C>,
    /// Cache of the starting indices of each line within the text
    lines: Vec<usize>
}

impl Text {
    /// Initializes this text
    pub fn new(input: &str) -> Text {
        let mut content = BigList::new(0);
        for c in input.chars() {
            let value = c as u32;
            if value <= 0xFFFF {
                content.push(value as u16);
            } else {
                let temp = value - 0x10000;
                let lead = (temp >> 10) + 0xD800;
                let trail = (temp & 0x03FF) + 0xDC00;
                content.push(lead as Utf16C);
                content.push(trail as Utf16C);
            }
        }
        let lines = find_lines_in(content.iter());
        Text { content, lines }
    }

    /// Initializes this text from a UTF-16 stream
    pub fn from_utf16_stream(input: &mut dyn Read, big_endian: bool) -> Text {
        let reader = &mut BufReader::new(input);
        let mut content = BigList::new(0);
        let iterator = Utf16IteratorRaw::new(reader, big_endian);
        for c in iterator {
            content.push(c);
        }
        let lines = find_lines_in(content.iter());
        Text { content, lines }
    }

    /// Initializes this text from a UTF-8 stream
    pub fn from_utf8_stream(input: &mut dyn Read) -> Text {
        let reader = &mut BufReader::new(input);
        let mut content = BigList::new(0);
        let iterator = Utf16IteratorOverUtf8::new(reader);
        for c in iterator {
            content.push(c);
        }
        let lines = find_lines_in(content.iter());
        Text { content, lines }
    }

    /// Gets the number of lines
    pub fn get_line_count(&self) -> usize {
        self.lines.len()
    }

    /// Gets whether the text is empty
    pub fn is_empty(&self) -> bool {
        self.content.is_empty()
    }

    /// Gets the size in number of characters
    pub fn len(&self) -> usize {
        self.content.len()
    }

    /// Gets whether the specified index is after the end of the text represented by this object
    pub fn is_end(&self, index: usize) -> bool {
        index >= self.content.len()
    }

    /// Gets the character at the specified index
    pub fn at(&self, index: usize) -> Utf16C {
        self.content[index]
    }

    /// Gets the substring beginning at the given index with the given length
    pub fn get_value(&self, index: usize, length: usize) -> String {
        utf16_to_string(&self.content, index, length)
    }

    /// Get the substring corresponding to the specified span
    pub fn get_value_for(&self, span: TextSpan) -> String {
        self.get_value(span.index, span.length)
    }

    /// Gets the starting index of the i-th line
    pub fn get_line_index(&self, line: usize) -> usize {
        self.lines[line - 1]
    }

    /// Gets the length of the i-th line
    pub fn get_line_length(&self, line: usize) -> usize {
        if line == self.lines.len() {
            self.content.len() - self.lines[line - 1]
        } else {
            self.lines[line] - self.lines[line - 1]
        }
    }

    /// Gets the string content of the i-th line
    pub fn get_line_content(&self, line: usize) -> String {
        self.get_value(self.get_line_index(line), self.get_line_length(line))
    }

    /// Gets the position at the given index
    pub fn get_position_at(&self, index: usize) -> TextPosition {
        let line = find_line_at(&self.lines, index);
        TextPosition {
            line: line + 1,
            column: index - self.lines[line] + 1
        }
    }

    /// Gets the position for a starting position and a length
    pub fn get_position_for(&self, position: TextPosition, length: usize) -> TextPosition {
        let index = self.lines[position.line - 1] + position.column - 1 + length;
        self.get_position_at(index)
    }

    /// Gets the context description for the current text at the specified position
    pub fn get_context_at(&self, position: TextPosition) -> TextContext {
        self.get_context_for(position, 1)
    }

    /// Gets the context description for the current text at the specified position
    pub fn get_context_for(&self, position: TextPosition, length: usize) -> TextContext {
        // gather the data for the line
        let line_index = self.get_line_index(position.line);
        let line_length = self.get_line_length(position.line);
        if line_length == 0 {
            return TextContext {
                content: String::from(""),
                pointer: String::from("^")
            };
        }

        // gather the start and end indices of the line's content to output
        let mut end = line_index + line_length - 1;
        while end != line_index + 1
            && (self.content[end] == 0x000A
                || self.content[end] == 0x000B
                || self.content[end] == 0x000C
                || self.content[end] == 0x000D
                || self.content[end] == 0x0085
                || self.content[end] == 0x2028
                || self.content[end] == 0x2029)
        {
            end -= 1;
        }
        let mut start = line_index;
        while start < end && is_white_space(self.content[start]) {
            start += 1;
        }
        if line_index + position.column - 1 < start {
            start = line_index;
        }
        if line_index + position.column - 1 > end {
            end = line_index + line_length - 1;
        }

        // build the pointer
        let mut pointer = String::new();
        for i in start..(line_index + position.column - 1) {
            pointer.push(if self.content[i] == 0x0009 { '\t' } else { ' ' });
        }
        pointer.push('^');
        for _i in 1..length {
            pointer.push('^');
        }

        // return the output
        TextContext {
            content: utf16_to_string(&self.content, start, end - start + 1),
            pointer
        }
    }

    /// Gets the context description for the current text at the specified span
    pub fn get_context_of(&self, span: TextSpan) -> TextContext {
        let position = self.get_position_at(span.index);
        self.get_context_for(position, span.length)
    }
}

/// `Utf16IteratorRaw` provides an iterator of UTF-16 code units
/// over an input of bytes assumed to represent UTF-16 code units
struct Utf16IteratorRaw<'a> {
    /// whether to use big-endian or little-endian
    big_endian: bool,
    /// The input reader
    input: &'a mut dyn Read
}

impl<'a> Iterator for Utf16IteratorRaw<'a> {
    type Item = Utf16C;
    fn next(&mut self) -> Option<Self::Item> {
        // read two bytes
        let mut bytes: [u8; 2] = [0; 2];
        let read = self.input.read(&mut bytes);
        if read.is_err() || read.unwrap() < 2 {
            return None;
        }
        if self.big_endian {
            Some(u16::from(bytes[1]) << 8 | u16::from(bytes[0]))
        } else {
            Some(u16::from(bytes[0]) << 8 | u16::from(bytes[1]))
        }
    }
}

impl<'a> Utf16IteratorRaw<'a> {
    /// Creates a new instance of the iterator
    pub fn new(input: &'a mut dyn Read, big_endian: bool) -> Utf16IteratorRaw {
        Utf16IteratorRaw { big_endian, input }
    }
}

/// Provides an iterator of UTF-16 code points
/// over an input of bytes assumed to represent UTF-8 code points
struct Utf16IteratorOverUtf8<'a> {
    /// The input reader
    input: &'a mut dyn Read,
    /// The next UTF-16 code point, if any
    next: Option<Utf16C>
}

impl<'a> Utf16IteratorOverUtf8<'a> {
    /// Reads the input into the buffer
    fn read(input: &mut dyn Read, buffer: &mut [u8]) -> usize {
        let read = input.read(buffer);
        match read {
            Err(e) => panic!("{}", e),
            Ok(size) => size
        }
    }
}

impl<'a> Iterator for Utf16IteratorOverUtf8<'a> {
    type Item = Utf16C;
    fn next(&mut self) -> Option<Self::Item> {
        // do we have a cached
        if self.next.is_some() {
            let result = self.next;
            self.next = None;
            return result;
        }
        // read the next byte
        let mut bytes: [u8; 1] = [0; 1];
        {
            if Utf16IteratorOverUtf8::read(&mut self.input, &mut bytes) == 0 {
                return None;
            }
        }
        let b0 = bytes[0] as u8;

        let c = match b0 {
            _ if b0 >> 3 == 0b11110 => {
                // this is 4 bytes encoding
                let mut others: [u8; 3] = [0; 3];
                if Utf16IteratorOverUtf8::read(&mut self.input, &mut others) < 3 {
                    return None;
                }
                (u32::from(b0) & 0b0000_0111) << 18
                    | (u32::from(others[0]) & 0b0011_1111) << 12
                    | (u32::from(others[1]) & 0b0011_1111) << 6
                    | (u32::from(others[2]) & 0b0011_1111)
            }
            _ if b0 >> 4 == 0b1110 => {
                // this is a 3 bytes encoding
                let mut others: [u8; 2] = [0; 2];
                if Utf16IteratorOverUtf8::read(&mut self.input, &mut others) < 2 {
                    return None;
                }
                (u32::from(b0) & 0b0000_1111) << 12
                    | (u32::from(others[0]) & 0b0011_1111) << 6
                    | (u32::from(others[1]) & 0b0011_1111)
            }
            _ if b0 >> 5 == 0b110 => {
                // this is a 2 bytes encoding
                if Utf16IteratorOverUtf8::read(&mut self.input, &mut bytes) < 1 {
                    return None;
                }
                (u32::from(b0) & 0b0001_1111) << 6 | (u32::from(bytes[0]) & 0b0011_1111)
            }
            _ if b0 >> 7 == 0 => {
                // this is a 1 byte encoding
                u32::from(b0)
            }
            _ => {
                return None;
            }
        };

        // now we have the decoded unicode character
        // encode it in UTF-16
        if (c >= 0xD800 && c < 0xE000) || c >= 0x0011_0000 {
            // not a valid unicode character
            return None;
        }
        if c <= 0xFFFF {
            // simple case
            return Some(c as Utf16C);
        }
        // we need to encode
        let temp = c - 0x10000;
        let lead = (temp >> 10) + 0xD800;
        let trail = (temp & 0x03FF) + 0xDC00;
        // store the trail and return the lead
        self.next = Some(trail as Utf16C);
        Some(lead as Utf16C)
    }
}

impl<'a> Utf16IteratorOverUtf8<'a> {
    /// Creates a new instance of the iterator
    pub fn new(input: &'a mut dyn Read) -> Utf16IteratorOverUtf8 {
        Utf16IteratorOverUtf8 { input, next: None }
    }
}

/// Determines whether [c1, c2] form a line ending sequence
/// Recognized sequences are:
/// [U+000D, U+000A] (this is Windows-style \r \n)
/// [U+????, U+000A] (this is unix style \n)
/// [U+000D, U+????] (this is MacOS style \r, without \n after)
/// Others:
/// [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
fn is_line_ending(c1: Utf16C, c2: Utf16C) -> bool {
    (c2 == 0x000B || c2 == 0x000C || c2 == 0x0085 || c2 == 0x2028 || c2 == 0x2029)
        || (c1 == 0x000D || c2 == 0x000A)
}

/// Determines whether the character is a whitespace
fn is_white_space(c: Utf16C) -> bool {
    c == 0x0020 || c == 0x0009 || c == 0x000B || c == 0x000C
}

/// Finds all the lines in this content
fn find_lines_in<T: Iterator<Item = Utf16C>>(iterator: T) -> Vec<usize> {
    let mut result = Vec::new();
    let mut c1;
    let mut c2 = 0;
    result.push(0);
    for (i, x) in iterator.enumerate() {
        c1 = c2;
        c2 = x;
        if is_line_ending(c1, c2) {
            result.push(if c1 == 0x000D && c2 != 0x000A {
                i
            } else {
                i + 1
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

/// Converts an excerpt of a UTF-16 buffer to a string
fn utf16_to_string(content: &BigList<Utf16C>, start: usize, length: usize) -> String {
    let mut buffer = Vec::with_capacity(length);
    for i in start..(start + length) {
        buffer.push(content[i]);
    }
    let result = String::from_utf16(&buffer);
    result.unwrap_or_default()
}

#[test]
fn test_text_lines() {
    let text = Text::new("this is\na new line");
    assert_eq!(text.lines.len(), 2);
    assert_eq!(text.lines[0], 0);
    assert_eq!(text.lines[1], 8);
}

#[test]
fn test_text_substring() {
    let text = Text::new("this is\na new line");
    assert_eq!(utf16_to_string(&text.content, 8, 5), "a new");
}

#[test]
fn test_read_utf8() {
    let bytes: [u8; 13] = [
        0x78, 0xE2, 0x80, 0xA8, 0xE2, 0x80, 0xA8, 0x78, 0xE2, 0x80, 0xA8, 0x79, 0x78
    ];
    let mut content = Vec::new();
    let reader = &mut bytes.as_ref();
    let iterator = Utf16IteratorOverUtf8::new(reader);
    for c in iterator {
        content.push(c);
    }
    assert_eq!(7, content.len());
    assert_eq!(0x78, content[0]);
    assert_eq!(0x2028, content[1]);
    assert_eq!(0x2028, content[2]);
    assert_eq!(0x78, content[3]);
    assert_eq!(0x2028, content[4]);
    assert_eq!(0x79, content[5]);
    assert_eq!(0x78, content[6]);
}
