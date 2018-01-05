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

using System.IO;
using System.Text;
using Hime.Redist.Utils;
using Hime.SDK.Grammars;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a generator for lexer code for the Rust language
	/// </summary>
	public class LexerRustCodeGenerator : Generator
	{
		/// <summary>
		/// The name of the generated lexer
		/// </summary>
		private readonly string name;
		/// <summary>
		/// Path to the automaton's binary resource
		/// </summary>
		private readonly string binResource;
		/// <summary>
		/// The terminals for the lexer
		/// </summary>
		private readonly ROList<Terminal> terminals;
		/// <summary>
		/// The contexts for the lexer
		/// </summary>
		private readonly ROList<string> contexts;
		/// <summary>
		/// The separator terminal
		/// </summary>
		private readonly Terminal separator;

		/// <summary>
		/// Initializes this code generator
		/// </summary>
		/// <param name="unit">The unit to generate code for</param>
		/// <param name="binResource">Path to the automaton's binary resource</param>
		public LexerRustCodeGenerator(Unit unit, string binResource)
		{
			this.name = unit.Name;
			this.binResource = binResource;
			this.terminals = unit.Expected;
			this.contexts = unit.Grammar.Contexts;
			this.separator = unit.Separator;
		}

		/// <summary>
		/// Generates code for the specified file
		/// </summary>
		/// <param name="file">The target file to generate code in</param>
		public void Generate(string file)
		{
			string baseLexer = contexts.Count > 1 ? "ContextSensitiveLexer" : "ContextFreeLexer";
			StreamWriter writer = new StreamWriter(file, false, new UTF8Encoding(false));

			writer.WriteLine("//! Module for the lexer and parser for " + name);
			writer.WriteLine("//! WARNING: this file has been generated by");
			writer.WriteLine("//! Hime Parser Generator " + CompilationTask.Version);
			writer.WriteLine();
			writer.WriteLine("use std::io::Read;");
			writer.WriteLine();
			writer.WriteLine("use hime_redist::errors::ParseErrors;");
			writer.WriteLine("use hime_redist::lexers::automaton::Automaton;");
			writer.WriteLine("use hime_redist::lexers::impls::" + baseLexer + ";");
			writer.WriteLine("use hime_redist::parsers::Parser;");
			writer.WriteLine("use hime_redist::parsers::lrk::LRkAutomaton;");
			writer.WriteLine("use hime_redist::parsers::lrk::LRkParser;");
			writer.WriteLine("use hime_redist::result::ParseResult;");
			writer.WriteLine("use hime_redist::symbols::SemanticBody;");
			writer.WriteLine("use hime_redist::symbols::Symbol;");
			writer.WriteLine("use hime_redist::text::Text;");
			writer.WriteLine("use hime_redist::tokens::TokenRepository;");
			writer.WriteLine();

			writer.WriteLine("/// Static resource for the serialized lexer automaton");
			writer.WriteLine("const LEXER_AUTOMATON: &'static [u8] = include_bytes!(\"" + binResource + "\");");
			writer.WriteLine();

			for (int i = 2; i != terminals.Count; i++)
			{
				Terminal terminal = terminals[i];
				if (terminal.Name.StartsWith(Grammar.PREFIX_GENERATED_TERMINAL))
					continue;
				writer.WriteLine("/// The unique identifier for terminal " + terminal.Name);
				writer.WriteLine("pub const {0}: u32 = 0x{1};", Helper.GetSymbolNameForRust(terminal.Name), terminal.ID.ToString("X4"));
			}
			writer.WriteLine();

			writer.WriteLine("/// The unique identifier for the default context");
			writer.WriteLine("pub const CONTEXT_DEFAULT: u16 = 0;");
			for (int i = 1; i != contexts.Count; i++)
			{
				string context = contexts[i];
				writer.WriteLine("/// The unique identifier for context " + context);
				writer.WriteLine("pub const CONTEXT_{0}: u16 = 0x{1};", context, i.ToString("X4"));
			}
			writer.WriteLine();

			writer.WriteLine("/// The collection of terminals matched by this lexer");
			writer.WriteLine("/// The terminals are in an order consistent with the automaton,");
			writer.WriteLine("/// so that terminal indices in the automaton can be used to retrieve the terminals in this table");
			writer.WriteLine("const TERMINALS: &'static [Symbol] = &[");
			bool first = true;
			foreach (Terminal terminal in terminals)
			{
				if (!first)
					writer.WriteLine(",");
				writer.Write("    Symbol { id: 0x" + terminal.ID.ToString("X4") + ", name: \"" + terminal.ToString().Replace("\"", "\\\"") + "\" }");
				first = false;
			}
			writer.WriteLine("];");
			writer.WriteLine();

			string sep = "FFFF";
			if (separator != null)
				sep = separator.ID.ToString("X4");

			writer.WriteLine("/// Creates a new lexer");
			writer.WriteLine("fn new_lexer<'a>(");
			writer.WriteLine("    repository: TokenRepository<'a>,");
			writer.WriteLine("    errors: &'a mut ParseErrors");
			writer.WriteLine(") -> " + baseLexer + "<'a> {");
			writer.WriteLine("    let automaton = Automaton::new(LEXER_AUTOMATON);");
			writer.WriteLine("    " + baseLexer + "::new(repository, errors, automaton, 0x" + sep + ")");
			writer.WriteLine("}");
			writer.WriteLine();
			writer.Close();
		}
	}
}