/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a graph of nodes that generalizes an AST
    /// </summary>
    class ASTGraph : AST
    {
        protected struct Cell
        {
            public Symbols.Symbol symbol;   // node's symbol
            public int first;               // first adjacent child
            public int count;               // number of adjacents
            public Cell(Symbols.Symbol symbol)
            {
                this.symbol = symbol;
                this.first = 0;
                this.count = 0;
            }
            public Cell(Symbols.Symbol symbol, int first, int count)
            {
                this.symbol = symbol;
                this.first = first;
                this.count = count;
            }
        }

        protected struct Meta
        {
            public int originalSID;
            public TreeAction headAction;
            public Meta(int originalSID)
            {
                this.originalSID = originalSID;
                this.headAction = TreeAction.None;
            }
            public Meta(int originalSID, TreeAction headAction)
            {
                this.originalSID = originalSID;
                this.headAction = headAction;
            }
        }

        protected struct Adjacent
        {
            public int child;
            public TreeAction action;
            public Adjacent(int child, TreeAction action)
            {
                this.child = child;
                this.action = action;
            }
        }

        // Graph data
        protected Utils.BigList<Cell> cells;
        protected Utils.BigList<Meta> meta;
        protected Utils.BigList<Adjacent> adjacents;
        protected int nextAdjacent;
        protected int root;

        /// <summary>
        /// Initializes this SPPF
        /// </summary>
        public ASTGraph()
        {
            this.cells = new Utils.BigList<Cell>();
            this.meta = new Utils.BigList<Meta>();
            this.adjacents = new Utils.BigList<Adjacent>();
            this.nextAdjacent = 0;
            this.root = -1;
            CreateNode(Symbols.Epsilon.Instance);
        }

        /// <summary>
        /// Gets the node for the epsilon symbol
        /// </summary>
        public int Epsilon { get { return 0; } }

        /// <summary>
        /// Gets the root node of this tree
        /// </summary>
        public ASTNode Root { get { return new ASTNode(this, this.root); } }

        /// <summary>
        /// Gets the symbol of the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The node's symbol</returns>
        public Symbols.Symbol GetSymbol(int node)
        {
            return cells[node].symbol;
        }

        /// <summary>
        /// Gets the tree action marking the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The action marking the node</returns>
        public TreeAction GetAction(int node)
        {
            return meta[node].headAction;
        }

        /// <summary>
        /// Gets the number of children of the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The node's numer of children</returns>
        public int GetChildrenCount(int node)
        {
            return cells[node].count;
        }

        /// <summary>
        /// Gets the i-th child of the given node
        /// </summary>
        /// <param name="parent">A node</param>
        /// <param name="i">The child's number</param>
        /// <returns>The i-th child</returns>
        public ASTNode GetChild(int parent, int i)
        {
            return new ASTNode(this, adjacents[cells[parent].first + i].child);
        }

        /// <summary>
        /// Gets an enumerator for the children of the given node
        /// </summary>
        /// <param name="parent">A node</param>
        /// <returns>An enumerator for the children</returns>
        public IEnumerator<ASTNode> GetChildren(int parent)
        {
            return new ChildEnumerator(this, parent);
        }

        /// <summary>
        /// Creates a new node with the given symbol.
        /// This node will not be marked for replacement.
        /// </summary>
        /// <param name="symbol">The new node's symbol</param>
        /// <returns>The new node</returns>
        public int CreateNode(Symbols.Symbol symbol)
        {
            cells.Add(new Cell(symbol));
            return meta.Add(new Meta(symbol.SymbolID));
        }

        /// <summary>
        /// Creates a new node with the given symbol
        /// </summary>
        /// <param name="symbol">The new node's symbol</param>
        /// <param name="action">The action applied on this node</param>
        /// <returns>The new node</returns>
        public int CreateNode(Symbols.Symbol symbol, TreeAction action)
        {
            cells.Add(new Cell(symbol));
            return meta.Add(new Meta(symbol.SymbolID, action));
        }

        /// <summary>
        /// Adds a new child to a parent node
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <param name="child">The new child node</param>
        /// <param name="action">The tree action associated to the child</param>
        /// <remarks>
        /// Warning: This method must be called sequentially for all the children of a parent node.
        /// Once new parent node is being built, it is not possible to add children to a previous parent anymore.
        /// </remarks>
        public void AddChild(int parent, int child, TreeAction action)
        {
            // Get the data of the parent cell
            Cell pCell = this.cells[parent];
            if (pCell.count == 0)
            {
                // This is the first child => fills the parent data
                pCell.first = nextAdjacent;
                pCell.count = 1;
            }
            else
            {
                // Just increment the number of children
                pCell.count = pCell.count + 1;
            }
            // Push the parent data back into the node list
            cells[parent] = pCell;
            // Add the child to the adjacency list
            adjacents.Add(new Adjacent(child, action));
            nextAdjacent++;
        }

        /// <summary>
        /// Copies the children from a node as children to another
        /// </summary>
        /// <param name="parent">Node receiving the copies</param>
        /// <param name="from">Node whose children are to be copied</param>
        public void AddChildren(int parent, int from)
        {
            Cell pCell = this.cells[parent];
            Cell fCell = this.cells[from];
            if (pCell.count == 0)
            {
                pCell.first = nextAdjacent;
                pCell.count = fCell.count;
            }
            else
            {
                pCell.count += fCell.count;
            }
            // Push the data back into the node list
            cells[parent] = pCell;
            // Do the copy
            adjacents.Duplicate(fCell.first, fCell.count);
            nextAdjacent += fCell.count;
        }

        /// <summary>
        /// Sets the given node as the root of the inner AST
        /// </summary>
        /// <param name="node">The node that will be the AST's root</param>
        public void SetupRoot(int node)
        {
            this.root = node;
        }

        /// <summary>
        /// Applies the promote actions on the children of the given sub root
        /// </summary>
        /// <param name="subRoot">A sub root</param>
        public void ApplyPromotes(int subRoot)
        {
            // Get the root data
            Cell rootCell = cells[subRoot];

            // Pre-check for promote actions
            bool hasPromotion = false;
            for (int i = 0; i != rootCell.count; i++)
            {
                if (adjacents[rootCell.first + i].action == TreeAction.Promote)
                {
                    hasPromotion = true;
                    break;
                }
            }
            // No promotion found => return
            if (!hasPromotion)
                return;

            // The data of the current root
            Cell current = rootCell;
            current.first = nextAdjacent;
            current.count = 0;
            
            // Apply the promotion actions
            // Index of the promoted node
            int promoted = -1;
            for (int i = 0; i != rootCell.count; i++)
            {
                Adjacent adjacent = adjacents[rootCell.first + i];
                if (adjacent.action == TreeAction.Promote)
                {
                    // Get the child data
                    Cell child = cells[adjacent.child];
                    if (promoted != -1)
                    {
                        // This is not the first promotion
                        // Save the promoted node data
                        cells[promoted] = current;
                        // Initialize a new set of children
                        current.first = nextAdjacent;
                        current.count = 1;
                        adjacents.Add(new Adjacent(promoted, TreeAction.None));
                        nextAdjacent++;
                    }
                    promoted = adjacent.child;
                    // Promote the symbol
                    current.symbol = child.symbol;
                    // Add the children
                    adjacents.Duplicate(child.first, child.count);
                    nextAdjacent += child.count;
                    current.count += child.count;
                }
                else
                {
                    // Just add the child
                    adjacents.Add(adjacent);
                    nextAdjacent++;
                    current.count++;
                }
            }

            // Push back the root data
            cells[subRoot] = current;
        }

        /// <summary>
        /// Represents and iterator for adjacents in this graph
        /// </summary>
        private class ChildEnumerator : IEnumerator<ASTNode>
        {
            private ASTGraph graph;
            private int first;
            private int current;
            private int end;

            public ChildEnumerator(ASTGraph graph, int node)
            {
                this.graph = graph;
                this.first = graph.cells[node].first;
                this.current = this.first - 1;
                this.end = this.first + graph.cells[node].count;
            }

            /// <summary>
            /// Gets the current node
            /// </summary>
            public ASTNode Current { get { return new ASTNode(graph, graph.adjacents[current].child); } }

            /// <summary>
            /// Gets the current node
            /// </summary>
            object System.Collections.IEnumerator.Current { get { return new ASTNode(graph, graph.adjacents[current].child); } }

            /// <summary>
            /// Disposes this enumerator
            /// </summary>
            public void Dispose()
            {
                graph = null;
            }

            /// <summary>
            /// Moves to the next node
            /// </summary>
            /// <returns>true if there are more nodes</returns>
            public bool MoveNext()
            {
                current++;
                return (current != end);
            }

            /// <summary>
            /// Resets this enumerator to the beginning
            /// </summary>
            public void Reset()
            {
                current = first - 1;
            }
        }
    }
}