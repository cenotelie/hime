using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public sealed class DFAState
    {
        private Dictionary<TerminalNFACharSpan, DFAState> transitions;
        private Terminal final;
        private List<Terminal> finals;
        private int iD;

        public Terminal Final { get { return final; } }
        public List<Terminal> Finals { get { return finals; } }
        public Dictionary<TerminalNFACharSpan, DFAState> Transitions { get { return transitions; } }
        public int FinalsCount { get { return finals.Count; } }
        public int TransitionsCount { get { return transitions.Count; } }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        public DFAState()
        {
            finals = new List<Terminal>();
            transitions = new Dictionary<TerminalNFACharSpan, DFAState>();
        }

        public void AddFinal(Terminal item)
        {
            if (!finals.Contains(item))
            {
                finals.Add(item);
                if (final == null)
                    final = item;
                else
                {
                    if (item.Priority > final.Priority)
                        final = item;
                }
            }
        }
        public void AddFinals(ICollection<Terminal> items)
        {
            foreach (Terminal item in items)
                AddFinal(item);
        }
        public void ClearFinals() { finals.Clear(); final = null; }

        public void AddTransition(TerminalNFACharSpan Value, DFAState Next) { transitions.Add(Value, Next); }
        public void ClearTransitions() { transitions.Clear(); }

        public void RepackTransitions()
        {
            Dictionary<DFAState, List<TerminalNFACharSpan>> Inverse = new Dictionary<DFAState, List<TerminalNFACharSpan>>();
            foreach (KeyValuePair<TerminalNFACharSpan, DFAState> Transition in transitions)
            {
                if (!Inverse.ContainsKey(Transition.Value))
                    Inverse.Add(Transition.Value, new List<TerminalNFACharSpan>());
                Inverse[Transition.Value].Add(Transition.Key);
            }
            transitions.Clear();
            foreach (DFAState Child in Inverse.Keys)
            {
                List<TerminalNFACharSpan> Keys = Inverse[Child];
                Keys.Sort(new System.Comparison<Automata.TerminalNFACharSpan>(Automata.TerminalNFACharSpan.Compare));
                for (int i = 0; i != Keys.Count; i++)
                {
                    TerminalNFACharSpan K1 = Keys[i];
                    for (int j = i + 1; j != Keys.Count; j++)
                    {
                        TerminalNFACharSpan K2 = Keys[j];
                        if (K2.Begin == K1.End + 1)
                        {
                            K1 = new TerminalNFACharSpan(K1.Begin, K2.End);
                            Keys[i] = K1;
                            Keys.RemoveAt(j);
                            j--;
                        }
                    }
                }
                foreach (TerminalNFACharSpan Key in Keys)
                    transitions.Add(Key, Child);
            }
        }
    }

    public sealed class DFAStateGroup
    {
        private List<DFAState> states;

        public ICollection<DFAState> States { get { return states; } }

        public DFAState Representative { get { return states[0]; } }

        public DFAStateGroup(DFAState Init)
        {
            states = new List<DFAState>();
            states.Add(Init);
        }

        public void AddState(DFAState State) { states.Add(State); }

        public DFAPartition Split(DFAPartition Current)
        {
            DFAPartition Partition = new DFAPartition();
            foreach (DFAState State in states)
                Partition.AddState(State, Current);
            return Partition;
        }

        public bool Contains(DFAState State) { return states.Contains(State); }
    }


    public sealed class DFAPartition
    {
        private List<DFAStateGroup> groups;

        public int GroupCount { get { return groups.Count; } }

        public DFAPartition()
        {
            groups = new List<DFAStateGroup>();
        }
        public DFAPartition(DFA DFA)
        {
            groups = new List<DFAStateGroup>();
            // Partition the DFA states between final and non-finals
            DFAStateGroup NonFinals = null;
            // For each state in the DFA
            foreach (DFAState State in DFA.States)
            {
                // If the state is final
                if (State.FinalsCount != 0)
                {
                    bool Added = false;
                    // Look for a correspondinf group in the existing ones
                    foreach (DFAStateGroup Group in groups)
                    {
                        // If the current state and the group have same finals set : group found
                        if (DFAPartition_SameFinals(Group.Representative, State))
                        {
                            // Add the set to the group
                            Group.AddState(State);
                            Added = true;
                            break;
                        }
                    }
                    // No previous group match
                    // => Create a new group from the current state
                    if (!Added)
                        groups.Add(new DFAStateGroup(State));
                }
                else // Here the state is not final
                {
                    // Check for the group existence : create it if necessary
                    if (NonFinals == null)
                        NonFinals = new DFAStateGroup(State);
                    else // add the state to the non-final group
                        NonFinals.AddState(State);
                }
            }
            // If the non-final group exists : this has to be the first group
            if (NonFinals != null)
                groups.Insert(0, NonFinals);
        }

        public bool DFAPartition_SameFinals(DFAState Left, DFAState Right)
        {
            if (Left.FinalsCount != Right.FinalsCount)
                return false;
            foreach (Terminal Final in Left.Finals)
            {
                if (!Right.Finals.Contains(Final))
                    return false;
            }
            return true;
        }

        public DFAPartition Refine()
        {
            DFAPartition New = new DFAPartition();
            // For each group in the current partition
            // Split the group and add the resulting groups to the new partition
            foreach (DFAStateGroup Group in groups)
                New.groups.AddRange(Group.Split(this).groups);
            return New;
        }

        public void AddState(DFAState State, DFAPartition Old)
        {
            bool Added = false;
            // For each group in the resulting groups set
            foreach (DFAStateGroup Group in groups)
            {
                // If the current state can be in this resulting group according to the old partition :
                if (AddState_SameGroup(Group.Representative, State, Old))
                {
                    // Add the state to the group
                    Group.AddState(State);
                    Added = true;
                }
            }
            // The state cannot be in any groups : create a new group
            if (!Added)
                groups.Add(new DFAStateGroup(State));
        }

        private static bool AddState_SameGroup(DFAState S1, DFAState S2, DFAPartition Old)
        {
            if (S1.Transitions.Count != S2.Transitions.Count)
                return false;
            // For each transition from state 1
            foreach (TerminalNFACharSpan Key in S1.Transitions.Keys)
            {
                // If state 2 does not have a transition with the same value : not same group
                if (!S2.Transitions.ContainsKey(Key))
                    return false;
                // Here State1 and State2 have both a transition of the same value
                // If the target of these transitions are in the same group in the old partition : same transition
                if (Old.AddState_GetGroupOf(S1.Transitions[Key]) != Old.AddState_GetGroupOf(S2.Transitions[Key]))
                    return false;
            }
            return true;
        }

        private DFAStateGroup AddState_GetGroupOf(DFAState State)
        {
            foreach (DFAStateGroup Group in groups)
                if (Group.Contains(State))
                    return Group;
            return null;
        }

        public List<DFAState> GetDFAStates()
        {
            // For each group in the partition
            foreach (DFAStateGroup Group in groups)
            {
                bool Found = false;
                foreach (DFAState State in Group.States)
                {
                    if (State.ID == 0)
                    {
                        groups.Remove(Group);
                        groups.Insert(0, Group);
                        Found = true;
                        break;
                    }
                }
                if (Found) break;
            }

            List<DFAState> States = new List<DFAState>();
            // For each group in the partition
            // Create the coresponding state and add the finals
            foreach (DFAStateGroup Group in groups)
            {
                // Create a new state
                DFAState New = new DFAState();
                // Add the terminal from the group to the new state
                New.AddFinals(Group.Representative.Finals);
                States.Add(New);
            }
            // Do linkage and setup ID
            for (int i = 0; i != groups.Count; i++)
            {
                States[i].ID = i;
                foreach (TerminalNFACharSpan Key in groups[i].Representative.Transitions.Keys)
                    States[i].AddTransition(Key, States[GetDFAStates_GetGroupIndexOf(groups[i].Representative.Transitions[Key])]);
            }
            return States;
        }

        private int GetDFAStates_GetGroupIndexOf(DFAState State)
        {
            for (int i = 0; i != groups.Count; i++)
                if (groups[i].Contains(State))
                    return i;
            return -1;
        }
    }


    public sealed class DFA
    {
        private List<DFAState> states;

        public ICollection<DFAState> States { get { return states; } }
        public DFAState Entry { get { return states[0]; } }

        private DFA(List<DFAState> States) { states = States; }
        public DFA(NFA NFA)
        {
            states = new List<DFAState>();

            List<NFAStateSet> nFAStateSets = new List<NFAStateSet>();
            // Create the first NFA set, add the entry and close it
            NFAStateSet NFAInit = new NFAStateSet();
            nFAStateSets.Add(NFAInit);
            NFAInit.Add(NFA.StateEntry);
            NFAInit.Close();
            // Create the initial state for the DFA
            DFAState DFAInit = new DFAState();
            states.Add(DFAInit);
            DFAInit.ID = 0;

            // For each set in the list of the NFA states
            for (int i = 0; i != nFAStateSets.Count; i++)
            {
                // Normalize transitions
                nFAStateSets[i].Normalize();

                // Get the transitions for the set
                Dictionary<TerminalNFACharSpan, NFAStateSet> Transitions = nFAStateSets[i].GetTransitions();
                // For each transition
                foreach (TerminalNFACharSpan Value in Transitions.Keys)
                {
                    // The child by the transition
                    NFAStateSet Next = Transitions[Value];
                    // Search for the child in the existing sets
                    bool Found = false;
                    for (int j = 0; j != nFAStateSets.Count; j++)
                    {
                        // An existing equivalent set is already present
                        if (nFAStateSets[j] == Next)
                        {
                            states[i].AddTransition(Value, states[j]);
                            Found = true;
                            break;
                        }
                    }
                    // The child is not already present
                    if (!Found)
                    {
                        // Add to the sets list
                        nFAStateSets.Add(Next);
                        // Create the corresponding DFA state
                        DFAState New = new DFAState();
                        New.ID = states.Count;
                        states.Add(New);
                        // Setup transition
                        states[i].AddTransition(Value, New);
                    }
                }
                // Add finals
                states[i].AddFinals(nFAStateSets[i].GetFinals());
            }
        }

        public DFA Minimize()
        {
            DFAPartition Current = new DFAPartition(this);
            DFAPartition New = Current.Refine();
            while (New.GroupCount != Current.GroupCount)
            {
                Current = New;
                New = Current.Refine();
            }
            return new DFA(New.GetDFAStates());
        }

        public void RepackTransitions()
        {
            foreach (DFAState State in states)
                State.RepackTransitions();
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
                            List<TerminalNFACharSpan> keys = new List<TerminalNFACharSpan>(antecedent.Transitions.Keys);
                            foreach (TerminalNFACharSpan key in keys)
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
                foreach (TerminalNFACharSpan value in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[value].ID.ToString(), value.ToString());
        }
    }
}
