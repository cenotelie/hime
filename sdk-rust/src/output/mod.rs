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

use crate::errors::{Error, UnmatchableTokenError};
use crate::finite::{DFAState, DFA};
use crate::grammars::{Grammar, TerminalRef, TerminalSet, OPTION_SEPARATOR};
use crate::lr::build_graph;
use crate::CharSpan;
use crate::CompilationTask;
use hime_redist::lexers::automaton::DEAD_STATE;
use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

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
    let _graph = build_graph(grammar, grammar_index, &expected, &dfa, method)?;
    // write data
    if let Err(error) = write_lexer_data(
        task.get_output_path_for(grammar),
        format!("{}.bin", &grammar.name),
        grammar,
        &dfa,
        &expected
    ) {
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

/// Writes the lexer's data
fn write_lexer_data(
    path: Option<String>,
    file_name: String,
    grammar: &Grammar,
    dfa: &DFA,
    expected: &TerminalSet
) -> Result<(), Error> {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = File::create(final_path)?;
    let mut writer = io::BufWriter::new(file);
    // write number of states
    write_u32(&mut writer, dfa.len() as u32)?;
    // write the offsets to all the states
    let mut offset: u32 = 0;
    for state in dfa.states.iter() {
        write_u32(&mut writer, offset)?;
        // adds the length required by this state
        offset += 3 + 256; // header + transitions for [0-255] characters
        let mut current_contexts = Vec::new();
        for item in state.items.iter() {
            let terminal = grammar.get_terminal(item.priority()).unwrap();
            if !current_contexts.contains(&terminal.context) {
                current_contexts.push(terminal.context);
                // context information
                offset += 2;
            }
        }
        for transition in state.transitions.keys() {
            if transition.end >= 255 {
                // transition outside the [0-255] range
                offset += 3;
            }
        }
    }
    // write each state
    for state in dfa.states.iter() {
        write_lexer_data_state(&mut writer, grammar, expected, state)?;
    }
    Ok(())
}

/// Writes the lexer's data
fn write_lexer_data_state(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet,
    state: &DFAState
) -> Result<(), Error> {
    let mut cache = [DEAD_STATE as u16; 256];
    let mut cached: u16 = 0; // number of cached transitions
    let mut slow = Vec::new();
    for (span, next) in state.transitions.iter() {
        if span.begin <= 255 {
            cached += 1;
            let end = if span.end >= 256 {
                slow.push((CharSpan::new(256, span.end), *next));
                256
            } else {
                span.end + 1
            };
            for i in span.begin..end {
                cache[i as usize] = *next as u16;
            }
        } else {
            slow.push((*span, *next));
        }
    }
    let mut contexts = Vec::new();
    let mut matched = Vec::new();
    for item in state.items.iter() {
        let terminal = grammar.get_terminal(item.priority()).unwrap();
        if !contexts.contains(&terminal.context) {
            // this is the first time this context is found in the current DFA state
            // this is the terminal with the most priority for this context
            contexts.push(terminal.context);
            matched.push(
                expected
                    .content
                    .iter()
                    .position(|t| t.priority() == terminal.id)
                    .unwrap()
            )
        }
    }

    // write the number of matched terminals
    write_u16(writer, matched.len() as u16)?;
    // write the total number of transitions
    write_u16(writer, slow.len() as u16 + cached)?;
    // write the number of non-cached transitions
    write_u16(writer, slow.len() as u16)?;
    // write the matched terminals
    for (context, index) in contexts.into_iter().zip(matched.into_iter()) {
        write_u16(writer, context as u16)?;
        write_u16(writer, index as u16)?;
    }
    // write the cached transitions
    for value in cache.iter() {
        write_u16(writer, *value)?;
    }
    // write the non-cached transitions
    slow.sort_by_key(|(span, _)| span.begin);
    for (span, next) in slow.into_iter() {
        write_u16(writer, span.begin)?;
        write_u16(writer, span.end)?;
        write_u16(writer, next as u16)?;
    }
    Ok(())
}

/// writes a u16 to a byte stream
fn write_u16(writer: &mut dyn Write, value: u16) -> Result<(), io::Error> {
    let buffer: [u8; 2] = [(value & 0xFF) as u8, (value >> 8) as u8];
    writer.write_all(&buffer)
}

/// writes a u32 to a byte stream
fn write_u32(writer: &mut dyn Write, value: u32) -> Result<(), io::Error> {
    let buffer: [u8; 4] = [
        (value & 0xFF) as u8,
        ((value & 0x0000_FF00) >> 8) as u8,
        ((value & 0x00FF_0000) >> 16) as u8,
        (value >> 24) as u8
    ];
    writer.write_all(&buffer)
}
