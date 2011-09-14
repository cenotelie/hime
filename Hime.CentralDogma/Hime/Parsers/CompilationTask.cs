/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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
        public ICollection<string> InputRawData { get; private set; }
        public ICollection<string> InputFiles { get; private set; }
        // TODO: should never be null
        public string GrammarName { get; set; }
        public string Namespace { get; set; }
        public EParsingMethod Method { get; set; }
        public string LexerFile { get; set; }
        public string ParserFile { get; set; }
        public bool ExportDebug { get; set; }
        public bool ExportLog { get; set; }
        public bool ExportDoc { get; set; }
        public bool ExportVisuals { get; set; }
        public string DOTBinary { get; set; }
        public bool Multithreaded { get; set; }
        internal EAccessModifier GeneratedCodeModifier { get; private set; }
        
        // internal data
        private Kernel.Naming.Namespace root;

        internal Kernel.Reporting.Reporter Reporter { get; private set; }
        internal ParserGenerator ParserGenerator { get; private set; }
        internal System.IO.StreamWriter LexerWriter { get; private set; }
        internal System.IO.StreamWriter ParserWriter { get; private set; }
        internal string Documentation { get; private set; }

        public Report Result { get { return Reporter.Result; } }

        public CompilationTask()
        {
            InputRawData = new List<string>();
            InputFiles = new List<string>();
            Method = EParsingMethod.RNGLALR1;
            ExportDebug = false;
            ExportLog = false;
            ExportDoc = false;
            ExportVisuals = false;
            Multithreaded = true;
            GeneratedCodeModifier = EAccessModifier.Public;
            Reporter = new Reporter();
        }

        public Report Execute()
        {
            try
            {
                ExecuteBody();
            }
            catch (Exception ex)
            {
                Reporter.Report(ex);
            }

            ExecuteExportLog();
            return Result;
        }
        
        // TODO: this method should really be private or internal, but this is just so nicer to test
        // TODO: think about it, it is a sign there is a small architectural problem there
        public void ExecuteBody()
        {
        	if (!ExecuteLoadData()) return;

            Grammar grammar = Execute_GetGrammar();
            if (grammar == null) return;

            InitializeGenerator();

            ExecuteBuildData(grammar);
            ExecuteOpenOutput();
            grammar.Build(this);
            Execute_Close();
        }
        
        private void ExecuteExportLog()
        {
            if (!ExportLog) return;
            string file = ParserFile.Replace(".cs", "_log.mht");
            Reporter.ExportMHTML(file, "Grammar Log");
        }

        // TODO: this method should really be private or internal, but this is just so nicer to test
        // TODO: think about it, it is a sign there is a small architectural problem there
        public bool ExecuteLoadData()
        {
            if (InputFiles.Count == 0 && InputRawData.Count == 0)
            {
                Reporter.Error("Compiler", "No input!");
                return false;
            }
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler(Reporter);
            List<System.IO.TextReader> readers = new List<System.IO.TextReader>();
            // TODO: they are both streams => could be unified!!
            foreach (string file in InputFiles)
            {
                TextReader reader = new StreamReader(file);
                readers.Add(reader);
                compiler.AddInput(reader, file);
            }
            foreach (string data in InputRawData)
            {
                TextReader reader = new StringReader(data);
                readers.Add(reader);
                compiler.AddInput(reader);
            }
            bool result = compiler.Compile();
            root = compiler.OutputRootNamespace;
            foreach (System.IO.TextReader reader in readers)
                reader.Close();
            return result;
        }
        
        private Grammar Execute_GetGrammar()
        {
            Grammar grammar = null;
            // TODO: think about it, what if GrammarName is null => do a test
            if (GrammarName != null)
            {
                try { grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.Naming.QualifiedName.ParseName(GrammarName)); }
                catch { Reporter.Error("Compiler", "Cannot find grammar: " + GrammarName); }
                return grammar;
            }
            grammar = Execute_FindGrammar(root);
            if (grammar == null)
            {
                Reporter.Error("Compiler", "Cannot find any grammar");
            }
            return grammar;
        }

        private Grammar Execute_FindGrammar(Hime.Kernel.Naming.Symbol symbol)
        {
            if (symbol is Hime.Parsers.Grammar)
                return (Hime.Parsers.Grammar)symbol;
            if (symbol is Kernel.Naming.Namespace)
            {
                Kernel.Naming.Namespace nmspace = (Kernel.Naming.Namespace)symbol;
                foreach (Kernel.Naming.Symbol child in nmspace.Children)
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
            switch (Method)
            {
                case EParsingMethod.LR0:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodLR0();
                    return;
                case EParsingMethod.LR1:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodLR1();
                    return;
                case EParsingMethod.LALR1:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodLALR1();
                    return;
                case EParsingMethod.LRStar:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodLRStar();
                    return;
                case EParsingMethod.RNGLR1:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodRNGLR1();
                    return;
                case EParsingMethod.RNGLALR1:
                    ParserGenerator = new Hime.Parsers.ContextFree.LR.MethodRNGLALR1();
                    return;
            }
            string message = "Unsupported parsing method: " + Method.ToString();
            Reporter.Error("Compiler", message);
            throw new ArgumentException(message);
        }
        
        private void ExecuteBuildData(Grammar grammar)
        {
            if (Namespace == null)
                Namespace = grammar.CompleteName.ToString();
            // TODO: Shouldn't this be done earlier => as soon as the grammar name is determined
            if (LexerFile == null)
            {
            	LexerFile = grammar.LocalName + "Lexer.cs";
            }
            // TODO: think about it, but should never be null?? Shouldn't this be done erarlier
            if (ParserFile == null)
            {
            	ParserFile = grammar.LocalName + "Parser.cs";
            }
            
            Documentation = null;
            if (ExportDoc)
                Documentation = ParserFile.Replace(".cs", "_doc.mht");
        }
        
        private StreamWriter OpenOutputStream(string fileName)
        {
        	StreamWriter result = new StreamWriter(fileName, false, Encoding.UTF8);
        	WriteHeader(result);
        	return result;
        }
        
        private void ExecuteOpenOutput()
        {            
            LexerWriter = OpenOutputStream(LexerFile);
            if (ParserFile == LexerFile)
            {
                ParserWriter = LexerWriter;
            }
            else
            {
                ParserWriter = OpenOutputStream(ParserFile);
            }
        }

        private void Execute_Close()
        {
            LexerWriter.WriteLine("}");
            LexerWriter.Close();
            if (ParserWriter != LexerWriter)
            {
                ParserWriter.WriteLine("}");
                ParserWriter.Close();
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
