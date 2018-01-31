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

using System.Collections.Generic;
using System.Text;
using Hime.Redist.Parsers;
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a LR state
	/// </summary>
	public class State
	{
		/// <summary>
		/// The state's kernel
		/// </summary>
		private readonly StateKernel kernel;
		/// <summary>
		/// The state's item
		/// </summary>
		private readonly List<Item> items;
		/// <summary>
		/// The state's children
		/// </summary>
		private readonly Dictionary<Symbol, State> children;
		/// <summary>
		/// The reductions in this state
		/// </summary>
		private StateReductions reductions;
		/// <summary>
		/// The contexts opening by transitions from this state
		/// </summary>
		private readonly Dictionary<Terminal, List<int>> openingContexts;

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
		public ROList<Item> Items { get { return new ROList<Item>(items); } }

		/// <summary>
		/// Gets the conflicts in this state
		/// </summary>
		public ROList<Conflict> Conflicts { get { return reductions.Conflicts; } }

		/// <summary>
		/// Gets the symbols that triggers transitions from this state
		/// </summary>
		public ICollection<Symbol> Transitions { get { return children.Keys; } }

		/// <summary>
		/// Initializes this state
		/// </summary>
		/// <param name="kernel">The state's kernel</param>
		/// <param name="items">The state's items</param>
		public State(StateKernel kernel, List<Item> items)
		{
			this.kernel = kernel;
			this.items = items;
			children = new Dictionary<Symbol, State>(new Symbol.EqualityComparer());
			openingContexts = new Dictionary<Terminal, List<int>>();
		}

		/// <summary>
		/// Determines whether this state has a transition triggered by the specified symbol
		/// </summary>
		/// <param name="symbol">A transition symbol</param>
		/// <returns><c>true</c> if this state has a transition triggered by the specified symbol; otherwise, <c>false</c></returns>
		public bool HasTransition(Symbol symbol)
		{
			return children.ContainsKey(symbol);
		}

		/// <summary>
		/// Gets the child of this state by the specified transition
		/// </summary>
		/// <param name="symbol">A transition symbol</param>
		/// <returns>The child by the specified transition</returns>
		public State GetChildBy(Symbol symbol)
		{
			return children[symbol];
		}

		/// <summary>
		/// Gets the contexts opened by a transition on the specified terminal
		/// </summary>
		/// <param name="terminal">A terminal</param>
		/// <returns>The opened contexts</returns>
		public ROList<int> GetContextsOpenedBy(Terminal terminal)
		{
			return !openingContexts.ContainsKey(terminal) ? new ROList<int>(new List<int>(0)) : new ROList<int>(openingContexts[terminal]);
		}

		/// <summary>
		/// Adds a transition to a child
		/// </summary>
		/// <param name="symbol">The transition symbol</param>
		/// <param name="child">The child state</param>
		public void AddChild(Symbol symbol, State child)
		{
			children.Add(symbol, child);
		}

		/// <summary>
		/// Copies the context information of the specified state (replaces any existing one)
		/// </summary>
		/// <param name="state">A state</param>
		public void CopyContextsOf(State state)
		{
			openingContexts.Clear();
			foreach (Terminal terminal in state.openingContexts.Keys)
				openingContexts.Add(terminal, new List<int>(state.openingContexts[terminal]));
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
					StateKernel nextKernel = new StateKernel();
					nextKernel.AddItem(item.GetChild());
					shifts.Add(next, nextKernel);
				}
			}
			// Close the children and add them to the graph
			foreach (Symbol next in shifts.Keys)
			{
				StateKernel nextKernel = shifts[next];
				State child = graph.ContainsState(nextKernel);
				if (child == null)
				{
					child = nextKernel.GetClosure();
					graph.Add(child);
				}
				children.Add(next, child);
			}
			// Build the context data
			foreach (Item item in items)
			{
				if (item.BaseRule.Context != 0 && item.DotPosition == 0 && item.Action == LRActionCode.Shift)
				{
					// this is the opening of a context
					List<Terminal> openingTerminals = new List<Terminal>();
					Symbol first = item.GetNextSymbol();
					if (first is Terminal)
						openingTerminals.Add(first as Terminal);
					else
						openingTerminals.AddRange((first as Variable).Firsts);
					foreach (Terminal terminal in openingTerminals)
					{
						List<int> contexts;
						if (openingContexts.ContainsKey(terminal))
							contexts = openingContexts[terminal];
						else
						{
							contexts = new List<int>();
							openingContexts.Add(terminal, contexts);
						}
						if (!contexts.Contains(item.BaseRule.Context))
							contexts.Add(item.BaseRule.Context);
					}
				}
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
		/// Determines whether the specified <see cref="Hime.SDK.Grammars.LR.State"/> is equal to the
		/// current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </summary>
		/// <param name='state'>
		/// The <see cref="Hime.SDK.Grammars.LR.State"/> to compare with the current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="Hime.SDK.Grammars.LR.State"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.LR.State"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(State state)
		{
			return (kernel.Equals(state.kernel));
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </summary>
		/// <param name='obj'>
		/// The <see cref="System.Object"/> to compare with the current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Hime.SDK.Grammars.LR.State"/>; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as State);
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Hime.SDK.Grammars.LR.State"/> object.
		/// </summary>
		/// <returns>
		/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.SDK.Grammars.LR.State"/>.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder("State ");
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
