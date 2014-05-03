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

namespace Hime.CentralDogma.Automata
{
	/// <summary>
	/// Represents a set of states in a Non-deterministic Finite Automaton
	/// A state can only appear once in a set
	/// </summary>
	class NFAStateSet
	{
		/// <summary>
		/// The backend storage for the states in this set
		/// </summary>
		private List<NFAState> backend;

		/// <summary>
		/// Initializes this set
		/// </summary>
		public NFAStateSet()
		{
			this.backend = new List<NFAState>();
		}

		/// <summary>
		/// Adds the given state in this set if it is not already present
		/// </summary>
		/// <param name="state">The state to add</param>
		public void Add(NFAState state)
		{
			if (!backend.Contains(state))
				backend.Add(state);
		}

		/// <summary>
		/// Adds the given states in this set if they are not already present
		/// </summary>
		/// <param name="states">The states to add</param>
		public void AddRange(IEnumerable<NFAState> states)
		{
			foreach (NFAState state in states)
			{
				if (!backend.Contains(state))
					backend.Add(state);
			}
		}

		/// <summary>
		/// Closes this set by transitively adding to it all reachable state by the epsilon transition
		/// </summary>
		private void Close_Normal()
		{
			for (int i = 0; i != backend.Count; i++)
				foreach (NFATransition transition in backend[i].Transitions)
					if (transition.Span.Equals(NFA.Epsilon))
						Add(transition.Next);
		}

		/// <summary>
		/// Closes this set by transitively adding to it all reachable state by the epsilon transition
		/// This looks for the watermark of states
		/// </summary>
		public void Close()
		{
			// Close the set
			Close_Normal();
			// Look for a positive and a negative node
			NFAState statePositive = null;
			NFAState stateNegative = null;
			foreach (NFAState State in backend)
			{
				if (State.Mark > 0)
					statePositive = State;
				if (State.Mark < 0)
					stateNegative = State;
			}
			// With both negative and positive states
			// remove the states immediately reached with epsilon from the positive state
			if (statePositive != null && stateNegative != null)
				foreach (NFATransition transition in statePositive.Transitions)
					if (transition.Span.Equals(NFA.Epsilon))
						backend.Remove(transition.Next);
		}

		/// <summary>
		/// Normalizes this set
		/// </summary>
		public void Normalize()
		{
			// Trace if modification has occured
			bool modification = true;
			// Repeat while modifications occured
			while (modification)
			{
				modification = false;
				// For each NFA state in the set
				for (int s1 = 0; s1 != backend.Count; s1++)
				{
					// For each transition in this NFA state Set[s1]
					for (int t1 = 0; t1 != backend[s1].Transitions.Count; t1++)
					{
						// If this is an ε transition, go to next transition
						if (backend[s1].Transitions[t1].Span.Equals(NFA.Epsilon))
							continue;
						//Confront to each transition in each NFA state of the set
						for (int s2 = 0; s2 != backend.Count; s2++)
						{
							for (int t2 = 0; t2 != backend[s2].Transitions.Count; t2++)
							{
								if (backend[s2].Transitions[t2].Span.Equals(NFA.Epsilon))
									continue;
								// If these are not the same transitions of the same state
								if ((s1 != s2) || (t1 != t2))
								{
									// If the two transition are equal : do nothing
									if (backend[s1].Transitions[t1].Span.Equals(backend[s2].Transitions[t2].Span))
										continue;
									// Get the intersection of the two spans
									CharSpan Inter = CharSpan.Intersect(backend[s1].Transitions[t1].Span, backend[s2].Transitions[t2].Span);
									// If no intersection : do nothing
									if (Inter.Length == 0)
										continue;

									// Split transition1 in 1, 2 or 3 transitions and modifiy the states accordingly
									CharSpan Part1;
									CharSpan Part2;
									Part1 = CharSpan.Split(backend[s1].Transitions[t1].Span, Inter, out Part2);
									backend[s1].ReplaceTransition(t1, new NFATransition(Inter, backend[s1].Transitions[t1].Next));
									if (Part1.Length != 0)
										backend[s1].AddTransition(Part1, backend[s1].Transitions[t1].Next);
									if (Part2.Length != 0)
										backend[s1].AddTransition(Part2, backend[s1].Transitions[t1].Next);

									// Split transition2 in 1, 2 or 3 transitions and modifiy the states accordingly
									Part1 = CharSpan.Split(backend[s2].Transitions[t2].Span, Inter, out Part2);
									backend[s2].ReplaceTransition(t2, new NFATransition(Inter, backend[s2].Transitions[t2].Next));
									if (Part1.Length != 0)
										backend[s2].AddTransition(Part1, backend[s2].Transitions[t2].Next);
									if (Part2.Length != 0)
										backend[s2].AddTransition(Part2, backend[s2].Transitions[t2].Next);
									modification = true;
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets transitions from this set to other sets
		/// </summary>
		/// <returns>The transitions from this set to other sets</returns>
		public Dictionary<CharSpan, NFAStateSet> GetTransitions()
		{
			Dictionary<CharSpan, NFAStateSet> transitions = new Dictionary<CharSpan, NFAStateSet>();
			// For each state
			foreach (NFAState State in backend)
			{
				// For each transition
				foreach (NFATransition transition in State.Transitions)
				{
					// If this is an ε-transition : pass
					if (transition.Span.Equals(NFA.Epsilon))
						continue;
					// Add the transition's target to set's transitions dictionnary
					if (transitions.ContainsKey(transition.Span))
						transitions[transition.Span].Add(transition.Next);
					else
					{
						// Create a new child
						NFAStateSet set = new NFAStateSet();
						set.Add(transition.Next);
						transitions.Add(transition.Span, set);
					}
				}
			}
			// Close all children
			foreach (NFAStateSet set in transitions.Values)
				set.Close();
			return transitions;
		}

		/// <summary>
		/// Gets all the final markers of all the states in this set
		/// </summary>
		/// <returns>The list of all the markers</returns>
		public List<FinalItem> GetFinals()
		{
			List<FinalItem> finals = new List<FinalItem>();
			foreach (NFAState state in backend)
				if (state.Item != null)
					finals.Add(state.Item);
			return finals;
		}

		/// <summary>
		/// Determines whether the given object is equal to this set
		/// </summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>True of the object is equal to this set</returns>
		public override bool Equals(object obj)
		{
			if (obj is NFAStateSet)
			{
				NFAStateSet right = (NFAStateSet)obj;
				if (this.backend.Count != right.backend.Count)
					return false;
				foreach (NFAState state in this.backend)
				{
					if (!right.backend.Contains(state))
						return false;
				}
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Get the hash-code for this set
		/// </summary>
		/// <returns>This set's hash-code</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}