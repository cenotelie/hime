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
 * Represents a match in the input
 *
 * @author Laurent Wouters
 */
public class TokenMatch {
    /**
     * The matching DFA state
     */
    public final int state;
    /**
     * Length of the matched input
     */
    public final int length;

    /**
     * Gets whether this is match indicates a success
     *
     * @return Whether this is match indicates a success
     */
    public boolean isSuccess() {
        return state != Automaton.DEAD_STATE;
    }

    /**
     * Initializes a failing match
     *
     * @param length The number of characters to advance in the input
     */
    public TokenMatch(int length) {
        this.state = Automaton.DEAD_STATE;
        this.length = length;
    }

    /**
     * Initializes a match
     *
     * @param state  The matching DFA state
     * @param length Length of the matched input
     */
    public TokenMatch(int state, int length) {
        this.state = state;
        this.length = length;
    }
}
