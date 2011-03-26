namespace Hime.Parsers.Automata
{
    /// <summary>
    /// Represents a span of characters used as a transition in a terminal automaton.
    /// </summary>
    public struct TerminalNFACharSpan
    {
        /// <summary>
        /// Beginning of the span
        /// </summary>
        private char p_SpanBegin;
        /// <summary>
        /// End of the span
        /// </summary>
        private char p_SpanEnd;

        /// <summary>
        /// Defines the null value for a span.
        /// </summary>
        public static TerminalNFACharSpan Null = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));

        /// <summary>
        /// Get the span's begin
        /// </summary>
        /// <value>Span's begin</value>
        public char Begin { get { return p_SpanBegin; } }
        /// <summary>
        /// Get the span's end
        /// </summary>
        /// <value>The span's end</value>
        public char End { get { return p_SpanEnd; } }
        /// <summary>
        /// Get the span's length
        /// </summary>
        /// <value>The number of characters in the span.</value>
        /// <remarks>This value is 0 for the null span.</remarks>
        public int Length { get { return p_SpanEnd - p_SpanBegin + 1; } }

        /// <summary>
        /// Creates and initialize a new span
        /// </summary>
        /// <param name="Begin">The span's beginning character</param>
        /// <param name="End">The span's end character</param>
        public TerminalNFACharSpan(char Begin, char End)
        {
            p_SpanBegin = Begin;
            p_SpanEnd = End;
        }

        /// <summary>
        /// Get the intersection between two spans
        /// </summary>
        /// <param name="Left">The left span</param>
        /// <param name="Right">The right span</param>
        /// <returns>Returns the intersection span that may be the Null span value if the two spans do not intersect</returns>
        public static TerminalNFACharSpan Intersect(TerminalNFACharSpan Left, TerminalNFACharSpan Right)
        {
            if (Left.p_SpanBegin < Right.p_SpanBegin)
            {
                if (Left.p_SpanEnd < Right.p_SpanBegin)
                    return Null;
                if (Left.p_SpanEnd < Right.p_SpanEnd)
                    return new TerminalNFACharSpan(Right.p_SpanBegin, Left.p_SpanEnd);
                return new TerminalNFACharSpan(Right.p_SpanBegin, Right.p_SpanEnd);
            }
            else
            {
                if (Right.p_SpanEnd < Left.p_SpanBegin)
                    return Null;
                if (Right.p_SpanEnd < Left.p_SpanEnd)
                    return new TerminalNFACharSpan(Left.p_SpanBegin, Right.p_SpanEnd);
                return new TerminalNFACharSpan(Left.p_SpanBegin, Left.p_SpanEnd);
            }
        }

        /// <summary>
        /// Split the Original span into 1 or 2 pieces according to the Splitter span
        /// </summary>
        /// <param name="Original">The span to split</param>
        /// <param name="Splitter">The splitter span</param>
        /// <param name="Rest">The possibly second piece</param>
        /// <returns>Returns the first piece that is contained by the Original</returns>
        /// <remarks>
        /// The function assumes that the Splitter span is contained by the Original span.
        /// The function will returns two pieces P1 and P2, so that: P1 + Splitter + P2 = Original.
        /// P1 and P2 my be empty. Note that if P1 is empty, P2 is empty and Splitter = Original.
        /// P1 is the value returned by the function and P2 is the out parameter Rest.
        /// </remarks>
        public static TerminalNFACharSpan Split(TerminalNFACharSpan Original, TerminalNFACharSpan Splitter, out TerminalNFACharSpan Rest)
        {
            if (Original.p_SpanBegin == Splitter.p_SpanBegin)
            {
                Rest = Null;
                if (Original.p_SpanEnd == Splitter.p_SpanEnd) return Null;
                return new TerminalNFACharSpan(System.Convert.ToChar(Splitter.p_SpanEnd + 1), Original.p_SpanEnd);
            }
            if (Original.p_SpanEnd == Splitter.p_SpanEnd)
            {
                Rest = Null;
                return new TerminalNFACharSpan(Original.p_SpanBegin, System.Convert.ToChar(Splitter.p_SpanBegin - 1));
            }
            Rest = new TerminalNFACharSpan(System.Convert.ToChar(Splitter.p_SpanEnd + 1), Original.p_SpanEnd);
            return new TerminalNFACharSpan(Original.p_SpanBegin, System.Convert.ToChar(Splitter.p_SpanBegin - 1));
        }

        /// <summary>
        /// Compare two spans according to their respective beginning's values
        /// </summary>
        /// <param name="Left">The left span</param>
        /// <param name="Right">The right span</param>
        /// <returns>Returns the comparison value between the two beginning characters.</returns>
        public static int Compare(TerminalNFACharSpan Left, TerminalNFACharSpan Right) { return Left.p_SpanBegin.CompareTo(Right.p_SpanBegin); }

        /// <summary>
        /// Override the ToString function
        /// </summary>
        /// <returns>Returns a string of the form [B-E] where B is the beginning character and E the end character</returns>
        public override string ToString()
        {
            if (p_SpanBegin > p_SpanEnd)
                return string.Empty;
            if (p_SpanBegin == p_SpanEnd)
                return CharToString(p_SpanBegin);
            return "[" + CharToString(p_SpanBegin) + "-" + CharToString(p_SpanEnd) + "]";
        }

        private string CharToString(char c)
        {
            System.Globalization.UnicodeCategory cat = char.GetUnicodeCategory(c);
            switch (cat)
            {
                case System.Globalization.UnicodeCategory.ModifierLetter:
                case System.Globalization.UnicodeCategory.NonSpacingMark:
                case System.Globalization.UnicodeCategory.SpacingCombiningMark:
                case System.Globalization.UnicodeCategory.EnclosingMark:
                case System.Globalization.UnicodeCategory.SpaceSeparator:
                case System.Globalization.UnicodeCategory.LineSeparator:
                case System.Globalization.UnicodeCategory.ParagraphSeparator:
                case System.Globalization.UnicodeCategory.Control:
                case System.Globalization.UnicodeCategory.Format:
                case System.Globalization.UnicodeCategory.Surrogate:
                case System.Globalization.UnicodeCategory.PrivateUse:
                case System.Globalization.UnicodeCategory.OtherNotAssigned:
                    return CharToString_NonPrintable(c);
                default:
                    return c.ToString();
            }
        }
        private string CharToString_NonPrintable(char c)
        {
            string result = "U+" + System.Convert.ToUInt16(c).ToString("X");
            return result;
        }

        /// <summary>
        /// Override the Equals function 
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>Returns true of obj is a span and is equal to the current one, returns false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is TerminalNFACharSpan)
            {
                TerminalNFACharSpan Span = (TerminalNFACharSpan)obj;
                return ((p_SpanBegin == Span.p_SpanBegin) && (p_SpanEnd == Span.p_SpanEnd));
            }
            return false;
        }
        /// <summary>
        /// Override the GetHashCode function
        /// </summary>
        /// <returns>Returns the value from the base's function</returns>
        public override int GetHashCode() { return base.GetHashCode(); }
    }




    /// <summary>
    /// Represents a NFA state for a terminal automata
    /// </summary>
    public sealed class NFAState
    {
        /// <summary>
        /// List of the transitions from the current state
        /// </summary>
        /// <remarks>A transition is a couple (span, state) where the span is the transition value and the state the target state at the end of the transition</remarks>
        private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>> p_Transitions;
        /// <summary>
        /// Terminal symbol that may be recognize at this state
        /// null meaning the state is not final
        /// </summary>
        private Terminal p_Final;
        /// <summary>
        /// Mark for the state
        /// 0 meaning the state is not marked
        /// </summary>
        private int p_Mark;

        /// <summary>
        /// List of the transitions from the current state
        /// </summary>
        /// <remarks>A transition is a couple (span, state) where the span is the transition value and the state the target state at the end of the transition</remarks>
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>> Transitions { get { return p_Transitions; } }
        /// <summary>
        /// Get or set the terminal symbol that may be recognize at this state
        /// null meaning the state is not final
        /// </summary>
        /// <value>Terminal symbol that may be recognize at this state</value>
        public Terminal Final
        {
            get { return p_Final; }
            set { p_Final = value; }
        }
        /// <summary>
        /// Get or set the mark for the state
        /// 0 meaning the state is not marked
        /// </summary>
        /// <value>The mark for the state</value>
        public int Mark
        {
            get { return p_Mark; }
            set { p_Mark = value; }
        }

        /// <summary>
        /// Construct the state
        /// </summary>
        public NFAState()
        {
            p_Transitions = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>>();
            p_Final = null;
            p_Mark = 0;
        }

        /// <summary>
        /// Add a new transition from this state
        /// </summary>
        /// <param name="Value">The value of the transition</param>
        /// <param name="Next">The target for the transition</param>
        public void AddTransition(TerminalNFACharSpan Value, NFAState Next) { p_Transitions.Add(new System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>(Value, Next)); }
        /// <summary>
        /// Remove all transitions from the state
        /// </summary>
        public void ClearTransitions() { p_Transitions.Clear(); }
    }


    /// <summary>
    /// Represents a terminal NFA
    /// </summary>
    public sealed class NFA
    {
        /// <summary>
        /// Collection of NFA states
        /// </summary>
        private System.Collections.Generic.List<NFAState> p_States;
        /// <summary>
        /// Entry state for the NFA
        /// </summary>
        private NFAState p_StateEntry;
        /// <summary>
        /// Exist state for the NFA
        /// </summary>
        private NFAState p_StateExit;


        /// <summary>
        /// Epsilon value for the ε-transitions
        /// </summary>
        public static readonly TerminalNFACharSpan Epsilon = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));
        /// <summary>
        /// Get an enumeration of the states in the NFA
        /// </summary>
        /// <value>An enumeration of the states in the NFA</value>
        public System.Collections.Generic.ICollection<NFAState> States { get { return p_States; } }
        /// <summary>
        /// Get the number of states in the NFA
        /// </summary>
        /// <value>The number of states in the NFA</value>
        public int StatesCount { get { return p_States.Count; } }
        /// <summary>
        /// Get or set the entry state for the NFA
        /// </summary>
        /// <value>The entry state for the NFA</value>
        public NFAState StateEntry
        {
            get { return p_StateEntry; }
            set { p_StateEntry = value; }
        }
        /// <summary>
        /// Get or set the exit state for the NFA
        /// </summary>
        /// <value>The exit state for the NFA</value>
        public NFAState StateExit
        {
            get { return p_StateExit; }
            set { p_StateExit = value; }
        }

        /// <summary>
        /// Construct an empty NFA
        /// </summary>
        public NFA()
        {
            p_States = new System.Collections.Generic.List<NFAState>();
        }

        public NFA(DFA DFA)
        {
            p_States = new System.Collections.Generic.List<NFAState>();
            System.Collections.Generic.List<DFAState> DFAStates = new System.Collections.Generic.List<DFAState>(DFA.States);
            foreach (DFAState State in DFAStates)
                p_States.Add(new NFAState());
            for (int i = 0; i != DFAStates.Count; i++)
            {
                p_States[i].Final = DFAStates[i].Final;
                foreach (TerminalNFACharSpan Transition in DFAStates[i].Transitions.Keys)
                    p_States[i].AddTransition(Transition, p_States[DFAStates.IndexOf(DFAStates[i].Transitions[Transition])]);
            }
            p_StateEntry = p_States[0];
        }

        /// <summary>
        /// Create a new state in the NFA and return it
        /// </summary>
        /// <returns>Returns the new state</returns>
        public NFAState AddNewState()
        {
            NFAState State = new NFAState();
            p_States.Add(State);
            return State;
        }

        /// <summary>
        /// Clone the current NFA and return the clone keeping the final informations
        /// </summary>
        /// <returns>Returns the clone</returns>
        public NFA Clone() { return Clone(true); }
        /// <summary>
        /// Clone the current NFA
        /// </summary>
        /// <param name="KeepFinals">Keep the final information</param>
        /// <returns>Returns the clone</returns>
        public NFA Clone(bool KeepFinals)
        {
            NFA Copy = new NFA();

            // Create new states for copy, add marks and copy finals if required
            for (int i = 0; i != p_States.Count; i++)
            {
                NFAState State = new NFAState();
                State.Mark = p_States[i].Mark;
                if (KeepFinals)
                    State.Final = p_States[i].Final;
                Copy.p_States.Add(State);
            }
            // Make linkage
            for (int i = 0; i != p_States.Count; i++)
            {
                foreach (System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState> Transition in p_States[i].Transitions)
                    Copy.p_States[i].AddTransition(Transition.Key, Copy.p_States[p_States.IndexOf(Transition.Value)]);
            }
            if (p_StateEntry != null)
                Copy.p_StateEntry = Copy.p_States[p_States.IndexOf(p_StateEntry)];
            if (p_StateExit != null)
                Copy.p_StateExit = Copy.p_States[p_States.IndexOf(p_StateExit)];
            return Copy;
        }

        public void InsertSubNFA(NFA Sub)
        {
            p_States.AddRange(Sub.p_States);
        }


        public static NFA OperatorOption(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = new NFAState();
            Final.p_StateExit = new NFAState();
            Final.p_States.Add(Final.p_StateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.p_States.AddRange(Sub.p_States);
            Final.p_States.Add(Final.p_StateExit);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Sub.p_StateEntry);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Final.p_StateExit);
            Sub.p_StateExit.AddTransition(NFA.Epsilon, Final.p_StateExit);
            return Final;
        }
        public static NFA OperatorStar(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = new NFAState();
            Final.p_StateExit = new NFAState();
            Final.p_States.Add(Final.p_StateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.p_States.AddRange(Sub.p_States);
            Final.p_States.Add(Final.p_StateExit);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Sub.p_StateEntry);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Final.p_StateExit);
            Sub.p_StateExit.AddTransition(NFA.Epsilon, Final.p_StateExit);
            Final.p_StateExit.AddTransition(NFA.Epsilon, Sub.p_StateEntry);
            return Final;
        }
        public static NFA OperatorPlus(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = new NFAState();
            Final.p_StateExit = new NFAState();
            Final.p_States.Add(Final.p_StateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.p_States.AddRange(Sub.p_States);
            Final.p_States.Add(Final.p_StateExit);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Sub.p_StateEntry);
            Sub.p_StateExit.AddTransition(NFA.Epsilon, Final.p_StateExit);
            Final.p_StateExit.AddTransition(NFA.Epsilon, Sub.p_StateEntry);
            return Final;
        }
        public static NFA OperatorRange(NFA Sub, bool UseClones, uint Min, uint Max)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = new NFAState();
            Final.p_StateExit = new NFAState();
            Final.p_States.Add(Final.p_StateEntry);

            NFAState Last = Final.p_StateEntry;
            for (uint i = 0; i != Min; i++)
            {
                NFA Inner = Sub.Clone();
                Final.p_States.AddRange(Inner.p_States);
                Last.AddTransition(NFA.Epsilon, Inner.p_StateEntry);
                Last = Inner.p_StateExit;
            }
            for (uint i = Min; i != Max; i++)
            {
                NFA Inner = OperatorOption(Sub, true);
                Final.p_States.AddRange(Inner.p_States);
                Last.AddTransition(NFA.Epsilon, Inner.p_StateEntry);
                Last = Inner.p_StateExit;
            }
            Final.p_States.Add(Final.p_StateExit);
            Last.AddTransition(NFA.Epsilon, Final.p_StateExit);
            
            if (Min == 0)
                Final.p_StateEntry.AddTransition(NFA.Epsilon, Final.p_StateExit);
            return Final;
        }
        public static NFA OperatorConcat(NFA Left, NFA Right, bool UseClones)
        {
            NFA Final = new NFA();
            if (UseClones)
            {
                Left = Left.Clone(true);
                Right = Right.Clone(true);
            }
            Final.p_States.AddRange(Left.p_States);
            Final.p_States.AddRange(Right.p_States);
            Final.p_StateEntry = Left.p_StateEntry;
            Final.p_StateExit = Right.p_StateExit;
            Left.p_StateExit.AddTransition(NFA.Epsilon, Right.p_StateEntry);
            return Final;
        }
        public static NFA OperatorDifference(NFA Left, NFA Right, bool UseClones)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = Final.AddNewState();
            Final.p_StateExit = Final.AddNewState();
            NFAState StatePositive = Final.AddNewState();
            NFAState StateNegative = Final.AddNewState();
            StatePositive.Mark = 1;
            StateNegative.Mark = -1;

            if (UseClones)
            {
                Left = Left.Clone(true);
                Right = Right.Clone(true);
            }
            Final.p_States.AddRange(Left.p_States);
            Final.p_States.AddRange(Right.p_States);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Left.p_StateEntry);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Right.p_StateEntry);
            Left.p_StateExit.AddTransition(NFA.Epsilon, StatePositive);
            Right.p_StateExit.AddTransition(NFA.Epsilon, StateNegative);
            StatePositive.AddTransition(NFA.Epsilon, Final.p_StateExit);

            Final.p_StateExit.Final = TerminalDummy.Instance;
            DFA Equivalent = new DFA(Final);
            Equivalent.Prune();
            Final = new NFA(Equivalent);
            Final.p_StateExit = Final.AddNewState();
            foreach (NFAState State in Final.p_States)
            {
                if (State.Final == TerminalDummy.Instance)
                {
                    State.Final = null;
                    State.AddTransition(NFA.Epsilon, Final.p_StateExit);
                }
            }
            return Final;
        }
        public static NFA OperatorUnion(NFA Left, NFA Right, bool UseClones)
        {
            NFA Final = new NFA();
            Final.p_StateEntry = new NFAState();
            Final.p_StateExit = new NFAState();
            Final.p_States.Add(Final.p_StateEntry);
            if (UseClones)
            {
                Left = Left.Clone(true);
                Right = Right.Clone(true);
            }
            Final.p_States.AddRange(Left.p_States);
            Final.p_States.AddRange(Right.p_States);
            Final.p_States.Add(Final.p_StateExit);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Left.p_StateEntry);
            Final.p_StateEntry.AddTransition(NFA.Epsilon, Right.p_StateEntry);
            Left.p_StateExit.AddTransition(NFA.Epsilon, Final.p_StateExit);
            Right.p_StateExit.AddTransition(NFA.Epsilon, Final.p_StateExit);
            return Final;
        }
    }


    /// <summary>
    /// Represents a set of NFA states.
    /// </summary>
    /// <remarks>This is used to build a DFA equivalent to a NFA</remarks>
    public sealed class NFAStateSet : System.Collections.Generic.List<NFAState>
    {
        /// <summary>
        /// Add the given item to the set if not already present
        /// </summary>
        /// <param name="item">The item to add</param>
        public new void Add(NFAState item)
        {
            if (!base.Contains(item))
                base.Add(item);
        }
        /// <summary>
        /// Add a collection of items to the set if not already present
        /// </summary>
        /// <param name="items">The items to add</param>
        public new void AddRange(System.Collections.Generic.IEnumerable<NFAState> items)
        {
            foreach (NFAState item in items)
            {
                if (!base.Contains(item))
                    base.Add(item);
            }
        }


        private void Close_Normal()
        {
            for (int i = 0; i != Count; i++)
                foreach (System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState> Transition in this[i].Transitions)
                    if (Transition.Key.Equals(NFA.Epsilon))
                        this.Add(Transition.Value);
        }
        public void Close()
        {
            // Close the set
            Close_Normal();

            NFAState StatePositive = null;
            NFAState StateNegative = null;
            foreach (NFAState State in this)
            {
                if (State.Mark > 0) StatePositive = State;
                if (State.Mark < 0) StateNegative = State;
            }
            if (StatePositive != null && StateNegative != null)
                foreach (System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState> T in StatePositive.Transitions)
                    if (T.Key.Equals(NFA.Epsilon))
                        this.Remove(T.Value);
        }

        /// <summary>
        /// Normalize the current set
        /// </summary>
        /// <remarks>
        /// Ensure that:
        /// for each couple of transition (t1, t2), t1 and t2 are two different transitions from any NFA states in the set
        /// -> t1's span == t2's span
        ///     or
        /// -> Intersection(t1's span, t2's span) == Empty.
        /// </remarks>
        public void Normalize()
        {
            // Trace if modification has occured
            bool Modify = true;
            // Repeat while modifications occured
            while (Modify)
            {
                Modify = false;
                // For each NFA state in the set
                for (int s1 = 0; s1 != this.Count; s1++)
                {
                    // For each transition in this NFA state Set[s1]
                    for (int t1 = 0; t1 != this[s1].Transitions.Count; t1++)
                    {
                        // If this is an ε transition, go to next transition
                        if (this[s1].Transitions[t1].Key.Equals(NFA.Epsilon))
                            continue;
                        //Confront to each transition in each NFA state of the set
                        for (int s2 = 0; s2 != this.Count; s2++)
                        {
                            for (int t2 = 0; t2 != this[s2].Transitions.Count; t2++)
                            {
                                if (this[s2].Transitions[t2].Key.Equals(NFA.Epsilon))
                                    continue;
                                // If these are not the same transitions of the same state
                                if ((s1 != s2) || (t1 != t2))
                                {
                                    // If the two transition are equal : do nothing
                                    if (this[s1].Transitions[t1].Key.Equals(this[s2].Transitions[t2].Key))
                                        continue;
                                    // Get the intersection of the two spans
                                    TerminalNFACharSpan Inter = TerminalNFACharSpan.Intersect(this[s1].Transitions[t1].Key, this[s2].Transitions[t2].Key);
                                    // If no intersection : do nothing
                                    if (Inter.Length == 0)
                                        continue;

                                    // Split transition1 in 1, 2 or 3 transitions and modifiy the states accordingly
                                    TerminalNFACharSpan Part1;
                                    TerminalNFACharSpan Part2;
                                    Part1 = TerminalNFACharSpan.Split(this[s1].Transitions[t1].Key, Inter, out Part2);
                                    this[s1].Transitions[t1] = new System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>(Inter, this[s1].Transitions[t1].Value);
                                    if (Part1.Length != 0) this[s1].AddTransition(Part1, this[s1].Transitions[t1].Value);
                                    if (Part2.Length != 0) this[s1].AddTransition(Part2, this[s1].Transitions[t1].Value);

                                    // Split transition2 in 1, 2 or 3 transitions and modifiy the states accordingly
                                    Part1 = TerminalNFACharSpan.Split(this[s2].Transitions[t2].Key, Inter, out Part2);
                                    this[s2].Transitions[t2] = new System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState>(Inter, this[s2].Transitions[t2].Value);
                                    if (Part1.Length != 0) this[s2].AddTransition(Part1, this[s2].Transitions[t2].Value);
                                    if (Part2.Length != 0) this[s2].AddTransition(Part2, this[s2].Transitions[t2].Value);
                                    Modify = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compute the children and transition table for the current set
        /// </summary>
        /// <returns>Returns the transition table</returns>
        public System.Collections.Generic.Dictionary<TerminalNFACharSpan, NFAStateSet> GetTransitions()
        {
            System.Collections.Generic.Dictionary<TerminalNFACharSpan, NFAStateSet> Transitions = new System.Collections.Generic.Dictionary<TerminalNFACharSpan, NFAStateSet>();
            // For each state
            foreach (NFAState State in this)
            {
                // For each transition
                foreach (System.Collections.Generic.KeyValuePair<TerminalNFACharSpan, NFAState> Transition in State.Transitions)
                {
                    // If this is an ε-transition : pass
                    if (Transition.Key.Equals(NFA.Epsilon))
                        continue;
                    // Add the transition's target to set's transitions dictionnary
                    if (Transitions.ContainsKey(Transition.Key))
                        Transitions[Transition.Key].Add(Transition.Value);
                    else
                    {
                        // Create a new child
                        NFAStateSet Set = new NFAStateSet();
                        Set.Add(Transition.Value);
                        Transitions.Add(Transition.Key, Set);
                    }
                }
            }
            // Close all children
            foreach (NFAStateSet Set in Transitions.Values)
                Set.Close();
            return Transitions;
        }

        /// <summary>
        /// Get the list of the terminal symbols in the current set
        /// </summary>
        /// <returns>The list of the terminal symbols in the current set</returns>
        public System.Collections.Generic.List<Terminal> GetFinals()
        {
            System.Collections.Generic.List<Terminal> Finals = new System.Collections.Generic.List<Terminal>();
            foreach (NFAState State in this)
                if (State.Final != null)
                    Finals.Add(State.Final);
            return Finals;
        }

        /// <summary>
        /// Define equality between two sets
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns true if the two operand have the same NFA states within, false otherwise</returns>
        public static bool operator ==(NFAStateSet Left, NFAStateSet Right)
        {
            if (Left.Count != Right.Count)
                return false;
            foreach (NFAState S in Left)
            {
                if (!Right.Contains(S))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Define difference between two sets
        /// </summary>
        /// <param name="Left">Left operand</param>
        /// <param name="Right">Right operand</param>
        /// <returns>Returns false if the two operand have the same NFA states within, true otherwise</returns>
        public static bool operator !=(NFAStateSet Left, NFAStateSet Right)
        {
            if (Left.Count != Right.Count)
                return false;
            foreach (NFAState S in Left)
            {
                if (!Right.Contains(S))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Override equality between objects
        /// </summary>
        /// <param name="obj">The object to test</param>
        /// <returns>Returns true if obj is a NFA state set and is equal to the current one</returns>
        public override bool Equals(object obj)
        {
            if (obj is NFAStateSet)
            {
                NFAStateSet Set = (NFAStateSet)obj;
                return (this == Set);
            }
            else
                return false;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
    }
}