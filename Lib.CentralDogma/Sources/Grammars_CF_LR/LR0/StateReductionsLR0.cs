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
