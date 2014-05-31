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
	/// Represents a base LR item
	/// </summary>
	public abstract class Item
	{
		/// <summary>
		/// The underlying grammar rule
		/// </summary>
		protected Rule rule;
		/// <summary>
		/// The dot position in the grammar rule
		/// </summary>
		protected int position;

		/// <summary>
		/// Gets the base rule for this item
		/// </summary>
		public Rule BaseRule { get { return rule; } }
		/// <summary>
		/// Gets the dot position in the grammar rule
		/// </summary>
		public int DotPosition { get { return position; } }
		/// <summary>
		/// Gets the action code for this item
		/// </summary>
		[System.CLSCompliant(false)]
		public LRActionCode Action
		{
			get
			{
				if (position != rule.Body.Choices[0].Length)
					return LRActionCode.Shift;
				return LRActionCode.Reduce;
			}
		}

		/// <summary>
		/// Gets the lookaheads for this item
		/// </summary>
		public abstract TerminalSet Lookaheads { get; }

		/// <summary>
		/// Initializes this item
		/// </summary>
		/// <param name="rule">The underlying rule</param>
		/// <param name="position">The dot position in the rule</param>
		public Item(Rule rule, int position)
		{
			this.rule = rule;
			this.position = position;
		}

		/// <summary>
		/// Gets the symbol following the dot in this item
		/// </summary>
		/// <returns>The symbol following the dot</returns>
		public Symbol GetNextSymbol()
		{
			return rule.Body.Choices[0][position].Symbol;
		}

		/// <summary>
		/// Gets rule choice following the dot in this item
		/// </summary>
		/// <returns>The choice following the dot</returns>
		public RuleChoice GetNextChoice()
		{
			return rule.Body.Choices[position + 1];
		}

		/// <summary>
		/// Gets the child of this item
		/// </summary>
		/// <returns>The child of this item</returns>
		public abstract Item GetChild();

		/// <summary>
		/// Closes this item to a set of items
		/// </summary>
		/// <param name="closure">The list to close</param>
		/// <param name="map">The current helper map</param>
		public abstract void CloseTo(List<Item> closure, Dictionary<Rule, Dictionary<int, List<Item>>> map);

		/// <summary>
		/// Base equality test for LR items
		/// </summary>
		/// <param name="item">The item to test</param>
		/// <returns>The equality result</returns>
		public bool BaseEquals(Item item)
		{
			if (this.rule != item.rule)
				return false;
			return (this.position == item.position);
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
		public abstract bool ItemEquals(Item item);

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.CentralDogma.Grammars.LR.Item"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return ItemEquals(obj as Item);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.CentralDogma.Grammars.LR.Item"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </returns>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </summary>
		/// <param name="withLookaheads">Whether to show the lookaheads</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.Item"/>.
		/// </returns>
		public string ToString(bool withLookaheads)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("\u3014");
			builder.Append(rule.Head.ToString());
			builder.Append(" \u2192");
			int i = 0;
			foreach (RuleBodyElement Part in rule.Body.Choices[0])
			{
				if (i == position)
					builder.Append(" \u25CF");
				builder.Append(" ");
				builder.Append(Part.ToString());
				i++;
			}
			if (i == position)
				builder.Append(" \u25CF");
			if (withLookaheads)
			{
				builder.Append(" \u25B6 ");
				TerminalSet lookaheads = Lookaheads;
				for (int j = 0; j != lookaheads.Count; j++)
				{
					if (j != 0)
						builder.Append(" ");
					builder.Append(lookaheads[j].ToString());
				}
			}
			builder.Append("\u3015");
			return builder.ToString();
		}
	}
}
