using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateReductionsGLALR1 : StateReductions
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

        public StateReductionsGLALR1() : base()
        {
            actionReductions = new List<StateActionReduce>();
        }

        public override void Build(State Set)
        {
            // Build shift actions
            foreach (Symbol Next in Set.Children.Keys)
            {
                List<StateAction> Actions = new List<StateAction>();
                Actions.Add(new StateActionShift(Next, Set.Children[Next]));
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
                        StateReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLALR1), conflicts, Item, Set, Lookahead);
                        StateActionReduce Reduction = new StateActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(Lookahead))
                    {
                        StateReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLALR1), conflicts, Item, Reductions[Lookahead], Set, Lookahead);
                        StateActionReduce Reduction = new StateActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                    // No conflict
                    else
                    {
                        Reductions.Add(Lookahead, Item);
                        StateActionReduce Reduction = new StateActionReduce(Lookahead, Item.BaseRule);
                        actionReductions.Add(Reduction);
                    }
                }
            }
        }
    }
}
