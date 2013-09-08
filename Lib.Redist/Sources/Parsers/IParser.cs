using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a parser
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Gets or sets whether the paser should try to recover from errors
        /// </summary>
        bool RecoverErrors { get; set; }

        /// <summary>
        /// Gets the errors encountered by the parser
        /// </summary>
        ICollection<Error> Errors { get; }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        ParseTree Parse();
    }
}
