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

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a transition in a Non-deterministic Finite Automaton
	/// </summary>
	public class NFATransition
	{
		/// <summary>
		/// The value on this transition
		/// </summary>
		private readonly CharSpan span;
		/// <summary>
		/// The next state by this transition
		/// </summary>
		private readonly NFAState next;

		/// <summary>
		/// Gets the value on this transition
		/// </summary>
		public CharSpan Span { get { return span; } }
		/// <summary>
		/// Gets the next state by this transition
		/// </summary>
		public NFAState Next { get { return next; } }

		/// <summary>
		/// Initializes this transition
		/// </summary>
		/// <param name="span">The transition's value</param>
		/// <param name="next">The next state by this transition</param>
		public NFATransition(CharSpan span, NFAState next)
		{
			this.span = span;
			this.next = next;
		}
	}
}