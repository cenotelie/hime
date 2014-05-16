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

import hime.redist.utils.BinaryInput;

/**
 * Represents a LR action in a LR parse table
 */
class LRAction {
    /**
     * No possible action => Error
     */
    public static final char CODE_NONE = 0;
    /**
     * Apply a reduction
     */
    public static final char CODE_REDUCE = 1;
    /**
     * Shift to another state
     */
    public static final char CODE_SHIFT = 2;
    /**
     * Accept the input
     */
    public static final char CODE_ACCEPT = 3;

    /**
     * The LR action code
     */
    private char code;
    /**
     * The data associated with the action
     */
    private char data;

    /**
     * Gets the action code
     *
     * @return The LR action code
     */
    public char getCode() {
        return code;
    }

    /**
     * Gets the data associated with the action
     * <p/>
     * If the code is Reduce, it is the index of the LRProduction
     * If the code is Shift, it is the index of the next state
     *
     * @return The data associated with the action
     */
    public char getData() {
        return data;
    }

    /**
     * Initializes this action from an input binary stream
     *
     * @param input The input to load from
     */
    public LRAction(BinaryInput input) {
        this.code = input.readChar();
        this.data = input.readChar();
    }
}
