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
using Hime.Redist.Parsers;
using Hime.Redist.Utils;

namespace Hime.SDK.Grammars.LR
{
	/// <summary>
	/// Represents a builder of LR graphs
	/// </summary>
	public class Builder
	{
		/// <summary>
		/// The grammar to build
		/// </summary>
		private readonly Grammar grammar;
		/// <summary>
		/// The found conflicts
		/// </summary>
		private readonly List<Conflict> conflicts;
		/// <summary>
		/// The other errors
		/// </summary>
		private readonly List<Error> errors;
		/// <summary>
		/// The graph to build
		/// </summary>
		private Graph graph;
		/// <summary>
		/// A GLR simulator
		/// </summary>
		private GraphInverse inverse;

		/// <summary>
		/// Gets all the conflicts produced by this builder
		/// </summary>
		public ROList<Conflict> Conflicts { get { return new ROList<Conflict>(conflicts); } }

		/// <summary>
		/// Gets all the errors produced by this builder (other than the conflicts)
		/// </summary>
		public ROList<Error> Errors { get { return new ROList<Error>(errors); } }

		/// <summary>
		/// Initializes this builder
		/// </summary>
		/// <param name="grammar">The grammar to build</param>
		public Builder(Grammar grammar)
		{
			this.grammar = grammar;
			conflicts = new List<Conflict>();
			errors = new List<Error>();
		}

		/// <summary>
		/// Build the specified grammar
		/// </summary>
		/// <param name="method">The parsing method to use</param>
		/// <returns>The resulting LR graph, or <c>null</c> if it could not be generated</returns>
		public Graph Build(ParsingMethod method)
		{
			// build the graph
			switch (method)
			{
			case ParsingMethod.LR0:
				graph = GetGraphLR0();
				break;
			case ParsingMethod.LR1:
				graph = GetGraphLR1();
				break;
			case ParsingMethod.LALR1:
				graph = GetGaphLALR1();
				break;
			case ParsingMethod.RNGLR1:
				graph = GetGraphRNGLR1();
				break;
			case ParsingMethod.RNGLALR1:
				graph = GetGraphRNGLALR1();
				break;
			}

			// builds the set of conflicts
			inverse = new GraphInverse(graph);
			foreach (State state in graph.States)
			{
				if (state.Conflicts.Count != 0)
				{
					List<Phrase> samples = inverse.GetInputsFor(state);
					foreach (Conflict conflict in state.Conflicts)
					{
						foreach (Phrase sample in samples)
						{
							Phrase temp = new Phrase(sample);
							temp.Append(conflict.ConflictSymbol);
							conflict.AddExample(temp);
						}
						conflicts.Add(conflict);
					}
				}
				List<List<State>> paths = null;
				foreach (Symbol symbol in state.Transitions)
				{
					Terminal terminal = symbol as Terminal;
					if (terminal == null)
						continue;
					if (terminal.Context == 0)
						continue;
					// this is a contextual terminal, can we reach this state without the right context being available
					if (paths == null)
						paths = inverse.GetStatePathsTo(state);
					foreach (List<State> path in paths)
					{
						path.Add(state); // append this state
						bool found = false;
						for (int i = 0; i != path.Count - 1; i++)
						{
							State element = path[i];
							foreach (Item item in element.Items)
							{
								if (item.DotPosition == 0 && item.BaseRule.Context == terminal.Context)
								{
									// this is the opening of a context only if we are not going to the next state using the associated variable
									found |= !state.HasTransition(item.BaseRule.Head) || state.GetChildBy(item.BaseRule.Head) != path[i + 1];
									break;
								}
							}
							if (found)
								break;
						}
						foreach (Item item in state.Items)
						{
							if (item.DotPosition == 0 && item.BaseRule.Context == terminal.Context)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							// this is problematic path
							ContextualError error = new ContextualError(state);
							foreach (Item item in state.Items)
							{
								if (item.Action == LRActionCode.Shift && item.GetNextSymbol() == terminal)
									error.AddItem(item);
							}
							errors.Add(error);
						}
					}
				}
			}
			return graph;
		}

		/// <summary>
		/// Gets the LR(0) graph
		/// </summary>
		/// <returns>The corresponding LR(0) graph</returns>
		private Graph GetGraphLR0()
		{
			// Create the base LR(0) graph
			Variable axiom = grammar.GetVariable(Grammar.GENERATED_AXIOM);
			ItemLR0 item = new ItemLR0(axiom.Rules[0], 0);
			StateKernel kernel = new StateKernel();
			kernel.AddItem(item);
			State state0 = kernel.GetClosure();
			Graph result = new Graph(state0);
			// Construct the graph
			foreach (State state in result.States)
				state.BuildReductions(new StateReductionsLR0());
			return result;
		}

		/// <summary>
		/// Gets the LR(1) graph
		/// </summary>
		/// <returns>The corresponding LR(1) graph</returns>
		private Graph GetGraphLR1()
		{
			// Create the first set
			Variable axiom = grammar.GetVariable(Grammar.GENERATED_AXIOM);
			ItemLR1 item = new ItemLR1(axiom.Rules[0], 0, Epsilon.Instance);
			StateKernel kernel = new StateKernel();
			kernel.AddItem(item);
			State state0 = kernel.GetClosure();
			Graph result = new Graph(state0);
			// Construct the graph
			foreach (State state in result.States)
				state.BuildReductions(new StateReductionsLR1());
			return result;
		}

		/// <summary>
		/// Gets the LALR(1) graph
		/// </summary>
		/// <returns>The corresponding LALR(1) graph</returns>
		private Graph GetGaphLALR1()
		{
			Graph graphLR0 = GetGraphLR0();
			KernelGraph kernels = new KernelGraph(graphLR0);
			return kernels.GetGraphLALR1();
		}

		/// <summary>
		/// Gets the RNGLR(1) graph
		/// </summary>
		/// <returns>The corresponding RNGLR(1) graph</returns>
		private Graph GetGraphRNGLR1()
		{
			Graph graphLR1 = GetGraphLR1();
			foreach (State state in graphLR1.States)
				state.BuildReductions(new StateReductionsRNGLR1());
			return graphLR1;
		}

		/// <summary>
		/// Gets the RNGLALR(1) graph
		/// </summary>
		/// <returns>The corresponding RNGLALR(1) graph</returns>
		private Graph GetGraphRNGLALR1()
		{
			Graph graphLALR1 = GetGaphLALR1();
			foreach (State state in graphLALR1.States)
				state.BuildReductions(new StateReductionsRNGLALR1());
			return graphLALR1;
		}
	}
}