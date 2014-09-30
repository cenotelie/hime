/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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

namespace Hime.CentralDogma.Automata
{
	/// <summary>
	/// Represents a state in a Finite Automaton
	/// </summary>
	public class State
	{
		/// <summary>
		/// List of the items on this state
		/// </summary>
		private SortedList<int, FinalItem> items;

		/// <summary>
		/// Gets the items on this state
		/// </summary>
		public ROList<FinalItem> Items { get { return new ROList<FinalItem>(items.Values); } }

		/// <summary>
		/// Gets whether this state is final (i.e. it is marked with final items)
		/// </summary>
		public bool IsFinal { get { return (items.Count != 0); } }

		/// <summary>
		/// Initializes this state
		/// </summary>
		public State()
		{
			this.items = new SortedList<int, FinalItem>(new PriorityComparer());
		}

		/// <summary>
		/// Adds a new item making this state a final state
		/// </summary>
		/// <param name="item">The new item</param>
		public void AddItem(FinalItem item)
		{
			if (!items.ContainsValue(item))
			{
				items.Add(item.Priority, item);
			}
		}

		/// <summary>
		/// Adds new items making this state a final state
		/// </summary>
		/// <param name="items">The new markers</param>
		public void AddItems(IEnumerable<FinalItem> items)
		{
			foreach (FinalItem item in items)
				AddItem(item);
		}

		/// <summary>
		/// Clears all markers for this states making it non-final
		/// </summary>
		public void ClearItems()
		{
			items.Clear();
		}
	}
}
