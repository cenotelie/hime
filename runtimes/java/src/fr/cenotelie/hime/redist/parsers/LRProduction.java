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

import fr.cenotelie.hime.redist.utils.BinaryInput;

/**
 * Represents a rule's production in a LR parser
 *
 * @author Laurent Wouters
 */
class LRProduction {
    /**
     * Index of the rule's head in the parser's array of variables
     */
    private final int head;
    /**
     * Action of the rule's head (replace or not)
     */
    private final byte headAction;
    /**
     * Size of the rule's body by only counting terminals and variables
     */
    private final int reducLength;
    /**
     * Bytecode for the rule's production
     */
    private final char[] bytecode;

    /**
     * Gets the index of the rule's head in the parser's array of variables
     *
     * @return The index of the rule's head in the parser's array of variables
     */
    public int getHead() {
        return head;
    }

    /**
     * Gets the action of the rule's head (replace or not)
     *
     * @return The action of the rule's head (replace or not)
     */
    public byte getHeadAction() {
        return headAction;
    }

    /**
     * Gets the size of the rule's body by only counting terminals and variables
     *
     * @return The size of the rule's body by only counting terminals and variables
     */
    public int getReductionLength() {
        return reducLength;
    }

    /**
     * Gets the length of the bytecode
     *
     * @return The length of the bytecode
     */
    public int getBytecodeLength() {
        return bytecode.length;
    }

    /**
     * Gets the op-code at the specified index in the bytecode
     *
     * @param index Index in the bytecode
     * @return The op-code at the specified index in the bytecode
     */
    public char getOpcode(int index) {
        return bytecode[index];
    }

    /**
     * Loads a new instance of the LRProduction class from a binary representation
     *
     * @param input The binary reader to read from
     */
    public LRProduction(BinaryInput input) {
        this.head = input.readChar();
        this.headAction = input.readByte();
        this.reducLength = input.readByte();
        this.bytecode = new char[input.readByte()];
        for (int i = 0; i != this.bytecode.length; i++)
            this.bytecode[i] = input.readChar();
    }
}
