using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    public struct TerminalNFACharSpan
    {
        private char p_SpanBegin;
        private char p_SpanEnd;

        public static TerminalNFACharSpan Null = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));

        public char Begin { get { return p_SpanBegin; } }
        public char End { get { return p_SpanEnd; } }
        public int Length { get { return p_SpanEnd - p_SpanBegin + 1; } }

        public TerminalNFACharSpan(char Begin, char End)
        {
            p_SpanBegin = Begin;
            p_SpanEnd = End;
        }

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

        public static int Compare(TerminalNFACharSpan Left, TerminalNFACharSpan Right) { return Left.p_SpanBegin.CompareTo(Right.p_SpanBegin); }

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

        public override bool Equals(object obj)
        {
            if (obj is TerminalNFACharSpan)
            {
                TerminalNFACharSpan Span = (TerminalNFACharSpan)obj;
                return ((p_SpanBegin == Span.p_SpanBegin) && (p_SpanEnd == Span.p_SpanEnd));
            }
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }




    public sealed class NFAState
    {
        private List<KeyValuePair<TerminalNFACharSpan, NFAState>> p_Transitions;
        private Terminal p_Final;
        private int p_Mark;

        public List<KeyValuePair<TerminalNFACharSpan, NFAState>> Transitions { get { return p_Transitions; } }
        public Terminal Final
        {
            get { return p_Final; }
            set { p_Final = value; }
        }
        public int Mark
        {
            get { return p_Mark; }
            set { p_Mark = value; }
        }

        public NFAState()
        {
            p_Transitions = new List<KeyValuePair<TerminalNFACharSpan, NFAState>>();
            p_Final = null;
            p_Mark = 0;
        }

        public void AddTransition(TerminalNFACharSpan Value, NFAState Next) { p_Transitions.Add(new KeyValuePair<TerminalNFACharSpan, NFAState>(Value, Next)); }
        public void ClearTransitions() { p_Transitions.Clear(); }
    }


    public sealed class NFA
    {
        private List<NFAState> p_States;
        private NFAState p_StateEntry;
        private NFAState p_StateExit;


        public static readonly TerminalNFACharSpan Epsilon = new TerminalNFACharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));
        public ICollection<NFAState> States { get { return p_States; } }
        public int StatesCount { get { return p_States.Count; } }
        public NFAState StateEntry
        {
            get { return p_StateEntry; }
            set { p_StateEntry = value; }
        }
        public NFAState StateExit
        {
            get { return p_StateExit; }
            set { p_StateExit = value; }
        }

        public NFA()
        {
            p_States = new List<NFAState>();
        }

        public NFA(DFA DFA)
        {
            p_States = new List<NFAState>();
            List<DFAState> DFAStates = new List<DFAState>(DFA.States);
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

        public NFAState AddNewState()
        {
            NFAState State = new NFAState();
            p_States.Add(State);
            return State;
        }

        public NFA Clone() { return Clone(true); }
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
                foreach (KeyValuePair<TerminalNFACharSpan, NFAState> Transition in p_States[i].Transitions)
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
