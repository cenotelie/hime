using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    public sealed class UnexpectedCharError : ParserError
    {
        /// <summary>
        /// Gets the unexpected char
        /// </summary>
        public char UnexpectedChar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the UnexpectedCharError class for the given character at the given line and column number
        /// </summary>
        /// <param name="unexpected">The errorneous character</param>
        /// <param name="line">The line number of the character</param>
        /// <param name="column">The column number of the character</param>
        internal UnexpectedCharError(char unexpected, int line, int column)
            : base(ParserErrorType.UnexpectedChar, line, column)
        {
            this.UnexpectedChar = unexpected;
            StringBuilder Builder = new StringBuilder("Unexpected character '");
            Builder.Append(unexpected);
            Builder.Append("' (0x");
            Builder.Append(Convert.ToInt32(unexpected).ToString("X"));
            Builder.Append(")");
            this.Message += Builder.ToString();
        }
    }
}