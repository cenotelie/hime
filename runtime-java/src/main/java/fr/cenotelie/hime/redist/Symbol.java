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

package fr.cenotelie.hime.redist;

/**
 * Represents a grammar symbol (terminal, variable or virtual)
 *
 * @author Laurent Wouters
 */
public class Symbol {
    /**
     * Symbol ID for inexistant symbol
     */
    public static final int SID_NOTHING = 0;
    /**
     * Symbol ID of the Epsilon terminal
     */
    public static final int SID_EPSILON = 1;
    /**
     * Symbol ID of the Dollar terminal
     */
    public static final int SID_DOLLAR = 2;

    /**
     * The symbol's unique identifier
     */
    private final int id;
    /**
     * The symbol's name
     */
    private final String name;

    /**
     * Gets the symbol's unique identifier
     *
     * @return The symbol's unique identifier
     */
    public int getID() {
        return id;
    }

    /**
     * Gets the symbol's name
     *
     * @return The symbol's name
     */
    public String getName() {
        return name;
    }

    /**
     * Initializes this symbol
     *
     * @param id   The id
     * @param name The symbol's name
     */
    public Symbol(int id, String name) {
        this.id = id;
        this.name = name;
    }

    @Override
    public String toString() {
        return name;
    }
}
