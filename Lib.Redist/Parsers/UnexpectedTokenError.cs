/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents an unexpected token error in a parser
    /// </summary>
    public sealed class UnexpectedTokenError : ParserError
    {
        /// <summary>
        /// Gets the unexpected token
        /// </summary>
        public Symbols.Token UnexpectedToken { get; private set; }

        /// <summary>
        /// Gets a list of the expected terminals
        /// </summary>
        public List<Symbols.Terminal> ExpectedTerminals { get; private set; }

		/// <summary>
        /// Initializes a new instance of the UnexpectedTokenError class with a token and an array of expected names
		/// </summary>
		/// <param name='token'>The unexpected token</param>
		/// <param name='expected'>The array of expected terminals</param>
        /// <param name="line">The line number of the token</param>
        /// <param name="column">The column number of the token</param>
        internal UnexpectedTokenError(Symbols.Token token, List<Symbols.Terminal> expected, int line, int column)
            : base(ParserErrorType.UnexpectedToken, line, column)
        {
            this.UnexpectedToken = token;
            this.ExpectedTerminals = expected;
            StringBuilder Builder = new StringBuilder("Unexpected token \"");
            Builder.Append(token.Value.ToString());
            Builder.Append("\"; expected: { ");
            for (int i = 0; i != expected.Count; i++)
            {
                if (i != 0) Builder.Append(", ");
                Builder.Append(expected[i].Name);
            }
            Builder.Append(" }");
            this.Message += Builder.ToString();
        }
    }
}