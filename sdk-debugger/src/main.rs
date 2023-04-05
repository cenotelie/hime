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

//! Main module

use std::env;
use std::error::Error;
use std::fs::File;
use std::io::{BufReader, Read};

use clap::{Arg, ArgMatches, Command};
use hime_redist::lexers::automaton::{Automaton, DEAD_STATE};
use hime_redist::parsers::lrk::LRkAutomaton;
use hime_redist::parsers::{
    get_op_code_base, get_op_code_tree_action, LR_ACTION_CODE_ACCEPT, LR_ACTION_CODE_NONE,
    LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_SHIFT, LR_OP_CODE_BASE_ADD_VIRTUAL,
    LR_OP_CODE_BASE_POP_STACK, LR_OP_CODE_BASE_SEMANTIC_ACTION, TREE_ACTION_DROP,
    TREE_ACTION_PROMOTE, TREE_ACTION_REPLACE_BY_CHILDREN, TREE_ACTION_REPLACE_BY_EPSILON
};

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

/// Main entry point
fn main() -> Result<(), Box<dyn Error>> {
    let matches = Command::new("Hime SDK Debugger")
        .version(format!("{CRATE_NAME} {CRATE_VERSION} tag={GIT_TAG} hash={GIT_HASH}").as_str())
        .author("Association Cénotélie <contact@cenotelie.fr>")
        .about("Debugger for the SDK.")
        .subcommand(
            Command::new("print")
                .override_help("Print compilation artifacts")
                .subcommand(
                    Command::new("lexer")
                        .override_help("Prints a lexer's automaton")
                        .arg(
                            Arg::new("lexer_file")
                                .value_name("FILE")
                                .help("Path to the lexer's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                )
                .subcommand(
                    Command::new("parser")
                        .override_help("Prints a parser's automaton")
                        .arg(
                            Arg::new("glr")
                                .long("glr")
                                .help("Whether this is a GLR automaton")
                                .takes_value(false)
                                .required(false)
                        )
                        .arg(
                            Arg::new("show_bytecode")
                                .short('b')
                                .long("bytecode")
                                .help("Activate to show the bytecode of reductions")
                                .takes_value(false)
                                .required(false)
                        )
                        .arg(
                            Arg::new("parser_file")
                                .value_name("FILE")
                                .help("Path to the parser's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                )
        )
        .subcommand(
            Command::new("diff")
                .override_help("Computes the diff between two artifacts")
                .subcommand(
                    Command::new("lexer")
                        .override_help("Computes the diff between two lexer automaton")
                        .arg(
                            Arg::new("file_left")
                                .value_name("FROM")
                                .help("Path to a lexer's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                        .arg(
                            Arg::new("file_right")
                                .value_name("TO")
                                .help("Path to a lexer's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                )
                .subcommand(
                    Command::new("parser")
                        .override_help("Computes the diff between two parser automaton")
                        .arg(
                            Arg::new("glr")
                                .long("glr")
                                .help("Whether this is a GLR automaton")
                                .takes_value(false)
                                .required(false)
                        )
                        .arg(
                            Arg::new("file_left")
                                .value_name("FROM")
                                .help("Path to a lexer's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                        .arg(
                            Arg::new("file_right")
                                .value_name("TO")
                                .help("Path to a lexer's automaton")
                                .takes_value(true)
                                .required(true)
                        )
                )
        )
        .get_matches();
    execute(&matches)
}

/// Executes the command
fn execute(matches: &ArgMatches) -> Result<(), Box<dyn Error>> {
    match matches.subcommand() {
        Some(("print", matches)) => match matches.subcommand() {
            Some(("lexer", matches)) => {
                let file_name = matches.value_of("lexer_file").unwrap();
                print_lexer(file_name)?;
            }
            Some(("parser", matches)) => {
                let file_name = matches.value_of("parser_file").unwrap();
                let is_glr = matches.is_present("glr");
                let show_bytecode = matches.is_present("show_bytecode");
                print_parser(file_name, is_glr, show_bytecode)?;
            }
            _ => panic!("invalid command")
        },
        Some(("diff", matches)) => match matches.subcommand() {
            Some(("lexer", matches)) => {
                let file_left = matches.value_of("file_left").unwrap();
                let file_right = matches.value_of("file_right").unwrap();
                diff_lexer(file_left, file_right)?;
            }
            Some(("parser", matches)) => {
                let is_glr = matches.is_present("glr");
                let file_left = matches.value_of("file_left").unwrap();
                let file_right = matches.value_of("file_right").unwrap();
                diff_parser(file_left, file_right, is_glr)?;
            }
            _ => panic!("invalid command")
        },
        _ => panic!("invalid command")
    }
    Ok(())
}

/// Prints a lexer's automaton
fn print_lexer(file_name: &str) -> Result<(), Box<dyn Error>> {
    let automaton = load_automaton_lexer(file_name)?;
    for (i, state) in automaton.get_states().enumerate() {
        print!("state {i}");
        let terminals = state.get_terminals_count();
        for i in 0..terminals {
            if i == 0 {
                print!(" [");
            } else {
                print!(", ");
            }
            let terminal = state.get_terminal(i);
            print!("0x{:X}", terminal.index);
            if terminal.context != 0 {
                print!(" ({})", terminal.context);
            }
        }
        if terminals > 0 {
            print!("]");
        }
        println!();
        for i in 0..256 {
            let next = state.get_cached_transition(i);
            if next != DEAD_STATE {
                println!(
                    "    0x{:X} ('{}') -> {}",
                    i,
                    char::from_u32(u32::from(i)).unwrap(),
                    next
                );
            }
        }
    }
    Ok(())
}

/// Prints a parser's automaton
fn print_parser(file_name: &str, is_glr: bool, show_bytecode: bool) -> Result<(), Box<dyn Error>> {
    if is_glr {
        print_parser_glr(file_name, show_bytecode)
    } else {
        print_parser_lrk(file_name, show_bytecode)
    }
}

/// Prints a parser's automaton
#[allow(clippy::cast_possible_truncation)]
fn print_parser_lrk(file_name: &str, show_bytecode: bool) -> Result<(), Box<dyn Error>> {
    let automaton = load_automaton_lrk(file_name)?;
    let states = automaton.get_states_count() as u32;
    let columns = automaton.get_columns_count();
    for state in 0..states {
        let contexts = automaton.get_contexts(state);
        println!("state {state}");
        for c in 0..columns {
            let action = automaton.get_action_at(state, c);
            match action.get_code() {
                LR_ACTION_CODE_SHIFT => {
                    print!("    on 0x{:X}, shift to {}", c, action.get_data());
                    if let Some(context) =
                        contexts.get_context_opened_by(automaton.get_sid_for_column(c))
                    {
                        print!(", open context {context}");
                    }
                    println!();
                }
                LR_ACTION_CODE_REDUCE => {
                    let reduction = automaton.get_production(action.get_data() as usize);
                    print!(
                        "    on 0x{:X}, reduce 0x{:X} by {}",
                        c, reduction.head, reduction.reduction_length
                    );
                    match reduction.head_action {
                        TREE_ACTION_DROP => print!(" and drop"),
                        TREE_ACTION_PROMOTE => print!(" and promote"),
                        TREE_ACTION_REPLACE_BY_CHILDREN => print!(" and replace by children"),
                        TREE_ACTION_REPLACE_BY_EPSILON => print!(" and replace by epsilon"),
                        _ => {}
                    }
                    if let Some(context) =
                        contexts.get_context_opened_by(automaton.get_sid_for_column(c))
                    {
                        print!(", open context {context}");
                    }
                    println!();
                    if show_bytecode {
                        print!("    +-> ");
                        let mut i = 0;
                        while i < reduction.bytecode.len() {
                            if i > 0 {
                                print!(" ");
                            }
                            let op_code = reduction.bytecode[i];
                            i += 1;
                            match get_op_code_base(op_code) {
                                LR_OP_CODE_BASE_POP_STACK => {
                                    print!("pop");
                                    match get_op_code_tree_action(op_code) {
                                        TREE_ACTION_DROP => print!("!"),
                                        TREE_ACTION_PROMOTE => print!("^"),
                                        _ => {}
                                    }
                                }
                                LR_OP_CODE_BASE_SEMANTIC_ACTION => {
                                    let index = reduction.bytecode[i];
                                    i += 1;
                                    print!("action {index}");
                                }
                                LR_OP_CODE_BASE_ADD_VIRTUAL => {
                                    let index = reduction.bytecode[i];
                                    i += 1;
                                    print!("virtual {index}");
                                    match get_op_code_tree_action(op_code) {
                                        TREE_ACTION_DROP => print!("!"),
                                        TREE_ACTION_PROMOTE => print!("^"),
                                        _ => {}
                                    }
                                }
                                _ => {}
                            }
                        }
                        println!();
                    }
                }
                LR_ACTION_CODE_ACCEPT => {
                    println!("    on 0x{c:X}, accept");
                }
                _ => {}
            }
        }
    }
    Ok(())
}

/// Prints a parser's automaton
fn print_parser_glr(_file_name: &str, _show_bytecode: bool) -> Result<(), Box<dyn Error>> {
    panic!("not implemented");
}

/// Computes the diff between two lexer automaton
#[allow(clippy::cast_possible_truncation)]
fn diff_lexer(file_left: &str, file_right: &str) -> Result<(), Box<dyn Error>> {
    let automaton_left = load_automaton_lexer(file_left)?;
    let automaton_right = load_automaton_lexer(file_right)?;
    let mut map_left_right = vec![0_usize; automaton_left.get_states_count()];
    let mut map_right_left = vec![0_usize; automaton_right.get_states_count()];

    for (s_left, state_left) in automaton_left.get_states().enumerate() {
        let s_right = map_left_right[s_left];
        if s_right == 0 && s_left != 0 {
            println!("State {s_left} on left is unmapped on right");
            continue;
        }
        let state_right = automaton_right.get_state(s_right as u32);
        let terminals_left = state_left.get_terminals().collect::<Vec<_>>();
        let terminals_right = state_right.get_terminals().collect::<Vec<_>>();
        for term_left in &terminals_left {
            match terminals_right.iter().find(|t| t.index == term_left.index) {
                Some(term_right) => {
                    if term_left.context != term_right.context {
                        println!(
                            "State ({}, {}) has different contexts for final terminal 0x{:X}",
                            s_left, s_right, term_left.index
                        );
                    }
                }
                None => {
                    println!(
                        "State ({}, {}) is final for terminal 0x{:X} on the left, not the right",
                        s_left, s_right, term_left.index
                    );
                }
            }
        }
        for term_right in &terminals_right {
            if terminals_left.iter().any(|r| r.index == term_right.index) {
                println!(
                    "State ({}, {}) is final for terminal 0x{:X} on the right, not the left",
                    s_left, s_right, term_right.index
                );
            }
        }
        for i in 0..256 {
            let next_left = state_left.get_cached_transition(i);
            let next_right = state_right.get_cached_transition(i);
            match (next_left, next_right) {
                (DEAD_STATE, DEAD_STATE) => {}
                (_, DEAD_STATE) => {
                    println!(
                        "State ({s_left}, {s_right}) has a transition on 0x{i:X} on the left, not on the right"
                    );
                }
                (DEAD_STATE, _) => {
                    println!(
                        "State ({s_left}, {s_right}) has a transition on 0x{i:X} on the right, not on the left"
                    );
                }
                (next_left, next_right) => {
                    map_left_right[next_left as usize] = next_right as usize;
                    map_right_left[next_right as usize] = next_left as usize;
                }
            }
        }
    }
    for (s_right, &s_left) in map_right_left.iter().enumerate() {
        if s_left == 0 && s_right != 0 {
            println!("State {s_right} on right is unmapped on left");
        }
    }
    Ok(())
}

/// Computes the diff between two parser automaton
fn diff_parser(file_left: &str, file_right: &str, is_glr: bool) -> Result<(), Box<dyn Error>> {
    if is_glr {
        diff_parser_glr(file_left, file_right)
    } else {
        diff_parser_lrk(file_left, file_right)
    }
}

/// Computes the diff between two parser automaton
#[allow(clippy::cast_possible_truncation, clippy::too_many_lines)]
fn diff_parser_lrk(file_left: &str, file_right: &str) -> Result<(), Box<dyn Error>> {
    let automaton_left = load_automaton_lrk(file_left)?;
    let states_left = automaton_left.get_states_count();
    let columns_left = automaton_left.get_columns_count();
    let automaton_right = load_automaton_lrk(file_right)?;
    let states_right = automaton_right.get_states_count();
    let columns_right = automaton_right.get_columns_count();
    let mut map_left_right = vec![0_usize; states_left];
    let mut map_right_left = vec![0_usize; states_right];

    for s_left in 0..states_left {
        let s_right = map_left_right[s_left];
        if s_right == 0 && s_left != 0 {
            println!("State {s_left} on left is unmapped on right");
            continue;
        }
        let contexts_left = automaton_left.get_contexts(s_left as u32);
        let contexts_right = automaton_right.get_contexts(s_right as u32);
        for c in 0..columns_left.min(columns_right) {
            let action_left = automaton_left.get_action_at(s_left as u32, c);
            let action_right = automaton_right.get_action_at(s_right as u32, c);
            match (action_left.get_code(), action_right.get_code()) {
                (LR_ACTION_CODE_SHIFT, LR_ACTION_CODE_SHIFT) => {
                    let target_left = action_left.get_data() as usize;
                    let target_right = action_right.get_data() as usize;
                    map_left_right[target_left] = target_right;
                    map_right_left[target_right] = target_left;
                    match (
                        contexts_left.get_context_opened_by(automaton_left.get_sid_for_column(c)),
                        contexts_right.get_context_opened_by(automaton_right.get_sid_for_column(c))
                    ) {
                        (None, None) => {}
                        (Some(from), Some(to)) => {
                            if from != to {
                                println!("States ({s_left},{s_right}), on 0x{c:X}, shift opens different contexts: {from} and {to}");
                            }
                        }
                        (Some(from), None) => {
                            println!(
                                "States ({s_left},{s_right}), on 0x{c:X}, left opens context {from}, right do not"
                            );
                        }
                        (None, Some(to)) => {
                            println!(
                                "States ({s_left},{s_right}), on 0x{c:X}, right opens context {to}, left do not"
                            );
                        }
                    }
                }
                (LR_ACTION_CODE_REDUCE, LR_ACTION_CODE_REDUCE) => {
                    let reduction_left =
                        automaton_left.get_production(action_left.get_data() as usize);
                    let reduction_right =
                        automaton_right.get_production(action_right.get_data() as usize);
                    if reduction_left.head != reduction_right.head {
                        println!(
                            "States ({},{}), on 0x{:X}, left reduces 0x{:X}, right reduces 0x{:X}",
                            s_left, s_right, c, reduction_left.head, reduction_right.head
                        );
                    }
                    if reduction_left.head_action != reduction_right.head_action {
                        println!(
                            "States ({s_left},{s_right}), on 0x{c:X}, left and right have different head actions on reduction"
                        );
                    }
                    if reduction_left.reduction_length != reduction_right.reduction_length {
                        println!(
                            "States ({s_left},{s_right}), on 0x{c:X}, left and right have different reduction length"
                        );
                    }
                    if reduction_left.bytecode.len() != reduction_right.bytecode.len() {
                        println!(
                            "States ({s_left},{s_right}), on 0x{c:X}, left and right have different reduction bytecode length"
                        );
                    }
                    let different = reduction_left
                        .bytecode
                        .iter()
                        .copied()
                        .zip(reduction_right.bytecode.iter().copied())
                        .any(|(l, r)| l != r);
                    if different {
                        println!(
                            "States ({s_left},{s_right}), on 0x{c:X}, left and right have different reduction bytecode"
                        );
                    }
                }
                (LR_ACTION_CODE_ACCEPT, LR_ACTION_CODE_ACCEPT)
                | (LR_ACTION_CODE_NONE, LR_ACTION_CODE_NONE) => {}
                (code_left, code_right) => {
                    println!(
                        "States ({}, {}) have different actions: {} and {}",
                        s_left,
                        s_right,
                        action_name(code_left),
                        action_name(code_right)
                    );
                }
            }
        }
        if columns_left > columns_right {
            for c in columns_right..columns_left {
                let action_left = automaton_left.get_action_at(s_left as u32, c);
                if action_left.get_code() != LR_ACTION_CODE_NONE {
                    println!(
                        "State {} on the left has a supplementary action {} on 0x{:X}",
                        s_left,
                        action_name(action_left.get_code()),
                        c
                    );
                }
            }
        }
        if columns_right > columns_left {
            for c in columns_left..columns_right {
                let action_right = automaton_right.get_action_at(s_right as u32, c);
                if action_right.get_code() != LR_ACTION_CODE_NONE {
                    println!(
                        "State {} on the right has a supplementary action {} on 0x{:X}",
                        s_right,
                        action_name(action_right.get_code()),
                        c
                    );
                }
            }
        }
    }
    for (s_right, &s_left) in map_right_left.iter().enumerate() {
        if s_left == 0 && s_right != 0 {
            println!("State {s_right} on right is unmapped on left");
        }
    }
    Ok(())
}

/// Gets the display name of an LR action code
fn action_name(code: u16) -> &'static str {
    match code {
        LR_ACTION_CODE_NONE => "none",
        LR_ACTION_CODE_SHIFT => "shift",
        LR_ACTION_CODE_REDUCE => "reduce",
        LR_ACTION_CODE_ACCEPT => "accept",
        _ => ""
    }
}

/// Computes the diff between two parser automaton
fn diff_parser_glr(_file_left: &str, _file_right: &str) -> Result<(), Box<dyn Error>> {
    panic!("not implemented");
}

/// Loads a lexer automaton from a file
fn load_automaton_lexer(file_name: &str) -> Result<Automaton, Box<dyn Error>> {
    let mut file = BufReader::new(File::open(file_name)?);
    let mut buffer = Vec::new();
    file.read_to_end(&mut buffer)?;
    Ok(Automaton::new(&buffer))
}

/// Loads an LR(k) automaton from a file
fn load_automaton_lrk(file_name: &str) -> Result<LRkAutomaton, Box<dyn Error>> {
    let mut file = BufReader::new(File::open(file_name)?);
    let mut buffer = Vec::new();
    file.read_to_end(&mut buffer)?;
    Ok(LRkAutomaton::new(&buffer))
}
