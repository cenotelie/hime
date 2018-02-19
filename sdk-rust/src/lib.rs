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
pub mod automata;
pub mod grammar;

use std::cmp::Ordering;

use hime_redist::text::TextContext;
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
    pub fn intersect(left: CharSpan, right: CharSpan) -> Option<CharSpan> {
        if left.begin < right.begin {
            if left.end < right.begin {
                None
            } else if left.end < right.end {
                Some(CharSpan {
                    begin: right.begin,
                    end: left.end
                })
            } else {
                Some(CharSpan {
                    begin: right.begin,
                    end: right.end
                })
            }
        } else {
            if right.end < left.begin {
                None
            } else if right.end < left.end {
                Some(CharSpan {
                    begin: left.begin,
                    end: right.end
                })
            } else {
                Some(CharSpan {
                    begin: left.begin,
                    end: left.end
                })
            }
        }
    }

    /// Splits the original span with the given splitter
    pub fn split(original: CharSpan, splitter: CharSpan) -> (Option<CharSpan>, Option<CharSpan>) {
        if original.begin == splitter.begin {
            if original.end == splitter.end {
                (None, None)
            } else {
                (
                    None,
                    Some(CharSpan {
                        begin: splitter.end + 1,
                        end: original.end
                    })
                )
            }
        } else if original.end == splitter.end {
            (
                Some(CharSpan {
                    begin: original.begin,
                    end: splitter.begin - 1
                }),
                None
            )
        } else {
            (
                Some(CharSpan {
                    begin: original.begin,
                    end: splitter.begin - 1
                }),
                Some(CharSpan {
                    begin: splitter.end + 1,
                    end: original.end
                })
            )
        }
    }
}

/// Represents a parsing method
pub enum ParsingMethod {
    /// The LR(0) parsing method
    LR0,
    /// The LR(1) parsing method
    LR1,
    /// The LALR(1) parsing method
    LALR1,
    /// The RNGLR parsing method based on a LR(1) graph
    RNGLR1,
    /// The RNGLR parsing method based on a LALR(1) graph
    RNGLALR1
}

/// The data about an even in a report
pub struct EventData {
    /// The message for this event
    message: String,
    /// The context for this event, if any
    context: Option<TextContext>
}

pub enum Event {
    /// An information event
    Info(EventData),
    /// A warning event
    Warning(EventData),
    /// An error event
    Error(EventData)
}

impl Event {
    /// Creates a new information event
    pub fn new_info(message: String) -> Event {
        Event::Info(EventData {
            message,
            context: None
        })
    }

    /// Creates a new information event with some context
    pub fn new_info_with_context(message: String, context: TextContext) -> Event {
        Event::Info(EventData {
            message,
            context: Some(context)
        })
    }

    /// Creates a new warning event
    pub fn new_warning(message: String) -> Event {
        Event::Warning(EventData {
            message,
            context: None
        })
    }

    /// Creates a new warning event with some context
    pub fn new_warning_with_context(message: String, context: TextContext) -> Event {
        Event::Warning(EventData {
            message,
            context: Some(context)
        })
    }

    /// Creates a new error event
    pub fn new_error(message: String) -> Event {
        Event::Error(EventData {
            message,
            context: None
        })
    }

    /// Creates a new error event with some context
    pub fn new_error_with_context(message: String, context: TextContext) -> Event {
        Event::Error(EventData {
            message,
            context: Some(context)
        })
    }

    /// Gets the message for this event
    pub fn message(&self) -> &str {
        match self {
            &Event::Info(ref data) => &data.message,
            &Event::Warning(ref data) => &data.message,
            &Event::Error(ref data) => &data.message
        }
    }

    /// Gets the context for this event
    pub fn context(&self) -> Option<&TextContext> {
        match self {
            &Event::Info(ref data) => data.context.as_ref(),
            &Event::Warning(ref data) => data.context.as_ref(),
            &Event::Error(ref data) => data.context.as_ref()
        }
    }
}

/// Represents a compilation report
pub struct Report {
    pub events: Vec<Event>
}

impl Report {
    /// Initializes a new report
    pub fn new() -> Report {
        Report {
            events: Vec::<Event>::new()
        }
    }

    /// Adds a new info entry to the log
    pub fn info(&mut self, message: String) {
        println!("[INFO] {0}", message);
        self.events.push(Event::new_info(message));
    }

    /// Adds a new info entry to the log
    pub fn info_with_context(&mut self, message: String, context: TextContext) {
        println!("[INFO] {0}", message);
        println!("\t{0}", context.content);
        println!("\t{0}", context.pointer);
        self.events
            .push(Event::new_info_with_context(message, context))
    }

    /// Adds a new warning entry to the log
    pub fn warn(&mut self, message: String) {
        println!("[WARNING] {0}", message);
        self.events.push(Event::new_warning(message));
    }

    /// Adds a new warning entry to the log
    pub fn warn_with_context(&mut self, message: String, context: TextContext) {
        println!("[WARNING] {0}", message);
        println!("\t{0}", context.content);
        println!("\t{0}", context.pointer);
        self.events
            .push(Event::new_warning_with_context(message, context))
    }

    /// Adds a new error entry to the log
    pub fn error(&mut self, message: String) {
        println!("[ERROR] {0}", message);
        self.events.push(Event::new_error(message));
    }

    /// Adds a new error entry to the log
    pub fn error_with_context(&mut self, message: String, context: TextContext) {
        println!("[ERROR] {0}", message);
        println!("\t{0}", context.content);
        println!("\t{0}", context.pointer);
        self.events
            .push(Event::new_error_with_context(message, context))
    }
}

/// The version of this SDK
pub const VERSION: &'static str = env!("CARGO_PKG_VERSION");

/// Represents a compilation task for the generation of lexers and parsers from grammars
pub struct CompilationTask {}
