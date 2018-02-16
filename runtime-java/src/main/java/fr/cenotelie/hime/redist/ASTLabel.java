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
 * Represents a label an AST node
 *
 * @author Laurent Wouters
 */
class ASTLabel implements SemanticElement {
    /**
     * The symbol being referenced
     */
    private final Symbol symbol;
    /**
     * The type of this symbol
     */
    private final SymbolType type;

    @Override
    public SymbolType getSymbolType() {
        return type;
    }

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
     * @param type   The type of this symbol
     */
    public ASTLabel(Symbol symbol, SymbolType type) {
        this.symbol = symbol;
        this.type = type;
    }

    @Override
    public String toString() {
        return symbol.getName();
    }
}
