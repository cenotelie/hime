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
 * Represents a node in a GSS
 */
class GSSNode {
    /**
     * The GLR state represented by this state
     */
    private int state;
    /**
     * The number of incoming edges
     */
    private int incomings;

    /**
     * Gets the GLR state represented by this state
     *
     * @return The GLR state represented by this state
     */
    public int getState() {
        return state;
    }

    /**
     * Gets the number of incoming edges
     *
     * @return The number of incoming edges
     */
    public int getIncomings() {
        return incomings;
    }

    /**
     * Increments the number of incoming edges
     */
    public void increment() {
        incomings++;
    }

    /**
     * Decrements the number of incoming edges
     */
    public void decrement() {
        incomings--;
    }

    /**
     * Initializes this node
     *
     * @param state The GLR state represented by this state
     */
    public GSSNode(int state) {
        this.state = state;
        this.incomings = 0;
    }
}
