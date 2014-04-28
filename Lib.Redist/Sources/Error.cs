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
	public class Error
	{
		/// <summary>
		/// Gets the error's type
		/// </summary>
		public ErrorType Type { get; protected set; }

		/// <summary>
		/// Gets the error's position in the input
		/// </summary>
		public TextPosition Position { get; protected set; }

		/// <summary>
		/// Gets the error's message
		/// </summary>
		public string Message { get; protected set; }

		/// <summary>
		/// Returns the string representation of this error
		/// </summary>
		/// <returns>The string representation of this error</returns>
		public override string ToString()
		{
			return this.Message;
		}

		/// <summary>
		/// Initializes a new instance of the ParserError
		/// </summary>
		/// <param name="type">Error's type</param>
		/// <param name='position'>Error's position in the input</param>
		protected Error(ErrorType type, TextPosition position)
		{
			this.Type = type;
			this.Position = position;
			this.Message = "@(" + position.Line + ", " + position.Column + ") ";
		}
	}
}