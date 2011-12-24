/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Graphs;
using System.Xml;
using System;

namespace Hime.Parsers.ContextFree.LR
{
    public class Graph
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
		
		internal void SerializeVisual(DOTSerializer serializer)
		{
			foreach (State state in this.States)
			{
				string serializedState = state.ToStringForSerialization(); 
                serializer.WriteNodeURL(serializedState, "Set_" + serializedState + ".html");
			}
            foreach (State state in this.States)
			{
				string serializedState = state.ToStringForSerialization(); 
                foreach (GrammarSymbol symbol in state.Children.Keys)
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
