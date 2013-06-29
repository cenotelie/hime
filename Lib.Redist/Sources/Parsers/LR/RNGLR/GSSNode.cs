namespace Hime.Redist.Parsers
{
    class GSSNode
    {
        private const int initEdgesCount = 5;

        private GSSGeneration generation;
        private int state;
        private GSSEdge[] edges;
        private int edgesCount;

        public int Generation { get { return generation.Index; } }
        public int State { get { return state; } }
        public int EdgesCount { get { return edgesCount; } }
        
        public GSSNode()
        {
            this.edges = new GSSEdge[initEdgesCount];
        }

        public void Initialize(GSSGeneration gen, int state)
        {
            this.generation = gen;
            this.state = state;
        }

        public void Clear()
        {
            this.generation = null;
            this.edgesCount = 0;
        }

        public void AddEdge(GSSNode state, SPPF label)
        {
            if (edgesCount == edges.Length)
            {
                GSSEdge[] temp = new GSSEdge[edges.Length + initEdgesCount];
                System.Array.Copy(edges, temp, edgesCount);
                edges = temp;
            }
            edges[edgesCount] = new GSSEdge(state, label);
            state.generation.Mark(state);
            edgesCount++;
        }

        public bool HasEdgeTo(GSSNode node)
        {
            for (int i = 0; i != edgesCount; i++)
                if (edges[i].To == node)
                    return true;
            return false;
        }
        
        public GSSEdge GetEdge(int index)
        {
        	return edges[index];
        }
    }
}
