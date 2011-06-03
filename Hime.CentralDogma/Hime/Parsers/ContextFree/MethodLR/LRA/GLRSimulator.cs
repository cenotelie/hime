using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class GLRSimulator
    {
        private Graph graph;
        private Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

        public GLRSimulator(Graph graph)
        {
            this.graph = graph;
            this.inverseGraph = new Dictionary<int, Dictionary<Symbol, List<State>>>();
            BuildInverse();
        }

        private void BuildInverse()
        {
            foreach (State set in graph.Sets)
            {
                foreach (Symbol symbol in set.Children.Keys)
                {
                    State child = set.Children[symbol];
                    if (!inverseGraph.ContainsKey(child.ID))
                        inverseGraph.Add(child.ID, new Dictionary<Symbol, List<State>>());
                    Dictionary<Symbol, List<State>> inverses = inverseGraph[child.ID];
                    if (!inverses.ContainsKey(symbol))
                        inverses.Add(symbol, new List<State>());
                    List<State> parents = inverses[symbol];
                    parents.Add(set);
                }
            }
        }

        public List<State> Simulate(List<State> sets, Terminal lookahead)
        {
            List<State> results = new List<State>();
            List<int> result_ids = new List<int>();
            foreach (State set in sets)
            {
                List<State> temp = Simulate(set, lookahead);
                foreach (State final in temp)
                {
                    if (!result_ids.Contains(final.ID))
                    {
                        results.Add(final);
                        result_ids.Add(final.ID);
                    }
                }
            }
            return results;
        }

        public List<State> Simulate(State state, Terminal lookahead)
        {
            // Sets before reductions
            List<State> before = new List<State>();
            List<int> before_ids = new List<int>();
            before.Add(state);
            before_ids.Add(state.ID);
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
                                if (!before_ids.Contains(next.ID))
                                {
                                    before.Add(next);
                                    before_ids.Add(next.ID);
                                }
                            }
                        }
                    }
                }
            }
            // Shifts
            List<State> results = new List<State>();
            List<int> results_ids = new List<int>();
            foreach (State s in before)
            {
                if (s.Children.ContainsKey(lookahead))
                {
                    State child = s.Children[lookahead];
                    if (!results_ids.Contains(child.ID))
                    {
                        results.Add(child);
                        results_ids.Add(child.ID);
                    }
                }
            }
            return results;
        }

        public List<State> Simulate(State state, Item item, Terminal lookahead)
        {
            List<State> results = new List<State>();
            List<int> results_ids = new List<int>();
            if (item.Action == ItemAction.Shift)
            {
                results.Add(state.Children[item.NextSymbol]);
                return results;
            }
            
            List<State> before = new List<State>();
            List<int> before_ids = new List<int>();
            // First reduction
            List<State> origins = GetOrigins(state, item.BaseRule.Definition.GetChoiceAtIndex(0));
            foreach (State origin in origins)
            {
                if (origin.Children.ContainsKey(item.BaseRule.Variable))
                {
                    State next = origin.Children[item.BaseRule.Variable];
                    if (!before_ids.Contains(next.ID))
                    {
                        before.Add(next);
                        before_ids.Add(next.ID);
                    }
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
                                if (!before_ids.Contains(next.ID))
                                {
                                    before.Add(next);
                                    before_ids.Add(next.ID);
                                }
                            }
                        }
                    }
                }
            }
            // Shifts
            foreach (State s in before)
            {
                if (s.Children.ContainsKey(lookahead))
                {
                    State child = s.Children[lookahead];
                    if (!results_ids.Contains(child.ID))
                    {
                        results.Add(child);
                        results_ids.Add(child.ID);
                    }
                }
            }
            return results;
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
                List<int> temp_ids = new List<int>();
                foreach (State next in result)
                {
                    if (!inverseGraph.ContainsKey(next.ID))
                        continue;
                    Dictionary<Symbol, List<State>> inverses = inverseGraph[next.ID];
                    if (!inverses.ContainsKey(symbol))
                        continue;
                    foreach (State previous in inverses[symbol])
                    {
                        if (!temp_ids.Contains(previous.ID))
                        {
                            temp.Add(previous);
                            temp_ids.Add(previous.ID);
                        }
                    }
                }
                result = temp;
                index--;
            }
            return result;
        }
    }
}
