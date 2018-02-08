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

using System.Collections.Generic;
using System.IO;
using System.Text;
using Hime.SDK.Grammars;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a generator for parser code for the Rust language
	/// </summary>
	public class ParserRustCodeGenerator : Generator
	{
		/// <summary>
		/// The nmespace of the generated code
		/// </summary>
		private readonly string nmespace;
		/// <summary>
		/// The name of the generated lexer
		/// </summary>
		private readonly string name;
		/// <summary>
		/// Path to the automaton's binary resource
		/// </summary>
		private readonly string binResource;
		/// <summary>
		/// The grammar to generate a parser for
		/// </summary>
		private readonly Grammar grammar;
		/// <summary>
		/// The type of the parser to generate
		/// </summary>
		private readonly string parserType;
		/// <summary>
		/// The type of the automaton
		/// </summary>
		private readonly string automatonType;
		/// <summary>
		/// Whether the target output is an assembly
		/// </summary>
		private readonly bool outputAssembly;

		/// <summary>
		/// Initializes this code generator
		/// </summary>
		/// <param name="unit">The unit to generate code for</param>
		/// <param name="nmespace">The nmespace of the generated code</param>
		/// <param name="binResource">Path to the automaton's binary resource</param>
		public ParserRustCodeGenerator(Unit unit, string nmespace, string binResource)
		{
			this.nmespace = nmespace;
			this.name = unit.Name;
			this.binResource = binResource;
			this.grammar = unit.Grammar;
			if (unit.Method == ParsingMethod.RNGLR1 || unit.Method == ParsingMethod.RNGLALR1)
			{
				this.parserType = "RNGLRParser";
				this.automatonType = "RNGLRAutomaton";
			}
			else
			{
				this.parserType = "LRkParser";
				this.automatonType = "LRkAutomaton";
			}
			this.outputAssembly = unit.CompilationMode == Mode.Assembly || unit.CompilationMode == Mode.SourceAndAssembly;
		}

		/// <summary>
		/// Generates code for the specified file
		/// </summary>
		/// <param name="file">The target file to generate code in</param>
		public void Generate(string file)
		{
			StreamWriter writer = new StreamWriter(file, true, new UTF8Encoding(false));

			writer.WriteLine("/// Static resource for the serialized parser automaton");
			writer.WriteLine("const PARSER_AUTOMATON: &'static [u8] = include_bytes!(\"" + binResource + "\");");
			writer.WriteLine();
			GenerateCodeSymbols(writer);
			GenerateCodeVariables(writer);
			GenerateCodeVirtuals(writer);
			GenerateCodeActions(writer);
			GeneratorCodeConstructors(writer);
			writer.Close();
		}

		/// <summary>
		/// Generates the code for the symbols
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GenerateCodeSymbols(StreamWriter stream)
		{
			foreach (Variable var in grammar.Variables)
			{
				if (var.Name.StartsWith(Grammar.PREFIX_GENERATED_VARIABLE))
					continue;
				stream.WriteLine("/// The unique identifier for variable " + var.Name);
				stream.WriteLine("pub const ID_VARIABLE_{0}: u32 = 0x{1};", Helper.ToUpperCase(var.Name), var.ID.ToString("X4"));
			}
			stream.WriteLine();
			foreach (Virtual var in grammar.Virtuals)
			{
				stream.WriteLine("/// The unique identifier for virtual " + var.Name);
				stream.WriteLine("pub const ID_VIRTUAL_{0}: u32 = 0x{1};", Helper.ToUpperCase(var.Name), var.ID.ToString("X4"));
			}
			stream.WriteLine();
		}

		/// <summary>
		/// Generates the code for the variables
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GenerateCodeVariables(StreamWriter stream)
		{
			stream.WriteLine("/// The collection of variables matched by this parser");
			stream.WriteLine("/// The variables are in an order consistent with the automaton,");
			stream.WriteLine("/// so that variable indices in the automaton can be used to retrieve the variables in this table");
			stream.WriteLine("const VARIABLES: &'static [Symbol] = &[");
			bool first = true;
			foreach (Variable var in grammar.Variables)
			{
				if (!first)
					stream.WriteLine(",");
				stream.Write("    Symbol { id: 0x" + var.ID.ToString("X4") + ", name: \"" + var.Name + "\" }");
				first = false;
			}
			stream.WriteLine("];");
			stream.WriteLine();
		}

		/// <summary>
		/// Generates the code for the virtual symbols
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GenerateCodeVirtuals(StreamWriter stream)
		{
			stream.WriteLine("/// The collection of virtuals matched by this parser");
			stream.WriteLine("/// The virtuals are in an order consistent with the automaton,");
			stream.WriteLine("/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table");
			stream.WriteLine("const VIRTUALS: &'static [Symbol] = &[");
			bool first = true;
			foreach (Virtual v in grammar.Virtuals)
			{
				if (!first)
					stream.WriteLine(",");
				stream.Write("    Symbol { id: 0x" + v.ID.ToString("X4") + ", name: \"" + v.Name + "\" }");
				first = false;
			}
			stream.WriteLine("];");
			stream.WriteLine();
		}

		/// <summary>
		/// Generates the code for the semantic actions
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GenerateCodeActions(StreamWriter stream)
		{
			if (grammar.Actions.Count == 0)
				return;
			stream.WriteLine("/// Represents a set of semantic actions in this parser");
			stream.WriteLine("pub trait Actions {");
			foreach (Action action in grammar.Actions)
			{
				stream.WriteLine("    /// The " + action.Name + " semantic action");
				stream.WriteLine("    fn " + Helper.ToSnakeCase(action.Name) + "(&mut self, head: Symbol, body: &SemanticBody);");
			}
			stream.WriteLine("}");
			stream.WriteLine();
			stream.WriteLine("/// The structure that implements no action");
			stream.WriteLine("pub struct NoActions {}");
			stream.WriteLine();
			stream.WriteLine("impl Actions for NoActions {");
			foreach (Action action in grammar.Actions)
				stream.WriteLine("    fn " + Helper.ToSnakeCase(action.Name) + "(&mut self, _head: Symbol, _body: &SemanticBody) {}");
			stream.WriteLine("}");
			stream.WriteLine();
		}

		/// <summary>
		/// Generates the code for the constructors
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GeneratorCodeConstructors(StreamWriter stream)
		{
			if (grammar.Actions.Count == 0)
			{
				stream.WriteLine("/// Parses the specified string with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_string\"]");
				}
				stream.WriteLine("pub fn parse_string(input: &str) -> ParseResult {");
				stream.WriteLine("    let text = Text::new(input);");
				stream.WriteLine("    parse_text(text)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf16\"]");
				}
				stream.WriteLine("pub fn parse_utf16(input: &mut Read, big_endian: bool) -> ParseResult {");
				stream.WriteLine("    let text = Text::from_utf16_stream(input, big_endian);");
				stream.WriteLine("    parse_text(text)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf8\"]");
				}
				stream.WriteLine("pub fn parse_utf8(input: &mut Read) -> ParseResult {");
				stream.WriteLine("    let text = Text::from_utf8_stream(input);");
				stream.WriteLine("    parse_text(text)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified text with this parser");
				stream.WriteLine("fn parse_text(text: Text) -> ParseResult {");
				stream.WriteLine("    let mut my_actions = |_index: usize, _head: Symbol, _body: &SemanticBody| ();");
				stream.WriteLine("    let mut result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);");
				stream.WriteLine("    {");
				stream.WriteLine("        let data = result.get_parsing_data();");
				stream.WriteLine("        let mut lexer = new_lexer(data.0, data.1);");
				stream.WriteLine("        let automaton = " + automatonType + "::new(PARSER_AUTOMATON);");
				stream.WriteLine("        let mut parser = " + parserType + "::new(&mut lexer, automaton, data.2, &mut my_actions);");
				stream.WriteLine("        parser.parse();");
				stream.WriteLine("    }");
				stream.WriteLine("    result");
				stream.WriteLine("}");
			}
			else
			{
				stream.WriteLine("/// Parses the specified string with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_string\"]");
				}
				stream.WriteLine("pub fn parse_string(input: &str) -> ParseResult {");
				stream.WriteLine("    let mut actions = NoActions {};");
				stream.WriteLine("    parse_string_with(input, &mut actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified string with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_string_with\"]");
				}
				stream.WriteLine("pub fn parse_string_with(input: &str, actions: &mut Actions) -> ParseResult {");
				stream.WriteLine("    let text = Text::new(input);");
				stream.WriteLine("    parse_text(text, actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf16\"]");
				}
				stream.WriteLine("pub fn parse_utf16(input: &mut Read, big_endian: bool) -> ParseResult {");
				stream.WriteLine("    let mut actions = NoActions {};");
				stream.WriteLine("    parse_utf16_with(input, big_endian, &mut actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf16_with\"]");
				}
				stream.WriteLine("pub fn parse_utf16_with(input: &mut Read, big_endian: bool, actions: &mut Actions) -> ParseResult {");
				stream.WriteLine("    let text = Text::from_utf16_stream(input, big_endian);");
				stream.WriteLine("    parse_text(text, actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf8\"]");
				}
				stream.WriteLine("pub fn parse_utf8(input: &mut Read) -> ParseResult {");
				stream.WriteLine("    let mut actions = NoActions {};");
				stream.WriteLine("    parse_utf8_with(input, &mut actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified stream of UTF-16 with this parser");
				if (outputAssembly)
				{
					stream.WriteLine("#[no_mangle]");
					stream.WriteLine("#[export_name = \"" + nmespace + "_parse_utf8_with\"]");
				}
				stream.WriteLine("pub fn parse_utf8_with(input: &mut Read, actions: &mut Actions) -> ParseResult {");
				stream.WriteLine("    let text = Text::from_utf8_stream(input);");
				stream.WriteLine("    parse_text(text, actions)");
				stream.WriteLine("}");
				stream.WriteLine();
				stream.WriteLine("/// Parses the specified text with this parser");
				stream.WriteLine("fn parse_text(text: Text, actions: &mut Actions) -> ParseResult {");
				stream.WriteLine("    let mut my_actions = |index: usize, head: Symbol, body: &SemanticBody| match index {");
				int i = 0;
				foreach (Action action in grammar.Actions)
				{
					stream.WriteLine("        " + i + " => actions." + Helper.ToSnakeCase(action.Name) + "(head, body),");
					i++;
				}
				stream.WriteLine("        _ => ()");
				stream.WriteLine("    };");
				stream.WriteLine();
				stream.WriteLine("    let mut result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);");
				stream.WriteLine("    {");
				stream.WriteLine("        let data = result.get_parsing_data();");
				stream.WriteLine("        let mut lexer = new_lexer(data.0, data.1);");
				stream.WriteLine("        let automaton = " + automatonType + "::new(PARSER_AUTOMATON);");
				stream.WriteLine("        let mut parser = " + parserType + "::new(&mut lexer, automaton, data.2, &mut my_actions);");
				stream.WriteLine("        parser.parse();");
				stream.WriteLine("    }");
				stream.WriteLine("    result");
				stream.WriteLine("}");
			}
		}
	}
}