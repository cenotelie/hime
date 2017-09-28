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
 * Represents a position in term of line and column in a text input
 *
 * @author Laurent Wouters
 */
public class TextPosition {
    /**
     * The line number
     */
    private final int line;
    /**
     * The column number
     */
    private final int column;

    /**
     * Gets the line number
     *
     * @return The line number
     */
    public int getLine() {
        return line;
    }

    /**
     * Gets the column number
     *
     * @return The column number
     */
    public int getColumn() {
        return column;
    }

    /**
     * Initializes this position with the given line and column numbers
     *
     * @param line   The line number
     * @param column The column number
     */
    public TextPosition(int line, int column) {
        this.line = line;
        this.column = column;
    }

    @Override
    public String toString() {
        return "(" + line + ", " + column + ")";
    }
}
