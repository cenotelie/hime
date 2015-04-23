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
using Hime.Redist.Utils;

namespace Hime.Redist
{
	/// <summary>
	/// Represents an unexpected token error in a parser
	/// </summary>
	public sealed class UnexpectedTokenError : ParseError
	{
		/// <summary>
		/// The unexpected symbol
		/// </summary>
		private readonly Token unexpected;
		/// <summary>
		/// The expected terminals
		/// </summary>
		private readonly ROList<Symbol> expected;

		/// <summary>
		/// Gets the error's type
		/// </summary>
		public override ParseErrorType Type { get { return ParseErrorType.UnexpectedToken; } }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public override string Message { get { return BuildMessage(); } }

		/// <summary>
		/// Gets the unexpected token
		/// </summary>
		public Token UnexpectedToken { get { return unexpected; } }

		/// <summary>
		/// Gets the expected terminals
		/// </summary>
		public ROList<Symbol> ExpectedTerminals { get { return expected; } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="token">The unexpected token</param>
		/// <param name="expected">The expected terminals</param>
		public UnexpectedTokenError(Token token, ROList<Symbol> expected)
			: base(token.Position)
		{
			this.unexpected = token;
			this.expected = expected;
		}

		/// <summary>
		/// Builds the message for this error
		/// </summary>
		/// <returns>The message for this error</returns>
		private string BuildMessage()
		{
			StringBuilder builder = new StringBuilder("Unexpected token \"");
			builder.Append(unexpected.Value);
			builder.Append("\"");
			if (expected.Count > 0)
			{
				builder.Append("; expected: ");
				for (int i = 0; i != expected.Count; i++)
				{
					if (i != 0)
						builder.Append(", ");
					builder.Append(expected[i].Name);
				}
			}
			return builder.ToString();
		}
	}
}