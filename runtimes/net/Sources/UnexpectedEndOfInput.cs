/**********************************************************************
* Copyright (c) 2015 Laurent Wouters and others
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

namespace Hime.Redist
{
	/// <summary>
	/// Represents the unexpected of the input text while more characters were expected
	/// </summary>
	public class UnexpectedEndOfInput : ParseError
	{
		/// <summary>
		/// Gets the error's type
		/// </summary>
		public override ParseErrorType Type { get { return ParseErrorType.UnexpectedEndOfInput; } }

		/// <summary>
		/// Gets the error's length in the input (in number of characters)
		/// </summary>
		public override int Length { get { return 0; } }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public override string Message { get { return "Unexpected end of input"; } }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="position">Error's position in the input</param>
		public UnexpectedEndOfInput(TextPosition position) : base(position)
		{
		}
	}
}