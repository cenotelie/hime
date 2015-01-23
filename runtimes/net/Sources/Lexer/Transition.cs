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
	/// Represents a transition in a lexer's automaton
	/// A transition is matched by a range of UTF-16 code points
	/// Its target is a state in the automaton
	/// </summary>
	public struct Transition
	{
		/// <summary>
		/// Start of the range
		/// </summary>
		private ushort start;
		/// <summary>
		/// End of the range
		/// </summary>
		private ushort end;
		/// <summary>
		/// The transition's target
		/// </summary>
		private int target;

		/// <summary>
		/// Gets the start of the UTF-16 code point range
		/// </summary>
		public int Start { get { return start; } }

		/// <summary>
		/// Gets the end of the UTF-16 code point range
		/// </summary>
		public int End { get { return end; } }

		/// <summary>
		/// Gets the target if this transition
		/// </summary>
		public int Target { get { return target; } }

		/// <summary>
		/// Initializes this transition
		/// </summary>
		/// <param name="table">The table containing the transition</param>
		/// <param name="offset">The offset of this transition in the table</param>
		internal Transition(ushort[] table, int offset)
		{
			start = table[offset];
			end = table[offset + 1];
			target = table[offset + 2];
		}
	}
}
