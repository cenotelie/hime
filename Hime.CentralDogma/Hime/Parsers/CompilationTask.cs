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
        private string grammarName;
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
        public string GrammarName
        {
            get { return grammarName; }
            set { grammarName = value; }
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
        private Kernel.Reporting.Reporter reporter;
        private ParserGenerator generator;
        private System.IO.StreamWriter lexerWriter;
        private System.IO.StreamWriter parserWriter;
        private string docFile;

        internal Kernel.Namespace Root { get { return root; } }
        internal Parsers.Grammar Grammar { get { return (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(grammarName)); } }
        internal Kernel.Reporting.Reporter Reporter { get { return reporter; } }
        internal ParserGenerator ParserGenerator { get { return generator; } }
        internal System.IO.StreamWriter LexerWriter { get { return lexerWriter; } }
        internal System.IO.StreamWriter ParserWriter { get { return parserWriter; } }
        internal string Documentation { get { return docFile; } }

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
        }


        public Report Execute()
        {
            reporter = new Reporter();

            try
            {
                if (!Execute_LoadData())
                    return Execute_Exit();

                Hime.Parsers.Grammar grammar = Execute_GetGrammar();
                if (grammar == null)
                    return Execute_Exit();

                Execute_BuildGenerator();
                if (generator == null)
                    return Execute_Exit();

                Execute_BuildData(grammar);
                Execute_OpenOutput();
                grammar.Build(this);
                Execute_Close();
            }
            catch (Exception ex)
            {
                reporter.Report(ex);   
            }

            return Execute_Exit();
        }
        
        private Hime.Kernel.Reporting.Report Execute_Exit()
        {
            if (exportLog)
            {
                string file = parserFile.Replace(".cs", "_log.mht");
                reporter.ExportMHTML(file, "Grammar Log");
            }
            return reporter.Result;
        }

        private bool Execute_LoadData()
        {
            if (fileInputs.Count == 0 && rawInputs.Count == 0)
            {
                reporter.Error("Compiler", "No input!");
                return false;
            }
            root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            foreach (string file in fileInputs)
            {
                if (!compiler.AddInputFile(file))
                    reporter.Error("Compiler", "Cannot access file: " + file);
            }
            foreach (string data in rawInputs)
                compiler.AddInputRawText(data);
            return compiler.Compile(root, reporter);
        }
        private Grammar Execute_GetGrammar()
        {
            Hime.Parsers.Grammar grammar = null;
            if (grammarName != null)
            {
                try { grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(grammarName)); }
                catch { reporter.Error("Compiler", "Cannot find grammar: " + grammarName); }
                return grammar;
            }
            grammar = Execute_FindGrammar(root);
            if (grammar != null)
                return grammar;
            reporter.Error("Compiler", "Cannot find any grammar");
            return null;
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
        private void Execute_BuildGenerator()
        {
            switch (method)
            {
                case ParsingMethod.LR0:
                    generator = new Hime.Parsers.CF.LR.MethodLR0();
                    return;
                case ParsingMethod.LR1:
                    generator = new Hime.Parsers.CF.LR.MethodLR1();
                    return;
                case ParsingMethod.LALR1:
                    generator = new Hime.Parsers.CF.LR.MethodLALR1();
                    return;
                case ParsingMethod.LRStar:
                    generator = new Hime.Parsers.CF.LR.MethodLRStar();
                    return;
                case ParsingMethod.RNGLR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLR1();
                    return;
                case ParsingMethod.RNGLALR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLALR1();
                    return;
            }
            reporter.Error("Compiler", "Unsupported parsing method: " + method.ToString());
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
            if (lexerFile == null)
            {
                lexerWriter = new System.IO.StreamWriter(parserFile, false, System.Text.Encoding.UTF8);
                parserWriter = lexerWriter;
                lexerWriter.WriteLine("using System.Collections.Generic;");
                lexerWriter.WriteLine("using Hime.Redist.Parsers;");
                lexerWriter.WriteLine("");
                lexerWriter.WriteLine("namespace " + Namespace);
                lexerWriter.WriteLine("{");
            }
            else
            {
                lexerWriter = new System.IO.StreamWriter(lexerFile, false, System.Text.Encoding.UTF8);
                lexerWriter.WriteLine("using System.Collections.Generic;");
                lexerWriter.WriteLine("using Hime.Redist.Parsers;");
                lexerWriter.WriteLine("");
                lexerWriter.WriteLine("namespace " + Namespace);
                lexerWriter.WriteLine("{");
                parserWriter = new System.IO.StreamWriter(parserFile, false, System.Text.Encoding.UTF8);
                parserWriter.WriteLine("using System.Collections.Generic;");
                parserWriter.WriteLine("using Hime.Redist.Parsers;");
                parserWriter.WriteLine("");
                parserWriter.WriteLine("namespace " + Namespace);
                parserWriter.WriteLine("{");
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
    }
}
