/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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

use std::env;
use std::process;

/// The parser must produce an AST that matches the expected one
const VERB_MATCHES: &'static str = "matches";
/// The parser must produce an AST that do NOT match the expected one
const VERB_NOMATCHES: &'static str = "nomatches";
/// The parser must fail
const VERB_FAILS: &'static str = "fails";
/// The parser have the specified output
const VERB_OUTPUTS: &'static str = "outputs";

/// The test was successful
const RESULT_SUCCESS: i32 = 0;
/// The test failed in the end
const RESULT_FAILURE_VERB: i32 = 1;
/// The test failed in its parsing phase
const RESULT_FAILURE_PARSING: i32 = 2;

/// Main entry point
fn main() {
    {
        let args = read_args();
    }
    process::exit(RESULT_FAILURE_PARSING);
}

/// Reads the arguments
fn read_args() -> Vec<String> {
    let mut args = Vec::<String>::new();
    for argument in env::args().enumerate() {
        if argument.0 != 0 {
            args.push(argument.1);
        }
    }
    args
}
