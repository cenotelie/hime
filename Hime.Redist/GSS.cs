using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class GSSNode
    {
        private int p_Generation;
        private ushort p_DFAState;
        private Dictionary<GSSNode, SPPFNode> p_Edges;

        public int Generation { get { return p_Generation; } }
        public ushort DFAState { get { return p_DFAState; } }
        public Dictionary<GSSNode, SPPFNode> Edges { get { return p_Edges; } }

        public void AddEdge(GSSNode state, SPPFNode label) { p_Edges.Add(state, label); }

        public GSSNode(ushort label, int generation)
        {
            p_Generation = generation;
            p_DFAState = label;
            p_Edges = new Dictionary<GSSNode, SPPFNode>();
        }

        public List<GSSNode> NodesAt(int length)
        {
            List<GSSNode> nodes = new List<GSSNode>();
            nodes.Add(this);
            while (length != 0)
            {
                List<GSSNode> nexts = new List<GSSNode>();
                foreach (GSSNode current in nodes)
                    foreach (GSSNode next in current.p_Edges.Keys)
                        if (!nexts.Contains(next))
                            nexts.Add(next);
                nodes = nexts;
                if (nodes.Count == 0)
                    return nodes;
                length--;
            }
            return nodes;
        }

        public List<List<GSSNode>> GetPaths(int length)
        {
            List<List<GSSNode>> paths = new List<List<GSSNode>>();
            List<GSSNode> first = new List<GSSNode>();
            first.Add(this);
            paths.Add(first);
            while (length != 0)
            {
                List<List<GSSNode>> nexts = new List<List<GSSNode>>();
                foreach (List<GSSNode> path in paths)
                {
                    GSSNode last = path[path.Count - 1];
                    foreach (GSSNode next in last.p_Edges.Keys)
                    {
                        List<GSSNode> newPath = new List<GSSNode>(path);
                        newPath.Add(next);
                        nexts.Add(newPath);
                    }
                }
                paths = nexts;
                length--;
            }
            return paths;
        }
    }
}
