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

//! Module for loading Hime grammars

pub mod parser;

use std::fs;
use std::io::Read;

use super::Report;
use grammar::Grammar;

use hime_redist::ast::AstNode;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_redist::utils::iterable::Iterable;


/// Represents a loader for a single grammar
pub struct GrammarLoader<'i> {
    /// The name of the resource containing the data that are loaded by this instance
    resource: String,
    /// The input from which the grammar is loaded
    input: &'i ParseResult,
    /// The root to load from
    root: &'i AstNode<'i>,
    /// Lists of the inherited grammars
    inherited: Vec<String>,
    /// The resulting grammar
    grammar: Grammar,
    /// Flag for the global casing of the grammar
    case_insensitive: bool
}

impl<'i> GrammarLoader<'i> {
    /// Initializes this load
    pub fn new(
        resource: String,
        input: &'i ParseResult,
        root: &'i AstNode<'i>
    ) -> GrammarLoader<'i> {
        let mut inherited = Vec::<String>::new();
        for child in root.children().at(1).children().iter() {
            inherited.push(child.get_value().unwrap());
        }
        let grammar = Grammar::new(root.children().at(0).get_value().unwrap());
        GrammarLoader {
            resource,
            input,
            root,
            inherited,
            grammar,
            case_insensitive: false
        }
    }

    /// Gets whether all dependencies are solved
    pub fn is_solved(&self) -> bool {
        self.inherited.is_empty()
    }

    /// Gets the remaining unsolved dependencies
    pub fn dependencies(&self) -> &Vec<String> {
        &self.inherited
    }

    /// Consumes into the resulting grammar
    pub fn into_grammar(self) -> Grammar {
        self.grammar
    }
}


/// Represents a loader of inputs that produces grammars
pub struct InputLoader<'r, 'i> {
    /// Next unique identifier for raw (anonymous) inputs
    next_id: usize,
    /// The report to use for events
    report: &'r mut Report,
    /// the input files
    input_files: Vec<&'i str>,
    /// The input streams
    input_streams: Vec<(String, &'i mut Read)>,
    /// The pre-parsed inputs
    input_parsed: Vec<(String, &'i ParseResult, &'i AstNode<'i>)>,
    /// Repositories of inner loaders
    parts: Vec<GrammarLoader<'r, 'i>>
}

impl<'r, 'i> InputLoader<'r, 'i> {
    /// Creates a new loader
    pub fn new(report: &'r mut Report) -> InputLoader<'r, 'i> {
        InputLoader {
            next_id: 0,
            report,
            input_files: Vec::<&'i str>::new(),
            input_streams: Vec::<(String, &'i mut Read)>::new(),
            input_parsed: Vec::<(String, &'i ParseResult, &'i AstNode<'i>)>::new(),
            parts: Vec::<GrammarLoader<'r, 'i>>::new()
        }
    }

    /// Adds a new file as input
    pub fn add_input_file(&mut self, file: &'i str) {
        self.input_files.push(file);
    }

    /// Adds a new stream as input
    pub fn add_input_stream(&mut self, name: &str, stream: &'i mut Read) {
        self.input_streams.push((name.to_owned(), stream));
    }

    /// Adds a new pre-parsed input
    pub fn add_input_parsed(&mut self, name: &str, input: &'i ParseResult, root: &'i AstNode<'i>) {
        self.input_parsed.push((name.to_owned(), input, root));
    }

    /// Parses the inputs and loads the grammars
    pub fn into_grammars(self) -> Option<Vec<Grammar>> {
        for file_name in self.input_files.iter() {
            let maybe_file = fs::File::open(*file_name);
            match maybe_file {
                Err(e) => {
                    self.report.error(e.to_string());
                },
                Ok(mut file) => {
                    let result = parser::parse_utf8(&mut file);
                }
            }
        }
        None
    }


}