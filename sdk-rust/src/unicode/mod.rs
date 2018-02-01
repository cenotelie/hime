/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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

//! Module for the Unicode support

pub mod blocks;

use std::cmp::Ordering;

use hime_redist::text::Utf16C;

/// Represents a Unicode code point
#[derive(Copy, Clone, Eq, PartialEq, Ord, PartialOrd)]
pub struct UnicodeCodePoint {
    /// The code point value
    pub value: u32
}

impl UnicodeCodePoint {
    /// Gets a value indicating whether this codepoint is in Unicode plane 0
    pub fn is_plane0(&self) -> bool {
        self.value <= 0xFFFF
    }

    /// Gets the UTF-16 encoding of this code point
    pub fn get_utf16(&self) -> [Utf16C; 2] {
        if self.value <= 0xFFFF {
            [self.value as Utf16C, 0 as Utf16C]
        } else {
            let temp = self.value - 0x10000;
            let lead = (temp >> 10) + 0xD800;
            let trail = (temp & 0x03FF) + 0xDC00;
            [lead as Utf16C, trail as Utf16C]
        }
    }
}

/// Represents a range of Unicode characters
#[derive(Copy, Clone, Eq, PartialEq, Ord)]
pub struct UnicodeSpan {
    /// Beginning of the range (included)
    pub begin: UnicodeCodePoint,
    /// End of the range (included)
    pub end: UnicodeCodePoint
}

impl PartialOrd for UnicodeSpan {
    fn partial_cmp(&self, other: &UnicodeSpan) -> Option<Ordering> {
        self.begin.value.partial_cmp(&other.begin.value)
    }
}

impl UnicodeSpan {
    /// Initializes a new span
    pub fn new(begin: u32, end: u32) -> UnicodeSpan {
        UnicodeSpan {
            begin: UnicodeCodePoint { value: begin },
            end: UnicodeCodePoint { value: end }
        }
    }

    /// Gets the range's length in number of characters
    pub fn len(&self) -> usize {
        (self.end.value - self.begin.value + 1) as usize
    }

    /// Gets a value indicating whether this span is entirely in Unicode plane 0
    pub fn is_plane0(&self) -> bool {
        self.begin.is_plane0() && self.end.is_plane0()
    }
}

/// Represents a Unicode block of characters
pub struct UnicodeBlock {
    /// The block's name
    pub name: String,
    /// The block's character span
    pub span: UnicodeSpan
}

impl UnicodeBlock {
    /// Initializes this Unicode block
    pub fn new(name: String, begin: u32, end: u32) -> UnicodeBlock {
        UnicodeBlock {
            name,
            span: UnicodeSpan::new(begin, end)
        }
    }
}

/// Represents a Unicode category
pub struct UnicodeCategory {
    /// The category's name
    pub name: String,
    /// The list of character spans contained in this category
    pub spans: Vec<UnicodeSpan>
}

impl UnicodeCategory {
    /// Initializes a new (empty) category
    pub fn new(name: String) -> UnicodeCategory {
        UnicodeCategory {
            name,
            spans: Vec::<UnicodeSpan>::new()
        }
    }

    /// Adds a span to this category
    pub fn add_span(&mut self, span: UnicodeSpan) {
        self.spans.push(span);
    }

    /// Aggregate the specified category into this one
    pub fn aggregate(&mut self, category: &UnicodeCategory) {
        for span in category.spans.iter() {
            self.spans.push(*span);
        }
    }
}
