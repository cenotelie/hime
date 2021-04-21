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

pub mod errors;
pub mod finite;
pub mod grammars;
pub mod loaders;
pub mod lr;
pub mod output;
pub mod sdk;
pub mod unicode;

use std::cmp::Ordering;
use std::fs;
use std::io::Read;

use hime_redist::ast::AstNode;
use hime_redist::text::{Text, TextPosition};

use crate::errors::{Error, Errors};
use crate::grammars::{
    Grammar, OPTION_ACCESS_MODIFIER, OPTION_METHOD, OPTION_MODE, OPTION_NAMESPACE,
    OPTION_OUTPUT_PATH, OPTION_RUNTIME
};
use crate::sdk::InMemoryParser;

/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

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

/// Represent an input for the compiler
#[derive(Debug, Clone)]
pub enum Input<'a> {
    /// A file name
    FileName(String),
    /// An raw input
    Raw(&'a str)
}

impl<'a> Input<'a> {
    /// Gets the input's name
    pub fn name(&self) -> String {
        match self {
            Input::FileName(ref file_name) => file_name.clone(),
            Input::Raw(_) => String::from("Raw input")
        }
    }

    ///
    pub fn open(&self) -> Result<Box<dyn Read + 'a>, std::io::Error> {
        match self {
            Input::FileName(file_name) => Ok(Box::new(fs::File::open(file_name)?)),
            Input::Raw(ref text) => Ok(Box::new(text.as_bytes()))
        }
    }
}

/// The data for an input that has been loaded
#[derive(Debug, Clone)]
pub struct LoadedInput {
    /// The input's name (file name)
    pub name: String,
    /// The input's content (full text)
    pub content: Text
}

/// The data resulting of loading inputs
#[derive(Debug, Clone)]
pub struct LoadedData {
    /// The loaded inputs
    pub inputs: Vec<LoadedInput>,
    /// The loaded grammars
    pub grammars: Vec<Grammar>
}

/// Reference to an input
#[derive(Debug, Copy, Clone, Eq, PartialEq)]
pub struct InputReference {
    /// The input's index
    pub input_index: usize,
    /// The position in the input
    pub position: TextPosition,
    /// The length in the input
    pub length: usize
}

impl InputReference {
    pub fn get_line_number_width(&self) -> usize {
        if self.position.line < 10 {
            1
        } else if self.position.line < 100 {
            2
        } else if self.position.line < 1000 {
            3
        } else if self.position.line < 10_000 {
            4
        } else if self.position.line < 100_000 {
            5
        } else {
            6
        }
    }
}

impl InputReference {
    /// Build a reference from the specifiec input name and AST node
    pub fn from<'a: 'b + 'd, 'b: 'd, 'c: 'd, 'd>(
        input_index: usize,
        node: &AstNode<'a, 'b, 'c, 'd>
    ) -> InputReference {
        let (position, span) = node.get_total_position_and_span().unwrap();
        InputReference {
            input_index,
            position,
            length: span.length
        }
    }
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

impl ParsingMethod {
    /// Gets whether conflicts shall be raised for this method
    pub fn raise_conflict(self) -> bool {
        !self.is_rnglr()
    }

