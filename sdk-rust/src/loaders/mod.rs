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

//! Loodaing facilities for grammars

pub mod parser;

use crate::grammars::Grammar;
use ansi_term::Colour::{Blue, Red};
use ansi_term::Style;
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_redist::text::TextContext;
use hime_redist::text::TextPosition;
use std::fs;
use std::io;

/// Represents an error for the loader
#[derive(Clone)]
pub struct LoaderError {
    /// The name of the originating file
    pub filename: String,
    /// The position in the file
    pub position: TextPosition,
    /// The context for the error
    pub context: TextContext,
    /// The error message
    pub message: String
}

impl LoaderError {
    /// Get the width of the line number of this error in number of characters
    pub fn line_number_width(&self) -> usize {
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

    /// Prints this error
    pub fn print(&self, max_width: usize) {
        error!(
            "{}{} {}",
            Red.bold().paint("error"),
            Style::new().bold().paint(":"),
            Style::new().bold().paint(&self.message)
        );
        let pad = String::from_utf8(vec![0x20; max_width]).unwrap();
        error!(
            "{}{} {}:{}",
            &pad,
            Blue.bold().paint("-->"),
            &self.filename,
            self.position
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        error!("{}{}", &pad, Blue.bold().paint("|"));
        let pad = String::from_utf8(vec![0x20; max_width + 1 - self.line_number_width()]).unwrap();
        error!(
            "{}{}{}  {}",
            Blue.bold().paint(format!("{}", self.position.line)),
            &pad,
            Blue.bold().paint("|"),
            &self.context.content
        );
        let pad = String::from_utf8(vec![0x20; max_width + 1]).unwrap();
        error!(
            "{}{}  {}",
            &pad,
            Blue.bold().paint("|"),
            Red.bold().paint(&self.context.pointer)
        );
        error!("");
    }
}

/// Prints the specified errors
pub fn print_errors(errors: &[LoaderError]) {
    if let Some(max_width) = errors.iter().map(|error| error.line_number_width()).max() {
        for error in errors.iter() {
            error.print(max_width);
        }
    }
}

/// Loads all inputs into grammars
pub fn load(inputs: &[String]) -> Result<Vec<Grammar>, ()> {
    let results = parse_inputs(inputs)?;
    let (mut completed, mut to_resolve): (Vec<Loader>, Vec<Loader>) = inputs
        .iter()
        .zip(results.into_iter())
        .map(|(input, result)| Loader::new(input.clone(), result))
        .partition(|loader| loader.is_solved());
    resolve_inheritance(&mut completed, &mut to_resolve)?;
    Ok(completed.into_iter().map(|loader| loader.grammar).collect())
}

/// Resolves inheritance and load grammars
fn resolve_inheritance(
    completed: &mut Vec<Loader>,
    to_resolve: &mut Vec<Loader>
) -> Result<(), ()> {
    loop {
        let mut modified = false;
        let mut finished_on_round = Vec::new();
        for mut target in to_resolve.drain(0..to_resolve.len()) {
            modified |= target.load(completed);
            if target.is_solved() {
                completed.push(target);
            } else {
                finished_on_round.push(target);
            }
        }
        to_resolve.append(&mut finished_on_round);
        if to_resolve.is_empty() {
            return Ok(());
        }
        if !modified {
            let mut errors = Vec::new();
            for loader in to_resolve.iter() {
                loader.collect_errors(&to_resolve, &mut errors);
            }
            print_errors(&errors);
            return Err(());
        }
    }
}

/// Parses the specified input
fn parse_input(input: &str) -> Result<ParseResult, ()> {
    info!("Reading input {} ...", input);
    let file = match fs::File::open(input) {
        Ok(file) => file,
        Err(error) => {
            error!("Failed to open {}: {:?}", input, &error);
            return Err(());
        }
    };
    let mut reader = io::BufReader::new(file);
    let result = parser::parse_utf8(&mut reader);
    let errors: Vec<LoaderError> = result
        .errors
        .errors
        .iter()
        .map(|error| {
            let position = error.get_position();
            let context = result.text.get_context_for(position, error.get_length());
            LoaderError {
                filename: input.to_string(),
                position,
                context,
                message: error.get_message()
            }
        })
        .collect();
    print_errors(&errors);
    if errors.is_empty() {
        Ok(result)
    } else {
        Err(())
    }
}

/// Parses all inputs
fn parse_inputs(inputs: &[String]) -> Result<Vec<ParseResult>, ()> {
    let mut results = Vec::new();
    let mut has_errors = false;
    for input in inputs.iter() {
        match parse_input(input) {
            Ok(result) => {
                results.push(result);
            }
            Err(_) => {
                has_errors = true;
            }
        }
    }
    if has_errors {
        Err(())
    } else {
        Ok(results)
    }
}

/// Represents a loader for a grammar
struct Loader {
    /// The name of the resource containing the data that are loaded by this instance
    resource: String,
    /// The parse result
    result: ParseResult,
    /// Lists of the inherited grammars
    inherited: Vec<String>,
    /// The resulting grammar
    grammar: Grammar,
    /// Flag for the global casing of the grammar
    case_insensitive: bool
}

impl Loader {
    /// Creates a new loader
    fn new(resource: String, result: ParseResult) -> Loader {
        let ast = result.get_ast();
        let root = ast.get_root();
        let name = root.children().at(0).get_value().unwrap();
        let inherited = root
            .children()
            .at(1)
            .children()
            .iter()
            .map(|node| node.get_value().unwrap())
            .collect();
        let mut loader = Loader {
            resource,
            result,
            inherited,
            grammar: Grammar::new(name),
            case_insensitive: false
        };
        if loader.is_solved() {
            loader.load_content();
        }
        loader
    }

    /// Prints errors for the unresolved inherited grammars
    fn collect_errors(&self, unresolved: &[Loader], errors: &mut Vec<LoaderError>) {
        let ast = self.result.get_ast();
        let root = ast.get_root();
        for node in root.children().at(1).children().iter() {
            let name = node.get_value().unwrap();
            if self.inherited.contains(&name) {
                // was not resolved
                if unresolved.iter().all(|l| l.grammar.name != name) {
                    // the dependency does not exist
                    errors.push(LoaderError {
                        filename: self.resource.clone(),
                        position: node.get_position().unwrap(),
                        context: node.get_context().unwrap(),
                        message: format!("Grammar `{}` is not defined", &name)
                    });
                }
            }
        }
    }

    /// Gets a value indicating whether all dependencies are solved
    fn is_solved(&self) -> bool {
        self.inherited.is_empty()
    }

    /// Attempts to load data for the grammar
    fn load(&mut self, completed: &[Loader]) -> bool {
        let mut modified = false;
        let grammar = &mut self.grammar;
        self.inherited.retain(|parent| {
            if let Some(loader) = completed.iter().find(|l| &l.grammar.name == parent) {
                grammar.inherit(&loader.grammar);
                modified = true;
                false
            } else {
                // not found => retain
                true
            }
        });
        if self.is_solved() {
            self.load_content();
        }
        modified
    }

    /// Loads the content of the grammar
    fn load_content(&mut self) {
        info!("Loading grammar {} ...", self.grammar.name);
    }
}
