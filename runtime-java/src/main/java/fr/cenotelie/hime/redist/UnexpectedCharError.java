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
 * Represents an unexpected character error in the input stream of a lexer
 *
 * @author Laurent Wouters
 */
public class UnexpectedCharError extends ParseError {
    /**
     * The unexpected char
     */
    private final String unexpected;

    /**
     * Gets the error's type
     *
     * @return The error's type
     */
    @Override
    public ParseErrorType getType() {
        return ParseErrorType.UnexpectedChar;
    }

    @Override
    public int getLength() {
        return unexpected.length();
    }

    @Override
    public String getMessage() {
        return buildMessage();
    }

    /**
     * Gets the unexpected char
     *
     * @return The unexpected char
     */
    public String getUnexpectedChar() {
        return unexpected;
    }

    /**
     * Initializes this error
     *
     * @param unexpected The errorneous character (as a string)
     * @param position   Error's position in the input
     */
    public UnexpectedCharError(String unexpected, TextPosition position) {
        super(position);
        this.unexpected = unexpected;
    }

    /**
     * Builds the message for this error
     *
     * @return The message for this error
     */
    private String buildMessage() {
        StringBuilder builder = new StringBuilder("Unexpected character '");
        builder.append(unexpected);
        builder.append("' (U+");
        if (unexpected.length() == 1) {
            builder.append(Integer.toHexString(unexpected.charAt(0)));
        } else {
            int lead = unexpected.charAt(0);
            int trail = unexpected.charAt(1);
            int cp = ((trail - 0xDC00) | ((lead - 0xD800) << 10)) + 0x10000;
            builder.append(Integer.toHexString(cp));
        }
        builder.append(")");
        return builder.toString();
    }
}
