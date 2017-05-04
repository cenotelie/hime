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
 * Represents a reference to a symbol
 *
 * @author Laurent Wouters
 */
class SymbolRef implements SemanticElement {
    /**
     * The symbol being referenced
     */
    private final Symbol symbol;

    @Override
    public TextPosition getPosition() {
        return new TextPosition(0, 0);
    }

    @Override
    public TextSpan getSpan() {
        return new TextSpan(0, 0);
    }

    @Override
    public TextContext getContext() {
        return new TextContext();
    }

    @Override
    public Symbol getSymbol() {
        return symbol;
    }

    @Override
    public String getValue() {
        return null;
    }

    /**
     * Initializes this reference
     *
     * @param symbol The symbol being referenced
     */
    public SymbolRef(Symbol symbol) {
        this.symbol = symbol;
    }

    @Override
    public String toString() {
        return symbol.getName();
    }
}
