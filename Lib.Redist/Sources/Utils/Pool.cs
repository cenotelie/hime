namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a pool of reusable objects
    /// </summary>
    /// <typeparam name="T">The type of objects within the pool</typeparam>
    class Pool<T> where T : new()
    {
        private T[] free;
        private int nextFree;
        private int created;
        
        /// <summary>
        /// Initializes the pool
        /// </summary>
        public Pool(int size)
        {
            this.free = new T[size];
            this.nextFree = -1;
            this.created = 0;
        }

        /// <summary>
        /// Acquires a free object in the pool
        /// </summary>
        /// <returns>The newly acquired object</returns>
        public T Acquire()
        {
            if (nextFree > 0)
                return free[nextFree--];
            created++;
            return new T();
        }

        /// <summary>
        /// Frees the given object and return it to the pool
        /// </summary>
        /// <param name="element">The object to free</param>
        public void Free(T element)
        {
            if (nextFree + 1 >= free.Length)
            {
                T[] r = new T[created];
                System.Array.Copy(free, r, free.Length);
                free = r;
            }
            free[++nextFree] = element;
        }
    }
}
