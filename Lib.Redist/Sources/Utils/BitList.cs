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
using System.Collections;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a list of booleans that is efficient in storage and addition.
    /// Items cannot be removed or inserted.
    /// </summary>
    class BitList
    {
        private const int upperShift = 10;
        private const int chunksSize = 1 << upperShift;
        private const int lowerMask = chunksSize - 1;
        
        private BitArray[] chunks;   // The data
        private int chunkIndex; // The index of the current chunk being filled
        private int cellIndex;  // The index of the next cell to be filled (relatively to the selected chunk)

        /// <summary>
        /// Initializes this list
        /// </summary>
        public BitList()
        {
            this.chunks = new BitArray[chunksSize];
            this.chunks[0] = new BitArray(chunksSize);
            this.chunkIndex = 0;
            this.cellIndex = 0;
        }

        /// <summary>
        /// Gets the size of this list
        /// </summary>
        public int Size
        {
            get { return (chunkIndex * chunksSize) + cellIndex; }
        }

        /// <summary>
        /// Gets or sets the value of the item at the given index
        /// </summary>
        /// <param name="index">Index of an item</param>
        /// <returns>The value of the item at the given index</returns>
        public bool this[int index]
        {
            get { return chunks[index >> upperShift][index & lowerMask]; }
            set { chunks[index >> upperShift][index & lowerMask] = value; }
        }

        /// <summary>
        /// Adds the given value at the end of this list
        /// </summary>
        /// <param name="value">The value to add</param>
        public void Add(bool value)
        {
            if (cellIndex == chunksSize)
                AddChunk();
            chunks[chunkIndex][cellIndex] = value;
            cellIndex++;
        }

        /// <summary>
        /// Adds a new (empty) chunk of cells
        /// </summary>
        private void AddChunk()
        {
            BitArray t = new BitArray(chunksSize);
            if (chunkIndex == chunks.Length - 1)
            {
                BitArray[] r = new BitArray[chunks.Length + chunksSize];
                Array.Copy(chunks, r, chunks.Length);
                chunks = r;
            }
            chunks[++chunkIndex] = t;
            cellIndex = 0;
        }
    }
}