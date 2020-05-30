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

//! Module for generating parser code in rust

use crate::errors::Error;
use crate::grammars::{Grammar, PREFIX_GENERATED_TERMINAL, PREFIX_GENERATED_VARIABLE};
use crate::output::get_parser_bin_name_rust;
use crate::output::helper::{to_snake_case, to_upper_case};
use crate::ParsingMethod;
use std::fs::OpenOptions;
use std::io::{self, Write};
use std::path::PathBuf;

/// Generates code for the specified file
pub fn write(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    method: ParsingMethod,
    nmespace: &str,
    output_assembly: bool
) -> Result<(), Error> {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = OpenOptions::new()
        .write(true)
        .append(true)
        .open(final_path)?;
    let mut writer = io::BufWriter::new(file);

    let (parser_type, automaton_type) = if method.is_rnglr() {
        ("RNGLRParser", "RNGLRAutomaton")
    } else {
        ("LRkParser", "LRkAutomaton")
    };
    let bin_name = get_parser_bin_name_rust(grammar);

    writeln!(
        writer,
        "/// Static resource for the serialized parser automaton"
    )?;
    writeln!(
        writer,
        "const PARSER_AUTOMATON: &[u8] = include_bytes!(\"{}\");",
        bin_name
    )?;
    writeln!(writer)?;

    write_code_symbols(&mut writer, grammar)?;
    write_code_variables(&mut writer, grammar)?;
    write_code_virtuals(&mut writer, grammar)?;
    write_code_actions(&mut writer, grammar)?;
    write_code_constructors(
        &mut writer,
        grammar,
        output_assembly,
        nmespace,
        automaton_type,
        parser_type
    )?;
    write_code_visitor(&mut writer, grammar)?;
    Ok(())
}

/// Generates the code for the symbols
fn write_code_symbols(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    for variable in grammar
        .variables
        .iter()
        .filter(|v| !v.name.starts_with(PREFIX_GENERATED_VARIABLE))
    {
        writeln!(
            writer,
            "/// The unique identifier for variable {}",
            &variable.name
        )?;
        writeln!(
            writer,
            "pub const ID_VARIABLE_{}: u32 = 0x{:04X};",
            to_upper_case(&variable.name),
            variable.id
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(
            writer,
            "/// The unique identifier for virtual {}",
            &symbol.name
        )?;
        writeln!(
            writer,
            "pub const ID_VIRTUAL_{}: u32 = 0x{:04X};",
            to_upper_case(&symbol.name),
            symbol.id
        )?;
    }
    writeln!(writer)?;
    Ok(())
}

/// Generates the code for the variables
fn write_code_variables(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(
        writer,
        "/// The collection of variables matched by this parser"
    )?;
    writeln!(
        writer,
        "/// The variables are in an order consistent with the automaton,"
    )?;
    writeln!(writer, "/// so that variable indices in the automaton can be used to retrieve the variables in this table")?;
    writeln!(writer, "const VARIABLES: &[Symbol] = &[")?;
    for (index, variable) in grammar.variables.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ", ")?;
        }
        write!(
            writer,
            "    Symbol {{ id: 0x{:04X}, name: \"{}\" }}",
            variable.id, &variable.name
        )?;
    }
    writeln!(writer, "];")?;
    writeln!(writer)?;
    Ok(())
}

/// Generates the code for the virtual symbols
fn write_code_virtuals(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(
        writer,
        "/// The collection of virtuals matched by this parser"
    )?;
    writeln!(
        writer,
        "/// The virtuals are in an order consistent with the automaton,"
    )?;
    writeln!(writer, "/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table")?;
    writeln!(writer, "const VIRTUALS: &[Symbol] = &[")?;
    for (index, symbol) in grammar.virtuals.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ", ")?;
        }
        write!(
            writer,
            "    Symbol {{ id: 0x{:04X}, name: \"{}\" }}",
            symbol.id, &symbol.name
        )?;
    }
    writeln!(writer, "];")?;
    writeln!(writer)?;
    Ok(())
}

/// Generates the code for the semantic actions
fn write_code_actions(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    if grammar.actions.is_empty() {
        return Ok(());
    }
    writeln!(
        writer,
        "/// Represents a set of semantic actions in this parser"
    )?;
    writeln!(writer, "pub trait Actions {{")?;
    for action in grammar.actions.iter() {
        writeln!(writer, "    /// The {} semantic action", &action.name)?;
        writeln!(
            writer,
            "    fn {}(&mut self, head: Symbol, body: &dyn SemanticBody);",
            to_snake_case(&action.name)
        )?;
    }
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(writer, "/// The structure that implements no action")?;
    writeln!(writer, "pub struct NoActions {{}}")?;
    writeln!(writer)?;
    writeln!(writer, "impl Actions for NoActions {{")?;
    for action in grammar.actions.iter() {
        writeln!(
            writer,
            "    fn {}(&mut self, _head: Symbol, _body: &dyn SemanticBody) {{}}",
            to_snake_case(&action.name)
        )?;
    }
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    Ok(())
}

