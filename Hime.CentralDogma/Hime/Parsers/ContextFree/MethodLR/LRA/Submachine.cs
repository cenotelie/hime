using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class SubMachine
    {
        private List<SubState> states;
        private Dictionary<CFRule, SubState> decideRules;
        private Dictionary<State, SubState> decideShifts;
        public ICollection<SubState> States { get { return states; } }

        public SubMachine()
        {
            states = new List<SubState>();
            decideRules = new Dictionary<CFRule, SubState>();
            decideShifts = new Dictionary<State, SubState>();
            states.Add(new SubState());
        }

        public void AddDeciderGraph(DeciderGraph graph)
        {
            List<SubState> translated = new List<SubState>();
            foreach (DeciderState state in graph.States)
            {
                SubState t = new SubState();
                translated.Add(t);
                foreach (Item item in state.Decisions.Keys)
                {
                    SubState decision = getNodeFor(graph.Set, item);
                    foreach (Terminal terminal in state.Decisions[item])
                        t.Transitions.Add(terminal, decision);
                }
            }
            for (int i = 0; i != translated.Count; i++)
            {
                foreach (Terminal t in graph.States[i].Transitions.Keys)
                    translated[i].Transitions.Add(t, translated[graph.States.IndexOf(graph.States[i].Transitions[t])]);
            }
            states[0].Transitions.Add(graph.Lookahead, translated[0]);
            states.AddRange(translated);
        }

        public void AddDecision(Terminal t, CFRule rule) { states[0].Transitions.Add(t, GetNodeFor(rule)); }
        public void AddDecision(Terminal t, State state) { states[0].Transitions.Add(t, GetNodeFor(state)); }

        private SubState getNodeFor(State set, Item item)
        {
            if (item.Action == ItemAction.Reduce)
                return GetNodeFor(item.BaseRule);
            else
                return GetNodeFor(set.Children[item.NextSymbol]);
        }
        private SubState GetNodeFor(CFRule rule)
        {
            if (decideRules.ContainsKey(rule))
                return decideRules[rule];
            SubState decision = new SubState(rule);
            states.Add(decision);
            decideRules.Add(rule, decision);
            return decision;
        }
        private SubState GetNodeFor(State state)
        {
            if (decideShifts.ContainsKey(state))
                return decideShifts[state];
            SubState decision = new SubState(state);
            states.Add(decision);
            decideShifts.Add(state, decision);
            return decision;
        }

        public void Close()
        {
            for (int i = 0; i != states.Count; i++)
                states[i].ID = i;
        }
    }

    class SubState
    {
        private int id;
        private Dictionary<Terminal, SubState> transitions;
        private CFRule decideRule;
        private State decideShift;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public Dictionary<Terminal, SubState> Transitions { get { return transitions; } }
        public CFRule RuleDecision { get { return decideRule; } }
        public State ShiftDecision { get { return decideShift; } }

        public SubState()
        {
            transitions = new Dictionary<Terminal, SubState>();
        }
        public SubState(CFRule decision)
        {
            transitions = new Dictionary<Terminal, SubState>();
            decideRule = decision;
        }
        public SubState(State decision)
        {
            transitions = new Dictionary<Terminal, SubState>();
            decideShift = decision;
        }
    }
}