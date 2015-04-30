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
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a LR conflict
	/// </summary>
	// should not inherit from Entry, rather should build an entry when needed!!
	public sealed class Conflict
	{
		/// <summary>
		/// The state raising this conflict
		/// </summary>
		private readonly State state;
		/// <summary>
		/// The conflict's type
		/// </summary>
		private readonly ConflictType type;
		/// <summary>
		/// The conflictuous lookahead
		/// </summary>
		private readonly Terminal lookahead;
		/// <summary>
		/// The set of conflictuous items
		/// </summary>
		private readonly List<Item> items;
		/// <summary>
		/// The example of conflictuous input
		/// </summary>
		private readonly List<Phrase> examples;

		/// <summary>
		/// Gets the state raising this conflict
		/// </summary>
		public State State { get { return state; } }
		/// <summary>
		/// Gets the type of the conflict
		/// </summary>
		public ConflictType ConflictType { get { return type; } }
		/// <summary>
		/// Gets the conflictuous terminal
		/// </summary>
		public Terminal ConflictSymbol { get { return lookahead; } }
		/// <summary>
		/// Gets the list of conflictuous items
		/// </summary>
		public ROList<Item> Items { get { return new ROList<Item>(items); } }
		/// <summary>
		/// Gets a list of examples of conflictuous examples
		/// </summary>
		public ROList<Phrase> Examples { get { return new ROList<Phrase>(examples); } }

		/// <summary>
		/// Initializes this conflict
		/// </summary>
		/// <param name="state">The state raising the state</param>
		/// <param name="type">The type of conflict</param>
		/// <param name="lookahead">The conflictuous lookahead</param>
		public Conflict(State state, ConflictType type, Terminal lookahead)
		{
			this.state = state;
			this.type = type;
			this.lookahead = lookahead;
			items = new List<Item>();
			examples = new List<Phrase>();
		}

		/// <summary>
		/// Initializes this conflict
		/// </summary>
		/// <param name="state">The state raising the state</param>
		/// <param name="type">The type of conflict</param>
		public Conflict(State state, ConflictType type)
		{
			this.state = state;
			this.type = type;
			lookahead = null;
			items = new List<Item>();
			examples = new List<Phrase>();
		}

		/// <summary>
		/// Adds a conflictuous item to this conflict
		/// </summary>
		/// <param name="item">The item to add</param>
		public void AddItem(Item item)
		{
			items.Add(item);
		}

		/// <summary>
		/// Adds an input example to this conflict
		/// </summary>
		/// <param name="example">The example to add</param>
		public void AddExample(Phrase example)
		{
			examples.Add(example);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Conflict"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.Conflict"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("Conflict ");
			if (type == ConflictType.ShiftReduce)
				builder.Append("Shift/Reduce");
			else
				builder.Append("Reduce/Reduce");
			builder.Append(" in ");
			builder.Append(state.ID);
			if (lookahead != null)
			{
				builder.Append(" on terminal '");
				builder.Append(lookahead.ToString());
				builder.Append("'");
			}
			builder.Append(" for items {");
			foreach (Item item in items)
			{
				builder.Append(" ");
				builder.Append(item.ToString());
			}
			builder.Append(" }");
			return builder.ToString();
		}
	}
}
