using System.Collections.Generic;
using System.Xml;
using System;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class GraphInverse
    {
        protected Graph graph;
        protected Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

        public bool HasIncomings(int target) { return inverseGraph.ContainsKey(target); }
        public ICollection<Symbol> GetIncomings(int target) { return inverseGraph[target].Keys; }
        public ICollection<State> GetOrigins(int target, Symbol transition) { return inverseGraph[target][transition]; }

        public GraphInverse(Graph graph)
        {
            this.graph = graph;
            this.inverseGraph = new Dictionary<int, Dictionary<Symbol, List<State>>>();
            BuildInverse();
        }

        protected void BuildInverse()
        {
            foreach (State set in graph.States)
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
    }
}
