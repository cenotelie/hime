/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/
package fr.cenotelie.hime.redist.parsers;

import java.lang.reflect.Array;
import java.util.Arrays;

/**
 * Represents a pool of reusable objects
 *
 * @param <T> Type of the pooled objects
 * @author Laurent Wouters
 */
class Pool<T> {
    /**
     * The factory for this pool
     */
    private final Factory<T> factory;
    /**
     * Cache of the free objects in this pool
     */
    private T[] free;
    /**
     * Index of the next free object in this pool
     */
    private int nextFree;
    /**
     * Total number of objects in this pool
     */
    private int allocated;

    /**
     * Initializes the pool
     *
     * @param factory  The factory for the pooled objects
     * @param initSize The initial size of the pool
     * @param type     The type of the pooled objects
     */
    public Pool(Factory<T> factory, int initSize, Class<T> type) {
        this.factory = factory;
        this.free = (T[]) Array.newInstance(type, initSize);
        this.nextFree = -1;
        this.allocated = 0;
    }

    /**
     * Acquires an object from this pool
     *
     * @return An object from this pool
     */
    public T acquire() {
        if (nextFree == -1) {
            // No free object => create new one
            T result = factory.createNew();
            allocated++;
            return result;
        } else {
            // Gets a free object from the top of the pool
            return free[nextFree--];
        }
    }

    /**
     * Returns the given object to this pool
     *
     * @param obj The returned object
     */
    public void putBack(T obj) {
        nextFree++;
        if (nextFree == free.length) {
            // The cache is not big enough => extend it to the number of allocated objects
            free = Arrays.copyOf(free, allocated);
        }
        free[nextFree] = obj;
    }
}
