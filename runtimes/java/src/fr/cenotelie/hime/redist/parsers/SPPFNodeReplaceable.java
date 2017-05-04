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

import java.util.Arrays;

/**
 * Represents a node in a Shared-Packed Parse Forest that can be replaced by its children
 *
 * @author Laurent Wouters
 */
class SPPFNodeReplaceable extends SPPFNode {
    /**
     * The label of this node
     */
    private final int label;
    /**
     * The children of this node
     */
    private final long[] children;
    /**
     * The tree actions on the children of this node
     */
    private final byte[] actions;

    @Override
    public boolean isReplaceable() {
        return true;
    }

    public int getOriginalSymbol() {
        return label;
    }

    /**
     * Gets the number of children for this version of the node
     *
     * @return The number of children for this version of the node
     */
    public int getChildrenCount() {
        return (children != null ? children.length : 0);
    }

    /**
     * Gets the children of the node for this version
     *
     * @return The children of the node for this version
     */
    public long[] getChildren() {
        return children;
    }

    /**
     * Gets the tree actions on the children of this node
     *
     * @return The tree actions on the children of this node
     */
    public byte[] getActions() {
        return actions;
    }

    /**
     * Initializes this node
     *
     * @param identifier     The identifier of this node
     * @param label          The label of this node
     * @param childrenBuffer A buffer for the children
     * @param actionsBuffer  A buffer for the actions on the children
     * @param childrenCount  The number of children
     */
    public SPPFNodeReplaceable(int identifier, int label, long[] childrenBuffer, byte[] actionsBuffer, int childrenCount) {
        super(identifier);
        this.label = label;
        this.children = childrenCount > 0 ? Arrays.copyOf(childrenBuffer, childrenCount) : null;
        this.actions = childrenCount > 0 ? Arrays.copyOf(actionsBuffer, childrenCount) : null;
    }
}
