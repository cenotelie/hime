/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters
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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.utils.BinaryInput;

/**
 * Represents the contexts of a LR state
 *
 * @author Laurent Wouters
 */
public class LRContexts {
    /**
     * The contexts
     */
    private final char[] content;

    /**
     * Gets the number of contexts
     *
     * @return The number of contexts
     */
    public int size() {
        return content != null ? content.length : 0;
    }

    /**
     * Gets the i-th context
     *
     * @param index An index
     * @return The i-th context
     */
    public int get(int index) {
        return content[index];
    }

    /**
     * Loads the contexts from the specified input
     *
     * @param input An input
     */
    public LRContexts(BinaryInput input) {
        int count = input.readChar();
        if (count > 0) {
            this.content = new char[count];
            for (int i = 0; i != count; i++) {
                this.content[i] = input.readChar();
            }
        } else {
            this.content = null;
        }
    }

    /**
     * Gets whether the specified context is in this collection
     *
     * @param context A context
     * @return <code>true</code> if the specified context is in this collection
     */
    public boolean contains(int context) {
        if (content != null) {
            for (int i = 0; i != content.length; i++)
                if (content[i] == context)
                    return true;
        }
        return false;
    }
}
