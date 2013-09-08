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
    abstract class StateReductions
    {
        private List<StateActionReduce> content;
        protected List<Conflict> conflicts;
        public ICollection<Conflict> Conflicts { get { return conflicts; } }

        public StateReductions()
        {
            this.content = new List<StateActionReduce>();
            this.conflicts = new List<Conflict>();
        }

        public abstract TerminalSet ExpectedTerminals { get; }
        public abstract void Build(State state);

        public void Add(StateActionReduce action)
        {
            this.content.Add(action);
        }

        public IEnumerator<StateActionReduce> GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        public int Count { get { return this.content.Count; } }

        public StateActionReduce this[int index]
        {
            get { return this.content[index]; }
        }
    }
}