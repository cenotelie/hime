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
 * Represents an error in a parser
 *
 * @author Laurent Wouters
 */
public abstract class ParseError {
    /**
     * The error's position in the input text
     */
    private final TextPosition position;

    /**
     * Gets the error's type
     *
     * @return The error's type
     */
    public abstract ParseErrorType getType();

    /**
     * Gets the error's position in the input
     *
     * @return The error's position in the input
     */
    public TextPosition getPosition() {
        return position;
    }

    /**
     * Gets the error's length in the input (in number of characters)
     *
     * @return The error's length in the input (in number of characters)
     */
    public abstract int getLength();

    /**
     * Gets the error's message
     *
     * @return The error's message
     */
    public abstract String getMessage();

    /**
     * Initializes this error
     *
     * @param position Error's position in the input
     */
    ParseError(TextPosition position) {
        this.position = position;
    }

    @Override
    public String toString() {
        return "@" + position.toString() + " " + getMessage();
    }
}
