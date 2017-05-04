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

import java.util.Collections;
import java.util.List;

/**
 * Represents an unexpected token error in a parser
 *
 * @author Laurent Wouters
 */
public class UnexpectedTokenError extends ParseError {
    /**
     * The unexpected token
     */
    private final Token unexpected;
    /**
     * The expected terminals
     */
    private final List<Symbol> expected;

    /**
     * Gets the error's type
     *
     * @return The error's type
     */
    @Override
    public ParseErrorType getType() {
        return ParseErrorType.UnexpectedToken;
    }

    @Override
    public int getLength() {
        return unexpected.getSpan().getLength();
    }

    @Override
    public String getMessage() {
        return buildMessage();
    }

    /**
     * Gets the unexpected token
     *
     * @return The unexpected token
     */
    public Token getUnexpectedToken() {
        return unexpected;
    }

    /**
     * Gets a list of the expected terminals
     *
     * @return A list of the expected terminals
     */
    public List<Symbol> getExpectedTerminals() {
        return expected;
    }

    /**
     * Initializes this error
     *
     * @param token    The unexpected token
     * @param expected The expected terminals
     */
    public UnexpectedTokenError(Token token, List<Symbol> expected) {
        super(token.getPosition());
        this.unexpected = token;
        this.expected = Collections.unmodifiableList(expected);
    }

    /**
     * Builds the message for this error
     *
     * @return The message for this error
     */
    private String buildMessage() {
        StringBuilder builder = new StringBuilder("Unexpected token \"");
        builder.append(unexpected.getValue());
        builder.append("\"");
        if (!expected.isEmpty()) {
            builder.append("; expected: ");
            for (int i = 0; i != expected.size(); i++) {
                if (i != 0)
                    builder.append(", ");
                builder.append(expected.get(i).getName());
            }
        }
        return builder.toString();
    }
}
