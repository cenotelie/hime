using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateReductionsLR0 : StateReductions
    {
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                if (this.Count != 0) {
                    Set.Add(this[0].Lookahead);
                }
                return Set;
            }
        }

        public StateReductionsLR0() : base() { }

        public override void Build(State Set)
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
                    Conflict Conflict = new Conflict("MethodLR0", Set, ConflictType.ShiftReduce);
                    Conflict.AddItem(Item);
                    conflicts.Add(Conflict);
                }
                if (Reduce != null)
                {
                    // Conflict Reduce/Reduce
                    Conflict Conflict = new Conflict("MethodLR0", Set, ConflictType.ReduceReduce);
                    Conflict.AddItem(Item);
                    Conflict.AddItem(Reduce);
                    conflicts.Add(Conflict);
                }
                else
                {
                    this.Add(new StateActionReduce(null, Item.BaseRule));
                    Reduce = Item;
                }
            }
        }
    }
}
