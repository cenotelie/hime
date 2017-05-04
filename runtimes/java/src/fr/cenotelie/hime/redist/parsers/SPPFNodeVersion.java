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
 * Represents a version of a node in a Shared-Packed Parse Forest
 *
 * @author Laurent Wouters
 */
class SPPFNodeVersion {
    /**
     * The label of the node for this version
     */
    private final int label;
    /**
     * The children of the node for this version
     */
    private final long[] children;

    /**
     * Gets the label of the node for this version
     *
     * @return The label of the node for this version
     */
    public int getLabel() {
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
     * Initializes this node version without children
     *
     * @param label The label for this version of the node
     */
    public SPPFNodeVersion(int label) {
        this.label = label;
        this.children = null;
    }

    /**
     * Initializes this node version
     *
     * @param label         The label for this version of the node
     * @param children      A buffer of children for this version of the node
     * @param childrenCount The number of children
     */
    public SPPFNodeVersion(int label, long[] children, int childrenCount) {
        this.label = label;
        this.children = children != null && childrenCount > 0 ? Arrays.copyOf(children, childrenCount) : null;
    }
}
