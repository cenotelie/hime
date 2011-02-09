namespace Hime.Parsers
{
    /// <summary>
    /// Grammar status
    /// </summary>
    public enum GrammarStatus
    {
        /// <summary>
        /// Grammar loaded
        /// </summary>
        Loaded,
        /// <summary>
        /// Grammar loaded and prepared
        /// </summary>
        Prepared,
        /// <summary>
        /// Grammar prepared and data generated
        /// </summary>
        DataGenerated,
        /// <summary>
        /// Grammar prepared and parser code generated
        /// </summary>
        CodeGenerated
    }


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

        public abstract Automata.DFA FinalDFA { get; }
        public abstract string GetOption(string Name);
        public abstract Terminal GetTerminal(string Name);

        public abstract System.Xml.XmlNode GenerateXMLNode(System.Xml.XmlDocument Document, ParseMethod Method, Hime.Kernel.Reporting.Reporter Log, bool DrawVisual);
        public abstract bool GenerateParser(string Namespace, ParseMethod Method, string File, Hime.Kernel.Reporting.Reporter Log);
        public abstract bool GenerateParser(string Namespace, ParseMethod Method, string File, Hime.Kernel.Reporting.Reporter Log, bool DrawVisual);
        public abstract void GenerateGrammarInfo(string File, Hime.Kernel.Reporting.Reporter Log);
    }


    public interface ParseMethod
    {
        string Name { get; }
        bool Construct(Grammar Grammar, Hime.Kernel.Reporting.Reporter Log);
        System.Xml.XmlNode GenerateData(System.Xml.XmlDocument Doc);
        System.Drawing.Bitmap GenerateVisual();
    }
}