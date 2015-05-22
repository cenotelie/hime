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

/**
 * Represents a state in a lexer's automaton
 */
public class State {
    /**
     * The automaton table
     */
    private char[] table;
    /**
     * The offset of this state within the table
     */
    private int offset;

    /**
     * Setups this state data
     *
     * @param table  The automaton table
     * @param offset An offset within the table
     */
    void setup(char[] table, int offset) {
        this.table = table;
        this.offset = offset;
    }

    /**
     * Gets the number of matched terminals in this state
     *
     * @return The number of matched terminals in this state
     */
    public int getTerminalsCount() {
        return table[offset];
    }

    /**
     * Retrieves the i-th matched terminal in this state
     *
     * @param index The index of the matched terminal
     * @param data  The matched terminal data to fill
     */
    public void retrieveTerminal(int index, MatchedTerminal data) {
        data.setup(table[offset + index * 2 + 3], table[offset + index * 2 + 4]);
    }

    /**
     * Gets whether this state is a dead end (no more transition)
     *
     * @return <code>true</code> if this state is a dead end (no more transition)
     */
    public boolean isDeadEnd() {
        return (table[offset + 1] == 0);
    }

    /**
     * Gets the target of a transition from this state on the specified value
     *
     * @param value An input value
     * @return The target of the transition
     */
    public int getTargetBy(int value) {
        if (value <= 255)
            return table[offset + table[offset] * 2 + 3 + value];
        int current = offset + 3 + table[offset] * 2 + 256;
        int count = table[offset + 2];
        for (int i = 0; i != count; i++) {
            if (value >= table[current] && value <= table[current + 1])
                return table[current + 2];
            current += 3;
        }
        return Automaton.DEAD_STATE;
    }
}
