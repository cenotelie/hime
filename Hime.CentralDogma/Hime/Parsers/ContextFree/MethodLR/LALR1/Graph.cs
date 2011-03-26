using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class KernelGraph
    {
        private Graph graphLR0;
        private Graph graphLALR1;
        private Dictionary<ItemSetKernel, ItemSet> kernelsToLR0;
        private Dictionary<ItemSet, ItemSetKernel> lR0ToKernels;
        private List<ItemLALR1> propagOrigins;
        private List<ItemLALR1> propagTargets;

        public KernelGraph(Graph GraphLR0)
        {
            graphLR0 = GraphLR0;
            kernelsToLR0 = new Dictionary<ItemSetKernel, ItemSet>();
            lR0ToKernels = new Dictionary<ItemSet, ItemSetKernel>();
            propagOrigins = new List<ItemLALR1>();
            propagTargets = new List<ItemLALR1>();
        }

        private void BuildKernels()
        {
            for (int i = 0; i != graphLR0.Sets.Count; i++)
            {
                ItemSet SetLR0 = graphLR0.Sets[i];
                ItemSetKernel KernelLALR1 = new ItemSetKernel();
                foreach (Item Item in SetLR0.Kernel.Items)
                {
                    ItemLALR1 ItemLALR1 = new ItemLALR1(Item);
                    if (i == 0)
                        ItemLALR1.Lookaheads.Add(TerminalEpsilon.Instance);
                    KernelLALR1.AddItem(ItemLALR1);
                }
                kernelsToLR0.Add(KernelLALR1, SetLR0);
                lR0ToKernels.Add(SetLR0, KernelLALR1);
            }
        }

        private void BuildPropagationTable()
        {
            foreach (ItemSetKernel KernelLALR1 in kernelsToLR0.Keys)
            {
                ItemSet SetLR0 = kernelsToLR0[KernelLALR1];
                // For each LALR(1) item in the kernel
                // Only the kernel needs to be examined as the other items will be discovered and treated
                // with the dummy closures
                foreach (ItemLALR1 ItemLALR1 in KernelLALR1.Items)
                {
                    // If ItemLALR1 is of the form [A -> alpha .]
                    // => The closure will only contain the item itself
                    // => Cannot be used to generate or propagate lookaheads
                    if (ItemLALR1.Action == ItemAction.Reduce)
                        continue;
                    // Item here is of the form [A -> alpha . beta]
                    // Create the corresponding dummy item : [A -> alpha . beta, dummy]
                    // This item is used to detect lookahead propagation
                    ItemLR1 DummyItem = new ItemLR1(ItemLALR1.BaseRule, ItemLALR1.DotPosition, TerminalDummy.Instance);
                    ItemSetKernel DummyKernel = new ItemSetKernel();
                    DummyKernel.AddItem(DummyItem);
                    ItemSet DummySet = DummyKernel.GetClosure();
                    // For each item in the closure of the dummy item
                    foreach (ItemLR1 Item in DummySet.Items)
                    {
                        // If the item action is a reduction
                        // => OnSymbol for this item will be created by the LALR(1) closure
                        // => Do nothing
                        if (Item.Action == ItemAction.Reduce)
                            continue;
                        // Get the child item in the child LALR(1) kernel
                        // SetLR0.Children[Item.NextSymbol] is the child LR(0) set by a Item.NextSymbol transition
                        // lR0ToKernels[SetLR0.Children[Item.NextSymbol]] is then the associated LALR(1) kernel
                        ItemLALR1 ChildLALR1 = (ItemLALR1)GetEquivalentInSet(lR0ToKernels[SetLR0.Children[Item.NextSymbol]], Item.GetChild());
                        // If the lookaheads of the item in the dummy set contains the dummy terminal
                        if (Item.Lookahead == TerminalDummy.Instance)
                        {
                            // => Propagation from the parent item to the child
                            propagOrigins.Add(ItemLALR1);
                            propagTargets.Add(ChildLALR1);
                        }
                        else
                        {
                            // => Spontaneous generation of lookaheads
                            ChildLALR1.Lookaheads.Add(Item.Lookahead);
                        }
                    }
                }
            }
        }

        private static Item GetEquivalentInSet(ItemSetKernel Kernel, Item Equivalent)
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
            foreach (ItemSetKernel KernelLALR1 in kernelsToLR0.Keys)
                graphLALR1.Add(KernelLALR1.GetClosure());
            // Link and build actions for each LALR(1) set
            for (int i = 0; i != graphLALR1.Sets.Count; i++)
            {
                ItemSet SetLALR1 = graphLALR1.Sets[i];
                ItemSet SetLR0 = graphLR0.Sets[i];
                // Set ID
                SetLALR1.ID = i;
                // Link
                foreach (Symbol Symbol in SetLR0.Children.Keys)
                {
                    ItemSet ChildLALR1 = graphLALR1.Sets[graphLR0.Sets.IndexOf(SetLR0.Children[Symbol])];
                    SetLALR1.Children.Add(Symbol, ChildLALR1);
                }
                // Build
                SetLALR1.BuildReductions(new ItemSetReductionsLALR1());
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
