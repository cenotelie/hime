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
        RNGLALR1 = 0
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
        private Kernel.Reporting.Reporter p_Reporter;

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



        public static CompilationTask Create(string data, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            CompilationTask task = new CompilationTask(grammar, method, genNamespace, lexer, parser, outLog, outDoc);
            task.p_RawInputs.Add(data);
            return task;
        }
        public static CompilationTask Create(string[] files, string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            CompilationTask task = new CompilationTask(grammar, method, genNamespace, lexer, parser, outLog, outDoc);
            for (int i = 0; i != files.Length; i++)
                task.p_FileInputs.Add(files[i]);
            return task;
        }

        private CompilationTask(string grammar, ParsingMethod method, string genNamespace, string lexer, string parser, bool outLog, bool outDoc)
        {
            p_RawInputs = new List<string>();
            p_FileInputs = new List<string>();
            p_GrammarName = grammar;
            p_Method = method;
            p_Namespace = genNamespace;
            p_LexerFile = lexer;
            p_ParserFile = parser;
            p_ExportLog = outLog;
            p_ExportDoc = outDoc;
        }


        

        public Hime.Kernel.Reporting.Report Execute()
        {
            if (!Execute_LoadData())
                return p_Reporter.Result;
            
            Hime.Parsers.Grammar grammar = Execute_GetGrammar();
            if (grammar == null)
                return p_Reporter.Result;
            
            Hime.Parsers.CF.CFParserGenerator generator = Execute_GetGenerator();
            if (generator == null)
                return p_Reporter.Result;


            GrammarBuildOptions Options = Execute_GetBuildOptions(grammar, generator);
            grammar.Build(Options);
            Options.Close();
            if (p_ExportLog)
            {
                string file = p_ParserFile.Replace(".cs", ".html");
                p_Reporter.ExportHTML(file, "Grammar Log");
                System.Diagnostics.Process.Start(file);
            }
            return p_Reporter.Result;
        }

        private bool Execute_LoadData()
        {
            p_Reporter = new Hime.Kernel.Reporting.Reporter(typeof(CompilationTask));
            if (p_FileInputs.Count == 0 && p_RawInputs.Count == 0)
            {
                p_Reporter.Error("Compiler", "No input!");
                return false;
            }
            p_Root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            foreach (string file in p_FileInputs)
            {
                if (!compiler.AddInputFile(file))
                    p_Reporter.Error("Compiler", "Cannot access file: " + file);
            }
            foreach (string data in p_RawInputs)
                compiler.AddInputRawText(data);
            compiler.Compile(p_Root, p_Reporter);
            return true;
        }
        private Grammar Execute_GetGrammar()
        {
            Hime.Parsers.Grammar grammar = null;
            if (p_GrammarName != null)
            {
                try { grammar = (Hime.Parsers.Grammar)p_Root.ResolveName(Hime.Kernel.QualifiedName.ParseName(p_GrammarName)); }
                catch { p_Reporter.Error("Compiler", "Cannot find grammar: " + p_GrammarName); }
                return grammar;
            }
            grammar = Execute_FindGrammar(p_Root);
            if (grammar != null)
                return grammar;
            p_Reporter.Error("Compiler", "Cannot find any grammar");
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
            switch (p_Method)
            {
                case ParsingMethod.LR0:
                    return new Hime.Parsers.CF.LR.MethodLR0();
                case ParsingMethod.LR1:
                    return new Hime.Parsers.CF.LR.MethodLR1();
                case ParsingMethod.LALR1:
                    return new Hime.Parsers.CF.LR.MethodLALR1();
                case ParsingMethod.RNGLR1:
                    return new Hime.Parsers.CF.LR.MethodRNGLR1();
                case ParsingMethod.RNGLALR1:
                    return new Hime.Parsers.CF.LR.MethodRNGLALR1();
            }
            p_Reporter.Error("Compiler", "Unsupported parsing method: " + p_Method.ToString());
            return null;
        }
        private GrammarBuildOptions Execute_GetBuildOptions(Grammar grammar, CF.CFParserGenerator generator)
        {
            if (p_Namespace == null)
                p_Namespace = grammar.CompleteName.ToString();
            if (p_ParserFile == null)
            {
                if (p_FileInputs.Count == 1)
                    p_ParserFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(p_FileInputs[0]), System.IO.Path.GetFileNameWithoutExtension(p_FileInputs[0]) + ".cs");
                else
                    p_ParserFile = grammar.LocalName + ".cs";
            }
            string doc = null;
            if (p_ExportDoc)
                doc = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(p_ParserFile));
            GrammarBuildOptions Options = null;
            if (p_LexerFile != null)
                Options = new GrammarBuildOptions(p_Reporter, p_Namespace, generator, p_LexerFile, p_ParserFile, doc);
            else
                Options = new GrammarBuildOptions(p_Reporter, p_Namespace, generator, p_ParserFile, doc);
            return Options;
        }
    }
}
