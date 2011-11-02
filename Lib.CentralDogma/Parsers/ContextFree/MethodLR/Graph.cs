/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Graphs;

namespace Hime.Parsers.ContextFree.LR
{
    public class Graph
    {
        private List<State> sets;
		
		// TODO: should try to put this private!!
        internal List<State> States { get { return sets; } }

        public Graph()
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

        public State ContainsSet(StateKernel Kernel)
        {
            foreach (State Potential in sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        public State AddUnique(State Set)
        {
            foreach (State Potential in sets)
            {
                // If same kernel : return the set
                if (Potential.Equals(Set))
                    return Potential;
            }
            sets.Add(Set);
            return Set;
        }

        public void Add(State Set)
        {
            sets.Add(Set);
        }
		
		internal void Serialize(DOTSerializer serializer)
		{
			foreach (State state in this.States)
			{
				string serializedState = state.ToStringForSerialization(); 
                serializer.WriteNode(serializedState, serializedState, "Set_" + serializedState + ".html");
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
    }
}
