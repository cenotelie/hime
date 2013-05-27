using System.Collections.Generic;
using Hime.Redist.Parsers;

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
        public static void ExportDOT(ASTNode root, string file)
        {
            Documentation.DOTSerializer serializer = new Documentation.DOTSerializer("CST", file);
            ExportDOT_CST(serializer, null, 0, root);
            serializer.Close();
        }

        private static int ExportDOT_CST(Documentation.DOTSerializer serializer, string parent, int nextID, ASTNode node)
        {
            string name = "node" + nextID;
            string label = node.Symbol.ToString();
            serializer.WriteNode(name, label, Documentation.DOTNodeShape.circle);
            if (parent != null)
                serializer.WriteEdge(parent, name, string.Empty);
            int result = nextID + 1;
            foreach (ASTNode child in node.Children)
                result = ExportDOT_CST(serializer, name, result, child);
            return result;
        }
    }
}
