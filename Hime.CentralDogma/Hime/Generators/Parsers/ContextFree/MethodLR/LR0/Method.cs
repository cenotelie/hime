namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Implements the LR(0) parsing method
    /// </summary>
    public class MethodLR0 : CFGrammarMethod
    {
        /// <summary>
        /// Represents the actions for a LR(0) set
        /// </summary>
        internal class SetActions : ILRItemSetActions
        {
            /// <summary>
            /// Reduction action (possibly empty)
            /// </summary>
            private LRItemSetActionReduce p_ActionReduce;
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
            public SetActions() { p_Conflicts = new System.Collections.Generic.List<LRConflict>(); }

            /// <summary>
            /// Build the actions for the given set of items
            /// </summary>
            /// <param name="Set">The set of items</param>
            public void Build(LRItemSet Set)
            {
                LRItem Reduce = null;
                // Look for Reduce actions
                foreach (LRItem Item in Set.Items)
                {
                    // Ignore Shift actions
                    if (Item.Action == LRItemAction.Shift)
                        continue;
                    if (Set.Children.Count != 0)
                    {
                        // Conflict Shift/Reduce
                        LRConflict Conflict = new LRConflict(GrammarParseMethod.LR0, LRConflictType.ShiftReduce);
                        Conflict.AddItem(Item);
                        p_Conflicts.Add(Conflict);
                    }
                    if (Reduce != null)
                    {
                        // Conflict Reduce/Reduce
                        LRConflict Conflict = new LRConflict(GrammarParseMethod.LR0, LRConflictType.ReduceReduce);
                        Conflict.AddItem(Item);
                        Conflict.AddItem(Reduce);
                        p_Conflicts.Add(Conflict);
                    }
                    else
                    {
                        p_ActionReduce = new LRItemSetActionReduce(null, Item.BaseRule);
                        Reduce = Item;
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
                if (p_ActionReduce != null)
                    Set.Add(p_ActionReduce.Lookahead);
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
                Node.Attributes.Append(Doc.CreateAttribute("Reduce"));

                if (p_ActionReduce != null)
                {
                    Node.Attributes["Reduce"].Value = "True";
                    Node.AppendChild(p_ActionReduce.GetXMLNode(Doc, Rules));
                }
                else
                {
                    Node.Attributes["Reduce"].Value = "False";
                    Node.AppendChild(Doc.CreateElement("OnTerminal"));
                    Node.AppendChild(Doc.CreateElement("OnVariable"));
                    foreach (Symbol Symbol in Set.Children.Keys)
                    {
                        if (Symbol is Terminal)
                            Node.ChildNodes[0].AppendChild(LRItemSetActionShift.BuildXMLNode(Doc, Symbol, Set.Children[Symbol]));
                        else
                            Node.ChildNodes[1].AppendChild(LRItemSetActionShift.BuildXMLNode(Doc, Symbol, Set.Children[Symbol]));
                    }
                }
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
        public MethodLR0() { }

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
            Log.Info("Constructing LR(0) data ...");
            p_Grammar = Grammar;
            p_Graph = ConstructGraph(Grammar, Log);
            // Output conflicts
            bool Error = false;
            foreach (LRItemSet Set in p_Graph.Sets)
            {
                foreach (LRConflict Conflict in Set.Conflicts)
                {
                    Log.Error("LR(0): " + Conflict.ToString());
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
            LRItemLR0 AxiomItem = new LRItemLR0(AxiomVar.Rules[0], 0);
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