    /// Gets whether this method uses RNGLR
    pub fn is_rnglr(self) -> bool {
        match self {
            ParsingMethod::LR0 => false,
            ParsingMethod::LR1 => false,
            ParsingMethod::LALR1 => false,
            ParsingMethod::RNGLR1 => true,
            ParsingMethod::RNGLALR1 => true
        }
    }
}

/// Represents a grammar's compilation mode
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub enum Mode {
    /// Generates the source code for the lexer and parser
    Sources,
    /// Generates the compiled assembly of the lexer and parser
    Assembly,
    /// Generates the source code for the lexer and parser and the compiled assembly
    SourcesAndAssembly
}

impl Mode {
    /// Gets whether a mode requires assembly output
    pub fn output_assembly(self) -> bool {
        match self {
            Mode::Sources => false,
            Mode::Assembly => true,
            Mode::SourcesAndAssembly => true
        }
    }
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
#[derive(Debug, Default, Clone)]
pub struct CompilationTask<'a> {
    /// The inputs
    pub inputs: Vec<Input<'a>>,
    /// The name of the grammar to compile in the case where several grammars are loaded.
    pub grammar_name: Option<String>,
    /// The compiler's output mode
    pub mode: Option<Mode>,
    /// The target runtime
    pub output_target: Option<Runtime>,
    /// The path to a local target runtime overriding the one specified in the project manifest
    pub output_target_runtime_path: Option<String>,
    /// The path for the compiler's output
    pub output_path: Option<String>,
    /// The namespace for the generated code
    pub output_namespace: Option<String>,
    /// The access modifier for the generated code
    pub output_modifier: Option<Modifier>,
    /// The parsing method use
    pub method: Option<ParsingMethod>
}

impl<'a> CompilationTask<'a> {
    /// Gets the compiler's output mode for the grammar
    pub fn get_mode_for(&self, grammar: &Grammar, grammar_index: usize) -> Result<Mode, Error> {
        match self.mode {
            Some(mode) => Ok(mode),
            None => match grammar.get_option(OPTION_MODE) {
                Some(option) => match option.value.as_ref() {
                    "sources" => Ok(Mode::Sources),
                    "all" => Ok(Mode::SourcesAndAssembly),
                    "assembly" => Ok(Mode::Assembly),
                    _ => Err(Error::InvalidOption(
                        grammar_index,
                        OPTION_MODE.to_string(),
                        vec![
                            String::from("sources"),
                            String::from("all"),
                            String::from("assembly"),
                        ]
                    ))
                },
                None => Ok(Mode::Sources)
            }
        }
    }

    /// Gets the target runtime for the grammar
    pub fn get_output_target_for(
        &self,
        grammar: &Grammar,
        grammar_index: usize
    ) -> Result<Runtime, Error> {
        match self.output_target {
            Some(target) => Ok(target),
            None => match grammar.get_option(OPTION_RUNTIME) {
                Some(option) => match option.value.as_ref() {
                    "net" => Ok(Runtime::Net),
                    "java" => Ok(Runtime::Java),
                    "rust" => Ok(Runtime::Rust),
                    _ => Err(Error::InvalidOption(
                        grammar_index,
                        OPTION_RUNTIME.to_string(),
                        vec![
                            String::from("net"),
                            String::from("java"),
                            String::from("rust"),
                        ]
                    ))
                },
                None => Ok(Runtime::Net)
            }
        }
    }

    /// Gets the path for the compiler's output
    pub fn get_output_path_for(&self, grammar: &Grammar) -> Option<String> {
        match self.output_path.as_ref() {
            Some(path) => Some(path.clone()),
            None => match grammar.get_option(OPTION_OUTPUT_PATH) {
                Some(path) => Some(path.value.clone()),
                None => None
            }
        }
    }

    /// Gets the namespace for the generated code
    pub fn get_output_namespace(&self, grammar: &Grammar) -> Option<String> {
        match self.output_namespace.as_ref() {
            Some(nmspace) => Some(nmspace.clone()),
            None => match grammar.get_option(OPTION_NAMESPACE) {
                Some(path) => Some(path.value.clone()),
                None => None
            }
        }
    }

    /// Gets the access modifier for the generated code
    pub fn get_output_modifier_for(
        &self,
        grammar: &Grammar,
        grammar_index: usize
    ) -> Result<Modifier, Error> {
        match self.output_modifier {
            Some(modifier) => Ok(modifier),
            None => match grammar.get_option(OPTION_ACCESS_MODIFIER) {
                Some(option) => match option.value.as_ref() {
                    "internal" => Ok(Modifier::Internal),
                    "public" => Ok(Modifier::Public),
                    _ => Err(Error::InvalidOption(
                        grammar_index,
                        OPTION_ACCESS_MODIFIER.to_string(),
                        vec![String::from("internal"), String::from("public")]
                    ))
                },
                None => Ok(Modifier::Internal)
            }
        }
    }

    /// Gets the parsing method
    pub fn get_parsing_method(
        &self,
        grammar: &Grammar,
        grammar_index: usize
    ) -> Result<ParsingMethod, Error> {
        match self.method {
            Some(method) => Ok(method),
            None => match grammar.get_option(OPTION_METHOD) {
                Some(option) => match option.value.as_ref() {
                    "lr0" => Ok(ParsingMethod::LR0),
                    "lr1" => Ok(ParsingMethod::LR1),
                    "lalr1" => Ok(ParsingMethod::LALR1),
                    "rnglr1" => Ok(ParsingMethod::RNGLR1),
                    "rnglalr1" => Ok(ParsingMethod::RNGLALR1),
                    _ => Err(Error::InvalidOption(
                        grammar_index,
                        OPTION_METHOD.to_string(),
                        vec![
                            String::from("lr0"),
                            String::from("lr1"),
                            String::from("lalr1"),
                            String::from("rnglr1"),
                            String::from("rnglalr1"),
                        ]
                    ))
                },
                None => Ok(ParsingMethod::LALR1)
            }
        }
    }

