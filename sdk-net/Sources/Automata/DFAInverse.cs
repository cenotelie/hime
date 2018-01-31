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

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents the inverse graph of a DFA
	/// </summary>
	public class DFAInverse
	{
		/// <summary>
		/// The reachable states
		/// </summary>
		private readonly List<DFAState> reachable;
		/// <summary>
		/// The inverse graph data
		/// </summary>
		private readonly Dictionary<DFAState, List<DFAState>> inverses;
		/// <summary>
		/// The final states
		/// </summary>
		private readonly List<DFAState> finals;

		/// <summary>
		/// Gets the reachable states of the DFA
		/// </summary>
		public ROList<DFAState> Reachable { get { return new ROList<DFAState>(reachable); } }

		/// <summary>
		/// Gets the inverse transitions
		/// </summary>
		public Dictionary<DFAState, List<DFAState>> Inverses { get { return inverses; } }

		/// <summary>
		/// Gets the final states of the original DFA
		/// </summary>
		public ROList<DFAState> Finals { get { return new ROList<DFAState>(finals); } }

		/// <summary>
		/// Builds this inverse graph from the specified DFA
		/// </summary>
		/// <param name="dfa">A DFA</param>
		public DFAInverse(DFA dfa)
		{
			reachable = new List<DFAState>();
			inverses = new Dictionary<DFAState, List<DFAState>>();
			finals = new List<DFAState>();

			// transitive closure of the first state
			reachable.Add(dfa.States[0]);
			inverses.Add(dfa.States[0], new List<DFAState>());
			for (int i = 0; i != reachable.Count; i++)
			{
				DFAState current = reachable[i];
				foreach (DFAState next in current.Children)
				{
					if (!reachable.Contains(next))
					{
						reachable.Add(next);
						inverses.Add(next, new List<DFAState>());
					}
					inverses[next].Add(current);
				}
				if (current.Items.Count != 0)
					finals.Add(current);
			}
		}

		/// <summary>
		/// Closes the specified list of states through inverse transitions
		/// </summary>
		/// <param name="states">The list of states to close</param>
		/// <returns>The closure</returns>
		public List<DFAState> CloseByAntecedents(IEnumerable<DFAState> states)
		{
			List<DFAState> result = new List<DFAState>(states);
			// transitive closure of the final states by their antecedents
			// final states are all reachable
			for (int i = 0; i != result.Count; i++)
			{
				DFAState state = result[i];
				if (!inverses.ContainsKey(state))
					continue;
				foreach (DFAState antecedent in inverses[state])
				{
					if (!result.Contains(antecedent) && reachable.Contains(antecedent))
					{
						// this antecedent is reachable and not yet in the closure
						// => add it to the closure
						result.Add(antecedent);
					}
				}
			}
			return result;
		}
	}
}