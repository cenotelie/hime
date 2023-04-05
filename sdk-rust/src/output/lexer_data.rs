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

//! Module for writing lexer automaton

use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

use hime_redist::lexers::automaton::DEAD_STATE;

use crate::errors::Error;
use crate::finite::{DFAState, DFA};
use crate::grammars::{Grammar, TerminalRef, TerminalSet};
use crate::output::helper::{write_u16, write_u32};
use crate::CharSpan;

/// Writes the lexer's data
pub fn write_lexer_data_file(
    path: Option<&String>,
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
    write_lexer_data(&mut writer, grammar, dfa, expected)
}

/// Writes the lexer's data
#[allow(clippy::cast_possible_truncation, clippy::module_name_repetitions)]
pub fn write_lexer_data(
    writer: &mut dyn Write,
    grammar: &Grammar,
    dfa: &DFA,
    expected: &TerminalSet
) -> Result<(), Error> {
    // write number of states
    write_u32(writer, dfa.len() as u32)?;
    // write the offsets to all the states
    let mut offset: u32 = 0;
    for state in &dfa.states {
        write_u32(writer, offset)?;
        // adds the length required by this state
        offset += 3 + 256; // header + transitions for [0-255] characters
        let mut current_contexts = Vec::new();
        for item in &state.items {
            let context = grammar.get_terminal_context((*item).into());
            if !current_contexts.contains(&context) {
                current_contexts.push(context);
                // context information
                offset += 2;
            }
        }
        for transition in state.transitions.keys() {
            if transition.end >= 256 {
                // transition outside the [0-255] range
                offset += 3;
            }
        }
    }
    // write each state
    for state in &dfa.states {
        write_lexer_data_state(writer, grammar, expected, state)?;
    }
    Ok(())
}

/// Writes the lexer's data
#[allow(clippy::cast_possible_truncation)]
fn write_lexer_data_state(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet,
    state: &DFAState
) -> Result<(), Error> {
    let mut cache = [DEAD_STATE as u16; 256];
    let mut cached: u16 = 0; // number of cached transitions
    let mut slow = Vec::new();
    for (span, next) in &state.transitions {
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
    for item in &state.items {
        let terminal = grammar.get_terminal(item.sid()).unwrap();
        let terminal_ref = TerminalRef::Terminal(terminal.id);
        if !contexts.contains(&terminal.context) {
            // this is the first time this context is found in the current DFA state
            // this is the terminal with the most priority for this context
            contexts.push(terminal.context);
            matched.push(
                expected
                    .content
                    .iter()
                    .position(|t| t == &terminal_ref)
                    .unwrap()
            );
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
    for (span, next) in slow {
        write_u16(writer, span.begin)?;
        write_u16(writer, span.end)?;
        write_u16(writer, next as u16)?;
    }
    Ok(())
}
