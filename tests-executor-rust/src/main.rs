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

// Used for tests debugging
// extern crate hime_generated;
extern crate hime_redist;
extern crate libloading;

use std::env;
use std::fs;
use std::io;
use std::io::Read;
use std::path;
use std::process;

use hime_redist::ast::AstNode;
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_redist::utils::iterable::Iterable;

/// The parser must produce an AST that matches the expected one
const VERB_MATCHES: &str = "matches";
/// The parser must produce an AST that do NOT match the expected one
const VERB_NOMATCHES: &str = "nomatches";
/// The parser must fail
const VERB_FAILS: &str = "fails";
/// The parser have the specified output
const VERB_OUTPUTS: &str = "outputs";

/// The test was successful
const RESULT_SUCCESS: i32 = 0;
/// The test failed in the end
const RESULT_FAILURE_VERB: i32 = 1;
/// The test failed in its parsing phase
const RESULT_FAILURE_PARSING: i32 = 2;

/// Main entry point
fn main() {
    let args = read_args();
    let exe_path = env::current_exe().unwrap_or_else(|error| panic!("{}", error));
    let my_path = exe_path.parent().unwrap();
    let library_name = get_parser_library_name(my_path);
    let library =
        libloading::Library::new(library_name).unwrap_or_else(|error| panic!("{}", error));
    let result = execute(my_path, &library, &args[0], &args[1]);
    process::exit(result);
}

#[test]
fn test_single() {
    let test_name = "test_position_lineendings_windows";
    let verb = VERB_MATCHES;
    let my_path = path::Path::new("/home/laurent/dev/hime-main/tests-results");
    let library_name = get_parser_library_name(my_path);
    let library =
        libloading::Library::new(library_name).unwrap_or_else(|error| panic!("{}", error));
    let result = execute(my_path, &library, test_name, verb);
    assert_eq!(RESULT_SUCCESS, result);
}

/// Gets the name of the shared parser library
#[cfg(any(linux, unix))]
fn get_parser_library_name(my_path: &path::Path) -> path::PathBuf {
    my_path.join("parsers-rust.so")
}

/// Gets the name of the shared parser library
#[cfg(macos)]
fn get_parser_library_name(my_path: &path::Path) -> path::PathBuf {
    my_path.join("parsers-rust.dylib")
}

