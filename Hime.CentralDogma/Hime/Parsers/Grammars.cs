using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class Grammar : Hime.Kernel.Symbol
    {
        protected Hime.Kernel.Symbol parent;
        protected Hime.Kernel.QualifiedName completeName;

        public override Hime.Kernel.Symbol Parent { get { return parent; } }
        public override Hime.Kernel.QualifiedName CompleteName { get { return completeName; } }

        protected override void SymbolSetParent(Hime.Kernel.Symbol Symbol){ parent = Symbol; }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { completeName = Name; }

        public abstract bool Build(GrammarBuildOptions Options);
    }


    public sealed class GrammarBuildOptions
    {
        private Hime.Kernel.Reporting.Reporter log;
        private string _namespace;
        private ParserGenerator method;
        private System.IO.StreamWriter lexerWriter;
        private System.IO.StreamWriter parserWriter;
        private string documentation;
        private bool doVisuals;

        public Hime.Kernel.Reporting.Reporter Reporter { get { return log; } }
        public string Namespace { get { return _namespace; } }
        public ParserGenerator ParserGenerator { get { return method; } }
        public System.IO.StreamWriter LexerWriter { get { return lexerWriter; } }
        public System.IO.StreamWriter ParserWriter { get { return parserWriter; } }
        public string Documentation { get { return documentation; } }
        public bool BuildVisuals { get { return doVisuals; } }

        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter reporter, string nmspace, ParserGenerator generator, string file, string doc, bool visuals)
        {
            _namespace = nmspace;
            log = reporter;
            method = generator;
            lexerWriter = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            parserWriter = lexerWriter;
            lexerWriter.WriteLine("using System.Collections.Generic;");
            lexerWriter.WriteLine("");
            lexerWriter.WriteLine("namespace " + Namespace);
            lexerWriter.WriteLine("{");
            documentation = doc;
            doVisuals = visuals;
        }
        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter reporter, string nmspace, ParserGenerator generator, string fileLexer, string fileParser, string doc, bool visuals)
        {
            _namespace = nmspace;
            log = reporter;
            method = generator;
            lexerWriter = new System.IO.StreamWriter(fileLexer, false, System.Text.Encoding.UTF8);
            lexerWriter.WriteLine("using System.Collections.Generic;");
            lexerWriter.WriteLine("");
            lexerWriter.WriteLine("namespace " + Namespace);
            lexerWriter.WriteLine("{");
            parserWriter = new System.IO.StreamWriter(fileParser, false, System.Text.Encoding.UTF8);
            parserWriter.WriteLine("using System.Collections.Generic;");
            parserWriter.WriteLine("");
            parserWriter.WriteLine("namespace " + Namespace);
            parserWriter.WriteLine("{");
            documentation = doc;
            doVisuals = visuals;
        }

        public void Close()
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

    public interface ParserData
    {
        ParserGenerator Generator { get; }
        bool Export(GrammarBuildOptions Options);
        System.Xml.XmlNode SerializeXML(System.Xml.XmlDocument Document);
        List<string> SerializeVisuals(string directory, bool doVisualLayout);
    }

    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }
}
