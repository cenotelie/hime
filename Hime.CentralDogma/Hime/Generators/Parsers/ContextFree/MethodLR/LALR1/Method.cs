namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Implements the LALR(1) parsing method
    /// </summary>
    public class MethodLALR1 : CFGrammarMethod
    {
        /// <summary>
        /// Represents the actions for a LALR(1) set
        /// </summary>
        internal class SetActions : ILRItemSetActions
        {
            /// <summary>
            /// Reduction actions
            /// </summary>
            private System.Collections.Generic.List<LRItemSetActionReduce> p_ActionReductions;
            /// <summary>
            /// List of the conflicts within the set (interface compliance)
            /// </summary>
            private System.Collections.Generic.List<LRConflict> p_Conflicts;

            /// <summary>
            /// Get an enumeration of the conflicts within the set
            /// </summary>
            /// <value>An enumeration of the conflicts within the set</value>
            public System.Collections.Generic.IEnumerable<LRConflict> Conflicts { get { return p_Conflicts; } }

            /// <summary>
            /// Constructs the actions
            /// </summary>
            public SetActions()
            {
                p_ActionReductions = new System.Collections.Generic.List<LRItemSetActionReduce>();
                p_Conflicts = new System.Collections.Generic.List<LRConflict>();
            }

            /// <summary>
            /// Build the actions for the given set of items
            /// </summary>
            /// <param name="Set">The set of items</param>
            public void Build(LRItemSet Set)
            {
                // Recutions dictionnary for the given set
                System.Collections.Generic.Dictionary<Terminal, LRItemLALR1> Reductions = new System.Collections.Generic.Dictionary<Terminal, LRItemLALR1>();
                // Construct reductions
                foreach (LRItemLALR1 Item in Set.Items)
                {
                    if (Item.Action == LRItemAction.Shift)
                        continue;
                    foreach (Terminal Lookahead in Item.Lookaheads)
                    {
                        // There is already a shift action for the lookahead => conflict
                        if (Set.Children.ContainsKey(Lookahead))
                            MethodLR1.SetActions.HandleConflict_ShiftReduce(p_Conflicts, Item, Set, Lookahead);
                        // There is already a reduction action for the lookahead => conflict
                        else if (Reductions.ContainsKey(Lookahead))
                            MethodLR1.SetActions.HandleConflict_ReduceReduce(p_Conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        else // No conflict
                        {
                            Reductions.Add(Lookahead, Item);
                            p_ActionReductions.Add(new LRItemSetActionReduce(Lookahead, Item.BaseRule));
                        }
                    }
                }
            }

            /// <summary>
            /// Get the terminals associated with some action
            /// </summary>
            /// <returns>Returns a set of all the expected terminal</returns>
            public TerminalSet GetExpectedTerminals()
            {
                TerminalSet Set = new TerminalSet();
                foreach (LRItemSetActionReduce Reduction in p_ActionReductions)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }

            /// <summary>
            /// Get the XML node representing the current set
            /// </summary>
            /// <param name="Set">The set of items</param>
            /// <param name="Doc">Parent XML document</param>
            /// <param name="Rules">List of the grammar rules</param>
            /// <returns>Returns the XML node</returns>
            public System.Xml.XmlNode GetXMLNode(LRItemSet Set, System.Xml.XmlDocument Doc, System.Collections.Generic.List<CFRule> Rules)
            {
                System.Xml.XmlNode Node = Doc.CreateElement("Actions");
                Node.AppendChild(Doc.CreateElement("OnTerminal"));
                Node.AppendChild(Doc.CreateElement("OnVariable"));
                // Export shift actions
                foreach (Symbol Symbol in Set.Children.Keys)
                {
                    if (Symbol is Terminal)
                        Node.ChildNodes[0].AppendChild(LRItemSetActionShift.BuildXMLNode(Doc, Symbol, Set.Children[Symbol]));
                    else
                        Node.ChildNodes[1].AppendChild(LRItemSetActionShift.BuildXMLNode(Doc, Symbol, Set.Children[Symbol]));
                }
                // Export reduction actions
                foreach (LRItemSetActionReduce Reduction in p_ActionReductions)
                    Node.ChildNodes[0].AppendChild(Reduction.GetXMLNode(Doc, Rules));
                return Node;
            }
        }

        /// <summary>
        /// Represents a graph of LALR(1) kernels based on a LR(0) graph
        /// </summary>
        internal class KernelGraph
        {
            /// <summary>
            /// The LR(0) graph
            /// </summary>
            private LRGraph p_GraphLR0;
            /// <summary>
            /// The LALR(1) graph
            /// </summary>
            private LRGraph p_GraphLALR1;
            /// <summary>
            /// Dictionnary associating LR(0) sets to LALR(1) kernels
            /// </summary>
            private System.Collections.Generic.Dictionary<LRItemSetKernel, LRItemSet> p_KernelsToLR0;
            /// <summary>
            /// Reverse-dictionnary associating LALR(1) kernels to LR(0) sets
            /// </summary>
            private System.Collections.Generic.Dictionary<LRItemSet, LRItemSetKernel> p_LR0ToKernels;
            /// <summary>
            /// Lookaheads propagation table : source LALR(1) items
            /// </summary>
            private System.Collections.Generic.List<LRItemLALR1> p_PropagOrigins;
            /// <summary>
            /// Lookaheads propagation table : destination LALR(1) items
            /// </summary>
            private System.Collections.Generic.List<LRItemLALR1> p_PropagTargets;

            /// <summary>
            /// Constructs the graph from the given LR(0) graph
            /// </summary>
            /// <param name="GraphLR0">LR(0) graph</param>
            public KernelGraph(LRGraph GraphLR0)
            {
                p_GraphLR0 = GraphLR0;
                p_KernelsToLR0 = new System.Collections.Generic.Dictionary<LRItemSetKernel, LRItemSet>();
                p_LR0ToKernels = new System.Collections.Generic.Dictionary<LRItemSet, LRItemSetKernel>();
                p_PropagOrigins = new System.Collections.Generic.List<LRItemLALR1>();
                p_PropagTargets = new System.Collections.Generic.List<LRItemLALR1>();
            }

            /// <summary>
            /// Create the LALR(1) kernels from the LR(0) sets
            /// </summary>
            private void BuildKernels()
            {
                for (int i = 0; i != p_GraphLR0.Sets.Count; i++)
                {
                    LRItemSet SetLR0 = p_GraphLR0.Sets[i];
                    LRItemSetKernel KernelLALR1 = new LRItemSetKernel();
                    foreach (LRItem Item in SetLR0.Kernel.Items)
                    {
                        LRItemLALR1 ItemLALR1 = new LRItemLALR1(Item);
                        if (i == 0)
                            ItemLALR1.Lookaheads.Add(TerminalEpsilon.Instance);
                        KernelLALR1.AddItem(ItemLALR1);
                    }
                    p_KernelsToLR0.Add(KernelLALR1, SetLR0);
                    p_LR0ToKernels.Add(SetLR0, KernelLALR1);
                }
            }

            /// <summary>
            /// Build the propagation table
            /// </summary>
            private void BuildPropagationTable()
            {
                foreach (LRItemSetKernel KernelLALR1 in p_KernelsToLR0.Keys)
                {
                    LRItemSet SetLR0 = p_KernelsToLR0[KernelLALR1];
                    // For each LALR(1) item in the kernel
                    // Only the kernel needs to be examined as the other items will be discovered and treated
                    // with the dummy closures
                    foreach (LRItemLALR1 ItemLALR1 in KernelLALR1.Items)
                    {
                        // If ItemLALR1 is of the form [A -> alpha .]
                        // => The closure will only contain the item itself
                        // => Cannot be used to generate or propagate lookaheads
                        if (ItemLALR1.Action == LRItemAction.Reduce)
                            continue;
                        // Item here is of the form [A -> alpha . beta]
                        // Create the corresponding dummy item : [A -> alpha . beta, dummy]
                        // This item is used to detect lookahead propagation
                        LRItemLALR1 DummyItem = new LRItemLALR1(ItemLALR1);
                        DummyItem.Lookaheads.Add(TerminalDummy.Instance);
                        LRItemSetKernel DummyKernel = new LRItemSetKernel();
                        DummyKernel.AddItem(DummyItem);
                        LRItemSet DummySet = DummyKernel.GetClosure();
                        // For each item in the closure of the dummy item
                        foreach (LRItemLALR1 Item in DummySet.Items)
                        {
                            // If the item action is a reduction
                            // => OnSymbol for this item will be created by the LALR(1) closure
                            // => Do nothing
                            if (Item.Action == LRItemAction.Reduce)
                                continue;
                            // Get the child item in the child LALR(1) kernel
                            // SetLR0.Children[Item.NextSymbol] is the child LR(0) set by a Item.NextSymbol transition
                            // p_LR0ToKernels[SetLR0.Children[Item.NextSymbol]] is then the associated LALR(1) kernel
                            LRItemLALR1 ChildLALR1 = (LRItemLALR1)GetEquivalentInSet(p_LR0ToKernels[SetLR0.Children[Item.NextSymbol]], Item.GetChild());
                            // If the lookaheads of the item in the dummy set contains the dummy terminal
                            if (Item.Lookaheads.Contains(TerminalDummy.Instance))
                            {
                                // => Propagation from the parent item to the child
                                p_PropagOrigins.Add(ItemLALR1);
                                p_PropagTargets.Add(ChildLALR1);
                                Item.Lookaheads.Remove(TerminalDummy.Instance);
                            }
                            if (Item.Lookaheads.Count != 0)
                            {
                                // => Spontaneous generation of lookaheads
                                ChildLALR1.Lookaheads.AddRange(Item.Lookaheads);
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// Get the child in the given kernel equals to the given copy
            /// </summary>
            /// <param name="Kernel">The kernel to search in</param>
            /// <param name="Equivalent">The searched value</param>
            /// <returns>Returns the item if found, null otherwise</returns>
            private static LRItem GetEquivalentInSet(LRItemSetKernel Kernel, LRItem Equivalent)
            {
                foreach (LRItem Potential in Kernel.Items)
                    if (Potential.Equals_Base(Equivalent))
                        return Potential;
                return null;
            }

            /// <summary>
            /// Propagate lookaheads in LALR(1) items using the propagation table
            /// </summary>
            private void BuildPropagate()
            {
                // Propagation table is built
                // Do passes to propagate
                int CountPass = 1;
                int CountModif = 1;
                while (CountModif != 0)
                {
                    CountModif = 0;
                    for (int i = 0; i != p_PropagOrigins.Count; i++)
                    {
                        CountModif -= p_PropagTargets[i].Lookaheads.Count;
                        p_PropagTargets[i].Lookaheads.AddRange(p_PropagOrigins[i].Lookaheads);
                        CountModif += p_PropagTargets[i].Lookaheads.Count;
                    }
                    CountPass++;
                }
            }

            /// <summary>
            /// Build the final LALR(1) sets and graph from the LALR(1) kernels
            /// </summary>
            private void BuildGraphLALR1()
            {
                // Build sets
                p_GraphLALR1 = new LRGraph();
                foreach (LRItemSetKernel KernelLALR1 in p_KernelsToLR0.Keys)
                    p_GraphLALR1.Add(KernelLALR1.GetClosure());
                // Link and build actions for each LALR(1) set
                for (int i = 0; i != p_GraphLALR1.Sets.Count; i++)
                {
                    LRItemSet SetLALR1 = p_GraphLALR1.Sets[i];
                    LRItemSet SetLR0 = p_GraphLR0.Sets[i];
                    // Set ID
                    SetLALR1.ID = i;
                    // Link
                    foreach (Symbol Symbol in SetLR0.Children.Keys)
                    {
                        LRItemSet ChildLALR1 = p_GraphLALR1.Sets[p_GraphLR0.Sets.IndexOf(SetLR0.Children[Symbol])];
                        SetLALR1.Children.Add(Symbol, ChildLALR1);
                    }
                    // Build
                    SetLALR1.BuildActions(new SetActions());
                }
            }

            /// <summary>
            /// Get the final LALR(1) graph
            /// </summary>
            /// <returns>Returns the LALR(1) graph</returns>
            public LRGraph GetGraphLALR1()
            {
                BuildKernels();
                BuildPropagationTable();
                BuildPropagate();
                BuildGraphLALR1();
                return p_GraphLALR1;
            }
        }


        /// <summary>
        /// The LR graph
        /// </summary>
        private LRGraph p_Graph;
        /// <summary>
        /// The context-free grammar
        /// </summary>
        private CFGrammar p_Grammar;

        /// <summary>
        /// Construct the method
        /// </summary>
        public MethodLALR1() { }

        /// <summary>
        /// Construct the LR graph
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Return true if the construction succeeded, false otherwise</returns>
        public bool Construct(Grammar Grammar, Kernel.Logs.Log Log)
        {
            if (Grammar is CFGrammar)
                return Construct((CFGrammar)Grammar, Log);
            return false;
        }
        /// <summary>
        /// Construct the LR graph
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Return true if the construction succeeded, false otherwise</returns>
        public bool Construct(CFGrammar Grammar, Kernel.Logs.Log Log)
        {
            Log.RawOutput("Constructing LALR(1) data ...");
            p_Grammar = Grammar;
            p_Graph = ConstructGraph(Grammar, Log);
            // Output conflicts
            bool Error = false;
            foreach (LRItemSet Set in p_Graph.Sets)
            {
                foreach (LRConflict Conflict in Set.Conflicts)
                {
                    Log.EntryBegin("Error");
                    Log.EntryAddData("LALR(1)");
                    Log.EntryAddData(Conflict.ToString());
                    Log.EntryEnd();
                    Error = true;
                }
            }
            Log.RawOutput("Done !");
            return (!Error);
        }

        /// <summary>
        /// Constructs a LR Graph for the given grammar
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Returns the constructed LR graph</returns>
        public static LRGraph ConstructGraph(CFGrammar Grammar, Kernel.Logs.Log Log)
        {
            LRGraph GraphLR0 = MethodLR0.ConstructGraph(Grammar, Log);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }

        /// <summary>
        /// Generate the parser data
        /// </summary>
        /// <param name="Doc">The parent document</param>
        /// <returns>The parser data</returns>
        public System.Xml.XmlNode GenerateData(System.Xml.XmlDocument Doc)
        {
            System.Xml.XmlNode Node = Doc.CreateElement("Parser");
            // Export rules
            System.Collections.Generic.List<CFRule> Rules = p_Grammar.Rules;
            Node.AppendChild(Doc.CreateElement("Rules"));
            foreach (CFRule Rule in Rules)
                Node.LastChild.AppendChild(Rule.GetXMLNode(Doc));
            // Export states
            Node.AppendChild(Doc.CreateElement("States"));
            foreach (LRItemSet Set in p_Graph.Sets)
                Node.LastChild.AppendChild(Set.GetXMLNode(Doc, Rules));
            return Node;
        }

        /// <summary>
        /// Generate a visual for the data
        /// </summary>
        /// <returns>Returns a bitmap</returns>
        public System.Drawing.Bitmap GenerateVisual() { return p_Graph.Draw(); }
    }
}