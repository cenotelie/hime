/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:25
 * 
 */
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    class GSSNode
    {
        private const int initEdgesCount = 5;
        private const int initPathsCount = 10;

        private ushort state;
        private int generation;
        private GSSEdge[] edges;
        private int edgesCount;

        public ushort State { get { return state; } }
        public int Generation { get { return generation; } }

        public GSSNode(ushort label, int generation)
        {
            this.state = label;
            this.generation = generation;
            this.edges = new GSSEdge[initEdgesCount];
            this.edgesCount = 0;
        }

        public void AddEdge(GSSNode state, AST.SPPFNode label)
        {
            if (edgesCount == edges.Length)
            {
                GSSEdge[] temp = new GSSEdge[edges.Length + initEdgesCount];
                System.Array.Copy(edges, temp, edgesCount);
                edges = temp;
            }
            edges[edgesCount].to = state;
            edges[edgesCount].label = label;
            edgesCount++;
        }

        public bool HasEdgeTo(GSSNode node)
        {
            for (int i = 0; i != edgesCount; i++)
                if (edges[i].to == node)
                    return true;
            return false;
        }

        public AST.SPPFNode GetLabelTo(GSSNode node)
        {
            for (int i = 0; i != edgesCount; i++)
                if (edges[i].to == node)
                    return edges[i].label;
            return null;
        }

        public GSSPath[] GetPaths0()
        {
            GSSPath[] paths = new GSSPath[1];
            paths[0].last = this;
            return paths;
        }

        public GSSPath[] GetPaths(int length, out int count)
        {
            GSSPath[] paths = new GSSPath[initPathsCount];
            paths[0].last = this;
            paths[0].labels = new AST.SPPFNode[length];
            // The number of paths in the list
            count = 1;
            // For the remaining hops
            for (int i = 0; i != length; i++)
            {
                int m = 0;          // Insertion index for the compaction process
                int next = count;   // Insertion index for new paths
                for (int p = 0; p != count; p++)
                {
                    GSSNode last = paths[p].last;
                    // The path stops here
                    if (last.edgesCount == 0)
                    {
                        // Cleanup
                        paths[p].labels = null;
                        continue;
                    }
                    // Look for new additional paths
                    for (int j = 1; j != last.edgesCount; j++)
                    {
                        // Extend the list of paths if necessary
                        if (next == paths.Length)
                        {
                            GSSPath[] temp = new GSSPath[paths.Length + initPathsCount];
                            System.Array.Copy(paths, temp, next);
                            paths = temp;
                        }
                        // Clone and extend the new path
                        paths[next].last = last.edges[j].to;
                        paths[next].labels = new AST.SPPFNode[length];
                        System.Array.Copy(paths[p].labels, paths[next].labels, i);
                        paths[next].labels[i] = last.edges[j].label;
                        // Go to next insert
                        next++;
                    }
                    // Continue the current path
                    paths[m].last = last.edges[0].to;
                    paths[m].labels = paths[p].labels;
                    paths[m].labels[i] = last.edges[0].label;
                    // goto next
                    m++;
                }
                // If Some previous paths have been removed (m != count)
                //    and some have been added (next != cout)
                // => Compact the list
                if (m != count && next != count)
                    for (int p = count; p != next; p++)
                        paths[m++] = paths[p];
                // m is now the exact number of paths
                count = m;
            }
            return paths;
        }
    }
}
