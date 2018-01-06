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

use std::env;
use std::io;

use hime_redist::ast::AstNode;
use hime_redist::errors::ParseError;
use hime_redist::errors::ParseErrorDataTrait;
use hime_redist::result::ParseResult;
use hime_redist::symbols::SemanticElementTrait;
use hime_redist::symbols::Symbol;
use hime_redist::text::TextPosition;
use hime_redist::text::TextSpan;
use hime_redist::utils::iterable::Iterable;

/// Main entry point
fn main() {
    let args = read_args();
    if args.len() != 2 {
        print_help();
        return;
    }
    let mut input = io::stdin();
    let text = do_parse(&mut input, &args[0], &args[1]);
    println!("{}", text);
}

/// Parses the input
fn do_parse(input: &mut io::Read, lib_name: &str, parser_module: &str) -> String {
    let mut function_name = String::new();
    function_name.push_str(parser_module);
    function_name.push_str("::parse_utf8");
    let library = libloading::Library::new(lib_name).unwrap_or_else(|error| panic!("{}", error));
    unsafe {
        let parser: libloading::Symbol<fn(&mut io::Read) -> ParseResult> = library
            .get(function_name.as_bytes())
            .unwrap_or_else(|error| panic!("{}", error));
        let result = parser(input);
        let mut text = String::new();
        serialize_result(&mut text, result);
        text
    }
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

/// Prints the help screen for this program
fn print_help() {
    println!("parseit {} (LGPL 3)", env!("CARGO_PKG_VERSION"));
    println!("Command line to parse a piece of input using a packaged parser");
    println!();
    println!("usage: parseit <parser_library> <parser_module>");
}

/// Serializes a parse error
fn serialize_result(builder: &mut String, result: ParseResult) {
    builder.push_str("{\"errors\": [");
    let errors = result.get_errors();
    for error in errors.iter().enumerate() {
        if error.0 != 0 {
            builder.push_str(", ");
        }
        serialize_error(builder, error.1);
    }
    builder.push_str("]");
    if errors.get_count() == 0 {
        builder.push_str(", \"root\": ");
        serialize_ast(builder, result.get_ast().get_root());
    }
    builder.push_str("}");
}

/// Serializes a parse error
fn serialize_error(builder: &mut String, error: &ParseError) {
    builder.push_str("{\"type\": \"");
    match error {
        &ParseError::UnexpectedEndOfInput(ref _x) => builder.push_str("UnexpectedEndOfInput"),
        &ParseError::UnexpectedChar(ref _x) => builder.push_str("UnexpectedChar"),
        &ParseError::UnexpectedToken(ref _x) => builder.push_str("UnexpectedToken"),
        &ParseError::IncorrectUTF16NoHighSurrogate(ref _x) => {
            builder.push_str("IncorrectUTF16NoHighSurrogate")
        }
        &ParseError::IncorrectUTF16NoLowSurrogate(ref _x) => {
            builder.push_str("IncorrectUTF16NoLowSurrogate")
        }
    }
    //builder.push_str(error);
    builder.push_str("\", \"position\": ");
    serialize_position(builder, error.get_position());
    builder.push_str(", \"length\": ");
    builder.push_str(&error.get_length().to_string());
    builder.push_str(", \"message\": \"");
    builder.push_str(&escape_str(&error.get_message()));
    builder.push_str("\"}");
}

/// Serializes an AST node
fn serialize_ast(builder: &mut String, node: AstNode) {
    builder.push_str("{\"symbol\": ");
    serialize_symbol(builder, node.get_symbol());
    match node.get_value() {
        None => (),
        Some(ref x) => {
            builder.push_str(", \"value\": \"");
            builder.push_str(&escape_str(x));
            builder.push_str("\"");
        }
    }
    match node.get_position() {
        None => (),
        Some(x) => {
            builder.push_str(", \"position\": ");
            serialize_position(builder, x);
        }
    }
    match node.get_span() {
        None => (),
        Some(x) => {
            builder.push_str(", \"position\": ");
            serialize_span(builder, x);
        }
    }
    builder.push_str(", \"children\": [");
    for child in node.children().iter().enumerate() {
        if child.0 != 0 {
            builder.push_str(", ");
        }
        serialize_ast(builder, child.1);
    }
    builder.push_str("]}");
}

/// Serializes a symbol
fn serialize_symbol(builder: &mut String, symbol: Symbol) {
    builder.push_str("{\"id\": ");
    builder.push_str(&symbol.id.to_string());
    builder.push_str(", \"name\": \"");
    builder.push_str(&escape_str(symbol.name));
    builder.push_str("\"}");
}

/// Serializes a text position
fn serialize_position(builder: &mut String, position: TextPosition) {
    builder.push_str("{\"line\": ");
    builder.push_str(&position.line.to_string());
    builder.push_str(", \"column\": ");
    builder.push_str(&position.column.to_string());
    builder.push_str("}");
}

/// Serializes a text span
fn serialize_span(builder: &mut String, span: TextSpan) {
    builder.push_str("{\"index\": ");
    builder.push_str(&span.index.to_string());
    builder.push_str(", \"length\": ");
    builder.push_str(&span.length.to_string());
    builder.push_str("}");
}

/// Escapes the input string for serialization in JSON
fn escape_str(value: &str) -> String {
    let mut result = String::with_capacity(value.len());
    for c in value.chars() {
        match c {
            '"' => result.push_str("\\\""),
            '\\' => result.push_str("\\\\"),
            '\0' => result.push_str("\\0"),
            '\u{0007}' => result.push_str("\\a"),
            '\t' => result.push_str("\\t"),
            '\r' => result.push_str("\\r"),
            '\n' => result.push_str("\\n"),
            '\u{0008}' => result.push_str("\\b"),
            '\u{000C}' => result.push_str("\\c"),
            _ => result.push(c),
        }
    }
    result
}
