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
using System.Collections.Generic;

namespace Hime.SDK.Grammars
{
	/// <summary>
	/// Represents a set of unique terminals (sorted by ID)
	/// </summary>
	public class TerminalSet : IList<Terminal>
	{
		/// <summary>
		/// The backing content
		/// </summary>
		private readonly SortedList<int, Terminal> content;

		/// <summary>
		/// Initializes this set as empty
		/// </summary>
		public TerminalSet()
		{
			content = new SortedList<int, Terminal>();
		}

		/// <summary>
		/// Initializes this set as a copy of the given set
		/// </summary>
		/// <param name="copied">The set to copy</param>
		public TerminalSet(TerminalSet copied)
		{
			content = new SortedList<int, Terminal>(copied.content);
		}

		#region Implementation of IList<Terminal>

		/// <summary>
		/// Adds the specified item
		/// </summary>
		/// <param name="item">The item to add to the current collection</param>
		/// <returns><c>true</c> if the item was added, <c>false</c> otherwise because an item with the same ID was already present</returns>
		public bool Add(Terminal item)
		{
			if (content.ContainsKey(item.ID))
				return false;
			content.Add(item.ID, item);
			return true;
		}

		/// <summary>
		/// Adds the specified items
		/// </summary>
		/// <param name="collection">The items to add to the current collection</param>
		/// <returns><c>true</c> if at least one item was added</returns>
		public bool AddRange(IEnumerable<Terminal> collection)
		{
			bool mod = false;
			foreach (Terminal item in collection)
			{
				if (!content.ContainsKey(item.ID))
				{
					mod = true;
					content.Add(item.ID, item);
				}
			}
			return mod;
		}

		/// <summary>
		/// Determines the index of a specific item in the current instance
		/// </summary>
		/// <param name="item">An item</param>
		/// <returns>The index of the item</returns>
		public int IndexOf(Terminal item)
		{
			return content.IndexOfKey(item.ID);
		}

		/// <summary>
		/// Insert the specified item at the specified index
		/// </summary>
		/// <param name="index">The index to insert at</param>
		/// <param name="item">The item to insert</param>
		/// <remarks>This method is not supported</remarks>
		public void Insert(int index, Terminal item)
		{
			throw new System.NotSupportedException();
		}

		/// <summary>
		/// Removes the item at the specified index
		/// </summary>
		/// <param name="index">An index in this collection</param>
		public void RemoveAt(int index)
		{
			content.RemoveAt(index);
		}

		/// <summary>
		/// Gets the item at the specified index
		/// </summary>
		/// <param name="index">An index in this collection</param>
		/// <remarks>The set operation is not supported</remarks>
		public Terminal this[int index]
		{
			get { return content.Values[index]; }
			set { throw new System.NotSupportedException(); }
		}

		/// <summary>
		/// Adds the specified item
		/// </summary>
		/// <param name="item">The item to add to the current collection</param>
		void ICollection<Terminal>.Add(Terminal item)
		{
			if (content.ContainsKey(item.ID))
				return;
			content.Add(item.ID, item);
		}

		/// <summary>
		/// Removes all items from this collection
		/// </summary>
		public void Clear()
		{
			content.Clear();
		}

		/// <summary>
		/// Determines whether the current collection contains a specific value
		/// </summary>
		/// <param name="item">The object to locate in the current collection</param>
		/// <returns>Whether the collection contains the specified item</returns>
		public bool Contains(Terminal item)
		{
			return content.ContainsKey(item.ID);
		}

		/// <summary>
		/// Copies the content of this collection to an array
		/// </summary>
		/// <param name="array">The array to copy to</param>
		/// <param name="arrayIndex">The starting index in the provided array</param>
		/// <remarks>This method is not supported</remarks>
		public void CopyTo(Terminal[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Gets the number of items in this collection
		/// </summary>
		public int Count { get { return content.Count; } }

		/// <summary>
		/// Gets a value indicating whether this instance is read only
		/// </summary>
		public bool IsReadOnly { get { return false; } }

		/// <summary>
		/// Removes the first occurrence of an item from the current collection
		/// </summary>
		/// <param name="item">The item to remove</param>
		/// <returns>Whether the item was present and was removed from this collection</returns>
		public bool Remove(Terminal item)
		{
			return content.Remove(item.ID);
		}

		/// <summary>
		/// Gets the enumerator
		/// </summary>
		/// <returns>The enumerator</returns>
		public IEnumerator<Terminal> GetEnumerator()
		{
			return content.Values.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator
		/// </summary>
		/// <returns>The enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return content.Values.GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.TerminalSet"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.TerminalSet"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("{");
			for (int i = 0; i != Count; i++)
			{
				if (i != 0)
					builder.Append(", ");
				builder.Append(this[i].Name);
			}
			builder.Append("}");
			return builder.ToString();
		}
	}
}
