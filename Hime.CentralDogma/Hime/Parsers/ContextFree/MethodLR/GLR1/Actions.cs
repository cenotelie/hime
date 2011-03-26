using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetActionsGLR1 : ItemSetReductions
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

        public ItemSetActionsGLR1() : base()
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
            Dictionary<Terminal, ItemLR1> Reductions = new Dictionary<Terminal, ItemLR1>();
            // Construct reductions
            foreach (ItemLR1 Item in Set.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                // There is already a shift action for the lookahead => conflict
                if (Set.Children.ContainsKey(Item.Lookahead))
                {
                    ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLR1), conflicts, Item, Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead))
                {
                    ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLR1), conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    ItemSetActionReduce Reduction = new ItemSetActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
            }
        }
    }
}