/// Gets the name of the shared parser library
#[cfg(windows)]
fn get_parser_library_name(my_path: &path::Path) -> path::PathBuf {
    my_path.join("parsers-rust.dll")
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

/// Gets the serialized expected AST
fn get_expected_ast(my_path: &path::Path, library: &libloading::Library) -> ParseResult {
    let file = my_path.join("expected.txt");
    let file_input = fs::File::open(file).unwrap_or_else(|error| panic!("{}", error));
    let mut input_reader = io::BufReader::new(file_input);
    unsafe {
        let parser: libloading::Symbol<fn(&mut dyn io::Read) -> ParseResult> = library
            .get(b"expected_tree_parse_utf8")
            .unwrap_or_else(|error| panic!("{}", error));
        parser(&mut input_reader)
    }
}

/// Gets the serialized expected output
fn get_expected_output(my_path: &path::Path) -> String {
    let file = my_path.join("expected.txt");
    let file_input = fs::File::open(file).unwrap_or_else(|error| panic!("{}", error));
    let mut input_reader = io::BufReader::new(file_input);
    let mut result = String::new();
    let _length = input_reader
        .read_to_string(&mut result)
        .unwrap_or_else(|error| panic!("{}", error));
    result
}

/// Gets the parsed input
fn get_parsed_input(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str
) -> ParseResult {
    let mut function_name = String::new();
    function_name.push_str(parser_name);
    function_name.push_str("_parse_utf8");
    let file = my_path.join("input.txt");
    let file_input = fs::File::open(file).unwrap_or_else(|error| panic!("{}", error));
    let mut input_reader = io::BufReader::new(file_input);
    unsafe {
        let parser: libloading::Symbol<fn(&mut dyn io::Read) -> ParseResult> = library
            .get(function_name.as_bytes())
            .unwrap_or_else(|error| panic!("{}", error));
        parser(&mut input_reader)
    }
}

/// Executes the specified test
fn execute(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str,
    verb: &str
) -> i32 {
    match verb {
        VERB_MATCHES => execute_test_matches(my_path, library, parser_name),
        VERB_NOMATCHES => execute_test_no_matches(my_path, library, parser_name),
        VERB_FAILS => execute_test_fails(my_path, library, parser_name),
        VERB_OUTPUTS => execute_test_outputs(my_path, library, parser_name),
        _ => RESULT_FAILURE_PARSING
    }
}

/// Executes the test as a parsing test with a matching condition
fn execute_test_matches(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str
) -> i32 {
    let expected = get_expected_ast(my_path, library);
    if expected.get_errors().get_count() > 0 {
        println!("Failed to parse the expected AST");
        return RESULT_FAILURE_PARSING;
    }
    let real_result = get_parsed_input(my_path, library, parser_name);
    for error in real_result.get_errors().iter() {
        println!("{}", error.get_message());
        let context = real_result.get_input().get_context_at(error.get_position());
        println!("{}", context.content);
        println!("{}", context.pointer);
    }
    if !real_result.is_success() {
        println!("Failed to parse the input");
        return RESULT_FAILURE_PARSING;
    }
    if real_result.get_errors().get_count() > 0 {
        println!("Some errors while parsing the input");
        return RESULT_FAILURE_PARSING;
    }
    if compare(
        expected.get_ast().get_root(),
        real_result.get_ast().get_root()
    ) {
        RESULT_SUCCESS
    } else {
        println!("Produced AST does not match the expected one");
        RESULT_FAILURE_VERB
    }
}

/// Executes the test as a parsing test with a non-matching condition
fn execute_test_no_matches(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str
) -> i32 {
    let expected = get_expected_ast(my_path, library);
    if expected.get_errors().get_count() > 0 {
        println!("Failed to parse the expected AST");
        return RESULT_FAILURE_PARSING;
    }
    let real_result = get_parsed_input(my_path, library, parser_name);
    for error in real_result.get_errors().iter() {
        println!("{}", error.get_message());
        let context = real_result.get_input().get_context_at(error.get_position());
        println!("{}", context.content);
        println!("{}", context.pointer);
    }
    if !real_result.is_success() {
        println!("Failed to parse the input");
        return RESULT_FAILURE_PARSING;
    }
    if real_result.get_errors().get_count() > 0 {
        println!("Some errors while parsing the input");
        return RESULT_FAILURE_PARSING;
    }
    if compare(
        expected.get_ast().get_root(),
        real_result.get_ast().get_root()
    ) {
        println!("Produced AST incorrectly matches the specified expectation");
        RESULT_FAILURE_VERB
    } else {
        RESULT_SUCCESS
    }
}

/// Executes the test as a parsing test with a failing condition
fn execute_test_fails(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str
) -> i32 {
    let real_result = get_parsed_input(my_path, library, parser_name);
    if !real_result.is_success() {
        return RESULT_SUCCESS;
    }
    if real_result.get_errors().get_count() > 0 {
        return RESULT_SUCCESS;
    }
    println!("No error found while parsing, while some were expected");
    RESULT_FAILURE_VERB
}

/// Executes the test as an output test
fn execute_test_outputs(
    my_path: &path::Path,
    library: &libloading::Library,
    parser_name: &str
) -> i32 {
    let output = get_expected_output(my_path);
    let expected_lines: Vec<&str> = find_lines_in(&output);
    let real_result = get_parsed_input(my_path, library, parser_name);
    if expected_lines.is_empty() || (expected_lines.len() == 1 && expected_lines[0].is_empty()) {
        // expect empty output
        if real_result.is_success() && real_result.get_errors().get_count() == 0 {
            return RESULT_SUCCESS;
        }
        for error in real_result.get_errors().iter() {
            println!("{}", error.get_message());
            let context = real_result.get_input().get_context_at(error.get_position());
            println!("{}", context.content);
            println!("{}", context.pointer);
        }
        println!("Expected an empty output but some error where found while parsing");
        return RESULT_FAILURE_VERB;
    }

    let mut i = 0;
    for error in real_result.get_errors().iter() {
        let message = format!("{}", error);
        let context = real_result.get_input().get_context_at(error.get_position());
        if i + 2 >= expected_lines.len() {
            println!("Unexpected error:");
            println!("{}", message);
            println!("{}", context.content);
            println!("{}", context.pointer);
            return RESULT_FAILURE_VERB;
        }
        if !message.starts_with(expected_lines[i]) {
            println!("Unexpected output: {}", message);
            println!("Expected prefix  : {}", expected_lines[i]);
            return RESULT_FAILURE_VERB;
        }
        if !context.content.starts_with(expected_lines[i + 1]) {
            println!("Unexpected output: {}", context.content);
            println!("Expected prefix  : {}", expected_lines[i + 1]);
            return RESULT_FAILURE_VERB;
        }
        if !context.pointer.starts_with(expected_lines[i + 2]) {
            println!("Unexpected output: {}", context.pointer);
            println!("Expected prefix  : {}", expected_lines[i + 2]);
            return RESULT_FAILURE_VERB;
        }
        i += 3;
    }
    if i == expected_lines.len() {
        return RESULT_SUCCESS;
    }
    for line in expected_lines.iter().skip(i) {
        println!("Missing output: {}", line);
    }
    RESULT_FAILURE_VERB
}

/// Compare the specified AST node to the expected node
fn compare(expected: AstNode, node: AstNode) -> bool {
    let name = expected
        .get_value()
        .unwrap_or_else(|| panic!("Malformed expected AST"));
    if !node.get_symbol().name.eq(&name) {
        return false;
    }
    let expected_children = expected.children();
    let predicate = expected_children.at(0);
    let predicate_children = predicate.children();
    if !predicate_children.is_empty() {
        let test = predicate_children
            .at(0)
            .get_value()
            .unwrap_or_else(|| panic!("Malformed expected AST"));
        let value_expected = unescape(
            predicate_children
                .at(1)
                .get_value()
                .unwrap_or_else(|| panic!("Malformed expected AST"))
        );
        let value_real = node
            .get_value()
            .unwrap_or_else(|| panic!("Malformed input AST"));
        if test.eq("=") && !value_expected.eq(&value_real)
            || test.eq("!=") && value_expected.eq(&value_real)
        {
            return false;
        }
    }

    let comparable = expected_children.at(1);
    let comparable_children = comparable.children();
    let node_children = node.children();
    if comparable_children.len() != node_children.len() {
        return false;
    }
    for i in 0..node_children.len() {
        if !compare(comparable_children.at(i), node_children.at(i)) {
            return false;
        }
    }
    true
}

/// Un-escapes a value
fn unescape(value: String) -> String {
    let mut result = String::new();
    let mut on_escape = false;
    for data in value.chars().enumerate() {
        if data.0 != 0 {
            // drop the first character
            match data.1 {
                '\\' => {
                    if on_escape {
                        result.push('\\');
                        on_escape = false;
                    } else {
                        on_escape = true
                    }
                }
                c => {
                    result.push(c);
                    on_escape = false;
                }
            }
        }
    }
    let length = result.len();
    result.truncate(length - 1);
    result
}

/// Determines whether [c1, c2] form a line ending sequence
/// Recognized sequences are:
/// [U+000D, U+000A] (this is Windows-style \r \n)
/// [U+????, U+000A] (this is unix style \n)
/// [U+000D, U+????] (this is MacOS style \r, without \n after)
/// Others:
/// [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
fn is_line_ending(c1: char, c2: char) -> bool {
    (c2 == '\u{000B}'
        || c2 == '\u{000C}'
        || c2 == '\u{0085}'
        || c2 == '\u{2028}'
        || c2 == '\u{2029}')
        || (c1 == '\u{000D}' || c2 == '\u{000A}')
}

/// Finds all the lines in this content
fn find_lines_in(input: &str) -> Vec<&str> {
    let mut result = Vec::<&str>::new();
    let mut c1;
    let mut c2: char = '\0';
    let mut start = 0;
    for (i, x) in input.chars().enumerate() {
        c1 = c2;
        c2 = x;
        if is_line_ending(c1, c2) {
            let end = if c1 == '\u{000D}' && c2 != '\u{000A}' {
                i - 1
            } else {
                i
            };
            result.push(&input[start..end]);
            start = end
                + (if c1 == '\u{000D}' && c2 == '\u{000A}' {
                    2
                } else {
                    1
                });
        }
    }
    if input.len() > start {
        result.push(&input[start..input.len()]);
    }
    result
}
