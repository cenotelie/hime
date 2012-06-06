/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
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
        private int column;
        private CSTNode subRoot;

        /// <summary>
        /// Gets the data represented by this symbol
        /// </summary>
        public override object Value { get { return value; } }
        /// <summary>
        /// Gets the text represented by this symbol
        /// </summary>
        public string ValueText { get { return value; } }
        /// <summary>
        /// Gets the line number where the text begins
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets the column number where the text begins
        /// </summary>
        public int Column { get { return column; } }
        /// <summary>
        /// Gets of sets the matched sub-grammar's root AST
        /// </summary>
        public CSTNode SubGrammarRoot
        {
            get { return subRoot; }
            set { subRoot = value; }
        }

        /// <summary>
        /// Initializes a new instance of the SymbolTokenText class with the given name, id, value and line number
        /// </summary>
        /// <param name="sid">Token's unique type identifier</param>
        /// <param name="name">Token's type name</param>
        /// <param name="value">Token's value</param>
        /// <param name="line">Token's line number</param>
        /// <param name="column">Token's column in the line</param>
        public SymbolTokenText(ushort sid, string name, string value, int line, int column)
            : base(sid, name)
        {
            this.value = value;
            this.line = line;
            this.column = column;
        }
    }
}