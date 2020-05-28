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

pub mod helper;
pub mod lexer_data;
pub mod lexer_java;
pub mod lexer_net;
pub mod lexer_rust;
pub mod parser_data;

use crate::errors::{Error, UnmatchableTokenError};
use crate::finite::DFA;
use crate::grammars::{Grammar, TerminalRef, TerminalSet, OPTION_SEPARATOR};
use crate::lr;
use crate::{CompilationTask, ParsingMethod, Runtime};

/// Build and output artifacts for a grammar
pub fn execute_for_grammar(
    task: &CompilationTask,
    grammar: &mut Grammar,
    grammar_index: usize
) -> Result<(), Vec<Error>> {
    if let Err(error) = grammar.prepare(grammar_index) {
        return Err(vec![error]);
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
    // Build DFA
    let dfa = grammar.build_dfa();
    // Check that no terminal match the empty string
    if dfa.states[0].is_final() {
        return Err(dfa.states[0]
            .items
            .iter()
            .map(|item| Error::TerminalMatchesEmpty(grammar_index, (*item).into()))
            .collect());
    }
    // Build the data for the lexer
    let expected = dfa.get_expected();
    let separator = match get_separator(grammar, grammar_index, &expected, &dfa) {
        Ok(separator) => separator,
        Err(error) => return Err(vec![error])
    };
    let method = match task.get_parsing_method(grammar, grammar_index) {
        Ok(method) => method,
        Err(error) => return Err(vec![error])
    };
    // Build the data for the parser
    let graph = lr::build_graph(grammar, grammar_index, &expected, &dfa, method)?;
    // write data
    let output_path = task.get_output_path_for(grammar);
    if let Err(error) = lexer_data::write_lexer_data(
        output_path.as_ref(),
        get_lexer_bin_name(grammar, runtime),
        grammar,
        &dfa,
        &expected
    ) {
        return Err(vec![error]);
    }
    if let Err(error) = match method {
        ParsingMethod::LR0 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LR1 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LALR1 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLR1 => parser_data::write_parser_rnglr_data(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLALR1 => parser_data::write_parser_rnglr_data(
            output_path.as_ref(),
            get_parser_bin_name(grammar, runtime),
            grammar,
            &expected,
            &graph
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
                &expected,
                separator,
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
                &expected,
                separator,
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
                &expected,
                separator,
                method.is_rnglr()
            ) {
                return Err(vec![error]);
            }
        }
    }
    Ok(())
}

/// Gets the separator for the grammar
fn get_separator(
    grammar: &mut Grammar,
    grammar_index: usize,
    expected: &TerminalSet,
    dfa: &DFA
) -> Result<Option<TerminalRef>, Error> {
    let option = match grammar.get_option(OPTION_SEPARATOR) {
        Some(option) => option,
        None => return Ok(None)
    };
    let terminal = match grammar.get_terminal_for_name(&option.value) {
        Some(terminal) => terminal,
        None => return Err(Error::SeparatorNotDefined(grammar_index))
    };
    let terminal_ref = TerminalRef::Terminal(terminal.id);
    // warn if the separator is context-sensitive
    if terminal.context != 0 {
        return Err(Error::SeparatorIsContextual(grammar_index, terminal_ref));
    }
    if expected.content.contains(&terminal_ref) {
        // the terminal is produced by the lexer => ok
        return Ok(Some(terminal_ref));
    }
    // the separator will not be produced by the lexer, try to investigate why
    let overriders = dfa.get_overriders(terminal_ref, 0);
    Err(Error::SeparatorCannotBeMatched(
        grammar_index,
        UnmatchableTokenError {
            terminal: terminal_ref,
            overriders
        }
    ))
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
