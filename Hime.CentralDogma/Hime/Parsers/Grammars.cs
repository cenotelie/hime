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
        private string _namespace;
        private Hime.Kernel.Reporting.Reporter log;
        private bool drawvisual;
        private ParserGenerator method;
        private System.IO.StreamWriter lexerWriter;
        private System.IO.StreamWriter parserWriter;
        private string documentationDir;

        public string Namespace { get { return _namespace; } }
        public Hime.Kernel.Reporting.Reporter Reporter { get { return log; } }
        public bool DrawVisual { get { return drawvisual; } }
        public ParserGenerator ParserGenerator { get { return method; } }
        public System.IO.StreamWriter LexerWriter { get { return lexerWriter; } }
        public System.IO.StreamWriter ParserWriter { get { return parserWriter; } }
        public string DocumentationDir { get { return documentationDir; } }

        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter Reporter, string Namespace, ParserGenerator Generator, string File, string DocDir)
        {
            _namespace = Namespace;
            log = Reporter;
            drawvisual = false;
            method = Generator;
            lexerWriter = new System.IO.StreamWriter(File, false, System.Text.Encoding.UTF8);
            parserWriter = lexerWriter;
            lexerWriter.WriteLine("using System.Collections.Generic;");
            lexerWriter.WriteLine("");
            lexerWriter.WriteLine("namespace " + Namespace);
            lexerWriter.WriteLine("{");
            documentationDir = DocDir;
        }
        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter Reporter, string Namespace, ParserGenerator Generator, string FileLexer, string FileParser, string DocDir)
        {
            _namespace = Namespace;
            log = Reporter;
            drawvisual = false;
            method = Generator;
            lexerWriter = new System.IO.StreamWriter(FileLexer, false, System.Text.Encoding.UTF8);
            lexerWriter.WriteLine("using System.Collections.Generic;");
            lexerWriter.WriteLine("");
            lexerWriter.WriteLine("namespace " + Namespace);
            lexerWriter.WriteLine("{");
            parserWriter = new System.IO.StreamWriter(FileParser, false, System.Text.Encoding.UTF8);
            parserWriter.WriteLine("using System.Collections.Generic;");
            parserWriter.WriteLine("");
            parserWriter.WriteLine("namespace " + Namespace);
            parserWriter.WriteLine("{");
            documentationDir = DocDir;
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
        void SerializeVisual(Kernel.Graphs.DOTSerializer Serializer);
    }

    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }
}
