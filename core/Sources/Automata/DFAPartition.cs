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
using System.Collections.Generic;

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a partition of a DFA
	/// This is used to compute minimal DFAs
	/// </summary>
	public class DFAPartition
	{
		/// <summary>
		/// The groups in this partition
		/// </summary>
		private List<DFAStateGroup> groups;

		/// <summary>
		/// Gets the number of groups in this partition
		/// </summary>
		public int GroupCount { get { return groups.Count; } }

		/// <summary>
		/// Initializes this partition
		/// </summary>
		public DFAPartition()
		{
			groups = new List<DFAStateGroup>();
		}

		/// <summary>
		/// Initializes this partition as the first partition of the given DFA
		/// The first partition is according to final, non-final states
		/// </summary>
		/// <param name="dfa">A dfa</param>
		public DFAPartition(DFA dfa)
		{
			groups = new List<DFAStateGroup>();
			// Partition the DFA states between final and non-finals
			DFAStateGroup nonFinals = null;
			// For each state in the DFA
			foreach (DFAState state in dfa.States)
			{
				// If the state is final
				if (state.Items.Count != 0)
				{
					bool added = false;
					// Look for a corresponding group in the existing ones
					foreach (DFAStateGroup group in groups)
					{
						// If the current state and the group have same finals set : group found
						if (DFAPartition_SameFinals(group.Representative, state))
						{
							// Add the set to the group
							group.AddState(state);
							added = true;
							break;
						}
					}
					// No previous group match
					// => Create a new group from the current state
					if (!added)
						groups.Add(new DFAStateGroup(state));
				}
				else // Here the state is not final
				{
					// Check for the group existence : create it if necessary
					if (nonFinals == null)
						nonFinals = new DFAStateGroup(state);
					else // add the state to the non-final group
						nonFinals.AddState(state);
				}
			}
			// If the non-final group exists : this has to be the first group
			if (nonFinals != null)
				groups.Insert(0, nonFinals);
		}

		/// <summary>
		/// Determines if the two states have the same marks
		/// </summary>
		/// <param name="left">The left state</param>
		/// <param name="right">The right state</param>
		/// <returns>True if the two states have the same marks</returns>
		private bool DFAPartition_SameFinals(DFAState left, DFAState right)
		{
			if (left.Items.Count != right.Items.Count)
				return false;
			foreach (FinalItem Final in left.Items)
			{
				if (!right.Items.Contains(Final))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Refines this partition into another one
		/// </summary>
		/// <returns>The refined partition</returns>
		public DFAPartition Refine()
		{
			DFAPartition newPartition = new DFAPartition();
			// For each group in the current partition
			// Split the group and add the resulting groups to the new partition
			foreach (DFAStateGroup group in groups)
				newPartition.groups.AddRange(group.Split(this).groups);
			return newPartition;
		}

		/// <summary>
		/// Adds a state to this partition, coming from the given old partition
		/// </summary>
		/// <param name="state">The state to add</param>
		/// <param name="old">The old partition</param>
		public void AddState(DFAState state, DFAPartition old)
		{
			bool added = false;
			// For each group in the resulting groups set
			foreach (DFAStateGroup group in groups)
			{
				// If the current state can be in this resulting group according to the old partition :
				if (AddState_SameGroup(group.Representative, state, old))
				{
					// Add the state to the group
					group.AddState(state);
					added = true;
				}
			}
			// The state cannot be in any groups : create a new group
			if (!added)
				groups.Add(new DFAStateGroup(state));
		}

		/// <summary>
		/// Determines whether two states should be in the same group
		/// </summary>
		/// <param name="s1">A state</param>
		/// <param name="s2">Another state</param>
		/// <param name="old">The old partition</param>
		/// <returns></returns>
		private static bool AddState_SameGroup(DFAState s1, DFAState s2, DFAPartition old)
		{
			if (s1.Transitions.Count != s2.Transitions.Count)
				return false;
			// For each transition from state 1
			foreach (CharSpan key in s1.Transitions)
			{
				// If state 2 does not have a transition with the same value : not same group
				if (!s2.HasTransition(key))
					return false;
				// Here State1 and State2 have both a transition of the same value
				// If the target of these transitions are in the same group in the old partition : same transition
				if (old.AddState_GetGroupOf(s1.GetChildBy(key)) != old.AddState_GetGroupOf(s2.GetChildBy(key)))
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets the group of the given state in this partition
		/// </summary>
		/// <param name="state">A state</param>
		/// <returns>The corresponding group</returns>
		private DFAStateGroup AddState_GetGroupOf(DFAState state)
		{
			foreach (DFAStateGroup group in groups)
				if (group.Contains(state))
					return group;
			return null;
		}

		/// <summary>
		/// Gets the new dfa states produced by this partition
		/// </summary>
		/// <returns>The dfa states produced by this partition</returns>
		public List<DFAState> GetDFAStates()
		{
			// For each group in the partition
			foreach (DFAStateGroup group in groups)
			{
				bool found = false;
				foreach (DFAState state in group.States)
				{
					if (state.ID == 0)
					{
						groups.Remove(group);
						groups.Insert(0, group);
						found = true;
						break;
					}
				}
				if (found)
					break;
			}

			List<DFAState> states = new List<DFAState>();
			// For each group in the partition
			// Create the coresponding state and add the finals
			foreach (DFAStateGroup group in groups)
			{
				// Create a new state
				DFAState newState = new DFAState(states.Count);
				// Add the terminal from the group to the new state
				newState.AddItems(group.Representative.Items);
				states.Add(newState);
			}
			// Do linkage
			for (int i = 0; i != groups.Count; i++)
				foreach (CharSpan Key in groups[i].Representative.Transitions)
					states[i].AddTransition(Key, states[GetDFAStates_GetGroupIndexOf(groups[i].Representative.GetChildBy(Key))]);
			return states;
		}

		private int GetDFAStates_GetGroupIndexOf(DFAState state)
		{
			for (int i = 0; i != groups.Count; i++)
				if (groups[i].Contains(state))
					return i;
			return -1;
		}
	}
}