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
	/// Represents a platform-agnostic emitter of lexer and parser for a given grammar
	/// </summary>
	public abstract class EmitterBase
	{
		/// <summary>
		/// The suffix for the emitted lexer data files
		/// </summary>
		public const string suffixLexerData = "Lexer.bin";
		/// <summary>
		/// The suffix for the emitted parser data files
		/// </summary>
		public const string suffixParserData = "Parser.bin";
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
		/// Gets the suffix for the emitted lexer code files
		/// </summary>
		public abstract string SuffixLexerCode { get; }
		/// <summary>
		/// Gets suffix for the emitted parser code files
		/// </summary>
		public abstract string SuffixParserCode { get; }
		/// <summary>
		/// Gets suffix for the emitted assemblies
		/// </summary>
		public abstract string SuffixAssembly { get; }


		/// <summary>
		/// Gets the full path and name for the lexer code artifact
		/// </summary>
		public string ArtifactLexerCode { get { return path + grammar.Name + SuffixLexerCode; } }
		/// <summary>
		/// Gets the full path and name for the lexer data artifact
		/// </summary>
		public string ArtifactLexerData { get { return path + grammar.Name + suffixLexerData; } }
		/// <summary>
		/// Gets the full path and name for the parser code artifact
		/// </summary>
		public string ArtifactParserCode { get { return path + grammar.Name + SuffixParserCode; } }
		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		public string ArtifactParserData { get { return path + grammar.Name + suffixParserData; } }
		/// <summary>
		/// Gets the full path and name for the assembly artifact
		/// </summary>
		public string ArtifactAssembly { get { return path + grammar.Name + SuffixAssembly; } }
		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		public string ArtifactDebugGrammar { get { return path + grammar.Name + suffixDebugGrammar; } }
		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		public string ArtifactDebugDFA { get { return path + grammar.Name + suffixDebugDFA; } }
		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		public string ArtifactDebugLRAsText { get { return path + grammar.Name + suffixDebugLRAsText; } }
		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		public string ArtifactDebugLRAsDOT { get { return path + grammar.Name + suffixDebugLRAsDOT; } }


		/// <summary>
		/// The reporter
		/// </summary>
		protected Reporter reporter;
		/// <summary>
		/// The grammar to emit data for
		/// </summary>
		protected Grammars.Grammar grammar;
		/// <summary>
		/// The path for the emitted artifacts
		/// </summary>
		protected string path;
		/// <summary>
		/// The namespace of the generated code
		/// </summary>
		protected string nmspace;
		/// <summary>
		/// The visibility modifier of the generated code
		/// </summary>
		protected Modifier modifier;
		/// <summary>
		/// The parsing method for the generated parser
		/// </summary>
		protected ParsingMethod method;
		/// <summary>
		/// The mode of this emitter
		/// </summary>
		protected Mode mode;

		/// <summary>
		/// The DFA to emit in a lexer
		/// </summary>
		protected Automata.DFA dfa;
		/// <summary>
		/// The terminals matched by the DFA and expected by the parser
		/// </summary>
		protected ROList<Grammars.Terminal> expected;
		/// <summary>
		/// The LR graph to emit in a parser
		/// </summary>
		protected Grammars.LR.Graph graph;


		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="grammar">The grammar to emit data for</param>
		public EmitterBase(Grammars.Grammar grammar) : this(new Reporter(), grammar)
		{
		}
		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="grammar">The grammar to emit data for</param>
		public EmitterBase(Reporter reporter, Grammars.Grammar grammar)
		{
			this.reporter = reporter;
			this.grammar = grammar;
		}

		/// <summary>
		/// Emit the lexer and parser artifacts
		/// </summary>
		/// <param name="path">The output path for the emitted artifacts</param>
		/// <param name="nmspace">The namespace for the emitted code</param>
		/// <param name="modifier">The visibility modifier for the emitted code</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="mode">The output mode</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		public bool Emit(string path, string nmspace, Modifier modifier, ParsingMethod method, Mode mode)
		{
			// setup
			this.path = path;
			if (this.path.Length > 0 && !this.path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				this.path += Path.DirectorySeparatorChar;
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
					File.Delete(ArtifactLexerCode);
					File.Delete(ArtifactLexerData);
					File.Delete(ArtifactParserCode);
					File.Delete(ArtifactParserData);
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
			reporter.Info("Exporting grammar debug data at " + ArtifactDebugGrammar + " ...");
			SDK.Serializers.Export(grammar, ArtifactDebugGrammar);
			if (dfa != null)
			{
				reporter.Info("Exporting DFA debug data at " + ArtifactDebugDFA + " ...");
				SDK.Serializers.ExportDOT(dfa, ArtifactDebugDFA);
			}
			if (graph != null)
			{
				reporter.Info("Exporting LR graph debug data (txt) at " + ArtifactDebugLRAsText + " ...");
				SDK.Serializers.Export(graph, ArtifactDebugLRAsText);
				reporter.Info("Exporting LR graph debug data (dot) at " + ArtifactDebugLRAsDOT + " ...");
				SDK.Serializers.ExportDOT(graph, ArtifactDebugLRAsDOT);
			}
			return true;
		}

		/// <summary>
		/// Generates the lexer for the given grammar
		/// </summary>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool GenerateLexer()
		{
			reporter.Info("Preparing lexer's data ...");
			dfa = GetDFAFor(grammar);
			if (dfa == null)
				return false;
			// retrieve the separator
			string name = grammar.GetOption(Grammars.Grammar.optionSeparator);
			Grammars.Terminal separator = name != null ? grammar.GetTerminalByName(name) : null;

			// generate the lexer's data
			reporter.Info("Exporting lexer data at " + ArtifactLexerData + " ...");
			LexerDataGenerator genData = new LexerDataGenerator(dfa);
			genData.Generate(ArtifactLexerData);
			expected = genData.Expected;

			// generate the lexer's code
			reporter.Info("Exporting lexer code at " + ArtifactLexerCode + " ...");
			Generator genCode = GetLexerCodeGenerator(separator);
			genCode.Generate(ArtifactLexerCode);
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
			reporter.Info("Preparing parser's data ...");
			Grammars.LR.Builder builder = new Grammars.LR.Builder(grammar);
			graph = builder.Build(method);
			foreach (Grammars.LR.Conflict conflict in builder.Conflicts)
				reporter.Error(conflict);
			if (builder.Conflicts.Count != 0)
				return false;
			// get the generator
			Generator generator = null;
			string parserType = null;
			switch (method)
			{
				case ParsingMethod.LR0:
				case ParsingMethod.LR1:
				case ParsingMethod.LALR1:
					generator = new ParserLRkDataGenerator(grammar, graph, expected);
					parserType = "LRk";
					break;
				case ParsingMethod.RNGLR1:
				case ParsingMethod.RNGLALR1:
					generator = new ParserRNGLRDataGenerator(grammar, graph, expected);
					parserType = "RNGLR";
					break;
			}

			// generate the parser's data
			reporter.Info("Exporting parser data at " + ArtifactParserData + " ...");
			generator.Generate(ArtifactParserData);

			// generate the parser's code
			reporter.Info("Exporting parser code at " + ArtifactParserCode + " ...");
			generator = GetParserCodeGenerator(parserType);
			generator.Generate(ArtifactParserCode);
			return true;
		}

		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="separator">The separator terminal</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected abstract Generator GetLexerCodeGenerator(Grammars.Terminal separator);

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="parserType">The type of parser to generate</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected abstract Generator GetParserCodeGenerator(string parserType);

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected abstract bool EmitAssembly();
	}
}