using System.Collections.Generic;
using Hime.Redist.AST;

namespace Hime.CentralDogma
{
    /// <summary>
    /// Convenience class for the exportation of ASTs to graphs
    /// </summary>
    public static class ASTExporter
    {
        /// <summary>
        /// Exports the given CST tree to a DOT graph in the specified file
        /// </summary>
        /// <param name="root">Root of the tree to export</param>
        /// <param name="file">DOT file to export to</param>
        public static void ExportDOT(CSTNode root, string file)
        {
            Documentation.DOTSerializer serializer = new Documentation.DOTSerializer("CST", file);
            ExportDOT_CST(serializer, null, 0, root);
            serializer.Close();
        }

        private static int ExportDOT_CST(Documentation.DOTSerializer serializer, string parent, int nextID, CSTNode node)
        {
            string name = "node" + nextID;
            string label = node.Symbol.ToString();
            serializer.WriteNode(name, label, Documentation.DOTNodeShape.circle);
            if (parent != null)
                serializer.WriteEdge(parent, name, string.Empty);
            int result = nextID + 1;
            foreach (CSTNode child in node.Children)
                result = ExportDOT_CST(serializer, name, result, child);
            return result;
        }

        /// <summary>
        /// Exports the given SPPF to a DOT graph in the specified file
        /// </summary>
        /// <param name="root">Root of the graph to export</param>
        /// <param name="file">DOT file to export to</param>
        public static void ExportDOT(SPPFNode root, string file)
        {
            List<SPPFNode> graph = new List<SPPFNode>();
            List<SPPFNode> finished = new List<SPPFNode>();
            Stack<SPPFNode> stack = new Stack<SPPFNode>();
            graph.Add(root);
            stack.Push(root);
            Documentation.DOTSerializer serializer = new Documentation.DOTSerializer("SPPF", file);
            while (stack.Count != 0)
            {
                SPPFNode node = stack.Pop();
                if (finished.Contains(node))
                    continue;
                string name = "node" + graph.IndexOf(node);
                string label = node.Symbol.ToString();
                serializer.WriteNode(name, label, Documentation.DOTNodeShape.circle);
                for (int i = 0; i != node.Families.Count; i++)
                {
                    string fname = name + "_f" + i;
                    serializer.WriteNode(fname, string.Empty, Documentation.DOTNodeShape.point);
                    serializer.WriteEdge(name, fname, string.Empty);
                    foreach (SPPFNode child in node.Families[i].Children)
                    {
                        int index = graph.IndexOf(child);
                        if (index == -1)
                        {
                            index = graph.Count;
                            graph.Add(child);
                        }
                        serializer.WriteEdge(fname, "node" + index, string.Empty);
                        stack.Push(child);
                    }
                }
                finished.Add(node);
            }
            serializer.Close();
        }
    }
}
