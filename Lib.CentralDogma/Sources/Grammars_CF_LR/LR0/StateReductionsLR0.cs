/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class StateReductionsLR0 : StateReductions
    {
        public override TerminalSet ExpectedTerminals
        {
            get { return ItemLR0.EmptySet; }
        }

        public StateReductionsLR0() : base() { }

        public override void Build(State state)
        {
            Item reduce = null;
            // Look for Reduce actions
            foreach (Item item in state.Items)
            {
                // Ignore Shift actions
                if (item.Action == ItemAction.Shift)
                    continue;
                if (state.Children.Count != 0)
                {
                    // Conflict Shift/Reduce
                    Conflict Conflict = new Conflict("LR(0)", state, ConflictType.ShiftReduce);
                    Conflict.AddItem(item);
                    conflicts.Add(Conflict);
                }
                if (reduce != null)
                {
                    // Conflict Reduce/Reduce
                    Conflict Conflict = new Conflict("LR(0)", state, ConflictType.ReduceReduce);
                    Conflict.AddItem(item);
                    Conflict.AddItem(reduce);
                    conflicts.Add(Conflict);
                }
                else
                {
                	this.Add(new StateActionReduce(NullTerminal.Instance, item.BaseRule));
                    reduce = item;
                }
            }
        }
    }
}
