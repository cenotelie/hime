using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class Graph
    {
        private List<ItemSet> sets;

        public List<ItemSet> Sets { get { return sets; } }

        public Graph()
        {
            sets = new List<ItemSet>();
        }

        public ItemSet ContainsSet(ItemSetKernel Kernel)
        {
            foreach (ItemSet Potential in sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        public ItemSet AddUnique(ItemSet Set)
        {
            foreach (ItemSet Potential in sets)
            {
                // If same kernel : return the set
                if (Potential.Equals(Set))
                    return Potential;
            }
            sets.Add(Set);
            return Set;
        }

        public void Add(ItemSet Set)
        {
            sets.Add(Set);
        }
    }
}
