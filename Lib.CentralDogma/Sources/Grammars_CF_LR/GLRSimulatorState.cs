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
    class GLRSimulatorState
    {

        private IList<GLRStackNode> nodes;
        private Dictionary<int, Dictionary<int, GLRStackNode>> map;
        public IList<GLRStackNode> Nodes { get { return nodes; } }

        public GLRSimulatorState()
        {
            nodes = new List<GLRStackNode>();
            map = new Dictionary<int, Dictionary<int, GLRStackNode>>();
        }
        public GLRSimulatorState(ICollection<GLRStackNode> nodes)
        {
            this.nodes = new List<GLRStackNode>();
            this.map = new Dictionary<int, Dictionary<int, GLRStackNode>>();
            foreach (GLRStackNode node in nodes)
                Add(node);
        }

        public GLRStackNode Add(State state)
        {
            int stateID = state.ID;
            if (!map.ContainsKey(stateID))
            {
                GLRStackNode node = new GLRStackNode(state);
                Dictionary<int, GLRStackNode> temp = new Dictionary<int, GLRStackNode>();
                temp.Add(node.ID, node);
                map.Add(stateID, temp);
                nodes.Add(node);
                return node;
            }
            else
            {
                Dictionary<int, GLRStackNode> temp = map[stateID];
                foreach (GLRStackNode node in temp.Values)
                    return node;
                return null;
            }
        }
        public GLRStackNode Add(GLRStackNode node)
        {
            int stateID = node.State.ID;
            int id = node.ID;
            if (!map.ContainsKey(stateID))
            {
                Dictionary<int, GLRStackNode> temp = new Dictionary<int, GLRStackNode>();
                temp.Add(id, node);
                map.Add(stateID, temp);
                nodes.Add(node);
            }
            else
            {
                Dictionary<int, GLRStackNode> temp = map[stateID];
                if (!temp.ContainsKey(id))
                {
                    temp.Add(id, node);
                    nodes.Add(node);
                }
            }
            return node;
        }
    }
}