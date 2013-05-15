namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Specifies the type of error
    /// </summary>
    public enum ParserErrorType
    {
        /// <summary>
        /// Lexical error occuring when an unexpected character is encountered in the input preventing to match tokens
        /// </summary>
        UnexpectedChar,
        /// <summary>
        /// Syntactic error occuring when an unexpected token is encountered by the parser
        /// </summary>
        UnexpectedToken
    }
}
