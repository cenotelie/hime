/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.Automata
{
    class NFA
    {
        private List<NFAState> states;
        private NFAState stateEntry;
        private NFAState stateExit;


        public static readonly CharSpan Epsilon = new CharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));
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

        NFA(DFA dfa)
        {
            states = new List<NFAState>();
            List<DFAState> dfaStates = new List<DFAState>(dfa.States);
            foreach (DFAState state in dfaStates)
                states.Add(new NFAState());
            for (int i = 0; i != dfaStates.Count; i++)
            {
                states[i].Final = dfaStates[i].Final;
                foreach (CharSpan transition in dfaStates[i].Transitions.Keys)
                    states[i].AddTransition(transition, states[dfaStates.IndexOf(dfaStates[i].Transitions[transition])]);
            }
            stateEntry = states[0];
        }

        public NFAState AddNewState()
        {
            NFAState state = new NFAState();
            states.Add(state);
            return state;
        }

        public NFA Clone() { return Clone(true); }
        public NFA Clone(bool keepFinals)
        {
            NFA copy = new NFA();

            // Create new states for copy, add marks and copy finals if required
            for (int i = 0; i != states.Count; i++)
            {
                NFAState state = new NFAState();
                state.Mark = states[i].Mark;
                if (keepFinals)
                    state.Final = states[i].Final;
                copy.states.Add(state);
            }
            // Make linkage
            for (int i = 0; i != states.Count; i++)
            {
                foreach (NFATransition transition in states[i].Transitions)
                    copy.states[i].AddTransition(transition.span, copy.states[states.IndexOf(transition.next)]);
            }
            if (stateEntry != null)
                copy.stateEntry = copy.states[states.IndexOf(stateEntry)];
            if (stateExit != null)
                copy.stateExit = copy.states[states.IndexOf(stateExit)];
            return copy;
        }

        public void InsertSubNFA(NFA sub)
        {
            states.AddRange(sub.states);
        }
		
		private static NFA BuildNFA()
		{
			NFA result = new NFA();
            result.stateEntry = result.AddNewState();
            result.stateExit = result.AddNewState();
			return result;
		}
		
		// TODO: should get rid of static methods
        public static NFA OperatorOption(NFA sub, bool useClones)
        {
            NFA final = BuildNFA();
            if (useClones)
                sub = sub.Clone();
            final.states.AddRange(sub.states);
            final.stateEntry.AddTransition(NFA.Epsilon, sub.stateEntry);
            final.stateEntry.AddTransition(NFA.Epsilon, final.stateExit);
            sub.stateExit.AddTransition(NFA.Epsilon, final.stateExit);
            return final;
        }
		// TODO: should get rid of static methods
		public static NFA OperatorStar(NFA sub, bool useClones)
        {
            NFA final = BuildNFA();
            if (useClones)
                sub = sub.Clone();
            final.states.AddRange(sub.states);
            final.stateEntry.AddTransition(NFA.Epsilon, sub.stateEntry);
            final.stateEntry.AddTransition(NFA.Epsilon, final.stateExit);
            sub.stateExit.AddTransition(NFA.Epsilon, final.stateExit);
            final.stateExit.AddTransition(NFA.Epsilon, sub.stateEntry);
            return final;
        }
        public static NFA OperatorPlus(NFA sub, bool useClones)
        {
            NFA final = BuildNFA();
            if (useClones)
                sub = sub.Clone();
            final.states.AddRange(sub.states);
            final.stateEntry.AddTransition(NFA.Epsilon, sub.stateEntry);
            sub.stateExit.AddTransition(NFA.Epsilon, final.stateExit);
            final.stateExit.AddTransition(NFA.Epsilon, sub.stateEntry);
            return final;
        }
        public static NFA OperatorRange(NFA sub, bool useClones, uint min, uint max)
        {
            NFA final = BuildNFA();

            NFAState last = final.stateEntry;
            for (uint i = 0; i != min; i++)
            {
                NFA inner = sub.Clone();
                final.states.AddRange(inner.states);
                last.AddTransition(NFA.Epsilon, inner.stateEntry);
                last = inner.stateExit;
            }
            for (uint i = min; i != max; i++)
            {
                NFA inner = OperatorOption(sub, true);
                final.states.AddRange(inner.states);
                last.AddTransition(NFA.Epsilon, inner.stateEntry);
                last = inner.stateExit;
            }
            last.AddTransition(NFA.Epsilon, final.stateExit);
            
            if (min == 0)
                final.stateEntry.AddTransition(NFA.Epsilon, final.stateExit);
            return final;
        }
		public static NFA OperatorUnion(NFA left, NFA right, bool useClones)
        {
            NFA final = BuildNFA();
            if (useClones)
            {
                left = left.Clone(true);
                right = right.Clone(true);
            }
            final.states.AddRange(left.states);
            final.states.AddRange(right.states);
            final.stateEntry.AddTransition(NFA.Epsilon, left.stateEntry);
            final.stateEntry.AddTransition(NFA.Epsilon, right.stateEntry);
            left.stateExit.AddTransition(NFA.Epsilon, final.stateExit);
            right.stateExit.AddTransition(NFA.Epsilon, final.stateExit);
            return final;
        }
        public static NFA OperatorConcat(NFA left, NFA right, bool useClones)
        {
            NFA final = new NFA();
            if (useClones)
            {
                left = left.Clone(true);
                right = right.Clone(true);
            }
            final.states.AddRange(left.states);
            final.states.AddRange(right.states);
            final.stateEntry = left.stateEntry;
            final.stateExit = right.stateExit;
            left.stateExit.AddTransition(NFA.Epsilon, right.stateEntry);
            return final;
        }
        public static NFA OperatorDifference(NFA left, NFA right, bool useClones)
        {
            NFA final = BuildNFA();
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
            final.stateEntry.AddTransition(NFA.Epsilon, left.stateEntry);
            final.stateEntry.AddTransition(NFA.Epsilon, right.stateEntry);
            left.stateExit.AddTransition(NFA.Epsilon, statePositive);
            right.stateExit.AddTransition(NFA.Epsilon, stateNegative);
            statePositive.AddTransition(NFA.Epsilon, final.stateExit);

            final.stateExit.Final = TerminalDummy.Instance;
            DFA equivalent = new DFA(final);
            equivalent.Prune();
            final = new NFA(equivalent);
            final.stateExit = final.AddNewState();
            foreach (NFAState state in final.states)
            {
                if (state.Final == TerminalDummy.Instance)
                {
                    state.Final = null;
                    state.AddTransition(NFA.Epsilon, final.stateExit);
                }
            }
            return final;
        }
    }
}
