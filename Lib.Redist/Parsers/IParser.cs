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
    /// Represents a parser
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Gets the variable symbols used by this parser
        /// </summary>
        Utils.SymbolDictionary<Symbols.Variable> Variables { get; }

        /// <summary>
        /// Gets the virtual symbols used by this parser
        /// </summary>
        Utils.SymbolDictionary<Symbols.Virtual> Virtuals { get; }

        /// <summary>
        /// Gets the errors encountered by the parser
        /// </summary>
        ICollection<ParserError> Errors { get; }

        /// <summary>
        /// Parses the input and returns the produced AST
        /// </summary>
        /// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
        AST.CSTNode Parse();

        /// <summary>
        /// Parses the input and returns whether the input is recognized
        /// </summary>
        /// <returns>True if the input is recognized, false otherwise</returns>
        bool Recognize();
    }
}
