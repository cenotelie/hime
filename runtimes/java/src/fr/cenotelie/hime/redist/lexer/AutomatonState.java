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

/**
 * Represents a state in the automaton of a lexer
 * <p>
 * Binary data structure:
 * uint16: number of matched terminals
 * uint16: total number of transitions
 * uint16: number of non-cached transitions
 * -- matched terminals
 * uint16: context identifier
 * uint16: index of the matched terminal
 * -- cache: 256 entries
 * uint16: next state's index for index of the entry
 * -- transitions
 * each transition is of the form:
 * uint16: start of the range
 * uint16: end of the range
 * uint16: next state's index
 *
 * @author Laurent Wouters
 */
class AutomatonState {
    /**
     * The automaton table
     */
    private char[] table;
    /**
     * The offset of this state within the table
     */
    private int offset;

    /**
     * Setups this state
     *
     * @param table  The automaton table
     * @param offset The offset of this state within the table
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
    public int getTerminalCount() {
        return table[offset];
    }

    /**
     * Gets the index of the first terminal matched in this state
     *
     * @return The index of the matched terminal
     */
    public int getTerminal() {
        return table[offset + 4];
    }

    /**
     * Gets the i-th matched terminal in this state
     *
     * @param index The index of the matched terminal
     * @param cache The matched terminal data
     */
    public void getTerminal(int index, MatchedTerminal cache) {
        cache.setup(table[offset + index * 2 + 3], table[offset + index * 2 + 4]);
    }

    /**
     * Gets whether this state is a dead end (no more transition)
     *
     * @return Whether this state is a dead end (no more transition)
     */
    public boolean isDeadEnd() {
        return (table[offset + 1] == 0);
    }

    /**
     * Gets the number of non-cached transitions in this state
     *
     * @return The number of non-cached transitions in this state
     */
    public int getBulkTransitionCount() {
        return table[offset + 2];
    }

    /**
     * Gets the target of a transition from this state on the specified value
     *
     * @param value An input value
     * @return The target of the transition
     */
    public int getTargetBy(int value) {
        if (value <= 255)
            return table[offset + getTerminalCount() * 2 + 3 + value];
        int current = offset + 3 + getTerminalCount() * 2 + 256;
        for (int i = 0; i != table[offset + 2]; i++) {
            if (value >= table[current] && value <= table[current + 1])
                return table[current + 2];
            current += 3;
        }
        return Automaton.DEAD_STATE;
    }

    /**
     * Gets the target of the cached transition for the specified value
     *
     * @param value An input value
     * @return The target of the cached transition
     */
    public int getCachedTransition(int value) {
        return table[offset + getTerminalCount() * 2 + 3 + value];
    }

    /**
     * Gets the target of the i-th non-cached transition in this state
     *
     * @param index The non-cached transition index
     * @return The target of the transition
     */
    public int getBulkTransition(int index) {
        return table[offset + 3 + getTerminalCount() * 2 + 256 + index * 3 + 2];
    }
}
