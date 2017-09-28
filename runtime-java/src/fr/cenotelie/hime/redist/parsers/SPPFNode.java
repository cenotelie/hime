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

/**
 * Represents a node in a Shared-Packed Parse Forest
 *
 * @author Laurent Wouters
 */
abstract class SPPFNode {
    /**
     * The identifier of this node
     */
    protected final int identifier;

    /**
     * Gets the identifier of this node
     *
     * @return The identifier of this node
     */
    public int getIdentifier() {
        return identifier;
    }

    /**
     * Gets whether this node must be replaced by its children
     *
     * @return Whether this node must be replaced by its children
     */
    public abstract boolean isReplaceable();

    /**
     * Gets the original symbol for this node
     *
     * @return The original symbol for this node
     */
    public abstract int getOriginalSymbol();

    /**
     * Initializes this node
     *
     * @param identifier The identifier of this node
     */
    public SPPFNode(int identifier) {
        this.identifier = identifier;
    }
}
