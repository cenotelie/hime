using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public struct TerminalNFACharSpan
    {
        private char spanBegin;
        private char spanEnd;

        public static TerminalNFACharSpan Null = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));

        public char Begin { get { return spanBegin; } }
        public char End { get { return spanEnd; } }
        public int Length { get { return spanEnd - spanBegin + 1; } }

        public TerminalNFACharSpan(char Begin, char End)
        {
            spanBegin = Begin;
            spanEnd = End;
        }

        public static TerminalNFACharSpan Intersect(TerminalNFACharSpan Left, TerminalNFACharSpan Right)
        {
            if (Left.spanBegin < Right.spanBegin)
            {
                if (Left.spanEnd < Right.spanBegin)
                    return Null;
                if (Left.spanEnd < Right.spanEnd)
                    return new TerminalNFACharSpan(Right.spanBegin, Left.spanEnd);
                return new TerminalNFACharSpan(Right.spanBegin, Right.spanEnd);
            }
            else
            {
                if (Right.spanEnd < Left.spanBegin)
                    return Null;
                if (Right.spanEnd < Left.spanEnd)
                    return new TerminalNFACharSpan(Left.spanBegin, Right.spanEnd);
                return new TerminalNFACharSpan(Left.spanBegin, Left.spanEnd);
            }
        }

        public static TerminalNFACharSpan Split(TerminalNFACharSpan Original, TerminalNFACharSpan Splitter, out TerminalNFACharSpan Rest)
        {
            if (Original.spanBegin == Splitter.spanBegin)
            {
                Rest = Null;
                if (Original.spanEnd == Splitter.spanEnd) return Null;
                return new TerminalNFACharSpan(System.Convert.ToChar(Splitter.spanEnd + 1), Original.spanEnd);
            }
            if (Original.spanEnd == Splitter.spanEnd)
            {
                Rest = Null;
                return new TerminalNFACharSpan(Original.spanBegin, System.Convert.ToChar(Splitter.spanBegin - 1));
            }
            Rest = new TerminalNFACharSpan(System.Convert.ToChar(Splitter.spanEnd + 1), Original.spanEnd);
            return new TerminalNFACharSpan(Original.spanBegin, System.Convert.ToChar(Splitter.spanBegin - 1));
        }

        public static int Compare(TerminalNFACharSpan Left, TerminalNFACharSpan Right) { return Left.spanBegin.CompareTo(Right.spanBegin); }

        public override string ToString()
        {
            if (spanBegin > spanEnd)
                return string.Empty;
            if (spanBegin == spanEnd)
                return CharToString(spanBegin);
            return "[" + CharToString(spanBegin) + "-" + CharToString(spanEnd) + "]";
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

        public override bool Equals(object obj)
        {
            if (obj is TerminalNFACharSpan)
            {
                TerminalNFACharSpan Span = (TerminalNFACharSpan)obj;
                return ((spanBegin == Span.spanBegin) && (spanEnd == Span.spanEnd));
            }
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }




    public sealed class NFAState
    {
        private List<KeyValuePair<TerminalNFACharSpan, NFAState>> transitions;
        private Terminal final;
        private int mark;

        public List<KeyValuePair<TerminalNFACharSpan, NFAState>> Transitions { get { return transitions; } }
        public Terminal Final
        {
            get { return final; }
            set { final = value; }
        }
        public int Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public NFAState()
        {
            transitions = new List<KeyValuePair<TerminalNFACharSpan, NFAState>>();
            final = null;
            mark = 0;
        }

        public void AddTransition(TerminalNFACharSpan Value, NFAState Next) { transitions.Add(new KeyValuePair<TerminalNFACharSpan, NFAState>(Value, Next)); }
        public void ClearTransitions() { transitions.Clear(); }
    }


    public sealed class NFA
    {
        private List<NFAState> states;
        private NFAState stateEntry;
        private NFAState stateExit;


        public static readonly TerminalNFACharSpan Epsilon = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));
        public ICollection<NFAState> States { get { return states; } }
        public int StatesCount { get { return states.Count; } }
        public NFAState StateEntry
        {
            get { return stateEntry; }
            set { stateEntry = value; }
        }
        public NFAState StateExit
        {
            get { return stateExit; }
            set { stateExit = value; }
        }

        public NFA()
        {
            states = new List<NFAState>();
        }

        public NFA(DFA DFA)
        {
            states = new List<NFAState>();
            List<DFAState> DFAStates = new List<DFAState>(DFA.States);
            foreach (DFAState State in DFAStates)
                states.Add(new NFAState());
            for (int i = 0; i != DFAStates.Count; i++)
            {
                states[i].Final = DFAStates[i].Final;
                foreach (TerminalNFACharSpan Transition in DFAStates[i].Transitions.Keys)
                    states[i].AddTransition(Transition, states[DFAStates.IndexOf(DFAStates[i].Transitions[Transition])]);
            }
            stateEntry = states[0];
        }

        public NFAState AddNewState()
        {
            NFAState State = new NFAState();
            states.Add(State);
            return State;
        }

        public NFA Clone() { return Clone(true); }
        public NFA Clone(bool KeepFinals)
        {
            NFA Copy = new NFA();

            // Create new states for copy, add marks and copy finals if required
            for (int i = 0; i != states.Count; i++)
            {
                NFAState State = new NFAState();
                State.Mark = states[i].Mark;
                if (KeepFinals)
                    State.Final = states[i].Final;
                Copy.states.Add(State);
            }
            // Make linkage
            for (int i = 0; i != states.Count; i++)
            {
                foreach (KeyValuePair<TerminalNFACharSpan, NFAState> Transition in states[i].Transitions)
                    Copy.states[i].AddTransition(Transition.Key, Copy.states[states.IndexOf(Transition.Value)]);
            }
            if (stateEntry != null)
                Copy.stateEntry = Copy.states[states.IndexOf(stateEntry)];
            if (stateExit != null)
                Copy.stateExit = Copy.states[states.IndexOf(stateExit)];
            return Copy;
        }

        public void InsertSubNFA(NFA Sub)
        {
            states.AddRange(Sub.states);
        }


        public static NFA OperatorOption(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.stateEntry = new NFAState();
            Final.stateExit = new NFAState();
            Final.states.Add(Final.stateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.states.AddRange(Sub.states);
            Final.states.Add(Final.stateExit);
            Final.stateEntry.AddTransition(NFA.Epsilon, Sub.stateEntry);
            Final.stateEntry.AddTransition(NFA.Epsilon, Final.stateExit);
            Sub.stateExit.AddTransition(NFA.Epsilon, Final.stateExit);
            return Final;
        }
        public static NFA OperatorStar(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.stateEntry = new NFAState();
            Final.stateExit = new NFAState();
            Final.states.Add(Final.stateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.states.AddRange(Sub.states);
            Final.states.Add(Final.stateExit);
            Final.stateEntry.AddTransition(NFA.Epsilon, Sub.stateEntry);
            Final.stateEntry.AddTransition(NFA.Epsilon, Final.stateExit);
            Sub.stateExit.AddTransition(NFA.Epsilon, Final.stateExit);
            Final.stateExit.AddTransition(NFA.Epsilon, Sub.stateEntry);
            return Final;
        }
        public static NFA OperatorPlus(NFA Sub, bool UseClones)
        {
            NFA Final = new NFA();
            Final.stateEntry = new NFAState();
            Final.stateExit = new NFAState();
            Final.states.Add(Final.stateEntry);
            if (UseClones)
                Sub = Sub.Clone();
            Final.states.AddRange(Sub.states);
            Final.states.Add(Final.stateExit);
            Final.stateEntry.AddTransition(NFA.Epsilon, Sub.stateEntry);
            Sub.stateExit.AddTransition(NFA.Epsilon, Final.stateExit);
            Final.stateExit.AddTransition(NFA.Epsilon, Sub.stateEntry);
            return Final;
        }
        public static NFA OperatorRange(NFA Sub, bool UseClones, uint Min, uint Max)
        {
            NFA Final = new NFA();
            Final.stateEntry = new NFAState();
            Final.stateExit = new NFAState();
            Final.states.Add(Final.stateEntry);

            NFAState Last = Final.stateEntry;
            for (uint i = 0; i != Min; i++)
            {
                NFA Inner = Sub.Clone();
                Final.states.AddRange(Inner.states);
                Last.AddTransition(NFA.Epsilon, Inner.stateEntry);
                Last = Inner.stateExit;
            }
            for (uint i = Min; i != Max; i++)
            {
                NFA Inner = OperatorOption(Sub, true);
                Final.states.AddRange(Inner.states);
                Last.AddTransition(NFA.Epsilon, Inner.stateEntry);
                Last = Inner.stateExit;
            }
            Final.states.Add(Final.stateExit);
            Last.AddTransition(NFA.Epsilon, Final.stateExit);
            
            if (Min == 0)
                Final.stateEntry.AddTransition(NFA.Epsilon, Final.stateExit);
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
            Final.states.AddRange(Left.states);
            Final.states.AddRange(Right.states);
            Final.stateEntry = Left.stateEntry;
            Final.stateExit = Right.stateExit;
            Left.stateExit.AddTransition(NFA.Epsilon, Right.stateEntry);
            return Final;
        }
        public static NFA OperatorDifference(NFA Left, NFA Right, bool UseClones)
        {
            NFA Final = new NFA();
            Final.stateEntry = Final.AddNewState();
            Final.stateExit = Final.AddNewState();
            NFAState StatePositive = Final.AddNewState();
            NFAState StateNegative = Final.AddNewState();
            StatePositive.Mark = 1;
            StateNegative.Mark = -1;

            if (UseClones)
            {
                Left = Left.Clone(true);
                Right = Right.Clone(true);
            }
            Final.states.AddRange(Left.states);
            Final.states.AddRange(Right.states);
            Final.stateEntry.AddTransition(NFA.Epsilon, Left.stateEntry);
            Final.stateEntry.AddTransition(NFA.Epsilon, Right.stateEntry);
            Left.stateExit.AddTransition(NFA.Epsilon, StatePositive);
            Right.stateExit.AddTransition(NFA.Epsilon, StateNegative);
            StatePositive.AddTransition(NFA.Epsilon, Final.stateExit);

            Final.stateExit.Final = TerminalDummy.Instance;
            DFA Equivalent = new DFA(Final);
            Equivalent.Prune();
            Final = new NFA(Equivalent);
            Final.stateExit = Final.AddNewState();
            foreach (NFAState State in Final.states)
            {
                if (State.Final == TerminalDummy.Instance)
                {
                    State.Final = null;
                    State.AddTransition(NFA.Epsilon, Final.stateExit);
                }
            }
            return Final;
        }
        public static NFA OperatorUnion(NFA Left, NFA Right, bool UseClones)
        {
            NFA Final = new NFA();
            Final.stateEntry = new NFAState();
            Final.stateExit = new NFAState();
            Final.states.Add(Final.stateEntry);
            if (UseClones)
            {
                Left = Left.Clone(true);
                Right = Right.Clone(true);
            }
            Final.states.AddRange(Left.states);
            Final.states.AddRange(Right.states);
            Final.states.Add(Final.stateExit);
            Final.stateEntry.AddTransition(NFA.Epsilon, Left.stateEntry);
            Final.stateEntry.AddTransition(NFA.Epsilon, Right.stateEntry);
            Left.stateExit.AddTransition(NFA.Epsilon, Final.stateExit);
            Right.stateExit.AddTransition(NFA.Epsilon, Final.stateExit);
            return Final;
        }
    }


    public sealed class NFAStateSet : List<NFAState>
    {
        public new void Add(NFAState item)
        {
            if (!base.Contains(item))
                base.Add(item);
        }
        public new void AddRange(IEnumerable<NFAState> items)
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
                foreach (KeyValuePair<TerminalNFACharSpan, NFAState> Transition in this[i].Transitions)
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
                foreach (KeyValuePair<TerminalNFACharSpan, NFAState> T in StatePositive.Transitions)
                    if (T.Key.Equals(NFA.Epsilon))
                        this.Remove(T.Value);
        }

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
                                    this[s1].Transitions[t1] = new KeyValuePair<TerminalNFACharSpan, NFAState>(Inter, this[s1].Transitions[t1].Value);
                                    if (Part1.Length != 0) this[s1].AddTransition(Part1, this[s1].Transitions[t1].Value);
                                    if (Part2.Length != 0) this[s1].AddTransition(Part2, this[s1].Transitions[t1].Value);

                                    // Split transition2 in 1, 2 or 3 transitions and modifiy the states accordingly
                                    Part1 = TerminalNFACharSpan.Split(this[s2].Transitions[t2].Key, Inter, out Part2);
                                    this[s2].Transitions[t2] = new KeyValuePair<TerminalNFACharSpan, NFAState>(Inter, this[s2].Transitions[t2].Value);
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

        public Dictionary<TerminalNFACharSpan, NFAStateSet> GetTransitions()
        {
            Dictionary<TerminalNFACharSpan, NFAStateSet> Transitions = new Dictionary<TerminalNFACharSpan, NFAStateSet>();
            // For each state
            foreach (NFAState State in this)
            {
                // For each transition
                foreach (KeyValuePair<TerminalNFACharSpan, NFAState> Transition in State.Transitions)
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

        public List<Terminal> GetFinals()
        {
            List<Terminal> Finals = new List<Terminal>();
            foreach (NFAState State in this)
                if (State.Final != null)
                    Finals.Add(State.Final);
            return Finals;
        }

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
