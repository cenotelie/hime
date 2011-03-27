using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class StateReductionsGLR1 : StateReductions
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

        public StateReductionsGLR1() : base()
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
            Dictionary<Terminal, ItemLR1> Reductions = new Dictionary<Terminal, ItemLR1>();
            // Construct reductions
            foreach (ItemLR1 Item in Set.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                // There is already a shift action for the lookahead => conflict
                if (Set.Children.ContainsKey(Item.Lookahead))
                {
                    StateReductionsLR1.HandleConflict_ShiftReduce(typeof(MethodGLR1), conflicts, Item, Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead))
                {
                    StateReductionsLR1.HandleConflict_ReduceReduce(typeof(MethodGLR1), conflicts, Item, Reductions[Item.Lookahead], Set, Item.Lookahead);
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    StateActionReduce Reduction = new StateActionReduce(Item.Lookahead, Item.BaseRule);
                    actionReductions.Add(Reduction);
                }
            }
        }
    }
}
