/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;
using Hime.Redist;

namespace Hime.CentralDogma.SDK
{
    /// <summary>
    /// Convenience class for the export of ASTs to graphs
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
