using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DeciderGraph
    {
        private List<DeciderState> states;

        public DeciderGraph(State set, Conflict conflict)
        {
            states = new List<DeciderState>();
            
        }

        public void Build(GLRSimulator simulator)
        {
            for (int i = 0; i != states.Count; i++)
            {
                DeciderState current = states[i];
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
        private List<List<State>> choices;
        private List<TerminalSet> decisions;
        private Dictionary<Terminal, DeciderState> transitions;

        public void AddTransition(Terminal terminal, DeciderState state) { transitions.Add(terminal, state); }

        public DeciderState(State set, Conflict conflict)
        {
            choices = new List<List<State>>();
            decisions = new List<TerminalSet>();
            transitions = new Dictionary<Terminal, DeciderState>();

        }

        private DeciderState()
        {
            choices = new List<List<State>>();
            decisions = new List<TerminalSet>();
            transitions = new Dictionary<Terminal, DeciderState>();
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
            foreach (List<State> choice in this.choices)
                next.choices.Add(simulator.Simulate(choice, lookahead));
            return next;
        }

        private TerminalSet ComputeFollowers()
        {
            List<TerminalSet> followers = new List<TerminalSet>();
            foreach (List<State> choice in choices)
                followers.Add(ComputeFollowers(choice));
            TerminalSet common = new TerminalSet();
            foreach (Terminal t in followers[0])
            {
                bool iscommon = true;
                for (int i = 1; i != followers.Count; i++)
                {
                    if (!followers[i].Contains(t))
                    {
                        iscommon = false;
                        break;
                    }
                }
                if (iscommon)
                    common.Add(t);
                else
                    decisions[0].Add(t);
            }

            for (int i = 1; i != followers.Count; i++)
            {
                foreach (Terminal t in followers[i])
                    if (!common.Contains(t))
                        decisions[i].Add(t);
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
            for (int i = 0; i != this.choices.Count; i++)
            {
                List<State> l1 = this.choices[i];
                List<State> l2 = right.choices[i];
                if (l1.Count != l2.Count)
                    return false;
                foreach (State set in l1)
                    if (!l2.Contains(set))
                        return false;
                /*TerminalSet s1 = this.decisions[i];
                TerminalSet s2 = right.decisions[i];
                if (s1.Count != s2.Count)
                    return false;
                foreach (Terminal t in s1)
                    if (!s2.Contains(t))
                        return false;*/
            }
            return true;
        }
	}
}
