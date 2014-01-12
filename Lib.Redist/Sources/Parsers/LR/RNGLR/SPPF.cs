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
    class SPPF : ASTGraph
    {
        private const int initHistorySize = 8;
        private const int initHistoryPartSize = 64;
        
        /// <summary>
        /// Represents a generation of node in the current history
        /// </summary>
        private class HistoryPart
        {
            public int generation;  // The represented generation
            public int[] data;      // The nodes at this generation
            public int index;       // Index for inserting new nodes

            public HistoryPart()
            {
                this.data = new int[initHistoryPartSize];
                this.index = 0;
            }
        }

        private HistoryPart[] history;
        private int nextHP;
        private bool hitHistory;

        /// <summary>
        /// Initializes this SPPF
        /// </summary>
        public SPPF()
        {
            this.history = new HistoryPart[initHistorySize];
            this.nextHP = 0;
            CreateNode(Symbols.Epsilon.Instance);
        }

        /// <summary>
        /// Gets the node for the epsilon symbol
        /// </summary>
        public int Epsilon { get { return 0; } }

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
        /// <param name="action">The action applied on this node</param>
        /// <returns>The resolved SPPF node</returns>
        public int Resolve(int generation, Symbols.Variable variable, TreeAction action)
        {
            // Sets the hit flag to false
            hitHistory = false;
            // Try to resolve against existing generations
            for (int i = 0; i != nextHP; i++)
                if (history[i].generation == generation)
                    return Resolve(history[i], variable, action);
            
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
            int result = CreateNode(variable, action);
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
        /// <param name="action">The action applied on this node</param>
        /// <returns>The resolved SPPF node</returns>
        private int Resolve(HistoryPart part, Symbols.Variable variable, TreeAction action)
        {
            // Look for the given symbol in the node at this generation
            for (int i = 0; i != part.index; i++)
            {
                // If the symbol ID matches
                if (meta[part.data[i]].originalSID == variable.SymbolID)
                {
                    // This is a hit
                    hitHistory = true;
                    return part.data[i];
                }
            }
            if (part.index == part.data.Length)
            {
                int[] temp = new int[part.data.Length + initHistoryPartSize];
                Array.Copy(part.data, temp, part.data.Length);
                part.data = temp;
            }
            int result = CreateNode(variable, action);
            part.data[part.index++] = result;
            return result;
        }
    }
}