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
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Hime.Redist;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a compilation task for the himecc
	/// </summary>
	public sealed class CompilationTask
	{
		public const string PostfixLexerCode = "Lexer.cs";
		public const string PostfixLexerData = "Lexer.bin";
		public const string PostfixParserCode = "Parser.cs";
		public const string PostfixParserData = "Parser.bin";
		public const string PostfixAssembly = ".dll";

		/// <summary>
		/// Gets the compiler's version
		/// </summary>
		public static string Version { get { return typeof(CompilationTask).Assembly.GetName().Version.ToString(); } }

		/// <summary>
		/// Gets ot sets the compiler's mode
		/// </summary>
		public CompilationMode Mode { get; set; }

		/// <summary>
		/// Gets or sets the name of the grammar to compile in the case where several grammars are loaded.
		/// If this property is not set, the first grammar to be found will be compiled.
		/// </summary>
		public string GrammarName { get; set; }

		/// <summary>
		/// Gets or sets the parsing method to use
		/// </summary>
		public ParsingMethod Method { get; set; }

		/// <summary>
		/// Gets ot sets the compiler's output files' prefix.
		/// If this property is not set, the name of the compiled grammar will be used as a prefix and the files output into the current directory
		/// </summary>
		/// <remarks>
		/// The compiler will generate the following files:
		/// Lexer code file:    ${prefix}Lexer.cs
		/// Lexer data file:    ${prefix}Lexer.bin
		/// Parser code file:   ${prefix}Parser.cs
		/// Parser data file:   ${prefix}Parser.bin
		/// Assembly:           ${prefix}.dll
		/// </remarks>
		public string OutputPrefix { get; set; }

		/// <summary>
		/// Gets or sets the namespace in which the generated Lexer and Parser classes will be put.
		/// If this property is not set, the namespace will be the name of the grammar.
		/// </summary>
		public string Namespace { get; set; }
		/// <summary>
		/// Gets or sets the access modifiers for the generated Lexer and Parser classes.
		/// The default value is Internal.
		/// </summary>
		public AccessModifier CodeAccess { get; set; }

		/// <summary>
		/// Next unique identifier for raw (anonymous) inputs
		/// </summary>
		private int nextRawID;
		/// <summary>
		/// The reporter
		/// </summary>
		private Reporter reporter;
		/// <summary>
		/// Repositories of inputs
		/// </summary>
		private List<KeyValuePair<string, TextReader>> inputs;
		/// <summary>
		/// Repositories of loaders
		/// </summary>
		private Dictionary<string, Grammars.GrammarLoader> loaders;

		/// <summary>
		/// Initializes a new compilation task
		/// </summary>
		public CompilationTask()
		{
			Mode = CompilationMode.Source;
			Method = ParsingMethod.LALR1;
			CodeAccess = AccessModifier.Internal;

			nextRawID = 0;
			reporter = new Reporter(typeof(CompilationTask));
			inputs = new List<KeyValuePair<string, TextReader>>();
			loaders = new Dictionary<string, Grammars.GrammarLoader>();
		}

		/// <summary>
		/// Gets a unique identifier for a raw (anonymous) input
		/// </summary>
		/// <returns>A unique identifier</returns>
		private string GetRawInputID()
		{
			return "raw" + (nextRawID++);
		}

		/// <summary>
		/// Adds a new file as input
		/// </summary>
		/// <param name="file">The input file</param>
		public void AddInputFile(string file)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(file, new StreamReader(file)));
		}
		/// <summary>
		/// Adds a new data string as input
		/// </summary>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string data)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StringReader(data)));
		}
		/// <summary>
		/// Adds a new named data string as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string name, string data)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, new StringReader(data)));
		}
		/// <summary>
		/// Adds a new data stream as input
		/// </summary>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(Stream stream)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StreamReader(stream)));
		}
		/// <summary>
		/// Adds a new named data stream as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(string name, Stream stream)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, new StreamReader(stream)));
		}
		/// <summary>
		/// Adds a new data reader as input
		/// </summary>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(TextReader reader)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), reader));
		}
		/// <summary>
		/// Adds a new named data reader as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(string name, TextReader reader)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, reader));
		}

		/// <summary>
		/// Executes this compilation task
		/// </summary>
		/// <returns>The compilation report</returns>
		public Report Execute()
		{
			try
			{
				ExecuteDo();
			}
			catch (Exception ex)
			{
				reporter.Error(ex);
			}
			return reporter.Result;
		}

		/// <summary>
		/// Executes the compilation task
		/// </summary>
		private void ExecuteDo()
		{
			reporter.Info("CentralDogma " + Version);

			// Load data
			if (!LoadInputs())
				return;
			// Solve dependencies and compile
			if (!SolveDependencies())
				return;
			// Retrieve the grammar to compile
			Grammars.Grammar grammar = RetrieveGrammar();
			if (grammar == null)
				return;
			string message = grammar.Prepare();
			if (message != null)
			{
				reporter.Error(message);
				return;
			}
			// Build names
			string prefix = (OutputPrefix != null) ? OutputPrefix : grammar.Name;
			string nmspace = (Namespace != null) ? Namespace : grammar.Name;
			// Export the lexer
			List<Grammars.Terminal> expected = GenerateLexer(grammar, prefix, nmspace);
			if (expected == null)
				return;
			// Export the paser
			if (!GenerateParser(grammar, prefix, nmspace, expected))
				return;
			// Build assembly
			if (Mode != CompilationMode.Source)
				BuildAssembly(prefix);
			// Cleanup
			if (Mode == CompilationMode.Assembly)
			{
				File.Delete(prefix + PostfixLexerCode);
				File.Delete(prefix + PostfixLexerData);
				File.Delete(prefix + PostfixParserCode);
				File.Delete(prefix + PostfixParserData);
			}
		}

		/// <summary>
		/// Parses the inputs
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool LoadInputs()
		{
			foreach (KeyValuePair<string, TextReader> pair in inputs)
				if (!LoadInput(pair.Key, pair.Value))
					return false;
			return true;
		}

		/// <summary>
		/// Parses the input with the given identifier
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input's reader</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool LoadInput(string name, TextReader reader)
		{
			bool hasErrors = false;
			Input.HimeGrammarLexer lexer = new Input.HimeGrammarLexer(reader);
			Input.HimeGrammarParser parser = new Input.HimeGrammarParser(lexer);
			ParseResult result = null;
			try
			{
				result = parser.Parse();
			}
			catch (Exception ex)
			{
				reporter.Error("Fatal error in " + name);
				reporter.Error(ex);
				hasErrors = true;
			}
			foreach (Error error in result.Errors)
			{
				reporter.Error(error);
				hasErrors = true;
			}
			if (result.IsSuccess)
			{
				foreach (ASTNode gnode in result.Root.Children)
				{
					Grammars.GrammarLoader loader = new Grammars.GrammarLoader(name, gnode, reporter);
					loaders.Add(loader.Grammar.Name, loader);
				}
			}
			reader.Close();
			return !hasErrors;
		}

		/// <summary>
		/// Solves the dependencies between the inputs and interprets the parsed inputs
		/// </summary>
		/// <returns><c>true</c> if all dependencies were solved</returns>
		private bool SolveDependencies()
		{
			int unsolved = 1;
			while (unsolved != 0)
			{
				unsolved = 0;
				int solved = 0;
				foreach (Grammars.GrammarLoader loader in loaders.Values)
				{
					if (loader.IsSolved)
						continue;
					loader.Load(loaders);
					if (loader.IsSolved)
						solved++;
					else
						unsolved++;
				}
				if (unsolved != 0 && solved == 0)
				{
					reporter.Error("Unable to solve all resource depedencies");
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Retrieves the top grammar to compile
		/// </summary>
		/// <returns>The top grammar to compile, or <c>null</c> if none could be found</returns>
		private Grammars.Grammar RetrieveGrammar()
		{
			if (GrammarName != null)
			{
				if (!loaders.ContainsKey(GrammarName))
					reporter.Error("Grammar " + GrammarName + " cannot be found");
				else
					return loaders[GrammarName].Grammar;
			}
			else
			{
				if (loaders.Count > 1)
					reporter.Error("Inputs contain more than one grammar, cannot decide which one to compile");
				else if (loaders.Count == 0)
					reporter.Error("No grammar in inputs");
				else
				{
					Dictionary<string, Grammars.GrammarLoader>.Enumerator enu = loaders.GetEnumerator();
					enu.MoveNext();
					return enu.Current.Value.Grammar;
				}
			}
			return null;
		}

		/// <summary>
		/// Generates the lexer for the given grammar
		/// </summary>
		/// <param name="grammar">The grammar to generate a lexer for</param>
		/// <param name="prefix">The prefix for the output artifacts</param>
		/// <param name="nmspace">The namespace for the generated code</param>
		/// <returns>The terminals matched by the lexer</returns>
		private List<Grammars.Terminal> GenerateLexer(Grammars.Grammar grammar, string prefix, string nmspace)
		{
			Automata.DFA dfa = GetDFAFor(grammar);
			if (dfa == null)
				return null;
			// retrieve the separator
			string name = grammar.GetOption("Separator");
			Grammars.Terminal separator = name != null ? grammar.GetTerminalByName(name) : null;
			// get the generator
			Output.LexerGenerator generator = new Hime.CentralDogma.Output.LexerGenerator(dfa, separator);
			// generate the lexer's code
			reporter.Info("Exporting lexer code at " + prefix + PostfixLexerCode + " ...");
			StreamWriter txtOutput = OpenOutputStream(prefix + PostfixLexerCode, nmspace, true);
			generator.GenerateCode(txtOutput, grammar.Name, CodeAccess, prefix + PostfixLexerData);
			CloseOutputStream(txtOutput);
			// generate the lexer's data
			reporter.Info("Exporting lexer data at " + prefix + PostfixLexerData + " ...");
			BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + PostfixLexerData, FileMode.Create));
			generator.GenerateData(binOutput);
			binOutput.Close();
			return generator.Expected;
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
			string name = grammar.GetOption("Separator");
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
		/// <param name="grammar">The grammar to generate a parser for</param>
		/// <param name="prefix">The prefix for the output artifacts</param>
		/// <param name="nmspace">The namespace for the generated code</param>
		/// <param name="expected">The terminals matched by the associated lexer</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool GenerateParser(Grammars.Grammar grammar, string prefix, string nmspace, List<Grammars.Terminal> expected)
		{
			// build the LR graph
			Grammars.LR.Builder builder = new Grammars.LR.Builder(grammar);
			Grammars.LR.Graph graph = builder.Build(Method);
			foreach (Grammars.LR.Conflict conflict in builder.Conflicts)
				reporter.Error(conflict);
			if (builder.Conflicts.Count != 0)
				return false;
			// get the generator
			Output.ParserGenerator generator = null;
			switch (Method)
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
			reporter.Info("Exporting parser code at " + prefix + PostfixParserCode + " ...");
			StreamWriter txtOutput = OpenOutputStream(prefix + PostfixParserCode, nmspace, false);
			generator.GenerateCode(txtOutput, grammar.Name, CodeAccess, prefix + PostfixParserData);
			CloseOutputStream(txtOutput);
			// generate the parser's data
			reporter.Info("Exporting parser data at " + prefix + PostfixParserData + " ...");
			BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + PostfixParserData, FileMode.Create));
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
			StreamWriter writer = new StreamWriter(fileName, false, new UTF8Encoding(false));
			writer.WriteLine("/*");
			writer.WriteLine(" * WARNING: this file has been generated by");
			writer.WriteLine(" * Hime Parser Generator " + Version);
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
		/// Builds the assembly for the generated lexer and parser
		/// </summary>
		/// <param name="prefix">The prefix for the generated assembly</param>
		private void BuildAssembly(string prefix)
		{
			reporter.Info("Building assembly " + prefix + PostfixAssembly + " ...");
			string redist = Assembly.GetAssembly(typeof(ParseResult)).Location;
			using (CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#"))
			{
				CompilerParameters compilerparams = new CompilerParameters();
				compilerparams.GenerateExecutable = false;
				compilerparams.GenerateInMemory = false;
				compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
				compilerparams.ReferencedAssemblies.Add("System.dll");
				compilerparams.ReferencedAssemblies.Add(redist);
				compilerparams.EmbeddedResources.Add(prefix + PostfixLexerData);
				compilerparams.EmbeddedResources.Add(prefix + PostfixParserData);
				compilerparams.OutputAssembly = prefix + PostfixAssembly;
				CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] {
					prefix + PostfixLexerCode,
					prefix + PostfixParserCode
				});
				foreach (CompilerError error in results.Errors)
					reporter.Error(error.ToString());
			}
		}
	}
}
