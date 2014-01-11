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

using System.Collections;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a generation of nodes in a GSS
	/// </summary>
    class GSSGeneration : IEnumerable<GSSNode>
    {
        private GSS stack;
        private int index;
        private int size;
        private GSSNode[] data;
        private BitArray marks;

        /// <summary>
        /// Gets the index of this generation
        /// </summary>
        public int Index { get { return index; } }
        /// <summary>
        /// Gets the size of this generation, i.e. the number of nodes it contains
        /// </summary>
        public int Size { get { return size; } }

        /// <summary>
        /// Initializes this generation
        /// </summary>
        /// <param name="stack">The parent GSS</param>
        /// <param name="index">The generation's index</param>
        /// <param name="nbstates">The number of states in the parent RNGLR automaton</param>
        public GSSGeneration(GSS stack, int index, int nbstates)
        {
            this.stack = stack;
            this.index = index;
            this.data = new GSSNode[nbstates];
            this.marks = new BitArray(nbstates);
        }

       	/// <summary>
       	/// Gets the GSS in this generation for the given RNGLR state, or null if there is none
       	/// </summary>
        public GSSNode this[int state] { get { return data[state]; } }

        /// <summary>
        /// Gets whether this generation contains a node for the given RNGLR state
        /// </summary>
        /// <param name="state">The RNGLR state</param>
        /// <returns>True if there is a node, false otherwise</returns>
        public bool Contains(int state) { return (data[state] == null); }

        /// <summary>
        /// Creates the GSS node in this generation for the given RNGLR state
        /// </summary>
        /// <param name="state">The RNGLR state</param>
        /// <returns>The corresponding GSS node</returns>
        public GSSNode CreateNode(int state)
        {
            GSSNode node = stack.AcquireNode();
            node.Initialize(this, state);
            data[state] = node;
            size++;
            return node;
        }
        
        /// <summary>
        /// Marks the given GSS node in this generation as having children in later generations
        /// </summary>
        /// <param name="node">The GSS node to be marked</param>
        public void Mark(GSSNode node)
        {
            marks[node.State] = true;
        }

        /// <summary>
        /// Collects unmarked GSS nodes in this generation and return them to the common pool
        /// </summary>
        public void Sweep()
        {
            int found = 0;
            for (int i = 0; i != data.Length; i++)
            {
                GSSNode node = data[i];
                if (node == null)
                    continue;
                found++;
                if (!marks[i])
                {
                    stack.ReturnNode(node);
                    data[i] = null;
                }
                if (found == size)
                    return;
            }
        }

        /// <summary>
        /// Gets an enumerator of the GSS nodes in this generation
        /// </summary>
        /// <returns>An enumerator of GSS nodes</returns>
        public IEnumerator<GSSNode> GetEnumerator() { return new Iterator(data); }

        /// <summary>
        /// Gets an enumerator of the GSS nodes in this generation
        /// </summary>
        /// <returns>An enumerator of GSS nodes</returns>
        IEnumerator IEnumerable.GetEnumerator() { return new Iterator(data); }

        private class Iterator : IEnumerator<GSSNode>
        {
            private GSSNode[] data;
            private int index;

            public Iterator(GSSNode[] data)
            {
                this.data = data;
                this.index = -1;
            }

            public void Dispose() { this.data = null; }
            public void Reset() { this.index = -1; }

            public GSSNode Current { get { return data[index]; } }
            object IEnumerator.Current { get { return data[index]; } }

            public bool MoveNext()
            {
                index++;
                while (index < data.Length && data[index] == null)
                    index++;
                return (index < data.Length);
            }
        }
    }
}
