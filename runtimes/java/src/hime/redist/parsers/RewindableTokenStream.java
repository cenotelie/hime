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
package hime.redist.parsers;

import hime.redist.Token;
import hime.redist.lexer.ILexer;

/**
 * Fast rewindable stream of token encapsulating a lexer
 */
class RewindableTokenStream {
    /**
     * The size of the ring buffer
     */
    private static final int ringSize = 32;

    /**
     * The input lexer to read token from
     */
    private ILexer lexer;
    /**
     * The ring buffer
     */
    private Token[] ring;
    /**
     * Start index of the ring in case the stream in rewound
     */
    private int ringStart;
    /**
     * Index for inserting new characters in the ring
     */
    private int ringNextEntry;

    /**
     * Initializes the rewindable stream with the given lexer
     *
     * @param lexer The encapsulated lexer
     */
    public RewindableTokenStream(ILexer lexer) {
        this.lexer = lexer;
        this.ring = new Token[ringSize];
        this.ringStart = 0;
        this.ringNextEntry = 0;
    }

    /**
     * Determines whether the ring buffer is empty
     *
     * @return true if the ring is empty; otherwise, false
     */
    private boolean isRingEmpty() {
        return (ringStart == ringNextEntry);
    }

    /**
     * Reads a token from the ring
     *
     * @return The next token in the ring buffer
     */
    private Token readRing() {
        Token token = ring[ringStart++];
        if (ringStart == ring.length)
            ringStart = 0;
        return token;
    }

    /**
     * Pushes the given token onto the ring
     *
     * @param value The token to push
     */
    private void pushInRing(Token value) {
        ring[ringNextEntry++] = value;
        if (ringNextEntry == ringSize)
            ringNextEntry = 0;
        ringStart = ringNextEntry;
    }

    /**
     * Goes back in the stream
     *
     * @param count Number of tokens to rewind
     */
    public void rewind(int count) {
        ringStart -= count;
        if (ringStart < 0)
            ringStart += ringSize;
    }

    /**
     * Gets the next token in the stream
     *
     * @return The next token
     */
    public Token getNextToken() {
        if (!isRingEmpty())
            return readRing();
        Token value = lexer.getNextToken();
        pushInRing(value);
        return value;
    }
}
