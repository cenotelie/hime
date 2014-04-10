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
        public List<DFAState> States { get { return states; } }
        
        /// <summary>
        /// Gets the entry state of this automaton
        /// </summary>
        public DFAState Entry { get { return states[0]; } }

		/// <summary>
		/// Initializes a new (empty DFA)
		/// </summary>
		public DFA ()
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
            DFAState dfaInit = new DFAState();
            states.Add(dfaInit);
            dfaInit.ID = 0;

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
                        DFAState newState = new DFAState();
                        newState.ID = states.Count;
                        states.Add(newState);
                        // Setup transition
                        states[i].AddTransition(value, newState);
                    }
                }
                // Add finals
                states[i].AddFinals(nfaStateSets[i].GetFinals());
            }
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
            Dictionary<DFAState, List<DFAState>> inverses = new Dictionary<DFAState, List<DFAState>>();
            List<DFAState> finals = new List<DFAState>();
            foreach (DFAState state in states)
            {
                foreach (DFAState next in state.Transitions.Values)
                {
                    if (!inverses.ContainsKey(next))
                        inverses.Add(next, new List<DFAState>());
                    inverses[next].Add(state);
                }
                if (state.FinalsCount != 0)
                    finals.Add(state);
            }

            for (int i = 0; i != finals.Count; i++)
            {
                DFAState state = finals[i];
                if (inverses.ContainsKey(state))
                    foreach (DFAState antecedent in inverses[state])
                        if (!finals.Contains(antecedent))
                            finals.Add(antecedent);
            }

            if (finals.Count == states.Count)
                return;
            for (int i = 0; i != states.Count; i++ )
            {
                DFAState state = states[i];
                if (!finals.Contains(state))
                {
                    states.Remove(state);
                    i--;
                    if (inverses.ContainsKey(state))
                    {
                        foreach (DFAState antecedent in inverses[state])
                        {
                            List<CharSpan> keys = new List<CharSpan>(antecedent.Transitions.Keys);
                            foreach (CharSpan key in keys)
                                if (antecedent.Transitions[key] == state)
                                    antecedent.Transitions.Remove(key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Serializes the graph of this automaton with the given serializer
        /// </summary>
        /// <param name="serializer">A serializer</param>
        public void SerializeGraph(Documentation.DOTSerializer serializer)
        {
            foreach (DFAState state in states)
            {
                if (state.TopItem != null)
                    serializer.WriteNode(state.ID.ToString(), state.ID.ToString() + " : " + state.TopItem.ToString(), Documentation.DOTNodeShape.ellipse);
                else
                    serializer.WriteNode(state.ID.ToString());
            }
            foreach (DFAState state in states)
                foreach (CharSpan value in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[value].ID.ToString(), value.ToString());
        }
    }
}
