using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetActionsGLALR1 : ItemSetReductions
    {
        private List<ItemSetActionReduce> actionReductions;

        public override ICollection<ItemSetActionReduce> Reductions { get { return actionReductions; } }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                foreach (ItemSetActionReduce Reduction in actionReductions)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }
        }

        public ItemSetActionsGLALR1() : base()
        {
            actionReductions = new List<ItemSetActionReduce>();
        }

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
                        ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLALR1), conflicts, Item, Set, Lookahead);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                    {
                        ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLALR1), conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                    // No conflict
                    else
                    {
                        Reductions.Add(Lookahead, Item);
                        ItemSetActionReduce Reduction = new ItemSetActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                }
            }
        }
    }
}
