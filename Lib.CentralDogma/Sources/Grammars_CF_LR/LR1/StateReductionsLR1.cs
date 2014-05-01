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
    class StateReductionsLR1 : StateReductions
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

        public StateReductionsLR1() : base() { }

        public override void Build(State state)
        {
            // Recutions dictionnary for the given set
            Dictionary<Terminal, ItemLR1> Reductions = new Dictionary<Terminal, ItemLR1>();
            // Construct reductions
            foreach (ItemLR1 Item in state.Items)
            {
                if (Item.Action == ItemAction.Shift)
                    continue;
                // There is already a shift action for the lookahead => conflict
                if (state.Children.ContainsKey(Item.Lookahead)) HandleConflict_ShiftReduce("LR(1)", conflicts, Item, state, Item.Lookahead);
                // There is already a reduction action for the lookahead => conflict
                else if (Reductions.ContainsKey(Item.Lookahead)) HandleConflict_ReduceReduce("LR(1)", conflicts, Item, Reductions[Item.Lookahead], state, Item.Lookahead);
                else // No conflict
                {
                    Reductions.Add(Item.Lookahead, Item);
                    this.Add(new StateActionReduce(Item.Lookahead, Item.BaseRule));
                }
            }
        }

        public static void HandleConflict_ShiftReduce(string component, List<Conflict> Conflicts, Item ConflictuousItem, State Set, Terminal Lookahead)
        {
            // Look for previous conflict
            foreach (Conflict Previous in Conflicts)
            {
                if (Previous.ConflictType == ConflictType.ShiftReduce && Previous.ConflictSymbol == Lookahead)
                {
                    // Previous conflict
                    Previous.AddItem(ConflictuousItem);
                    return;
                }
            }
            // No previous conflict was found
            Conflict Conflict = new Conflict(component, Set, ConflictType.ShiftReduce, Lookahead);
            foreach (Item Item in Set.Items)
                if (Item.Action == ItemAction.Shift && Item.NextSymbol.ID == Lookahead.ID)
                    Conflict.AddItem(Item);
            Conflict.AddItem(ConflictuousItem);
            Conflicts.Add(Conflict);
        }

        public static void HandleConflict_ReduceReduce(string component, List<Conflict> Conflicts, Item ConflictuousItem, Item PreviousItem, State Set, Terminal Lookahead)
        {
            // Look for previous conflict
            foreach (Conflict Previous in Conflicts)
            {
                if (Previous.ConflictType == ConflictType.ReduceReduce && Previous.ConflictSymbol == Lookahead)
                {
                    // Previous conflict
                    Previous.AddItem(ConflictuousItem);
                    return;
                }
            }
            // No previous conflict was found
            Conflict Conflict = new Conflict(component, Set, ConflictType.ReduceReduce, Lookahead);
            Conflict.AddItem(PreviousItem);
            Conflict.AddItem(ConflictuousItem);
            Conflicts.Add(Conflict);
        }
    }
}
