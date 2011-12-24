/*
 * Author: Laurent Wouters
 * Date: 16/12/2011
 * Time: 16:06
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Graphs;
using System.Xml;
using System;

namespace Hime.Parsers.ContextFree.LR
{
    public class GraphInverse
    {
        protected Graph graph;
        protected Dictionary<int, Dictionary<GrammarSymbol, List<State>>> inverseGraph;

        public bool HasIncomings(int target) { return inverseGraph.ContainsKey(target); }
        public ICollection<GrammarSymbol> GetIncomings(int target) { return inverseGraph[target].Keys; }
        public ICollection<State> GetOrigins(int target, GrammarSymbol transition) { return inverseGraph[target][transition]; }

        public GraphInverse(Graph graph)
        {
            this.graph = graph;
            this.inverseGraph = new Dictionary<int, Dictionary<GrammarSymbol, List<State>>>();
            BuildInverse();
        }

        protected void BuildInverse()
        {
            foreach (State set in graph.States)
            {
                foreach (GrammarSymbol symbol in set.Children.Keys)
                {
                    State child = set.Children[symbol];
                    if (!inverseGraph.ContainsKey(child.ID))
                        inverseGraph.Add(child.ID, new Dictionary<GrammarSymbol, List<State>>());
                    Dictionary<GrammarSymbol, List<State>> inverses = inverseGraph[child.ID];
                    if (!inverses.ContainsKey(symbol))
                        inverses.Add(symbol, new List<State>());
                    List<State> parents = inverses[symbol];
                    parents.Add(set);
                }
            }
        }
    }
}
