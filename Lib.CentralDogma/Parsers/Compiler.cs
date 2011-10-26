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
using Hime.Kernel.Reporting;
using Hime.Kernel.Naming;
using Hime.Kernel.Resources;

namespace Hime.Parsers
{
    public sealed class Compiler
    {
        static Compiler()
        {
            ResourceLoader.RegisterPlugin(new ContextFree.CFGrammarLoader());
        }

        private Reporter reporter;

        public string Version { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public Reporter Reporter { get { return reporter; } }

        public Compiler()
        {
            reporter = new Reporter();
        }

        public Report Execute(CompilationTask task)
        {
            this.reporter.BeginSection("Compilation Task");
            try 
			{ 
				string gname = ExecuteDo(task); 
				if (task.ExportLog) reporter.ExportMHTML(GetLogName(gname) + ".mht", "Compiler Log");
			}
			catch (Exception ex) 
			{ 
				reporter.Report(ex); 
			}
            reporter.EndSection();
            
            return reporter.Result;
        }

        // TODO: this method should be private but it is used in tests, either refactor tests or this
        public string ExecuteDo(CompilationTask task)
        {
            // Load data
            Namespace root = LoadData(task.InputFiles, task.InputRawData);
            if (root == null)
                return null;

            // Retrieve the grammar to compile
            Grammar grammar = null;
            if (task.GrammarName != null)
                grammar = GetGrammar(root, task.GrammarName);
            else
                grammar = GetGrammar(root);
            if (grammar == null)
                return null;

            // Get the lexer data
            LexerData lexerData = grammar.GetLexerData(reporter);
            // Get the parser data
            ParserData parserData = grammar.GetParserData(reporter, GetParserGenerator(task.Method));

            // Build names
            string gname = grammar.LocalName;
            string lexerFile = GetLexerName(gname) + ".cs";
            string parserFile = GetParserName(gname) + ".cs";
            if (task.LexerFile != null)
                lexerFile = task.LexerFile;
            if (task.ParserFile != null)
                parserFile = task.ParserFile;
            if (lexerFile == parserFile)
                throw new ArgumentException("Output files for lexer and parser must be different");
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

        public Namespace LoadData(ICollection<string> files, ICollection<string> raws)
        {
            if (files.Count == 0 && raws.Count == 0)
            {
                reporter.Error("Compiler", "No input!");
                return null;
            }
            ResourceLoader loader = new ResourceLoader(reporter);
            List<TextReader> readers = new List<TextReader>();
            // TODO: they are both streams => could be unified!!
            foreach (string file in files)
            {
                TextReader reader = new StreamReader(file);
                readers.Add(reader);
                loader.AddInput(reader, file);
            }
            foreach (string data in raws)
            {
                TextReader reader = new StringReader(data);
                readers.Add(reader);
                loader.AddInput(reader);
            }
            bool result = loader.Load();
            Namespace root = loader.OutputRootNamespace;
            foreach (System.IO.TextReader reader in readers)
                reader.Close();
            if (!result)
                return null;
            return root;
        }

        public Grammar GetGrammar(Namespace root, string qname)
        {
            try
            {
                Symbol symbol = root.ResolveName(QualifiedName.ParseName(qname));
                if (!(symbol is Grammar))
                {
                    reporter.Error("Compiler", "Symbol " + qname + " is not a grammar");
                    return null;
                }
                return symbol as Grammar;
            }
            catch {
                reporter.Error("Compiler", "Cannot resolve grammar: " + qname);
                return null;
            }
        }

        public Grammar GetGrammar(Namespace root)
        {
            Grammar grammar = FindFirstGrammar(root);
            if (grammar == null)
                reporter.Error("Compiler", "Cannot find any grammar");
            return grammar;
        }

        private Grammar FindFirstGrammar(Symbol symbol)
        {
            if (symbol is Grammar)
                return symbol as Grammar;
            if (symbol is Namespace)
            {
                Namespace nmspace = symbol as Namespace;
                foreach (Symbol child in nmspace.Children)
                {
                    Grammar gram = FindFirstGrammar(child);
                    if (gram != null)
                        return gram;
                }
            }
            return null;
        }

        public ParserGenerator GetParserGenerator(ParsingMethod method)
        {
            switch (method)
            {
                case ParsingMethod.LR0:
                    return new Hime.Parsers.ContextFree.LR.MethodLR0();
                case ParsingMethod.LR1:
                    return new Hime.Parsers.ContextFree.LR.MethodLR1();
                case ParsingMethod.LALR1:
                    return new Hime.Parsers.ContextFree.LR.MethodLALR1();
                case ParsingMethod.LRStar:
                    return new Hime.Parsers.ContextFree.LR.MethodLRStar();
                case ParsingMethod.RNGLR1:
                    return new Hime.Parsers.ContextFree.LR.MethodRNGLR1();
                case ParsingMethod.RNGLALR1:
                    return new Hime.Parsers.ContextFree.LR.MethodRNGLALR1();
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