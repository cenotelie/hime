namespace Hime.Parsers.CF.LR
{
    /// <summary>
    /// Represents a LR graph
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// List of the sets in the graph
        /// </summary>
        private System.Collections.Generic.List<ItemSet> p_Sets;

        /// <summary>
        /// Get a list of the sets
        /// </summary>
        /// <value>A list of the sets</value>
        public System.Collections.Generic.List<ItemSet> Sets { get { return p_Sets; } }

        /// <summary>
        /// Constructs an empty graph
        /// </summary>
        public Graph()
        {
            p_Sets = new System.Collections.Generic.List<ItemSet>();
        }

        /// <summary>
        /// Test if the graph contains a set with the same kernel
        /// </summary>
        /// <param name="Kernel">The kernel to search for</param>
        /// <returns>Returns the complete set in the graph with an equivalent kernel or null if none was found</returns>
        public ItemSet ContainsSet(ItemSetKernel Kernel)
        {
            foreach (ItemSet Potential in p_Sets)
                if (Potential.Kernel.Equals(Kernel))
                    return Potential;
            return null;
        }

        /// <summary>
        /// Add the given set to the graph if not already present
        /// </summary>
        /// <param name="Set">The set to add</param>
        /// <returns>Returns the new set or the equivalent one that was already in the graph</returns>
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

        /// <summary>
        /// Add the given set to the graph without checking
        /// </summary>
        /// <param name="Set">The set to add</param>
        public void Add(ItemSet Set)
        {
            p_Sets.Add(Set);
        }
    }
}