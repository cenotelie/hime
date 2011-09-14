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
    /// Represents a lexer
    /// </summary>
    public interface ILexer
    {
        /// <summary>
        /// Gets the current line number in the input
        /// </summary>
        int CurrentLine { get; }
        /// <summary>
        /// Gets a clone of this lexer
        /// </summary>
        /// <returns>A clone of this lexer</returns>
        ILexer Clone();
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken();
        /// <summary>
        /// Get the next token in the input that has is of one of the provided IDs
        /// </summary>
        /// <param name="ids">The possible IDs of the next expected token</param>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken(ushort[] ids);
    }
}