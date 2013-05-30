using System;
using System.Text;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an unexpected character error in the input stream of a lexer
    /// </summary>
    public sealed class UnexpectedCharError : Error
    {
        /// <summary>
        /// Gets the unexpected char
        /// </summary>
        public char UnexpectedChar { get; private set; }

        /// <summary>
        /// Initializes a new instance of the UnexpectedCharError class for the given character
        /// </summary>
        /// <param name="unexpected">The errorneous character</param>
        /// <param name='position'>Error's position in the input</param>
        internal UnexpectedCharError(char unexpected, TextPosition position)
            : base(ErrorType.UnexpectedChar, position)
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