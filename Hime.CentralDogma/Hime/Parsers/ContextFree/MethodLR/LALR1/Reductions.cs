using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateReductionsLALR1 : StateReductions
    {
        private List<StateActionReduce> actionReductions;

        public override ICollection<StateActionReduce> Reductions { get { return actionReductions; } }
        public override TerminalSet ExpectedTerminals
        {
            get
            {
                TerminalSet Set = new TerminalSet();
                foreach (StateActionReduce Reduction in actionReductions)
                    Set.Add(Reduction.Lookahead);
                return Set;
            }
        }

        public StateReductionsLALR1() : base()
        {
            actionReductions = new List<StateActionReduce>();
        }

        public override void Build(State Set)
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
                        StateReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodLALR1), conflicts, Item, Set, Lookahead);
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                        StateReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodLALR1), conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                    else // No conflict
                    {
                        Reductions.Add(Lookahead, Item);
                        actionReductions.Add(new StateActionReduce(Lookahead, Item.BaseRule));
                    }
                }
            }
        }
    }
}
