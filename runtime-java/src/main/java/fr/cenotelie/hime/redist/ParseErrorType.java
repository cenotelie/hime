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
 * Specifies the type of error
 *
 * @author Laurent Wouters
 */
public enum ParseErrorType {
    /**
     * Lexical error occurring when the end of input has been encountered while more characters were expected
     */
    UnexpectedEndOfInput,
    /**
     * Lexical error occurring when an unexpected character is encountered in the input preventing to match tokens
     */
    UnexpectedChar,
    /**
     * Syntactic error occurring when an unexpected token is encountered by the parser
     */
    UnexpectedToken,
    /**
     * Lexical error occurring when the low surrogate encoding point is missing in a UTF-16 encoding sequence with an expected high and low surrogate pair
     */
    IncorrectUTF16NoLowSurrogate,
    /**
     * Lexical error occurring when the high surrogate encoding point is missing in a UTF-16 encoding sequence with an expected high and low surrogate pair
     */
    IncorrectUTF16NoHighSurrogate
}
