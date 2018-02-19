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
 * Represents a token as an output element of a lexer
 *
 * @author Laurent Wouters
 */
public class Token implements SemanticElement {
    /**
     * The repository containing this token
     */
    private final TokenRepository repository;
    /**
     * The index of this token in the text
     */
    final int index;

    /**
     * Initializes this token
     *
     * @param repository The repository containing the token
     * @param index      The token's index
     */
    public Token(TokenRepository repository, int index) {
        this.repository = repository;
        this.index = index;
    }

    @Override
    public SymbolType getSymbolType() {
        return SymbolType.Terminal;
    }

    @Override
    public TextPosition getPosition() {
        return repository.getPosition(index);
    }

    @Override
    public TextSpan getSpan() {
        return repository.getSpan(index);
    }

    @Override
    public TextContext getContext() {
        return repository.getContext(index);
    }

    @Override
    public Symbol getSymbol() {
        return repository.getSymbol(index);
    }

    @Override
    public String getValue() {
        return repository.getValue(index);
    }

    @Override
    public String toString() {
        String name = repository.getSymbol(index).getName();
        String value = repository.getValue(index);
        return name + " = " + value;
    }
}
