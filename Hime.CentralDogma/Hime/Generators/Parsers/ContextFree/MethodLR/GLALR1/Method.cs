namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Implements the GLALR(1) parsing method
    /// </summary>
    public class MethodGLALR1 : CFGrammarMethod
    {
        /// <summary>
        /// Represents the actions for a GLALR(1) set
        /// </summary>
        internal class SetActions : ILRItemSetActions
        {
            /// <summary>
            /// Dictionnary associating a list of possible set actions to a symbol
            /// </summary>
            private System.Collections.Generic.Dictionary<Symbol, System.Collections.Generic.List<ILRItemSetAction>> p_Actions;
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
                p_Actions = new System.Collections.Generic.Dictionary<Symbol, System.Collections.Generic.List<ILRItemSetAction>>();
                p_ActionReductions = new System.Collections.Generic.List<LRItemSetActionReduce>();
                p_Conflicts = new System.Collections.Generic.List<LRConflict>();
            }

            /// <summary>
            /// Build the actions for the given set of items
            /// </summary>
            /// <param name="Set">The set of items</param>
            public void Build(LRItemSet Set)
            {
                // Build shift actions
                foreach (Symbol Next in Set.Children.Keys)
                {
                    System.Collections.Generic.List<ILRItemSetAction> Actions = new System.Collections.Generic.List<ILRItemSetAction>();
                    Actions.Add(new LRItemSetActionShift(Next, Set.Children[Next]));
                }

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
                        {
                            MethodLR1.SetActions.HandleConflict_ShiftReduce(p_Conflicts, Item, Set, Lookahead);
                            if (!p_Actions.ContainsKey(Lookahead))
                                p_Actions.Add(Lookahead, new System.Collections.Generic.List<ILRItemSetAction>());
                            p_Actions[Lookahead].Add(new LRItemSetActionReduce(Lookahead, Item.BaseRule));
                        }
                        // There is already a reduction action for the lookahead => conflict
                        else if (Reductions.ContainsKey(Lookahead))
                        {
                            MethodLR1.SetActions.HandleConflict_ReduceReduce(p_Conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                            if (!p_Actions.ContainsKey(Lookahead))
                                p_Actions.Add(Lookahead, new System.Collections.Generic.List<ILRItemSetAction>());
                            p_Actions[Lookahead].Add(new LRItemSetActionReduce(Lookahead, Item.BaseRule));
                        }
                        // No conflict
                        else
                        {
                            Reductions.Add(Lookahead, Item);
                            LRItemSetActionReduce Reduction = new LRItemSetActionReduce(Lookahead, Item.BaseRule);
                            p_ActionReductions.Add(Reduction);
                            p_Actions[Lookahead].Add(Reduction);
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
                foreach (Symbol Symbol in p_Actions.Keys)
                {
                    System.Xml.XmlNode SymbolNode = null;
                    if (Symbol is Variable)
                        SymbolNode = Doc.CreateElement("OnVariable");
                    else
                        SymbolNode = Doc.CreateElement("OnTerminal");
                    SymbolNode.Attributes.Append(Doc.CreateAttribute("SID"));
                    SymbolNode.Attributes.Append(Doc.CreateAttribute("Name"));
                    SymbolNode.Attributes["SID"].Value = Symbol.SID.ToString("X");
                    SymbolNode.Attributes["Name"].Value = Symbol.LocalName;
                    foreach (ILRItemSetAction Action in p_Actions[Symbol])
                        SymbolNode.AppendChild(Action.GetXMLNode(Doc, Rules));
                    Node.AppendChild(SymbolNode);
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
        public MethodGLALR1() { }

        /// <summary>
        /// Construct the LR graph
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Return true if the construction succeeded, false otherwise</returns>
        public bool Construct(Grammar Grammar, Hime.Kernel.Reporting.Reporter Log)
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
        public bool Construct(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            Log.Info("GLALR(1)", "Constructing GLALR(1) data ...");
            p_Grammar = Grammar;
            p_Graph = ConstructGraph(Grammar, Log);
            // Output conflicts
            bool Error = false;
            foreach (LRItemSet Set in p_Graph.Sets)
            {
                foreach (LRConflict Conflict in Set.Conflicts)
                {
                    Log.Report(Conflict);
                    Error = true;
                }
            }
            Log.Info("GLALR(1)", "Done !");
            return (!Error);
        }

        /// <summary>
        /// Constructs a LR Graph for the given grammar
        /// </summary>
        /// <param name="Grammar">The original grammar</param>
        /// <param name="Log">Log for output</param>
        /// <returns>Returns the constructed LR graph</returns>
        public static LRGraph ConstructGraph(CFGrammar Grammar, Hime.Kernel.Reporting.Reporter Log)
        {
            LRGraph GraphLALR1 = MethodLALR1.ConstructGraph(Grammar, Log);
            foreach (LRItemSet Set in GraphLALR1.Sets)
                Set.BuildActions(new SetActions());
            return GraphLALR1;
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