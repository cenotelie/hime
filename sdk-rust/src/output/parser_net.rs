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

//! Module for generating parser code in C#

use crate::errors::Error;
use crate::grammars::{Grammar, PREFIX_GENERATED_TERMINAL, PREFIX_GENERATED_VARIABLE};
use crate::output::get_parser_bin_name_net;
use crate::output::helper::to_upper_camel_case;
use crate::{Modifier, ParsingMethod, CRATE_VERSION};
use std::fs::File;
use std::io::{self, Write};
use std::path::PathBuf;

/// Generates code for the specified file
pub fn write(
    path: Option<&String>,
    file_name: String,
    grammar: &Grammar,
    method: ParsingMethod,
    nmespace: &str,
    modifier: Modifier
) -> Result<(), Error> {
    let mut final_path = PathBuf::new();
    if let Some(path) = path {
        final_path.push(path);
    }
    final_path.push(file_name);
    let file = File::create(final_path)?;
    let mut writer = io::BufWriter::new(file);

    let name = to_upper_camel_case(&grammar.name);
    let modifier = match modifier {
        Modifier::Public => "public",
        Modifier::Internal => "internal"
    };
    let (parser_type, automaton_type) = if method.is_rnglr() {
        ("RNGLRParser", "RNGLRAutomaton")
    } else {
        ("LRkParser", "LRkAutomaton")
    };
    let bin_name = get_parser_bin_name_net(grammar);

    writeln!(writer, "/*")?;
    writeln!(writer, " * WARNING: this file has been generated by")?;
    writeln!(writer, " * Hime Parser Generator {}", CRATE_VERSION)?;
    writeln!(writer, " */")?;
    writeln!(writer)?;
    writeln!(writer, "using System.CodeDom.Compiler;")?;
    writeln!(writer, "using System.Collections.Generic;")?;
    writeln!(writer, "using System.IO;")?;
    writeln!(writer, "using Hime.Redist;")?;
    writeln!(writer, "using Hime.Redist.Lexer;")?;
    writeln!(writer)?;
    writeln!(writer, "namespace {}", nmespace)?;
    writeln!(writer, "{{")?;
    writeln!(writer, "\t/// <summary>")?;
    writeln!(writer, "\t/// Represents a parser")?;
    writeln!(writer, "\t/// </summary>")?;
    writeln!(
        writer,
        "\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(
        writer,
        "\t{} class {}Parser : {}",
        modifier, &name, parser_type
    )?;
    writeln!(writer, "\t{{")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// The automaton for this parser")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\tprivate static readonly {} commonAutomaton = {}.Find(typeof({}Parser), \"{}\");",
        automaton_type, automaton_type, &name, &bin_name
    )?;

    write_code_symbols(&mut writer, grammar)?;
    write_code_variables(&mut writer, grammar)?;
    write_code_virtuals(&mut writer, grammar)?;
    write_code_actions(&mut writer, grammar)?;
    write_code_constructors(&mut writer, grammar)?;
    write_code_visitor(&mut writer, grammar)?;

    writeln!(writer, "\t}}")?;
    writeln!(writer, "}}")?;
    Ok(())
}

/// Generates the code for the symbols
fn write_code_symbols(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Contains the constant IDs for the variables and virtuals in this parser"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(writer, "\t\tpublic class ID")?;
    writeln!(writer, "\t\t{{")?;
    for variable in grammar
        .variables
        .iter()
        .filter(|v| !v.name.starts_with(PREFIX_GENERATED_VARIABLE))
    {
        writeln!(writer, "\t\t\t/// <summary>")?;
        writeln!(
            writer,
            "\t\t\t/// The unique identifier for variable {}",
            &variable.name
        )?;
        writeln!(writer, "\t\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t\tpublic const int Variable{} = 0x{:04X};",
            to_upper_camel_case(&variable.name),
            variable.id
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(writer, "\t\t\t/// <summary>")?;
        writeln!(
            writer,
            "\t\t\t/// The unique identifier for virtual {}",
            &symbol.name
        )?;
        writeln!(writer, "\t\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t\tpublic const int Virtual{} = 0x{:04X};",
            to_upper_camel_case(&symbol.name),
            symbol.id
        )?;
    }
    writeln!(writer, "\t\t}}")?;
    Ok(())
}

/// Generates the code for the variables
fn write_code_variables(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// The collection of variables matched by this parser"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(writer, "\t\t/// <remarks>")?;
    writeln!(
        writer,
        "\t\t/// The variables are in an order consistent with the automaton,"
    )?;
    writeln!(writer, "\t\t/// so that variable indices in the automaton can be used to retrieve the variables in this table")?;
    writeln!(writer, "\t\t/// </remarks>")?;
    writeln!(
        writer,
        "\t\tprivate static readonly Symbol[] variables = {{"
    )?;
    for (index, variable) in grammar.variables.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ", ")?;
        }
        write!(writer, "\t\t\t")?;
        write!(
            writer,
            "new Symbol(0x{:04X}, \"{}\")",
            variable.id, &variable.name
        )?;
    }
    writeln!(writer, " }};")?;
    Ok(())
}

