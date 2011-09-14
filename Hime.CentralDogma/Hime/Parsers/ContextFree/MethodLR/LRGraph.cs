/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree.LR
{
    public class Graph
    {
        private List<State> sets;

        public List<State> States { get { return sets; } }

        public Graph()
        {
            sets = new List<State>();
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
    }
}
