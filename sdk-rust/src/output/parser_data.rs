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

//! Module for writing parser LR automaton

use std::collections::HashMap;
use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

use hime_redist::parsers::{
    LR_ACTION_CODE_ACCEPT, LR_ACTION_CODE_NONE, LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_SHIFT,
    LR_OP_CODE_BASE_ADD_NULLABLE_VARIABLE, LR_OP_CODE_BASE_ADD_VIRTUAL, LR_OP_CODE_BASE_POP_STACK,
    LR_OP_CODE_BASE_SEMANTIC_ACTION
};

use crate::errors::Error;
use crate::grammars::{
    Grammar, Rule, RuleRef, SymbolRef, TerminalRef, TerminalSet, GENERATED_AXIOM
};
use crate::lr::{Graph, State};
use crate::output::helper::{write_u16, write_u32, write_u8};

/// Writes the data for a LR(k) parser
pub fn write_parser_lrk_data_file(
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
    write_parser_lrk_data(&mut writer, grammar, expected, graph)
}

/// Writes the data for a LR(k) parser
pub fn write_parser_lrk_data(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet,
    graph: &Graph
) -> Result<(), Error> {
    let mut rules = Vec::new();
    for variable in grammar.variables.iter() {
        for i in 0..variable.rules.len() {
            rules.push(RuleRef::new(variable.id, i));
        }
    }
    // number of columns
    write_u16(writer, (expected.len() + grammar.variables.len()) as u16)?;
    // number of states
    write_u16(writer, graph.states.len() as u16)?;
    // number of rules
    write_u16(writer, rules.len() as u16)?;

    write_parser_column_headers(writer, grammar, expected)?;
    write_parser_opening_contexts(writer, graph)?;

    // write the LR table
    for state in graph.states.iter() {
        write_parser_lrk_data_state(writer, grammar, expected, &rules, state)?;
    }
    // write production rules
    for variable in grammar.variables.iter() {
        for rule in variable.rules.iter() {
            write_parser_lrk_data_rule(writer, grammar, rule)?;
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
    write_u16(writer, LR_ACTION_CODE_NONE)?;
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
    write_u8(writer, rule.head_action as u8)?;
    write_u8(writer, rule.body.choices[0].len() as u8)?;
    let length = rule
        .body
        .elements
        .iter()
        .map(|element| match element.symbol {
            SymbolRef::Virtual(_) => 2,
            SymbolRef::Action(_) => 2,
            _ => 1
        })
        .sum();
    write_u8(writer, length)?;
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
pub fn write_parser_rnglr_data_file(
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
    write_parser_rnglr_data(&mut writer, grammar, expected, graph)
}

/// Writes the data for a RNGLR parser
pub fn write_parser_rnglr_data(
    writer: &mut dyn Write,
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

    // index of the axiom variable
    write_u16(writer, axiom_index as u16)?;
    // nb of colimns
    write_u16(writer, (expected.len() + grammar.variables.len()) as u16)?;
    // nb of rows
    write_u16(writer, graph.states.len() as u16)?;
    // nb of actions
    write_u32(writer, total)?;
    // nb of rules
    write_u16(writer, rules.len() as u16)?;
    // nb of nullables
    write_u16(writer, nullables.len() as u16)?;

    write_parser_column_headers(writer, grammar, expected)?;
    write_parser_opening_contexts(writer, graph)?;

    //write the offset tables
    for (count, offset) in counts.into_iter().zip(offsets.into_iter()) {
        write_u16(writer, count)?;
        write_u32(writer, offset)?;
    }

    for state in graph.states.iter() {
        write_parser_rnglr_data_action_table(writer, expected, grammar, &rules, state)?;
    }

    for (rule_ref, length) in rules.into_iter() {
        write_parser_rnglr_data_rule(writer, grammar, rule_ref.get_rule_in(grammar), length)?;
    }

    // write the indexes for nullables production
    for index in nullables.into_iter() {
        write_u16(writer, index as u16)?;
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
    write_u8(writer, rule.head_action as u8)?;
    write_u8(writer, length as u8)?;
    let mut bcl = 0;
    let mut pop = 0;
    for element in rule.body.elements.iter() {
        match element.symbol {
            SymbolRef::Virtual(_) => {
                bcl += 2;
            }
            SymbolRef::Action(_) => {
                bcl += 2;
            }
            _ => {
                if pop >= length {
                    bcl += 2;
                } else {
                    bcl += 1;
                    pop += 1;
                }
            }
        };
    }
    write_u8(writer, bcl)?;
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
