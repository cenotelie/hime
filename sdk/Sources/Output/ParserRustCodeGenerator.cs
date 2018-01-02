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
		/// The visibility modifier for the generated code
		/// </summary>
		private readonly Modifier modifier;
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
		private readonly Grammars.Grammar grammar;
		/// <summary>
		/// The type of the parser to generate
		/// </summary>
		private readonly string parserType;
		/// <summary>
		/// The type of the automaton
		/// </summary>
		private readonly string automatonType;

		/// <summary>
		/// Initializes this code generator
		/// </summary>
		/// <param name="unit">The unit to generate code for</param>
		/// <param name="binResource">Path to the automaton's binary resource</param>
		public ParserRustCodeGenerator(Unit unit, string binResource)
		{
			nmespace = unit.Namespace;
			modifier = unit.Modifier;
			name = unit.Name;
			this.binResource = binResource;
			grammar = unit.Grammar;
			if (unit.Method == ParsingMethod.RNGLR1 || unit.Method == ParsingMethod.RNGLALR1)
			{
				parserType = "RNGLRParser";
				automatonType = "RNGLRAutomaton";
			}
			else
			{
				parserType = "LRkParser";
				automatonType = "LRkAutomaton";
			}
		}

		/// <summary>
		/// Generates code for the specified file
		/// </summary>
		/// <param name="file">The target file to generate code in</param>
		public void Generate(string file)
		{
			StreamWriter writer = new StreamWriter(file, true, new System.Text.UTF8Encoding(false));

			writer.WriteLine("/// Static resource for the serialized parser automaton");
			writer.WriteLine("const PARSER_AUTOMATON: &'static [u8] = include_bytes!(\"" + binResource + "\");");
			writer.WriteLine();

			GenerateCodeSymbols(writer);
			GenerateCodeVariables(writer);
			GenerateCodeVirtuals(writer);

			GeneratorCodeConstructors(writer);

			// writer.WriteLine("/**");
			// writer.WriteLine(" * Represents a parser");
			// writer.WriteLine(" */");
			// writer.WriteLine(mod + "class " + name + "Parser extends " + parserType + " {");

			// writer.WriteLine("    /**");
			// writer.WriteLine("     * The automaton for this parser");
			// writer.WriteLine("     */");
			// writer.WriteLine("    private static final " + automatonType + " commonAutomaton = " + automatonType + ".find(" + name + "Parser.class, \"" + binResource + "\");");


			// GenerateCodeActions(writer);
			

			// writer.WriteLine("}");
			writer.Close();
		}

		/// <summary>
		/// Generates the code for the symbols
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GenerateCodeSymbols(StreamWriter stream)
		{
			Dictionary<Grammars.Variable, string> nameVariables = new Dictionary<Grammars.Variable, string>();
			Dictionary<Grammars.Virtual, string> nameVirtuals = new Dictionary<Grammars.Virtual, string>();
			foreach (Grammars.Variable var in grammar.Variables)
			{
				if (var.Name.StartsWith(Grammars.Grammar.PREFIX_GENERATED_VARIABLE))
					continue;
				nameVariables.Add(var, Helper.SanitizeNameRust(var.Name));
			}
			foreach (Grammars.Virtual var in grammar.Virtuals)
			{
				string name = Helper.SanitizeNameRust(var.Name);
				while (nameVariables.ContainsValue(name) || nameVirtuals.ContainsValue(name))
					name += Helper.VIRTUAL_SUFFIX.ToUpper();
				nameVirtuals.Add(var, name);
			}

			foreach (KeyValuePair<Grammars.Variable, string> pair in nameVariables)
			{
				stream.WriteLine("/// The unique identifier for variable " + pair.Key.Name);
				stream.WriteLine("pub const {0}: u32 = 0x{1};", pair.Value, pair.Key.ID.ToString("X4"));
			}
			stream.WriteLine();
			foreach (KeyValuePair<Grammars.Virtual, string> pair in nameVirtuals)
			{
				stream.WriteLine("/// The unique identifier for virtual " + pair.Key.Name);
				stream.WriteLine("pub const {0}: u32 = 0x{1};", pair.Value, pair.Key.ID.ToString("X4"));
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
			foreach (Grammars.Variable var in grammar.Variables)
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
			foreach (Grammars.Virtual v in grammar.Virtuals)
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
			stream.WriteLine("    /**");
			stream.WriteLine("     * Represents a set of semantic actions in this parser");
			stream.WriteLine("     */");
			stream.WriteLine("    public static class Actions {");
			foreach (Grammars.Action action in grammar.Actions)
			{
				stream.WriteLine("        /**");
				stream.WriteLine("         * The " + action.Name + " semantic action");
				stream.WriteLine("         */");
				stream.WriteLine("        public void " + action.Name + "(Symbol head, SemanticBody body) { }");
			}
			stream.WriteLine();
			stream.WriteLine("    }");

			stream.WriteLine("    /**");
			stream.WriteLine("     * Represents a set of empty semantic actions (do nothing)");
			stream.WriteLine("     */");
			stream.WriteLine("    private static final Actions noActions = new Actions();");

			stream.WriteLine("    /**");
			stream.WriteLine("     * Gets the set of semantic actions in the form a table consistent with the automaton");
			stream.WriteLine("     *");
			stream.WriteLine("     * @param input A set of semantic actions");
			stream.WriteLine("     * @return A table of semantic actions");
			stream.WriteLine("     */");
			stream.WriteLine("    private static SemanticAction[] getUserActions(final Actions input) {");
			stream.WriteLine("        SemanticAction[] result = new SemanticAction[" + grammar.Actions.Count + "];");
			int i = 0;
			foreach (Grammars.Action action in grammar.Actions)
			{
				stream.WriteLine("        result[" + i + "] = new SemanticAction() { @Override public void execute(Symbol head, SemanticBody body) { input." + action.Name + "(head, body); } };");
				i++;
			}
			stream.WriteLine("        return result;");
			stream.WriteLine("    }");

			stream.WriteLine("    /**");
			stream.WriteLine("     * Gets the set of semantic actions in the form a table consistent with the automaton");
			stream.WriteLine("     *");
			stream.WriteLine("     * @param input A set of semantic actions");
			stream.WriteLine("     * @return A table of semantic actions");
			stream.WriteLine("     */");
			stream.WriteLine("    private static SemanticAction[] getUserActions(Map<String, SemanticAction> input)");
			stream.WriteLine("    {");
			stream.WriteLine("        SemanticAction[] result = new SemanticAction[" + grammar.Actions.Count + "];");
			i = 0;
			foreach (Grammars.Action action in grammar.Actions)
			{
				stream.WriteLine("        result[" + i + "] = input.get(\"" + action.Name + "\");");
				i++;
			}
			stream.WriteLine("        return result;");
			stream.WriteLine("    }");
		}

		/// <summary>
		/// Generates the code for the constructors
		/// </summary>
		/// <param name="stream">The output stream</param>
		private void GeneratorCodeConstructors(StreamWriter stream)
		{
			stream.WriteLine("/// Parses the specified string with this parser");
			stream.WriteLine("pub fn parse_string(input: &str) -> ParseResult {");
			stream.WriteLine("    let text = Text::new(input);");
			stream.WriteLine("    let result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);");
			stream.WriteLine("    let lexer = new_lexer(&mut result);");
			stream.WriteLine("    let automaton = let automaton = LRkAutomaton::new(PARSER_AUTOMATON);");
			stream.WriteLine("    result");
			stream.WriteLine("}");
			stream.WriteLine();

			stream.WriteLine("/// Parses the specified stream of UTF-16 encoding points");
			stream.WriteLine("pub fn parse_utf16(input: &mut Read, big_endian: bool) -> ParseResult {");
			stream.WriteLine("    let text = Text::from_utf16_stream(input, big_endian);");
			stream.WriteLine("    let result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);");
			stream.WriteLine("    result");
			stream.WriteLine("}");
			stream.WriteLine();

			stream.WriteLine("/// Parses the specified stream of UTF-8 encoding points");
			stream.WriteLine("pub fn parse_utf8(input: &mut Read) -> ParseResult {");
			stream.WriteLine("    let text = Text::from_utf8_stream(input);");
			stream.WriteLine("    let result = ParseResult::new(TERMINALS, VARIABLES, VIRTUALS, text);");
			stream.WriteLine("    result");
			stream.WriteLine("}");
			stream.WriteLine();
		}
	}
}