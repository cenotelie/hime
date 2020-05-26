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
use crate::grammars::{
    Grammar, Rule, RuleRef, SymbolRef, TerminalRef, TerminalSet, GENERATED_AXIOM, OPTION_SEPARATOR
};
use crate::lr::{self, Graph, State};
use crate::{CharSpan, CompilationTask, ParsingMethod};
use hime_redist::lexers::automaton::DEAD_STATE;
use hime_redist::parsers::{
    LR_ACTION_CODE_ACCEPT, LR_ACTION_CODE_NONE, LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_SHIFT,
    LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE, LR_OP_CODE_BASE_ADD_VIRTUAL, LR_OP_CODE_BASE_POP_STACK,
    LR_OP_CODE_BASE_SEMANTIC_ACTION
};
use std::collections::HashMap;
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
    if let Err(error) = write_lexer_data(
        output_path.as_ref(),
        format!("{}Lexer.bin", &grammar.name),
        grammar,
        &dfa,
        &expected
    ) {
        return Err(vec![error]);
    }
    if let Err(error) = match method {
        ParsingMethod::LR0 => write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LR1 => write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::LALR1 => write_parser_lrk_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLR1 => write_parser_rnglr_data(
            output_path.as_ref(),
            format!("{}Parser.bin", &grammar.name),
            grammar,
            &expected,
            &graph
        ),
        ParsingMethod::RNGLALR1 => write_parser_rnglr_data(
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

/// Writes the lexer's data
fn write_lexer_data(
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
            let context = grammar.get_terminal_context((*item).into());
            if !current_contexts.contains(&context) {
                current_contexts.push(context);
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

/// Writes the data for a LR(k) parser
fn write_parser_lrk_data(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    expected: &TerminalSet,
    graph: &Graph
) -> Result<(), Error> {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = File::create(final_path)?;
    let mut writer = io::BufWriter::new(file);
    let mut rules = Vec::new();
    for variable in grammar.variables.iter() {
        for i in 0..variable.rules.len() {
            rules.push(RuleRef::new(variable.id, i));
        }
    }
    // number of columns
    write_u16(
        &mut writer,
        (expected.len() + grammar.variables.len()) as u16
    )?;
    // number of states
    write_u16(&mut writer, graph.states.len() as u16)?;
    // number of rules
    write_u16(&mut writer, rules.len() as u16)?;

    write_parser_column_headers(&mut writer, grammar, expected)?;
    write_parser_opening_contexts(&mut writer, graph)?;

    // write the LR table
    for state in graph.states.iter() {
        write_parser_lrk_data_state(&mut writer, grammar, expected, &rules, state)?;
    }
    // write production rules
    for variable in grammar.variables.iter() {
        for rule in variable.rules.iter() {
            write_parser_lrk_data_rule(&mut writer, grammar, rule)?;
        }
    }
    Ok(())
}

/// Writes the column headers for a parser data
fn write_parser_column_headers(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet
) -> Result<(), Error> {
    // writes the columns's id
    for terminal_ref in expected.content.iter() {
        write_u16(writer, terminal_ref.sid() as u16)?;
    }
    for variable in grammar.variables.iter() {
        write_u16(writer, variable.id as u16)?;
    }
    Ok(())
}

/// Write the opening context informations for each setate
fn write_parser_opening_contexts(writer: &mut dyn Write, graph: &Graph) -> Result<(), Error> {
    // write context openings for each state
    for state in graph.states.iter() {
        let count: usize = state
            .opening_contexts
            .iter()
            .map(|(_, contexts)| contexts.len())
            .sum();
        write_u16(writer, count as u16)?;
        for (terminal, contexts) in state.opening_contexts.iter() {
            for context in contexts.iter() {
                write_u16(writer, terminal.sid() as u16)?;
                write_u16(writer, *context as u16)?;
            }
        }
    }
    Ok(())
}

/// Generates the parser's binary data for the provided LR state
fn write_parser_lrk_data_state(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet,
    rules: &[RuleRef],
    state: &State
) -> Result<(), Error> {
    // write action on epsilon
    if state.get_reduction_for(TerminalRef::Epsilon).is_some()
        || state.get_reduction_for(TerminalRef::NullTerminal).is_some()
    {
        write_u16(writer, LR_ACTION_CODE_ACCEPT)?;
    } else {
        write_u16(writer, LR_ACTION_CODE_NONE)?;
    }
    // write actions for terminals
    for terminal in expected.content.iter().skip(1) {
        let terminal = *terminal;
        if let Some(next) = state.children.get(&terminal.into()) {
            write_u16(writer, LR_ACTION_CODE_SHIFT)?;
            write_u16(writer, *next as u16)?;
        } else if let Some(reduction) = state.get_reduction_for(terminal) {
            let index = rules
                .iter()
                .position(|rule| rule == &reduction.rule)
                .unwrap();
            write_u16(writer, LR_ACTION_CODE_REDUCE)?;
            write_u16(writer, index as u16)?;
        } else if let Some(reduction) = state.get_reduction_for(TerminalRef::NullTerminal) {
            let index = rules
                .iter()
                .position(|rule| rule == &reduction.rule)
                .unwrap();
            write_u16(writer, LR_ACTION_CODE_REDUCE)?;
            write_u16(writer, index as u16)?;
        } else {
            write_u16(writer, LR_ACTION_CODE_NONE)?;
            write_u16(writer, LR_ACTION_CODE_NONE)?;
        }
    }
    // write actions for terminals
    for variable in grammar.variables.iter() {
        if let Some(next) = state.children.get(&SymbolRef::Variable(variable.id)) {
            write_u16(writer, LR_ACTION_CODE_SHIFT)?;
            write_u16(writer, *next as u16)?;
        } else {
            write_u16(writer, LR_ACTION_CODE_NONE)?;
            write_u16(writer, LR_ACTION_CODE_NONE)?;
        }
    }
    Ok(())
}

/// Generates the parser's binary representation of a rule production
fn write_parser_lrk_data_rule(
    writer: &mut dyn Write,
    grammar: &Grammar,
    rule: &Rule
) -> Result<(), Error> {
    let head_index = grammar
        .variables
        .iter()
        .position(|variable| variable.id == rule.head)
        .unwrap();
    write_u16(writer, head_index as u16)?;
    write_u16(writer, rule.head_action)?;
    write_u16(writer, rule.body.choices[0].len() as u16)?;
    let mut length = 0;
    for element in rule.body.elements.iter() {
        length += match element.symbol {
            SymbolRef::Virtual(_) => 2,
            SymbolRef::Action(_) => 2,
            _ => 1
        };
    }
    write_u16(writer, length)?;
    for element in rule.body.elements.iter() {
        match element.symbol {
            SymbolRef::Virtual(id) => {
                let index = grammar
                    .virtuals
                    .iter()
                    .position(|symbol| symbol.id == id)
                    .unwrap();
                write_u16(writer, LR_OP_CODE_BASE_ADD_VIRTUAL | element.action)?;
                write_u16(writer, index as u16)?;
            }
            SymbolRef::Action(id) => {
                let index = grammar
                    .actions
                    .iter()
                    .position(|symbol| symbol.id == id)
                    .unwrap();
                write_u16(writer, LR_OP_CODE_BASE_SEMANTIC_ACTION)?;
                write_u16(writer, index as u16)?;
            }
            _ => {
                write_u16(writer, LR_OP_CODE_BASE_POP_STACK | element.action)?;
            }
        }
    }
    Ok(())
}

/// Writes the data for a RNGLR parser
fn write_parser_rnglr_data(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    expected: &TerminalSet,
    graph: &Graph
) -> Result<(), Error> {
    // complete list of rules, including new ones for the right-nullable parts
    let mut rules = Vec::new();
    // index of the nullable rule for the variable with the same index
    let mut nullables = Vec::new();
    for variable in grammar.variables.iter() {
        let mut temp = Vec::new();
        for (rule_index, rule) in variable.rules.iter().enumerate() {
            // Add normal rule
            temp.push((
                RuleRef::new(variable.id, rule_index),
                rule.body.choices[0].len()
            ));
            // Look for right-nullable choices
            for i in 1..rule.body.choices[0].len() {
                if rule.body.choices[i]
                    .firsts
                    .content
                    .contains(&TerminalRef::Epsilon)
                {
                    temp.push((RuleRef::new(variable.id, rule_index), i));
                }
            }
        }
        let mut null_index: u16 = 0xFFFF;
        // nullable variable?
        if variable.firsts.content.contains(&TerminalRef::Epsilon) {
            // look for a nullable rule
            if let Some((index, _)) = temp.iter().enumerate().find(|(_, (_, l))| *l == 0) {
                // Found a 0-length reduction rule => perfect
                null_index = (rules.len() + index) as u16;
            } else {
                // no 0-length reduction rule => create a new 0-length reduction with a nullable rule
                let (rule_ref, _) = temp
                    .iter()
                    .find(|(rule_ref, _)| {
                        variable.rules[rule_ref.index].body.choices[0]
                            .firsts
                            .content
                            .contains(&TerminalRef::Epsilon)
                    })
                    .copied()
                    .unwrap();
                null_index = (rules.len() + temp.len()) as u16;
                temp.push((rule_ref, 0));
            }
        }
        // commit
        nullables.push(null_index);
        rules.append(&mut temp);
    }

    let mut total: u32 = 0;
    let mut offsets: Vec<u32> = Vec::new(); // for each state, the offset in the action table
    let mut counts: Vec<u16> = Vec::new(); // for each state, the number of actions
    for state in graph.states.iter() {
        total = write_parser_rnglr_data_generate_offset(
            expected,
            grammar,
            &mut offsets,
            &mut counts,
            total,
            state
        );
    }
    let axiom_index = grammar
        .variables
        .iter()
        .position(|variable| variable.name == GENERATED_AXIOM)
        .unwrap();

    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = File::create(final_path)?;
    let mut writer = io::BufWriter::new(file);

    // index of the axiom variable
    write_u16(&mut writer, axiom_index as u16)?;
    // nb of colimns
    write_u16(
        &mut writer,
        (expected.len() + grammar.variables.len()) as u16
    )?;
    // nb of rows
    write_u16(&mut writer, graph.states.len() as u16)?;
    // nb of actions
    write_u32(&mut writer, total)?;
    // nb of rules
    write_u16(&mut writer, rules.len() as u16)?;
    // nb of nullables
    write_u16(&mut writer, nullables.len() as u16)?;

    write_parser_column_headers(&mut writer, grammar, expected)?;
    write_parser_opening_contexts(&mut writer, graph)?;

    //write the offset tables
    for (count, offset) in counts.into_iter().zip(offsets.into_iter()) {
        write_u16(&mut writer, count)?;
        write_u32(&mut writer, offset)?;
    }

    for state in graph.states.iter() {
        write_parser_rnglr_data_action_table(&mut writer, expected, grammar, &rules, state)?;
    }

    for (rule_ref, length) in rules.into_iter() {
        write_parser_rnglr_data_rule(&mut writer, grammar, rule_ref.get_rule_in(grammar), length)?;
    }

    // write the indexes for nullables production
    for index in nullables.into_iter() {
        write_u16(&mut writer, index as u16)?;
    }

    Ok(())
}

/// Builds the offset table for the RNGLR actions
fn write_parser_rnglr_data_generate_offset(
    expected: &TerminalSet,
    grammar: &Grammar,
    offsets: &mut Vec<u32>,
    counts: &mut Vec<u16>,
    mut total: u32,
    state: &State
) -> u32 {
    let mut reductions_counter: HashMap<TerminalRef, usize> = HashMap::new();
    for reduction in state.reductions.iter() {
        let counter = reductions_counter
            .entry(reduction.lookahead.terminal)
            .or_default();
        *counter += 1;
    }
    for terminal in expected.content.iter() {
        let mut count = if state.children.contains_key(&(*terminal).into()) {
            1
        } else {
            0
        };
        if let Some(counter) = reductions_counter.get(terminal) {
            count += *counter;
        }
        offsets.push(total);
        counts.push(count as u16);
        total += count as u32;
    }
    for variable in grammar.variables.iter() {
        let count = if state
            .children
            .contains_key(&SymbolRef::Variable(variable.id))
        {
            1
        } else {
            0
        };
        offsets.push(total);
        counts.push(count as u16);
        total += count as u32;
    }
    total
}

/// Generates the action table for a state
fn write_parser_rnglr_data_action_table(
    writer: &mut dyn Write,
    expected: &TerminalSet,
    grammar: &Grammar,
    rules: &[(RuleRef, usize)],
    state: &State
) -> Result<(), Error> {
    if state.get_reduction_for(TerminalRef::Epsilon).is_some() {
        // There can be only one reduction on epsilon
        write_u16(writer, LR_ACTION_CODE_ACCEPT)?;
        write_u16(writer, LR_ACTION_CODE_NONE)?;
    }
    // write actions for terminals
    for terminal in expected.content.iter().skip(1) {
        let terminal = *terminal;
        if let Some(next) = state.children.get(&terminal.into()) {
            write_u16(writer, LR_ACTION_CODE_SHIFT)?;
            write_u16(writer, *next as u16)?;
        }
        for reduction in state
            .reductions
            .iter()
            .filter(|r| r.lookahead.terminal == terminal)
        {
            let index = rules
                .iter()
                .position(|(rule, length)| rule == &reduction.rule && *length == reduction.length)
                .unwrap();
            write_u16(writer, LR_ACTION_CODE_REDUCE)?;
            write_u16(writer, index as u16)?;
        }
    }
    for variable in grammar.variables.iter() {
        if let Some(next) = state.children.get(&SymbolRef::Variable(variable.id)) {
            write_u16(writer, LR_ACTION_CODE_SHIFT)?;
            write_u16(writer, *next as u16)?;
        }
    }
    Ok(())
}

/// Generates the parser's binary representation of a rule production
fn write_parser_rnglr_data_rule(
    writer: &mut dyn Write,
    grammar: &Grammar,
    rule: &Rule,
    length: usize
) -> Result<(), Error> {
    let head_index = grammar
        .variables
        .iter()
        .position(|variable| variable.id == rule.head)
        .unwrap();
    write_u16(writer, head_index as u16)?;
    write_u16(writer, rule.head_action)?;
    write_u16(writer, length as u16)?;
    let mut length = 0;
    let mut pop = 0;
    for element in rule.body.elements.iter() {
        match element.symbol {
            SymbolRef::Virtual(_) => {
                length += 2;
            }
            SymbolRef::Action(_) => {
                length += 2;
            }
            _ => {
                if pop >= length {
                    length += 2;
                } else {
                    length += 1;
                    pop += 1;
                }
            }
        };
    }
    write_u16(writer, length)?;
    pop = 0;
    for element in rule.body.elements.iter() {
        match element.symbol {
            SymbolRef::Virtual(id) => {
                let index = grammar
                    .virtuals
                    .iter()
                    .position(|symbol| symbol.id == id)
                    .unwrap();
                write_u16(writer, LR_OP_CODE_BASE_ADD_VIRTUAL | element.action)?;
                write_u16(writer, index as u16)?;
            }
            SymbolRef::Action(id) => {
                let index = grammar
                    .actions
                    .iter()
                    .position(|symbol| symbol.id == id)
                    .unwrap();
                write_u16(writer, LR_OP_CODE_BASE_SEMANTIC_ACTION)?;
                write_u16(writer, index as u16)?;
            }
            _ => {
                if pop >= length {
                    // Here the symbol must be a variable
                    // let variable_id = element.symbol.sid();
                    if let SymbolRef::Variable(id) = element.symbol {
                        let index = grammar
                            .variables
                            .iter()
                            .position(|symbol| symbol.id == id)
                            .unwrap();
                        write_u16(
                            writer,
                            LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE | element.action
                        )?;
                        write_u16(writer, index as u16)?;
                    }
                } else {
                    write_u16(writer, LR_OP_CODE_BASE_POP_STACK | element.action)?;
                    pop += 1;
                }
            }
        }
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
