/**********************************************************************
* Copyright (c) 2015 Laurent Wouters and others
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

namespace Hime.Redist.Utils
{
	/// <summary>
	/// Fast reusable buffer
	/// </summary>
	/// <typeparam name="T">The type of elements in this buffer</typeparam>
	struct Buffer<T>
	{
		/// <summary>
		/// The inner data backing this buffer
		/// </summary>
		private T[] inner;
		/// <summary>
		/// The number of elements in this buffer
		/// </summary>
		private int size;

		/// <summary>
		/// Gets the number of elements in this buffer
		/// </summary>
		public int Size { get { return size; } }

		/// <summary>
		/// Gets the i-th element of this buffer
		/// </summary>
		/// <param name="index">Index within this buffer</param>
		/// <returns>The i-th element</returns>
		public T this[int index] { get { return inner[index]; } }

		/// <summary>
		/// Initializes this buffer
		/// </summary>
		/// <param name="capacity">The initial capacity</param>
		public Buffer(int capacity)
		{
			this.inner = new T[capacity];
			this.size = 0;
		}

		/// <summary>
		/// Resets the content of this buffer
		/// </summary>
		public void Reset()
		{
			this.size = 0;
		}

		/// <summary>
		/// Adds an element to this buffer
		/// </summary>
		/// <param name="element">An element</param>
		public void Add(T element)
		{
			if (size == inner.Length)
				System.Array.Resize(ref inner, inner.Length * 2);
			inner[size] = element;
			size++;
		}
	}
}
