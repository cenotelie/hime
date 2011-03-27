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

        public DeciderGraph Analyse(State lrset, Conflict conflict)
        {
            DeciderGraph graph = new DeciderGraph(lrset, conflict);
            graph.Build(simulator);
            return graph;
        }
    }
}
