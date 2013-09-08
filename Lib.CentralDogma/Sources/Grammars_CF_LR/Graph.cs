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
using System.Xml;
using System;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    class Graph
    {
        private List<State> sets;
		
		// TODO: should try to put this private!!
        internal List<State> States { get { return sets; } }

        internal Graph()
        {
            sets = new List<State>();
        }
		
		internal Graph(State state) : this()
		{
			this.Add(state);
			for (int i = 0; i != this.States.Count; i++)
            {
               	this.States[i].BuildGraph(this);
                this.States[i].ID = i;
            }
		}

        internal State ContainsSet(StateKernel Kernel)
        {
            foreach (State Potential in sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        internal void Add(State Set)
        {
            sets.Add(Set);
        }
		
		internal void SerializeVisual(Documentation.DOTSerializer serializer)
		{
			foreach (State state in this.States)
			{
				string serializedState = state.ToStringForSerialization(); 
                serializer.WriteNodeURL(serializedState, "Set_" + serializedState + ".html");
			}
            foreach (State state in this.States)
			{
				string serializedState = state.ToStringForSerialization(); 
                foreach (Symbol symbol in state.Children.Keys)
				{
					// TODO: should do a search for symbol.ToString().Replace("\"", "\\\"") and factor!!
                    serializer.WriteEdge(serializedState, state.Children[symbol].ToStringForSerialization(), symbol.ToString().Replace("\"", "\\\""));
				}
			}
		}
		
		internal XmlNode Serialize(XmlDocument document)
        {
            GraphInverse inverse = new GraphInverse(this);
            XmlNode nodegraph = document.CreateElement("LRGraph");
            foreach (State state in this.States)
                nodegraph.AppendChild(state.Serialize(document, inverse));
            return nodegraph;
        }


    }
}
