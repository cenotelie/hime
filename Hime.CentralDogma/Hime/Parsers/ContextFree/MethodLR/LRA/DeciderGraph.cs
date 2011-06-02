using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DeciderGraph
    {
        private GLRSimulator simulator;
        private State set;
        private Terminal lookahead;
        private List<Item> conflictuous;
        private List<DeciderState> states;

        public State Set { get { return set; } }
        public Terminal Lookahead { get { return lookahead; } }
        public IList<Item> Conflictuous { get { return conflictuous; } }
        public IList<DeciderState> States { get { return states; } }

        public DeciderGraph(State set, Terminal lookahead, GLRSimulator simulator)
        {
            this.simulator = simulator;
            this.set = set;
            this.lookahead = lookahead;
            states = new List<DeciderState>();
            conflictuous = new List<Item>();
            foreach (Conflict conflict in set.Conflicts)
                if (conflict.ConflictSymbol == lookahead)
                    foreach (Item item in conflict.Items)
                        if (!conflictuous.Contains(item))
                            conflictuous.Add(item);
            Dictionary<Item, List<State>> init = new Dictionary<Item, List<State>>();
            foreach (Item item in conflictuous)
                init.Add(item, simulator.Simulate(set, item, lookahead));
            states.Add(new DeciderState(init));
        }

        public void Build()
        {
            for (int i = 0; i != states.Count; i++)
            {
                DeciderState current = states[i];
                current.ID = i;
                Dictionary<Terminal, DeciderState> dic = current.ComputeNexts(simulator);
                foreach (Terminal terminal in dic.Keys)
                {
                    DeciderState next = dic[terminal];
                    next = AddUnique(next);
                    current.AddTransition(terminal, next);
                }
            }
        }

        public DeciderState AddUnique(DeciderState candidate)
        {
            foreach (DeciderState potential in states)
                if (potential.Equals(candidate))
                    return potential;
            states.Add(candidate);
            return candidate;
        }
    }

	class DeciderState
	{
        private int id;
        private Dictionary<Item, List<State>> choices;
        private Dictionary<Item, TerminalSet> decisions;
        private Dictionary<Terminal, DeciderState> transitions;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public Dictionary<Item, List<State>> Choices { get { return choices; } }
        public Dictionary<Item, TerminalSet> Decisions { get { return decisions; } }
        public Dictionary<Terminal, DeciderState> Transitions { get { return transitions; } }

        public void AddTransition(Terminal terminal, DeciderState state) { transitions.Add(terminal, state); }

        public DeciderState(Dictionary<Item, List<State>> init)
        {
            choices = init;
            decisions = new Dictionary<Item, TerminalSet>();
            transitions = new Dictionary<Terminal, DeciderState>();
        }

        private DeciderState()
        {
            choices = new Dictionary<Item, List<State>>();
            decisions = new Dictionary<Item, TerminalSet>();
            transitions = new Dictionary<Terminal, DeciderState>();
        }

        private void AddDecision(Item item, Terminal t)
        {
            if (!decisions.ContainsKey(item))
                decisions.Add(item, new TerminalSet());
            decisions[item].Add(t);
        }

        public Dictionary<Terminal, DeciderState> ComputeNexts(GLRSimulator simulator)
        {
            TerminalSet common = ComputeFollowers();
            Dictionary<Terminal, DeciderState> result = new Dictionary<Terminal, DeciderState>();
            foreach (Terminal t in common)
                result.Add(t, ComputeNext(simulator, t));
            return result;
        }

        private DeciderState ComputeNext(GLRSimulator simulator, Terminal lookahead)
        {
            DeciderState next = new DeciderState();
            foreach (Item item in choices.Keys)
                next.choices.Add(item, simulator.Simulate(choices[item], lookahead));
            return next;
        }

        private TerminalSet ComputeFollowers()
        {
            Dictionary<Item, TerminalSet> followers = new Dictionary<Item, TerminalSet>();
            foreach (Item item in choices.Keys)
                followers.Add(item, ComputeFollowers(choices[item]));
            TerminalSet common = new TerminalSet();
            Item firstKey = null;
            foreach (Item item in followers.Keys)
            {
                firstKey = item;
                break;
            }
            foreach (Terminal t in followers[firstKey])
            {
                bool iscommon = true;
                foreach (Item item in followers.Keys)
                {
                    if (item == firstKey)
                        continue;
                    if (!followers[item].Contains(t))
                    {
                        iscommon = false;
                        break;
                    }
                }
                if (iscommon)
                    common.Add(t);
                else
                    AddDecision(firstKey, t);
            }

            foreach (Item item in followers.Keys)
            {
                if (item == firstKey)
                    continue;
                foreach (Terminal t in followers[item])
                    if (!common.Contains(t))
                        AddDecision(item, t);
            }
            return common;
        }
        private TerminalSet ComputeFollowers(List<State> choice)
        {
            TerminalSet terminals = new TerminalSet();
            foreach (State set in choice)
            {
                foreach (Symbol symbol in set.Children.Keys)
                    if (symbol is Terminal)
                        terminals.Add((Terminal)symbol);
                terminals.AddRange(set.Reductions.ExpectedTerminals);
            }
            return terminals;
        }

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            if (!(obj is DeciderState))
                return false;
            DeciderState right = (DeciderState)obj;
            if (this.choices.Count != right.choices.Count)
                return false;
            foreach (Item key in this.choices.Keys)
            {
                if (!right.choices.ContainsKey(key))
                    return false;
                List<State> l1 = this.choices[key];
                List<State> l2 = right.choices[key];
                if (l1.Count != l2.Count)
                    return false;
                foreach (State set in l1)
                    if (!l2.Contains(set))
                        return false;
            }
            return true;
        }
	}
}
