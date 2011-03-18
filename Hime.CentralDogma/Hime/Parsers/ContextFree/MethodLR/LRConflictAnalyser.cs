using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ConflictAnalyser
    {
        private Graph p_Graph;
        private Dictionary<ItemSet, Dictionary<Symbol, List<ItemSet>>> p_InverseGraph;

        public ConflictAnalyser(Graph graph)
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
    }
}