/// Generates the code for the virtual symbols
fn write_code_virtuals(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// The collection of virtuals matched by this parser"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(writer, "\t\t/// <remarks>")?;
    writeln!(
        writer,
        "\t\t/// The virtuals are in an order consistent with the automaton,"
    )?;
    writeln!(writer, "\t\t/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table")?;
    writeln!(writer, "\t\t/// </remarks>")?;
    writeln!(writer, "\t\tprivate static readonly Symbol[] virtuals = {{")?;
    for (index, symbol) in grammar.virtuals.iter().enumerate() {
        if index > 0 {
            writeln!(writer, ", ")?;
        }
        write!(writer, "\t\t\t")?;
        write!(
            writer,
            "new Symbol(0x{:04X}, \"{}\")",
            symbol.id, &symbol.name
        )?;
    }
    writeln!(writer, " }};")?;
    Ok(())
}

/// Generates the code for the semantic actions
fn write_code_actions(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    if grammar.actions.is_empty() {
        return Ok(());
    }
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Represents a set of semantic actions in this parser"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(writer, "\t\tpublic class Actions")?;
    writeln!(writer, "\t\t{{")?;
    for action in grammar.actions.iter() {
        writeln!(writer, "\t\t\t/// <summary>")?;
        writeln!(writer, "\t\t\t/// The {} semantic action", &action.name)?;
        writeln!(writer, "\t\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t\tpublic virtual void {}(Symbol head, SemanticBody body) {{}}",
            to_upper_camel_case(&action.name)
        )?;
    }
    writeln!(writer)?;
    writeln!(writer, "\t\t}}")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Represents a set of empty semantic actions (do nothing)"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\tprivate static readonly Actions noActions = new Actions();"
    )?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Gets the set of semantic actions in the form a table consistent with the automaton")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"input\">A set of semantic actions</param>"
    )?;
    writeln!(
        writer,
        "\t\t/// <returns>A table of semantic actions</returns>"
    )?;
    writeln!(
        writer,
        "\t\tprivate static SemanticAction[] GetUserActions(Actions input)"
    )?;
    writeln!(writer, "\t\t{{")?;
    writeln!(
        writer,
        "\t\t\tSemanticAction[] result = new SemanticAction[{}];",
        grammar.actions.len()
    )?;
    for (index, action) in grammar.actions.iter().enumerate() {
        writeln!(
            writer,
            "\t\t\tresult[{}] = new SemanticAction(input.{});",
            index,
            to_upper_camel_case(&action.name)
        )?;
    }
    writeln!(writer, "\t\t\treturn result;")?;
    writeln!(writer, "\t\t}}")?;

    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Gets the set of semantic actions in the form a table consistent with the automaton")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"input\">A set of semantic actions</param>"
    )?;
    writeln!(
        writer,
        "\t\t/// <returns>A table of semantic actions</returns>"
    )?;
    writeln!(writer, "\t\tprivate static SemanticAction[] GetUserActions(Dictionary<string, SemanticAction> input)")?;
    writeln!(writer, "\t\t{{")?;
    writeln!(
        writer,
        "\t\t\tSemanticAction[] result = new SemanticAction[{}];",
        grammar.actions.len()
    )?;
    for (index, action) in grammar.actions.iter().enumerate() {
        writeln!(
            writer,
            "\t\t\tresult[{}] = input[\"{}\"];",
            index, &action.name
        )?;
    }
    writeln!(writer, "\t\t\treturn result;")?;
    writeln!(writer, "\t\t}}")?;
    Ok(())
}

/// Generates the code for the constructors
fn write_code_constructors(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    let name = to_upper_camel_case(&grammar.name);
    if grammar.actions.is_empty() {
        writeln!(writer, "\t\t/// <summary>")?;
        writeln!(writer, "\t\t/// Initializes a new instance of the parser")?;
        writeln!(writer, "\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t/// <param name=\"lexer\">The input lexer</param>"
        )?;
        writeln!(writer, "\t\tpublic {}Parser({}Lexer lexer) : base (commonAutomaton, variables, virtuals, null, lexer) {{ }}", &name, &name)?;
    } else {
        writeln!(writer, "\t\t/// <summary>")?;
        writeln!(writer, "\t\t/// Initializes a new instance of the parser")?;
        writeln!(writer, "\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t/// <param name=\"lexer\">The input lexer</param>"
        )?;
        writeln!(writer, "\t\tpublic {}Parser({}Lexer lexer) : base (commonAutomaton, variables, virtuals, GetUserActions(noActions), lexer) {{ }}", &name, &name)?;

        writeln!(writer, "\t\t/// <summary>")?;
        writeln!(writer, "\t\t/// Initializes a new instance of the parser")?;
        writeln!(writer, "\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t/// <param name=\"lexer\">The input lexer</param>"
        )?;
        writeln!(
            writer,
            "\t\t/// <param name=\"actions\">The set of semantic actions</param>"
        )?;
        writeln!(writer, "\t\tpublic {}Parser({}Lexer lexer, Actions actions) : base (commonAutomaton, variables, virtuals, GetUserActions(actions), lexer) {{ }}", &name, &name)?;

        writeln!(writer, "\t\t/// <summary>")?;
        writeln!(writer, "\t\t/// Initializes a new instance of the parser")?;
        writeln!(writer, "\t\t/// </summary>")?;
        writeln!(
            writer,
            "\t\t/// <param name=\"lexer\">The input lexer</param>"
        )?;
        writeln!(
            writer,
            "\t\t/// <param name=\"actions\">The set of semantic actions</param>"
        )?;
        writeln!(writer, "\t\tpublic {}Parser({}Lexer lexer, Dictionary<string, SemanticAction> actions) : base (commonAutomaton, variables, virtuals, GetUserActions(actions), lexer) {{ }}", &name, &name)?;
    }
    Ok(())
}

