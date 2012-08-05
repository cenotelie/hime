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
    /// Represents an error in a parser
    /// </summary>
    public class ParserError
    {
        /// <summary>
        /// Gets the error's type
        /// </summary>
        public ParserErrorType Type { get; protected set; }

        /// <summary>
        /// Gets the error's line in the input
        /// </summary>
        public int Line { get; protected set; }

        /// <summary>
        /// Gets the error's column in the input
        /// </summary>
        public int Column { get; protected set; }

        /// <summary>
        /// Gets the error's message
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return this.Message; }
		
		/// <summary>
        /// Initializes a new instance of the ParserError
		/// </summary>
		/// <param name='line'>Error's line number in the input</param>
		/// <param name='column'>Error's column in the input</param>
		protected ParserError(ParserErrorType type, int line, int column)
		{
            this.Type = type;
            this.Line = line;
            this.Column = column;
			this.Message = "@("+ line + ", " + column + ") ";
		}
    }
}