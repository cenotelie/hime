namespace Hime.Generators.Parsers
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
    /// Parsing Method
    /// </summary>
    public enum GrammarParseMethod
    {
        /// <summary>
        /// LR(0) Method
        /// </summary>
        LR0,
        /// <summary>
        /// LR(1) Method
        /// </summary>
        LR1,
        /// <summary>
        /// LALR(1) Method
        /// </summary>
        LALR1,
        /// <summary>
        /// Generalised LR(1) Method (Tomita)
        /// </summary>
        GLR1,
        /// <summary>
        /// Generalised LALR(1) Method (Tomita)
        /// </summary>
        GLALR1,
        /// <summary>
        /// Right nulled generalised LR(1) Method
        /// </summary>
        RNGLR1,
        /// <summary>
        /// Right nulled generalised LALR(1) Method
        /// </summary>
        RNGLALR1
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

        public abstract System.Xml.XmlNode GenerateXMLNode(System.Xml.XmlDocument Document, GrammarParseMethod MethodName, log4net.ILog Log, bool DrawVisual);
        public abstract bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log);
        public abstract bool GenerateParser(string Namespace, GrammarParseMethod MethodName, string File, log4net.ILog Log, bool DrawVisual);
        public abstract void GenerateGrammarInfo(string File, log4net.ILog Log);
    }



    public interface GrammarMethod
    {
        bool Construct(Grammar Grammar, log4net.ILog Log);
        System.Xml.XmlNode GenerateData(System.Xml.XmlDocument Doc);
        System.Drawing.Bitmap GenerateVisual();
    }

}