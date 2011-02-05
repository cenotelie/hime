namespace Hime.Generators.Parsers.ContextFree.LR
{
    /// <summary>
    /// Implements the RNGLR(1) parsing method
    /// </summary>
    public class MethodRNGLR1 : CFGrammarMethod
    {
        /// <summary>
        /// Represents the actions for a RNGLR(1) set
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
            private System.Collections.Generic.List<LRItemSetActionRNReduce> p_ActionReductions;
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
                p_ActionReductions = new System.Collections.Generic.List<LRItemSetActionRNReduce>();
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

                // Redutions dictionnary for the given set
                System.Collections.Generic.Dictionary<Terminal, LRItemLR1> Reductions = new System.Collections.Generic.Dictionary<Terminal, LRItemLR1>();
                // Construct reductions
                foreach (LRItemLR1 Item in Set.Items)
                {
                    // Check for right nulled reduction
                    if (Item.Action == LRItemAction.Shift && !Item.BaseRule.Definition.GetChoiceAtIndex(Item.DotPosition).Firsts.Contains(TerminalEpsilon.Instance))
                        continue;
                    // There is already a shift action for the lookahead => conflict
                    if (Set.Children.ContainsKey(Item.Lookahead))
                    {
                        MethodLR1.SetActions.HandleConflict_ShiftReduce(p_Conflicts, Item, Set, Item.Lookahead);
                        if (!p_Actions.ContainsKey(Item.Lookahead))
                            p_Actions.Add(Item.Lookahead, new System.Collections.Generic.List<ILRItemSetAction>());
                        p_Actions[Item.Lookahead].Add(new LRItemSetActionReduce(Item.Lookahead, Item.BaseRule));
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Item.Lookahead))
                    {
                        MethodLR1.SetActions.HandleConflict_ReduceReduce(p_Conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                        if (!p_Actions.ContainsKey(Item.Lookahead))
                            p_Actions.Add(Item.Lookahead, new System.Collections.Generic.List<ILRItemSetAction>());
                        p_Actions[Item.Lookahead].Add(new LRItemSetActionReduce(Item.Lookahead, Item.BaseRule));
                    }
                    else // No conflict
                    {
                        Reductions.Add(Item.Lookahead, Item);
                        p_ActionReductions.Add(new LRItemSetActionRNReduce(Item.Lookahead, Item.BaseRule, Item.DotPosition));
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
        public MethodRNGLR1() { }

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
            Log.RawOutput("Constructing RNGLR(1) data ...");
            p_Grammar = Grammar;
            p_Graph = ConstructGraph(Grammar, Log);
            // Output conflicts
            bool Error = false;
            foreach (LRItemSet Set in p_Graph.Sets)
            {
                foreach (LRConflict Conflict in Set.Conflicts)
                {
                    Log.EntryBegin("Warning");
                    Log.EntryAddData("RNGLR(1)");
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
            LRGraph GraphLR1 = MethodLR1.ConstructGraph(Grammar, Log);
            foreach (LRItemSet Set in GraphLR1.Sets)
                Set.BuildActions(new SetActions());
            return GraphLR1;
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