/// Generates the code for the constructors
fn write_code_constructors(
    writer: &mut dyn Write,
    grammar: &Grammar,
    output_assembly: bool,
    nmespace: &str,
    automaton_type: &str,
    parser_type: &str
) -> Result<(), Error> {
    if grammar.actions.is_empty() {
        writeln!(writer, "/// Parses the specified string with this parser")?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_string\"]", nmespace)?;
        }
        writeln!(writer, "pub fn parse_string(input: &str) -> ParseResult {{")?;
        writeln!(writer, "    let text = Text::new(input);")?;
        writeln!(writer, "    parse_text(text)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf16\"]", nmespace)?;
        }
        writeln!(
            writer,
            "pub fn parse_utf16(input: &mut dyn Read, big_endian: bool) -> ParseResult {{"
        )?;
        writeln!(
            writer,
            "    let text = Text::from_utf16_stream(input, big_endian);"
        )?;
        writeln!(writer, "    parse_text(text)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf8\"]", nmespace)?;
        }
        writeln!(
            writer,
            "pub fn parse_utf8(input: &mut dyn Read) -> ParseResult {{"
        )?;
        writeln!(writer, "    let text = Text::from_utf8_stream(input);")?;
        writeln!(writer, "    parse_text(text)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(writer, "/// Parses the specified text with this parser")?;
        writeln!(writer, "fn parse_text(text: Text) -> ParseResult {{")?;
        writeln!(
            writer,
            "    let mut my_actions = |_index: usize, _head: Symbol, _body: &dyn SemanticBody| ();"
        )?;
        writeln!(
            writer,
            "    let mut result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);"
        )?;
        writeln!(writer, "    {{")?;
        writeln!(writer, "        let data = result.get_parsing_data();")?;
        writeln!(writer, "        let mut lexer = new_lexer(data.0, data.1);")?;
        writeln!(
            writer,
            "        let automaton = {}::new(PARSER_AUTOMATON);",
            automaton_type
        )?;
        writeln!(
            writer,
            "        let mut parser = {}::new(&mut lexer, automaton, data.2, &mut my_actions);",
            parser_type
        )?;
        writeln!(writer, "        parser.parse();")?;
        writeln!(writer, "    }}")?;
        writeln!(writer, "    result")?;
        writeln!(writer, "}}")?;
    } else {
        writeln!(writer, "/// Parses the specified string with this parser")?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_string\"]", nmespace)?;
        }
        writeln!(writer, "pub fn parse_string(input: &str) -> ParseResult {{")?;
        writeln!(writer, "    let mut actions = NoActions {{}};")?;
        writeln!(writer, "    parse_string_with(input, &mut actions)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(writer, "/// Parses the specified string with this parser")?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(
                writer,
                "#[export_name = \"{}_parse_string_with\"]",
                nmespace
            )?;
        }
        writeln!(
            writer,
            "pub fn parse_string_with(input: &str, actions: &mut dyn Actions) -> ParseResult {{"
        )?;
        writeln!(writer, "    let text = Text::new(input);")?;
        writeln!(writer, "    parse_text(text, actions)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf16\"]", nmespace)?;
        }
        writeln!(
            writer,
            "pub fn parse_utf16(input: &mut dyn Read, big_endian: bool) -> ParseResult {{"
        )?;
        writeln!(writer, "    let mut actions = NoActions {{}};")?;
        writeln!(
            writer,
            "    parse_utf16_with(input, big_endian, &mut actions)"
        )?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf16_with\"]", nmespace)?;
        }
        writeln!(writer, "pub fn parse_utf16_with(input: &mut dyn Read, big_endian: bool, actions: &mut dyn Actions) -> ParseResult {{")?;
        writeln!(
            writer,
            "    let text = Text::from_utf16_stream(input, big_endian);"
        )?;
        writeln!(writer, "    parse_text(text, actions)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf8\"]", nmespace)?;
        }
        writeln!(
            writer,
            "pub fn parse_utf8(input: &mut dyn Read) -> ParseResult {{"
        )?;
        writeln!(writer, "    let mut actions = NoActions {{}};")?;
        writeln!(writer, "    parse_utf8_with(input, &mut actions)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-16 with this parser"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(writer, "#[export_name = \"{}_parse_utf8_with\"]", nmespace)?;
        }
        writeln!(writer, "pub fn parse_utf8_with(input: &mut dyn Read, actions: &mut dyn Actions) -> ParseResult {{")?;
        writeln!(writer, "    let text = Text::from_utf8_stream(input);")?;
        writeln!(writer, "    parse_text(text, actions)")?;
        writeln!(writer, "}}")?;
        writeln!(writer)?;
        writeln!(writer, "/// Parses the specified text with this parser")?;
        writeln!(
            writer,
            "fn parse_text(text: Text, actions: &mut dyn Actions) -> ParseResult {{"
        )?;
        writeln!(writer, "    let mut my_actions = |index: usize, head: Symbol, body: &dyn SemanticBody| match index {{")?;
        for (index, action) in grammar.actions.iter().enumerate() {
            writeln!(
                writer,
                "        {} => actions.{}(head, body),",
                index,
                to_snake_case(&action.name)
            )?;
        }
        writeln!(writer, "        _ => ()")?;
        writeln!(writer, "    }};")?;
        writeln!(writer)?;
        writeln!(
            writer,
            "    let mut result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);"
        )?;
        writeln!(writer, "    {{")?;
        writeln!(writer, "        let data = result.get_parsing_data();")?;
        writeln!(writer, "        let mut lexer = new_lexer(data.0, data.1);")?;
        writeln!(
            writer,
            "        let automaton = {}::new(PARSER_AUTOMATON);",
            automaton_type
        )?;
        writeln!(
            writer,
            "        let mut parser = {}::new(&mut lexer, automaton, data.2, &mut my_actions);",
            parser_type
        )?;
        writeln!(writer, "        parser.parse();")?;
        writeln!(writer, "    }}")?;
        writeln!(writer, "    result")?;
        writeln!(writer, "}}")?;
    }
    Ok(())
}

