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

use std::{env, io};

use clap::{App, Arg};
use hime_redist::result::ParseResult;

/// The version of this program
pub const CRATE_VERSION: &str = env!("CARGO_PKG_VERSION");
/// The commit that was used to build the application
pub const GIT_HASH: &str = env!("GIT_HASH");
/// The git tag that was used to build the application
pub const GIT_TAG: &str = env!("GIT_TAG");

/// Main entry point
fn main() {
    let matches = App::new("Hime ParseIt")
        .version(format!("{} tag={} hash={}", CRATE_VERSION, GIT_TAG, GIT_HASH).as_str())
        .author("Association Cénotélie <contact@cenotelie.fr>")
        .about("Command line to parse a piece of input using a packaged parser.")
        .arg(
            Arg::with_name("library")
                .value_name("LIBRARY")
                .help("Path to the compiled library containing the parser")
                .takes_value(true)
                .required(true)
        )
        .arg(
            Arg::with_name("module")
                .value_name("MODULE")
                .help("The module inside the library that contains the parser")
                .takes_value(true)
                .required(true)
        )
        .get_matches();

    let library = matches.value_of("library").unwrap();
    let module = matches.value_of("module").unwrap();
    let mut input = io::stdin();
    do_parse(&mut input, library, module);
}

/// Parses the input
fn do_parse(input: &mut dyn io::Read, lib_name: &str, parser_module: &str) {
    let mut function_name = String::new();
    function_name.push_str(parser_module);
    function_name.push_str("_parse_utf8");
    unsafe {
        let library = libloading::Library::new(lib_name).unwrap();
        let parser: libloading::Symbol<fn(&mut dyn io::Read) -> ParseResult> =
            library.get(function_name.as_bytes()).unwrap();
        let result = parser(input);
        serde_json::to_writer(std::io::stdout(), &result).unwrap()
    }
}
