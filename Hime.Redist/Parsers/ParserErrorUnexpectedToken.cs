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
    /// Represents an unexpected token error in a parser
    /// </summary>
    public sealed class ParserErrorUnexpectedToken : ParserError
    {
        private SymbolToken token;
        private List<string> expected;
        private System.Collections.ObjectModel.ReadOnlyCollection<string> readOnlyExpected;
        private string message;

        /// <summary>
        /// Gets the unexpected token
        /// </summary>
        public SymbolToken UnexpectedToken { get { return token; } }
        /// <summary>
        /// Gets a collection of the expected tokens
        /// </summary>
        public ICollection<string> ExpectedTokens { get { return readOnlyExpected; } }
        /// <summary>
        /// Gets the error's message
        /// </summary>
        public string Message { get { return message; } }

        /// <summary>
        /// Initializes a new instance of the ParserErrorUnexpectedToken class with a token and an array of expected names
        /// </summary>
        /// <param name="token">The unexpected token</param>
        /// <param name="expected">The array of expected tokens' names</param>
        public ParserErrorUnexpectedToken(SymbolToken token, string[] expected)
        {
            this.token = token;
            this.expected = new List<string>(expected);
            readOnlyExpected = new System.Collections.ObjectModel.ReadOnlyCollection<string>(this.expected);
            System.Text.StringBuilder Builder = new System.Text.StringBuilder("Unexpected token ");
            if (token is SymbolTokenText)
            {
                SymbolTokenText tt = token as SymbolTokenText;
                Builder.Append("@");
                Builder.Append(tt.Line);
                Builder.Append(" ");
            }
            Builder.Append(token.Name);
            Builder.Append(": ");
            Builder.Append(token.Value.ToString());
            Builder.Append("; expected: { ");
            for (int i = 0; i != this.expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(this.expected[i]);
            }
            Builder.Append(" }.");
            message = Builder.ToString();
        }

        /// <summary>
        /// Returns the string representation of this error
        /// </summary>
        /// <returns>The string representation of this error</returns>
        public override string ToString() { return message; }
    }
}