/// Generates the visitor for the parse result
fn write_code_visitor(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(writer)?;
    writeln!(writer, "/// Visitor interface")?;
    writeln!(writer, "pub trait Visitor {{")?;
    for terminal in grammar.terminals.iter() {
        if terminal.id <= 2 || terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "    fn on_terminal_{}(&self, _node: &AstNode) {{}}",
            to_snake_case(&terminal.name)
        )?;
    }
    for variable in grammar.variables.iter() {
        if variable.name.starts_with(PREFIX_GENERATED_VARIABLE) {
            continue;
        }
        writeln!(
            writer,
            "    fn on_variable_{}(&self, _node: &AstNode) {{}}",
            to_snake_case(&variable.name)
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(
            writer,
            "    fn on_virtual_{}(&self, _node: &AstNode) {{}}",
            to_snake_case(&symbol.name)
        )?;
    }
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(writer, "/// Walk the AST of a result using a visitor")?;
    writeln!(
        writer,
        "pub fn visit(result: &ParseResult, visitor: &dyn Visitor) {{"
    )?;
    writeln!(writer, "    let ast = result.get_ast();")?;
    writeln!(writer, "    let root = ast.get_root();")?;
    writeln!(writer, "    visit_ast_node(root, visitor);")?;
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(
        writer,
        "/// Walk the sub-AST from the specified node using a visitor"
    )?;
    writeln!(
        writer,
        "pub fn visit_ast_node<'a>(node: AstNode<'a>, visitor: &dyn Visitor) {{"
    )?;
    writeln!(writer, "    let children = node.children();")?;
    writeln!(writer, "    for child in children.iter() {{")?;
    writeln!(writer, "        visit_ast_node(child, visitor);")?;
    writeln!(writer, "    }}")?;
    writeln!(writer, "    match node.get_symbol().id {{")?;
    for terminal in grammar.terminals.iter() {
        if terminal.id <= 2 || terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "        0x{:04X} => visitor.on_terminal_{}(&node),",
            terminal.id,
            to_snake_case(&terminal.name)
        )?;
    }
    for variable in grammar.variables.iter() {
        if variable.name.starts_with(PREFIX_GENERATED_VARIABLE) {
            continue;
        }
        writeln!(
            writer,
            "        0x{:04X} => visitor.on_variable_{}(&node),",
            variable.id,
            to_snake_case(&variable.name)
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(
            writer,
            "        0x{:04X} => visitor.on_virtual_{}(&node),",
            symbol.id,
            to_snake_case(&symbol.name)
        )?;
    }
    writeln!(writer, "        _ => ()")?;
    writeln!(writer, "    }};")?;
    writeln!(writer, "}}")?;
    Ok(())
}
