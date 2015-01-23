/**********************************************************************
 * Copyright (c) 2014 Laurent Wouters and others
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
package org.xowl.hime.redist.parsers;

/**
 * Represents a generation in a Graph-Structured Stack
 * Because GSS nodes and edges are always created sequentially,
 * a generation basically describes a span in a buffer of GSS nodes or edges
 */
class GSSGeneration {
    /**
     * The start index of this generation in the list of nodes
     */
    private int start;
    /**
     * The number of nodes in this generation
     */
    private int count;

    /**
     * Gets the start index of this generation in the list of nodes
     *
     * @return The start index of this generation in the list of nodes
     */
    public int getStart() {
        return start;
    }

    /**
     * Gets the number of nodes in this generation
     *
     * @return The number of nodes in this generation
     */
    public int getCount() {
        return count;
    }

    /**
     * Increments the number of elements in this generation
     */
    public void increment() {
        count++;
    }

    /**
     * Initializes this generation
     *
     * @param start The start index of this generation in the list of nodes
     */
    public GSSGeneration(int start) {
        this.start = start;
        this.count = 0;
    }
}
