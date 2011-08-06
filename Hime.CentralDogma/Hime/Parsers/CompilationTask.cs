using System;
using System.Collections.Generic;
using System.Text;
using Hime.Kernel.Reporting;
using System.IO;

namespace Hime.Parsers
{
    public sealed class CompilationTask
    {
        // parameters
        private List<string> rawInputs;
        private List<string> fileInputs;
        private string _namespace;
        private bool exportDebug;
        private bool exportLog;
        private bool exportDoc;
        private bool exportVisuals;
        private string dotBinary;
        private bool multithreaded;

        public ICollection<string> InputRawData { get { return rawInputs; } }
        public ICollection<string> InputFiles { get { return fileInputs; } }
        
        // TODO: should never be null
        public string GrammarName
        {
            private get;
            set;
        }
        
        // TODO: should use default getters in the whole solution
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }

        public EParsingMethod Method
        {
            private get;
            set;
        }
        
        public string LexerFile
        {
            get;
            set;
        }
        public string ParserFile
        {
            get;
            set;
        }
        
        internal EAccessModifier GeneratedCodeModifier
        {
            get;
            private set;
        }
        
        public bool ExportDebug
        {
            get { return exportDebug; }
            set { exportDebug = value; }
        }
        public bool ExportLog
        {
            get { return exportLog; }
            set { exportLog = value; }
        }
        public bool ExportDoc
        {
            get { return exportDoc; }
            set { exportDoc = value; }
        }
        public bool ExportVisuals
        {
            get { return exportVisuals; }
            set { exportVisuals = value; }
        }
        public string DOTBinary
        {
            get { return dotBinary; }
            set { dotBinary = value; }
        }
        public bool Multithreaded
        {
            get { return multithreaded; }
            set { multithreaded = value; }
        }

        // internal data
        private Kernel.Namespace root;
        public Reporter reporter;
        private ParserGenerator generator;
        private StreamWriter lexerWriter;
        private StreamWriter parserWriter;
        private string docFile;

        internal Kernel.Reporting.Reporter Reporter { get { return reporter; } }
        internal ParserGenerator ParserGenerator { get { return generator; } }
        internal System.IO.StreamWriter LexerWriter { get { return lexerWriter; } }
        internal System.IO.StreamWriter ParserWriter { get { return parserWriter; } }
        internal string Documentation { get { return docFile; } }
        
        public Report Result { get { return this.reporter.Result; } }

        public CompilationTask()
        {
            rawInputs = new List<string>();
            fileInputs = new List<string>();
            this.GeneratedCodeModifier = EAccessModifier.Public;
            this.Method = EParsingMethod.RNGLALR1;
            exportDebug = false;
            exportLog = false;
            exportDoc = false;
            exportVisuals = false;
            multithreaded = true;
            this.reporter = new Reporter();
        }

        public Report Execute()
        {
            try
            {
                this.ExecuteBody();
            }
            catch (Exception ex)
            {
                this.reporter.Report(ex);
            }

            this.ExecuteExportLog();
            return this.Result;
        }
        
        // TODO: this method should really be private or internal, but this is just so nicer to test
        // TODO: think about it, it is a sign there is a small architectural problem there
        public void ExecuteBody()
        {
        	if (!ExecuteLoadData()) return;

            Grammar grammar = Execute_GetGrammar();
            if (grammar == null) return;

            this.InitializeGenerator();

            ExecuteBuildData(grammar);
            ExecuteOpenOutput();
            grammar.Build(this);
            Execute_Close();
        }
        
        private void ExecuteExportLog()
        {
            if (!exportLog) return;
            string file = this.ParserFile.Replace(".cs", "_log.mht");
            reporter.ExportMHTML(file, "Grammar Log");
        }

        // TODO: this method should really be private or internal, but this is just so nicer to test
        // TODO: think about it, it is a sign there is a small architectural problem there
        public bool ExecuteLoadData()
        {
            if (fileInputs.Count == 0 && rawInputs.Count == 0)
            {
                reporter.Error("Compiler", "No input!");
                return false;
            }
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler(this.reporter);
            List<System.IO.TextReader> readers = new List<System.IO.TextReader>();
            // TODO: they are both streams => could be unified!!
            foreach (string file in fileInputs)
            {
                TextReader reader = new StreamReader(file);
                readers.Add(reader);
                compiler.AddInput(reader, file);
            }
            foreach (string data in rawInputs)
            {
                TextReader reader = new StringReader(data);
                readers.Add(reader);
                compiler.AddInput(reader);
            }
            bool result = compiler.Compile();
            this.root = compiler.OutputRootNamespace;
            foreach (System.IO.TextReader reader in readers)
                reader.Close();
            return result;
        }
        
