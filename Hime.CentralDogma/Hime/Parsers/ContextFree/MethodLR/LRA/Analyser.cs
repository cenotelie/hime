using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ConflictAnalyser
    {
        private GLRSimulator simulator;

        public ConflictAnalyser(Graph graph)
        {
            simulator = new GLRSimulator(graph);
        }

        public DeciderGraph Analyse(State lrset, Terminal lookahead)
        {
            DeciderGraph graph = new DeciderGraph(lrset, lookahead, simulator);
            graph.Build();
            return graph;
        }
    }
}
