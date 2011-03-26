using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetReductionsLALR1 : ItemSetReductions
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

        public ItemSetReductionsLALR1() : base()
        {
            actionReductions = new List<ItemSetActionReduce>();
        }

        public override void Build(ItemSet Set)
        {
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
                        ItemSetReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodLALR1), conflicts, Item, Set, Lookahead);
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                        ItemSetReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodLALR1), conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                    else // No conflict
                    {
                        Reductions.Add(Lookahead, Item);
                        actionReductions.Add(new ItemSetActionReduce(Lookahead, Item.BaseRule));
                    }
                }
            }
        }
    }
}
