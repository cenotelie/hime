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
    public sealed class CompilationTask
    {
        public string Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        
        public ICollection<string> InputRawData { get; private set; }
        public ICollection<string> InputFiles { get; private set; }
        public string GrammarName { get; set; }
        public string Namespace { get; set; }
        public ParsingMethod Method { get; set; }
        public string LexerFile { get; set; }
        public string ParserFile { get; set; }
        public bool ExportDebug { get; set; }
        public bool ExportLog { get; set; }
        public bool ExportDocumentation { get; set; }
        public bool ExportVisuals { get; set; }
        public string DOTBinary { get; set; }
        public AccessModifier GeneratedCodeModifier { get; set; }
        
        private Dictionary<string, CompilerPlugin> plugins;
        private Reporter reporter;
        private Dictionary<string, Grammar> grammars;
        private Dictionary<string, GrammarLoader> loaders;
        private SyntaxTreeNode intermediateRoot;

        public CompilationTask()
        {
            InputRawData = new List<string>();
            InputFiles = new List<string>();
            Method = ParsingMethod.RNGLALR1;
            ExportDebug = false;
            ExportLog = false;
            ExportDocumentation = false;
            ExportVisuals = false;
            GeneratedCodeModifier = AccessModifier.Public;

            plugins = new Dictionary<string, CompilerPlugin>();
            plugins.Add("cf_grammar", new ContextFree.CFPlugin());
            reporter = new Reporter();
            grammars = new Dictionary<string, Grammar>();
            loaders = new Dictionary<string, GrammarLoader>();
        }

        public Report Execute()
        {
            string gname = null;
            try { gname = ExecuteDo(); }
            catch (Exception ex) { reporter.Report(ex); }
            reporter.EndSection();

            if (ExportLog) reporter.ExportMHTML(GetLogName(gname) + ".mht", "Compiler Log");
            return reporter.Result;
        }

        private string ExecuteDo()
        {
            reporter.Info("Compiler", "CentralDogma " + Version);
            foreach (string name in plugins.Keys)
                reporter.Info("Compiler", "Registered plugin " + plugins[name].ToString() + " for " + name);

            // Load data
            if (LoadInputs())
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
                    loader.Load(loaders);
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
            if (GrammarName != null)
            {
                if (!loaders.ContainsKey(GrammarName))
                    reporter.Fatal("Compiler", "Grammar " + GrammarName + " cannot be found");
                else
                    grammar = loaders[GrammarName].Grammar;
            }
            else
            {
                if (loaders.Count != 1)
                    reporter.Fatal("Compiler", "Inputs contain more than one grammar, cannot decide which one to compile");
                else
                {
                    Dictionary<string, GrammarLoader>.Enumerator enu = loaders.GetEnumerator();
                    enu.MoveNext();
                    grammar = enu.Current.Value.Grammar;
                }
            }
            if (grammar == null)
                return null;

            // Get the lexer data
            LexerData lexerData = grammar.GetLexerData(reporter);
            // Get the parser data
            ParserData parserData = grammar.GetParserData(reporter, GetParserGenerator(Method));

            // Build names
            string gname = grammar.Name;
            string lexerFile = GetLexerName(gname) + ".cs";
            string parserFile = GetParserName(gname) + ".cs";
            if (LexerFile != null)
                lexerFile = LexerFile;
            if (ParserFile != null)
                parserFile = ParserFile;
            if (lexerFile == parserFile)
            {
                reporter.Fatal("Compiler", "Output files for lexer and parser must be different");
                return gname;
            }
            string nmspace = gname;
            if (Namespace != null)
                nmspace = Namespace;

            // Export lexer
            StreamWriter txtOutput = OpenOutputStream(lexerFile, nmspace);
            BinaryWriter binOutput = new BinaryWriter(new FileStream(gname + ".lexer", FileMode.Create));
            lexerData.ExportCode(txtOutput, GetLexerName(gname), GeneratedCodeModifier, gname);
            lexerData.ExportData(binOutput);
            CloseOutputStream(txtOutput);
            binOutput.Close();
            // Export parser
            txtOutput = OpenOutputStream(parserFile, nmspace);
            binOutput = new BinaryWriter(new FileStream(gname + ".parser", FileMode.Create));
            parserData.ExportCode(txtOutput, GetParserName(gname), GeneratedCodeModifier, GetLexerName(gname), lexerData.Expected, gname);
            parserData.ExportData(binOutput);
            CloseOutputStream(txtOutput);
            binOutput.Close();

            // Export documentation
            if (ExportDocumentation)
                parserData.Document(GetDocumentationName(gname), ExportVisuals, DOTBinary);
            return gname;
        }

        private string GetLexerName(string grammarName) { return grammarName + "Lexer"; }
        private string GetParserName(string grammarName) { return grammarName + "Parser"; }
        private string GetLogName(string grammarName) { return grammarName + "Log"; }
        private string GetDocumentationName(string grammarName) { return grammarName + "Doc"; }

        internal bool LoadInputs()
        {
            // TODO: they are both streams => could be unified!!
            intermediateRoot = new SyntaxTreeNode(null);
            foreach (string file in InputFiles)
            {
                TextReader reader = new StreamReader(file);
                if (LoadInput(file, reader))
                    return true;
            }
            foreach (string data in InputRawData)
            {
                TextReader reader = new StringReader(data);
                if (LoadInput(null, reader))
                    return true;
            }
            return false;
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
