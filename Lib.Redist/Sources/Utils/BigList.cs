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

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a list of items that is efficient in storage and addition.
    /// Items cannot be removed or inserted.
    /// </summary>
    /// <typeparam name="T">The type of the stored items</typeparam>
    class BigList<T>
    {
        protected const int upperShift = 10;
        protected const int chunksSize = 1 << upperShift;
        protected const int lowerMask = chunksSize - 1;

        protected T[][] chunks;   // The data
        protected int chunkIndex; // The index of the current chunk being filled
        protected int cellIndex;  // The index of the next cell to be filled (relatively to the selected chunk)

        /// <summary>
        /// Initializes this list
        /// </summary>
        public BigList()
        {
            this.chunks = new T[chunksSize][];
            this.chunks[0] = new T[chunksSize];
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
        public T this[int index]
        {
            get { return chunks[index >> upperShift][index & lowerMask]; }
            set { chunks[index >> upperShift][index & lowerMask] = value; }
        }

        /// <summary>
        /// Adds the given value at the end of this list
        /// </summary>
        /// <param name="value">The value to add</param>
        /// <returns>The index of the value in this list</returns>
        public int Add(T value)
        {
            if (cellIndex == chunksSize)
                AddChunk();
            chunks[chunkIndex][cellIndex] = value;
            int index = (chunkIndex << upperShift | cellIndex);
            cellIndex++;
            return index;
        }

        /// <summary>
        /// Copies the given values at the end of this list
        /// </summary>
        /// <param name="values">The values to add</param>
        /// <param name="index">The starting index of the values to store</param>
        /// <param name="length">The number of values to store</param>
        /// <returns>The index within this list at which the values have been added</returns>
        public int Add(T[] values, int index, int length)
        {
            int start = (chunkIndex << upperShift | cellIndex);
            while (cellIndex + length > chunksSize)
            {
                int count = chunksSize - cellIndex;
                if (count == 0)
                {
                    AddChunk();
                    continue;
                }
                Array.Copy(values, index, chunks[chunkIndex], cellIndex, count);
                index += count;
                length -= count;
                AddChunk();
            }
            Array.Copy(values, index, chunks[chunkIndex], cellIndex, length);
            cellIndex += length;
            return start;
        }

        /// <summary>
        /// Adds a new (empty) chunk of cells
        /// </summary>
        private void AddChunk()
        {
            T[] t = new T[chunksSize];
            if (chunkIndex == chunks.Length - 1)
            {
                T[][] r = new T[chunks.Length + chunksSize][];
                Array.Copy(chunks, r, chunks.Length);
                chunks = r;
            }
            chunks[++chunkIndex] = t;
            cellIndex = 0;
        }
    }
}