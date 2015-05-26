/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/

package org.xowl.hime.redist.lexer;

import java.util.Arrays;

/**
 * Fast reusable buffer of token kernels
 *
 * @author Laurent Wouters
 */
public class Buffer {
    /**
     * The inner data backing this buffer
     */
    private TokenKernel[] inner;
    /**
     * The number of elements in this buffer
     */
    private int size;

    /**
     * Gets the number of elements in this buffer
     *
     * @return The number of elements in this buffer
     */
    public int getSize() {
        return size;
    }

    /**
     * Gets the i-th element of this buffer
     *
     * @param index Index within this buffer
     * @return The i-th element
     */
    public TokenKernel at(int index) {
        return inner[index];
    }

    /**
     * Initializes this buffer
     *
     * @param capacity The initial capacity
     */
    public Buffer(int capacity) {
        this.inner = new TokenKernel[capacity];
        this.size = 0;
    }

    /**
     * Resets the content of this buffer
     */
    public void reset() {
        size = 0;
    }

    /**
     * Adds an element to this buffer
     *
     * @param element An element
     */
    public void add(TokenKernel element) {
        if (size == inner.length)
            inner = Arrays.copyOf(inner, inner.length * 2);
        inner[size] = element;
        size++;
    }
}
