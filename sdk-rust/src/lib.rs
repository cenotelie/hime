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

//! The Rust implementation of the SDK for the generation of lexers and parsers from [Hime](https://bitbucket.org/cenotelie/hime) grammars.
//! For more information about how to generate parsers using Hime, head to [Hime](https://cenotelie.fr/hime).
//! The code for this library is available on [Bitbucket](https://bitbucket.org/cenotelie/hime).
//! This software is developed by the [Assocation Cénotélie](https://cenotelie.fr/), France.
//!
//! # Usage
//! This crate is [on crates.io](https://crates.io/crates/hime_sdk) and can be
//! used by adding `hime_sdk` to your dependencies in your project's `Cargo.toml`.
//!
//! ```toml
//! [dependencies]
//! hime_sdk = "3.3.1"
//! ```
//!
//! and this to your crate root:
//!
//! ```rust
//! extern crate hime_sdk;
//! ```

extern crate hime_redist;
#[macro_use]
extern crate lazy_static;

pub mod unicode;
pub mod input;
pub mod output;

use std::cmp::Ordering;

use hime_redist::text::Text;
use hime_redist::text::TextPosition;
use hime_redist::text::Utf16C;

/// Represents a range of characters
#[derive(Copy, Clone, Eq, PartialEq, Ord)]
pub struct CharSpan {
    /// Beginning of the range (included)
    pub begin: Utf16C,
    /// End of the range (included)
    pub end: Utf16C
}

impl PartialOrd for CharSpan {
    fn partial_cmp(&self, other: &CharSpan) -> Option<Ordering> {
        self.begin.partial_cmp(&other.begin)
    }
}

/// Constant value for an invalid value
pub const CHAR_SPAN_NULL: CharSpan = CharSpan { begin: 1, end: 0 };

impl CharSpan {
    /// Gets whether this is the null span
    pub fn is_null(&self) -> bool {
        self.begin == 1 && self.end == 0
    }

    /// Gets the range's length in number of characters
    pub fn len(&self) -> isize {
        (self.end as isize) - (self.begin as isize) + 1
    }

    /// Gets the intersection between two spans
    pub fn intersect(left: CharSpan, right: CharSpan) -> CharSpan {
        if left.begin < right.begin {
            if left.end < right.begin {
                CHAR_SPAN_NULL
            } else if left.end < right.end {
                CharSpan {
                    begin: right.begin,
                    end: left.end
                }
            } else {
                CharSpan {
                    begin: right.begin,
                    end: right.end
                }
            }
        } else {
            if right.end < left.begin {
                CHAR_SPAN_NULL
            } else if right.end < left.end {
                CharSpan {
                    begin: left.begin,
                    end: right.end
                }
            } else {
                CharSpan {
                    begin: left.begin,
                    end: left.end
                }
            }
        }
    }

    /// Splits the original span with the given splitter
    pub fn split(original: CharSpan, splitter: CharSpan) -> (CharSpan, CharSpan) {
        if original.begin == splitter.begin {
            if original.end == splitter.end {
                (CHAR_SPAN_NULL, CHAR_SPAN_NULL)
            } else {
                (
                    CharSpan {
                        begin: splitter.end + 1,
                        end: original.end
                    },
                    CHAR_SPAN_NULL
                )
            }
        } else if original.end == splitter.end {
            (
                CharSpan {
                    begin: original.begin,
                    end: splitter.begin - 1
                },
                CHAR_SPAN_NULL
            )
        } else {
            (
                CharSpan {
                    begin: original.begin,
                    end: splitter.begin - 1
                },
                CharSpan {
                    begin: splitter.end + 1,
                    end: original.end
                }
            )
        }
    }
}

/// Represents a compilation report
pub struct Report {
    /// The list of info messages in this report
    pub infos: Vec<String>,
    /// The list of warnings in this report
    pub warnings: Vec<String>,
    /// The list of errors in this report
    pub errors: Vec<String>
}

impl Report {
    /// Initializes a new report
    pub fn new() -> Report {
        Report {
            infos: Vec::<String>::new(),
            warnings: Vec::<String>::new(),
            errors: Vec::<String>::new()
        }
    }

    /// Adds a new info entry to the log
    pub fn info(&mut self, message: String) {
        println!("[INFO] {0}", message);
        self.infos.push(message);
    }

    /// Adds a new info entry to the log
    pub fn info_from_input(&mut self, message: String, input: &Text, position: TextPosition) {
        self.info(message);
        Report::output_context(input, position);
    }

    /// Adds a new warning entry to the log
    pub fn warn(&mut self, message: String) {
        println!("[WARNING] {0}", message);
        self.warnings.push(message);
    }

    /// Adds a new warning entry to the log
    pub fn warn_from_input(&mut self, message: String, input: &Text, position: TextPosition) {
        self.warn(message);
        Report::output_context(input, position);
    }

    /// Adds a new error entry to the log
    pub fn error(&mut self, message: String) {
        println!("[WARNING] {0}", message);
        self.errors.push(message);
    }

    /// Adds a new error entry to the log
    pub fn error_from_input(&mut self, message: String, input: &Text, position: TextPosition) {
        self.error(message);
        Report::output_context(input, position);
    }

    /// Outputs the context of a message in the console
    fn output_context(input: &Text, position: TextPosition) {
        let context = input.get_context_at(position);
        println!("\t{0}", context.content);
        println!("\t{0}", context.pointer);
    }
}
