/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
*
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/
using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Represents an emitter of lexer and parser for a given grammar
	/// </summary>
	public class Emitter
	{
		/// <summary>
		/// The suffix for the emitted lexer code files
		/// </summary>
		public const string suffixLexerCode = "Lexer.cs";
		/// <summary>
		/// The suffix for the emitted lexer data files
		/// </summary>
		public const string suffixLexerData = "Lexer.bin";
		/// <summary>
		/// The suffix for the emitted parser code files
		/// </summary>
		public const string suffixParserCode = "Parser.cs";
		/// <summary>
		/// The suffix for the emitted parser data files
		/// </summary>
		public const string suffixParserData = "Parser.bin";
		/// <summary>
		/// The suffix for the emitted assemblies
		/// </summary>
		public const string suffixAssembly = ".dll";
		/// <summary>
		/// The suffix for the emitted debug grammar data
		/// </summary>
		public const string suffixDebugGrammar = "Grammar.txt";
		/// <summary>
		/// The suffix for the emitted debug DFA data
		/// </summary>
		public const string suffixDebugDFA = "DFA.dot";
		/// <summary>
		/// The suffix for the emitted debug LR graph as text
		/// </summary>
		public const string suffixDebugLRAsText = "LRGraph.txt";
		/// <summary>
		/// The suffix for the emitted debug LR graph as DOT
		/// </summary>
		public const string suffixDebugLRAsDOT = "LRGraph.dot";


		/// <summary>
		/// The reporter
		/// </summary>
		private Reporter reporter;
		/// <summary>
		/// The grammar to emit data for
		/// </summary>
		private Grammars.Grammar grammar;
		/// <summary>
		/// The prefix for the emitted artifacts
		/// </summary>
		private string prefix;
		/// <summary>
		/// The namespace of the generated code
		/// </summary>
		private string nmspace;
		/// <summary>
		/// The visibility modifier of the generated code
		/// </summary>
		private Modifier modifier;
		/// <summary>
		/// The parsing method for the generated parser
		/// </summary>
		private ParsingMethod method;
		/// <summary>
		/// The mode of this emitter
		/// </summary>
		private Mode mode;

		/// <summary>
		/// The DFA to emit in a lexer
		/// </summary>
		private Automata.DFA dfa;
		/// <summary>
		/// The terminals matched by the DFA and expected by the parser
		/// </summary>
		private ROList<Grammars.Terminal> expected;
		/// <summary>
		/// The LR graph to emit in a parser
		/// </summary>
		private Grammars.LR.Graph graph;


		/// <summary>
		/// Initializes this loader
		/// </summary>
		/// <param name="grammar">The grammar to emit data for</param>
		public Emitter(Grammars.Grammar grammar) : this(new Reporter(typeof(Emitter)), grammar)
		{
		}
		/// <summary>
		/// Initializes this loader
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="grammar">The grammar to emit data for</param>
		public Emitter(Reporter reporter, Grammars.Grammar grammar)
		{
			this.reporter = reporter;
			this.grammar = grammar;
		}

		/// <summary>
		/// Emit the lexer and parser artifacts
		/// </summary>
		/// <param name="prefix">The prefix for the emitted artifacts</param>
		/// <param name="nmspace">The namespace for the emitted code</param>
		/// <param name="modifier">The visibility modifier for the emitted code</param>
		/// <param name="method">The parsing method to use</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		public bool Emit(string prefix, string nmspace, Modifier modifier, ParsingMethod method, Mode mode)
		{
			// setup
			this.prefix = prefix;
			this.nmspace = nmspace;
			this.modifier = modifier;
			this.method = method;
			this.mode = mode;
			this.dfa = null;
			this.expected = new ROList<Grammars.Terminal>(null);
			this.graph = null;
			// emit the artifacts
			bool success = true;
			if (!EmitBaseArtifacts())
				success = false;
			if (mode == Mode.Debug && !EmitDebugArtifacts())
				success = false;
			return success;
		}

		/// <summary>
		/// Emits the base artifacts for the lexer and parser
		/// </summary>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool EmitBaseArtifacts()
		{
			if (!GenerateLexer())
				return false;
			if (!GenerateParser())
				return false;
			if (mode == Mode.Assembly || mode == Mode.SourceAndAssembly)
			{
				if (!EmitAssembly())
					return false;
				if (mode == Mode.Assembly)
				{
					File.Delete(prefix + suffixLexerCode);
					File.Delete(prefix + suffixLexerData);
					File.Delete(prefix + suffixParserCode);
					File.Delete(prefix + suffixParserData);
				}
			}
			return true;
		}

		/// <summary>
		/// Emits the debug artifacts for the lexer and parser
		/// </summary>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool EmitDebugArtifacts()
		{
			reporter.Info("Exporting grammar debug data at " + prefix + suffixDebugGrammar + " ...");
			SDK.Serializers.Export(grammar, prefix + suffixDebugGrammar);
			if (dfa != null)
			{
				reporter.Info("Exporting DFA debug data at " + prefix + suffixDebugDFA + " ...");
				SDK.Serializers.ExportDOT(dfa, prefix + suffixDebugDFA);
			}
			if (graph != null)
			{
				reporter.Info("Exporting LR graph debug data (txt) at " + prefix + suffixDebugLRAsDOT + " ...");
				SDK.Serializers.ExportDOT(graph, prefix + suffixDebugLRAsDOT);
				reporter.Info("Exporting LR graph debug data (dot) at " + prefix + suffixDebugLRAsText + " ...");
				SDK.Serializers.Export(graph, prefix + suffixDebugLRAsText);
			}
			return true;
		}

		/// <summary>
		/// Generates the lexer for the given grammar
		/// </summary>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool GenerateLexer()
		{
			dfa = GetDFAFor(grammar);
			if (dfa == null)
				return false;
			// retrieve the separator
			string name = grammar.GetOption(Grammars.Grammar.optionSeparator);
			Grammars.Terminal separator = name != null ? grammar.GetTerminalByName(name) : null;
			// get the generator
			Output.LexerGenerator generator = new Hime.CentralDogma.Output.LexerGenerator(dfa, separator);
			// generate the lexer's code
			reporter.Info("Exporting lexer code at " + prefix + suffixLexerCode + " ...");
			StreamWriter txtOutput = OpenOutputStream(prefix + suffixLexerCode, nmspace, true);
			generator.GenerateCode(txtOutput, grammar.Name, modifier, prefix + suffixLexerData);
			CloseOutputStream(txtOutput);
			// generate the lexer's data
			reporter.Info("Exporting lexer data at " + prefix + suffixLexerData + " ...");
			BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + suffixLexerData, FileMode.Create));
			generator.GenerateData(binOutput);
			binOutput.Close();
			expected = generator.Expected;
			return true;
		}

		/// <summary>
		/// Gets the DFA for the provided grammar
		/// </summary>
		/// <param name="grammar">The grammar to generate a lexer for</param>
		/// <returns>The corresponding DFA; or <c>null</c> if it is not well-formed</returns>
		private Automata.DFA GetDFAFor(Grammars.Grammar grammar)
		{
			// build the lexer's dfa
			Automata.DFA dfa = grammar.BuildDFA();
			// retrieve the separator
			string name = grammar.GetOption(Grammars.Grammar.optionSeparator);
			Grammars.Terminal separator = name != null ? grammar.GetTerminalByName(name) : null;
			if (name != null)
			{
				// a separator is defined
				if (separator == null)
				{
					// but could not be found ...
					reporter.Error(string.Format("Terminal {0} specified as the separator is undefined", name));
					return null;
				}
				// look for the separator in the dfa
				bool found = false;
				Automata.FinalItem superceding = null;
				foreach (Automata.DFAState state in dfa.States)
				{
					if (state.TopItem == separator)
					{
						found = true;
						break;
					}
					else if (state.Items.Contains(separator))
					{
						superceding = state.TopItem;
					}
				}
				if (!found)
				{
					if (superceding != null)
						reporter.Error(string.Format("Terminal {0} defined as the separator cannot be matched, it is superceded by {1}", separator, superceding));
					else
						reporter.Error(string.Format("Terminal {0} defined as the separator cannot be matched", separator));
					return null;
				}
			}
			// check well-formedness
			foreach (Automata.FinalItem item in dfa.Entry.Items)
				reporter.Error(string.Format("Terminal {0} can be an empty string, this is forbidden", item.ToString()));
			if (dfa.Entry.TopItem != null)
				return null;
			return dfa;
		}

		/// <summary>
		/// Generates the parser for the given grammar
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool GenerateParser()
		{
			// build the LR graph
			Grammars.LR.Builder builder = new Grammars.LR.Builder(grammar);
			graph = builder.Build(method);
			foreach (Grammars.LR.Conflict conflict in builder.Conflicts)
				reporter.Error(conflict);
			if (builder.Conflicts.Count != 0)
				return false;
			// get the generator
			Output.ParserGenerator generator = null;
			switch (method)
			{
				case ParsingMethod.LR0:
				case ParsingMethod.LR1:
				case ParsingMethod.LALR1:
					generator = new Output.ParserGeneratorLRk(grammar, graph, expected);
					break;
				case ParsingMethod.RNGLR1:
				case ParsingMethod.RNGLALR1:
					generator = new Output.ParserGeneratorRNGLR(grammar, graph, expected);
					break;
			}
			// generate the parser's code
			reporter.Info("Exporting parser code at " + prefix + suffixParserCode + " ...");
			StreamWriter txtOutput = OpenOutputStream(prefix + suffixParserCode, nmspace, false);
			generator.GenerateCode(txtOutput, grammar.Name, modifier, prefix + suffixParserData);
			CloseOutputStream(txtOutput);
			// generate the parser's data
			reporter.Info("Exporting parser data at " + prefix + suffixParserData + " ...");
			BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + suffixParserData, FileMode.Create));
			generator.GenerateData(binOutput);
			binOutput.Close();
			return true;
		}

		/// <summary>
		/// Opens a text stream to an output file for writing code
		/// </summary>
		/// <param name="fileName">The file to output to</param>
		/// <param name="nmespace">The namespace for the code to output</param>
		/// <param name="lexer"><c>true</c> if this is to output a lexer</param>
		/// <returns>The stream to write to</returns>
		/// <remarks>It is the responsability of the caller to close the returned stream</remarks>
		private StreamWriter OpenOutputStream(string fileName, string nmespace, bool lexer)
		{
			StreamWriter writer = new StreamWriter(fileName, false, new System.Text.UTF8Encoding(false));
			writer.WriteLine("/*");
			writer.WriteLine(" * WARNING: this file has been generated by");
			writer.WriteLine(" * Hime Parser Generator " + CompilationTask.Version);
			writer.WriteLine(" */");
			writer.WriteLine();
			writer.WriteLine("using System.Collections.Generic;");
			writer.WriteLine("using Hime.Redist;");
			if (lexer)
				writer.WriteLine("using Hime.Redist.Lexer;");
			else
				writer.WriteLine("using Hime.Redist.Parsers;");
			writer.WriteLine();
			writer.WriteLine("namespace " + nmespace);
			writer.WriteLine("{");
			return writer;
		}

		/// <summary>
		/// Closes an stream that has been opened
		/// </summary>
		/// <param name="writer">The stream to close</param>
		private void CloseOutputStream(StreamWriter writer)
		{
			writer.WriteLine("}");
			writer.Close();
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool EmitAssembly()
		{
			reporter.Info("Building assembly " + prefix + suffixAssembly + " ...");
			string redist = System.Reflection.Assembly.GetAssembly(typeof(Hime.Redist.ParseResult)).Location;
			bool hasError = false;
			using (System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#"))
			{
				System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
				compilerparams.GenerateExecutable = false;
				compilerparams.GenerateInMemory = false;
				compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
				compilerparams.ReferencedAssemblies.Add("System.dll");
				compilerparams.ReferencedAssemblies.Add(redist);
				compilerparams.EmbeddedResources.Add(prefix + suffixLexerData);
				compilerparams.EmbeddedResources.Add(prefix + suffixParserData);
				compilerparams.OutputAssembly = prefix + suffixAssembly;
				System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] {
					prefix + suffixLexerCode,
					prefix + suffixParserCode
				});
				foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
				{
					reporter.Error(error.ToString());
					hasError = true;
				}
			}
			return (!hasError);
		}
	}
}