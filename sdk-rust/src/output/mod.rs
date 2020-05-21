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

use crate::errors::Error;
use crate::finite::{FinalItem, DFA};
use crate::grammars::{Grammar, TerminalRef, TerminalSet, OPTION_SEPARATOR};
use crate::lr::build_graph;
use crate::CompilationTask;

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
            .map(|item| {
                Error::TerminalMatchesEmpty(grammar_index, TerminalRef::Terminal(item.priority()))
            })
            .collect());
    }
    // Build the list of expected terminals
    let expected = dfa.get_expected();
    let _separator = match get_separator(grammar, grammar_index, &expected, &dfa) {
        Ok(separator) => separator,
        Err(error) => return Err(vec![error])
    };
    let method = match task.get_parsing_method(grammar, grammar_index) {
        Ok(method) => method,
        Err(error) => return Err(vec![error])
    };
    let _graph = build_graph(grammar, grammar_index, method)?;
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
    let mut overriders = TerminalSet::default();
    let separator_final = FinalItem::Terminal(terminal.id);
    for state in dfa.states.iter() {
        if state.items.contains(&separator_final) {
            // separator is final of this state
            for item in state.items.iter().rev() {
                if item == &separator_final {
                    break;
                }
                // this final item has more priority than the separator
                overriders.add(TerminalRef::Terminal(item.priority()));
            }
        }
    }
    Err(Error::SeparatorCannotBeMatched(
        grammar_index,
        terminal_ref,
        overriders.content
    ))
}
