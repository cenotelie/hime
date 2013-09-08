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
    class NFAState
    {
        private List<NFATransition> transitions;
        private FinalItem item;
        private int mark;

        public List<NFATransition> Transitions { get { return transitions; } }
        public FinalItem Item
        {
            get { return item; }
            set { item = value; }
        }
        public int Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public NFAState()
        {
            transitions = new List<NFATransition>();
            item = null;
            mark = 0;
        }

        public void AddTransition(CharSpan value, NFAState next) { transitions.Add(new NFATransition(value, next)); }
        public void ClearTransitions() { transitions.Clear(); }
    }
}