using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DeciderGraph
    {
        private List<DeciderState> states;

        public DeciderGraph()
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
        private List<List<ItemSet>> choices;
        private List<TerminalSet> decisions;
        private Dictionary<Terminal, DeciderState> transitions;

        public void AddTransition(Terminal terminal, DeciderState state) { transitions.Add(terminal, state); }

        public DeciderState()
        {
            choices = new List<List<ItemSet>>();
            decisions = new List<TerminalSet>();
            transitions = new Dictionary<Terminal, DeciderState>();
        }

        public Dictionary<Terminal, DeciderState> ComputeNexts(GLRSimulator simulator)
        {
            return null;
        }


        private TerminalSet ComputeFollowers()
        {
            List<TerminalSet> followers = new List<TerminalSet>();
            foreach (List<ItemSet> choice in choices)
                followers.Add(ComputeFollowers(choice));
            TerminalSet common = new TerminalSet();
            

            return common;
        }
        private TerminalSet ComputeFollowers(List<ItemSet> choice)
        {
            TerminalSet terminals = new TerminalSet();
            foreach (ItemSet set in choice)
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
                List<ItemSet> l1 = this.choices[i];
                List<ItemSet> l2 = right.choices[i];
                if (l1.Count != l2.Count)
                    return false;
                foreach (ItemSet set in l1)
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
