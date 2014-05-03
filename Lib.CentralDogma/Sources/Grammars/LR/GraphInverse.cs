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

namespace Hime.CentralDogma.Grammars.LR
{
	/// <summary>
	/// Represents the inverse graph of an LR graph
	/// </summary>
	public class GraphInverse
	{
		/// <summary>
		/// The original graph
		/// </summary>
		protected Graph graph;
		/// <summary>
		/// The inverse graph
		/// </summary>
		protected Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

		/// <summary>
		/// Determines whether the given state has incoming transitions
		/// </summary>
		/// <param name="target">The target state to investigate</param>
		/// <returns><c>true</c> if this target has an incoming transition; otherwise, <c>false</c></returns>
		public bool HasIncomings(int target)
		{
			return inverseGraph.ContainsKey(target);
		}

		/// <summary>
		/// Gets the incoming transition labels to the given state
		/// </summary>
		/// <param name="target">The target state to investigate</param>
		/// <returns>The label in the incoming transitions</returns>
		public ICollection<Symbol> GetIncomings(int target)
		{
			return inverseGraph[target].Keys;
		}

		/// <summary>
		/// Gets the origins of the incoming transitions to the given state
		/// </summary>
		/// <param name="target">The target state to investigate</param>
		/// <returns>The origins of the incoming transitions</returns>
		public ICollection<State> GetOrigins(int target, Symbol transition)
		{
			return inverseGraph[target][transition];
		}

		/// <summary>
		/// Initializes the inverse graph from a given LR graph
		/// </summary>
		/// <param name="graph">The to inverse</param>
		public GraphInverse(Graph graph)
		{
			this.graph = graph;
			this.inverseGraph = new Dictionary<int, Dictionary<Symbol, List<State>>>();
			BuildInverse();
		}

		/// <summary>
		/// Builds the inverse data
		/// </summary>
		protected void BuildInverse()
		{
			foreach (State state in graph.States)
			{
				foreach (Symbol symbol in state.Transitions)
				{
					State child = state.GetChildBy(symbol);
					if (!inverseGraph.ContainsKey(child.ID))
						inverseGraph.Add(child.ID, new Dictionary<Symbol, List<State>>());
					Dictionary<Symbol, List<State>> inverses = inverseGraph[child.ID];
					if (!inverses.ContainsKey(symbol))
						inverses.Add(symbol, new List<State>());
					List<State> parents = inverses[symbol];
					parents.Add(state);
				}
			}
		}
	}
}
