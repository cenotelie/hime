using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class KernelGraph
    {
        private Graph graphLR0;
        private Graph graphLALR1;
        private List<StateKernel> kernels;
        private List<ItemLALR1> propagOrigins;
        private List<ItemLALR1> propagTargets;

        public KernelGraph(Graph graphLR0)
        {
            this.graphLR0 = graphLR0;
            this.kernels = new List<StateKernel>();
            this.propagOrigins = new List<ItemLALR1>();
            this.propagTargets = new List<ItemLALR1>();
        }

        private void BuildKernels()
        {
            for (int i = 0; i != graphLR0.States.Count; i++)
            {
                State setLR0 = graphLR0.States[i];
                StateKernel kernelLALR1 = new StateKernel();
                foreach (Item itemLR0 in setLR0.Kernel.Items)
                {
                    ItemLALR1 itemLALR1 = new ItemLALR1(itemLR0);
                    if (i == 0)
                        itemLALR1.Lookaheads.Add(Epsilon.Instance);
                    kernelLALR1.AddItem(itemLALR1);
                }
                kernels.Add(kernelLALR1);
            }
        }

        private void BuildPropagationTable()
        {
            for (int i = 0; i != kernels.Count; i++)
            {
                StateKernel kernelLALR1 = kernels[i];
                State setLR0 = graphLR0.States[i];
                // For each LALR(1) item in the kernel
                // Only the kernel needs to be examined as the other items will be discovered and treated
                // with the dummy closures
                foreach (ItemLALR1 itemLALR1 in kernelLALR1.Items)
                {
                    // If ItemLALR1 is of the form [A -> alpha .]
                    // => The closure will only contain the item itself
                    // => Cannot be used to generate or propagate lookaheads
                    if (itemLALR1.Action == ItemAction.Reduce)
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
                        if (item.Action == ItemAction.Reduce)
                            continue;
                        // Get the child item in the child LALR(1) kernel
                        State childLR0 = setLR0.Children[item.NextSymbol];
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

        private static Item GetEquivalentInSet(StateKernel Kernel, Item Equivalent)
        {
            foreach (Item Potential in Kernel.Items)
                if (Potential.Equals_Base(Equivalent))
                    return Potential;
            return null;
        }

        private void BuildPropagate()
        {
            // Propagation table is built
            // Do passes to propagate
            int CountPass = 1;
            int CountModif = 1;
            while (CountModif != 0)
            {
                CountModif = 0;
                for (int i = 0; i != propagOrigins.Count; i++)
                {
                    CountModif -= propagTargets[i].Lookaheads.Count;
                    propagTargets[i].Lookaheads.AddRange(propagOrigins[i].Lookaheads);
                    CountModif += propagTargets[i].Lookaheads.Count;
                }
                CountPass++;
            }
        }

        private void BuildGraphLALR1()
        {
            // Build sets
            graphLALR1 = new Graph();
            foreach (StateKernel kernelLALR1 in kernels)
                graphLALR1.Add(kernelLALR1.GetClosure());
            // Link and build actions for each LALR(1) set
            for (int i = 0; i != graphLALR1.States.Count; i++)
            {
                State setLALR1 = graphLALR1.States[i];
                State setLR0 = graphLR0.States[i];
                // Set ID
                setLALR1.ID = i;
                // Link
                foreach (Symbol symbol in setLR0.Children.Keys)
                {
                    State childLALR1 = graphLALR1.States[setLR0.Children[symbol].ID];
                    setLALR1.Children.Add(symbol, childLALR1);
                }
                // Build
                setLALR1.BuildReductions(new StateReductionsLALR1());
            }
        }

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
