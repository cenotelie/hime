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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.ParseResult;
import org.xowl.hime.redist.Symbol;

import java.util.List;

/**
 * Represents a parser
 */
public interface IParser {
    /**
     * Gets the variable symbols used by this parser
     *
     * @return The variable symbols used by this parser
     */
    List<Symbol> getVariables();

    /**
     * Gets the virtual symbols used by this parser
     *
     * @return The virtual symbols used by this parser
     */
    List<Symbol> getVirtuals();

    /**
     * Gets whether the parser should try to recover from errors
     *
     * @return Whether the parser should try to recover from errors
     */
    boolean getRecover();

    /**
     * Sets whether the parser should try to recover from errors
     *
     * @param recover Whether the parser should try to recover from errors
     */
    void setRecover(boolean recover);

    /**
     * Gets whether this parser is in debug mode
     *
     * @return Whether this parser is in debug mode
     */
    boolean isDebugMode();

    /**
     * Sets whether this parser is in debug mode
     *
     * @param mode Whether this parser is in debug mode
     */
    void setDebugMode(boolean mode);

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    ParseResult parse();
}
