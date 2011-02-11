namespace Hime.Parsers
{
    /// <summary>
    /// Common interface for grammars
    /// </summary>
    public abstract class Grammar : Hime.Kernel.Symbol
    {
        protected Hime.Kernel.Symbol p_Parent;
        protected Hime.Kernel.QualifiedName p_CompleteName;

        public override Hime.Kernel.Symbol Parent { get { return p_Parent; } }
        public override Hime.Kernel.QualifiedName CompleteName { get { return p_CompleteName; } }

        protected override void SymbolSetParent(Hime.Kernel.Symbol Symbol){ p_Parent = Symbol; }
        protected override void SymbolSetCompleteName(Hime.Kernel.QualifiedName Name) { p_CompleteName = Name; }

        public abstract bool Build(GrammarBuildOptions Options);
    }


    public class GrammarBuildOptions
    {
        private string p_Namespace;
        private Hime.Kernel.Reporting.Reporter p_Log;
        private bool p_Drawvisual;
        private ParserGenerator p_Method;
        private System.IO.StreamWriter p_LexerWriter;
        private System.IO.StreamWriter p_ParserWriter;

        public string Namespace { get { return p_Namespace; } }
        public Hime.Kernel.Reporting.Reporter Reporter { get { return p_Log; } }
        public bool DrawVisual { get { return p_Drawvisual; } }
        public ParserGenerator ParserGenerator { get { return p_Method; } }
        public System.IO.StreamWriter LexerWriter { get { return p_LexerWriter; } }
        public System.IO.StreamWriter ParserWriter { get { return p_ParserWriter; } }

        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter Reporter, string Namespace, ParserGenerator Generator, string File)
        {
            p_Namespace = Namespace;
            p_Log = Reporter;
            p_Drawvisual = false;
            p_Method = Generator;
            p_LexerWriter = new System.IO.StreamWriter(File);
            p_ParserWriter = p_LexerWriter;
            p_LexerWriter.WriteLine("namespace " + Namespace);
            p_LexerWriter.WriteLine("{");
        }
        public GrammarBuildOptions(Hime.Kernel.Reporting.Reporter Reporter, string Namespace, ParserGenerator Generator, string FileLexer, string FileParser)
        {
            p_Namespace = Namespace;
            p_Log = Reporter;
            p_Drawvisual = false;
            p_Method = Generator;
            p_LexerWriter = new System.IO.StreamWriter(FileLexer);
            p_LexerWriter.WriteLine("namespace " + Namespace);
            p_LexerWriter.WriteLine("{"); 
            p_ParserWriter = new System.IO.StreamWriter(FileParser);
            p_ParserWriter.WriteLine("namespace " + Namespace);
            p_ParserWriter.WriteLine("{");
        }

        public void Close()
        {
            p_LexerWriter.WriteLine("}");
            p_LexerWriter.Close();
            if (p_ParserWriter != p_LexerWriter)
            {
                p_ParserWriter.WriteLine("}");
                p_ParserWriter.Close();
            }
        }
    }

    public interface ParserData
    {
        ParserGenerator Generator { get; }
        bool Export(GrammarBuildOptions Options);
    }

    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar Grammar, Hime.Kernel.Reporting.Reporter Reporter);
    }
}