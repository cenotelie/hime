using System;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a pool of reusable sub-trees.
    /// This class is used to minimize the allocation of sub-trees, which are only temporary structures used to build the final parse tree.
    /// </summary>
    class SubTreePool
    {
        private int itemsCapacity;
        private SubTree[] free;
        private int nextFree;
        private int allocated;

        /// <summary>
        /// Initializes this pool
        /// </summary>
        /// <param name="itemsCapacity">The capacity of the sub-trees within this pool</param>
        /// <param name="size">The initial pool's size</param>
        public SubTreePool(int itemsCapacity, int size)
        {
            this.itemsCapacity = itemsCapacity;
            this.free = new SubTree[size];
            this.nextFree = -1;
            this.allocated = 0;
        }

        /// <summary>
        /// Gets a sub-tree
        /// </summary>
        /// <returns>Returns a sub-tree</returns>
        /// <remarks>
        /// If this pool does not contain a free sub-tree, it will create one.
        /// </remarks>
        public SubTree Acquire()
        {
            if (nextFree == -1)
            {
                // Create new one
                SubTree result = new SubTree(this, itemsCapacity);
                allocated++;
                return result;
            }
            else
            {
                return free[nextFree--];
            }
        }

        /// <summary>
        /// Free the given sub-tree and returns it to the pool
        /// </summary>
        /// <param name="subTree">The sub-tree to free</param>
        public void Free(SubTree subTree)
        {
            nextFree++;
            if (nextFree == free.Length)
            {
                SubTree[] temp = new SubTree[allocated];
                Array.Copy(free, temp, free.Length);
                free = temp;
            }
            free[nextFree] = subTree;
        }
    }
}
