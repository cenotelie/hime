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
        public const byte TypeToken = 1;
        public const byte TypeVariable = 2;
        public const byte TypeVirtual = 3;

        /// <summary>
        /// Represents a label on a graph's node
        /// </summary>
        protected struct Label
        {
            public int id;      // symbol's identifier
            public int index;   // index of the symbol in its respective table
            public byte type;   // symbol's type
            public Label(int id, byte type, int index)
            {
                this.id = id;
                this.index = index;
                this.type = type;
            }
        }

        /// <summary>
        /// Represents the data of a
        /// </summary>
        protected struct Node
        {
            public int originalSID;     // original sid of the node
            public int first;           // index of the first child in the adjacency table
            public int count;           // number of children
            public TreeAction action;   // the action to be applied on this node
            public Node(int sid)
            {
                this.originalSID = sid;
                this.first = 0;
                this.count = 0;
                this.action = TreeAction.None;
            }
            public Node(int sid, TreeAction action)
            {
                this.originalSID = sid;
                this.first = 0;
                this.count = 0;
                this.action = action;
            }
        }

        /// <summary>
        /// Represents an edge in this graph
        /// </summary>
        protected struct Adjacent
        {
            public int child;           // Index of the child node
            public TreeAction action;   // Action for this child
            public Adjacent(int child, TreeAction action)
            {
                this.child = child;
                this.action = action;
            }
        }

        // Symbol table data
        protected TokenizedText tableTokens;
        protected SymbolDictionary tableVariables;
        protected SymbolDictionary tableVirtuals;

        // Graph data
        protected Utils.BigList<Label> labels;
        protected Utils.BigList<Node> nodes;
        protected Utils.BigList<Adjacent> adjacents;
        protected int nextAdjacent;
        protected int root;

        /// <summary>
        /// Initializes this SPPF
        /// </summary>
        public ASTGraph(TokenizedText text, SymbolDictionary variables, SymbolDictionary virtuals)
        {
            this.tableTokens = text;
            this.tableVariables = variables;
            this.tableVirtuals = virtuals;

            this.labels = new Utils.BigList<Label>();
            this.nodes = new Utils.BigList<Node>();
            this.adjacents = new Utils.BigList<Adjacent>();
            this.nextAdjacent = 0;
            this.root = -1;
        }

        /// <summary>
        /// Gets the root node of this tree
        /// </summary>
        public ASTNode Root { get { return new ASTNode(this, this.root); } }

        /// <summary>
        /// Gets the symbol of the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The node's symbol</returns>
        public Symbol GetSymbol(int node)
        {
            Label label = labels[node];
            switch (label.type)
            {
                case TypeToken:
                    return tableTokens.GetSymbolAt(label.index);
                case TypeVariable:
                    return tableVariables[label.index];
                case TypeVirtual:
                    return tableVirtuals[label.index];
            }
            // This cannot happen
            return new Symbol(0, string.Empty);
        }

        /// <summary>
        /// Gets the tree action marking the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The action marking the node</returns>
        public TreeAction GetAction(int node)
        {
            return nodes[node].action;
        }

        /// <summary>
        /// Gets the number of children of the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The node's numer of children</returns>
        public int GetChildrenCount(int node)
        {
            return nodes[node].count;
        }

        /// <summary>
        /// Gets the i-th child of the given node
        /// </summary>
        /// <param name="parent">A node</param>
        /// <param name="i">The child's number</param>
        /// <returns>The i-th child</returns>
        public ASTNode GetChild(int parent, int i)
        {
            return new ASTNode(this, adjacents[nodes[parent].first + i].child);
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
        /// Gets the position in the input text of the given node
        /// </summary>
        /// <param name="node">A node</param>
        /// <returns>The position in the text</returns>
        public TextPosition GetPosition(int node)
        {
            Label label = labels[node];
            if (label.type == TypeToken)
                return tableTokens.GetPositionOf(tableTokens[label.index]); ;
            return new TextPosition(0, 0);
        }

        /// <summary>
        /// Creates a new node with the given symbol.
        /// This node will not be marked for replacement.
        /// </summary>
        /// <param name="id">The symbol's ID</param>
        /// <param name="type">The symbol's type</param>
        /// <param name="index">The symbol's index in its respective table</param>
        /// <returns>The new node</returns>
        public int CreateNode(int id, byte type, int index)
        {
            labels.Add(new Label(id, type, index));
            return nodes.Add(new Node(id));
        }

        /// <summary>
        /// Creates a new node with the given symbol
        /// </summary>
        /// <param name="id">The symbol's ID</param>
        /// <param name="type">The symbol's type</param>
        /// <param name="index">The symbol's index in its respective table</param>
        /// <param name="action">The action applied on this node</param>
        /// <returns>The new node</returns>
        public int CreateNode(int id, byte type, int index, TreeAction action)
        {
            labels.Add(new Label(id, type, index));
            return nodes.Add(new Node(id, action));
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
            Node pNode = nodes[parent];
            if (pNode.count == 0)
            {
                // This is the first child => fills the parent data
                pNode.first = nextAdjacent;
                pNode.count = 1;
            }
            else
            {
                // Just increment the number of children
                pNode.count = pNode.count + 1;
            }
            // Push the parent data back into the node list
            nodes[parent] = pNode;
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
            Node pNode = nodes[parent];
            Node fNode = nodes[from];
            if (pNode.count == 0)
            {
                pNode.first = nextAdjacent;
                pNode.count = fNode.count;
            }
            else
            {
                pNode.count += fNode.count;
            }
            // Push the data back into the node list
            nodes[parent] = pNode;
            // Do the copy
            adjacents.Duplicate(fNode.first, fNode.count);
            nextAdjacent += fNode.count;
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
            Node rootNode = nodes[subRoot];

            // Pre-check for promote actions
            bool hasPromotion = false;
            for (int i = 0; i != rootNode.count; i++)
            {
                if (adjacents[rootNode.first + i].action == TreeAction.Promote)
                {
                    hasPromotion = true;
                    break;
                }
            }
            // No promotion found => return
            if (!hasPromotion)
                return;

            // The data of the current root
            Node current = rootNode;
            current.first = nextAdjacent;
            current.count = 0;
            
            // Apply the promotion actions
            // Index of the promoted node
            int promoted = -1;
            for (int i = 0; i != rootNode.count; i++)
            {
                Adjacent adjacent = adjacents[rootNode.first + i];
                if (adjacent.action == TreeAction.Promote)
                {
                    // Get the child data
                    Node child = nodes[adjacent.child];
                    if (promoted != -1)
                    {
                        // This is not the first promotion
                        // Save the promoted node data
                        labels[promoted] = labels[subRoot];
                        nodes[promoted] = current;
                        // Initialize a new set of children
                        current.first = nextAdjacent;
                        current.count = 1;
                        adjacents.Add(new Adjacent(promoted, TreeAction.None));
                        nextAdjacent++;
                    }
                    promoted = adjacent.child;
                    // Promote the symbol
                    labels[subRoot] = labels[adjacent.child];
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
            nodes[subRoot] = current;
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
                this.first = graph.nodes[node].first;
                this.current = this.first - 1;
                this.end = this.first + graph.nodes[node].count;
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