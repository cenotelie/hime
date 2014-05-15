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

package hime.redist;

/**
 * Represents a token as an output element of a lexer
 */
public class Token {
    private int sid;
    private int index;

    /**
     * Gets the id of the terminal symbol associated to this token
     *
     * @return The id of the terminal symbol associated to this token
     */
    public int getSymbolID() {
        return sid;
    }

    /**
     * Gets the index of this token in a lexer's stream of token
     *
     * @return The index of this token in a lexer's stream of token
     */
    public int getIndex() {
        return index;
    }

    /**
     * Initializes this token
     *
     * @param sid   The terminal's id
     * @param index The token's index
     */
    public Token(int sid, int index) {
        this.sid = sid;
        this.index = index;
    }
}
