/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hime.Utils.Reporting;
using Hime.Utils.Resources;
using Hime.Parsers.ContextFree.LR;
using Hime.Redist.Parsers;

namespace Hime.Parsers
{
    /// <summary>
    /// Represents a compilation task for the himecc
    /// </summary>
    public sealed class CompilationTask
    {
        internal const string LexerCode = "Lexer.cs";
        internal const string LexerData = "Lexer.bin";
        internal const string ParserCode = "Parser.cs";
        internal const string ParserData = "Parser.bin";
        internal const string Log = "Log.mht";
        internal const string Doc = "Doc";

        /// <summary>
        /// Gets the compiler's version
        /// </summary>
        public string Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        
        /// <summary>
        /// Gets the raw input strings
        /// </summary>
        public ICollection<string> InputRawData { get; private set; }
        /// <summary>
        /// Gets the input files
        /// </summary>
        public ICollection<string> InputFiles { get; private set; }
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
        /// </remarks>
        public string Output { get; set; }
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
        /// Gets or sets the flag to export the compilation log.
        /// </summary>
        public bool ExportLog { get; set; }
        /// <summary>
        /// Gets ot sets the flag to export the documentation about the compiled grammar.
        /// </summary>
        public bool ExportDocumentation { get; set; }
        

        private Dictionary<string, CompilerPlugin> plugins;
        private Reporter reporter;
        private Dictionary<string, Grammar> grammars;
        private Dictionary<string, GrammarLoader> loaders;
        private CSTNode intermediateRoot;

        /// <summary>
        /// Initializes a new compilation task
        /// </summary>
        public CompilationTask()
        {
            InputRawData = new List<string>();
            InputFiles = new List<string>();
            Method = ParsingMethod.RNGLALR1;
            ExportLog = false;
            ExportDocumentation = false;
            CodeAccess = AccessModifier.Internal;

            plugins = new Dictionary<string, CompilerPlugin>();
            plugins.Add("cf_grammar", new ContextFree.CFPlugin());
            reporter = new Reporter();
            grammars = new Dictionary<string, Grammar>();
            loaders = new Dictionary<string, GrammarLoader>();
        }

        public Report Execute()
        {
            string prefix = null;
            try { prefix = ExecuteDo(); }
            catch (Exception ex)
            {
                reporter.Report(ex);
                prefix = string.Empty;
            }
            reporter.EndSection();

            if (ExportLog)
                reporter.ExportMHTML(prefix + Log, "Compiler Log");
            return reporter.Result;
        }

        internal string ExecuteDo()
        {
            reporter.Info("Compiler", "CentralDogma " + Version);
            foreach (string name in plugins.Keys)
                reporter.Info("Compiler", "Registered plugin " + plugins[name].ToString() + " for " + name);

            // Load data
            if (!LoadInputs())
                return null;
            // Solve dependencies and compile
            if (!SolveDependencies())
                return null;
            // Retrieve the grammar to compile
            Grammar grammar = RetrieveGrammar();
            if (grammar == null)
                return null;
            // Get the lexer data
            LexerData lexerData = grammar.GetLexerData(reporter);
            // Get the parser data
            ParserData parserData = grammar.GetParserData(reporter, GetParserGenerator(Method));

            // Build names
            string prefix = (Output != null) ? Output : grammar.Name;
            string nmspace = (Namespace != null) ? Namespace : grammar.Name;

            // Export lexer code
            reporter.Info("Compiler", "Exporting lexer code at " + prefix + LexerCode + " ...");
            StreamWriter txtOutput = OpenOutputStream(prefix + LexerCode, nmspace);
            lexerData.ExportCode(txtOutput, grammar.Name + "Lexer", CodeAccess);
            CloseOutputStream(txtOutput);
            reporter.Info("Compiler", "Done!");
            
            // Export lexer data
            reporter.Info("Compiler", "Exporting lexer data at " + prefix + LexerData + " ...");
            BinaryWriter binOutput = new BinaryWriter(new FileStream(prefix + LexerData, FileMode.Create));
            lexerData.ExportData(binOutput);
            binOutput.Close();
            reporter.Info("Compiler", "Done!");
            
            // Export parser code
            reporter.Info("Compiler", "Exporting parser data at " + prefix + ParserCode + " ...");
            txtOutput = OpenOutputStream(prefix + ParserCode, nmspace);
            parserData.ExportCode(txtOutput, grammar.Name + "Parser", CodeAccess, grammar.Name + "Lexer", lexerData.Expected);
            CloseOutputStream(txtOutput);
            reporter.Info("Compiler", "Done!");

            // Export parser data
            reporter.Info("Compiler", "Exporting parser data at " + prefix + ParserData + " ...");
            binOutput = new BinaryWriter(new FileStream(prefix + ParserData, FileMode.Create));
            parserData.ExportData(binOutput);
            binOutput.Close();
            reporter.Info("Compiler", "Done!");
            
            // Export documentation
            if (ExportDocumentation)
            {
                reporter.Info("Compiler", "Exporting parser documentation at " + prefix + Doc);
                parserData.Document(prefix + Doc);
                reporter.Info("Compiler", "Done!");
            }
            return prefix;
        }

