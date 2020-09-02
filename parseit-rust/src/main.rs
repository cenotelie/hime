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

extern crate hime_redist;
extern crate libloading;
extern crate serde_json;

use std::env;
use std::io;

use hime_redist::result::ParseResult;

/// Main entry point
fn main() {
    let args = read_args();
    if args.len() != 2 {
        print_help();
        return;
    }
    let mut input = io::stdin();
    do_parse(&mut input, &args[0], &args[1]);
}

/// Parses the input
fn do_parse(input: &mut dyn io::Read, lib_name: &str, parser_module: &str) {
    let mut function_name = String::new();
    function_name.push_str(parser_module);
    function_name.push_str("_parse_utf8");
    let library = libloading::Library::new(lib_name).unwrap_or_else(|error| panic!("{}", error));
    unsafe {
        let parser: libloading::Symbol<fn(&mut dyn io::Read) -> ParseResult> = library
            .get(function_name.as_bytes())
            .unwrap_or_else(|error| panic!("{}", error));
        let result = parser(input);
        serde_json::to_writer(std::io::stdout(), &result).unwrap()
    }
}

/// Reads the arguments
fn read_args() -> Vec<String> {
    env::args().skip(1).collect()
}

/// Prints the help screen for this program
fn print_help() {
    println!("parseit {} (LGPL 3)", env!("CARGO_PKG_VERSION"));
    println!("Command line to parse a piece of input using a packaged parser");
    println!();
    println!("usage: parseit <parser_library> <parser_module>");
}
