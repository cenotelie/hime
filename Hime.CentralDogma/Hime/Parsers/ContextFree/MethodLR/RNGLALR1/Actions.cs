namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the actions for a RNGLALR(1) set
    /// </summary>
    internal class ItemSetActionsRNGLALR1 : ItemSetReductions
    {
        /// <summary>
        /// Dictionnary associating a list of possible set actions to a symbol
        /// </summary>
        private System.Collections.Generic.Dictionary<Symbol, System.Collections.Generic.List<ItemSetAction>> p_Actions;
        /// <summary>
        /// Reduction actions
        /// </summary>
        private System.Collections.Generic.List<ItemSetActionRNReduce> p_ActionReductions;

        public override System.Collections.Generic.IEnumerable<ItemSetAction> Actions
        {
            get
            {
                System.Collections.Generic.List<ItemSetAction> Result = new System.Collections.Generic.List<ItemSetAction>();
                foreach (System.Collections.Generic.List<ItemSetAction> actions in p_Actions.Values)
                    Result.AddRange(actions);
                foreach (ItemSetActionReduce action in p_ActionReductions)
                    Result.Add(action);
                return Result;
            }
        }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                foreach (ItemSetActionReduce Reduction in p_ActionReductions)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }
        }

        /// <summary>
        /// Constructs the actions
        /// </summary>
        public ItemSetActionsRNGLALR1() : base()
        {
            p_Actions = new System.Collections.Generic.Dictionary<Symbol, System.Collections.Generic.List<ItemSetAction>>();
            p_ActionReductions = new System.Collections.Generic.List<ItemSetActionRNReduce>();
        }

        /// <summary>
        /// Build the actions for the given set of items
        /// </summary>
        /// <param name="Set">The set of items</param>
        public override void Build(ItemSet Set)
        {
            // Build shift actions
            foreach (Symbol Next in Set.Children.Keys)
            {
                System.Collections.Generic.List<ItemSetAction> Actions = new System.Collections.Generic.List<ItemSetAction>();
                Actions.Add(new ItemSetActionShift(Next, Set.Children[Next]));
            }

            // Redutions dictionnary for the given set
            System.Collections.Generic.Dictionary<Terminal, ItemLALR1> Reductions = new System.Collections.Generic.Dictionary<Terminal, ItemLALR1>();
            // Construct reductions
            foreach (ItemLALR1 Item in Set.Items)
            {
                // Check for right nulled reduction
                if (Item.Action == ItemAction.Shift && !Item.BaseRule.Definition.GetChoiceAtIndex(Item.DotPosition).Firsts.Contains(TerminalEpsilon.Instance))
                    continue;
                foreach (Terminal Lookahead in Item.Lookaheads)
                {
                    // There is already a shift action for the lookahead => conflict
                    if (Set.Children.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ShiftReduce(p_Conflicts, Item, Set, Lookahead);
                        if (!p_Actions.ContainsKey(Lookahead))
                            p_Actions.Add(Lookahead, new System.Collections.Generic.List<ItemSetAction>());
                        p_Actions[Lookahead].Add(new ItemSetActionReduce(Lookahead, Item.BaseRule));
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ReduceReduce(p_Conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        if (!p_Actions.ContainsKey(Lookahead))
                            p_Actions.Add(Lookahead, new System.Collections.Generic.List<ItemSetAction>());
                        p_Actions[Lookahead].Add(new ItemSetActionReduce(Lookahead, Item.BaseRule));
                    }
                    else // No conflict
                    {
                        Reductions.Add(Lookahead, Item);
                        p_ActionReductions.Add(new ItemSetActionRNReduce(Lookahead, Item.BaseRule, Item.DotPosition));
                    }
                }
            }
        }
    }
}