        private Grammar Execute_GetGrammar()
        {
            Grammar grammar = null;
            // TODO: think about it, what if GrammarName is null => do a test
            if (this.GrammarName != null)
            {
                try { grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(this.GrammarName)); }
                catch { reporter.Error("Compiler", "Cannot find grammar: " + this.GrammarName); }
                return grammar;
            }
            grammar = Execute_FindGrammar(root);
            if (grammar == null)
            {
	            reporter.Error("Compiler", "Cannot find any grammar");
            }
            return grammar;
        }
        
        private Grammar Execute_FindGrammar(Hime.Kernel.Symbol symbol)
        {
            if (symbol is Hime.Parsers.Grammar)
                return (Hime.Parsers.Grammar)symbol;
            if (symbol is Kernel.Namespace)
            {
                Kernel.Namespace nmspace = (Kernel.Namespace)symbol;
                foreach (Kernel.Symbol child in nmspace.Children)
                {
                    Hime.Parsers.Grammar gram = Execute_FindGrammar(child);
                    if (gram != null)
                        return gram;
                }
            }
            return null;
        }
        
        private void InitializeGenerator()
        {
            switch (this.Method)
            {
                case EParsingMethod.LR0:
                    this.generator = new Hime.Parsers.CF.LR.MethodLR0();
                    return;
                case EParsingMethod.LR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodLR1();
                    return;
                case EParsingMethod.LALR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodLALR1();
                    return;
                case EParsingMethod.LRStar:
                    this.generator = new Hime.Parsers.CF.LR.MethodLRStar();
                    return;
                case EParsingMethod.RNGLR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodRNGLR1();
                    return;
                case EParsingMethod.RNGLALR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodRNGLALR1();
                    return;
            }
            string message = "Unsupported parsing method: " + this.Method.ToString();
            reporter.Error("Compiler", message);
            throw new ArgumentException(message);
        }
        
        private void ExecuteBuildData(Grammar grammar)
        {
            if (_namespace == null)
                _namespace = grammar.CompleteName.ToString();
            // TODO: Shouldn't this be done earlier => as soon as the grammar name is determined
            if (this.LexerFile == null)
            {
            	this.LexerFile = grammar.LocalName + "Lexer.cs";
            }
            // TODO: think about it, but should never be null?? Shouldn't this be done erarlier
            if (this.ParserFile == null)
            {
            	this.ParserFile = grammar.LocalName + "Parser.cs";
            }
            
            docFile = null;
            if (exportDoc)
                docFile = this.ParserFile.Replace(".cs", "_doc.mht");
        }
        
        private StreamWriter OpenOutputStream(string fileName)
        {
        	StreamWriter result = new StreamWriter(fileName, false, Encoding.UTF8);
        	WriteHeader(result);
        	return result;
        }
        
        private void ExecuteOpenOutput()
        {            
            lexerWriter = OpenOutputStream(this.LexerFile);
            if (this.ParserFile == this.LexerFile)
            {
                this.parserWriter = this.lexerWriter;
            }
            else
            {
            	this.parserWriter = OpenOutputStream(this.ParserFile);
            }
        }

        private void Execute_Close()
        {
            lexerWriter.WriteLine("}");
            lexerWriter.Close();
            if (parserWriter != lexerWriter)
            {
                parserWriter.WriteLine("}");
                parserWriter.Close();
            }
        }

        private void WriteHeader(System.IO.StreamWriter writer)
        {
        	// TODO: maybe write a getter GetVersion for this two lines?
            String name = "Hime Parser Generator ";
            name += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            writer.WriteLine("/*");
            writer.WriteLine(" * WARNING: this file has been generated by");
            writer.WriteLine(" * " + name);
            writer.WriteLine(" */");
            writer.WriteLine();
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using Hime.Redist.Parsers;");
            writer.WriteLine();
            writer.WriteLine("namespace " + Namespace);
            writer.WriteLine("{");
        }
    }
}
