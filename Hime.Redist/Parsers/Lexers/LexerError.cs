using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an error in a lexer
    /// </summary>
    public interface LexerError
    {
        /// <summary>
        /// Gets the error's message
        /// </summary>
        string Message { get; }
    }
}