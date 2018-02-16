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
 * Represent an op-code for a LR production
 * An op-code can be either an instruction or raw data
 *
 * @author Laurent Wouters
 */
class LROpCode {
    /**
     * Bit mask for the tree action part of an instruction
     */
    private static final char MASK_TREE_ACTION = 0x0003;
    /**
     * Bit mask for the base part of an instruction
     */
    private static final char MASK_BASE = 0xFFFC;

    /**
     * Keep the node as is
     */
    public static final byte TREE_ACTION_NONE = 0;
    /**
     * Replace the node by its children
     */
    public static final byte TREE_ACTION_REPLACE_BY_CHILDREN = 1;
    /**
     * Drop the node and all its descendants
     */
    public static final byte TREE_ACTION_DROP = 2;
    /**
     * Promote the node, i.e. replace its parent with it and insert its children where it was
     */
    public static final byte TREE_ACTION_PROMOTE = 3;
    /**
     * Replace the node by epsilon
     */
    public static final byte TREE_ACTION_REPLACE_BY_EPSILON = 4;

    /**
     * Pop an AST from the stack
     */
    public static final char BASE_POP_STACK = 0;
    /**
     * Add a virtual symbol
     */
    public static final char BASE_ADD_VIRTUAL = 4;
    /**
     * Execute a semantic action
     */
    public static final char BASE_SEMANTIC_ACTION = 8;
    /**
     * Add a null variable
     * This can be found only in RNGLR productions
     */
    public static final char BASE_ADD_NULL_VARIABLE = 16;

    /**
     * Gets the tree action included in this code
     *
     * @param code An op-code
     * @return The tree action included in this code
     */
    public static byte getTreeAction(char code) {
        return (byte) (code & MASK_TREE_ACTION);
    }

    /**
     * Gets the base instruction in this code
     *
     * @param code An op-code
     * @return The base instruction in this code
     */
    public static int getBase(char code) {
        return (code & MASK_BASE);
    }
}
