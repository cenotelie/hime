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
using System.Collections.Specialized;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a Shared Packed Parse Forest as a graph of nodes
    /// </summary>
    class SPPF
    {
        /// <summary>
        /// Represents a node in a SPPF
        /// </summary>
        public struct Node
        {
            public int index;
            public Node(int index)
            {
                this.index = index;
            }
        }

        private const int initHistorySize = 8;
        private const int initHistoryPartSize = 64;
        
        /// <summary>
        /// Represents a generation of node in the current history
        /// </summary>
        private class HistoryPart
        {
            public int generation;  // The represented generation
            public Node[] data;     // The nodes at this generation
            public int index;       // Index for inserting new nodes

            public HistoryPart()
            {
                this.data = new Node[initHistoryPartSize];
                this.index = 0;
            }
        }

        // Graph data
        private Utils.BigList<ParseTree.Cell> data;
        private Utils.BitList replaceable;
        private Utils.BigList<Node> adjacentNodes;
        private Utils.BigList<TreeAction> adjacentActions;
        private int nextAdjacent;

        // History data
        private HistoryPart[] history;
        private int nextHP;
        private bool hitHistory;

        /// <summary>
        /// Initializes this SPPF
        /// </summary>
        public SPPF()
        {
            this.data = new Utils.BigList<ParseTree.Cell>();
            this.replaceable = new Utils.BitList();
            this.adjacentNodes = new Utils.BigList<Node>();
            this.adjacentActions = new Utils.BigList<TreeAction>();
            this.nextAdjacent = 0;
            this.history = new HistoryPart[initHistorySize];
            this.nextHP = 0;
            CreateNode(Symbols.Epsilon.Instance);
        }

        /// <summary>
        /// Gets the node for the epsilon symbol
        /// </summary>
        public Node Epsilon { get { return new Node(0); } }

        /// <summary>
        /// Creates a new node with the given symbol.
        /// This node will not be marked for replacement.
        /// </summary>
        /// <param name="symbol">The new node's symbol</param>
        /// <returns>The new node</returns>
        public Node CreateNode(Symbols.Symbol symbol)
        {
            int key = data.Add(new ParseTree.Cell(symbol));
            replaceable.Add(false);
            return new Node(key);
        }

        /// <summary>
        /// Creates a new node with the given symbol
        /// </summary>
        /// <param name="symbol">The new node's symbol</param>
        /// <param name="replaced">true if the created node should be marked for replacement</param>
        /// <returns>The new node</returns>
        public Node CreateNode(Symbols.Symbol symbol, bool replaced)
        {
            int key = data.Add(new ParseTree.Cell(symbol));
            replaceable.Add(replaced);
            return new Node(key);
        }

        /// <summary>
        /// Gets the symbol associated with the given node
        /// </summary>
        /// <param name="node">A SPPF node</param>
        /// <returns>The symbol associated with the given node</returns>
        public Symbols.Symbol GetNodeSymbol(Node node)
        {
            return data[node.index].symbol;
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
        public void AddChild(Node parent, Node child, TreeAction action)
        {
             // Get the data of the parent cell
            ParseTree.Cell pCell = this.data[parent.index];
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
            data[parent.index] = pCell;
            // Add the child to the adjacency list
            adjacentNodes.Add(child);
            adjacentActions.Add(action);
            nextAdjacent++;
        }

        /// <summary>
        /// Clears the current history
        /// </summary>
        public void ClearHistory()
        {
            nextHP = 0;
        }

        /// <summary>
        /// Gets whether the last call to resolve resulted in a history hit
        /// </summary>
        public bool HistoryHit { get { return hitHistory; } }

        /// <summary>
        /// Resolves a SPPF node against the current history.
        /// If a node is not found in the history, it will be created.
        /// </summary>
        /// <param name="generation">The identifier of the generation to look the node into</param>
        /// <param name="variable">The symbol associated to the node to look for</param>
        /// <param name="replaced">Whether the node to look for should be marked for replacement</param>
        /// <returns>The resolved SPPF node</returns>
        public Node Resolve(int generation, Symbols.Variable variable, bool replaced)
        {
            // Sets the hit flag to false
            hitHistory = false;
            // Try to resolve against existing generations
            for (int i = 0; i != nextHP; i++)
                if (history[i].generation == generation)
                    return Resolve(history[i], variable, replaced);
            
            // The generation to look for does not exists
            if (nextHP == history.Length)
            {
                // We are at the end of the history, extend it
                HistoryPart[] temp = new HistoryPart[history.Length + initHistorySize];
                Array.Copy(history, temp, history.Length);
                history = temp;
            }
            // The next history part has not been created yet
            if (history[nextHP] == null)
                history[nextHP] = new HistoryPart();
            // Setups the next history part
            history[nextHP].generation = generation;
            history[nextHP].index = 1;
            
            //  Create the result
            Node result = CreateNode(variable, replaced);
            // Push it into the history part
            history[nextHP].data[0] = result;
            
            nextHP++;
            return result;
        }

        /// <summary>
        /// Resolves a SPPf node against the given history part representing a generation
        /// </summary>
        /// <param name="part">The history part to look into</param>
        /// <param name="variable">The symbol associated to the node to look for</param>
        /// <param name="replaced">Whether the node to look for should be marked for replacement</param>
        /// <returns>The resolved SPPF node</returns>
        private Node Resolve(HistoryPart part, Symbols.Variable variable, bool replaced)
        {
            // Look for the given symbol in the node at this generation
            for (int i = 0; i != part.index; i++)
            {
                // If the symbol ID matches
                if (GetNodeSymbol(part.data[i]).SymbolID == variable.SymbolID)
                {
                    // This is a hit
                    hitHistory = true;
                    return part.data[i];
                }
            }
            if (part.index == part.data.Length)
            {
                Node[] temp = new Node[part.data.Length + initHistoryPartSize];
                Array.Copy(part.data, temp, part.data.Length);
                part.data = temp;
            }
            Node result = CreateNode(variable, replaced);
            part.data[part.index++] = result;
            return result;
        }

        /// <summary>
        /// Gets the parse tree starting at the given node
        /// </summary>
        /// <param name="node">The SPPF node to use as root of the parse tree</param>
        /// <returns>The parse tree</returns>
        public ParseTree BuildTreeFrom(Node node)
        {
            return null;
        }
    }
}