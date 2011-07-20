using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Base class for text-based LR(0) parsers
    /// </summary>
    public abstract class LR0TextParser : LR0BaseParser
    {
        protected LR0TextParser(LexerText lexer) : base(lexer) { }
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <param name="lexer">Base lexer for reading tokens</param>
        /// <param name="state">Parser's current state</param>
        /// <returns>The next token in the input</returns>
        protected override SymbolToken GetNextToken(ILexer lexer, ushort state) { return lexer.GetNextToken(); }
    }
}