using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an error in a parser
    /// </summary>
    public interface ParserError
    {
        /// <summary>
        /// Gets the error's message
        /// </summary>
        string Message { get; }
    }
}