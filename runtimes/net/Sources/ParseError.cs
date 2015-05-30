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

namespace Hime.Redist
{
	/// <summary>
	/// Represents an error in a parser
	/// </summary>
	public abstract class ParseError
	{
		/// <summary>
		/// The error's position in the input text
		/// </summary>
		private readonly TextPosition position;

		/// <summary>
		/// Gets the error's type
		/// </summary>
		public abstract ParseErrorType Type { get; }

		/// <summary>
		/// Gets the error's position in the input
		/// </summary>
		public TextPosition Position { get { return position; } }

		/// <summary>
		/// Gets the error's length in the input (in number of characters)
		/// </summary>
		public abstract int Length { get; }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public abstract string Message { get; }

		/// <summary>
		/// Initializes this error
		/// </summary>
		/// <param name="position">Error's position in the input</param>
		protected ParseError(TextPosition position)
		{
			this.position = position;
		}

		/// <summary>
		/// Returns the string representation of this error
		/// </summary>
		/// <returns>The string representation of this error</returns>
		public override string ToString()
		{
			return "@" + position + " " + Message;
		}
	}
}