using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    public sealed class LexerTextErrorDiscardedChar : LexerTextError
    {
        private char discarded;
        private string message;

        /// <summary>
        /// Gets the character that caused the error
        /// </summary>
        public char Discarded { get { return discarded; } }
        /// <summary>
        /// Gets the error's message
        /// </summary>
        public override string Message { get { return message; } }

        /// <summary>
        /// Initializes a new instance of the LexerTextErrorDiscardedChar class for the given character at the given line and column number
        /// </summary>
        /// <param name="discarded">The errorneous character</param>
        /// <param name="line">The line number of the character</param>
        /// <param name="column">The column number of the character</param>
        public LexerTextErrorDiscardedChar(char discarded, int line, int column)
            : base(line, column)
        {
            this.discarded = discarded;
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unrecognized character '");
            Builder.Append(discarded.ToString());
            Builder.Append("' (0x");
            Builder.Append(System.Convert.ToInt32(discarded).ToString("X"));
            Builder.Append(")");
            message = Builder.ToString();
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return message; }
    }
}