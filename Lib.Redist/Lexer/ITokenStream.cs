/*
 * Author: Laurent Wouters
 */

namespace Hime.Redist.Lexer
{
    public interface ITokenStream
    {
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        Symbols.Token GetNextToken();
    }
}
