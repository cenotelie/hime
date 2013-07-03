using System;

namespace Hime.Redist.Parsers
{
    class SubTreeFactory : Utils.Factory<SubTree>
    {
        private int capacity;

        public SubTreeFactory(int capacity)
        {
            this.capacity = capacity;
        }

        public SubTree CreateNew(Utils.Pool<SubTree> pool)
        {
            return new SubTree(pool, capacity);
        }
    }
}
