using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a parser
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Gets the errors encountered by the parser
        /// </summary>
        ICollection<ParserError> Errors { get; }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        ParseTree Parse();
    }
}
