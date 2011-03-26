using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public sealed class DFAState
    {
        private Dictionary<TerminalNFACharSpan, DFAState> p_Transitions;
        private Terminal p_Final;
        private List<Terminal> p_Finals;
        private int p_ID;

        public Terminal Final { get { return p_Final; } }
        public List<Terminal> Finals { get { return p_Finals; } }
        public Dictionary<TerminalNFACharSpan, DFAState> Transitions { get { return p_Transitions; } }
        public int FinalsCount { get { return p_Finals.Count; } }
        public int TransitionsCount { get { return p_Transitions.Count; } }
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }

        public DFAState()
        {
            p_Finals = new List<Terminal>();
            p_Transitions = new Dictionary<TerminalNFACharSpan, DFAState>();
        }

        public void AddFinal(Terminal item)
        {
            if (!p_Finals.Contains(item))
            {
                p_Finals.Add(item);
                if (p_Final == null)
                    p_Final = item;
                else
                {
                    if (item.Priority > p_Final.Priority)
                        p_Final = item;
                }
            }
        }
        public void AddFinals(ICollection<Terminal> items)
        {
            foreach (Terminal item in items)
                AddFinal(item);
        }
        public void ClearFinals() { p_Finals.Clear(); p_Final = null; }

        public void AddTransition(TerminalNFACharSpan Value, DFAState Next) { p_Transitions.Add(Value, Next); }
        public void ClearTransitions() { p_Transitions.Clear(); }

        public void RepackTransitions()
        {
            Dictionary<DFAState, List<TerminalNFACharSpan>> Inverse = new Dictionary<DFAState, List<TerminalNFACharSpan>>();
            foreach (KeyValuePair<TerminalNFACharSpan, DFAState> Transition in p_Transitions)
            {
                if (!Inverse.ContainsKey(Transition.Value))
                    Inverse.Add(Transition.Value, new List<TerminalNFACharSpan>());
                Inverse[Transition.Value].Add(Transition.Key);
            }
            p_Transitions.Clear();
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
                    p_Transitions.Add(Key, Child);
            }
        }
    }

    public sealed class DFAStateGroup
    {
        private List<DFAState> p_States;

        public ICollection<DFAState> States { get { return p_States; } }

        public DFAState Representative { get { return p_States[0]; } }

        public DFAStateGroup(DFAState Init)
        {
            p_States = new List<DFAState>();
            p_States.Add(Init);
        }

        public void AddState(DFAState State) { p_States.Add(State); }

        public DFAPartition Split(DFAPartition Current)
        {
            DFAPartition Partition = new DFAPartition();
            foreach (DFAState State in p_States)
                Partition.AddState(State, Current);
            return Partition;
        }

        public bool Contains(DFAState State) { return p_States.Contains(State); }
    }


    public sealed class DFAPartition
    {
        private List<DFAStateGroup> p_Groups;

        public int GroupCount { get { return p_Groups.Count; } }

        public DFAPartition()
        {
            p_Groups = new List<DFAStateGroup>();
        }
        public DFAPartition(DFA DFA)
        {
            p_Groups = new List<DFAStateGroup>();
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
                    foreach (DFAStateGroup Group in p_Groups)
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
                        p_Groups.Add(new DFAStateGroup(State));
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
                p_Groups.Insert(0, NonFinals);
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
            foreach (DFAStateGroup Group in p_Groups)
                New.p_Groups.AddRange(Group.Split(this).p_Groups);
            return New;
        }

        public void AddState(DFAState State, DFAPartition Old)
        {
            bool Added = false;
            // For each group in the resulting groups set
            foreach (DFAStateGroup Group in p_Groups)
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
                p_Groups.Add(new DFAStateGroup(State));
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
            foreach (DFAStateGroup Group in p_Groups)
                if (Group.Contains(State))
                    return Group;
            return null;
        }

        public List<DFAState> GetDFAStates()
        {
            // For each group in the partition
            foreach (DFAStateGroup Group in p_Groups)
            {
                bool Found = false;
                foreach (DFAState State in Group.States)
                {
                    if (State.ID == 0)
                    {
                        p_Groups.Remove(Group);
                        p_Groups.Insert(0, Group);
                        Found = true;
                        break;
                    }
                }
                if (Found) break;
            }

            List<DFAState> States = new List<DFAState>();
            // For each group in the partition
            // Create the coresponding state and add the finals
            foreach (DFAStateGroup Group in p_Groups)
            {
                // Create a new state
                DFAState New = new DFAState();
                // Add the terminal from the group to the new state
                New.AddFinals(Group.Representative.Finals);
                States.Add(New);
            }
            // Do linkage and setup ID
            for (int i = 0; i != p_Groups.Count; i++)
            {
                States[i].ID = i;
                foreach (TerminalNFACharSpan Key in p_Groups[i].Representative.Transitions.Keys)
                    States[i].AddTransition(Key, States[GetDFAStates_GetGroupIndexOf(p_Groups[i].Representative.Transitions[Key])]);
            }
            return States;
        }

        private int GetDFAStates_GetGroupIndexOf(DFAState State)
        {
            for (int i = 0; i != p_Groups.Count; i++)
                if (p_Groups[i].Contains(State))
                    return i;
            return -1;
        }
    }


    public sealed class DFA
    {
        private List<DFAState> p_States;

        public ICollection<DFAState> States { get { return p_States; } }
        public DFAState Entry { get { return p_States[0]; } }

        private DFA(List<DFAState> States) { p_States = States; }
        public DFA(NFA NFA)
        {
            p_States = new List<DFAState>();

            List<NFAStateSet> p_NFAStateSets = new List<NFAStateSet>();
            // Create the first NFA set, add the entry and close it
            NFAStateSet NFAInit = new NFAStateSet();
            p_NFAStateSets.Add(NFAInit);
            NFAInit.Add(NFA.StateEntry);
            NFAInit.Close();
            // Create the initial state for the DFA
            DFAState DFAInit = new DFAState();
            p_States.Add(DFAInit);
            DFAInit.ID = 0;

            // For each set in the list of the NFA states
            for (int i = 0; i != p_NFAStateSets.Count; i++)
            {
                // Normalize transitions
                p_NFAStateSets[i].Normalize();

                // Get the transitions for the set
                Dictionary<TerminalNFACharSpan, NFAStateSet> Transitions = p_NFAStateSets[i].GetTransitions();
                // For each transition
                foreach (TerminalNFACharSpan Value in Transitions.Keys)
                {
                    // The child by the transition
                    NFAStateSet Next = Transitions[Value];
                    // Search for the child in the existing sets
                    bool Found = false;
                    for (int j = 0; j != p_NFAStateSets.Count; j++)
                    {
                        // An existing equivalent set is already present
                        if (p_NFAStateSets[j] == Next)
                        {
                            p_States[i].AddTransition(Value, p_States[j]);
                            Found = true;
                            break;
                        }
                    }
                    // The child is not already present
                    if (!Found)
                    {
                        // Add to the sets list
                        p_NFAStateSets.Add(Next);
                        // Create the corresponding DFA state
                        DFAState New = new DFAState();
                        New.ID = p_States.Count;
                        p_States.Add(New);
                        // Setup transition
                        p_States[i].AddTransition(Value, New);
                    }
                }
                // Add finals
                p_States[i].AddFinals(p_NFAStateSets[i].GetFinals());
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
            foreach (DFAState State in p_States)
                State.RepackTransitions();
        }

        public void Prune()
        {
            Dictionary<DFAState, List<DFAState>> inverses = new Dictionary<DFAState, List<DFAState>>();
            List<DFAState> finals = new List<DFAState>();
            foreach (DFAState state in p_States)
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

            if (finals.Count == p_States.Count)
                return;
            for (int i = 0; i != p_States.Count; i++ )
            {
                DFAState state = p_States[i];
                if (!finals.Contains(state))
                {
                    p_States.Remove(state);
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
            foreach (DFAState state in p_States)
            {
                if (state.Final != null)
                    serializer.WriteNode(state.ID.ToString(), state.ID.ToString() + " : " + state.Final.LocalName, Kernel.Graphs.DOTNodeShape.ellipse);
                else
                    serializer.WriteNode(state.ID.ToString());
            }
            foreach (DFAState state in p_States)
                foreach (TerminalNFACharSpan value in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[value].ID.ToString(), value.ToString());
        }
    }
}
