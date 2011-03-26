using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class DeciderGraph
    {
        private List<DeciderState> p_States;

        public DeciderGraph()
        {
            p_States = new List<DeciderState>();
        }

        public void Build(GLRSimulator simulator)
        {
            for (int i = 0; i != p_States.Count; i++)
            {
                DeciderState current = p_States[i];
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
            foreach (DeciderState potential in p_States)
                if (potential.Equals(candidate))
                    return potential;
            p_States.Add(candidate);
            return candidate;
        }
    }

	class DeciderState
	{
        private List<List<ItemSet>> p_Choices;
        private List<TerminalSet> p_Decisions;
        private Dictionary<Terminal, DeciderState> p_Transitions;

        public void AddTransition(Terminal terminal, DeciderState state) { p_Transitions.Add(terminal, state); }

        public DeciderState()
        {
            p_Choices = new List<List<ItemSet>>();
            p_Decisions = new List<TerminalSet>();
            p_Transitions = new Dictionary<Terminal, DeciderState>();
        }

        public Dictionary<Terminal, DeciderState> ComputeNexts(GLRSimulator simulator)
        {
            return null;
        }


        private TerminalSet ComputeFollowers()
        {
            List<TerminalSet> followers = new List<TerminalSet>();
            foreach (List<ItemSet> choice in p_Choices)
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
            if (this.p_Choices.Count != right.p_Choices.Count)
                return false;
            for (int i = 0; i != this.p_Choices.Count; i++)
            {
                List<ItemSet> l1 = this.p_Choices[i];
                List<ItemSet> l2 = right.p_Choices[i];
                if (l1.Count != l2.Count)
                    return false;
                foreach (ItemSet set in l1)
                    if (!l2.Contains(set))
                        return false;
                /*TerminalSet s1 = this.p_Decisions[i];
                TerminalSet s2 = right.p_Decisions[i];
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
