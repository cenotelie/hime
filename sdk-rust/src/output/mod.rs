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
pub mod parser_data;

use crate::errors::{Error, UnmatchableTokenError};
use crate::finite::DFA;
use crate::grammars::{Grammar, TerminalRef, TerminalSet, OPTION_SEPARATOR};
use crate::lr;
use crate::{CompilationTask, ParsingMethod};

/// Build and output artifacts for a grammar
pub fn execute_for_grammar(
    task: &CompilationTask,
    grammar: &mut Grammar,
    grammar_index: usize
) -> Result<(), Vec<Error>> {
    if let Err(error) = grammar.prepare(grammar_index) {
        return Err(vec![error]);
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
    let _separator = match get_separator(grammar, grammar_index, &expected, &dfa) {
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
        format!("{}Lexer.bin", &grammar.name),
        grammar,
        &dfa,
        &expected
    ) {
        return Err(vec![error]);
    }
    if let Err(error) = match method {
        ParsingMethod::LR0 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LR1 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LALR1 => parser_data::write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLR1 => parser_data::write_parser_rnglr_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLALR1 => parser_data::write_parser_rnglr_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        )
    } {
        return Err(vec![error]);
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
