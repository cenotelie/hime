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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents a match in the input
	/// </summary>
	struct TokenMatch
	{
		/// <summary>
		/// The matching DFA state
		/// </summary>
		public readonly int state;
		/// <summary>
		/// Length of the matched input
		/// </summary>
		public readonly int length;

		/// <summary>
		/// Gets whether this is match indicates a success
		/// </summary>
		public bool IsSuccess { get { return state != Automaton.DEAD_STATE; } }

		/// <summary>
		/// Initializes a failing match
		/// </summary>
		/// <param name='length'>The number of characters to advance in the input</param>
		public TokenMatch(int  length)
		{
			state = Automaton.DEAD_STATE;
			this.length = length;
		}

		/// <summary>
		/// Initializes a match
		/// </summary>
		/// <param name='state'>The matching DFA state</param>
		/// <param name='length'>Length of the matched input</param>
		public TokenMatch(int state, int length)
		{
			this.state = state;
			this.length = length;
		}
	}
}