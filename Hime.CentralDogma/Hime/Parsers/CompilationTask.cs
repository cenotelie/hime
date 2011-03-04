using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Parsers
{
    public enum ParsingMethod
    {
        LR0,
        LR1,
        LALR1,
        RNGLR1,
        RNGLALR1
    }

    public sealed class CompilationTask
    {
        private List<string> p_RawInputs;
        private List<string> p_FileInputs;
        private string p_GrammarName;
        private string p_Namespace;
        private ParsingMethod p_Method;
        private string p_LexerFile;
        private string p_ParserFile;
        private bool p_ExportLog;
        private bool p_ExportDoc;
        private Kernel.Namespace p_Root;

        public IEnumerable<string> InputRawData { get { return p_RawInputs; } }
        public IEnumerable<string> InputFiles { get { return p_FileInputs; } }
        public string GrammarName { get { return p_GrammarName; } }
        public string Namespace { get { return p_Namespace; } }
        public ParsingMethod Method { get { return p_Method; } }
        public string LexerFile { get { return p_LexerFile; } }
        public string ParserFile { get { return p_ParserFile; } }
        public bool ExportLog { get { return p_ExportLog; } }
        public bool ExportDoc { get { return p_ExportDoc; } }
        
        public Kernel.Namespace Root { get { return p_Root; } }
        public Parsers.Grammar Grammar { get { return (Hime.Parsers.Grammar)p_Root.ResolveName(Hime.Kernel.QualifiedName.ParseName(p_GrammarName)); } }

        public CompilationTask(string data, string grammar, ParsingMethod method, string genNamespace, string output, bool outLog, bool outDoc)
            : this(data, grammar, method, genNamespace, null, output, outLog, outDoc) { }
        public CompilationTask(string data, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            p_RawInputs = new List<string>();
            p_FileInputs = new List<string>();
            p_RawInputs.Add(data);
            p_GrammarName = grammar;
            p_Method = method;
            p_Namespace = genNamespace;
            p_LexerFile = lexer;
            p_ParserFile = parser;
            p_ExportLog = outLog;
            p_ExportDoc = outLog;
        }
        public CompilationTask(string[] files, string grammar, ParsingMethod method, string genNamespace, string output, bool outLog, bool outDoc)
            : this(files, grammar, method, genNamespace, null, output, outLog, outDoc) { }
        public CompilationTask(string[] files, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            p_RawInputs = new List<string>();
            p_FileInputs = new List<string>();
            for (int i = 0; i != files.Length; i++)
                p_FileInputs.Add(files[i]);
            p_GrammarName = grammar;
            p_Method = method;
            p_Namespace = genNamespace;
            p_LexerFile = lexer;
            p_ParserFile = parser;
            p_ExportLog = outLog;
            p_ExportDoc = outLog;
        }

        public Hime.Kernel.Reporting.Report LoadData()
        {
            Hime.Kernel.Reporting.Reporter Reporter = new Hime.Kernel.Reporting.Reporter(typeof(CompilationTask));
            p_Root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            foreach (string file in p_FileInputs)
                compiler.AddInputFile(file);
            foreach (string data in p_RawInputs)
                compiler.AddInputRawText(data);
            compiler.Compile(p_Root, Reporter);
            return Reporter.Result;
        }

        public Hime.Kernel.Reporting.Report Execute()
        {
            Hime.Kernel.Reporting.Reporter Reporter = new Hime.Kernel.Reporting.Reporter(typeof(CompilationTask));
            p_Root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            foreach (string file in p_FileInputs)
                compiler.AddInputFile(file);
            foreach (string data in p_RawInputs)
                compiler.AddInputRawText(data);
            compiler.Compile(p_Root, Reporter);
            Hime.Parsers.Grammar grammar = (Hime.Parsers.Grammar)p_Root.ResolveName(Hime.Kernel.QualifiedName.ParseName(p_GrammarName));

            Hime.Parsers.CF.CFParserGenerator generator = null;
            switch (p_Method)
            {
                case ParsingMethod.LR0:
                    generator = new Hime.Parsers.CF.LR.MethodLR0();
                    break;
                case ParsingMethod.LR1:
                    generator = new Hime.Parsers.CF.LR.MethodLR1();
                    break;
                case ParsingMethod.LALR1:
                    generator = new Hime.Parsers.CF.LR.MethodLALR1();
                    break;
                case ParsingMethod.RNGLR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLR1();
                    break;
                case ParsingMethod.RNGLALR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLALR1();
                    break;
            }
            Hime.Parsers.GrammarBuildOptions Options = null;
            if (p_LexerFile != null)
                Options = new Hime.Parsers.GrammarBuildOptions(Reporter, p_Namespace, generator, p_LexerFile, p_ParserFile);
            else
                Options = new Hime.Parsers.GrammarBuildOptions(Reporter, p_Namespace, generator, p_ParserFile);

            grammar.Build(Options);
            Options.Close();
            if (p_ExportLog)
            {
                string file = p_ParserFile.Replace(".cs", ".html");
                Reporter.ExportHTML(file, "Grammar Log");
                System.Diagnostics.Process.Start(file);
            }
            return Reporter.Result;
        }
    }
}
