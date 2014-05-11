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
using System;
using System.Text;

namespace Hime.Redist
{
	/// <summary>
	/// Represents an unexpected character error in the input stream of a lexer
	/// </summary>
	public sealed class UnexpectedCharError : ParseError
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
			: base(ParseErrorType.UnexpectedChar, position)
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