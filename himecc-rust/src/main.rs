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

//! Generator of lexers and parsers for the Hime runtime.

mod errors;

use std::io::{self, Read};
use std::{env, process};

use clap::{App, Arg};
use hime_sdk::errors::{Error, Errors};
use hime_sdk::{CompilationTask, Input, Mode, Modifier, ParsingMethod, Runtime};

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

pub fn main() {
    let matches = App::new("Hime Parser Generator")
        .version(format!("{} {} tag={} hash={}", CRATE_NAME, CRATE_VERSION, GIT_TAG, GIT_HASH).as_str())
        .author("Association Cénotélie <contact@cenotelie.fr>")
        .about("Generator of lexers and parsers for the Hime runtime.")
        .arg(
            Arg::with_name("output_mode")
                .value_name("MODE")
                .short("o")
                .long("output")
                .help("The output mode.")
                .takes_value(true)
                .required(false)
                .default_value("sources")
                .possible_values(&[
                    "sources",
                    "assembly",
                    "all"
                ])
        )
        .arg(
            Arg::with_name("output_target")
                .value_name("TARGET")
                .short("t")
                .long("target")
                .help("The target runtime.")
                .takes_value(true)
                .required(false)
                .default_value("net")
                .possible_values(&[
                    "net",
                    "java",
                    "rust"
                ])
        )
        .arg(
            Arg::with_name("output_target_runtime_path")
                .value_name("RUNTIME")
                .short("r")
                .long("runtime")
                .help("The path to a specific target runtime.")
                .takes_value(true)
                .required(false)
        )
        .arg(
            Arg::with_name("output_path")
                .value_name("PATH")
                .short("p")
                .long("path")
                .help("The path to write the output. By default, the current directory is used.")
                .takes_value(true)
                .required(false)
        )
        .arg(
            Arg::with_name("output_access")
                .value_name("ACCESS")
                .short("a")
                .long("access")
                .help("The access modifier for the generated code.")
                .takes_value(true)
                .required(false)
                .default_value("internal")
                .possible_values(&[
                    "internal",
                    "public"
                ])
        )
        .arg(
            Arg::with_name("output_namespace")
                .value_name("NMSPCE")
                .short("n")
                .long("namespace")
                .help("The namespace to use for the generated code. If none is given, and the target runtime requires one, the name of the grammar will be used.")
                .takes_value(true)
                .required(false)
        )
        .arg(
            Arg::with_name("parsing_method")
                .value_name("METHOD")
                .short("m")
                .long("method")
                .help("The parsing method to use.")
                .takes_value(true)
                .required(false)
                .default_value("lalr1")
                .possible_values(&[
                    "lr0",
                    "lr1",
                    "lalr1",
                    "rnglr1",
                    "rnglalr1"
                ])
        )
        .arg(
            Arg::with_name("grammar_name")
                .value_name("GRAMMAR")
                .short("g")
                .long("grammar")
                .help("The name of the grammar to compile if there are multiple.")
                .takes_value(true)
                .required(false)
        )
        .arg(
            Arg::with_name("test")
                .long("test")
                .help("Compiles the target grammar in-memory and test it against an input read from std::in and output the AST or parse errors")
                .required(false)
        )
        .arg(
            Arg::with_name("inputs")
                .value_name("INPUTS")
                .help("The file names of the input grammars")
                .takes_value(true)
                .required(true)
                .multiple(true)
        )
        .get_matches();

    let mut task = CompilationTask::default();
    match matches.value_of("output_mode") {
        Some("sources") => task.mode = Some(Mode::Sources),
        Some("assembly") => task.mode = Some(Mode::Assembly),
        Some("all") => task.mode = Some(Mode::SourcesAndAssembly),
        _ => {}
    }
    match matches.value_of("output_target") {
        Some("net") => task.output_target = Some(Runtime::Net),
        Some("java") => task.output_target = Some(Runtime::Java),
        Some("rust") => task.output_target = Some(Runtime::Rust),
        _ => {}
    }
    task.output_target_runtime_path = matches
        .value_of("output_target_runtime_path")
        .map(|v| v.to_string());
    task.output_path = matches.value_of("output_path").map(|v| v.to_string());
    match matches.value_of("output_access") {
        Some("internal") => task.output_modifier = Some(Modifier::Internal),
        Some("public") => task.output_modifier = Some(Modifier::Public),
        _ => {}
    }
    task.output_namespace = matches.value_of("output_namespace").map(|v| v.to_string());
    match matches.value_of("parsing_method") {
        Some("lr0") => task.method = Some(ParsingMethod::LR0),
        Some("lr1") => task.method = Some(ParsingMethod::LR1),
        Some("lalr1") => task.method = Some(ParsingMethod::LALR1),
        Some("rnglr1") => task.method = Some(ParsingMethod::RNGLR1),
        Some("rnglalr1") => task.method = Some(ParsingMethod::RNGLALR1),
        _ => {}
    }
    task.grammar_name = matches.value_of("grammar_name").map(|v| v.to_string());
    if let Some(inputs) = matches.values_of("inputs") {
        for input in inputs {
            task.inputs.push(Input::FileName(input.to_string()));
        }
    }
    let result = if matches.is_present("test") {
        execute_test(task)
    } else {
        execute_normal(task)
    };
    if let Err(errors) = result {
        errors::print_errors(&errors);
        process::exit(1);
    } else {
        process::exit(0);
    }
}

/// Executes the normal operation of the compiler
fn execute_normal(task: CompilationTask) -> Result<(), Errors> {
    task.execute()?;
    Ok(())
}

/// Executes the compiler in test mode
/// Compiles the target grammar in-memory
/// Test it against the input read from std::in
/// Output the result
fn execute_test(task: CompilationTask) -> Result<(), Errors> {
    let mut data = task.load()?;
    if data.grammars.is_empty() || (data.grammars.len() > 1 && task.grammar_name.is_none()) {
        return Err(Errors::from(data, vec![Error::GrammarNotSpecified]));
    }
    let (grammar_index, grammar) = if data.grammars.len() == 1 {
        (0, &mut data.grammars[0])
    } else {
        let name = task.grammar_name.as_ref().unwrap();
        match data
            .grammars
            .iter_mut()
            .enumerate()
            .find(|(_, grammar)| &grammar.name == name)
        {
            Some(couple) => couple,
            None => {
                return Err(Errors::from(
                    data,
                    vec![Error::GrammarNotFound(name.to_owned())]
                ));
            }
        }
    };
    let parser = match task.generate_in_memory(grammar, grammar_index) {
        Ok(p) => p,
        Err(errs) => {
            return Err(Errors::from(data, errs));
        }
    };

    let mut input_stream = io::stdin();
    let mut input = String::new();
    match input_stream.read_to_string(&mut input) {
        Ok(_) => {}
        Err(error) => {
            return Err(Errors::from(data, vec![Error::Io(error)]));
        }
    }
    let result = parser.parse(&input);
    match serde_json::to_writer(std::io::stdout(), &result) {
        Ok(r) => r,
        Err(error) => {
            return Err(Errors::from(data, vec![Error::Msg(error.to_string())]));
        }
    };
    Ok(())
}
