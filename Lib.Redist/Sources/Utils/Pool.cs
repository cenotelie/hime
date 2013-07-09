using System;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents a pool of reusable objects
    /// </summary>
    /// <typeparam name="T">Type of the pooled objects</typeparam>
    class Pool<T>
    {
        private Factory<T> factory;
        private T[] free;
        private int nextFree;
        private int allocated;

        /// <summary>
        /// Initializes the pool
        /// </summary>
        /// <param name="factory">The factory for the pooled objects</param>
        /// <param name="initSize">The initial size of the pool</param>
        public Pool(Factory<T> factory, int initSize)
        {
            this.factory = factory;
            this.free = new T[initSize];
            this.nextFree = -1;
            this.allocated = 0;
        }

        /// <summary>
        /// Acquires an object from this pool
        /// </summary>
        /// <returns>An object from this pool</returns>
        public T Acquire()
        {
            if (nextFree == -1)
            {
                // Create new one
                T result = factory.CreateNew(this);
                allocated++;
                return result;
            }
            else
            {
                return free[nextFree--];
            }
        }

        /// <summary>
        /// Returns the given object to this pool
        /// </summary>
        /// <param name="obj">The returned object</param>
        public void Return(T obj)
        {
            nextFree++;
            if (nextFree == free.Length)
            {
                T[] temp = new T[allocated];
                Array.Copy(free, temp, free.Length);
                free = temp;
            }
            free[nextFree] = obj;
        }
    }
}
