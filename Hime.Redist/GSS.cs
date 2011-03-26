using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    public class GSSNode
    {
        private int generation;
        private ushort dFAState;
        private Dictionary<GSSNode, SPPFNode> edges;

        public int Generation { get { return generation; } }
        public ushort DFAState { get { return dFAState; } }
        public Dictionary<GSSNode, SPPFNode> Edges { get { return edges; } }

        public void AddEdge(GSSNode state, SPPFNode label) { edges.Add(state, label); }

        public GSSNode(ushort label, int generation)
        {
            this.generation = generation;
            this.dFAState = label;
            this.edges = new Dictionary<GSSNode, SPPFNode>();
        }

        public List<GSSNode> NodesAt(int length)
        {
            List<GSSNode> nodes = new List<GSSNode>();
            nodes.Add(this);
            while (length != 0)
            {
                List<GSSNode> nexts = new List<GSSNode>();
                foreach (GSSNode current in nodes)
                    foreach (GSSNode next in current.edges.Keys)
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
                    foreach (GSSNode next in last.edges.Keys)
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
