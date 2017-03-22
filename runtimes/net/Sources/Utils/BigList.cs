/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;

namespace Hime.Redist.Utils
{
	/// <summary>
	/// Represents a list of items that is efficient in storage and addition.
	/// Items cannot be removed or inserted.
	/// </summary>
	/// <typeparam name="T">The type of the stored items</typeparam>
	/// <remarks>
	/// The internal representation is an array of pointers to arrays of T.
	/// The basic arrays of T (chunks) have a fixed size.
	/// </remarks>
	public class BigList<T>
	{
		/// <summary>
		/// The number of bits allocated to the lowest part of the index (within a chunk)
		/// </summary>
		private const int UPPER_SHIFT = 8;
		/// <summary>
		/// The size of the chunks
		/// </summary>
		private const int CHUNKS_SIZE = 1 << UPPER_SHIFT;
		/// <summary>
		/// Bit mask for the lowest part of the index (within a chunk)
		/// </summary>
		private const int LOWER_MASK = CHUNKS_SIZE - 1;
		/// <summary>
		/// Initial size of the higer array (pointers to the chunks)
		/// </summary>
		private const int INIT_CHUNK_COUNT = CHUNKS_SIZE;

		/// <summary>
		/// The data
		/// </summary>
		private T[][] chunks;
		/// <summary>
		/// The index of the current chunk
		/// </summary>
		private int chunkIndex;
		/// <summary>
		/// The index of the next available cell within the current chunk
		/// </summary>
		private int cellIndex;

		/// <summary>
		/// Initializes this list
		/// </summary>
		public BigList()
		{
			chunks = new T[INIT_CHUNK_COUNT][];
			chunks[0] = new T[CHUNKS_SIZE];
			chunkIndex = 0;
			cellIndex = 0;
		}

		/// <summary>
		/// Gets the size of this list
		/// </summary>
		public int Size
		{
			get { return (chunkIndex * CHUNKS_SIZE) + cellIndex; }
		}

		/// <summary>
		/// Gets or sets the value of the item at the given index
		/// </summary>
		/// <param name="index">Index of an item</param>
		/// <returns>The value of the item at the given index</returns>
		public T this[int index]
		{
			get { return chunks[index >> UPPER_SHIFT][index & LOWER_MASK]; }
			set { chunks[index >> UPPER_SHIFT][index & LOWER_MASK] = value; }
		}

		/// <summary>
		/// Copies the specified range of items to the given buffer
		/// </summary>
		/// <param name="index">The starting index of the range of items to copy</param>
		/// <param name="count">The size of the range of items to copy</param>
		/// <param name="buffer">The buffer to copy the items in</param>
		/// <param name="start">The starting index within the buffer to copy the items to</param>
		public void CopyTo(int index, int count, T[] buffer, int start)
		{
			int indexUpper = index >> UPPER_SHIFT;
			int indexLower = index & LOWER_MASK;
			while (indexLower + count >= CHUNKS_SIZE)
			{
				// while we can copy chunks
				int length = CHUNKS_SIZE - indexLower;
				Array.Copy(chunks[indexUpper], indexLower, buffer, start, length);
				count -= length;
				start += length;
				indexUpper++;
				indexLower = 0;
			}
			if (count > 0)
			{
				Array.Copy(chunks[indexUpper], indexLower, buffer, start, count);
			}
		}

		/// <summary>
		/// Adds the given value at the end of this list
		/// </summary>
		/// <param name="value">The value to add</param>
		/// <returns>The index of the value in this list</returns>
		public int Add(T value)
		{
			if (cellIndex == CHUNKS_SIZE)
				AddChunk();
			chunks[chunkIndex][cellIndex] = value;
			int index = (chunkIndex << UPPER_SHIFT | cellIndex);
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
			int start = Size;
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
			int start = Size;
			if (count <= 0)
				return start;
			int chunk = from >> UPPER_SHIFT;     // The current chunk to copy from
			int cell = from & LOWER_MASK;        // The current starting index in the chunk
			while (cell + count > CHUNKS_SIZE)
			{
				DoCopy(chunks[chunk], cell, CHUNKS_SIZE - cell);
				count -= CHUNKS_SIZE - cell;
				chunk++;
				cell = 0;
			}
			DoCopy(chunks[chunk], cell, count);
			return start;
		}

		/// <summary>
		/// Removes the specified number of values from the end of this list
		/// </summary>
		/// <param name="count">The number of values to remove</param>
		public void Remove(int count)
		{
			chunkIndex -= count >> UPPER_SHIFT;
			cellIndex -= count & LOWER_MASK;
		}

		/// <summary>
		/// Copies the given values at the end of this list
		/// </summary>
		/// <param name="values">The values to add</param>
		/// <param name="index">The starting index of the values to store</param>
		/// <param name="length">The number of values to store</param>
		private void DoCopy(T[] values, int index, int length)
		{
			while (cellIndex + length > CHUNKS_SIZE)
			{
				int count = CHUNKS_SIZE - cellIndex;
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
			if (chunkIndex == chunks.Length - 1)
				Array.Resize(ref chunks, chunks.Length + INIT_CHUNK_COUNT);
			chunkIndex++;
			T[] chunk = chunks[chunkIndex];
			if (chunk == null)
			{
				chunk = new T[CHUNKS_SIZE];
				chunks[chunkIndex] = chunk;
			}
			cellIndex = 0;
		}
	}
}