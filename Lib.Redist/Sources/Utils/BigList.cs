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
        private const int upperShift = 13;
        private const int chunksSize = 1 << upperShift;
        private const int lowerMask = chunksSize - 1;
        private const int initChunkCount = chunksSize;

        private T[][] chunks;   // The data
        private int chunkIndex; // The index of the current chunk being filled
        private int cellIndex;  // The index of the next cell to be filled (relatively to the selected chunk)

        /// <summary>
        /// Initializes this list
        /// </summary>
        public BigList()
        {
            this.chunks = new T[initChunkCount][];
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
            if (length > 0)
                DoCopy(values, index, length);
            return start;
        }

        /// <summary>
        /// Copies the values from the given index at the end of the list
        /// </summary>
        /// <param name="from">The index to start copy from</param>
        /// <param name="count">The number of items to copy</param>
        /// <returns>The index within this list at which the values have been copied to</returns>
        public int Duplicate(int from, int count)
        {
            int start = (chunkIndex << upperShift | cellIndex);
            if (count <= 0)
                return start;

            int chunk = from >> upperShift;     // The current chunk to copy from
            int cell = from & lowerMask;        // The current starting index in the chunk
            while (cell + count > chunksSize)
            {
                DoCopy(chunks[chunk], cell, chunksSize - cell);
                count -= chunksSize - cell;
                chunk++;
                cell = 0;
            }
            DoCopy(chunks[chunk], cell, count);

            return start;
        }

        /// <summary>
        /// Copies the given values at the end of this list
        /// </summary>
        /// <param name="values">The values to add</param>
        /// <param name="index">The starting index of the values to store</param>
        /// <param name="length">The number of values to store</param>
        private void DoCopy(T[] values, int index, int length)
        {
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
        }

        /// <summary>
        /// Adds a new (empty) chunk of cells
        /// </summary>
        private void AddChunk()
        {
            T[] t = new T[chunksSize];
            if (chunkIndex == chunks.Length - 1)
            {
                T[][] r = new T[chunks.Length + initChunkCount][];
                Array.Copy(chunks, r, chunks.Length);
                chunks = r;
            }
            chunks[++chunkIndex] = t;
            cellIndex = 0;
        }
    }
}