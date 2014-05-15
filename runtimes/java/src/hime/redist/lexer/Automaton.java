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

package hime.redist.lexer;

import hime.redist.utils.BinaryInput;

/**
 * Data structure for a text lexer automaton
 */
public class Automaton {
    /**
     * Table of indices in the states table
     */
    private int[] table;
    /**
     * Lexer's DFA table of states
     */
    private char[] states;
    /**
     * The number of states in this automaton
     */
    private int statesCount;

    /**
     * Gets the number of states in this automaton
     *
     * @return the number of states in this automaton
     */
    public int getStatesCount() {
        return statesCount;
    }

    /**
     * Initializes a new automaton from the given binary stream
     *
     * @param input A binary stream
     */
    public Automaton(BinaryInput input) {
        this.statesCount = input.readInt();
        this.table = new int[this.statesCount];
        for (int i = 0; i != this.statesCount; i++)
            this.table[i] = input.readInt();
        this.states = new char[(input.length() - 4 - table.length * 4) / 2];
        for (int i = 0; i != this.states.length; i++)
            this.states[i] = input.readChar();
    }

    /**
     * Loads an automaton from a resource
     *
     * @param lexerType The type of the lexer to load an automaton for
     * @param name      The full name of the resource to load from
     * @return The automaton
     */
    public static Automaton find(Class lexerType, String name) {
        BinaryInput input = new BinaryInput(lexerType, name);
        return new Automaton(input);
    }

    /**
     * Gets the offset of the given state in the table
     *
     * @param state he DFA state which offset shall be retrieved
     * @return The offset of the given DFA state
     */
    public int getOffsetOf(int state) {
        return table[state];
    }

    /**
     * Gets the recognized terminal index for the DFA at the given offset
     *
     * @param offset The DFA state's offset
     * @return The index of the terminal recognized at this state, or 0xFFFF if none
     */
    public int getStateRecognizedTerminal(int offset) {
        return states[offset];
    }

    /**
     * Checks whether the DFA state at the given offset does not have any transition
     *
     * @param offset The DFA state's offset
     * @return true if the state at the given offset has no transition
     */
    public boolean isStateDeadEnd(int offset) {
        return (states[offset + 1] == 0);
    }

    /**
     * Gets the number of non-cached transitions from the DFA state at the given offset
     *
     * @param offset The DFA state's offset
     * @return The number of non-cached transitions
     */
    public int getStateBulkTransitionsCount(int offset) {
        return states[offset + 2];
    }

    /**
     * Gets the transition from the DFA state at the given offset with the input value (max 255)
     *
     * @param offset The DFA state's offset
     * @param value  The input value
     * @return The state obtained by the transition, or 0xFFFF if none is found
     */
    public int getStateCachedTransition(int offset, int value) {
        return states[offset + 3 + value];
    }

    /**
     * Gets the transition from the DFA state at the given offset with the input value (min 256)
     *
     * @param offset The DFA state's offset
     * @param value  The input value
     * @return The state obtained by the transition, or 0xFFFF if none is found
     */
    public int getStateBulkTransition(int offset, int value) {
        int count = states[offset + 2];
        offset += 259;
        for (int i = 0; i != count; i++) {
            if (value >= states[offset] && value <= states[offset + 1])
                return states[offset + 2];
            offset += 3;
        }
        return 0xFFFF;
    }
}
