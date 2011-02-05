namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Implements the LR(1) parsing method
    /// </summary>
    public class MethodLR1 : CFGrammarMethod
    {
        /// <summary>
        /// Represents the actions for a LR(1) set
        /// </summary>
        internal class SetActions : ILRItemSetActions
        {
            /// <summary>
            /// Reduction actions
            /// </summary>
            private System.Collections.Generic.List<LRItemSetActionReduce> p_ActionReductions;
            /// <summary>
            /// List of the conflicts within the set
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
                System.Collections.Generic.Dictionary<Terminal, LRItemLR1> Reductions = new System.Collections.Generic.Dictionary<Terminal, LRItemLR1>();
                // Construct reductions
                foreach (LRItemLR1 Item in Set.Items)
                {
                    if (Item.Action == LRItemAction.Shift)
                        continue;
                    // There is already a shift action for the lookahead => conflict
                    if (Set.Children.ContainsKey(Item.Lookahead)) HandleConflict_ShiftReduce(p_Conflicts, Item, Set, Item.Lookahead);
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Item.Lookahead)) HandleConflict_ReduceReduce(p_Conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                    else // No conflict
                    {
                        Reductions.Add(Item.Lookahead, Item);
                        p_ActionReductions.Add(new LRItemSetActionReduce(Item.Lookahead, Item.BaseRule));
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
            /// Handle a LR(1) Shift/Reduce conflict
            /// </summary>
            /// <param name="Conflicts">List of the previous conflicts</param>
            /// <param name="ConflictuousItem">New conflictuous item</param>
            /// <param name="Set">Set containing the conflictuous items</param>
            /// <param name="OnSymbol">OnSymbol provoking the conflict</param>
            public static void HandleConflict_ShiftReduce(System.Collections.Generic.List<LRConflict> Conflicts, LRItem ConflictuousItem, LRItemSet Set, Terminal Lookahead)
            {
                // Look for previous conflict
                foreach (LRConflict Previous in Conflicts)
                {
                    if (Previous.ConflictType == LRConflictType.ShiftReduce && Previous.ConflictSymbol == Lookahead)
                    {
                        // Previous conflict
                        Previous.AddItem(ConflictuousItem);
                        return;
                    }
                }
                // No previous conflict was found
                LRConflict Conflict = new LRConflict(GrammarParseMethod.LR1, LRConflictType.ShiftReduce, Lookahead);
                foreach (LRItem Item in Set.Items)
                    if (Item.Action == LRItemAction.Shift && Item.NextSymbol.SID == Lookahead.SID)
                        Conflict.AddItem(Item);
                Conflict.AddItem(ConflictuousItem);
                Conflicts.Add(Conflict);
            }

            /// <summary>
            /// Handle a LR(1) Reduce/Reduce conflict
            /// </summary>
            /// <param name="Conflicts">List of the previous conflicts</param>
            /// <param name="ConflictuousItem">New conflictuous item</param>
            /// <param name="PreviousItem">Previous item with the reduction action</param>
            /// <param name="Set">Set containing the conflictuous items</param>
            /// <param name="OnSymbol">OnSymbol provoking the conflict</param>
            public static void HandleConflict_ReduceReduce(System.Collections.Generic.List<LRConflict> Conflicts, LRItem ConflictuousItem, LRItem PreviousItem, LRItemSet Set, Terminal Lookahead)
            {
                // Look for previous conflict
                foreach (LRConflict Previous in Conflicts)
                {
                    if (Previous.ConflictType == LRConflictType.ReduceReduce && Previous.ConflictSymbol == Lookahead)
                    {
                        // Previous conflict
                        Previous.AddItem(ConflictuousItem);
                        return;
                    }
                }
                // No previous conflict was found
                LRConflict Conflict = new LRConflict(GrammarParseMethod.LR1, LRConflictType.ReduceReduce, Lookahead);
                Conflict.AddItem(PreviousItem);
                Conflict.AddItem(ConflictuousItem);
                Conflicts.Add(Conflict);
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
        public MethodLR1() { }

        /// <summary>
        /// Construct the LR graph
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Return true if the construction succeeded, false otherwise</returns>
        public bool Construct(Grammar Grammar, log4net.ILog Log)
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
        public bool Construct(CFGrammar Grammar, log4net.ILog Log)
        {
            Log.Info("Constructing LR(1) data ...");
            p_Grammar = Grammar;
            p_Graph = ConstructGraph(Grammar, Log);
            // Output conflicts
            bool Error = false;
            foreach (LRItemSet Set in p_Graph.Sets)
            {
                foreach (LRConflict Conflict in Set.Conflicts)
                {
                    Log.Error("LR(1): " + Conflict.ToString());
                    Error = true;
                }
            }
            Log.Info("Done !");
            return (!Error);
        }

        /// <summary>
        /// Constructs a LR Graph for the given grammar
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Returns the constructed LR graph</returns>
        public static LRGraph ConstructGraph(CFGrammar Grammar, log4net.ILog Log)
        {
            // Create the first set
            CFVariable AxiomVar = Grammar.GetVariable("_Axiom_");
            LRItemLR1 AxiomItem = new LRItemLR1(AxiomVar.Rules[0], 0, TerminalEpsilon.Instance);
            LRItemSetKernel AxiomKernel = new LRItemSetKernel();
            AxiomKernel.AddItem(AxiomItem);
            LRItemSet AxiomSet = AxiomKernel.GetClosure();
            LRGraph Graph = new LRGraph();
            // Construct the graph
            Graph.Sets.Add(AxiomSet);
            for (int i = 0; i != Graph.Sets.Count; i++)
            {
                Graph.Sets[i].BuildGraph(Graph);
                Graph.Sets[i].ID = i;
                Log.Info("Set I" + i.ToString() + " on " + Graph.Sets.Count.ToString() + " completed");
            }
            foreach (LRItemSet Set in Graph.Sets)
                Set.BuildActions(new SetActions());
            return Graph;
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