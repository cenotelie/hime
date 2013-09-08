/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
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

        public override void Build(State state)
        {
            // Recutions dictionnary for the given set
            Dictionary<Terminal, ItemLALR1> Reductions = new Dictionary<Terminal, ItemLALR1>();
            // Construct reductions
            foreach (ItemLALR1 item in state.Items)
            {
                if (item.Action == ItemAction.Shift)
                    continue;
                foreach (Terminal lookahead in item.Lookaheads)
                {
                    // There is already a shift action for the lookahead => conflict
                    if (state.Children.ContainsKey(lookahead))
                        StateReductionsLR1.HandleConflict_ShiftReduce("LALR(1)", conflicts, item, state, lookahead);
                    // There is already a reduction action for the lookahead => conflict
                    else if (Reductions.ContainsKey(lookahead))
                        StateReductionsLR1.HandleConflict_ReduceReduce("LALR(1)", conflicts, item, Reductions[lookahead], state, lookahead);
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
