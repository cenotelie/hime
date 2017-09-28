/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

namespace Hime.Redist
{
	/// <summary>
	/// Represents a position in term of line and column in a text input
	/// </summary>
	public struct TextPosition
	{
		/// <summary>
		/// The line number
		/// </summary>
		private readonly int line;
		/// <summary>
		/// The column number
		/// </summary>
		private readonly int column;

		/// <summary>
		/// Gets the line number
		/// </summary>
		public int Line { get { return line; } }

		/// <summary>
		/// Gets the column number
		/// </summary>
		public int Column { get { return column; } }

		/// <summary>
		/// Initializes this position with the given line and column numbers
		/// </summary>
		/// <param name="line">The line number</param>
		/// <param name="column">The column number</param>
		public TextPosition(int line, int column)
		{
			this.line = line;
			this.column = column;
		}

		/// <summary>
		/// Gets a string representation of this position
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "(" + line + ", " + column + ")";
		}
	}
}
