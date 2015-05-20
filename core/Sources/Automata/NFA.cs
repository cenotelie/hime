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
using Hime.Redist.Utils;

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a Non-deterministic Finite Automaton
	/// </summary>
	public class NFA
	{
		/// <summary>
		/// The list of all the states in this automaton
		/// </summary>
		private readonly List<NFAState> states;

		/// <summary>
		/// Represents the value epsilon on NFA transtions
		/// </summary>
		public static readonly CharSpan EPSILON = CharSpan.NULL;

		/// <summary>
		/// Gets the collection of states in this automaton
		/// </summary>
		public ROList<NFAState> States { get { return new ROList<NFAState>(states); } }

		/// <summary>
		/// Gets the number of states in this automaton
		/// </summary>
		public int StatesCount { get { return states.Count; } }

		/// <summary>
		/// Gets or sets the entry set for this automaton
		/// </summary>
		public NFAState StateEntry
		{
			get;
			set;
		}

		/// <summary>
		/// Gets ot sets the exit state for this automaton
		/// </summary>
		public NFAState StateExit
		{
			get;
			set;
		}

		/// <summary>
		/// Initializes an empty automaton (no state)
		/// </summary>
		private NFA()
		{
			states = new List<NFAState>();
		}

		/// <summary>
		/// Initializes this automaton as a copy of the given DFA
		/// This automaton will not have an exit state
		/// </summary>
		/// <param name="dfa">The DFA to copy</param>
		public NFA(DFA dfa)
		{
			states = new List<NFAState>();
			List<DFAState> dfaStates = new List<DFAState>(dfa.States);
			for (int i = 0; i != dfaStates.Count; i++)
				states.Add(new NFAState());
			for (int i = 0; i != dfaStates.Count; i++)
			{
				states[i].AddItems(dfaStates[i].Items);
				foreach (CharSpan transition in dfaStates[i].Transitions)
					states[i].AddTransition(transition, states[dfaStates.IndexOf(dfaStates[i].GetChildBy(transition))]);
			}
			StateEntry = states[0];
		}

		/// <summary>
		/// Adds a new state to this automaton
		/// </summary>
		/// <returns>The new state</returns>
		public NFAState AddNewState()
		{
			NFAState state = new NFAState();
			states.Add(state);
			return state;
		}

		/// <summary>
		/// Clones this automaton
		/// </summary>
		/// <returns>The cloned automaton</returns>
		public NFA Clone()
		{
			return Clone(true);
		}

		/// <summary>
		/// Clones this automaton
		/// </summary>
		/// <param name="keepFinals">Whether to keep the marks for the final states</param>
		/// <returns>The cloned automaton</returns>
		public NFA Clone(bool keepFinals)
		{
			NFA copy = new NFA();

			// Create new states for copy, add marks and copy finals if required
			for (int i = 0; i != states.Count; i++)
			{
				NFAState state = new NFAState();
				state.Mark = states[i].Mark;
				if (keepFinals)
					state.AddItems(states[i].Items);
				copy.states.Add(state);
			}
			// Make linkage
			for (int i = 0; i != states.Count; i++)
			{
				foreach (NFATransition transition in states[i].Transitions)
					copy.states[i].AddTransition(transition.Span, copy.states[states.IndexOf(transition.Next)]);
			}
			if (StateEntry != null)
				copy.StateEntry = copy.states[states.IndexOf(StateEntry)];
			if (StateExit != null)
				copy.StateExit = copy.states[states.IndexOf(StateExit)];
			return copy;
		}

		/// <summary>
		/// Inserts all the states of the given automaton into this one
		/// This does not make a copy of the states, this directly includes them
		/// </summary>
		/// <param name="sub">Sub-automaton to include in this one</param>
		public void InsertSubNFA(NFA sub)
		{
			states.AddRange(sub.states);
		}

		/// <summary>
		/// Creates and initializes a minimal automaton with an entry state and a separate exit state
		/// </summary>
		/// <returns>The created automaton</returns>
		public static NFA NewMinimal()
		{
			NFA result = new NFA();
			result.StateEntry = result.AddNewState();
			result.StateExit = result.AddNewState();
			return result;
		}

		/// <summary>
		/// Creates an automaton that represents makes the given sub-automaton optional
		/// </summary>
		/// <param name="sub">The sub-automaton to make optional</param>
		/// <param name="useClones">True to completely clone the sub-automaton</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewOptional(NFA sub, bool useClones)
		{
			NFA final = NewMinimal();
			if (useClones)
				sub = sub.Clone();
			final.states.AddRange(sub.states);
			final.StateEntry.AddTransition(NFA.EPSILON, sub.StateEntry);
			final.StateEntry.AddTransition(NFA.EPSILON, final.StateExit);
			sub.StateExit.AddTransition(NFA.EPSILON, final.StateExit);
			return final;
		}

		/// <summary>
		/// Creates an automaton that repeats the sub-automaton zero or more times
		/// </summary>
		/// <param name="sub">The sub-automaton</param>
		/// <param name="useClones">True to completely clone the sub-automaton</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewRepeatZeroMore(NFA sub, bool useClones)
		{
			NFA final = NewMinimal();
			if (useClones)
				sub = sub.Clone();
			final.states.AddRange(sub.states);
			final.StateEntry.AddTransition(NFA.EPSILON, sub.StateEntry);
			final.StateEntry.AddTransition(NFA.EPSILON, final.StateExit);
			sub.StateExit.AddTransition(NFA.EPSILON, final.StateExit);
			final.StateExit.AddTransition(NFA.EPSILON, sub.StateEntry);
			return final;
		}

		/// <summary>
		/// Creates an automaton that repeats the sub-automaton one or more times
		/// </summary>
		/// <param name="sub">The sub-automaton</param>
		/// <param name="useClones">True to completely clone the sub-automaton</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewRepeatOneOrMore(NFA sub, bool useClones)
		{
			NFA final = NewMinimal();
			if (useClones)
				sub = sub.Clone();
			final.states.AddRange(sub.states);
			final.StateEntry.AddTransition(NFA.EPSILON, sub.StateEntry);
			sub.StateExit.AddTransition(NFA.EPSILON, final.StateExit);
			final.StateExit.AddTransition(NFA.EPSILON, sub.StateEntry);
			return final;
		}

		/// <summary>
		/// Creates an automaton that repeats the sub-automaton a number of times in the given range [min, max]
		/// </summary>
		/// <param name="sub">The sub-automaton</param>
		/// <param name="min">The minimum (included) number of time to repeat</param>
		/// <param name="max">The maximum (included) number of time to repeat</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewRepeatRange(NFA sub, int min, int max)
		{
			NFA final = NewMinimal();

			NFAState last = final.StateEntry;
			for (int i = 0; i != min; i++)
			{
				NFA inner = sub.Clone();
				final.states.AddRange(inner.states);
				last.AddTransition(NFA.EPSILON, inner.StateEntry);
				last = inner.StateExit;
			}
			for (int i = min; i != max; i++)
			{
				NFA inner = NewOptional(sub, true);
				final.states.AddRange(inner.states);
				last.AddTransition(NFA.EPSILON, inner.StateEntry);
				last = inner.StateExit;
			}
			last.AddTransition(NFA.EPSILON, final.StateExit);

			if (min == 0)
				final.StateEntry.AddTransition(NFA.EPSILON, final.StateExit);
			return final;
		}

		/// <summary>
		/// Creates an automaton that is the union of the two sub-automaton
		/// </summary>
		/// <param name="left">The left automaton</param>
		/// <param name="right">The right automaton</param>
		/// <param name="useClones">True to completely clone the sub-automata</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewUnion(NFA left, NFA right, bool useClones)
		{
			NFA final = NewMinimal();
			if (useClones)
			{
				left = left.Clone(true);
				right = right.Clone(true);
			}
			final.states.AddRange(left.states);
			final.states.AddRange(right.states);
			final.StateEntry.AddTransition(NFA.EPSILON, left.StateEntry);
			final.StateEntry.AddTransition(NFA.EPSILON, right.StateEntry);
			left.StateExit.AddTransition(NFA.EPSILON, final.StateExit);
			right.StateExit.AddTransition(NFA.EPSILON, final.StateExit);
			return final;
		}

		/// <summary>
		/// Creates an automaton that concatenates the two sub-automaton
		/// </summary>
		/// <param name="left">The left automaton</param>
		/// <param name="right">The right automaton</param>
		/// <param name="useClones">True to completely clone the sub-automata</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewConcatenation(NFA left, NFA right, bool useClones)
		{
			NFA final = new NFA();
			if (useClones)
			{
				left = left.Clone(true);
				right = right.Clone(true);
			}
			final.states.AddRange(left.states);
			final.states.AddRange(right.states);
			final.StateEntry = left.StateEntry;
			final.StateExit = right.StateExit;
			left.StateExit.AddTransition(NFA.EPSILON, right.StateEntry);
			return final;
		}

		/// <summary>
		/// Creates an automaton that is the difference between the left and right sub-automata
		/// </summary>
		/// <param name="left">The left automaton</param>
		/// <param name="right">The right automaton</param>
		/// <param name="useClones">True to completely clone the sub-automata</param>
		/// <returns>The new automaton</returns>
		// TODO: should get rid of static methods
		public static NFA NewDifference(NFA left, NFA right, bool useClones)
		{
			NFA final = NewMinimal();
			NFAState statePositive = final.AddNewState();
			NFAState stateNegative = final.AddNewState();
			statePositive.Mark = 1;
			stateNegative.Mark = -1;

			if (useClones)
			{
				left = left.Clone(true);
				right = right.Clone(true);
			}
			final.states.AddRange(left.states);
			final.states.AddRange(right.states);
			final.StateEntry.AddTransition(NFA.EPSILON, left.StateEntry);
			final.StateEntry.AddTransition(NFA.EPSILON, right.StateEntry);
			left.StateExit.AddTransition(NFA.EPSILON, statePositive);
			right.StateExit.AddTransition(NFA.EPSILON, stateNegative);
			statePositive.AddTransition(NFA.EPSILON, final.StateExit);

			final.StateExit.AddItem(DummyItem.Instance);
			DFA equivalent = new DFA(final);
			equivalent.Prune();
			final = new NFA(equivalent);
			final.StateExit = final.AddNewState();
			foreach (NFAState state in final.states)
			{
				if (state.Items.Contains(DummyItem.Instance))
				{
					state.ClearItems();
					state.AddTransition(NFA.EPSILON, final.StateExit);
				}
			}
			return final;
		}
	}
}
