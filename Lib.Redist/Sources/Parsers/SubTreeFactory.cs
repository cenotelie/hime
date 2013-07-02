using System;

namespace Hime.Redist.Parsers
{
    class SubTreeFactory : Utils.PooledObjectFactory<SubTree>
    {
        private int capacity;

        public SubTreeFactory(int capacity)
        {
            this.capacity = capacity;
        }

        public SubTree CreateNew(Utils.ObjectPool<SubTree> pool)
        {
            return new SubTree(pool, capacity);
        }
    }
}
