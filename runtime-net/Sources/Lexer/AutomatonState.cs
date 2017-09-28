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
	/// Represents a state in the automaton of a lexer
	/// </summary>
	/// <remarks>
	/// Binary data structure:
	/// uint16: number of matched terminals
	/// uint16: total number of transitions
	/// uint16: number of non-cached transitions
	/// -- matched terminals
	/// uint16: context identifier
	/// uint16: index of the matched terminal
	/// -- cache: 256 entries
	/// uint16: next state's index for index of the entry
	/// -- transitions
	/// each transition is of the form:
	/// uint16: start of the range
	/// uint16: end of the range
	/// uint16: next state's index
	/// </remarks>
	public struct AutomatonState
	{
		/// <summary>
		/// The automaton table
		/// </summary>
		private readonly ushort[] table;
		/// <summary>
		/// The offset of this state within the table
		/// </summary>
		private readonly int offset;

		/// <summary>
		/// Initializes this state
		/// </summary>
		/// <param name="table">The automaton table</param>
		/// <param name="offset">The offset of this state within the table</param>
		internal AutomatonState(ushort[] table, int offset)
		{
			this.table = table;
			this.offset = offset;
		}

		/// <summary>
		/// Gets the number of matched terminals in this state
		/// </summary>
		public int TerminalsCount { get { return table[offset]; } }

		/// <summary>
		/// Gets the i-th matched terminal in this state
		/// </summary>
		/// <param name="index">The index of the matched terminal</param>
		/// <returns>The matched terminal data</returns>
		public MatchedTerminal GetTerminal(int index)
		{
			return new MatchedTerminal(table[offset + index * 2 + 3], table[offset + index * 2 + 4]);
		}

		/// <summary>
		/// Gets whether this state is a dead end (no more transition)
		/// </summary>
		public bool IsDeadEnd { get { return (table[offset + 1] == 0); } }

		/// <summary>
		/// Gets the number of non-cached transitions in this state
		/// </summary>
		public int BulkTransitionsCount { get { return table[offset + 2]; } }

		/// <summary>
		/// Gets the target of a transition from this state on the specified value
		/// </summary>
		/// <param name="value">An input value</param>
		/// <returns>The target of the transition</returns>
		public int GetTargetBy(int value)
		{
			if (value <= 255)
				return GetCachedTransition(value);
			int current = offset + 3 + TerminalsCount * 2 + 256;
			for (int i = 0; i != BulkTransitionsCount; i++)
			{
				if (value >= table[current] && value <= table[current + 1])
					return table[current + 2];
				current += 3;
			}
			return Automaton.DEAD_STATE;
		}

		/// <summary>
		/// Gets the target of the cached transition for the specified value
		/// </summary>
		/// <param name="value">An input value</param>
		/// <returns>The target of the cached transition</returns>
		public int GetCachedTransition(int value)
		{
			return table[offset + TerminalsCount * 2 + 3 + value];
		}

		/// <summary>
		/// Gets the i-th non-cached transition in this state
		/// </summary>
		/// <param name="index">The non-cached transition index</param>
		/// <returns>The transition</returns>
		public AutomatonTransition GetBulkTransition(int index)
		{
			return new AutomatonTransition(table, offset + 3 + TerminalsCount * 2 + 256 + index * 3);
		}
	}
}
