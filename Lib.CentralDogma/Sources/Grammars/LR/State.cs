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
	/// Represents a LR state
	/// </summary>
	public class State
	{
		/// <summary>
		/// The state's kernel
		/// </summary>
		private StateKernel kernel;
		/// <summary>
		/// The state's item
		/// </summary>
		private List<Item> items;
		/// <summary>
		/// The state's children
		/// </summary>
		private Dictionary<Symbol, State> children;
		/// <summary>
		/// The reductions in this state
		/// </summary>
		private StateReductions reductions;

		/// <summary>
		/// Gets or sets the state's identifier
		/// </summary>
		public int ID { get; set; }
		/// <summary>
		/// Gets the state's kernel
		/// </summary>
		public StateKernel Kernel { get { return kernel; } }
		/// <summary>
		/// Gets the state's reduction
		/// </summary>
		public StateReductions Reductions { get { return reductions; } }
		/// <summary>
		/// Gets the items in this state
		/// </summary>
		public ICollection<Item> Items { get { return items; } }
		/// <summary>
		/// Gets the state's children
		/// </summary>
		/// <value>
		/// The children.
		/// </value>
		public Dictionary<Symbol, State> Children { get { return children; } }
		/// <summary>
		/// Gets the conflicts in this state
		/// </summary>
		public ROList<Conflict> Conflicts { get { return reductions.Conflicts; } }

		/// <summary>
		/// Initializes this state
		/// </summary>
		/// <param name='Kernel'>The state's kernel</param>
		/// <param name='Items'>The state's items</param>
		public State(StateKernel kernel, List<Item> items)
		{
			this.kernel = kernel;
			this.items = items;
			this.children = new Dictionary<Symbol, State>(new Symbol.EqualityComparer());
		}

		/// <summary>
		/// Builds the given parent graph
		/// </summary>
		/// <param name="graph">The parent graph</param>
		public void BuildGraph(Graph graph)
		{
			// Shift dictionnary for the current set
			Dictionary<Symbol, StateKernel> shifts = new Dictionary<Symbol, StateKernel>();
			// Build the children kernels from the shift actions
			foreach (Item item in items)
			{
				// Ignore reduce actions
				if (item.Action == LRActionCode.Reduce)
					continue;

				Symbol next = item.GetNextSymbol();
				if (shifts.ContainsKey(next))
					shifts[next].AddItem(item.GetChild());
				else
				{
					StateKernel Kernel = new StateKernel();
					Kernel.AddItem(item.GetChild());
					shifts.Add(next, Kernel);
				}
			}
			// Close the children and add them to the graph
			foreach (Symbol next in shifts.Keys)
			{
				StateKernel kernel = shifts[next];
				State child = graph.ContainsState(kernel);
				if (child == null)
				{
					child = kernel.GetClosure();
					graph.Add(child);
				}
				children.Add(next, child);
			}
		}

		/// <summary>
		/// Builds the reductions on this state
		/// </summary>
		/// <param name="reductions">The reductions to build</param>
		public void BuildReductions(StateReductions reductions)
		{
			this.reductions = reductions;
			this.reductions.Build(this);
		}

		/// <summary>
		/// Determines whether the specified <see cref="Hime.CentralDogma.Grammars.LR.State"/> is equal to the
		/// current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </summary>
		/// <param name='Set'>
		/// The <see cref="Hime.CentralDogma.Grammars.LR.State"/> to compare with the current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Hime.CentralDogma.Grammars.LR.State"/> is equal to the current
		/// <see cref="Hime.CentralDogma.Grammars.LR.State"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(State Set)
		{
			return (kernel.Equals(Set.kernel));
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.CentralDogma.Grammars.LR.State"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as State);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.CentralDogma.Grammars.LR.State"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.LR.State"/>.
		/// </returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder("State ");
			builder.Append(ID);
			builder.Append(" = {");
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
