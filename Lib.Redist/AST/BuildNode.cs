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

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in the raw AST produced by a LR(k) parser before tree actions
    /// Tree actions are directly executed on this structure, producing the final AST
    /// </summary>
    class BuildNode
    {
        /// <summary>
        /// The final AST node that would correspond to this node
        /// </summary>
        protected ASTNode value;
        /// <summary>
        /// The children of this node
        /// </summary>
        protected BuildNode[] children;
        /// <summary>
        /// The AST action for this node
        /// </summary>
        protected Parsers.LRTreeAction action;

        /// <summary>
        /// Gets the final AST node for this node
        /// </summary>
        public ASTNode Value { get { return value; } }

        /// <summary>
        /// Initializes this build node with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol for this node</param>
        public BuildNode(Symbols.Symbol symbol)
        {
            this.value = new ASTNode(symbol);
        }

        /// <summary>
        /// Sets the tree action for this node, provided it is not 0 (no action)
        /// </summary>
        /// <param name="action">The tree action for this node</param>
        public void SetAction(Parsers.LRTreeAction action)
        {
            if (action != Parsers.LRTreeAction.None)
                this.action = action;
        }

        /// <summary>
        /// Builds the AST at this level with the given children
        /// </summary>
        /// <typeparam name="T">The type of children</typeparam>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        public void Build<T>(T[] children, int length) where T : BuildNode
        {
            if (action == Parsers.LRTreeAction.Replace)
                BuildReplaceable(children, length);
            else
                BuildNormal(children, length);
        }

        /// <summary>
        /// Builds the AST at this level as a replaceable node
        /// </summary>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        private void BuildReplaceable<T>(T[] children, int length) where T : BuildNode
        {
            int size = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRTreeAction.Replace:
                        size += child.children.Length;
                        break;
                    case Parsers.LRTreeAction.Drop:
                        break;
                    default:
                        size++;
                        break;
                }
            }
            BuildNode[] replacement = new BuildNode[size];
            int index = 0;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRTreeAction.Replace:
                        System.Array.Copy(child.children, 0, replacement, index, child.children.Length);
                        index += child.children.Length;
                        break;
                    case Parsers.LRTreeAction.Drop:
                        break;
                    default:
                        replacement[index] = child;
                        index++;
                        break;
                }
            }
            this.children = replacement;
        }

        /// <summary>
        /// Builds the AST at this level as a normal node
        /// </summary>
        /// <param name="children">Array of children</param>
        /// <param name="length">Number of children</param>
        private void BuildNormal<T>(T[] children, int length) where T : BuildNode
        {
            bool firstPromote = true;
            for (int i = 0; i != length; i++)
            {
                BuildNode child = children[i];
                switch (child.action)
                {
                    case Parsers.LRTreeAction.Replace:
                        foreach (BuildNode subchild in child.children)
                            firstPromote = ExecuteReplacement(subchild, firstPromote);
                        break;
                    case Parsers.LRTreeAction.Drop:
                        break;
                    case Parsers.LRTreeAction.Promote:
                        if (firstPromote)
                        {
                            child.value.Children.InsertRange(0, this.value.Children);
                            this.value = child.value;
                            firstPromote = false;
                        }
                        else
                        {
                            child.value.Children.Insert(0, this.value);
                            this.value = child.value;
                        }
                        break;
                    default:
                        this.value.Children.Add(child.value);
                        break;
                }
            }
        }

        private bool ExecuteReplacement(BuildNode child, bool firstPromote)
        {
            if (child.action != Parsers.LRTreeAction.Promote)
            {
                this.value.Children.Add(child.value);
                return firstPromote;
            }
            else if (firstPromote)
            {
                child.value.Children.InsertRange(0, this.value.Children);
                this.value = child.value;
            }
            else
            {
                child.value.Children.Insert(0, this.value);
                this.value = child.value;
            }
            return false;
        }
    }
}
