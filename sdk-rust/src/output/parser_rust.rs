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

use std::fs::OpenOptions;
use std::io::{self, Write};
use std::path::PathBuf;

use crate::errors::Error;
use crate::grammars::{Grammar, TerminalSet, PREFIX_GENERATED_TERMINAL, PREFIX_GENERATED_VARIABLE};
use crate::output::get_parser_bin_name_rust;
use crate::output::helper::{to_snake_case, to_upper_case};
use crate::ParsingMethod;

/// Generates code for the specified file
#[allow(clippy::too_many_arguments)]
pub fn write(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    expected: &TerminalSet,
    method: ParsingMethod,
    nmespace: &str,
    output_assembly: bool,
    with_std: bool,
    compress_automata: bool,
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

    let (parser_type, automaton_type, parser_ctor) = if method.is_rnglr() {
        ("RNGLRParser", "RNGLRAutomaton", "new_with_ast")
    } else {
        ("LRkParser", "LRkAutomaton", "new")
    };
    let bin_name = get_parser_bin_name_rust(grammar);

    if compress_automata {
        writeln!(
            writer,
            r#"include_flate::flate!(static PARSER_AUTOMATON: [u8] from "{bin_name}");"#
        )?;
    } else {
        writeln!(
            writer,
            "/// Static resource for the serialized parser automaton"
        )?;
        writeln!(
            writer,
            "static PARSER_AUTOMATON: &[u8] = include_bytes!(\"{bin_name}\");"
        )?;
    }
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
        parser_type,
        parser_ctor,
        "AstImpl",
        "ParseResultAst",
        "",
        with_std,
        compress_automata,
    )?;
    if method.is_rnglr() {
        writeln!(writer)?;
        write_code_constructors(
            &mut writer,
            grammar,
            output_assembly,
            nmespace,
            automaton_type,
            parser_type,
            "new_with_sppf",
            "SppfImpl",
            "ParseResultSppf",
            "_to_sppf",
            with_std,
            compress_automata,
        )?;
    }
    write_code_visitor(&mut writer, grammar, expected)?;
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
            "/// The unique identifier for variable `{}`",
            &variable.name
        )?;
        writeln!(
            writer,
            "pub const ID_VARIABLE_{}: u32 = 0x{:04X};",
            to_upper_case(&variable.name),
            variable.id
        )?;
    }
    writeln!(writer)?;
    for symbol in &grammar.virtuals {
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
    writeln!(writer, "pub const VARIABLES: &[Symbol] = &[")?;
    for (index, variable) in grammar.variables.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ",")?;
        }
        writeln!(writer, "    Symbol {{")?;
        writeln!(writer, "        id: 0x{:04X},", variable.id)?;
        writeln!(writer, "        name: \"{}\"", &variable.name)?;
        write!(writer, "    }}")?;
    }
    writeln!(writer)?;
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
    writeln!(writer, "pub const VIRTUALS: &[Symbol] = &[")?;
    for (index, symbol) in grammar.virtuals.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ",")?;
        }
        writeln!(writer, "    Symbol {{")?;
        writeln!(writer, "        id: 0x{:04X},", symbol.id)?;
        writeln!(writer, "        name: \"{}\"", &symbol.name)?;
        write!(writer, "    }}")?;
    }
    writeln!(writer)?;
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
    writeln!(writer, "#[allow(unused_variables)]")?;
    writeln!(writer, "pub trait Actions {{")?;
    for action in &grammar.actions {
        writeln!(writer, "    /// The {} semantic action", &action.name)?;
        writeln!(
            writer,
            "    fn {}(&mut self, head: Symbol, body: &dyn SemanticBody) {{}}",
            to_snake_case(&action.name)
        )?;
    }
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(writer, "/// The structure that implements no action")?;
    writeln!(writer, "pub struct NoActions {{}}")?;
    writeln!(writer)?;
    writeln!(writer, "impl Actions for NoActions {{}}")?;
    writeln!(writer)?;
    Ok(())
}