    /// Executes this task
    pub fn execute(&self) -> Result<LoadedData, Errors> {
        let mut data = loaders::load(&self.inputs)?;
        // select the grammars to build
        match &self.grammar_name {
            None => {}
            Some(name) => {
                data.grammars.retain(|g| &g.name == name);
                if data.grammars.is_empty() {
                    let error = Error::GrammarNotFound(name.to_string());
                    return Err(Errors::from(data, vec![error]));
                }
            }
        }
        let mut errors = Vec::new();
        // prepare the grammars
        for (index, grammar) in data.grammars.iter_mut().enumerate() {
            if let Err(mut errs) = output::execute_for_grammar(self, grammar, index) {
                errors.append(&mut errs);
            }
        }
        // output assemblies
        self.execute_output_assembly(&data, Runtime::Net, &mut errors);
        self.execute_output_assembly(&data, Runtime::Java, &mut errors);
        self.execute_output_assembly(&data, Runtime::Rust, &mut errors);
        if !errors.is_empty() {
            Err(Errors::from(data, errors))
        } else {
            Ok(data)
        }
    }

    /// Loads the data for this task
    pub fn load(&self) -> Result<LoadedData, Errors> {
        loaders::load(&self.inputs)
    }

    /// Generates the in-memory parser for a grammar
    pub fn generate_in_memory<'g>(
        &self,
        grammar: &'g mut Grammar,
        grammar_index: usize
    ) -> Result<InMemoryParser<'g>, Vec<Error>> {
        output::build_in_memory_grammar(self, grammar, grammar_index)
    }

    /// Build an assembly for the relevant grammars
    fn execute_output_assembly(&self, data: &LoadedData, target: Runtime, errors: &mut Vec<Error>) {
        // aggregate all targets for assembly
        let units = self.gather_grammars_for_assembly(data, target, errors);
        if units.is_empty() {
            return;
        }
        if let Err(error) = output::build_assembly(self, &units, target) {
            errors.push(error);
            return;
        }
        // cleanup the sources for assembly only grammars
        self.delete_sources(&units, errors);
    }

    /// Gather the grammars for build an assembly for a target
    fn gather_grammars_for_assembly<'b>(
        &self,
        data: &'b LoadedData,
        target: Runtime,
        errors: &mut Vec<Error>
    ) -> Vec<(usize, &'b Grammar)> {
        data.grammars
            .iter()
            .enumerate()
            .filter(|(index, grammar)| {
                match self.get_output_target_for(grammar, *index) {
                    Err(error) => {
                        errors.push(error);
                        return false;
                    }
                    Ok(runtime) => {
                        if runtime != target {
                            return false;
                        }
                    }
                };
                match self.get_mode_for(grammar, *index) {
                    Ok(Mode::Assembly) => true,
                    Ok(Mode::SourcesAndAssembly) => true,
                    Ok(Mode::Sources) => false,
                    Err(error) => {
                        errors.push(error);
                        false
                    }
                }
            })
            .collect()
    }

    /// Delete the sources for appropriate units
    fn delete_sources(&self, units: &[(usize, &Grammar)], errors: &mut Vec<Error>) {
        // gather all source files
        let mut all_files = Vec::new();
        for (index, grammar) in units.iter() {
            let shall_delete = match self.get_mode_for(grammar, *index) {
                Ok(Mode::Sources) => false,
                Ok(Mode::SourcesAndAssembly) => false,
                Ok(Mode::Assembly) => true,
                Err(error) => {
                    errors.push(error);
                    false
                }
            };
            if shall_delete {
                match output::get_sources(self, grammar, *index) {
                    Ok(mut sources) => all_files.append(&mut sources),
                    Err(error) => {
                        errors.push(error);
                    }
                }
            }
        }
        // cleanup output for source only targets
        for file in all_files.into_iter() {
            if let Err(error) = std::fs::remove_file(file) {
                errors.push(Error::from(error));
            }
        }
    }
}
