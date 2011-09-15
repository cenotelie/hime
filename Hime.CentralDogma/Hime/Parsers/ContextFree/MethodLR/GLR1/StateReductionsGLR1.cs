/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class StateReductionsGLR1 : StateReductions
    {
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                foreach (StateActionReduce Reduction in this)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }
        }

        public StateReductionsGLR1() : base() { }

        public override void Build(State Set)
        {
            // Build shift actions
            foreach (Symbol Next in Set.Children.Keys)
            {
                List<StateAction> Actions = new List<StateAction>();
                Actions.Add(new StateActionShift(Next, Set.Children[Next]));
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
                    StateReductionsLR1.HandleConflict_ShiftReduce("GLR(1)", conflicts, Item, Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    this.Add(Reduction);
                }
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead))
                {
                    StateReductionsLR1.HandleConflict_ReduceReduce("GLR(1)", conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    this.Add(Reduction);
                }
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    this.Add(Reduction);
                }
            }
        }
    }
}
