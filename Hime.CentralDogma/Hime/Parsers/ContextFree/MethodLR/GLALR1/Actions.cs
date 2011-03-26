using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents the actions for a GLALR(1) set
    /// </summary>
    class ItemSetActionsGLALR1 : ItemSetReductions
    {
        /// <summary>
        /// Reduction actions
        /// </summary>
        private List<ItemSetActionReduce> p_ActionReductions;

        public override ICollection<ItemSetActionReduce> Reductions { get { return p_ActionReductions; } }
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
        public ItemSetActionsGLALR1() : base()
        {
            p_ActionReductions = new List<ItemSetActionReduce>();
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
                List<ItemSetAction> Actions = new List<ItemSetAction>();
                Actions.Add(new ItemSetActionShift(Next, Set.Children[Next]));
            }

            // Recutions dictionnary for the given set
            Dictionary<Terminal, ItemLALR1> Reductions = new Dictionary<Terminal, ItemLALR1>();
            // Construct reductions
            foreach (ItemLALR1 Item in Set.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                foreach (Terminal Lookahead in Item.Lookaheads)
                {
                    // There is already a shift action for the lookahead => conflict
                    if (Set.Children.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLALR1), p_Conflicts, Item, Set, Lookahead);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        p_ActionReductions.Add(Reduction);
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLALR1), p_Conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        p_ActionReductions.Add(Reduction);
                    }
                    // No conflict
                    else
                    {
                        Reductions.Add(Lookahead, Item);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        p_ActionReductions.Add(Reduction);
                    }
                }
            }
        }
    }
}
