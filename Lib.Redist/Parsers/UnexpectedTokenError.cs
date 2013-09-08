/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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
        public IList<Symbols.Terminal> ExpectedTerminals { get; private set; }

		/// <summary>
        /// Initializes a new instance of the UnexpectedTokenError class with a token and an array of expected names
		/// </summary>
		/// <param name='token'>The unexpected token</param>
		/// <param name='expected'>The expected terminals</param>
        /// <param name="line">The line number of the token</param>
        /// <param name="column">The column number of the token</param>
        internal UnexpectedTokenError(Symbols.Token token, IList<Symbols.Terminal> expected, int line, int column)
            : base(ParserErrorType.UnexpectedToken, line, column)
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