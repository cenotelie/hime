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
	/// Represents a LR(0) item
	/// </summary>
	public class ItemLR0 : Item
	{
		/// <summary>
		/// Initializes this item
		/// </summary>
		/// <param name="rule">The underlying rule</param>
		/// <param name="position">The dot position in the rule</param>
		public ItemLR0(Rule rule, int position) : base(rule, position)
		{
		}

		/// <summary>
		///  Gets the lookaheads for this item
		/// </summary>
		/// <remarks>This is always the empty set</remarks>
		public override TerminalSet Lookaheads { get { return new TerminalSet(); } }

		/// <summary>
		///  Gets the child of this item
		/// </summary>
		/// <returns>The child of this item</returns>
		public override Item GetChild()
		{
			if (Action == LRActionCode.Reduce)
				return null;
			return new ItemLR0(rule, position + 1);
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
			foreach (Rule rule in nextVar.Rules)
			{
				if (!map.ContainsKey(rule))
					map.Add(rule, new Dictionary<int, List<Item>>());
				Dictionary<int, List<Item>> sub = map[rule];
				if (sub.ContainsKey(0))
					continue;
				List<Item> list = new List<Item>();
				sub.Add(0, list);

				ItemLR0 child = new ItemLR0(rule, 0);
				closure.Add(child);
				list.Add(child);
			}
		}

		/// <summary>
		/// Determines whether the specified <see cref="Hime.CentralDogma.Grammars.LR.Item"/> is equal to the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="Hime.CentralDogma.Grammars.LR.Item"/> to compare with the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Hime.CentralDogma.Grammars.LR.Item"/> is equal to the current
		/// <see cref="Hime.CentralDogma.Grammars.LR.Item"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool ItemEquals(Item item)
		{
			return BaseEquals(item);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("[");
			builder.Append(rule.Head.ToString());
			builder.Append(" ->");
			int i = 0;
			foreach (RuleBodyElement part in rule.Body.Choices[0])
			{
				if (i == position)
					builder.Append(" " + dot);
				builder.Append(" ");
				builder.Append(part.ToString());
				i++;
			}
			if (i == position)
				builder.Append(" " + dot);
			builder.Append("]");
			return builder.ToString();
		}
	}
}
