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
	/// Represents a builder of LR graphs
	/// </summary>
	public class Builder
	{
		/// <summary>
		/// The grammar to build
		/// </summary>
		private Grammar grammar;
		/// <summary>
		/// The graph to build
		/// </summary>
		private Graph graph;
		/// <summary>
		/// The found conflicts
		/// </summary>
		private List<Conflict> conflicts;
		/// <summary>
		/// A GLR simulator
		/// </summary>
		private GLRSimulator simulator;

		/// <summary>
		/// Gets the conflicts produced by this builder
		/// </summary>
		public List<Conflict> Conflicts { get { return conflicts; } }

		/// <summary>
		/// Initializes this builder
		/// </summary>
		/// <param name="grammar">The grammar to build</param>
		public Builder(Grammar grammar)
		{
			this.grammar = grammar;
			this.conflicts = new List<Conflict>();
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
			simulator = new GLRSimulator(graph);
			conflicts = new List<Conflict>();
			foreach (State state in graph.States)
			{
				if (state.Conflicts.Count != 0)
				{
					List<Phrase> samples = simulator.GetInputsFor(state);
					foreach (Conflict conflict in state.Conflicts)
					{
						foreach (Phrase sample in samples)
						{
							Phrase temp = new Phrase(sample);
							temp.Append(conflict.ConflictSymbol);
							conflict.Examples.Add(temp);
						}
						conflicts.Add(conflict);
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
			Variable axiom = grammar.GetVariable(Grammar.generatedAxiom);
			ItemLR0 item = new ItemLR0(axiom.Rules[0], 0);
			StateKernel kernel = new StateKernel();
			kernel.AddItem(item);
			State state0 = kernel.GetClosure();
			Graph graph = new Graph(state0);
			// Construct the graph
			foreach (State state in graph.States)
				state.BuildReductions(new StateReductionsLR0());
			return graph;
		}

		/// <summary>
		/// Gets the LR(1) graph
		/// </summary>
		/// <returns>The corresponding LR(1) graph</returns>
		private Graph GetGraphLR1()
		{
			// Create the first set
			Variable axiom = grammar.GetVariable(Grammar.generatedAxiom);
			ItemLR1 item = new ItemLR1(axiom.Rules[0], 0, Epsilon.Instance);
			StateKernel kernel = new StateKernel();
			kernel.AddItem(item);
			State state0 = kernel.GetClosure();
			Graph graph = new Graph(state0);
			// Construct the graph
			foreach (State state in graph.States)
				state.BuildReductions(new StateReductionsLR1());
			return graph;
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