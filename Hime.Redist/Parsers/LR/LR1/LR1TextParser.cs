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
    /// Base class for text-based LR(1) parsers
    /// </summary>
    public abstract class LR1TextParser : LR1BaseParser
    {
        protected LR1TextParser(LexerText lexer) : base(lexer) { }
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }
}