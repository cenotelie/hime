/*
 * Author: Laurent Wouters
 */

using System.Collections.Generic;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Handler for lexical errors
    /// </summary>
    /// <param name="error">The new error</param>
    public delegate void AddLexicalError(Parsers.ParserError error);

    /// <summary>
    /// Represents a lexer
    /// </summary>
    public interface ILexer : ITokenStream
    {
        /// <summary>
        /// Gets the terminals matched by this lexer
        /// </summary>
        Utils.SymbolDictionary<Symbols.Terminal> Terminals { get; }

        /// <summary>
        /// Gets the current line number
        /// </summary>
        int CurrentLine { get; }

        /// <summary>
        /// Gets the current column number
        /// </summary>
        int CurrentColumn { get; }

        /// <summary>
        /// Events for lexical errors
        /// </summary>
        event AddLexicalError OnError;
    }
}
