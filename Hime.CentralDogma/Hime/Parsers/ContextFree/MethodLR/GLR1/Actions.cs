namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the actions for a GLR(1) set
    /// </summary>
    internal class ItemSetActionsGLR1 : ItemSetReductions
    {
        /// <summary>
        /// Reduction actions
        /// </summary>
        private System.Collections.Generic.List<ItemSetActionReduce> p_ActionReductions;

        public override System.Collections.Generic.IEnumerable<ItemSetActionReduce> Reductions { get { return p_ActionReductions; } }
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
        public ItemSetActionsGLR1() : base()
        {
            p_ActionReductions = new System.Collections.Generic.List<ItemSetActionReduce>();
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

            // Recutions dictionnary for the given set
            System.Collections.Generic.Dictionary<Terminal, ItemLR1> Reductions = new System.Collections.Generic.Dictionary<Terminal, ItemLR1>();
            // Construct reductions
            foreach (ItemLR1 Item in Set.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                // There is already a shift action for the lookahead => conflict
                if (Set.Children.ContainsKey(Item.Lookahead))
                {
                    ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLR1), p_Conflicts, Item, Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    p_ActionReductions.Add(Reduction);
                }
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead))
                {
                    ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLR1), p_Conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    p_ActionReductions.Add(Reduction);
                }
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    p_ActionReductions.Add(Reduction);
                }
            }
        }
    }
}