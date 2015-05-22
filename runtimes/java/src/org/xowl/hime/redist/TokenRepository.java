/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/

package org.xowl.hime.redist;

import org.xowl.hime.redist.utils.BigList;

import java.util.Iterator;
import java.util.List;

/**
 * A repository of matched tokens
 *
 * @author Laurent Wouters
 */
public class TokenRepository implements Iterable<Token> {
    /**
     * Represents the metadata of a token
     */
    private static class Cell {
        /**
         * The terminal's index
         */
        public final int terminal;
        /**
         * The index in the input of this token
         */
        public final int inputIndex;
        /**
         * The length in the input of this token
         */
        public final int inputLength;

        /**
         * Initializes this cell
         *
         * @param terminal    The terminal's index
         * @param inputIndex  The index in the input of this token
         * @param inputLength The length in the input of this token
         */
        public Cell(int terminal, int inputIndex, int inputLength) {
            this.terminal = terminal;
            this.inputIndex = inputIndex;
            this.inputLength = inputLength;
        }
    }

    /**
     * Represents an iterator over all the tokens in this repository
     */
    private class LinearEnumerator implements Iterator<Token> {
        /**
         * The index of the current token
         */
        private int current;

        /**
         * Initializes this iterator
         */
        public LinearEnumerator() {
            current = 0;
        }

        @Override
        public boolean hasNext() {
            return (current < cells.size());
        }

        @Override
        public Token next() {
            Token result = null;
            if (current < cells.size()) {
                result = at(current);
                current++;
            }
            return result;
        }
    }

    /**
     * Gets the number of tokens in this repository
     *
     * @return the number of tokens in this repository
     */
    public int size() {
        return cells.size();
    }

    /**
     * Gets the token at the specified index
     *
     * @param index An index in this repository
     * @return The token at the specified index
     */
    public Token at(int index) {
        return new Token(this, index);
    }

    /**
     * The terminal symbols matched in this content
     */
    private final List<Symbol> terminals;
    /**
     * The base text
     */
    private final Text text;
    /**
     * The token data in this content
     */
    private final BigList<Cell> cells;

    /**
     * Initializes this repository
     *
     * @param terminals The terminal symbols
     * @param text      The base text
     */
    public TokenRepository(List<Symbol> terminals, Text text) {
        this.terminals = terminals;
        this.text = text;
        this.cells = new BigList<Cell>(Cell.class, Cell[].class);
    }

    /**
     * Gets the position in the input text of the given token
     *
     * @param index A token's index
     * @return The position in the text
     */
    protected TextPosition getPosition(int index) {
        return text.getPositionAt(cells.get(index).inputIndex);
    }

    /**
     * Gets the span in the input text of the given token
     *
     * @param index A token's index
     * @return The span in the text
     */
    protected TextSpan getSpan(int index) {
        Cell cell = cells.get(index);
        return new TextSpan(cell.inputIndex, cell.inputLength);
    }

    /**
     * Gets the context in the input of the given token
     *
     * @param index A token's index
     * @return The context
     */
    protected TextContext getContext(int index) {
        return text.getContext(getSpan(index));
    }

    /**
     * Gets the grammar symbol associated to the given token
     *
     * @param index A token's index
     * @return The associated symbol
     */
    protected Symbol getSymbol(int index) {
        return terminals.get(cells.get(index).terminal);
    }

    /**
     * Gets the value of the given token
     *
     * @param index A token's index
     * @return The associated value
     */
    protected String getValue(int index) {
        Cell cell = cells.get(index);
        return text.getValue(cell.inputIndex, cell.inputLength);
    }

    @Override
    public Iterator<Token> iterator() {
        return new LinearEnumerator();
    }

    /**
     * Registers a new token in this repository
     *
     * @param terminal The index of the matched terminal
     * @param index    The starting index of the matched value in the input
     * @param length   The length of the matched value in the input
     * @return The index of the added token
     */
    public int add(int terminal, int index, int length) {
        return cells.add(new Cell(terminal, index, length));
    }
}
