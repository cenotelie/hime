using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Hime.Redist
{
    /// <summary>
    /// Represents an unexpected token error in a parser
    /// </summary>
    public sealed class UnexpectedTokenError : Error
    {
        /// <summary>
        /// Gets the unexpected token
        /// </summary>
        public Symbols.Token UnexpectedToken { get; private set; }

        /// <summary>
        /// Gets a list of the expected terminals
        /// </summary>
        public IList<Symbols.Terminal> ExpectedTerminals { get; private set; }

		/// <summary>
        /// Initializes a new instance of the UnexpectedTokenError class with a token and an array of expected names
		/// </summary>
		/// <param name='token'>The unexpected token</param>
		/// <param name='expected'>The expected terminals</param>
        internal UnexpectedTokenError(Symbols.Token token, IList<Symbols.Terminal> expected)
            : base(ErrorType.UnexpectedToken, token.Position)
        {
            this.UnexpectedToken = token;
            this.ExpectedTerminals = new ReadOnlyCollection<Symbols.Terminal>(expected);
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