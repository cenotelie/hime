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

package fr.cenotelie.hime.redist;

/**
 * Utility class for the compact representation of references to an element in a table
 *
 * @author Laurent Wouters
 */
public class TableElemRef {
    /**
     * Marks as other (used for SPPF nodes)
     */
    public static final byte TABLE_NONE = 0;
    /**
     * Table of tokens
     */
    public static final byte TABLE_TOKEN = 1;
    /**
     * Table of variables
     */
    public static final byte TABLE_VARIABLE = 2;
    /**
     * Tables of virtuals
     */
    public static final byte TABLE_VIRTUAL = 3;

    /**
     * Gets a reference for the specified information
     *
     * @param type  The element's type
     * @param index The element's index in its table
     * @return The corresponding reference
     */
    public static int encode(byte type, int index) {
        return ((int) type << 30) | index;
    }

    /**
     * Gets the element's type for the specified reference
     *
     * @param reference A reference
     * @return The element's type
     */
    public static byte getType(int reference) {
        return (byte) (reference >>> 30);
    }

    /**
     * Gets the element's index in its respective table
     *
     * @param reference A reference
     * @return The element's index in its respective table
     */
    public static int getIndex(int reference) {
        return (reference & 0x3FFFFFFF);
    }
}
