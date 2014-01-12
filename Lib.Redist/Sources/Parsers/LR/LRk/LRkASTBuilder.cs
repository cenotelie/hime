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

using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents the builder of Parse Trees for LR(k) parsers
	/// </summary>
    class LRkASTBuilder : SemanticBody
    {
        // The graph being built
        private ASTGraph graph;

        // Stack of semantic objects
        private int[] stack;
        private int stackNext;

        // Reduction data
        private int subRoot;
        private int popCount;

        #region SemanticBody
        /// <summary>
        /// Gets the symbol at the i-th index in the cache
        /// </summary>
        /// <param name="index">Index of the symbol</param>
        /// <returns>The symbol at the given index</returns>
        public Symbol this[int index]
        {
            get
            {
                return graph.GetChild(subRoot, index).Symbol;
            }
        }
        /// <summary>
        /// Gets the length of this body
        /// </summary>
        public int Length { get { return graph.GetChildrenCount(subRoot); } }
        #endregion

        /// <summary>
        /// Initializes the builder with the given stack size
        /// </summary>
        /// <param name="stackSize">The maximal size of the stack</param>
        public LRkASTBuilder(int stackSize, TokenizedText text, SymbolDictionary variables, SymbolDictionary virtuals)
        {
            this.graph = new ASTGraph(text, variables, virtuals);
            this.stack = new int[stackSize];
            this.stackNext = 0;
        }

        /// <summary>
        /// Push a symbol onto the stack
        /// </summary>
        /// <param name="id">The symbol's ID</param>
        /// <param name="type">The symbol's type</param>
        /// <param name="index">The symbol's index in its respective table</param>
        public void StackPush(int id, byte type, int index)
        {
            int key = graph.CreateNode(id, type, index);
            stack[stackNext++] = key;
        }

        /// <summary>
        /// Prepares for the forthcoming reduction operations
        /// </summary>
        /// <param name="id">The reduced variable id</param>
        /// <param name="index">The reduced variable index</param>
        /// <param name="length">The length of the reduction</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionPrepare(int id, int index, int length, TreeAction action)
        {
            subRoot = graph.CreateNode(id, ASTGraph.TypeVariable, index, action);
            popCount = 0;
            stackNext -= length;
        }

        /// <summary>
        /// During a redution, pops the top symbol from the stack and gives it a tree action
        /// </summary>
        /// <param name="action">The tree action to apply to the symbol</param>
        public void ReductionPop(TreeAction action)
        {
            int sub = stack[stackNext + popCount];
            TreeAction subAction = graph.GetAction(sub);

            if (subAction == TreeAction.Replace)
            {
                // Copy the adjacency data
                graph.AddChildren(subRoot, sub);
            }
            else if (action == TreeAction.Drop)
            {
                // Do nothing to drop the child
            }
            else
            {
                graph.AddChild(subRoot, sub, (action != TreeAction.None) ? action : subAction);
            }
            popCount++;
        }

        /// <summary>
        /// During a reduction, inserts a virtual symbol
        /// </summary>
        /// <param name="symbol">The virtual symbol's id</param>
        /// <param name="index">The virtual symbol's index</param>
        /// <param name="action">The tree action applied onto the symbol</param>
        public void ReductionVirtual(int id, int index, TreeAction action)
        {
            if (action == TreeAction.Drop)
                return; // why would you do this?
            graph.AddChild(subRoot, graph.CreateNode(id, ASTGraph.TypeVirtual, index), action);
        }

        /// <summary>
        /// During a reduction, inserts a semantic action
        /// </summary>
        /// <param name="callback">The semantic action</param>
        public void ReductionSemantic(UserAction callback)
        {
            callback(graph.GetSymbol(subRoot), this);
        }

        /// <summary>
        /// Finalizes the reduction operation
        /// </summary>
        public void Reduce()
        {
            if (graph.GetAction(subRoot) != TreeAction.Replace)
                graph.ApplyPromotes(subRoot);
            // Put it on the stack
            stack[stackNext++] = subRoot;
        }

        /// <summary>
        /// Finalizes the parse tree and returns it
        /// </summary>
        /// <returns>The final parse tree</returns>
        public AST GetTree()
        {
            graph.SetupRoot(stack[stackNext - 2]);
            return graph;
        }
    }
}
