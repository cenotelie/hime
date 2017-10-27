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
 * Represents an element of parsing data
 *
 * @author Laurent Wouters
 */
public interface SemanticElement {
    /**
     * Gets the type of symbol this element represents
     *
     * @return The type of symbol this element represents
     */
    SymbolType getSymbolType();

    /**
     * Gets the position in the input text of this element
     *
     * @return The position in the input text of this element
     */
    TextPosition getPosition();

    /**
     * Gets the span in the input text of this element
     *
     * @return The span in the input text of this element
     */
    TextSpan getSpan();

    /**
     * Gets the context of this element in the input
     *
     * @return The context of this element in the input
     */
    TextContext getContext();

    /**
     * Gets the grammar symbol associated to this element
     *
     * @return The grammar symbol associated to this element
     */
    Symbol getSymbol();

    /**
     * Gets the value of this element, if any
     *
     * @return The value of this element, if any
     */
    String getValue();
}
