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
    class GLRStackNode
    {
        private int id;
        private State state;
        private Dictionary<Symbol, Dictionary<int, GLRStackNode>> previous;

        public int ID { get { return id; } }
        public State State { get { return state; } }
        public Dictionary<Symbol, Dictionary<int, GLRStackNode>> Previous { get { return previous; } }

        public GLRStackNode(State state)
        {
            this.id = GetHashCode();
            this.state = state;
            this.previous = new Dictionary<Symbol, Dictionary<int, GLRStackNode>>(Symbol.EqualityComparer.Instance);
        }

        public void AddPrevious(Symbol symbol, GLRStackNode node)
        {
            if (!previous.ContainsKey(symbol))
            {
                Dictionary<int, GLRStackNode> nodes = new Dictionary<int, GLRStackNode>();
                nodes.Add(node.id, node);
                previous.Add(symbol, nodes);
            }
            else
            {
                Dictionary<int, GLRStackNode> nodes = previous[symbol];
                if (nodes.ContainsKey(node.id))
                    return;
                nodes.Add(node.id, node);
            }
        }
    }
}