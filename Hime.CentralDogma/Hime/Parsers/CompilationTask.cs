using System;
using System.Collections.Generic;
using System.Text;
using Hime.Kernel.Reporting;

namespace Hime.Parsers
{
    public enum ParsingMethod : byte
    {
        LR0 = 1,
        LR1 = 2,
        LALR1 = 3,
        RNGLR1 = 4,
        RNGLALR1 = 0,
        LRStar = 5,
        LRAutomata = 6
    }

    public sealed class CompilationTask
    {
        // parameters
        private List<string> rawInputs;
        private List<string> fileInputs;
        private string _namespace;
        private ParsingMethod method;
        private string lexerFile;
        private string parserFile;
        private bool exportDebug;
        private bool exportLog;
        private bool exportDoc;
        private bool exportVisuals;
        private string dotBinary;
        private bool multithreaded;

        public ICollection<string> InputRawData { get { return rawInputs; } }
        public ICollection<string> InputFiles { get { return fileInputs; } }
        // TODO: should never be null and should put setter as private
        public string GrammarName
        {
            private get; 
            set;
        }
        public string Namespace
        {
            get { return _namespace; }
            set { _namespace = value; }
        }
        public ParsingMethod Method
        {
            get { return method; }
            set { method = value; }
        }
        public string LexerFile
        {
            get { return lexerFile; }
            set { lexerFile = value; }
        }
        public string ParserFile
        {
            get { return parserFile; }
            set { parserFile = value; }
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
        private System.IO.StreamWriter lexerWriter;
        private System.IO.StreamWriter parserWriter;
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
            method = ParsingMethod.RNGLALR1;
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

            Execute_BuildData(grammar);
            Execute_OpenOutput();
            grammar.Build(this);
            Execute_Close();
        }
        
        private void ExecuteExportLog()
        {
            if (!exportLog) return;
            string file = parserFile.Replace(".cs", "_log.mht");
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
            foreach (string file in fileInputs)
                compiler.AddInput(new System.IO.StreamReader(file), file);
            foreach (string data in rawInputs)
                compiler.AddInput(new System.IO.StringReader(data));
            bool result = compiler.Compile();
            this.root = compiler.OutputRootNamespace;
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
            switch (method)
            {
                case ParsingMethod.LR0:
                    this.generator = new Hime.Parsers.CF.LR.MethodLR0();
                    return;
                case ParsingMethod.LR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodLR1();
                    return;
                case ParsingMethod.LALR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodLALR1();
                    return;
                case ParsingMethod.LRStar:
                    this.generator = new Hime.Parsers.CF.LR.MethodLRStar();
                    return;
                case ParsingMethod.RNGLR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodRNGLR1();
                    return;
                case ParsingMethod.RNGLALR1:
                    this.generator = new Hime.Parsers.CF.LR.MethodRNGLALR1();
                    return;
            }
            reporter.Error("Compiler", "Unsupported parsing method: " + method.ToString());
            throw new ArgumentException("Unsupported parsing method: " + method.ToString());
        }
        
        private void Execute_BuildData(Grammar grammar)
        {
            if (_namespace == null)
                _namespace = grammar.CompleteName.ToString();
            if (parserFile == null)
            {
                if (fileInputs.Count == 1)
                    parserFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fileInputs[0]), grammar.LocalName + ".cs");
                else
                    parserFile = grammar.LocalName + ".cs";
            }
            docFile = null;
            if (exportDoc)
                docFile = parserFile.Replace(".cs", "_doc.mht");
        }
        
        private void Execute_OpenOutput()
        {
            String name = "Hime Parser Generator ";
            name += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (lexerFile == null)
            {
                lexerWriter = new System.IO.StreamWriter(parserFile, false, System.Text.Encoding.UTF8);
                parserWriter = lexerWriter;
                Execute_WriteHeader(lexerWriter, name);
            }
            else
            {
                lexerWriter = new System.IO.StreamWriter(lexerFile, false, System.Text.Encoding.UTF8);
                Execute_WriteHeader(lexerWriter, name);
                parserWriter = new System.IO.StreamWriter(parserFile, false, System.Text.Encoding.UTF8);
                Execute_WriteHeader(parserWriter, name);
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

        private void Execute_WriteHeader(System.IO.StreamWriter writer, string name)
        {
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
