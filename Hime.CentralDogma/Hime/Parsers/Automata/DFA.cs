namespace Hime.Parsers.Automata
{
    /// <summary>
    /// Represents a terminal DFA state
    /// </summary>
    public sealed class DFAState
    {
        /// <summary>
        /// The transitions from the current state
        /// </summary>
        private System.Collections.Generic.Dictionary<TerminalNFACharSpan, DFAState> p_Transitions;
        /// <summary>
        /// The symbol recognized at this state with maximal priority
        /// </summary>
        private Terminal p_Final;
        /// <summary>
        /// The list of the terminal symbols recognized at this state
        /// </summary>
        private System.Collections.Generic.List<Terminal> p_Finals;
        /// <summary>
        /// The ID of the state
        /// </summary>
        private int p_ID;

        /// <summary>
        /// Get the terminal recognized at this state with the maximal priority
        /// </summary>
        public Terminal Final { get { return p_Final; } }
        /// <summary>
        /// Get the list of the terminal recognized at this state
        /// </summary>
        public System.Collections.Generic.List<Terminal> Finals { get { return p_Finals; } }
        /// <summary>
        /// Get the transitions for the current state
        /// </summary>
        /// <value>The transitions from the current state</value>
        public System.Collections.Generic.Dictionary<TerminalNFACharSpan, DFAState> Transitions { get { return p_Transitions; } }
        /// <summary>
        /// Get the number of finals for this state
        /// </summary>
        /// <value>The number of terminal symbols recognized at this state</value>
        public int FinalsCount { get { return p_Finals.Count; } }
        /// <summary>
        /// Get the number of transitions
        /// </summary>
        /// <value>The number of transition from this state</value>
        public int TransitionsCount { get { return p_Transitions.Count; } }
        /// <summary>
        /// Get or set the state's ID
        /// </summary>
        /// <value>The state's ID</value>
        public int ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }

        /// <summary>
        /// Construct the state
        /// </summary>
        public DFAState()
        {
            p_Finals = new System.Collections.Generic.List<Terminal>();
            p_Transitions = new System.Collections.Generic.Dictionary<TerminalNFACharSpan, DFAState>();
        }

        /// <summary>
        /// Add the given terminal as a final for this state
        /// </summary>
        /// <param name="item">The terminal to add</param>
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
        /// <summary>
        /// Add the given terminals as a finals for this state
        /// </summary>
        /// <param name="items">The terminals to add</param>
        public void AddFinals(System.Collections.Generic.IEnumerable<Terminal> items)
        {
            foreach (Terminal item in items)
                AddFinal(item);
        }
        /// <summary>
        /// Remove all finals for the current set
        /// </summary>
        public void ClearFinals() { p_Finals.Clear(); p_Final = null; }

        /// <summary>
        /// Add a new transition from this state
        /// </summary>
        /// <param name="Value">The transition value</param>
        /// <param name="Next">The target of the transition</param>
        public void AddTransition(TerminalNFACharSpan Value, DFAState Next) { p_Transitions.Add(Value, Next); }
        /// <summary>
        /// Remove all transitions for the state
        /// </summary>
        public void ClearTransitions() { p_Transitions.Clear(); }

        public void RepackTransitions()
        {
            System.Collections.Generic.Dictionary<DFAState, System.Collections.Generic.List<TerminalNFACharSpan>> Inverse = new System.Collections.Generic.Dictionary<DFAState, System.Collections.Generic.List<TerminalNFACharSpan>>();
            foreach (System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, DFAState> Transition in p_Transitions)
            {
                if (!Inverse.ContainsKey(Transition.Value))
                    Inverse.Add(Transition.Value, new System.Collections.Generic.List<TerminalNFACharSpan>());
                Inverse[Transition.Value].Add(Transition.Key);
            }
            p_Transitions.Clear();
            foreach (DFAState Child in Inverse.Keys)
            {
                System.Collections.Generic.List<TerminalNFACharSpan> Keys = Inverse[Child];
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

    /// <summary>
    /// Represents a set of DFA states
    /// </summary>
    public sealed class DFAStateGroup
    {
        /// <summary>
        /// The list of states in the current group
        /// </summary>
        private System.Collections.Generic.List<DFAState> p_States;

        public System.Collections.Generic.IEnumerable<DFAState> States { get { return p_States; } }

        /// <summary>
        /// Get the DFA state representative of the current group
        /// </summary>
        /// <value>The DFA state representative of the current group</value>
        public DFAState Representative { get { return p_States[0]; } }

        /// <summary>
        /// Construct the group with the given state as reprentant
        /// </summary>
        /// <param name="Init">The initial state for the group</param>
        public DFAStateGroup(DFAState Init)
        {
            p_States = new System.Collections.Generic.List<DFAState>();
            p_States.Add(Init);
        }

        /// <summary>
        /// Add a new state in the group
        /// </summary>
        /// <param name="State">The state to add</param>
        public void AddState(DFAState State) { p_States.Add(State); }

        /// <summary>
        /// Split the group according to the current partition
        /// </summary>
        /// <param name="Current">the current partition</param>
        /// <returns>Returns the list of the resulting groups as a partition</returns>
        public DFAPartition Split(DFAPartition Current)
        {
            DFAPartition Partition = new DFAPartition();
            foreach (DFAState State in p_States)
                Partition.AddState(State, Current);
            return Partition;
        }

        /// <summary>
        /// Determine if the given state is in the group
        /// </summary>
        /// <param name="State">State to test</param>
        /// <returns>Returns true if the state is in the group, false otherwise</returns>
        public bool Contains(DFAState State) { return p_States.Contains(State); }
    }


    /// <summary>
    /// Represents a partition of a DFA. A partition is a set of DFA state groups
    /// </summary>
    public sealed class DFAPartition
    {
        /// <summary>
        /// List of the partition's groups
        /// </summary>
        private System.Collections.Generic.List<DFAStateGroup> p_Groups;

        /// <summary>
        /// Get the number of groups in the partition
        /// </summary>
        /// <value>The number of groups in the partition</value>
        public int GroupCount { get { return p_Groups.Count; } }

        /// <summary>
        /// Construct an empty partition
        /// </summary>
        public DFAPartition()
        {
            p_Groups = new System.Collections.Generic.List<DFAStateGroup>();
        }
        /// <summary>
        /// Construct the partition from the given DFA
        /// </summary>
        /// <param name="DFA">The original DFA</param>
        public DFAPartition(DFA DFA)
        {
            p_Groups = new System.Collections.Generic.List<DFAStateGroup>();
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

        /// <summary>
        /// Determine of two states have the same finals
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns true if the two states have the same finals, false otherwise</returns>
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

        /// <summary>
        /// Refine the current partition
        /// </summary>
        /// <returns></returns>
        public DFAPartition Refine()
        {
            DFAPartition New = new DFAPartition();
            // For each group in the current partition
            // Split the group and add the resulting groups to the new partition
            foreach (DFAStateGroup Group in p_Groups)
                New.p_Groups.AddRange(Group.Split(this).p_Groups);
            return New;
        }

        /// <summary>
        /// Add the state to the current partition
        /// </summary>
        /// <param name="State">The state to insert</param>
        /// <param name="Old">The old partition</param>
        /// <remarks>
        /// This function is used to split a group.
        /// The current partition is a set of group resulting from the splitting of a group.
        /// </remarks>
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

        /// <summary>
        /// Determine if two state have to be in the same group in a new partition
        /// </summary>
        /// <param name="S1">State 1</param>
        /// <param name="S2">State 2</param>
        /// <param name="Old">the old partition</param>
        /// <returns>Returns true if the two states have to be in the same group</returns>
        /// <remarks>
        /// Two states have to be in the same group if they have the same transitions to the same groups in the old partition.
        /// This function is used to split a group.
        /// The current partition is a set of group resulting from the splitting of a group.
        /// </remarks>
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

        /// <summary>
        /// Get the group in the partition of the given state
        /// </summary>
        /// <param name="State">The state to look for</param>
        /// <returns>The group containing the state or null if not found</returns>
        private DFAStateGroup AddState_GetGroupOf(DFAState State)
        {
            foreach (DFAStateGroup Group in p_Groups)
                if (Group.Contains(State))
                    return Group;
            return null;
        }

        /// <summary>
        /// Get the DFA states resulting from the current partition
        /// </summary>
        /// <returns>The DFA corresponding to the groups in the partition</returns>
        public System.Collections.Generic.List<DFAState> GetDFAStates()
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

            System.Collections.Generic.List<DFAState> States = new System.Collections.Generic.List<DFAState>();
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

        /// <summary>
        /// Get the group index in the partition of the given state
        /// </summary>
        /// <param name="State">The state to look for</param>
        /// <returns>The index of the group containing the state or -1 if not found</returns>
        private int GetDFAStates_GetGroupIndexOf(DFAState State)
        {
            for (int i = 0; i != p_Groups.Count; i++)
                if (p_Groups[i].Contains(State))
                    return i;
            return -1;
        }
    }


    /// <summary>
    /// Represent a terminal DFA
    /// </summary>
    public sealed class DFA
    {
        /// <summary>
        /// List of the DFA states
        /// </summary>
        private System.Collections.Generic.List<DFAState> p_States;

        /// <summary>
        /// Get an enumeration of the DFA states
        /// </summary>
        /// <value>An enumeration of the DFA states</value>
        public System.Collections.Generic.IEnumerable<DFAState> States { get { return p_States; } }
        /// <summary>
        /// Get the entry state for the DFA
        /// </summary>
        public DFAState Entry { get { return p_States[0]; } }

        /// <summary>
        /// Construct the DFA form the given states
        /// </summary>
        /// <param name="States">The states for the DFA</param>
        private DFA(System.Collections.Generic.List<DFAState> States) { p_States = States; }
        /// <summary>
        /// Construct the DFA from the given NFA
        /// </summary>
        /// <param name="NFA">The original equivalent NFA</param>
        public DFA(NFA NFA)
        {
            p_States = new System.Collections.Generic.List<DFAState>();

            System.Collections.Generic.List<NFAStateSet> p_NFAStateSets = new System.Collections.Generic.List<NFAStateSet>();
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
                System.Collections.Generic.Dictionary<TerminalNFACharSpan, NFAStateSet> Transitions = p_NFAStateSets[i].GetTransitions();
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

        /// <summary>
        /// Create a new minimum DFA equivalent to the current one
        /// </summary>
        /// <returns>Return the minimum equivalent DFA</returns>
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

        /// <summary>
        /// Remove non-terminal states that cannot lead to a terminal state
        /// </summary>
        public void Prune()
        {
            System.Collections.Generic.Dictionary<DFAState, System.Collections.Generic.List<DFAState>> inverses = new System.Collections.Generic.Dictionary<DFAState, System.Collections.Generic.List<DFAState>>();
            System.Collections.Generic.List<DFAState> finals = new System.Collections.Generic.List<DFAState>();
            foreach (DFAState state in p_States)
            {
                foreach (DFAState next in state.Transitions.Values)
                {
                    if (!inverses.ContainsKey(next))
                        inverses.Add(next, new System.Collections.Generic.List<DFAState>());
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
                            System.Collections.Generic.List<TerminalNFACharSpan> keys = new System.Collections.Generic.List<TerminalNFACharSpan>(antecedent.Transitions.Keys);
                            foreach (TerminalNFACharSpan key in keys)
                                if (antecedent.Transitions[key] == state)
                                    antecedent.Transitions.Remove(key);
                        }
                    }
                }
            }
        }
    }
}