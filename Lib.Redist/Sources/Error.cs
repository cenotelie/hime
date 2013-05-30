using System.Collections.Generic;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an error in a parser
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Gets the error's type
        /// </summary>
        public ErrorType Type { get; protected set; }

        /// <summary>
        /// Gets the error's position in the input
        /// </summary>
        public TextPosition Position { get; protected set; }

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
        /// <param name="type">Error's type</param>
		/// <param name='position'>Error's position in the input</param>
        protected Error(ErrorType type, TextPosition position)
		{
            this.Type = type;
            this.Position = position;
			this.Message = "@("+ position.Line + ", " + position.Column + ") ";
		}
    }
}