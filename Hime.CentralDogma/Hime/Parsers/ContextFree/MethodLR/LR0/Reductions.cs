using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateReductionsLR0 : StateReductions
    {
        private StateActionReduce actionReduce;

        public override ICollection<StateActionReduce> Reductions
        {
            get
            {
                List<StateActionReduce> Result = new List<StateActionReduce>();
                Result.Add(actionReduce);
                return Result;
            }
        }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                if (actionReduce != null)
                    Set.Add(actionReduce.Lookahead);
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
                    Conflict Conflict = new Conflict(typeof(MethodLR0), ConflictType.ShiftReduce);
                    Conflict.AddItem(Item);
                    conflicts.Add(Conflict);
                }
                if (Reduce != null)
                {
                    // Conflict Reduce/Reduce
                    Conflict Conflict = new Conflict(typeof(MethodLR0), ConflictType.ReduceReduce);
                    Conflict.AddItem(Item);
                    Conflict.AddItem(Reduce);
                    conflicts.Add(Conflict);
                }
                else
                {
                    actionReduce = new StateActionReduce(null, Item.BaseRule);
                    Reduce = Item;
                }
            }
        }
    }
}
