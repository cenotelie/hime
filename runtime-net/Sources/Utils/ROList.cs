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

using System.Collections;
using System.Collections.Generic;

namespace Hime.Redist.Utils
{
	/// <summary>
	/// Represents a lightweight interface for a readonly list of T elements
	/// </summary>
	/// <typeparam name="T">The type of elements in this list</typeparam>
	public struct ROList<T> : IEnumerable<T>
	{
		/// <summary>
		/// The inner data set
		/// </summary>
		private readonly IList<T> inner;

		/// <summary>
		/// Gets the number of elements in this list
		/// </summary>
		public int Count { get { return inner.Count; } }

		/// <summary>
		/// Gets the element at the specified index
		/// </summary>
		/// <param name="index">An index in this list</param>
		public T this[int index] { get { return inner[index]; } }

		/// <summary>
		/// Initializes this list
		/// </summary>
		/// <param name="original">The original items</param>
		public ROList(IList<T> original)
		{
			inner = original;
		}

		/// <summary>
		/// Determines whether this list contains the specified item
		/// </summary>
		/// <param name="item">The item to look for</param>
		/// <returns><c>true</c> if the item is in this list</returns>
		public bool Contains(T item)
		{
			return inner.Contains(item);
		}

		/// <summary>
		/// Determines the index of the specified item in this list
		/// </summary>
		/// <param name="item">The item to look for</param>
		/// <returns>The index of the specified item, or -1</returns>
		public int IndexOf(T item)
		{
			return inner.IndexOf(item);
		}

		/// <summary>
		/// Gets the enumerator
		/// </summary>
		/// <returns>The enumerator</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return inner.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator
		/// </summary>
		/// <returns>The enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return inner.GetEnumerator();
		}
	}
}