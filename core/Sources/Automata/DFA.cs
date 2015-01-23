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
	/// Represents a Deterministic Finite-state Automaton
	/// </summary>
	public class DFA
	{
		/// <summary>
		/// The list of states in this automaton
		/// </summary>
		private List<DFAState> states;

		/// <summary>
		/// Gets the states in this automaton
		/// </summary>
		public ROList<DFAState> States { get { return new ROList<DFAState>(states); } }

		/// <summary>
		/// Gets the number of states in this automaton
		/// </summary>
		public int StatesCount { get { return states.Count; } }

		/// <summary>
		/// Gets the entry state of this automaton
		/// </summary>
		public DFAState Entry { get { return states[0]; } }

		/// <summary>
		/// Initializes a new (empty DFA)
		/// </summary>
		public DFA()
		{
			this.states = new List<DFAState>();
		}

		/// <summary>
		/// Initializes a DFA with a list of existing states
		/// </summary>
		/// <param name="states">Some states</param>
		private DFA(List<DFAState> states)
		{
			this.states = states;
		}

		/// <summary>
		/// Initializes this dfa as equivalent to the given nfa
		/// </summary>
		/// <param name="nfa">A nfa</param>
		public DFA(NFA nfa)
		{
			states = new List<DFAState>();

			List<NFAStateSet> nfaStateSets = new List<NFAStateSet>();
			// Create the first NFA set, add the entry and close it
			NFAStateSet nfaInit = new NFAStateSet();
			nfaStateSets.Add(nfaInit);
			nfaInit.Add(nfa.StateEntry);
			nfaInit.Close();
			// Create the initial state for the DFA
			DFAState dfaInit = new DFAState(0);
			states.Add(dfaInit);

			// For each set in the list of the NFA states
			for (int i = 0; i != nfaStateSets.Count; i++)
			{
				// Normalize transitions
				nfaStateSets[i].Normalize();

				// Get the transitions for the set
				Dictionary<CharSpan, NFAStateSet> transitions = nfaStateSets[i].GetTransitions();
				// For each transition
				foreach (CharSpan value in transitions.Keys)
				{
					// The child by the transition
					NFAStateSet next = transitions[value];
					// Search for the child in the existing sets
					bool found = false;
					for (int j = 0; j != nfaStateSets.Count; j++)
					{
						// An existing equivalent set is already present
						if (nfaStateSets[j].Equals(next))
						{
							states[i].AddTransition(value, states[j]);
							found = true;
							break;
						}
					}
					// The child is not already present
					if (!found)
					{
						// Add to the sets list
						nfaStateSets.Add(next);
						// Create the corresponding DFA state
						DFAState newState = new DFAState(states.Count);
						states.Add(newState);
						// Setup transition
						states[i].AddTransition(value, newState);
					}
				}
				// Add finals
				states[i].AddItems(nfaStateSets[i].GetFinals());
			}
		}

		/// <summary>
		/// Creates a new state in this DFA
		/// </summary>
		/// <returns>The create state</returns>
		public DFAState CreateState()
		{
			DFAState state = new DFAState(states.Count);
			states.Add(state);
			return state;
		}

		/// <summary>
		/// Gets the minimal automaton equivalent to this ine
		/// </summary>
		/// <returns>The minimal DFA</returns>
		public DFA Minimize()
		{
			DFAPartition current = new DFAPartition(this);
			DFAPartition newPartition = current.Refine();
			while (newPartition.GroupCount != current.GroupCount)
			{
				current = newPartition;
				newPartition = current.Refine();
			}
			return new DFA(newPartition.GetDFAStates());
		}

		/// <summary>
		/// Repacks the transitions of all the states in this automaton
		/// </summary>
		public void RepackTransitions()
		{
			foreach (DFAState state in states)
				state.RepackTransitions();
		}

		/// <summary>
		/// Prunes all the unreachable states from this automaton
		/// </summary>
		public void Prune()
		{
			DFAInverse inverse = new DFAInverse(this);
			List<DFAState> finals = inverse.CloseByAntecedents(inverse.Finals);
			
			// no changes
			if (finals.Count == states.Count)
				return;
			
			// Prune the transitions
			for (int i = 0; i != finals.Count; i++)
			{
				DFAState state = finals[i];
				List<CharSpan> transitions = new List<CharSpan>(state.Transitions);
				foreach (CharSpan key in transitions)
				{
					DFAState child = state.GetChildBy(key);
					if (!finals.Contains(child))
					{
						// the child should be dropped
						state.RemoveTransition(key);
					}
				}
			}

			// prune
			DFAState first = states[0];
			finals.Remove(states[0]);
			states.Clear();
			states.Add(first);
			states.AddRange(finals);
		}

		/// <summary>
		/// Extracts the sub-DFA that produces the specified final item
		/// </summary>
		/// <param name="final">A final item</param>
		/// <returns>The sub-DFA for the specified item</returns>
		public DFA ExtractSubFor(FinalItem final)
		{
			if (final == null)
				throw new System.ArgumentException("The specified item must not be null");
			List<DFAState> targets = new List<DFAState>();
			foreach (DFAState state in states)
				if (state.IsFinal && state.Items[0] == final)
					targets.Add(state);
			if (targets.Count == 0)
				throw new System.ArgumentException("The specified item is never produced by this DFA");
			return ExtractSubTo(targets);
		}

		/// <summary>
		/// Extracts the sub-DFA that leads to the specified state
		/// </summary>
		/// <param name="state">A state in this DFA</param>
		/// <returns>The sub-DFA for the specified state</returns>
		public DFA ExtractSubTo(DFAState state)
		{
			if (state == null)
				throw new System.ArgumentException("The specified state must not be null");
			if (!states.Contains(state))
				throw new System.ArgumentException("The specified state is not in this DFA");
			List<DFAState> targets = new List<DFAState>(1);
			targets.Add(state);
			return ExtractSubTo(targets);
		}

		/// <summary>
		/// Extracts the sub-DFA that leads to any of the specified states
		/// </summary>
		/// <param name="targets">States in this DFA</param>
		/// <returns>The sub-DFA for the specified states</returns>
		public DFA ExtractSubTo(IEnumerable<DFAState> targets)
		{
			if (targets == null)
				throw new System.ArgumentException("The specified collection must not be null");	

			// build a list of all the required states
			DFAInverse inverse = new DFAInverse(this);
			List<DFAState> originals = inverse.CloseByAntecedents(targets);

			// setup the list
			int index = originals.IndexOf(states[0]);
			if (index == -1)
				throw new System.ArgumentException("The specified states are not reachable from the starting state");
			originals[index] = originals[0];
			originals[0] = states[0];

			// copies the states
			List<DFAState> copy = new List<DFAState>();
			for (int i = 0; i != originals.Count; i++)
				copy.Add(new DFAState(i));
			for (int i = 0; i != originals.Count; i++)
			{
				DFAState original = originals[i];
				DFAState clone = copy[i];
				clone.AddItems(original.Items);
				foreach (CharSpan key in original.Transitions)
				{
					index = originals.IndexOf(original.GetChildBy(key));
					if (index != -1)
						clone.AddTransition(key, copy[index]);
				}
			}
			return new DFA(copy);
		}
	}
}
