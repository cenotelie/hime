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

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents a platform-agnostic emitter of lexer and parser for a given grammar
	/// </summary>
	public abstract class EmitterBase
	{
		/// <summary>
		/// The suffix for the emitted lexer data files
		/// </summary>
		public const string SUFFIX_LEXER_DATA = "Lexer.bin";
		/// <summary>
		/// The suffix for the emitted parser data files
		/// </summary>
		public const string SUFFIX_PARSER_DATA = "Parser.bin";
		/// <summary>
		/// The suffix for the emitted debug grammar data
		/// </summary>
		public const string SUFFIX_DEBUG_GRAMMAR = "Grammar.txt";
		/// <summary>
		/// The suffix for the emitted debug DFA data
		/// </summary>
		public const string SUFFIX_DEBUG_DFA = "DFA.dot";
		/// <summary>
		/// The suffix for the emitted debug LR graph as text
		/// </summary>
		public const string SUFFIX_DEBUG_LR_AS_TEXT = "LRGraph.txt";
		/// <summary>
		/// The suffix for the emitted debug LR graph as DOT
		/// </summary>
		public const string SUFFIX_DEBUG_LR_AS_DOT = "LRGraph.dot";
		/// <summary>
		/// The default name of the assembly when it contains multiple parsers
		/// </summary>
		public const string DEFAULT_COMPOSITE_ASSEMBLY_NAME = "Parsers";

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
		/// The reporter
		/// </summary>
		protected readonly Reporter reporter;
		/// <summary>
		/// The units to emit artifacts for
		/// </summary>
		protected readonly List<Unit> units;
		/// <summary>
		/// The path for the emitted artifacts
		/// </summary>
		protected string path;
		/// <summary>
		/// The mode of this emitter
		/// </summary>
		protected Mode mode;

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="units">The units to emit data for</param>
		protected EmitterBase(List<Unit> units) : this(new Reporter(), units)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		protected EmitterBase(Unit unit) : this(new Reporter(), unit)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="units">The units to emit data for</param>
		protected EmitterBase(Reporter reporter, List<Unit> units)
		{
			this.reporter = reporter;
			this.units = new List<Unit>(units);
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="unit">The unit to emit data for</param>
		protected EmitterBase(Reporter reporter, Unit unit)
		{
			this.reporter = reporter;
			units = new List<Unit>();
			units.Add(unit);
		}

		/// <summary>
		/// Gets the full path and name for the lexer code artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactLexerCode(Unit unit)
		{
			return path + unit.Name + SuffixLexerCode;
		}

		/// <summary>
		/// Gets the full path and name for the lexer data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactLexerData(Unit unit)
		{
			return path + unit.Name + SUFFIX_LEXER_DATA;
		}

		/// <summary>
		/// Gets the full path and name for the parser code artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactParserCode(Unit unit)
		{
			return path + unit.Name + SuffixParserCode;
		}

		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactParserData(Unit unit)
		{
			return path + unit.Name + SUFFIX_PARSER_DATA;
		}

		/// <summary>
		/// Gets the full path and name for the assembly artifact
		/// </summary>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactAssembly()
		{
			if (units.Count == 1)
				return path + units[0].Name + SuffixAssembly;
			return path + DEFAULT_COMPOSITE_ASSEMBLY_NAME + SuffixAssembly;
		}

		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactDebugGrammar(Unit unit)
		{
			return path + unit.Name + SUFFIX_DEBUG_GRAMMAR;
		}

		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactDebugDFA(Unit unit)
		{
			return path + unit.Name + SUFFIX_DEBUG_DFA;
		}

		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactDebugLRAsText(Unit unit)
		{
			return path + unit.Name + SUFFIX_DEBUG_LR_AS_TEXT;
		}

		/// <summary>
		/// Gets the full path and name for the parser data artifact
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <returns>The full path and name for the lexer code artifact</returns>
		public string GetArtifactDebugLRAsDOT(Unit unit)
		{
			return path + unit.Name + SUFFIX_DEBUG_LR_AS_DOT;
		}

		/// <summary>
		/// Emit the lexer and parser artifacts
		/// </summary>
		/// <param name="path">The output path for the emitted artifacts</param>
		/// <param name="mode">The output mode</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		public bool Emit(string path, Mode mode)
		{
			// setup
			this.path = path;
			if (this.path.Length > 0 && !this.path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				this.path += Path.DirectorySeparatorChar;
			this.mode = mode;

			bool errors = false;
			for (int i = 0; i != units.Count; i++)
			{
				if (!EmitBaseArtifacts(units[i]))
				{
					reporter.Error("Failed to emit the base artifacts for " + units[i].Name);
					reporter.Warn("Grammar " + units[i].Name + " will be dropped");
					units.RemoveAt(i);
					i--;
					errors = true;
				}
			}

			if (mode == Mode.Assembly || mode == Mode.SourceAndAssembly)
			{
				if (!EmitAssembly())
				{
					reporter.Error("Failed to emit the assembly " + GetArtifactAssembly());
					return false;
				}
				if (mode == Mode.Assembly)
				{
					foreach (Unit unit in units)
					{
						File.Delete(GetArtifactLexerCode(unit));
						File.Delete(GetArtifactLexerData(unit));
						File.Delete(GetArtifactParserCode(unit));
						File.Delete(GetArtifactParserData(unit));
					}
				}
			}
			return !errors;
		}

		/// <summary>
		/// Emits the base artifacts (lexer, parser and debug) for the specified unit
		/// </summary>
		/// <param name="unit">The unit to generate artifacts for</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool EmitBaseArtifacts(Unit unit)
		{
			// prepare the unit
			bool ok = unit.Prepare(reporter);
			if (ok)
				ok = unit.BuildLexerData(reporter);
			if (ok)
				ok = unit.BuildParserData(reporter);
			// output the artifacts
			if (ok)
				ok = GenerateLexer(unit);
			if (ok)
				ok = GenerateParser(unit);
			if (mode == Mode.Debug && ok)
				ok = EmitDebugArtifacts(unit);
			return ok;
		}

		/// <summary>
		/// Emits the debug artifacts for the lexer and parser
		/// </summary>
		/// <param name="unit">The unit to generate artifacts for</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool EmitDebugArtifacts(Unit unit)
		{
			reporter.Info("Exporting grammar debug data at " + GetArtifactDebugGrammar(unit) + " ...");
			Reflection.Serializers.Export(unit.Grammar, GetArtifactDebugGrammar(unit));
			if (unit.DFA != null)
			{
				reporter.Info("Exporting DFA debug data at " + GetArtifactDebugDFA(unit) + " ...");
				Reflection.Serializers.ExportDOT(unit.DFA, GetArtifactDebugDFA(unit));
			}
			if (unit.Graph != null)
			{
				reporter.Info("Exporting LR graph debug data (txt) at " + GetArtifactDebugLRAsText(unit) + " ...");
				Reflection.Serializers.Export(unit.Graph, GetArtifactDebugLRAsText(unit));
				reporter.Info("Exporting LR graph debug data (dot) at " + GetArtifactDebugLRAsDOT(unit) + " ...");
				Reflection.Serializers.ExportDOT(unit.Graph, GetArtifactDebugLRAsDOT(unit));
			}
			return true;
		}

		/// <summary>
		/// Generates the lexer for the given unit
		/// </summary>
		/// <param name="unit">The unit to generate a lexer for</param>
		/// <returns><c>true</c> if this operation succeeded</returns>
		private bool GenerateLexer(Unit unit)
		{
			// generate the lexer's data
			reporter.Info("Exporting lexer data at " + GetArtifactLexerData(unit) + " ...");
			LexerDataGenerator genData = new LexerDataGenerator(unit.DFA, unit.Expected);
			genData.Generate(GetArtifactLexerData(unit));

			// generate the lexer's code
			reporter.Info("Exporting lexer code at " + GetArtifactLexerCode(unit) + " ...");
			Generator genCode = GetLexerCodeGenerator(unit);
			genCode.Generate(GetArtifactLexerCode(unit));
			return true;
		}

		/// <summary>
		/// Generates the parser for the given grammar
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool GenerateParser(Unit unit)
		{
			// get the generator
			Generator generator = null;
			switch (unit.Method)
			{
			case ParsingMethod.LR0:
			case ParsingMethod.LR1:
			case ParsingMethod.LALR1:
				generator = new ParserLRkDataGenerator(unit);
				break;
			case ParsingMethod.RNGLR1:
			case ParsingMethod.RNGLALR1:
				generator = new ParserRNGLRDataGenerator(unit);
				break;
			}

			// generate the parser's data
			reporter.Info("Exporting parser data at " + GetArtifactParserData(unit) + " ...");
			generator.Generate(GetArtifactParserData(unit));

			// generate the parser's code
			reporter.Info("Exporting parser code at " + GetArtifactParserCode(unit) + " ...");
			generator = GetParserCodeGenerator(unit);
			generator.Generate(GetArtifactParserCode(unit));
			return true;
		}

		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="unit">The unit to generate a lexer for</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected abstract Generator GetLexerCodeGenerator(Unit unit);

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected abstract Generator GetParserCodeGenerator(Unit unit);

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected abstract bool EmitAssembly();
	}
}