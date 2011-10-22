/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected token error in a parser
    /// </summary>
    public sealed class UnexpectedTokenError : ParserError
    {
        private SymbolToken token;
        private ReadOnlyCollection<string> readOnlyExpected;
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
        /// Gets the unexpected token
        /// </summary>
        public SymbolToken UnexpectedToken { get { return token; } }
        /// <summary>
        /// Gets a collection of the expected tokens
        /// </summary>
        public ICollection<string> ExpectedTokens { get { return readOnlyExpected; } }

        /// <summary>
        /// Initializes a new instance of the ParserErrorUnexpectedToken class with a token and an array of expected names
        /// </summary>
        /// <param name="token">The unexpected token</param>
        /// <param name="expected">The array of expected tokens' names</param>
        public UnexpectedTokenError(SymbolToken token, string[] expected, int line, int column)
        {
            this.token = token;
            this.readOnlyExpected = new ReadOnlyCollection<string>(expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder();
            Builder.Append("@(");
            Builder.Append(line);
            Builder.Append(", ");
            Builder.Append(column);
            Builder.Append(") Unexpected token \"");
            Builder.Append(token.Value.ToString());
            Builder.Append("\"; expected: { ");
            for (int i = 0; i != expected.Length; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(expected[i]);
            }
            Builder.Append(" }");
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