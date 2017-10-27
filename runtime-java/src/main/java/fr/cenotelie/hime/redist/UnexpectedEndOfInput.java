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
 * Represents the unexpected of the input text while more characters were expected
 *
 * @author Laurent Wouters
 */
public class UnexpectedEndOfInput extends ParseError {
    @Override
    public ParseErrorType getType() {
        return ParseErrorType.UnexpectedEndOfInput;
    }

    @Override
    public int getLength() {
        return 0;
    }

    @Override
    public String getMessage() {
        return "Unexpected end of input";
    }

    /**
     * Initializes this error
     *
     * @param position Error's position in the input
     */
    public UnexpectedEndOfInput(TextPosition position) {
        super(position);
    }
}
