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
 * Represents the kernel of a token, i.e. the identifying information of a token
 *
 * @author Laurent Wouters
 */
public class TokenKernel {
    /**
     * The identifier of the matched terminal
     */
    private final int terminalID;
    /**
     * The token's index in its repository
     */
    private final int index;

    /**
     * Gets the identifier of the matched terminal
     *
     * @return The identifier of the matched terminal
     */
    public int getTerminalID() {
        return terminalID;
    }

    /**
     * Gets the token's index in its repository
     *
     * @return The token's index in its repository
     */
    public int getIndex() {
        return index;
    }

    /**
     * Initializes this kernel
     *
     * @param id    The identifier of the matched terminal
     * @param index The token's index in its repository
     */
    public TokenKernel(int id, int index) {
        this.terminalID = id;
        this.index = index;
    }
}
