namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a node in a GSS
	/// </summary>
    class GSSNode
    {
        private const int initEdgesCount = 5;

        private GSSGeneration generation;
        private int state;
        private GSSEdge[] edges;
        private int edgesCount;

        /// <summary>
        /// Gets the index of the generation to which this node belong to
        /// </summary>
        public int Generation { get { return generation.Index; } }
        /// <summary>
        /// Gets the RNGLR state represented by this node
        /// </summary>
        public int State { get { return state; } }
        /// <summary>
        /// Gets the number of edges starting at this node
        /// </summary>
        public int EdgesCount { get { return edgesCount; } }
        
        /// <summary>
        /// Creates a new node
        /// </summary>
        public GSSNode()
        {
            this.edges = new GSSEdge[initEdgesCount];
        }

        /// <summary>
        /// (Re-)initializes this node
        /// </summary>
        /// <param name="gen">This node's parent generation</param>
        /// <param name="state">The state represented by this node</param>
        public void Initialize(GSSGeneration gen, int state)
        {
            this.generation = gen;
            this.state = state;
        }
        
        /// <summary>
        /// Adds an edge from this node
        /// </summary>
        /// <param name="target">The edge's target</param>
        /// <param name="label">The edge's label</param>
        public void AddEdge(GSSNode target, SPPF label)
        {
            if (edgesCount == edges.Length)
            {
                GSSEdge[] temp = new GSSEdge[edges.Length + initEdgesCount];
                System.Array.Copy(edges, temp, edgesCount);
                edges = temp;
            }
            edges[edgesCount] = new GSSEdge(target, label);
            target.generation.Mark(target);
            edgesCount++;
        }

        /// <summary>
        /// Determines whether this node has an edge to the given node
        /// </summary>
        /// <param name="node">The potential target node</param>
        /// <returns></returns>
        public bool HasEdgeTo(GSSNode node)
        {
            for (int i = 0; i != edgesCount; i++)
                if (edges[i].To == node)
                    return true;
            return false;
        }
        
        /// <summary>
        /// Gets the edge at the given index
        /// </summary>
        /// <param name="index">Edge's index</param>
        /// <returns>The corresponding edge</returns>
        public GSSEdge GetEdge(int index)
        {
        	return edges[index];
        }
    }
}
