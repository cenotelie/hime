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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents the kernel of a LR state
	/// </summary>
	public sealed class StateKernel
	{
		/// <summary>
		/// The items in this kernel, organized in in a map
		/// </summary>
		private readonly Dictionary<Rule, Dictionary<int, List<Item>>> dictItems;
		/// <summary>
		/// The items in this kernel
		/// </summary>
		private readonly List<Item> items;

		/// <summary>
		/// Gets the kernel's size
		/// </summary>
		public int Size { get { return items.Count; } }
		/// <summary>
		/// Gets the items in this kernel
		/// </summary>
		public ICollection<Item> Items { get { return items; } }

		/// <summary>
		/// Initializes this kernel
		/// </summary>
		public StateKernel()
		{
			dictItems = new Dictionary<Rule, Dictionary<int, List<Item>>>();
			items = new List<Item>();
		}

		/// <summary>
		/// Adds the item to this kernel
		/// </summary>
		/// <param name="item">A LR item</param>
		public void AddItem(Item item)
		{
			if (!dictItems.ContainsKey(item.BaseRule))
				dictItems.Add(item.BaseRule, new Dictionary<int, List<Item>>());
			Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
			if (!sub.ContainsKey(item.DotPosition))
				sub.Add(item.DotPosition, new List<Item>());
			sub[item.DotPosition].Add(item);
			items.Add(item);
		}

		/// <summary>
		/// Determines whether the given item is in this kernel
		/// </summary>
		/// <param name="item">A LR item</param>
		/// <returns><c>true</c> if the item is in this kernel</returns>
		public bool ContainsItem(Item item)
		{
			if (!dictItems.ContainsKey(item.BaseRule))
				return false;
			Dictionary<int, List<Item>> sub = dictItems[item.BaseRule];
			return (sub.ContainsKey(item.DotPosition) && sub[item.DotPosition].Contains(item));
		}

		/// <summary>
		/// Gets the closure of this kernel
		/// </summary>
		/// <returns>The closure</returns>
		public State GetClosure()
		{
			// The set's items
			Dictionary<Rule, Dictionary<int, List<Item>>> map = new Dictionary<Rule, Dictionary<int, List<Item>>>();
			foreach (Rule rule in dictItems.Keys)
			{
				Dictionary<int, List<Item>> clone = new Dictionary<int, List<Item>>();
				Dictionary<int, List<Item>> original = dictItems[rule];
				map.Add(rule, clone);
				foreach (int position in original.Keys)
				{
					List<Item> list = new List<Item>(original[position]);
					clone.Add(position, list);
				}
			}
			List<Item> closure = new List<Item>(items);
			// Close the set
			for (int i = 0; i != closure.Count; i++)
				closure[i].CloseTo(closure, map);
			return new State(this, closure);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Hime.SDK.Grammars.LR.StateKernel"/> is equal to
		/// the current <see cref="Hime.SDK.Grammars.LR.StateKernel"/>.
		/// </summary>
		/// <param name='kernel'>
		/// The <see cref="Hime.SDK.Grammars.LR.StateKernel"/> to compare with the current <see cref="Hime.SDK.Grammars.LR.StateKernel"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Hime.SDK.Grammars.LR.StateKernel"/> is equal to the
		/// current <see cref="Hime.SDK.Grammars.LR.StateKernel"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(StateKernel kernel)
		{
			if (items.Count != kernel.items.Count)
				return false;
			if (dictItems.Count != kernel.dictItems.Count)
				return false;
			foreach (Rule rule in dictItems.Keys)
			{
				if (!kernel.dictItems.ContainsKey(rule))
					return false;
				Dictionary<int, List<Item>> left = dictItems[rule];
				Dictionary<int, List<Item>> right = kernel.dictItems[rule];
				if (left.Count != right.Count)
					return false;
				foreach (int position in left.Keys)
				{
					if (!right.ContainsKey(position))
						return false;
					List<Item> l1 = left[position];
					List<Item> l2 = right[position];
					if (l1.Count != l2.Count)
						return false;
					foreach (Item item in l1)
						if (!l2.Contains(item))
							return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.Grammars.LR.StateKernel"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.Grammars.LR.StateKernel"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.LR.StateKernel"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as StateKernel);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.Grammars.LR.StateKernel"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}