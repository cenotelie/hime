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
using Hime.Redist.Parsers;

namespace Hime.CentralDogma.Grammars.LR
{
	/// <summary>
	/// Represents a LALR(1) item
	/// </summary>
	public class ItemLALR1 : Item
	{
		/// <summary>
		/// The lookaheads for this item
		/// </summary>
		private TerminalSet lookaheads;

		/// <summary>
		///  Gets the lookaheads for this item
		/// </summary>
		public override TerminalSet Lookaheads { get { return lookaheads; } }

		/// <summary>
		/// Initializes this item
		/// </summary>
		/// <param name="rule">The underlying rule</param>
		/// <param name="position">The dot position in the rule</param>
		/// <param name="lookaheads">The lookaheads for this item</param>
		public ItemLALR1(Rule rule, int position, TerminalSet lookaheads) : base(rule, position)
		{
			this.lookaheads = new TerminalSet(lookaheads);
		}

		/// <summary>
		/// Initializes this item
		/// </summary>
		/// <param name="copied">The item to copy</param>
		public ItemLALR1(Item copied) : base(copied.BaseRule, copied.DotPosition)
		{
			this.lookaheads = new TerminalSet();
		}

		/// <summary>
		///  Gets the child of this item
		/// </summary>
		/// <returns>The child of this item</returns>
		public override Item GetChild()
		{
			if (Action == LRActionCode.Reduce)
				return null;
			return new ItemLALR1(rule, position + 1, new TerminalSet(lookaheads));
		}

		/// <summary>
		/// Closes this item to a set of items
		/// </summary>
		/// <param name="closure">The list to close</param>
		/// <param name="map">The current helper map</param>
		public override void CloseTo(List<Item> closure, Dictionary<Rule, Dictionary<int, List<Item>>> map)
		{
			// the item was of the form [Var -> alpha .] (reduction)
			// nothing to do
			if (Action == LRActionCode.Reduce)
				return;
			// Get the next symbol in the item
			Symbol next = GetNextSymbol();
			// Here the item is of the form [Var -> alpha . next beta]
			// If the next symbol is not a variable : do nothing
			// If the next symbol is a variable :
			Variable nextVar = next as Variable;
			if (nextVar == null)
				return;
			// Firsts is a copy of the Firsts set for beta (next choice)
			// Firsts will contains symbols that may follow Next
			// Firsts will therefore be the lookahead for child items
			TerminalSet firsts = new TerminalSet(GetNextChoice().Firsts);
			// If beta is nullifiable (contains ε) :
			if (firsts.Contains(Epsilon.Instance))
			{
				// Remove ε
				firsts.Remove(Epsilon.Instance);
				// Add the item's lookaheads
				firsts.AddRange(lookaheads);
			}
			// For each rule that has Next as a head variable :
			foreach (Rule rule in nextVar.Rules)
			{
				if (!map.ContainsKey(rule))
					map.Add(rule, new Dictionary<int, List<Item>>());
				Dictionary<int, List<Item>> sub = map[rule];
				if (sub.ContainsKey(0))
				{
					List<Item> previouses = sub[0];
					ItemLALR1 previous = previouses[0] as ItemLALR1;
					previous.Lookaheads.AddRange(firsts);
				}
				else
				{
					List<Item> items = new List<Item>();
					sub.Add(0, items);
					ItemLALR1 New = new ItemLALR1(rule, 0, firsts);
					closure.Add(New);
					items.Add(New);
				}
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="Hime.CentralDogma.Grammars.LR.Item"/> is equal to the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <param name='item'>
		/// The <see cref="Hime.CentralDogma.Grammars.LR.Item"/> to compare with the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Hime.CentralDogma.Grammars.LR.Item"/> is equal to the current
		/// <see cref="Hime.CentralDogma.Grammars.LR.Item"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool ItemEquals(Item item)
		{
			ItemLALR1 tested = (ItemLALR1)item;
			if (!BaseEquals(tested))
				return false;
			if (this.lookaheads.Count != tested.lookaheads.Count)
				return false;
			foreach (Terminal terminal in this.lookaheads)
			{
				if (!tested.lookaheads.Contains(terminal))
					return false;
			}
			return true;
		}
	}
}
