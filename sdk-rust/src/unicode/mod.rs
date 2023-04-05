/*******************************************************************************
 * Copyright (c) 2019 Association Cénotélie (cenotelie.fr)
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

//! Unicode support

mod blocks;
mod categories;

use std::collections::HashMap;

use lazy_static::lazy_static;

lazy_static! {
    /// Contains the supported Unicode blocks
    pub static ref BLOCKS: HashMap<&'static str, Block> = blocks::get_blocks();
}

lazy_static! {
    /// Contains the supported Unicode blocks
    pub static ref CATEGORIES: HashMap<&'static str, Category> = categories::get_categories();
}

/// Represents a Unicode code point
#[derive(Debug, Copy, Clone, Eq, PartialEq, PartialOrd, Ord, Hash)]
pub struct CodePoint(u32);

impl CodePoint {
    /// Initializes the code point
    ///
    /// # Panics
    ///
    /// Raise a panic when the value is not a valid code point
    #[must_use]
    pub fn new(value: u32) -> CodePoint {
        assert!(
            !((0xD800..=0xDFFF).contains(&value) || value >= 0x0011_0000),
            "The value is not a valid Unicode character code point"
        );
        CodePoint(value)
    }

    /// Gets the code point value
    #[must_use]
    pub fn value(self) -> u32 {
        self.0
    }

    /// Gets a value indicating whether this codepoint is in Unicode plane 0
    #[must_use]
    pub fn is_plane0(self) -> bool {
        self.0 <= 0xFFFF
    }

    /// Gets the UTF-16 encoding of this code point
    #[allow(clippy::cast_possible_truncation)]
    #[must_use]
    pub fn get_utf16(self) -> [u16; 2] {
        if self.0 <= 0xFFFF {
            // plane 0
            return [self.0 as u16, 0];
        }
        let temp = self.0 - 0x10000;
        let lead = (temp >> 10) + 0xD800;
        let trail = (temp & 0x03FF) + 0xDC00;
        [lead as u16, trail as u16]
    }
}

/// Represents a range of Unicode characters
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct Span {
    /// Beginning of the range (included)
    pub begin: CodePoint,
    /// End of the range (included)
    pub end: CodePoint
}

impl Span {
    /// Initializes this character span
    #[must_use]
    pub fn new(begin: u32, end: u32) -> Span {
        Span {
            begin: CodePoint::new(begin),
            end: CodePoint::new(end)
        }
    }

    /// Gets the range's length in number of characters
    #[must_use]
    pub fn len(self) -> u32 {
        self.end.0 - self.begin.0 + 1
    }

    /// Gets whether the range is empty
    #[must_use]
    pub fn is_empty(self) -> bool {
        self.end.0 < self.begin.0
    }

    /// Gets a value indicating whether this codepoint is in Unicode plane 0
    #[must_use]
    pub fn is_plane0(self) -> bool {
        self.begin.is_plane0() && self.end.is_plane0()
    }
}

/// Represents a Unicode block of characters
#[derive(Debug, Clone)]
pub struct Block {
    /// The block's name
    pub name: String,
    /// The block's character span
    pub span: Span
}

impl Block {
    /// Initializes this Unicode block
    #[must_use]
    pub fn new(name: &str, begin: u32, end: u32) -> Block {
        Block {
            name: name.to_string(),
            span: Span::new(begin, end)
        }
    }

    /// Initializes this Unicode block
    #[must_use]
    pub fn new_owned(name: String, begin: u32, end: u32) -> Block {
        Block {
            name,
            span: Span::new(begin, end)
        }
    }
}

/// Represents a Unicode category
#[derive(Debug, Clone)]
pub struct Category {
    /// The category's name
    pub name: String,
    /// The list of character spans contained in this category
    pub spans: Vec<Span>
}

impl Category {
    /// Represents a Unicode category
    #[must_use]
    pub fn new(name: &'static str) -> Category {
        Category {
            name: name.to_string(),
            spans: Vec::new()
        }
    }

    /// Represents a Unicode category
    #[must_use]
    pub fn new_owned(name: String) -> Category {
        Category {
            name,
            spans: Vec::new()
        }
    }

    /// Adds a span to this category
    pub fn add_span(&mut self, begin: u32, end: u32) {
        self.spans.push(Span::new(begin, end));
    }

    /// Aggregate the specified category into this one
    pub fn aggregate(&mut self, category: &Category) {
        self.spans.extend_from_slice(&category.spans);
    }
}
