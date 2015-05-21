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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents the information of a terminal matched at the state of a lexer's automaton
	/// </summary>
	public struct MatchedTerminal
	{
		/// <summary>
		/// The context
		/// </summary>
		private readonly ushort context;
		/// <summary>
		/// The terminal's index
		/// </summary>
		private readonly ushort index;

		/// <summary>
		/// Gets the context required for the terminal to be matched
		/// </summary>
		public int Context { get { return context; } }

		/// <summary>
		/// Gets the index of the matched terminal in the terminal table of the associated lexer
		/// </summary>
		public int Index { get { return index; } }

		/// <summary>
		/// Initializes this matched terminal data
		/// </summary>
		/// <param name="context">The context</param>
		/// <param name="index">The terminal's index</param>
		internal MatchedTerminal(ushort context, ushort index)
		{
			this.context = context;
			this.index = index;
		}
	}
}
