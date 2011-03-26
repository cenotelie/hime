using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class ItemSetReductionsLR1 : ItemSetReductions
    {
        private List<ItemSetActionReduce> actionReductions;

        public override ICollection<ItemSetActionReduce> Reductions
        {
            get
            {
                List<ItemSetActionReduce> Result = new List<ItemSetActionReduce>();
                foreach (ItemSetActionReduce action in actionReductions)
                    Result.Add(action);
                return Result;
            }
        }
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

        public ItemSetReductionsLR1() : base()
        {
            actionReductions = new List<ItemSetActionReduce>();
        }

        public override void Build(ItemSet Set)
        {
            // Recutions dictionnary for the given set
            Dictionary<Terminal, ItemLR1> Reductions = new Dictionary<Terminal, ItemLR1>();
            // Construct reductions
            foreach (ItemLR1 Item in Set.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                // There is already a shift action for the lookahead => conflict
                if (Set.Children.ContainsKey(Item.Lookahead)) HandleConflict_ShiftReduce(typeof(MethodLR1), conflicts, Item, Set, Item.Lookahead);
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead)) HandleConflict_ReduceReduce(typeof(MethodLR1), conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    actionReductions.Add(new ItemSetActionReduce(Item.Lookahead, Item.BaseRule));
                }
            }
        }

        public static void HandleConflict_ShiftReduce(System.Type MethodType, List<Conflict> Conflicts, Item ConflictuousItem, ItemSet Set, Terminal Lookahead)
        {
            // Look for previous conflict
            foreach (Conflict Previous in Conflicts)
            {
                if (Previous.ConflictType == ConflictType.ShiftReduce && Previous.ConflictSymbol == Lookahead)
                {
                    // Previous conflict
                    Previous.AddItem(ConflictuousItem);
                    return;
                }
            }
            // No previous conflict was found
            Conflict Conflict = new Conflict(MethodType, ConflictType.ShiftReduce, Lookahead);
            foreach (Item Item in Set.Items)
                if (Item.Action == ItemAction.Shift && Item.NextSymbol.SID == Lookahead.SID)
                    Conflict.AddItem(Item);
            Conflict.AddItem(ConflictuousItem);
            Conflicts.Add(Conflict);
        }

        public static void HandleConflict_ReduceReduce(System.Type MethodType, List<Conflict> Conflicts, Item ConflictuousItem, Item PreviousItem, ItemSet Set, Terminal Lookahead)
        {
            // Look for previous conflict
            foreach (Conflict Previous in Conflicts)
            {
                if (Previous.ConflictType == ConflictType.ReduceReduce && Previous.ConflictSymbol == Lookahead)
                {
                    // Previous conflict
                    Previous.AddItem(ConflictuousItem);
                    return;
                }
            }
            // No previous conflict was found
            Conflict Conflict = new Conflict(MethodType, ConflictType.ReduceReduce, Lookahead);
            Conflict.AddItem(PreviousItem);
            Conflict.AddItem(ConflictuousItem);
            Conflicts.Add(Conflict);
        }
    }
}