        internal bool LoadInputs()
        {
            intermediateRoot = new CSTNode(null);
            foreach (string file in InputFiles)
            {
                TextReader reader = new StreamReader(file);
                if (!LoadInput(file, reader))
                    return false;
            }
            foreach (string data in InputRawData)
            {
                TextReader reader = new StringReader(data);
                if (!LoadInput(null, reader))
                    return false;
            }
            return true;
        }

        internal bool LoadInput(string file, TextReader reader)
        {
            bool hasErrors = false;
            if (file != null)
                reporter.Info("Compiler", "Loading compilation unit " + file);
            else
                reporter.Info("Compiler", "Loading compilation unit from raw resources");
            Input.FileCentralDogmaLexer lexer = new Input.FileCentralDogmaLexer(reader);
            Input.FileCentralDogmaParser parser = new Input.FileCentralDogmaParser(lexer);
            CSTNode root = null;
            try { root = parser.Parse(); }
            catch (Exception ex)
            {
                reporter.Fatal("Compiler", "Fatal error while parsing the input");
                reporter.Report(ex);
                hasErrors = true;
            }
            foreach (ParserError error in parser.Errors)
            {
                reporter.Report(new Entry(ELevel.Error, "Parser", error.Message));
                hasErrors = true;
            }
            if (root != null)
            {
                foreach (CSTNode gnode in root.Children)
                {
                    if (!plugins.ContainsKey(gnode.Symbol.Name))
                    {
                        reporter.Fatal("Compiler", "No compiler plugin found for resource " + gnode.Symbol.Name);
                        hasErrors = true;
                        continue;
                    }
                    CompilerPlugin plugin = plugins[gnode.Symbol.Name];
                    GrammarLoader loader = plugin.GetLoader(gnode, reporter);
                    loaders.Add(loader.Name, loader);
                }
            }
            return !hasErrors;
        }

        internal bool SolveDependencies()
        {
            int unsolved = 1;
            while (unsolved != 0)
            {
                unsolved = 0;
                int solved = 0;
                foreach (GrammarLoader loader in loaders.Values)
                {
                    if (loader.IsSolved) continue;
                    loader.Load(loaders);
                    if (loader.IsSolved) solved++;
                    else unsolved++;
                }
                if (unsolved != 0 && solved == 0)
                {
                    reporter.Fatal("Compiler", "Unable to solve all resource depedencies");
                    return false;
                }
            }
            return true;
        }

        internal Grammar RetrieveGrammar()
        {
            if (GrammarName != null)
            {
                if (!loaders.ContainsKey(GrammarName))
                    reporter.Fatal("Compiler", "Grammar " + GrammarName + " cannot be found");
                else
                    return loaders[GrammarName].Grammar;
            }
            else
            {
                if (loaders.Count != 1)
                    reporter.Fatal("Compiler", "Inputs contain more than one grammar, cannot decide which one to compile");
                else
                {
                    Dictionary<string, GrammarLoader>.Enumerator enu = loaders.GetEnumerator();
                    enu.MoveNext();
                    return enu.Current.Value.Grammar;
                }
            }
            return null;
        }

        internal ParserGenerator GetParserGenerator(ParsingMethod method)
        {
            switch (method)
            {
                case ParsingMethod.LR0:
                    return new MethodLR0(this.reporter);
                case ParsingMethod.LR1:
                    return new MethodLR1(this.reporter);
                case ParsingMethod.LALR1:
                    return new MethodLALR1(this.reporter);
                case ParsingMethod.LRStar:
                    return new MethodLRStar(this.reporter);
                case ParsingMethod.RNGLR1:
                    return new MethodRNGLR1(this.reporter);
                case ParsingMethod.RNGLALR1:
                    return new MethodRNGLALR1(this.reporter);
            }
            // cannot fall here because all possibilities are exhausted in the switch above
            return null;
        }

        internal StreamWriter OpenOutputStream(string fileName, string nmespace)
        {
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8);
            writer.WriteLine("/*");
            writer.WriteLine(" * WARNING: this file has been generated by");
            writer.WriteLine(" * Hime Parser Generator " + Version);
            writer.WriteLine(" */");
            writer.WriteLine();
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Hime.Redist.Parsers;");
            writer.WriteLine();
            writer.WriteLine("namespace " + nmespace);
            writer.WriteLine("{");
            return writer;
        }

        internal void CloseOutputStream(StreamWriter writer)
        {
            writer.WriteLine("}");
            writer.Close();
        }
    }
}
