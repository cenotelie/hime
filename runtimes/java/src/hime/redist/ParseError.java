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
 * Represents an error in a parser
 */
public abstract class ParseError {
    /**
     * The error's type
     */
    protected ParseErrorType type;
    /**
     * The error's position in the input
     */
    protected TextPosition position;
    /**
     * The error's message
     */
    protected String message;

    /**
     * Gets the error's type
     *
     * @return The error's type
     */
    public ParseErrorType getType() {
        return type;
    }

    /**
     * Gets the error's position in the input
     *
     * @return The error's position in the input
     */
    public TextPosition getPosition() {
        return position;
    }

    /**
     * Gets the error's message
     *
     * @return The error's message
     */
    public String getMessage() {
        return message;
    }

    /**
     * Initializes a new instance of the ParserError
     *
     * @param type     Error's type
     * @param position Error's position in the input
     */
    protected ParseError(ParseErrorType type, TextPosition position) {
        this.type = type;
        this.position = position;
        this.message = "@" + position.toString() + " ";
    }

    @Override
    public String toString() {
        return message;
    }
}
