using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an error in the input stream of a lexer
    /// </summary>
    public abstract class LexerTextError : LexerError
    {
        /// <summary>
        /// Error's line number
        /// </summary>
        protected int line;
        /// <summary>
        /// Error's column
        /// </summary>
        protected int column;

        /// <summary>
        /// Gets the line number of the error
        /// </summary>
        public int Line { get { return line; } }
        /// <summary>
        /// Gets the column number of the error
        /// </summary>
        public int Column { get { return column; } }

        /// <summary>
        /// Gets the error's message
        /// </summary>
        public abstract string Message { get; }

        /// <summary>
        /// Initializes a new instance of the LexerTextError class at the given line and column number
        /// </summary>
        /// <param name="line">The line number of the error</param>
        /// <param name="column">The column number of the error</param>
        protected LexerTextError(int line, int column)
        {
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public abstract override string ToString();
    }
}