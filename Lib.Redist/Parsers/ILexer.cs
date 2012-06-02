using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Handler for lexical errors
    /// </summary>
    /// <param name="error">The new error</param>
    public delegate void OnErrorHandler(ParserError error);

    /// <summary>
    /// Represents a lexer
    /// </summary>
    public interface ILexer
    {
        /// <summary>
        /// Gets a copy of this lexer in the same state
        /// </summary>
        /// <returns>A copy of this lexer in the same state</returns>
        ILexer Clone();

        /// <summary>
        /// Sets the error handler for this lexer
        /// </summary>
        /// <param name="handler">The error handler</param>
        void SetErrorHandler(OnErrorHandler handler);

        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        SymbolToken GetNextToken();

        /// <summary>
        /// Gets the current line number
        /// </summary>
        int CurrentLine { get; }
        /// <summary>
        /// Gets the current column number
        /// </summary>
        int CurrentColumn { get; }
    }
}
