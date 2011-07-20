using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a piece of text matched by a lexer
    /// </summary>
    public sealed class SymbolTokenText : SymbolToken
    {
        private string value;
        private int line;
        private SyntaxTreeNode subGrammarRoot;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Gets the text represented by this symbol
        /// </summary>
        public string ValueText { get { return value; } }
        /// <summary>
        /// Gets the line number where the text was found
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets or sets the root of the abstract syntax tree produced by parsing the text of this symbol
        /// </summary>
        public SyntaxTreeNode SubGrammarRoot
        {
            get { return subGrammarRoot; }
            set { subGrammarRoot = value; }
        }

        /// <summary>
        /// Initializes a new instance of the SymbolTokenText class with the given name, id, value and line number
        /// </summary>
        /// <param name="sid">The unique ID of the symbol</param>
        /// <param name="name">The name of the symbol</param>
        /// <param name="value">The text represented by the symbol</param>
        /// <param name="line">The line number where the text was found</param>
        public SymbolTokenText(ushort sid, string name, string value, int line)
            : base(sid, name)
        {
            this.value = value;
            this.line = line;
            this.subGrammarRoot = null;
        }
    }
}