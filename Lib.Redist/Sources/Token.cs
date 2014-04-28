/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
	/// Represents a token as an output element of a lexer
	/// </summary>
	public struct Token
	{
		private int sid;
		private int index;

		/// <summary>
		/// Gets the id of the terminal symbol associated to this token
		/// </summary>
		public int SymbolID { get { return sid; } }

		/// <summary>
		/// Gets the index of this token in a lexer's stream of token
		/// </summary>
		public int Index { get { return index; } }

		/// <summary>
		/// Initializes this token
		/// </summary>
		/// <param name="sid">The terminal's id</param>
		/// <param name="index">The token's index</param>
		public Token(int sid, int index)
		{
			this.sid = sid;
			this.index = index;
		}
	}
}
