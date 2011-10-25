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
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    public sealed class UnexpectedCharError : ParserError
    {
        private char discarded;
        private string message;
        private int line;
        private int column;

        /// <summary>
        /// Gets the error's message
        /// </summary>
        public string Message { get { return message; } }

        /// <summary>
        /// Gets the error's line number
        /// </summary>
        public int Line { get { return line; } }

        /// <summary>
        /// Get the error's column number
        /// </summary>
        public int Column { get { return column; } }

        /// <summary>
        /// Gets the character that caused the error
        /// </summary>
        public char Discarded { get { return discarded; } }

        /// <summary>
        /// Initializes a new instance of the LexerTextErrorDiscardedChar class for the given character at the given line and column number
        /// </summary>
        /// <param name="discarded">The errorneous character</param>
        /// <param name="line">The line number of the character</param>
        /// <param name="column">The column number of the character</param>
        public UnexpectedCharError(char discarded, int line, int column)
        {
            this.discarded = discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("@(");
            Builder.Append(line);
            Builder.Append(", ");
            Builder.Append(column);
            Builder.Append(") Unexpected character '");
            Builder.Append(discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(discarded).ToString("X"));
            Builder.Append(")");
            this.message = Builder.ToString();
            this.line = line;
            this.column = column;
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return message; }
    }
}