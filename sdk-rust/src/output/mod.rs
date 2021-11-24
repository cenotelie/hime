/*******************************************************************************
 * Copyright (c) 2020 Association Cénotélie (cenotelie.fr)
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

//! Module for producing output

mod assembly_java;
mod assembly_net;
mod assembly_rust;
pub mod helper;
mod lexer_data;
mod lexer_java;
mod lexer_net;
mod lexer_rust;
mod parser_data;
mod parser_java;
mod parser_net;
mod parser_rust;

use std::env;
use std::fs::File;
use std::io::{self, Write};
use std::path::{Path, PathBuf};

use hime_redist::lexers::automaton::Automaton;
use hime_redist::parsers::lrk::LRkAutomaton;
use hime_redist::parsers::rnglr::RNGLRAutomaton;
use hime_redist::symbols::Symbol;
use rand::distributions::Alphanumeric;
use rand::{thread_rng, Rng};

use crate::errors::Error;
use crate::grammars::{BuildData, Grammar, PREFIX_GENERATED_TERMINAL};
use crate::sdk::{InMemoryParser, ParserAutomaton};
use crate::{CompilationTask, ParsingMethod, Runtime};

/// Output artifacts for a grammar
pub fn output_grammar_artifacts(
    task: &CompilationTask,
    grammar: &Grammar,
    grammar_index: usize,
    data: &BuildData
) -> Result<(), Vec<Error>> {
    // gather required options
    let mode = match task.get_mode_for(grammar, grammar_index) {
        Ok(mode) => mode,
        Err(error) => return Err(vec![error])
    };
    let runtime = match task.get_output_target_for(grammar, grammar_index) {
        Ok(runtime) => runtime,
        Err(error) => return Err(vec![error])
    };
    let nmspace = match task.get_output_namespace(grammar) {
        Some(nmspace) => nmspace,
        None => grammar.name.clone()
    };
    let nmspace = match runtime {
        Runtime::Net => helper::get_namespace_net(&nmspace),
        Runtime::Java => helper::get_namespace_java(&nmspace),
        Runtime::Rust => helper::get_namespace_rust(&nmspace)
    };
    let modifier = match task.get_output_modifier_for(grammar, grammar_index) {
        Ok(modifier) => modifier,
        Err(error) => return Err(vec![error])
    };

    // write data
    let output_path = task.get_output_path_for(grammar);
    if let Err(error) = lexer_data::write_lexer_data_file(
        output_path.as_ref(),
        get_lexer_bin_name(grammar, runtime),
        grammar,
        &data.dfa,
        &data.expected
    ) {
        return Err(vec![error]);
    }
    if let Err(error) = match data.method {
        ParsingMethod::LR0 => parser_data::write_parser_lrk_data_file(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &data.expected,
            &data.graph
        ),
        ParsingMethod::LR1 => parser_data::write_parser_lrk_data_file(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &data.expected,
            &data.graph
        ),
        ParsingMethod::LALR1 => parser_data::write_parser_lrk_data_file(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &data.expected,
            &data.graph
        ),
        ParsingMethod::RNGLR1 => parser_data::write_parser_rnglr_data_file(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &data.expected,
            &data.graph
        ),
        ParsingMethod::RNGLALR1 => parser_data::write_parser_rnglr_data_file(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &data.expected,
            &data.graph
        )
    } {
        return Err(vec![error]);
    }
    // write code
    match runtime {
        Runtime::Net => {
            if let Err(error) = lexer_net::write(
                output_path.as_ref(),
                format!("{}Lexer.cs", helper::to_upper_camel_case(&grammar.name)),
                grammar,
                &data.expected,
                data.separator,
                &nmspace,
                modifier
            ) {
                return Err(vec![error]);
            }
            if let Err(error) = parser_net::write(
                output_path.as_ref(),
                format!("{}Parser.cs", helper::to_upper_camel_case(&grammar.name)),
                grammar,
                &data.expected,
                data.method,
                &nmspace,
                modifier
            ) {
                return Err(vec![error]);
            }
        }
        Runtime::Java => {
            if let Err(error) = lexer_java::write(
                output_path.as_ref(),
                format!("{}Lexer.java", helper::to_upper_camel_case(&grammar.name)),
                grammar,
                &data.expected,
                data.separator,
                &nmspace,
                modifier
            ) {
                return Err(vec![error]);
            }
            if let Err(error) = parser_java::write(
                output_path.as_ref(),
                format!("{}Parser.java", helper::to_upper_camel_case(&grammar.name)),
                grammar,
                &data.expected,
                data.method,
                &nmspace,
                modifier
            ) {
                return Err(vec![error]);
            }
        }
        Runtime::Rust => {
            if let Err(error) = lexer_rust::write(
                output_path.as_ref(),
                format!("{}.rs", helper::to_snake_case(&grammar.name)),
                grammar,
                &data.expected,
                data.separator,
                data.method.is_rnglr()
            ) {
                return Err(vec![error]);
            }
            if let Err(error) = parser_rust::write(
                output_path.as_ref(),
                format!("{}.rs", helper::to_snake_case(&grammar.name)),
                grammar,
                &data.expected,
                data.method,
                &nmspace,
                mode.output_assembly()
            ) {
                return Err(vec![error]);
            }
        }
    }
    Ok(())
}

/// Builds the in-memory parser for a grammar
pub fn build_in_memory_grammar<'a>(
    grammar: &'a Grammar,
    data: &BuildData
) -> Result<InMemoryParser<'a>, Vec<Error>> {
    // get symbols
    let mut terminals: Vec<Symbol<'a>> = Vec::new();
    for terminal_ref in data.expected.content.iter().skip(2) {
        let terminal = grammar.get_terminal(terminal_ref.sid()).unwrap();
        if terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        } else {
            terminals.push(Symbol {
                id: terminal.id as u32,
                name: &terminal.name
            });
        }
    }
    let variables: Vec<Symbol<'a>> = grammar
        .variables
        .iter()
        .map(|variable| Symbol {
            id: variable.id as u32,
            name: &variable.name
        })
        .collect();
    let virtuals: Vec<Symbol<'a>> = grammar
        .virtuals
        .iter()
        .map(|symbol| Symbol {
            id: symbol.id as u32,
            name: &symbol.name
        })
        .collect();

    // build automata
    let mut lexer_automaton = Vec::new();
    if let Err(error) =
        lexer_data::write_lexer_data(&mut lexer_automaton, grammar, &data.dfa, &data.expected)
    {
        return Err(vec![error]);
    }
    let mut parser_automaton = Vec::new();
    if let Err(error) = if data.method.is_rnglr() {
        parser_data::write_parser_lrk_data(
            &mut parser_automaton,
            grammar,
            &data.expected,
            &data.graph
        )
    } else {
        parser_data::write_parser_rnglr_data(
            &mut parser_automaton,
            grammar,
            &data.expected,
            &data.graph
        )
    } {
        return Err(vec![error]);
    }

    Ok(InMemoryParser {
        name: &grammar.name,
        terminals,
        variables,
        virtuals,
        separator: match data.separator {
            None => 0xFFFF,
            Some(terminal_ref) => terminal_ref.sid() as u32
        },
        lexer_automaton: Automaton::new(&lexer_automaton),
        lexer_is_context_sensitive: grammar.contexts.len() > 1,
        parser_automaton: if data.method.is_rnglr() {
            ParserAutomaton::Rnglr(RNGLRAutomaton::new(&parser_automaton))
        } else {
            ParserAutomaton::Lrk(LRkAutomaton::new(&parser_automaton))
        }
    })
}

/// Gets the list of sources to produce for a grammar
pub fn get_sources(
    task: &CompilationTask,
    grammar: &Grammar,
    grammar_index: usize
) -> Result<Vec<PathBuf>, Error> {
    let runtime = task.get_output_target_for(grammar, grammar_index)?;
    let output_path = task.get_output_path_for(grammar);
    Ok(match runtime {
        Runtime::Net => vec![
            build_file(
                output_path.as_ref(),
                format!("{}Lexer.cs", helper::to_upper_camel_case(&grammar.name))
            ),
            build_file(
                output_path.as_ref(),
                format!("{}Parser.cs", helper::to_upper_camel_case(&grammar.name))
            ),
            build_file(output_path.as_ref(), get_lexer_bin_name_net(grammar)),
            build_file(output_path.as_ref(), get_parser_bin_name_net(grammar)),
        ],
        Runtime::Java => vec![
            build_file(
                output_path.as_ref(),
                format!("{}Lexer.java", helper::to_upper_camel_case(&grammar.name))
            ),
            build_file(
                output_path.as_ref(),
                format!("{}Parser.java", helper::to_upper_camel_case(&grammar.name))
            ),
            build_file(output_path.as_ref(), get_lexer_bin_name_java(grammar)),
            build_file(output_path.as_ref(), get_parser_bin_name_java(grammar)),
        ],
        Runtime::Rust => vec![
            build_file(
                output_path.as_ref(),
                format!("{}.rs", helper::to_snake_case(&grammar.name))
            ),
            build_file(output_path.as_ref(), get_lexer_bin_name_rust(grammar)),
            build_file(output_path.as_ref(), get_parser_bin_name_rust(grammar)),
        ]
    })
}

/// Build a path buf for an output file
fn build_file(path: Option<&String>, file_name: String) -> PathBuf {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    final_path
}

/// Builds an assembly for a runtime
pub fn build_assembly(
    task: &CompilationTask,
    units: &[(usize, &Grammar)],
    runtime: Runtime
) -> Result<(), Error> {
    match runtime {
        Runtime::Net => assembly_net::build(task, units),
        Runtime::Java => assembly_java::build(task, units),
        Runtime::Rust => assembly_rust::build(task, units)
    }
}

/// Gets the name of the file for the lexer automaton
fn get_lexer_bin_name(grammar: &Grammar, runtime: Runtime) -> String {
    match runtime {
        Runtime::Net => get_lexer_bin_name_net(grammar),
        Runtime::Java => get_lexer_bin_name_java(grammar),
        Runtime::Rust => get_lexer_bin_name_rust(grammar)
    }
}

/// Gets the name of the file for the lexer automaton in .Net
fn get_lexer_bin_name_net(grammar: &Grammar) -> String {
    format!("{}Lexer.bin", helper::to_upper_camel_case(&grammar.name))
}

/// Gets the name of the file for the lexer automaton in Java
fn get_lexer_bin_name_java(grammar: &Grammar) -> String {
    format!("{}Lexer.bin", helper::to_upper_camel_case(&grammar.name))
}

/// Gets the name of the file for the lexer automaton in Rust
fn get_lexer_bin_name_rust(grammar: &Grammar) -> String {
    format!("{}_lexer.bin", helper::to_snake_case(&grammar.name))
}

/// Gets the name of the file for the parser automaton
fn get_parser_bin_name(grammar: &Grammar, runtime: Runtime) -> String {
    match runtime {
        Runtime::Net => get_parser_bin_name_net(grammar),
        Runtime::Java => get_parser_bin_name_java(grammar),
        Runtime::Rust => get_parser_bin_name_rust(grammar)
    }
}

/// Gets the name of the file for the parser automaton in .Net
fn get_parser_bin_name_net(grammar: &Grammar) -> String {
    format!("{}Parser.bin", helper::to_upper_camel_case(&grammar.name))
}

/// Gets the name of the file for the parser automaton in Java
fn get_parser_bin_name_java(grammar: &Grammar) -> String {
    format!("{}Parser.bin", helper::to_upper_camel_case(&grammar.name))
}

/// Gets the name of the file for the parser automaton in Rust
fn get_parser_bin_name_rust(grammar: &Grammar) -> String {
    format!("{}_parser.bin", helper::to_snake_case(&grammar.name))
}

/// Creates a temp folder
pub fn temporary_folder() -> PathBuf {
    let mut result = env::temp_dir();
    let name: String = thread_rng().sample_iter(&Alphanumeric).take(10).collect();
    result.push(name);
    result
}

/// Export a resource a target file
fn export_resource(folder: &Path, file_name: &str, content: &[u8]) -> Result<(), Error> {
    let mut target = folder.to_path_buf();
    target.push(file_name);
    let file = File::create(target)?;
    let mut writer = io::BufWriter::new(file);
    writer.write_all(content)?;
    Ok(())
}