/// Generates the code for the constructors
#[allow(clippy::too_many_lines, clippy::too_many_arguments)]
fn write_code_constructors(
    writer: &mut dyn Write,
    grammar: &Grammar,
    output_assembly: bool,
    nmespace: &str,
    automaton_type: &str,
    parser_type: &str,
    parser_ctor: &str,
    tree_type: &str,
    parse_result_type: &str,
    fn_suffix: &str,
    with_std: bool,
    compress_automata: bool,
) -> Result<(), Error> {
    let has_actions = !grammar.actions.is_empty();
    writeln!(writer, "/// Parses the specified string with this parser")?;
    if output_assembly {
        writeln!(writer, "#[no_mangle]")?;
        writeln!(
            writer,
            "#[export_name = \"{nmespace}_parse_str{fn_suffix}\"]"
        )?;
    }
    writeln!(writer, "#[must_use]")?;
    writeln!(
        writer,
        "pub fn parse_str{fn_suffix}(input: &str) -> ParseResult<'static, '_, 'static, {tree_type}> {{"
    )?;
    writeln!(writer, "    let text = Text::from_str(input);")?;
    writeln!(
        writer,
        "    parse_text{fn_suffix}(text{})",
        if has_actions {
            ", &mut NoActions {{}}"
        } else {
            ""
        }
    )?;
    writeln!(writer, "}}")?;
    if has_actions {
        writeln!(writer)?;
        writeln!(writer, "/// Parses the specified string with this parser")?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(
                writer,
                "#[export_name = \"{nmespace}_parse_str{fn_suffix}_with\"]"
            )?;
        }
        writeln!(
            writer,
            "pub fn parse_str{fn_suffix}_with<'t>(input: &'t str, actions: &mut dyn Actions) -> ParseResult<'static, 't, 'static, {tree_type}> {{"
        )?;
        writeln!(writer, "    let text = Text::from_str(input);")?;
        writeln!(writer, "    parse_text{fn_suffix}(text, actions)")?;
        writeln!(writer, "}}")?;
    }

    writeln!(writer)?;
    writeln!(writer, "/// Parses the specified string with this parser")?;
    if output_assembly {
        writeln!(writer, "#[no_mangle]")?;
        writeln!(
            writer,
            "#[export_name = \"{nmespace}_parse_string{fn_suffix}\"]"
        )?;
    }
    writeln!(writer, "#[must_use]")?;
    writeln!(
        writer,
        "pub fn parse_string{fn_suffix}(input: String) -> {parse_result_type} {{"
    )?;
    writeln!(writer, "    let text = Text::from_string(input);")?;
    writeln!(
        writer,
        "    parse_text{fn_suffix}(text{})",
        if has_actions {
            ", &mut NoActions {{}}"
        } else {
            ""
        }
    )?;
    writeln!(writer, "}}")?;
    if has_actions {
        writeln!(writer)?;
        writeln!(writer, "/// Parses the specified string with this parser")?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(
                writer,
                "#[export_name = \"{nmespace}_parse_string{fn_suffix}_with\"]"
            )?;
        }
        writeln!(
            writer,
            "pub fn parse_string_with{fn_suffix}(input: String, actions: &mut dyn Actions) -> {parse_result_type} {{"
        )?;
        writeln!(writer, "    let text = Text::from_string(input);")?;
        writeln!(writer, "    parse_text{fn_suffix}(text, actions)")?;
        writeln!(writer, "}}")?;
    }

    if with_std {
        writeln!(writer)?;
        writeln!(
            writer,
            "/// Parses the specified stream of UTF-8 with this parser"
        )?;
        writeln!(writer, "///")?;
        writeln!(writer, "/// # Errors")?;
        writeln!(writer, "///")?;
        writeln!(
            writer,
            "/// Return an `std::io::Error` when reading the stream as UTF-8 fails"
        )?;
        if output_assembly {
            writeln!(writer, "#[no_mangle]")?;
            writeln!(
                writer,
                "#[export_name = \"{nmespace}_parse_utf8_stream{fn_suffix}\"]"
            )?;
        }
        writeln!(
            writer,
            "pub fn parse_utf8_stream{fn_suffix}(input: &mut dyn std::io::Read) -> Result<{parse_result_type}, std::io::Error> {{"
        )?;
        writeln!(writer, "    let text = Text::from_utf8_stream(input)?;")?;
        writeln!(
            writer,
            "    Ok(parse_text{fn_suffix}(text{}))",
            if has_actions {
                ", &mut NoActions {{}}"
            } else {
                ""
            }
        )?;
        writeln!(writer, "}}")?;
        if has_actions {
            writeln!(writer)?;
            if output_assembly {
                writeln!(writer, "#[no_mangle]")?;
                writeln!(
                    writer,
                    "#[export_name = \"{nmespace}_parse_utf8_stream{fn_suffix}_with\"]"
                )?;
            }
            writeln!(
                writer,
                "pub fn parse_utf8_stream{fn_suffix}_with(input: &mut dyn std::io::Read, actions: &mut dyn Actions) -> {parse_result_type} {{"
            )?;
            writeln!(
                writer,
                "    let text = Text::from_utf8_stream(input).unwrap();"
            )?;
            writeln!(writer, "    parse_text{fn_suffix}(text, actions)")?;
            writeln!(writer, "}}")?;
        }
    }

    writeln!(writer)?;
    writeln!(writer, "/// Parses the specified text with this parser")?;
    writeln!(
        writer,
        "fn parse_text{fn_suffix}<'t>(text: Text<'t>{}) -> ParseResult<'static, 't, 'static, {tree_type}> {{",
        if has_actions {
            ", actions: &mut dyn Actions"
        } else {
            ""
        }
    )?;
    writeln!(
        writer,
        "    parse_text{fn_suffix}_with(text, TERMINALS, VARIABLES, VIRTUALS{})",
        if has_actions { ", actions" } else { "" }
    )?;
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(writer, "/// Parses the specified text with this parser")?;
    writeln!(writer, "fn parse_text{fn_suffix}_with<'s, 't, 'a>(")?;
    writeln!(writer, "    text: Text<'t>,")?;
    writeln!(writer, "    terminals: &'a [Symbol<'s>],")?;
    writeln!(writer, "    variables: &'a [Symbol<'s>],")?;
    writeln!(writer, "    virtuals: &'a [Symbol<'s>],")?;
    if has_actions {
        writeln!(writer, "    actions: &mut dyn Actions")?;
    }
    writeln!(writer, ") -> ParseResult<'s, 't, 'a, {tree_type}> {{")?;
    if has_actions {
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
    } else {
        writeln!(writer, "    let mut my_actions = |_index: usize, _head: Symbol, _body: &dyn SemanticBody| {{}};")?;
    }
    writeln!(
        writer,
        "    let mut result = ParseResult::<{tree_type}>::new(terminals, variables, virtuals, text);"
    )?;
    writeln!(writer, "    {{")?;
    writeln!(writer, "        let data = result.get_parsing_data();")?;
    writeln!(writer, "        let mut lexer = new_lexer(data.0, data.1);")?;
    writeln!(
        writer,
        "        let automaton = {automaton_type}::new(PARSER_AUTOMATON{});",
        if compress_automata { ".as_ref()" } else { "" }
    )?;
    writeln!(
        writer,
        "        let mut parser = {parser_type}::{parser_ctor}(&mut lexer, variables, virtuals, automaton, data.2, &mut my_actions);"
    )?;
    writeln!(writer, "        parser.parse();")?;
    writeln!(writer, "    }}")?;
    writeln!(writer, "    result")?;
    writeln!(writer, "}}")?;
    Ok(())
}

