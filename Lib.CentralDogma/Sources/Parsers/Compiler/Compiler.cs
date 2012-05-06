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
    public sealed class Compiler
    {
        private Dictionary<string, CompilerPlugin> plugins;
        private Reporter reporter;
        private CompilationTask task;
        private Dictionary<string, Grammar> grammars;
        private Dictionary<string, GrammarLoader> loaders;
        private SyntaxTreeNode intermediateRoot;

        public string Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public Compiler(CompilationTask task)
        {
            this.plugins = new Dictionary<string, CompilerPlugin>();
			this.reporter = new Reporter();
            this.task = task;
            this.grammars = new Dictionary<string, Grammar>();
            this.loaders = new Dictionary<string, GrammarLoader>();
        }

        public Report Execute()
        {
            reporter.BeginSection("Compilation Task");
            string gname = null;
            try { gname = ExecuteDo(); }
			catch (Exception ex) { reporter.Report(ex); }
            reporter.EndSection();
            
            if (task.ExportLog) reporter.ExportMHTML(GetLogName(gname) + ".mht", "Compiler Log");
            return reporter.Result;
        }

        // TODO: this method should be private but it is used in tests, either refactor tests or this
        internal string ExecuteDo()
        {
            reporter.Info("Loader", "CentralDogma " + Version);
            foreach (string name in plugins.Keys)
                reporter.Info("Loader", "Registered plugin " + plugins[name].ToString() + " for " + name);
            
            // Load data
            if (!LoadInputs())
                return null;

            // Solve dependencies and compile
            int unsolved = 1;
            while (unsolved != 0)
            {
                unsolved = 0;
                int solved = 0;
                foreach (GrammarLoader loader in loaders.Values)
                {
                    if (loader.IsSolved) continue;
                    loader.Resolve(loaders);
                    if (loader.IsSolved) solved++;
                    else unsolved++;
                }
                if (unsolved != 0 && solved == 0)
                {
                    reporter.Fatal("Compiler", "Unable to solve all resource depedencies");
                    break;
                }
            }

            // Retrieve the grammar to compile
            Grammar grammar = null;
            if (task.GrammarName != null)
            {
                if (!loaders.ContainsKey(task.GrammarName))
                    reporter.Fatal("Compiler", "Grammar " + task.GrammarName + " cannot be found");
                else
                    grammar = loaders[task.GrammarName].Grammar;
            }
            else
            {
                if (loaders.Count != 1)
                    reporter.Fatal("Compiler", "Inputs contain more than one grammar, cannot decide which one to compile");
                else
                    grammar = loaders.GetEnumerator().Current.Value.Grammar;
            }
            if (grammar == null)
                return null;

            // Get the lexer data
            LexerData lexerData = grammar.GetLexerData(reporter);
            // Get the parser data
            ParserData parserData = grammar.GetParserData(reporter, GetParserGenerator(task.Method));

            // Build names
            string gname = grammar.Name;
            string lexerFile = GetLexerName(gname) + ".cs";
            string parserFile = GetParserName(gname) + ".cs";
            if (task.LexerFile != null)
                lexerFile = task.LexerFile;
            if (task.ParserFile != null)
                parserFile = task.ParserFile;
            if (lexerFile == parserFile)
            {
                reporter.Fatal("Compiler", "Output files for lexer and parser must be different");
                return gname;
            }
            string nmspace = gname;
            if (task.Namespace != null)
                nmspace = task.Namespace;

            // Export lexer
            StreamWriter stream = OpenOutputStream(lexerFile, nmspace);
            lexerData.Export(stream, GetLexerName(gname), task.GeneratedCodeModifier);
            CloseOutputStream(stream);
            // Export parser
            stream = OpenOutputStream(parserFile, nmspace);
            parserData.Export(stream, GetParserName(gname), task.GeneratedCodeModifier, GetLexerName(gname), lexerData.Expected, task.ExportDebug);
            CloseOutputStream(stream);

            // Export documentation
            if (task.ExportDocumentation)
                parserData.Document(GetDocumentationName(gname) + ".mht", task.ExportVisuals, task.DOTBinary);
            return gname;
        }

        private string GetLexerName(string grammarName) { return grammarName + "Lexer"; }
        private string GetParserName(string grammarName) { return grammarName + "Parser"; }
        private string GetLogName(string grammarName) { return grammarName + "Log"; }
        private string GetDocumentationName(string grammarName) { return grammarName + "Doc"; }

        private bool LoadInputs()
        {
            // TODO: they are both streams => could be unified!!
            intermediateRoot = new SyntaxTreeNode(null);
            foreach (string file in task.InputFiles)
            {
                TextReader reader = new StreamReader(file);
                if (!LoadInput(file, reader))
                    return false;
            }
            foreach (string data in task.InputRawData)
            {
                TextReader reader = new StringReader(data);
                if (!LoadInput(null, reader))
                    return false;
            }
            return true;
        }

        private bool LoadInput(string file, TextReader reader)
        {
            bool hasErrors = false;
            if (file != null)
                reporter.Info("Compiler", "Loading compilation unit " + file);
            else
                reporter.Info("Compiler", "Loading compilation unit from raw resources");
            Input.FileCentralDogmaLexer lexer = new Input.FileCentralDogmaLexer(reader);
            Input.FileCentralDogmaParser parser = new Input.FileCentralDogmaParser(lexer);
            SyntaxTreeNode root = null;
            try { root = parser.Analyse(); }
            catch (Exception ex)
            {
                reporter.Fatal("Compiler", "Fatal error while parser the input");
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
                foreach (SyntaxTreeNode gnode in root.Children)
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
            return hasErrors;
        }

        private ParserGenerator GetParserGenerator(ParsingMethod method)
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
            string message = "Unsupported parsing method: " + method.ToString();
            reporter.Error("Compiler", message);
            throw new ArgumentException(message);
        }

        private StreamWriter OpenOutputStream(string fileName, string nmespace)
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

        private void CloseOutputStream(StreamWriter writer)
        {
            writer.WriteLine("}");
            writer.Close();
        }
    }
}