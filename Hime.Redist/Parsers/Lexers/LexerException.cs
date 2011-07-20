using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an exception in a lexer
    /// </summary>
    [System.Serializable]
    public class LexerException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the LexerException class
        /// </summary>
        public LexerException() : base() { }
        /// <summary>
        /// Initializes a new instance of the LexerException class with the given message
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        public LexerException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the LexerException class with the given message and exception
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        /// <param name="innerException">The inner catched exception</param>
        public LexerException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}