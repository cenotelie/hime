/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public sealed class DFA
    {
        private List<DFAState> states;

        public ICollection<DFAState> States { get { return states; } }
        public DFAState Entry { get { return states[0]; } }

        private DFA(List<DFAState> states) { this.states = states; }
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
                        if (nfaStateSets[j] == next)
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

        public void RepackTransitions()
        {
            foreach (DFAState state in states)
                state.RepackTransitions();
        }

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

        public void SerializeGraph(Kernel.Graphs.DOTSerializer serializer)
        {
            foreach (DFAState state in states)
            {
                if (state.Final != null)
                    serializer.WriteNode(state.ID.ToString(), state.ID.ToString() + " : " + state.Final.LocalName, Kernel.Graphs.DOTNodeShape.ellipse);
                else
                    serializer.WriteNode(state.ID.ToString());
            }
            foreach (DFAState state in states)
                foreach (CharSpan value in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[value].ID.ToString(), value.ToString());
        }
    }
}
