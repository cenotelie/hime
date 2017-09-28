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

package fr.cenotelie.hime.redist.lexer;

import fr.cenotelie.hime.redist.utils.BinaryInput;

/**
 * Represents the automaton of a lexer
 * Binary data structure of lexers:
 * uint32: number of entries in the states index table
 * -- states offset table
 * each entry is of the form:
 * uint32: offset of the state from the beginning of the states table in number of uint16
 * -- states table
 * See AutomatonState
 *
 * @author Laurent Wouters
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
    private final int[] table;
    /**
     * Lexer's DFA table of states
     */
    private final char[] states;
    /**
     * The number of states in this automaton
     */
    private final int statesCount;

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
    private Automaton(BinaryInput input) {
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
     * Retrieves the data of the specified state
     *
     * @param state A state's index
     * @param data  The data of the specified state
     */
    public void retrieveState(int state, AutomatonState data) {
        data.setup(states, table[state]);
    }
}
