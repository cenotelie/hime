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

package hime.redist.parsers;

/**
 * Utility class for the compact representation of a reference to a symbol in a table
 */
class SymbolRef {
    /**
     * Gets a reference for the specified information
     *
     * @param type  The symbol's type
     * @param index The symbol's index in its respective table
     * @return The corresponding reference
     */
    public static int encode(byte type, int index) {
        return ((int) type << 30) | index;
    }

    /**
     * Gets the type of the symbol for the specified reference
     *
     * @param ref A reference to a symbol
     * @return The type of the symbol referred to
     */
    public static byte getType(int ref) {
        return (byte) (ref >>> 30);
    }

    /**
     * Gets the index of the symbol in its respective table
     *
     * @param ref A reference to a symbol
     * @return The index of the symbol in its respective table
     */
    public static int getIndex(int ref) {
        return (ref & 0x3FFFFFFF);
    }
}
