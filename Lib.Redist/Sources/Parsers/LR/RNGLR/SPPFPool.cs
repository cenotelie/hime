using System;

namespace Hime.Redist.Parsers
{
    class SPPFPool
    {
        private int itemsCapacity;
        private SPPFSubTree[] free;
        private int nextFree;
        private int allocated;

        public SPPFPool(int itemsCapacity, int size)
        {
            this.itemsCapacity = itemsCapacity;
            this.free = new SPPFSubTree[size];
            this.nextFree = -1;
            this.allocated = 0;
        }

        public SPPFSubTree Acquire()
        {
            if (nextFree == -1)
            {
                // Create new one
                SPPFSubTree result = new SPPFSubTree(this, itemsCapacity);
                allocated++;
                return result;
            }
            else
            {
                return free[nextFree--];
            }
        }

        public void Free(SPPFSubTree SPPFSubTree)
        {
            nextFree++;
            if (nextFree == free.Length)
            {
                SPPFSubTree[] temp = new SPPFSubTree[allocated];
                Array.Copy(free, temp, free.Length);
                free = temp;
            }
            free[nextFree] = SPPFSubTree;
        }
    }
}
