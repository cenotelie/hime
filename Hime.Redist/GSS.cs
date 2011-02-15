namespace Hime.Redist.Parsers
{
    public class GSSNode
    {
        private ushort p_DFAState;
        private System.Collections.Generic.Dictionary<GSSNode, SPPFNode> p_Edges;

        public ushort DFAState { get { return p_DFAState; } }
        public System.Collections.Generic.Dictionary<GSSNode, SPPFNode> Edges { get { return p_Edges; } }

        public GSSNode(ushort label)
        {
            p_DFAState = label;
            p_Edges = new System.Collections.Generic.Dictionary<GSSNode, SPPFNode>();
        }

        public System.Collections.Generic.List<GSSNode> NodesAt(int length)
        {
            System.Collections.Generic.List<GSSNode> nodes = new System.Collections.Generic.List<GSSNode>();
            nodes.Add(this);
            while (length != 0)
            {
                System.Collections.Generic.List<GSSNode> nexts = new System.Collections.Generic.List<GSSNode>();
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

        public System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>> GetPaths(int length)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>> paths = new System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>>();
            System.Collections.Generic.List<GSSNode> first = new System.Collections.Generic.List<GSSNode>();
            first.Add(this);
            paths.Add(first);
            while (length != 0)
            {
                System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>> nexts = new System.Collections.Generic.List<System.Collections.Generic.List<GSSNode>>();
                foreach (System.Collections.Generic.List<GSSNode> path in paths)
                {
                    GSSNode last = path[path.Count - 1];
                    foreach (GSSNode next in last.p_Edges.Keys)
                    {
                        System.Collections.Generic.List<GSSNode> newPath = new System.Collections.Generic.List<GSSNode>(path);
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