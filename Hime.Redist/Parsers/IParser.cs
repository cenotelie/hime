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
        /// Runs the parser and return the root of the abstract syntax tree
        /// </summary>
        /// <returns>The root of the abstract syntax tree representing the input, or null if errors when encountered</returns>
        SyntaxTreeNode Analyse();
    }
}