/// Generates the visitor for the parse result
fn write_code_visitor(writer: &mut dyn Write, grammar: &Grammar) -> Result<(), Error> {
    writeln!(writer)?;
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Visitor interface")?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t[GeneratedCodeAttribute(\"Hime.SDK\", \"{}\")]",
        CRATE_VERSION
    )?;
    writeln!(writer, "\t\tpublic class Visitor")?;
    writeln!(writer, "\t\t{{")?;
    for terminal in grammar.terminals.iter() {
        if terminal.id <= 2 || terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "\t\t\tpublic virtual void OnTerminal{}(ASTNode node) {{}}",
            to_upper_camel_case(&terminal.name)
        )?;
    }
    for variable in grammar.variables.iter() {
        if variable.name.starts_with(PREFIX_GENERATED_VARIABLE) {
            continue;
        }
        writeln!(
            writer,
            "\t\t\tpublic virtual void OnVariable{}(ASTNode node) {{}}",
            to_upper_camel_case(&variable.name)
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(
            writer,
            "\t\t\tpublic virtual void OnVirtual{}(ASTNode node) {{}}",
            to_upper_camel_case(&symbol.name)
        )?;
    }
    writeln!(writer, "\t\t}}")?;
    writeln!(writer)?;
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(writer, "\t\t/// Walk the AST of a result using a visitor")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"result\">The parse result</param>"
    )?;
    writeln!(
        writer,
        "\t\t/// <param name=\"visitor\">The visitor to use</param>"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\tpublic static void Visit(ParseResult result, Visitor visitor)"
    )?;
    writeln!(writer, "\t\t{{")?;
    writeln!(writer, "\t\t\tVisitASTNode(result.Root, visitor);")?;
    writeln!(writer, "\t\t}}")?;
    writeln!(writer)?;
    writeln!(writer, "\t\t/// <summary>")?;
    writeln!(
        writer,
        "\t\t/// Walk the sub-AST from the specified node using a visitor"
    )?;
    writeln!(writer, "\t\t/// </summary>")?;
    writeln!(
        writer,
        "\t\t/// <param name=\"node\">The AST node to start from</param>"
    )?;
    writeln!(
        writer,
        "\t\t/// <param name=\"visitor\">The visitor to use</param>"
    )?;
    writeln!(
        writer,
        "\t\tpublic static void VisitASTNode(ASTNode node, Visitor visitor)"
    )?;
    writeln!(writer, "\t\t{{")?;
    writeln!(
        writer,
        "\t\t\tfor (int i = 0; i < node.Children.Count; i++)"
    )?;
    writeln!(writer, "\t\t\t\tVisitASTNode(node.Children[i], visitor);")?;
    writeln!(writer, "\t\t\tswitch(node.Symbol.ID)")?;
    writeln!(writer, "\t\t\t{{")?;
    for terminal in grammar.terminals.iter() {
        if terminal.id <= 2 || terminal.name.starts_with(PREFIX_GENERATED_TERMINAL) {
            continue;
        }
        writeln!(
            writer,
            "\t\t\t\tcase 0x{:04X}: visitor.OnTerminal{}(node); break;",
            terminal.id,
            to_upper_camel_case(&terminal.name)
        )?;
    }
    for variable in grammar.variables.iter() {
        if variable.name.starts_with(PREFIX_GENERATED_VARIABLE) {
            continue;
        }
        writeln!(
            writer,
            "\t\t\t\tcase 0x{:04X}: visitor.OnVariable{}(node); break;",
            variable.id,
            to_upper_camel_case(&variable.name)
        )?;
    }
    for symbol in grammar.virtuals.iter() {
        writeln!(
            writer,
            "\t\t\t\tcase 0x{:04X}: visitor.OnVirtual{}(node); break;",
            symbol.id,
            to_upper_camel_case(&symbol.name)
        )?;
    }
    writeln!(writer, "\t\t\t}}")?;
    writeln!(writer, "\t\t}}")?;
    Ok(())
}
