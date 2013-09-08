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

namespace Hime.CentralDogma.Automata
{
    class DFAStateGroup
    {
        private List<DFAState> states;

        public ICollection<DFAState> States { get { return states; } }

        public DFAState Representative { get { return states[0]; } }

        public DFAStateGroup(DFAState init)
        {
            states = new List<DFAState>();
            states.Add(init);
        }

        public void AddState(DFAState state) { states.Add(state); }

        public DFAPartition Split(DFAPartition current)
        {
            DFAPartition partition = new DFAPartition();
            foreach (DFAState state in states)
                partition.AddState(state, current);
            return partition;
        }

        public bool Contains(DFAState state) { return states.Contains(state); }
    }
}