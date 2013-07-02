using System;

namespace Hime.Redist.Utils
{
	class ArrayPool<T>
	{
		private int capacity;
		private T[][] free;
        private int nextFree;
        private int allocated;

        public ArrayPool(int capacity, int poolSize)
        {
            this.capacity = capacity;
            this.free = new T[poolSize][];
            this.nextFree = -1;
            this.allocated = 0;
        }

        public T[] Acquire()
        {
            if (nextFree == -1)
            {
                // Create new one
                allocated++;
                return new T[capacity];
            }
            else
            {
                return free[nextFree--];
            }
        }

        public void Returns(T[] item)
        {
            nextFree++;
            if (nextFree == free.Length)
            {
                T[][] temp = new T[allocated][];
                Array.Copy(free, temp, free.Length);
                free = temp;
            }
            free[nextFree] = item;
        }
	}
}