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

extern crate ansi_term;
extern crate clap;
extern crate hime_redist;
extern crate hime_sdk;

use clap::{App, Arg};
use hime_sdk::{CompilationTask, Mode, Modifier, ParsingMethod, Runtime};
use std::env;
use std::process;

/// The name of this program
pub const CRATE_NAME: &str = env!("CARGO_PKG_NAME");
/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_NAME");

pub fn main() {
    let matches = App::new("Hime Parser Generator")
        .version(CRATE_VERSION)
        .author("Association Cénotélie <contact@cenotelie.fr>")
        .about("Generator of lexers and parsers for the Hime runtime.")
        .arg(
            Arg::with_name("regenerate")
                .short("r")
                .long("regenerate")
                .help("Regenerate the parser for Hime grammars")
                .takes_value(false)
                .required(false)
        )
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
                    "debug"
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
                .default_value("lalr")
                .possible_values(&[
                    "lalr",
                    "rnglr"
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
            Arg::with_name("inputs")
                .value_name("INPUTS")
                .help("The file names of the input grammars")
                .takes_value(true)
                .required(true)
                .multiple(true)
        )
        .get_matches();

    if matches.value_of("regenerate").is_some() {
        // TODO: regenerate
    } else {
        let mut task = CompilationTask::default();
        match matches.value_of("output_mode") {
            Some("sources") => task.mode = Mode::Source,
            Some("assembly") => task.mode = Mode::Assembly,
            Some("debug") => task.mode = Mode::Debug,
            _ => {}
        }
        match matches.value_of("output_target") {
            Some("net") => task.output_target = Runtime::Net,
            Some("java") => task.output_target = Runtime::Java,
            Some("rust") => task.output_target = Runtime::Rust,
            _ => {}
        }
        task.output_path = matches.value_of("output_path").map(|v| v.to_string());
        match matches.value_of("output_access") {
            Some("internal") => task.output_modifier = Modifier::Internal,
            Some("public") => task.output_modifier = Modifier::Public,
            _ => {}
        }
        task.output_namespace = matches.value_of("output_namespace").map(|v| v.to_string());
        match matches.value_of("parsing_method") {
            Some("lalr") => task.method = ParsingMethod::LALR1,
            Some("rnglr") => task.method = ParsingMethod::RNGLALR1,
            _ => {}
        }
        task.grammar_name = matches.value_of("grammar_name").map(|v| v.to_string());
        if let Some(inputs) = matches.values_of("inputs") {
            for input in inputs {
                task.input_files.push(input.to_string());
            }
        }
        if task.execute().is_ok() {
            process::exit(0);
        } else {
            process::exit(1);
        }
    }
}
