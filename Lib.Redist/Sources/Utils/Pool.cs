/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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
