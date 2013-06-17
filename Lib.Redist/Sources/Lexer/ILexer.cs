using System.Collections.Generic;

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Handler for lexical errors
    /// </summary>
    /// <param name="error">The new error</param>
    internal delegate void AddLexicalError(Error error);

    /// <summary>
    /// Represents a lexer
    /// </summary>
    public interface ILexer : ITokenStream
    {
        /// <summary>
        /// Gets the terminals matched by this lexer
        /// </summary>
        SymbolDictionary<Symbols.Terminal> Terminals { get; }
        
        /// <summary>
        /// Gets the text content that served as input
        /// </summary>
        TextContent Input { get; }
    }
}
