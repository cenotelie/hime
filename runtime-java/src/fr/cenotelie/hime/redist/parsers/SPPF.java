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

import fr.cenotelie.hime.redist.utils.BigList;

/**
 * Represents a Shared-Packed Parse Forest
 *
 * @author Laurent Wouters
 */
class SPPF {
    /**
     * Represents the epsilon node
     */
    public static final int EPSILON = -1;

    /**
     * The nodes in the SPPF
     */
    private final BigList<SPPFNode> nodes;

    /**
     * Initializes this SPPF
     */
    public SPPF() {
        this.nodes = new BigList<>(SPPFNode.class, SPPFNode[].class);
    }

    /**
     * Gets the SPPF node for the specified identifier
     *
     * @param identifier The identifier of an SPPF node
     * @return The SPPF node
     */
    public SPPFNode getNode(int identifier) {
        return nodes.get(identifier);
    }

    /**
     * Creates a new single node in the SPPF
     *
     * @param label The original label for this node
     * @return The identifier of the new node
     */
    public int newNode(int label) {
        return nodes.add(new SPPFNodeNormal(nodes.size(), label));
    }

    /**
     * Creates a new single node in the SPPF
     *
     * @param original       The original symbol of this node
     * @param label          The label on the first version of this node
     * @param childrenBuffer A buffer for the children
     * @param childrenCount  The number of children
     * @return The identifier of the new node
     */
    public int newNode(int original, int label, long[] childrenBuffer, int childrenCount) {
        return nodes.add(new SPPFNodeNormal(nodes.size(), original, label, childrenBuffer, childrenCount));
    }

    /**
     * Creates a new replaceable node in the SPPF
     *
     * @param label          The label of this node
     * @param childrenBuffer A buffer for the children
     * @param actionsBuffer  A buffer for the actions on the children
     * @param childrenCount  The number of children
     * @return The identifier of the new node
     */
    public int newReplaceableNode(int label, long[] childrenBuffer, byte[] actionsBuffer, int childrenCount) {
        return nodes.add(new SPPFNodeReplaceable(nodes.size(), label, childrenBuffer, actionsBuffer, childrenCount));
    }

    /**
     * Gets a reference to a node in a specific version
     *
     * @param nodeId  The identifier of the node to refer to
     * @param version The version of the node to refer to
     * @return The reference
     */
    public static long reference(int nodeId, int version) {
        return (((long) nodeId) << 32) | ((long) version);
    }

    /**
     * Gets the identifier of the node referred to
     *
     * @param reference The reference
     * @return The identifier of the node
     */
    public static int refNodeId(long reference) {
        return (int) (reference >>> 32);
    }

    /**
     * Gets the version of the node referred to
     *
     * @param reference The reference
     * @return The version of the node
     */
    public static int refVersion(long reference) {
        return (int) (reference & 0xFFFFFFFFL);
    }
}
