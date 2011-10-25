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
    /// Represents an exception in a parser
    /// </summary>
    public class ParserException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the ParserException class
        /// </summary>
        public ParserException() : base() { }
        /// <summary>
        /// Initializes a new instance of the ParserException class with the given message
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        public ParserException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the ParserException class with the given message and exception
        /// </summary>
        /// <param name="message">The message conveyed by this exception</param>
        /// <param name="innerException">The inner catched exception</param>
        public ParserException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}