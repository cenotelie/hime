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

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a simulator of GLR parsers
	/// </summary>
	public class GLRSimulator : GraphInverse
	{
		/// <summary>
		/// Initializes this simulator from the specified LR graph
		/// </summary>
		/// <param name='graph'>The LR graph to simulate</param>
		public GLRSimulator(Graph graph) : base(graph)
		{
		}

		/// <summary>
		/// Simulates the parser from the given generation on the specified lookahead
		/// </summary>
		/// <param name="from">The generation to start from</param>
		/// <param name="lookahead">The lookahead to simulate on</param>
		/// <returns>The generation produced by the simulation</returns>
		public GLRGeneration Simulate(GLRGeneration from, Terminal lookahead)
		{
			// Works on a copy of the given generation so as to not modify it
			// Nodes before reductions
			GLRGeneration current = new GLRGeneration(from);

			// Apply reductions
			// use a for loop because the collection will be modified
			for (int i = 0; i != current.Nodes.Count; i++)
			{
				GLRStackNode node = current.Nodes[i];
				// for all reductions on this state
				foreach (StateActionReduce reduction in node.State.Reductions)
				{
					if (reduction.Lookahead.ID != lookahead.ID)
						continue;
					// this is a reduction triggered by the given lookahead
					// gets the origin generation
					GLRGeneration origin = GetOrigin(node, reduction.ToReduceRule.Body.Choices[0]);
					// for each stack node in the origin
					foreach (GLRStackNode nOrigin in origin.Nodes)
					{
						if (nOrigin.State.HasTransition(reduction.ToReduceRule.Head))
						{
							// there is a transition from the LR state represented by this origin stack node by the reduced variable
							// get the target state
							State next = nOrigin.State.GetChildBy(reduction.ToReduceRule.Head);
							// resolve this state in the current generation
							GLRStackNode follower = current.Resolve(next);
							// adds an edge from the new node to the original one, labelled with the rule's head
							follower.AddEdge(reduction.ToReduceRule.Head, nOrigin);
						}
					}
				}
			}

			// All reductions have been applied, now create the next generation with the shift actions
			GLRGeneration result = new GLRGeneration();
			foreach (GLRStackNode node in current)
			{
				if (node.State.HasTransition(lookahead))
				{
					// there is a transition in the LR graph from the state
					// represented by the current stack node by the lookahead
					State next = node.State.GetChildBy(lookahead);
					// resolve the next state in the target generation
					GLRStackNode follower = result.Resolve(next);
					// adds the corresponding edge to the current node in the current generation
					follower.AddEdge(lookahead, node);
				}
			}
			return result;
		}

		/// <summary>
		/// Simulates the parser from the given LR state and item on the given lookahead
		/// </summary>
		/// <param name="state">A LR state</param>
		/// <param name="item">A LR item in the state</param>
		/// <param name="lookahead">The lookahead to simulate on</param>
		/// <returns>The generation produced by the simulation</returns>
		public GLRGeneration Simulate(State state, Item item, Terminal lookahead)
		{
			// creates the current generation
			GLRGeneration current = new GLRGeneration();
			// resolve the staring state in the current generation
			GLRStackNode start = current.Resolve(state);
			// creates the target generation
			GLRGeneration result = new GLRGeneration();

			if (item.Action == LRActionCode.Shift)
			{
				// the item to simulate is a shift action, simply populate the target generation
				// resolve the target state in the target generation
				GLRStackNode next = result.Resolve(state.GetChildBy(item.GetNextSymbol()));
				// adds the corresponding edge
				next.AddEdge(item.GetNextSymbol(), start);
				// stop here
				return result;
			}

			// here the item is a reduction
			// gets the origin generation
			GLRGeneration origin = GetOrigin(current, item.BaseRule.Body.Choices[0]);
			// for each stack node in the origin
			foreach (GLRStackNode node in origin.Nodes)
			{
				if (node.State.HasTransition(item.BaseRule.Head))
				{
					// there is a transition from the LR state represented by this origin stack node by the reduced variable
					// get the target state
					State target = node.State.GetChildBy(item.BaseRule.Head);
					// resolve this state in the target generation
					GLRStackNode next = result.Resolve(target);
					// adds an edge from the new node to the original one, labelled with the rule's head
					next.AddEdge(item.BaseRule.Head, node);
				}
			}
			// finalized by simulating from the target generation
			return Simulate(result, lookahead);
		}

		/// <summary>
		/// Gets a generation likely to represent an ancestor of the one containing the given stack node
		/// </summary>
		/// <param name="node">A stack node</param>
		/// <param name="definition">A rule definition</param>
		/// <returns>A generation likely to represent an ancestor of the one containing the given stack node</returns>
		private GLRGeneration GetOrigin(GLRStackNode node, RuleChoice definition)
		{
			// builds a dummy generation with the given stack node in it
			GLRGeneration current = new GLRGeneration();
			current.Resolve(node.State);
			// delegates
			return GetOrigin(current, definition);
		}

		/// <summary>
		/// Gets a generation likely to represent an ancestor of the given one following the given rule definition
		/// </summary>
		/// <param name="current">A generation</param>
		/// <param name="definition">A rule definition</param>
		/// <returns>A generation likely to represent an ancestor of the given one following the specified rule definition</returns>
		private GLRGeneration GetOrigin(GLRGeneration current, RuleChoice definition)
		{
			// iteratively build the ancestors from symbols in the rule definition
			// we have to use the reverse order because we 'go back' to the anscestors
			GLRGeneration result = current;
			int index = definition.Length - 1;
			while (index != -1)
			{
				// gets the symbol for this hop
				Symbol symbol = definition[index].Symbol;
				// gets the ancestor for the given symbol
				result = GetOrigin(result, symbol);
				index--;
			}
			// return the result
			return result;
		}

		/// <summary>
		/// Gets the generation likely to represent an ancestor of the given one by one symbol
		/// </summary>
		/// <param name="current">A generation</param>
		/// <param name="symbol">A symbol</param>
		/// <returns>A generation likely to represent an ancestor of the given one by one symbol</returns>
		private GLRGeneration GetOrigin(GLRGeneration current, Symbol symbol)
		{
			// create the ancestor
			GLRGeneration result = new GLRGeneration();
			foreach (GLRStackNode node in current.Nodes)
			{
				GLRStackNode previous = node.GetPreviousBy(symbol);
				if (previous != null)
				{
					// we have an edge to a previous stack node!
					// resolve the represented state in the ancestor
					GLRStackNode target = result.Resolve(previous.State);
					// add an edge
					node.AddEdge(symbol, target);
					// stop here
					continue;
				}
				// here we do not have an edge
				// lookup the inverse graph to find the possible ancestor LR states
				if (!inverseGraph.ContainsKey(node.ID))
					// no data in the inverse graph about the current state, stop here
					continue;
				Dictionary<Symbol, List<State>> inverses = inverseGraph[node.State.ID];
				if (!inverses.ContainsKey(symbol))
					continue;
				// for each ancestor LR state in the LR graph
				foreach (State ancestor in inverses[symbol])
				{
					// resolve in the target generation
					GLRStackNode target = result.Resolve(ancestor);
					// add an edge
					node.AddEdge(symbol, target);
				}
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
			foreach (ICollection<Symbol> path in GetPaths(state))
			{
				Phrase input = new Phrase();
				foreach (Symbol s in path)
				{
					if (s is Terminal)
						// this is terminal => simply add it to the input
						input.Append(s as Terminal);
					else
						// this is a variable => try to decompose it
						BuildInput(input, s as Variable, new Stack<RuleChoice>());
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
			for (int i=0; i!=def.Length; i++)
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
						for (int j=0; j!=definition.Length; j++)
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
		/// Queue element for exploring paths in the LR graph
		/// </summary>
		private class ENode
		{
			/// <summary>
			/// The associated LR state
			/// </summary>
			public State state;
			/// <summary>
			/// The next element
			/// </summary>
			public ENode next;
			/// <summary>
			/// The transition to investigate
			/// </summary>
			public Symbol transition;

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

		private List<ICollection<Symbol>> GetPaths(State state)
		{
			Dictionary<int, SortedList<int, ENode>> visited = new Dictionary<int, SortedList<int, ENode>>();
			LinkedList<ENode> queue = new LinkedList<ENode>();
			List<ENode> goals = new List<ENode>();
			queue.AddFirst(new ENode(state, null, null));

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

			List<ICollection<Symbol>> paths = new List<ICollection<Symbol>>();
			foreach (ENode start in goals)
			{
				ENode node = start;
				LinkedList<Symbol> path = new LinkedList<Symbol>();
				while (node.next != null)
				{
					path.AddLast(node.transition);
					node = node.next;
				}
				paths.Add(path);
			}
			return paths;
		}
	}
}
