namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the actions for a RNGLALR(1) set
    /// </summary>
    internal class ItemSetReductionsRNGLALR1 : ItemSetReductions
    {
        /// <summary>
        /// Reduction actions
        /// </summary>
        private System.Collections.Generic.List<ItemSetActionRNReduce> p_ActionReductions;

        public override System.Collections.Generic.IEnumerable<ItemSetActionReduce> Reductions
        {
            get
            {
                System.Collections.Generic.List<ItemSetActionReduce> Temp = new System.Collections.Generic.List<ItemSetActionReduce>();
                foreach (ItemSetActionRNReduce action in p_ActionReductions)
                    Temp.Add(action);
                return Temp;
            }
        }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                foreach (ItemSetActionRNReduce Reduction in p_ActionReductions)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }
        }

        /// <summary>
        /// Constructs the actions
        /// </summary>
        public ItemSetReductionsRNGLALR1() : base()
        {
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
                        ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodRNGLALR1), p_Conflicts, Item, Set, Lookahead);
                        ItemSetActionRNReduce Reduction = new ItemSetActionRNReduce(Lookahead, Item.BaseRule, Item.DotPosition);
                        p_ActionReductions.Add(Reduction);
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodRNGLALR1), p_Conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        ItemSetActionRNReduce Reduction = new ItemSetActionRNReduce(Lookahead, Item.BaseRule, Item.DotPosition);
                        p_ActionReductions.Add(Reduction);
                    }
                    else // No conflict
                    {
                        Reductions.Add(Lookahead, Item);
                        ItemSetActionRNReduce Reduction = new ItemSetActionRNReduce(Lookahead, Item.BaseRule, Item.DotPosition);
                        p_ActionReductions.Add(Reduction);
                    }
                }
            }
        }
    }
}