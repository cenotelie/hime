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

using System.Collections.Generic;

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a group of DFA states within a partition
	/// </summary>
	public class DFAStateGroup
	{
		/// <summary>
		/// The states in this group
		/// </summary>
		private readonly List<DFAState> states;

		/// <summary>
		/// Gets the states in this group
		/// </summary>
		public ICollection<DFAState> States { get { return states; } }

		/// <summary>
		/// Gets the representative state of this group
		/// </summary>
		public DFAState Representative { get { return states[0]; } }

		/// <summary>
		/// Initializes this group with a representative state
		/// </summary>
		/// <param name="init">The representative state</param>
		public DFAStateGroup(DFAState init)
		{
			states = new List<DFAState>();
			states.Add(init);
		}

		/// <summary>
		/// Adds a state to this group
		/// </summary>
		/// <param name="state">The state to add</param>
		public void AddState(DFAState state)
		{
			states.Add(state);
		}

		/// <summary>
		/// Splits the given partition with this group
		/// </summary>
		/// <param name="current">The current partition</param>
		/// <returns>The resulting partition</returns>
		public DFAPartition Split(DFAPartition current)
		{
			DFAPartition partition = new DFAPartition();
			foreach (DFAState state in states)
				partition.AddState(state, current);
			return partition;
		}

		/// <summary>
		/// Determines whether the given state is in this group
		/// </summary>
		/// <param name="state">A state</param>
		/// <returns>True of the state is in this group</returns>
		public bool Contains(DFAState state)
		{
			return states.Contains(state);
		}
	}
}