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
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a LR graph
	/// </summary>
	public class Graph
	{
		/// <summary>
		/// The states in this graph
		/// </summary>
		private readonly List<State> states;

		/// <summary>
		/// Gets the states in this graph
		/// </summary>
		public ROList<State> States { get { return new ROList<State>(states); } }

		/// <summary>
		/// Initializes a new empty graph
		/// </summary>
		public Graph()
		{
			states = new List<State>();
		}

		/// <summary>
		/// Initializes a graph from the given state
		/// </summary>
		/// <param name='state'>The LR state to build from</param>
		public Graph(State state) : this()
		{
			states = new List<State>();
			Add(state);
			for (int i = 0; i != states.Count; i++)
				states[i].BuildGraph(this);
		}

		/// <summary>
		/// Determines whether the given state (as a kernel) is already in this graph
		/// </summary>
		/// <param name="kernel">A kernel</param>
		/// <returns>The corresponding state, or null if none is found</returns>
		public State ContainsState(StateKernel kernel)
		{
			foreach (State potential in states)
				if (potential.Kernel.Equals(kernel))
					return potential;
			return null;
		}

		/// <summary>
		/// Adds a state to this graph
		/// </summary>
		/// <param name="state">The state to add</param>
		public void Add(State state)
		{
			state.ID = states.Count;
			states.Add(state);
		}
	}
}
