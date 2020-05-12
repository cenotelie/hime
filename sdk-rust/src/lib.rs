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

//! Rust SDK for the Hime parser generator

#[macro_use]
extern crate lazy_static;
extern crate ansi_term;
extern crate hime_redist;

pub mod automata;
pub mod errors;
pub mod grammars;
pub mod loaders;
pub mod output;
pub mod unicode;

use std::cmp::Ordering;

/// Represents a range of characters
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct CharSpan {
    /// Beginning of the range (included)
    pub begin: u16,
    /// End of the range (included)
    pub end: u16
}

/// Constant value for an invalid value
pub const CHARSPAN_INVALID: CharSpan = CharSpan { begin: 1, end: 0 };

impl CharSpan {
    /// Creates a new span
    pub fn new(begin: u16, end: u16) -> CharSpan {
        CharSpan { begin, end }
    }

    /// Gets the range's length in number of characters
    pub fn len(self) -> u16 {
        self.end - self.begin + 1
    }

    /// Gets whether the range is empty
    pub fn is_empty(self) -> bool {
        self.end < self.begin
    }

    /// Gets the intersection between two spans
    pub fn intersect(self, right: CharSpan) -> CharSpan {
        let result = CharSpan {
            begin: self.begin.max(right.begin),
            end: self.end.min(right.end)
        };
        if result.is_empty() {
            CHARSPAN_INVALID
        } else {
            result
        }
    }

    /// Splits the span with the given splitter
    pub fn split(self, splitter: CharSpan) -> (CharSpan, CharSpan) {
        if self.begin == splitter.begin {
            if self.end == splitter.end {
                return (CHARSPAN_INVALID, CHARSPAN_INVALID);
            }
            return (
                CharSpan {
                    begin: splitter.end + 1,
                    end: self.end
                },
                CHARSPAN_INVALID
            );
        }
        if self.end == splitter.end {
            return (
                CharSpan {
                    begin: self.begin,
                    end: splitter.begin - 1
                },
                CHARSPAN_INVALID
            );
        }
        (
            CharSpan {
                begin: self.begin,
                end: splitter.begin - 1
            },
            CharSpan {
                begin: splitter.end + 1,
                end: self.end
            }
        )
    }
}

impl Ord for CharSpan {
    fn cmp(&self, other: &CharSpan) -> Ordering {
        self.begin.cmp(&other.begin)
    }
}

impl PartialOrd for CharSpan {
    fn partial_cmp(&self, other: &CharSpan) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

#[test]
fn test_charspan_intersect() {
    // empty charspan
    assert_eq!(
        CharSpan::new(1, 0).intersect(CharSpan::new(3, 4)),
        CHARSPAN_INVALID
    );
    assert_eq!(
        CharSpan::new(3, 4).intersect(CharSpan::new(1, 0)),
        CHARSPAN_INVALID
    );
    assert_eq!(
        CharSpan::new(1, 0).intersect(CharSpan::new(1, 0)),
        CHARSPAN_INVALID
    );
    // no intersection
    assert_eq!(
        CharSpan::new(1, 2).intersect(CharSpan::new(3, 4)),
        CHARSPAN_INVALID
    );
    assert_eq!(
        CharSpan::new(3, 4).intersect(CharSpan::new(1, 2)),
        CHARSPAN_INVALID
    );
    // intersect single
    assert_eq!(
        CharSpan::new(1, 2).intersect(CharSpan::new(2, 3)),
        CharSpan::new(2, 2)
    );
    assert_eq!(
        CharSpan::new(2, 3).intersect(CharSpan::new(1, 2)),
        CharSpan::new(2, 2)
    );
    // intersect range
    assert_eq!(
        CharSpan::new(1, 5).intersect(CharSpan::new(3, 10)),
        CharSpan::new(3, 5)
    );
    assert_eq!(
        CharSpan::new(3, 10).intersect(CharSpan::new(1, 5)),
        CharSpan::new(3, 5)
    );
    // containment
    assert_eq!(
        CharSpan::new(1, 10).intersect(CharSpan::new(3, 5)),
        CharSpan::new(3, 5)
    );
    assert_eq!(
        CharSpan::new(3, 5).intersect(CharSpan::new(1, 10)),
        CharSpan::new(3, 5)
    );
}

#[test]
fn test_charspan_split() {
    // align left
    assert_eq!(
        CharSpan::new(1, 10).split(CharSpan::new(1, 3)),
        (CharSpan::new(4, 10), CHARSPAN_INVALID)
    );
    // align right
    assert_eq!(
        CharSpan::new(1, 10).split(CharSpan::new(6, 10)),
        (CharSpan::new(1, 5), CHARSPAN_INVALID)
    );
    // full
    assert_eq!(
        CharSpan::new(1, 10).split(CharSpan::new(1, 10)),
        (CHARSPAN_INVALID, CHARSPAN_INVALID)
    );
    // split middle
    assert_eq!(
        CharSpan::new(1, 10).split(CharSpan::new(4, 6)),
        (CharSpan::new(1, 3), CharSpan::new(7, 10))
    );
}

/// Represents a parsing method
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
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

/// Represents a grammar's compilation mode
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub enum Mode {
    /// Generates the source code for the lexer and parser
    Source,
    /// Generates the compiled assembly of the lexer and parser
    Assembly,
    /// Generates the source code for the lexer and parser and the compiled assembly
    SourceAndAssembly,
    /// Generates the source code for the lexer and parser, as well as the debug data
    Debug
}

/// Represents the target runtime to compile for
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub enum Runtime {
    /// The .Net platform
    Net,
    /// The Java platform
    Java,
    /// The Rust language
    Rust
}

/// Represents the access modifiers for the generated code
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub enum Modifier {
    /// Generated classes are public
    Public,
    /// Generated classes are internal
    Internal
}

/// Represents a compilation task for the generation of lexers and parsers from grammars
#[derive(Debug, Clone)]
pub struct CompilationTask {
    /// The input file names
    pub input_files: Vec<String>,
    /// The name of the grammar to compile in the case where several grammars are loaded.
    pub grammar_name: Option<String>,
    /// The compiler's output mode
    pub mode: Mode,
    /// The target runtime
    pub output_target: Runtime,
    /// The path to a local Rust target runtime
    pub output_rust_runtime: Option<String>,
    /// The path for the compiler's output
    pub output_path: Option<String>,
    /// The namespace for the generated code
    pub output_namespace: Option<String>,
    /// The access modifier for the generated code
    pub output_modifier: Modifier,
    /// The parsing method use
    pub method: ParsingMethod
}

impl CompilationTask {
    /// Creates a new task with default values
    pub fn new() -> CompilationTask {
        CompilationTask {
            input_files: Vec::new(),
            grammar_name: None,
            mode: Mode::Source,
            output_target: Runtime::Net,
            output_rust_runtime: None,
            output_path: None,
            output_namespace: None,
            output_modifier: Modifier::Internal,
            method: ParsingMethod::LALR1
        }
    }

    /// Executes this task
    pub fn execute(&self) -> Result<(), ()> {
        let _grammars = match loaders::load(&self.input_files) {
            Ok(grammars) => grammars,
            Err(_) => return Err(())
        };
        Ok(())
    }
}
