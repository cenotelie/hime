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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents the inverse graph of an LR graph
	/// </summary>
	public class GraphInverse
	{
		/// <summary>
		/// Queue element for exploring paths in the LR graph
		/// </summary>
		private class ENode
		{
			/// <summary>
			/// The associated LR state
			/// </summary>
			public readonly State state;
			/// <summary>
			/// The next element
			/// </summary>
			public readonly ENode next;
			/// <summary>
			/// The transition to investigate
			/// </summary>
			public readonly Symbol transition;

			/// <summary>
			/// Initializes this element
			/// </summary>
			/// <param name="state">The associated LR state</param>
			/// <param name="next">The next queue element</param>
			/// <param name="transition">The transition</param>
			public ENode(State state, ENode next, Symbol transition)
			{
				this.state = state;
				this.next = next;
				this.transition = transition;
			}
		}

		/// <summary>
		/// The original graph
		/// </summary>
		private readonly Graph graph;
		/// <summary>
		/// The inverse graph
		/// </summary>
		private readonly Dictionary<int, Dictionary<Symbol, List<State>>> inverseGraph;

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
		/// <param name="transition">The symbol to look for on the transitions</param>
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
			inverseGraph = new Dictionary<int, Dictionary<Symbol, List<State>>>();
			BuildInverse();
		}

		/// <summary>
		/// Gets the states on all the paths to the specified target state
		/// </summary>
		/// <param name="target">The target state</param>
		/// <returns>The states on all the paths leading to the target state</returns>
		public List<List<State>> GetStatePathsTo(State target)
		{
			List<List<State>> result = new List<List<State>>();
			foreach (ENode start in GetPathsTo(target))
			{
				List<State> path = new List<State>();
				ENode node = start;
				while (node.next != null)
				{
					path.Add(node.state);
					node = node.next;
				}
				result.Add(path);
			}
			return result;
		}

		/// <summary>
		/// Gets possible inputs that allows for reaching the specified state from state 0
		/// </summary>
		/// <param name="state">A LR state</param>
		/// <returns>A list of possible inputs for reaching the specified state</returns>
		public List<Phrase> GetInputsFor(State state)
		{
			List<Phrase> result = new List<Phrase>();
			// for all paths from state 0 to the specified state
			foreach (ENode start in GetPathsTo(state))
			{
				Phrase input = new Phrase();
				ENode node = start;
				while (node.next != null)
				{
					Terminal terminal = node.transition as Terminal;
					if (terminal != null)
						// this is terminal => simply add it to the input
						input.Append(terminal);
					else
						// this is a variable => try to decompose it
						BuildInput(input, node.transition as Variable, new Stack<RuleChoice>());
					node = node.next;
				}
				result.Add(input);
			}
			return result;
		}

		/// <summary>
		/// Builds the input by decomposing the given variable
		/// </summary>
		/// <param name="sample">The input sample to build</param>
		/// <param name="var">The variable to decompose</param>
		/// <param name="stack">The stack of rule definitions currently in use for the decomposition</param>
		/// <remarks>
		/// This methods recursively triggers the production of encoutered variables to arrive to the terminal symbols.
		/// The methods also tries do not go into an infinite loop by keeping track of the rule definitions that are currently used.
		/// If a rule definition has already been used (is in the stack) the method avoids it
		/// </remarks>
		private void BuildInput(Phrase sample, Variable var, Stack<RuleChoice> stack)
		{
			// TODO: there is a bug in this method, it may call itself forever

			// if the variable to decompose is nullable (epsilon is in the FIRSTS state), stop here
			if (var.Firsts.Contains(Epsilon.Instance))
				return;

			// builds the list of possible rule definition for the variable
			List<RuleChoice> definitions = new List<RuleChoice>();
			foreach (Rule rule in var.Rules)
				definitions.Add(rule.Body.Choices[0]);

			// try to find a rule definition that is not already used (in the stack)
			RuleChoice def = null;
			foreach (RuleChoice d in definitions)
			{
				if (!stack.Contains(d))
				{
					// if it is not in the stack of rule definition
					// select it and stop
					def = d;
					break;
				}
			}

			// if all identified rule definitions are in the stack (currently in use)
			// fuck it, just use the first one ...
			if (def == null)
				def = definitions[0];

			RuleChoice[] stackElements = stack.ToArray();
			// push the rule definition to use onto the stack
			stack.Push(def);
			// walk the rule definition to build the sample
			for (int i = 0; i != def.Length; i++)
			{
				RuleBodyElement part = def[i];
				if (part.Symbol is Terminal)
				{
					// easy, just add it to the sample
					sample.Append(part.Symbol as Terminal);
				}
				else
				{
					bool symbolFound = false;
					// TODO: cleanup this code, this is really not a nice patch!!
					// TODO: this is surely ineficient, but I don't understand the algorithm to do better yet
					// The idea is to try and avoid infinite recursive call by checking the symbol was not already processed before
					// This code checks whether the variable in this part is not already in one of the rule definition currently in the stack
					foreach (RuleChoice definition in stackElements)
					{
						// for all elements
						for (int j = 0; j != definition.Length; j++)
						{
							RuleBodyElement definitionPart = definition[j];
							if (definitionPart.Symbol.Equals(part.Symbol))
							{
								symbolFound = true;
							}
						}
					}
					// if part.Symbol is not the same as another part.symbol found in a previous definition
					if (!symbolFound)
						BuildInput(sample, part.Symbol as Variable, stack);
					// TODO: what do we do if the variable was found?
				}
			}
			stack.Pop();
		}

		/// <summary>
		/// Builds the inverse data
		/// </summary>
		private void BuildInverse()
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

		/// <summary>
		/// Gets all the paths from state 0 to the specified one
		/// </summary>
		/// <param name="target">The target state</param>
		/// <returns>The paths</returns>
		private List<ENode> GetPathsTo(State target)
		{
			Dictionary<int, SortedList<int, ENode>> visited = new Dictionary<int, SortedList<int, ENode>>();
			LinkedList<ENode> queue = new LinkedList<ENode>();
			List<ENode> goals = new List<ENode>();
			queue.AddFirst(new ENode(target, null, null));
			while (queue.Count != 0)
			{
				ENode current = queue.First.Value;
				queue.RemoveFirst();
				if (!inverseGraph.ContainsKey(current.state.ID))
					continue;
				Dictionary<Symbol, List<State>> transitions = inverseGraph[current.state.ID];
				foreach (Symbol s in transitions.Keys)
				{
					foreach (State previous in transitions[s])
					{
						if (visited.ContainsKey(previous.ID))
						{
							if (visited[previous.ID].ContainsKey(s.ID))
								continue;
						}
						else
							visited.Add(previous.ID, new SortedList<int, ENode>());
						ENode pnode = new ENode(previous, current, s);
						visited[previous.ID].Add(s.ID, pnode);
						if (previous.ID == 0)
							goals.Add(pnode);
						else
							queue.AddLast(pnode);
					}
				}
			}
			return goals;
		}
	}
}
