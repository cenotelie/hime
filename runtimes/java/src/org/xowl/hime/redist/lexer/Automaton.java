/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/

package org.xowl.hime.redist.lexer;

import org.xowl.hime.redist.utils.BinaryInput;

/**
 * Data structure for a text lexer automaton
 */
public class Automaton {
    /**
     * Identifier of inexistant state in an automaton
     */
    public static final int DEAD_STATE = 0xFFFF;
    /**
     * Identifier of the default context
     */
    public static final int DEFAULT_CONTEXT = 0;

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
     * Gets the number of states in this automaton
     *
     * @return the number of states in this automaton
     */
    public int getStatesCount() {
        return statesCount;
    }

    /**
     * Retrieves the data of the specified state
     *
     * @param state A state's index
     * @param data  The data of the specified state
     */
    public void retrieveState(int state, State data) {
        data.setup(states, table[state]);
    }
}
