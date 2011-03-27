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

        public List<State> Simulate(State state, Terminal lookahead)
        {
            // Sets before reductions
            List<State> before = new List<State>();
            before.Add(state);
            // Reduce
            for (int i = 0; i != before.Count; i++)
            {
                State current = before[i];
                foreach (StateActionReduce reduction in current.Reductions)
                {
                    if (reduction.Lookahead == lookahead)
                    {
                        List<State> origins = GetOrigins(current, reduction.ToReduceRule.Definition.GetChoiceAtIndex(0));
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

        public List<State> Simulate(State state, Item item, Terminal lookahead)
        {
            List<State> result = new List<State>();
            if (item.Action == ItemAction.Shift)
            {
                result.Add(state.Children[item.NextSymbol]);
                return result;
            }
            
            List<State> before = new List<State>();
            // First reduction
            List<State> origins = GetOrigins(state, item.BaseRule.Definition.GetChoiceAtIndex(0));
            foreach (State origin in origins)
            {
                if (origin.Children.ContainsKey(item.BaseRule.Variable))
                {
                    State next = origin.Children[item.BaseRule.Variable];
                    if (!before.Contains(next))
                        before.Add(next);
                }
            }
            // Reduce
            for (int i = 0; i != before.Count; i++)
            {
                State current = before[i];
                foreach (StateActionReduce reduction in current.Reductions)
                {
                    if (reduction.Lookahead == lookahead)
                    {
                        origins = GetOrigins(current, reduction.ToReduceRule.Definition.GetChoiceAtIndex(0));
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
                    if (!inverseGraph.ContainsKey(next))
                        continue;
                    Dictionary<Symbol, List<State>> inverses = inverseGraph[next];
                    if (!inverses.ContainsKey(symbol))
                        continue;
                    foreach (State previous in inverses[symbol])
                    {
                        if (!temp.Contains(previous))
                            temp.Add(previous);
                    }
                }
                result = temp;
                index--;
            }
            return result;
        }
    }
}
