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
 * Represents a node in a Shared-Packed Parse Forest
 * A node can have multiple versions
 *
 * @author Laurent Wouters
 */
class SPPFNodeNormal extends SPPFNode {
    /**
     * The size of the version buffer
     */
    private static final int VERSION_COUNT = 4;

    /**
     * The label of this node
     */
    private final int original;
    /**
     * The different versions of this node
     */
    private SPPFNodeVersion[] versions;
    /**
     * The number of versions of this node
     */
    private int versionsCount;

    @Override
    public boolean isReplaceable() {
        return false;
    }

    public int getOriginalSymbol() {
        return original;
    }

    /**
     * Gets a specific version of this node
     *
     * @return The default version of this node
     */
    public SPPFNodeVersion getDefaultVersion() {
        return versions[0];
    }

    /**
     * Gets a specific version of this node
     *
     * @param version The version number
     * @return The requested version
     */
    public SPPFNodeVersion getVersion(int version) {
        return versions[version];
    }

    /**
     * Initializes this node
     *
     * @param identifier The identifier of this node
     * @param label      The label of this node
     */
    public SPPFNodeNormal(int identifier, int label) {
        super(identifier);
        this.original = label;
        this.versions = new SPPFNodeVersion[VERSION_COUNT];
        this.versions[0] = new SPPFNodeVersion(label);
        this.versionsCount = 1;
    }

    /**
     * Initializes this node
     *
     * @param identifier     The identifier of this node
     * @param original       The original symbol of this node
     * @param label          The label on the first version of this node
     * @param childrenBuffer A buffer for the children
     * @param childrenCount  The number of children
     */
    public SPPFNodeNormal(int identifier, int original, int label, long[] childrenBuffer, int childrenCount) {
        super(identifier);
        this.original = original;
        this.versions = new SPPFNodeVersion[VERSION_COUNT];
        this.versions[0] = new SPPFNodeVersion(label, childrenBuffer, childrenCount);
        this.versionsCount = 1;
    }

    /**
     * Adds a new version to this node
     *
     * @param label         The label for this version of the node
     * @param children      A buffer of children for this version of the node
     * @param childrenCount The number of children
     * @return The reference to this new version
     */
    public long newVersion(int label, long[] children, int childrenCount) {
        if (versionsCount == versions.length)
            versions = Arrays.copyOf(versions, versions.length + VERSION_COUNT);
        versions[versionsCount] = new SPPFNodeVersion(label, children, childrenCount);
        long result = SPPF.reference(identifier, versionsCount);
        versionsCount++;
        return result;
    }
}
