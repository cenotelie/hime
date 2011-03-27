using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class GLRSimulator
    {
        private Graph graph;
        private Dictionary<State, Dictionary<Symbol, List<State>>> inverseGraph;

        public GLRSimulator(Graph graph)
        {
            this.graph = graph;
            this.inverseGraph = new Dictionary<State, Dictionary<Symbol, List<State>>>();
            BuildInverse();
        }

        private void BuildInverse()
        {
            foreach (State set in graph.Sets)
            {
                foreach (Symbol symbol in set.Children.Keys)
                {
                    State child = set.Children[symbol];
                    if (!inverseGraph.ContainsKey(child))
                        inverseGraph.Add(child, new Dictionary<Symbol, List<State>>());
                    Dictionary<Symbol, List<State>> inverses = inverseGraph[child];
                    if (!inverses.ContainsKey(symbol))
                        inverses.Add(symbol, new List<State>());
                    List<State> parents = inverses[symbol];
                    parents.Add(set);
                }
            }
        }

        public List<State> Simulate(List<State> sets, Terminal lookahead)
        {
            List<State> result = new List<State>();
            foreach (State set in sets)
            {
                List<State> temp = Simulate(set, lookahead);
                foreach (State final in temp)
                    if (!result.Contains(final))
                        result.Add(final);
            }
            return result;
        }

        public List<State> Simulate(State set, Terminal lookahead)
        {
            // Sets before reductions
            List<State> before = new List<State>();
            before.Add(set);
            // Reduce
            for (int i = 0; i != before.Count; i++)
            {
                State current = before[i];
                foreach (StateActionReduce reduction in set.Reductions.Reductions)
                {
                    if (reduction.Lookahead == lookahead)
                    {
                        List<State> origins = GetOrigins(set, reduction.ToReduceRule.Definition.GetChoiceAtIndex(0));
                        foreach (State origin in origins)
                        {
                            if (origin.Children.ContainsKey(reduction.ToReduceRule.Variable))
                            {
                                State next = origin.Children[reduction.ToReduceRule.Variable];
                                if (!before.Contains(next))
                                    before.Add(next);
                            }
                        }
                    }
                }
            }
            // Shifts
            List<State> result = new List<State>();
            foreach (State s in before)
            {
                if (s.Children.ContainsKey(lookahead))
                    if (!result.Contains(s.Children[lookahead]))
                        result.Add(s.Children[lookahead]);
            }
            return result;
        }

        private List<State> GetOrigins(State target, CFRuleDefinition definition)
        {
            List<State> result = new List<State>();
            result.Add(target);
            int index = definition.Length - 1;
            while (index != -1)
            {
                Symbol symbol = definition.Parts[index].Symbol;
                List<State> temp = new List<State>();
                foreach (State next in result)
                {
                    foreach (State previous in inverseGraph[next][symbol])
                    {
                        if (!temp.Contains(previous))
                            temp.Add(previous);
                    }
                }
                result = temp;
            }
            return result;
        }
    }
}
