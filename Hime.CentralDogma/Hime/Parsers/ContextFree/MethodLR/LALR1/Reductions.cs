using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    class StateReductionsLALR1 : StateReductions
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

        public StateReductionsLALR1() : base() { }

        public override void Build(State set)
        {
            // Recutions dictionnary for the given set
            Dictionary<Terminal, ItemLALR1> Reductions = new Dictionary<Terminal, ItemLALR1>();
            // Construct reductions
            foreach (ItemLALR1 item in set.Items)
            {
                if (item.Action == ItemAction.Shift)
                    continue;
                foreach (Terminal lookahead in item.Lookaheads)
                {
                    // There is already a shift action for the lookahead => conflict
                    if (set.Children.ContainsKey(lookahead))
                        StateReductionsLR1.HandleConflict_ShiftReduce("LALR(1)", conflicts, item, set, lookahead);
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(lookahead))
                        StateReductionsLR1.HandleConflict_ReduceReduce("LALR(1)", conflicts, item, Reductions[lookahead], set, lookahead);
                    else // No conflict
                    {
                        Reductions.Add(lookahead, item);
                        this.Add(new StateActionReduce(lookahead, item.BaseRule));
                    }
                }
            }
        }
    }
}
