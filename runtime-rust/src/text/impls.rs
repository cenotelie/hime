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

//! Module for the implementations of text APIs

use std::io::Read;
use std::io::BufReader;

use super::super::utils::iterable::Iterable;
use super::super::utils::biglist::BigList;
use super::utf16::Utf16IteratorRaw;
use super::utf16::Utf16IteratorOverUtf8;
use super::Text;
use super::TextContext;
use super::TextPosition;
use super::Utf16C;

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
fn find_lines_in<'a, T: Iterable<'a, Item=Utf16C>>(iterable: &'a T) -> Vec<usize> {
    let mut result = Vec::<usize>::new();
    let mut c1;
    let mut c2 = 0;
    let mut i = 0;
    result.push(0);
    for x in iterable.iter() {
        c1 = c2;
        c2 = x;
        if is_line_ending(c1, c2) {
            result.push(if c1 == 0x000D && c2 != 0x000A { i } else { i + 1 });
        }
        i = i + 1;
    }
    result
}

/// Finds the index of the line at the given input index in the content
fn find_line_at(lines: &Vec<usize>, index: usize) -> usize {
    for i in 1..lines.len() {
        if index < lines[i] {
            return i - 1;
        }
    }
    return lines.len() - 1;
}

/// Converts an excerpt of a UTF-16 buffer to a string
fn utf16_to_string(content: &BigList<Utf16C>, start: usize, length: usize) -> String {
    let mut buffer = Vec::<Utf16C>::with_capacity(length);
    for i in start..(start + length) {
        buffer.push(content[i]);
    }
    let result = String::from_utf16(&buffer);
    if result.is_ok() {
        result.unwrap()
    } else {
        String::new()
    }
}


/// Text provider that fetches and stores the full content of an input lexer
/// All line numbers and column numbers are 1-based.
/// Indices in the content are 0-based.
pub struct PrefetchedText {
    /// The full content of the input
    content: BigList<Utf16C>,
    /// Cache of the starting indices of each line within the text
    lines: Vec<usize>
}

impl PrefetchedText {
    /// Initializes this text
    pub fn new(input: &str) -> PrefetchedText {
        let mut content = BigList::<Utf16C>::new(0);
        for c in input.chars() {
            let value = c as u32;
            if value <= 0xFFFF {
                content.add(value as u16);
            } else {
                let temp = value - 0x10000;
                let lead = (temp >> 10) + 0xD800;
                let trail = (temp & 0x03FF) + 0xDC00;
                content.add(lead as Utf16C);
                content.add(trail as Utf16C);
            }
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }

    /// Initializes this text from a UTF-16 stream
    pub fn from_utf16_stream(input: &mut Read, big_endian: bool) -> PrefetchedText {
        let reader = &mut BufReader::new(input);
        let mut content = BigList::<Utf16C>::new(0);
        let iterator = Utf16IteratorRaw::new(reader, big_endian);
        for c in iterator {
            content.add(c);
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }

    /// Initializes this text from a UTF-8 stream
    pub fn from_utf8_stream(input: &mut Read) -> PrefetchedText {
        let reader = &mut BufReader::new(input);
        let mut content = BigList::<Utf16C>::new(0);
        let iterator = Utf16IteratorOverUtf8::new(reader);
        for c in iterator {
            content.add(c);
        }
        let lines = find_lines_in(&content);
        PrefetchedText {
            content,
            lines
        }
    }
}

impl Text for PrefetchedText {
    /// Gets the number of lines
    fn get_line_count(&self) -> usize {
        self.lines.len()
    }

    /// Gets the size in number of characters
    fn get_size(&self) -> usize {
        self.content.size()
    }

    /// Gets whether the specified index is after the end of the text represented by this object
    fn is_end(&self, index: usize) -> bool {
        index >= self.content.size()
    }

    /// Gets the character at the specified index
    fn get_at(&self, index: usize) -> Utf16C {
        self.content[index]
    }

    /// Gets the substring beginning at the given index with the given length
    fn get_value(&self, index: usize, length: usize) -> String {
        utf16_to_string(&self.content, index, length)
    }

    /// Gets the starting index of the i-th line
    fn get_line_index(&self, line: usize) -> usize {
        self.lines[line]
    }

    /// Gets the length of the i-th line
    fn get_line_length(&self, line: usize) -> usize {
        if line == self.lines.len() - 1 {
            self.content.size() - self.lines[line - 1]
        } else {
            self.lines[line] - self.lines[line - 1]
        }
    }

    /// Gets the position at the given index
    fn get_position_at(&self, index: usize) -> TextPosition {
        let line = find_line_at(&self.lines, index);
        TextPosition {
            line: line + 1,
            column: index - self.lines[line] + 1
        }
    }

    /// Gets the context description for the current text at the specified position
    fn get_context_for(&self, position: TextPosition, length: usize) -> TextContext {
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
        while end != line_index + 1 && (self.content[end] == 0x000A || self.content[end] == 0x000B || self.content[end] == 0x000C || self.content[end] == 0x000D || self.content[end] == 0x0085 || self.content[end] == 0x2028 || self.content[end] == 0x2029) {
            end = end - 1;
        }
        let mut start = line_index;
        while start < end && is_white_space(self.content[start]) {
            start = start + 1;
        }
        if line_index + position.column - 1 < start {
            start = line_index;
        }
        if line_index + position.column - 1 > end {
            end = line_index + line_length - 1;
        }

        // build the pointer
        let mut pointer = String::new();
        for i in start..(line_index + position.column) {
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
}

#[test]
fn test_prefetched_text_lines() {
    let prefetched = PrefetchedText::new("this is\na new line");
    assert_eq!(prefetched.lines.len(), 2);
    assert_eq!(prefetched.lines[0], 0);
    assert_eq!(prefetched.lines[1], 8);
}

#[test]
fn test_prefetched_text_substring() {
    let prefetched = PrefetchedText::new("this is\na new line");
    assert_eq!(utf16_to_string(&prefetched.content, 8, 5), "a new");
}