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

package fr.cenotelie.hime.redist.utils;

import java.util.Arrays;

/**
 * Represents a list of native integers
 *
 * @author Laurent Wouters
 */
public class IntList {
    /**
     * The inner buffer
     */
    private int[] buffer;
    /**
     * The index of the last item put into this list
     */
    private int head;
    /**
     * The initial capacity of this list
     */
    private final int initCapacity;

    /**
     * Initializes this list
     *
     * @param capacity The initial capacity of this list
     */
    public IntList(int capacity) {
        this.buffer = new int[capacity];
        this.head = -1;
        this.initCapacity = capacity;
    }

    /**
     * Gets the size of this list
     *
     * @return The size of this list
     */
    public int size() {
        return head + 1;
    }

    /**
     * Gets the value of the specified index
     *
     * @param index An index
     * @return The corresponding value
     */
    public int get(int index) {
        return buffer[index];
    }

    /**
     * Adds a value to the end of this list
     *
     * @param value The value to append
     */
    public void add(int value) {
        head++;
        if (head == buffer.length)
            buffer = Arrays.copyOf(buffer, buffer.length + initCapacity);
        buffer[head] = value;
    }

    /**
     * Gets whether this list contains the specified value
     *
     * @param value The value to look for
     * @return true if the value is in the list
     */
    public boolean contains(int value) {
        for (int i = 0; i != head + 1; i++) {
            if (buffer[i] == value)
                return true;
        }
        return false;
    }
}
