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
        internal const string PostfixLexerCode = "Lexer.cs";
        internal const string PostfixLexerData = "Lexer.bin";
        internal const string PostfixParserCode = "Parser.cs";
        internal const string PostfixParserData = "Parser.bin";
        internal const string PostfixAssembly = ".dll";
        internal const string PostfixLog = "Log.mht";
        internal const string PostfixDoc = "Doc";

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
        /// Gets or sets the flag to export the compilation log.
        /// </summary>
        public bool OutputLog { get; set; }
        /// <summary>
        /// Gets ot sets the flag to export the documentation about the compiled grammar.
        /// </summary>
        public bool OutputDocumentation { get; set; }

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
		/// Repositories of the registered compiler plugins
		/// </summary>
        private Dictionary<string, CompilerPlugin> plugins;
		/// <summary>
		/// The reporter
		/// </summary>
        private Reporting.Reporter reporter;
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
            OutputLog = false;
            OutputDocumentation = false;
            CodeAccess = AccessModifier.Internal;

            nextRawID = 0;
            plugins = new Dictionary<string, CompilerPlugin>();
            plugins.Add("cf_grammar", new Grammars.ContextFree.CFPlugin());
            reporter = new Reporting.Reporter(typeof(CompilationTask), "Compilation log");
            inputs = new List<KeyValuePair<string, TextReader>>();
            loaders = new Dictionary<string, Grammars.GrammarLoader>();
        }

		/// <summary>
		/// Gets a unique identifier for a raw (anonymous) input
		/// </summary>
		/// <returns>A unique identifier</returns>
        private string GetRawInputID() { return "raw" + (nextRawID++); }

        /// <summary>
        /// Adds a new file as input
        /// </summary>
        /// <param name="file">The input file</param>
        public void AddInputFile(string file) { inputs.Add(new KeyValuePair<string, TextReader>(file, new StreamReader(file))); }
        /// <summary>
        /// Adds a new data string as input
        /// </summary>
        /// <param name="data">The data string</param>
        public void AddInputRaw(string data) { inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StringReader(data))); }
        /// <summary>
        /// Adds a new named data string as input
        /// </summary>
        /// <param name="name">The input's name</param>
        /// <param name="data">The data string</param>
        public void AddInputRaw(string name, string data) { inputs.Add(new KeyValuePair<string, TextReader>(name, new StringReader(data))); }
        /// <summary>
        /// Adds a new data stream as input
        /// </summary>
        /// <param name="stream">The input stream</param>
        public void AddInputRaw(Stream stream) { inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StreamReader(stream))); }
        /// <summary>
        /// Adds a new named data stream as input
        /// </summary>
        /// <param name="name">The input's name</param>
        /// <param name="stream">The input stream</param>
        public void AddInputRaw(string name, Stream stream) { inputs.Add(new KeyValuePair<string, TextReader>(name, new StreamReader(stream))); }
        /// <summary>
        /// Adds a new data reader as input
        /// </summary>
        /// <param name="reader">The input reader</param>
        public void AddInputRaw(TextReader reader) { inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), reader)); }
        /// <summary>
        /// Adds a new named data reader as input
        /// </summary>
        /// <param name="name">The input's name</param>
        /// <param name="reader">The input reader</param>
        public void AddInputRaw(string name, TextReader reader) { inputs.Add(new KeyValuePair<string, TextReader>(name, reader)); }

        /// <summary>
        /// Executes this compilation task
        /// </summary>
        /// <returns>The compilation report</returns>
        public Reporting.Report Execute()
        {
            string prefix = null;
            try { prefix = ExecuteDo(); }
            catch (Exception ex)
            {
                reporter.Report(ex);
                prefix = string.Empty;
            }

            if (OutputLog)
                reporter.ExportMHTML(prefix + PostfixLog);
            return reporter.Result;
        }

		/// <summary>
		/// Executes the compilation task
		/// </summary>
		/// <returns>The prefix for the outputs</returns>
        private string ExecuteDo()
        {
            reporter.Info("CentralDogma " + Version);
            foreach (string name in plugins.Keys)
                reporter.Info("Registered plugin " + plugins[name].ToString() + " for " + name);

            // Load data
            if (!LoadInputs())
                return null;
            // Solve dependencies and compile
            if (!SolveDependencies())
                return null;
            // Retrieve the grammar to compile
            Grammars.Grammar grammar = RetrieveGrammar();
            if (grammar == null)
                return null;
            // Get the lexer data
            Grammars.LexerData lexerData = grammar.GetLexerData(reporter);
            // Get the parser data
            Grammars.ParserData parserData = grammar.GetParserData(reporter, GetParserGenerator(Method));

            // If there is any error => abort now
            if (reporter.Result.HasErrors)
            	return null;

            // Build names
            string prefix = (OutputPrefix != null) ? OutputPrefix : grammar.Name;
            string nmspace = (Namespace != null) ? Namespace : grammar.Name;

            // Export lexer code
            reporter.Info("Exporting lexer code at " + prefix + PostfixLexerCode + " ...");
            StreamWriter txtOutput = OpenOutputStream(prefix + PostfixLexerCode, nmspace, true);
            lexerData.ExportCode(txtOutput, grammar.Name, CodeAccess, prefix + PostfixLexerData);
            CloseOutputStream(txtOutput);
            reporter.Info("Done!");
            
            // Export lexer data
            reporter.Info("Exporting lexer data at " + prefix + PostfixLexerData + " ...");
            BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + PostfixLexerData, FileMode.Create));
            lexerData.ExportData(binOutput);
            binOutput.Close();
            reporter.Info("Done!");
            
            // Export parser code
            reporter.Info("Exporting parser data at " + prefix + PostfixParserCode + " ...");
            txtOutput = OpenOutputStream(prefix + PostfixParserCode, nmspace, false);
            parserData.ExportCode(txtOutput, grammar.Name, CodeAccess, prefix + PostfixParserData, lexerData.Expected);
            CloseOutputStream(txtOutput);
            reporter.Info("Done!");

            // Export parser data
            reporter.Info("Exporting parser data at " + prefix + PostfixParserData + " ...");
            binOutput = new BinaryWriter(new FileStream(prefix + PostfixParserData, FileMode.Create));
            parserData.ExportData(binOutput);
            binOutput.Close();
            reporter.Info("Done!");

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

            // Export documentation
            if (OutputDocumentation)
            {
                reporter.Info("Exporting parser documentation at " + prefix + PostfixDoc);
                parserData.Document(prefix + PostfixDoc);
                reporter.Info("Done!");
            }
            return prefix;
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
        private bool LoadInput(string name, TextReader reader)
        {
            bool hasErrors = false;
            Input.FileCentralDogmaLexer lexer = new Input.FileCentralDogmaLexer(reader);
            Input.FileCentralDogmaParser parser = new Input.FileCentralDogmaParser(lexer);
            ParseResult result = null;
            try { result = parser.Parse(); }
            catch (Exception ex)
            {
                reporter.Error("Fatal error in " + name);
                reporter.Report(ex);
                hasErrors = true;
            }
            foreach (Error error in result.Errors)
            {
                reporter.Report(new Reporting.Entry(Reporting.ELevel.Error, name + " " + error.Message));
                hasErrors = true;
            }
            if (result.IsSuccess)
            {
                foreach (ASTNode gnode in result.Root.Children)
                {
                    if (!plugins.ContainsKey(gnode.Symbol.Name))
                    {
                        reporter.Error(name + " @" + gnode.Position + " No compiler plugin found for resource " + gnode.Symbol.Name);
                        hasErrors = true;
                        continue;
                    }
                    CompilerPlugin plugin = plugins[gnode.Symbol.Name];
                    Grammars.GrammarLoader loader = plugin.GetLoader(name, gnode, reporter);
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
                    if (loader.IsSolved) continue;
                    loader.Load(loaders);
                    if (loader.IsSolved) solved++;
                    else unsolved++;
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
		/// Gets the parser generator for the given parsing method
		/// </summary>
		/// <param name="method">A parsing method</param>
		/// <returns>The corresponding parser generator</returns>
        private Grammars.ParserGenerator GetParserGenerator(ParsingMethod method)
        {
            switch (method)
            {
                case ParsingMethod.LR0:
                    return new Grammars.ContextFree.LR.MethodLR0(this.reporter);
                case ParsingMethod.LR1:
                    return new Grammars.ContextFree.LR.MethodLR1(this.reporter);
                case ParsingMethod.LALR1:
                    return new Grammars.ContextFree.LR.MethodLALR1(this.reporter);
                case ParsingMethod.LRStar:
                    return new Grammars.ContextFree.LR.MethodLRStar(this.reporter);
                case ParsingMethod.RNGLR1:
                    return new Grammars.ContextFree.LR.MethodRNGLR1(this.reporter);
                case ParsingMethod.RNGLALR1:
                    return new Grammars.ContextFree.LR.MethodRNGLALR1(this.reporter);
            }
            // cannot fall here because all possibilities are exhausted in the switch above
            return null;
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
                CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { prefix + PostfixLexerCode, prefix + PostfixParserCode });
                foreach (CompilerError error in results.Errors)
                    reporter.Error(error.ToString());
            }
            reporter.Info("Done!");
        }
    }
}
