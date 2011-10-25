/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a node in a graph structured stack
    /// </summary>
    public sealed class GSSNode
    {
        private int generation;
        private ushort dFAState;
        private Dictionary<GSSNode, SPPFNode> edges;

        /// <summary>
        /// Gets the generation of this node
        /// </summary>
        public int Generation { get { return generation; } }
        /// <summary>
        /// Gets the id of the DFA state represented by this GSS node
        /// </summary>
        public ushort DFAState { get { return dFAState; } }
        /// <summary>
        /// Gets the edges coming out of this node
        /// </summary>
        public Dictionary<GSSNode, SPPFNode> Edges { get { return edges; } }

        /// <summary>
        /// Adds a new edge from this node
        /// </summary>
        /// <param name="state">The previous GSS node</param>
        /// <param name="label">The label of the edge</param>
        public void AddEdge(GSSNode state, SPPFNode label) { edges.Add(state, label); }

        /// <summary>
        /// Initializes a new instance of the GSSNode class with the given DFA state id and generation
        /// </summary>
        /// <param name="label">The ID of the represented DFA state</param>
        /// <param name="generation">The generation of the node</param>
        public GSSNode(ushort label, int generation)
        {
            this.generation = generation;
            this.dFAState = label;
            this.edges = new Dictionary<GSSNode, SPPFNode>();
        }

        /// <summary>
        /// Gets all the node at a given distance from this node
        /// </summary>
        /// <param name="length">The distance</param>
        /// <returns>The list of nodes a the given distance</returns>
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

        /// <summary>
        /// Gets all the paths of the given length starting at this node
        /// </summary>
        /// <param name="length">The length of paths</param>
        /// <returns>The paths of the given length starting at this node</returns>
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
