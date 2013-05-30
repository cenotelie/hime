namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Represents a stream of tokens that can be accessed sequentially
    /// </summary>
    public interface ITokenStream
    {
        /// <summary>
        /// Gets the next token in the input
        /// </summary>
        /// <returns>The next token in the input</returns>
        Symbols.Token GetNextToken();
    }
}
