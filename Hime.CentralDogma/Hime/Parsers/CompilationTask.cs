using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Parsers
{
    public enum ParsingMethod : byte
    {
        LR0 = 1,
        LR1 = 2,
        LALR1 = 3,
        RNGLR1 = 4,
        RNGLALR1 = 0,
        LRA = 5
    }

    public sealed class CompilationTask
    {
        private List<string> rawInputs;
        private List<string> fileInputs;
        private string grammarName;
        private string _namespace;
        private ParsingMethod method;
        private string lexerFile;
        private string parserFile;
        private bool exportLog;
        private bool exportDoc;
        private Kernel.Namespace root;
        private Kernel.Reporting.Reporter reporter;

        public ICollection<string> InputRawData { get { return rawInputs; } }
        public ICollection<string> InputFiles { get { return fileInputs; } }
        public string GrammarName { get { return grammarName; } }
        public string Namespace { get { return _namespace; } }
        public ParsingMethod Method { get { return method; } }
        public string LexerFile { get { return lexerFile; } }
        public string ParserFile { get { return parserFile; } }
        public bool ExportLog { get { return exportLog; } }
        public bool ExportDoc { get { return exportDoc; } }
        
        public Kernel.Namespace Root { get { return root; } }
        public Parsers.Grammar Grammar { get { return (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(grammarName)); } }



        public static CompilationTask Create(string data, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            CompilationTask task = new CompilationTask(grammar, method, genNamespace, lexer, parser, outLog, outDoc);
            task.rawInputs.Add(data);
            return task;
        }
        public static CompilationTask Create(string[] files, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            CompilationTask task = new CompilationTask(grammar, method, genNamespace, lexer, parser, outLog, outDoc);
            for (int i = 0; i != files.Length; i++)
                task.fileInputs.Add(files[i]);
            return task;
        }

        private CompilationTask(string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            rawInputs = new List<string>();
            fileInputs = new List<string>();
            grammarName = grammar;
            this.method = method;
            _namespace = genNamespace;
            lexerFile = lexer;
            parserFile = parser;
            exportLog = outLog;
            exportDoc = outDoc;
        }


        

        public Hime.Kernel.Reporting.Report Execute()
        {
            if (!Execute_LoadData())
                return reporter.Result;
            
            Hime.Parsers.Grammar grammar = Execute_GetGrammar();
            if (grammar == null)
                return reporter.Result;
            
            Hime.Parsers.CF.CFParserGenerator generator = Execute_GetGenerator();
            if (generator == null)
                return reporter.Result;


            GrammarBuildOptions Options = Execute_GetBuildOptions(grammar, generator);
            grammar.Build(Options);
            Options.Close();
            if (exportLog)
            {
                string file = parserFile.Replace(".cs", ".html");
                reporter.ExportHTML(file, "Grammar Log");
                System.Diagnostics.Process.Start(file);
            }
            return reporter.Result;
        }

        private bool Execute_LoadData()
        {
            reporter = new Hime.Kernel.Reporting.Reporter(typeof(CompilationTask));
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
            compiler.Compile(root, reporter);
            return true;
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
        private CF.CFParserGenerator Execute_GetGenerator()
        {
            switch (method)
            {
                case ParsingMethod.LR0:
                    return new Hime.Parsers.CF.LR.MethodLR0();
                case ParsingMethod.LR1:
                    return new Hime.Parsers.CF.LR.MethodLR1();
                case ParsingMethod.LALR1:
                    return new Hime.Parsers.CF.LR.MethodLALR1();
                case ParsingMethod.LRA:
                    return new Hime.Parsers.CF.LR.MethodLRA();
                case ParsingMethod.RNGLR1:
                    return new Hime.Parsers.CF.LR.MethodRNGLR1();
                case ParsingMethod.RNGLALR1:
                    return new Hime.Parsers.CF.LR.MethodRNGLALR1();
            }
            reporter.Error("Compiler", "Unsupported parsing method: " + method.ToString());
            return null;
        }
        private GrammarBuildOptions Execute_GetBuildOptions(Grammar grammar, CF.CFParserGenerator generator)
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
            string doc = null;
            if (exportDoc)
                doc = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(parserFile)), grammar.LocalName + "_doc");
            GrammarBuildOptions Options = null;
            if (lexerFile != null)
                Options = new GrammarBuildOptions(reporter, _namespace, generator, lexerFile, parserFile, doc);
            else
                Options = new GrammarBuildOptions(reporter, _namespace, generator, parserFile, doc);
            return Options;
        }
    }
}
