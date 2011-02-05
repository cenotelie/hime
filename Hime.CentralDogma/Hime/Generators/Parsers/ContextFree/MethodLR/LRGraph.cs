namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Represents a LR graph
    /// </summary>
    public class LRGraph
    {
        /// <summary>
        /// List of the sets in the graph
        /// </summary>
        private System.Collections.Generic.List<LRItemSet> p_Sets;

        /// <summary>
        /// Get a list of the sets
        /// </summary>
        /// <value>A list of the sets</value>
        public System.Collections.Generic.List<LRItemSet> Sets { get { return p_Sets; } }

        /// <summary>
        /// Constructs an empty graph
        /// </summary>
        public LRGraph()
        {
            p_Sets = new System.Collections.Generic.List<LRItemSet>();
        }

        /// <summary>
        /// Test if the graph contains a set with the same kernel
        /// </summary>
        /// <param name="Kernel">The kernel to search for</param>
        /// <returns>Returns the complete set in the graph with an equivalent kernel or null if none was found</returns>
        public LRItemSet ContainsSet(LRItemSetKernel Kernel)
        {
            foreach (LRItemSet Potential in p_Sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        /// <summary>
        /// Add the given set to the graph if not already present
        /// </summary>
        /// <param name="Set">The set to add</param>
        /// <returns>Returns the new set or the equivalent one that was already in the graph</returns>
        public LRItemSet AddUnique(LRItemSet Set)
        {
            foreach (LRItemSet Potential in p_Sets)
            {
                // If same kernel : return the set
                if (Potential.Equals(Set))
                    return Potential;
            }
            p_Sets.Add(Set);
            return Set;
        }

        /// <summary>
        /// Add the given set to the graph without checking
        /// </summary>
        /// <param name="Set">The set to add</param>
        public void Add(LRItemSet Set)
        {
            p_Sets.Add(Set);
        }



        private Hime.Kernel.Graph.Vertex Draw_BuildGraph(Hime.Kernel.Graph.Graph Graph, LRItemSet Set)
        {
            Hime.Kernel.Graph.Vertex Vertex = Graph.GetRepresentationOf(Set);
            if (Vertex == null)
            {
                Vertex = Graph.AddVertex(Set);
                foreach (Symbol Edge in Set.Children.Keys)
                {
                    Hime.Kernel.Graph.Vertex Child = Draw_BuildGraph(Graph, Set.Children[Edge]);
                    Graph.AddEdge(Vertex, Child, Edge).Visual = new Hime.Kernel.Graph.EdgeVisualArrow();
                }
            }
            return Vertex;
        }

        public System.Drawing.Bitmap Draw()
        {
            Hime.Kernel.Graph.Graph Graph = new Hime.Kernel.Graph.Graph(14);
            Draw_BuildGraph(Graph, p_Sets[0]);
            Hime.Kernel.Graph.PlacementAnnealingMethod Method = new Hime.Kernel.Graph.PlacementAnnealingMethod();
            Graph.Build(Method);
            return Graph.Draw(1);
        }
    }
}