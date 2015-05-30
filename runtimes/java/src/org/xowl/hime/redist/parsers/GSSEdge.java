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

/**
 * Represents an edge in a Graph-Structured Stack
 *
 * @author Laurent Wouters
 */
class GSSEdge {
    /**
     * The index of the node from which this edge starts
     */
    private final int from;
    /**
     * The index of the node to which this edge arrives to
     */
    private final int to;
    /**
     * The label on this edge
     */
    private final GSSLabel label;

    /**
     * Gets the index of the node from which this edge starts
     *
     * @return The index of the node from which this edge starts
     */
    public int getFrom() {
        return from;
    }

    /**
     * Gets the index of the node to which this edge arrives to
     *
     * @return The index of the node to which this edge arrives to
     */
    public int getTo() {
        return to;
    }

    /**
     * Gets the label on this edge
     *
     * @return The label on this edge
     */
    public GSSLabel getLabel() {
        return label;
    }

    /**
     * Initializes this edge
     *
     * @param from Index of the node from which this edge starts
     * @param to   Index of the node to which this edge arrives to
     */
    public GSSEdge(int from, int to, GSSLabel label) {
        this.from = from;
        this.to = to;
        this.label = label;
    }
}
