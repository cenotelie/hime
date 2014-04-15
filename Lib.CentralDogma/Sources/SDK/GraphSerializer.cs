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
using Hime.Redist.AST;
using Hime.CentralDogma.Automata;

namespace Hime.CentralDogma.SDK
{
    /// <summary>
    /// Convenience class for the export of graph-like data structures to files
    /// </summary>
    public static class GraphSerializer
    {
        /// <summary>
        /// Exports the given CST tree to a DOT graph in the specified file
        /// </summary>
        /// <param name="root">Root of the tree to export</param>
        /// <param name="file">DOT file to export to</param>
        public static void ExportDOT(ASTNode root, string file)
        {
            DOTSerializer serializer = new DOTSerializer("CST", file);
            ExportDOT_CST(serializer, null, 0, root);
            serializer.Close();
        }
        
        /// <summary>
        /// Exports the given CST node with the given serializer
        /// </summary>
        /// <param name="serializer">The DOT serializer</param>
        /// <param name="parent">The parent node ID</param>
        /// <param name="nextID">The next available ID for the generated DOT data</param>
        /// <param name="node">The node to serialize</param>
        /// <returns>The next available ID for the generate DOT data</returns>
        private static int ExportDOT_CST(DOTSerializer serializer, string parent, int nextID, ASTNode node)
        {
            string name = "node" + nextID;
            string label = node.Symbol.ToString();
            serializer.WriteNode(name, label, DOTNodeShape.circle);
            if (parent != null)
                serializer.WriteEdge(parent, name, string.Empty);
            int result = nextID + 1;
            foreach (ASTNode child in node.Children)
                result = ExportDOT_CST(serializer, name, result, child);
            return result;
        }


        /// <summary>
        /// Exports the given DFA to a DOT graph in the specified file
        /// </summary>
        /// <param name="dfa">The DFA to export</param>
        /// <param name="file">DOT file to export to</param>
        public static void ExportDOT(DFA dfa, string file)
        {
        	DOTSerializer serializer = new DOTSerializer("DFA", file);
            foreach (DFAState state in dfa.States)
            {
                if (state.TopItem != null)
                    serializer.WriteNode(state.ID.ToString(), state.ID.ToString() + " : " + state.TopItem.ToString(), DOTNodeShape.ellipse);
                else
                    serializer.WriteNode(state.ID.ToString());
            }
            foreach (DFAState state in dfa.States)
                foreach (CharSpan value in state.Transitions.Keys)
                    serializer.WriteEdge(state.ID.ToString(), state.Transitions[value].ID.ToString(), value.ToString());
            serializer.Close();
        }

		/// <summary>
		/// Exports the given NFA to a DOT graph in the specified file
		/// </summary>
		/// <param name="nfa">The NFA to export</param>
		/// <param name="file">DOT file to export to</param>
		public static void ExportDOT(NFA nfa, string file)
        {
        	DOTSerializer serializer = new DOTSerializer("DFA", file);
        	for (int i=0; i!=nfa.States.Count; i++)
            {
        		NFAState state = nfa.States[i];
                if (state.Item != null)
                    serializer.WriteNode(i.ToString(), i.ToString() + " : " + state.Item.ToString(), DOTNodeShape.ellipse);
                else
                    serializer.WriteNode(i.ToString());
            }
            for (int i=0; i!=nfa.States.Count; i++)
            {
        		NFAState state = nfa.States[i];
        		foreach (NFATransition transition in state.Transitions)
        		{
        			int to = nfa.States.IndexOf(transition.Next);
        			serializer.WriteEdge(i.ToString(), to.ToString(), transition.Span.ToString());
        		}
            }
            serializer.Close();
        }
    }
}