/// Generates the visitor for the parse result
fn write_code_visitor(
    writer: &mut dyn Write,
    grammar: &Grammar,
    expected: &TerminalSet,
) -> Result<(), Error> {
    writeln!(writer)?;
    writeln!(writer, "/// Visitor interface")?;
    writeln!(writer, "#[allow(unused_variables)]")?;
    writeln!(writer, "pub trait Visitor {{")?;
    for terminal_ref in &expected.content {
        let Some(terminal) = grammar.get_terminal(terminal_ref.sid()) else {
            continue;
        };
        if terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "    fn on_terminal_{}(&self, node: &AstNode) {{}}",
            to_snake_case(&terminal.name)
        )?;
    }
    for variable in &grammar.variables {
        if variable.name.starts_with(PREFIX_GENERATED_VARIABLE) {
            continue;
        }
        writeln!(
            writer,
            "    fn on_variable_{}(&self, node: &AstNode) {{}}",
            to_snake_case(&variable.name)
        )?;
    }
    for symbol in &grammar.virtuals {
        writeln!(
            writer,
            "    fn on_virtual_{}(&self, node: &AstNode) {{}}",
            to_snake_case(&symbol.name)
        )?;
    }
    writeln!(writer, "}}")?;
    writeln!(writer)?;
    writeln!(writer, "/// Walk the AST of a result using a visitor")?;
    writeln!(
        writer,
        "pub fn visit(result: &ParseResult<AstImpl>, visitor: &dyn Visitor) {{"
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
        "pub fn visit_ast_node(node: AstNode, visitor: &dyn Visitor) {{"
    )?;
    writeln!(writer, "    let children = node.children();")?;
    writeln!(writer, "    for child in children.iter() {{")?;
    writeln!(writer, "        visit_ast_node(child, visitor);")?;
    writeln!(writer, "    }}")?;
    writeln!(writer, "    match node.get_symbol().id {{")?;
    for terminal_ref in &expected.content {
        let Some(terminal) = grammar.get_terminal(terminal_ref.sid()) else {
            continue;
        };
        if terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "        0x{:04X} => visitor.on_terminal_{}(&node),",
            terminal.id,
            to_snake_case(&terminal.name)
        )?;
    }
    for variable in &grammar.variables {
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
    for symbol in &grammar.virtuals {
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
