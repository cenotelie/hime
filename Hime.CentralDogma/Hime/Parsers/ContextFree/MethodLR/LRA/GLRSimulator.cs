using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class GLRSimulator
    {
        private Graph p_Graph;
        private Dictionary<ItemSet, Dictionary<Symbol, List<ItemSet>>> p_InverseGraph;

        public GLRSimulator(Graph graph)
        {
            p_Graph = graph;
            p_InverseGraph = new Dictionary<ItemSet, Dictionary<Symbol, List<ItemSet>>>();
            BuildInverse();
        }

        private void BuildInverse()
        {
            foreach (ItemSet set in p_Graph.Sets)
            {
                foreach (Symbol symbol in set.Children.Keys)
                {
                    ItemSet child = set.Children[symbol];
                    if (!p_InverseGraph.ContainsKey(child))
                        p_InverseGraph.Add(child, new Dictionary<Symbol, List<ItemSet>>());
                    Dictionary<Symbol, List<ItemSet>> inverses = p_InverseGraph[child];
                    if (!inverses.ContainsKey(symbol))
                        inverses.Add(symbol, new List<ItemSet>());
                    List<ItemSet> parents = inverses[symbol];
                    parents.Add(set);
                }
            }
        }

        public List<ItemSet> Simulate(List<ItemSet> sets, Terminal lookahead)
        {
            List<ItemSet> result = new List<ItemSet>();
            foreach (ItemSet set in sets)
            {
                List<ItemSet> temp = Simulate(set, lookahead);
                foreach (ItemSet final in temp)
                    if (!result.Contains(final))
                        result.Add(final);
            }
            return result;
        }

        private List<ItemSet> Simulate(ItemSet set, Terminal lookahead)
        {
            // Sets before reductions
            List<ItemSet> before = new List<ItemSet>();
            before.Add(set);
            // Reduce
            for (int i = 0; i != before.Count; i++)
            {
                ItemSet current = before[i];
                foreach (ItemSetActionReduce reduction in set.Reductions.Reductions)
                {
                    if (reduction.Lookahead == lookahead)
                    {
                        List<ItemSet> origins = GetOrigins(set, reduction.ToReduceRule.Definition.GetChoiceAtIndex(0));
                        foreach (ItemSet origin in origins)
                        {
                            if (origin.Children.ContainsKey(reduction.ToReduceRule.Variable))
                            {
                                ItemSet next = origin.Children[reduction.ToReduceRule.Variable];
                                if (!before.Contains(next))
                                    before.Add(next);
                            }
                        }
                    }
                }
            }
            // Shifts
            List<ItemSet> result = new List<ItemSet>();
            foreach (ItemSet s in before)
            {
                if (s.Children.ContainsKey(lookahead))
                    if (!result.Contains(s.Children[lookahead]))
                        result.Add(s.Children[lookahead]);
            }
            return result;
        }

        private List<ItemSet> GetOrigins(ItemSet target, CFRuleDefinition definition)
        {
            List<ItemSet> result = new List<ItemSet>();
            result.Add(target);
            int index = definition.Length - 1;
            while (index != -1)
            {
                Symbol symbol = definition.Parts[index].Symbol;
                List<ItemSet> temp = new List<ItemSet>();
                foreach (ItemSet next in result)
                {
                    foreach (ItemSet previous in p_InverseGraph[next][symbol])
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
