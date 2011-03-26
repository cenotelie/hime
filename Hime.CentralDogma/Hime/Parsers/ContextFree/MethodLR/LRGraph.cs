using System.Collections.Generic;

namespace Hime.Parsers.CF.LR
{
    class Graph
    {
        private List<ItemSet> p_Sets;

        public List<ItemSet> Sets { get { return p_Sets; } }

        public Graph()
        {
            p_Sets = new List<ItemSet>();
        }

        public ItemSet ContainsSet(ItemSetKernel Kernel)
        {
            foreach (ItemSet Potential in p_Sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        public ItemSet AddUnique(ItemSet Set)
        {
            foreach (ItemSet Potential in p_Sets)
            {
                // If same kernel : return the set
                if (Potential.Equals(Set))
                    return Potential;
            }
            p_Sets.Add(Set);
            return Set;
        }

        public void Add(ItemSet Set)
        {
            p_Sets.Add(Set);
        }
    }
}
