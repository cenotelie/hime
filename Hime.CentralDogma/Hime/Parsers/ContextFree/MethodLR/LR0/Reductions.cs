using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetReductionsLR0 : ItemSetReductions
    {
        private ItemSetActionReduce p_ActionReduce;

        public override ICollection<ItemSetActionReduce> Reductions
        {
            get
            {
                List<ItemSetActionReduce> Result = new List<ItemSetActionReduce>();
                Result.Add(p_ActionReduce);
                return Result;
            }
        }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                if (p_ActionReduce != null)
                    Set.Add(p_ActionReduce.Lookahead);
                return Set;
            }
        }

        public ItemSetReductionsLR0() : base() { }

        public override void Build(ItemSet Set)
        {
            Item Reduce = null;
            // Look for Reduce actions
            foreach (Item Item in Set.Items)
            {
                // Ignore Shift actions
                if (Item.Action == ItemAction.Shift)
                    continue;
                if (Set.Children.Count != 0)
                {
                    // Conflict Shift/Reduce
                    Conflict Conflict = new Conflict(typeof(MethodLR0), ConflictType.ShiftReduce);
                    Conflict.AddItem(Item);
                    p_Conflicts.Add(Conflict);
                }
                if (Reduce != null)
                {
                    // Conflict Reduce/Reduce
                    Conflict Conflict = new Conflict(typeof(MethodLR0), ConflictType.ReduceReduce);
                    Conflict.AddItem(Item);
                    Conflict.AddItem(Reduce);
                    p_Conflicts.Add(Conflict);
                }
                else
                {
                    p_ActionReduce = new ItemSetActionReduce(null, Item.BaseRule);
                    Reduce = Item;
                }
            }
        }
    }
}
