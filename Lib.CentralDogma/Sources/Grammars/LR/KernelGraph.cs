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
	/// Represents a graph of LR kernels used to build a LALR(1) graph from a LR(0) graph
	/// </summary>
	public class KernelGraph
	{
		/// <summary>
		/// The LR(0) graph
		/// </summary>
		private Graph graphLR0;
		/// <summary>
		/// The result LALR(1) graph
		/// </summary>
		private Graph graphLALR1;
		/// <summary>
		/// The produced kernels
		/// </summary>
		private List<StateKernel> kernels;
		/// <summary>
		/// The origins of propagations
		/// </summary>
		private List<ItemLALR1> propagOrigins;
		/// <summary>
		/// The targets of propagations
		/// </summary>
		private List<ItemLALR1> propagTargets;

		/// <summary>
		/// Initializes this graph of kernels
		/// </summary>
		/// <param name="graphLR0">The original LR(0) graph</param>
		public KernelGraph(Graph graphLR0)
		{
			this.graphLR0 = graphLR0;
			this.kernels = new List<StateKernel>();
			this.propagOrigins = new List<ItemLALR1>();
			this.propagTargets = new List<ItemLALR1>();
		}

		/// <summary>
		/// Builds the kernels
		/// </summary>
		private void BuildKernels()
		{
			for (int i = 0; i != graphLR0.States.Count; i++)
			{
				State stateLR0 = graphLR0.States[i];
				StateKernel kernelLALR1 = new StateKernel();
				foreach (Item itemLR0 in stateLR0.Kernel.Items)
				{
					ItemLALR1 itemLALR1 = new ItemLALR1(itemLR0);
					if (i == 0)
						itemLALR1.Lookaheads.Add(Epsilon.Instance);
					kernelLALR1.AddItem(itemLALR1);
				}
				kernels.Add(kernelLALR1);
			}
		}

		/// <summary>
		/// Builds the propagation table
		/// </summary>
		/// <remarks>
		/// The propagation table is a couple of list where
		/// items in the first list propagate to items in the second list at the same index
		/// </remarks>
		private void BuildPropagationTable()
		{
			for (int i = 0; i != kernels.Count; i++)
			{
				StateKernel kernelLALR1 = kernels[i];
				State stateLR0 = graphLR0.States[i];
				// For each LALR(1) item in the kernel
				// Only the kernel needs to be examined as the other items will be discovered and treated
				// with the dummy closures
				foreach (ItemLALR1 itemLALR1 in kernelLALR1.Items)
				{
					// If ItemLALR1 is of the form [A -> alpha .]
					// => The closure will only contain the item itself
					// => Cannot be used to generate or propagate lookaheads
					if (itemLALR1.Action == LRActionCode.Reduce)
						continue;
					// Item here is of the form [A -> alpha . beta]
					// Create the corresponding dummy item : [A -> alpha . beta, dummy]
					// This item is used to detect lookahead propagation
					ItemLR1 dummyItem = new ItemLR1(itemLALR1.BaseRule, itemLALR1.DotPosition, Dummy.Instance);
					StateKernel dummyKernel = new StateKernel();
					dummyKernel.AddItem(dummyItem);
					State dummySet = dummyKernel.GetClosure();
					// For each item in the closure of the dummy item
					foreach (ItemLR1 item in dummySet.Items)
					{
						// If the item action is a reduction
						// => OnSymbol for this item will be created by the LALR(1) closure
						// => Do nothing
						if (item.Action == LRActionCode.Reduce)
							continue;
						// Get the child item in the child LALR(1) kernel
						State childLR0 = stateLR0.Children[item.GetNextSymbol()];
						StateKernel childKernel = kernels[childLR0.ID];
						ItemLALR1 childLALR1 = (ItemLALR1)GetEquivalentInSet(childKernel, item.GetChild());
						// If the lookaheads of the item in the dummy set contains the dummy terminal
						if (item.Lookahead == Dummy.Instance)
						{
							// => Propagation from the parent item to the child
							propagOrigins.Add(itemLALR1);
							propagTargets.Add(childLALR1);
						}
						else
						{
							// => Spontaneous generation of lookaheads
							childLALR1.Lookaheads.Add(item.Lookahead);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the item equivalent to the specified one in the kernel
		/// </summary>
		/// <param name="kernel">A kernel</param>
		/// <param name="equivalent">An item</param>
		/// <returns>The equivalent item</returns>
		private static Item GetEquivalentInSet(StateKernel kernel, Item equivalent)
		{
			foreach (Item item in kernel.Items)
				if (item.BaseEquals(equivalent))
					return item;
			return null;
		}

		/// <summary>
		/// Execute the propagations
		/// </summary>
		private void BuildPropagate()
		{
			// Propagation table is built
			// Do passes to propagate
			int modifications = 1;
			while (modifications != 0)
			{
				modifications = 0;
				for (int i = 0; i != propagOrigins.Count; i++)
				{
					modifications -= propagTargets[i].Lookaheads.Count;
					propagTargets[i].Lookaheads.AddRange(propagOrigins[i].Lookaheads);
					modifications += propagTargets[i].Lookaheads.Count;
				}
			}
		}

		/// <summary>
		/// Builds the LALR(1) graph
		/// </summary>
		private void BuildGraphLALR1()
		{
			// Build states
			graphLALR1 = new Graph();
			foreach (StateKernel kernelLALR1 in kernels)
				graphLALR1.Add(kernelLALR1.GetClosure());
			// Link and build actions for each LALR(1) set
			for (int i = 0; i != graphLALR1.States.Count; i++)
			{
				State stateLALR1 = graphLALR1.States[i];
				State stateLR0 = graphLR0.States[i];
				// Set ID
				stateLALR1.ID = i;
				// Link
				foreach (Symbol symbol in stateLR0.Children.Keys)
				{
					State childLALR1 = graphLALR1.States[stateLR0.Children[symbol].ID];
					stateLALR1.Children.Add(symbol, childLALR1);
				}
				// Build
				stateLALR1.BuildReductions(new StateReductionsLALR1());
			}
		}

		/// <summary>
		/// Gets the produced LALR(1) graph
		/// </summary>
		/// <returns>A LALR(1) graph</returns>
		public Graph GetGraphLALR1()
		{
			BuildKernels();
			BuildPropagationTable();
			BuildPropagate();
			BuildGraphLALR1();
			return graphLALR1;
		}
	}
}
