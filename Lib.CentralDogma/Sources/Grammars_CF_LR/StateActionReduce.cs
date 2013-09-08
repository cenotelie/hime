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

using System;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class StateActionReduce : StateAction
    {
        protected Terminal lookahead;
        protected CFRule toReduce;

        public ItemAction ActionType { get { return ItemAction.Shift; } }
        public Symbol OnSymbol { get { return lookahead; } }
        public Terminal Lookahead { get { return lookahead; } }
        public CFRule ToReduceRule { get { return toReduce; } }

        public StateActionReduce(Terminal Lookahead, CFRule ToReduce)
        {
        	lookahead = Lookahead;
            toReduce = ToReduce;
        }
    }
}
