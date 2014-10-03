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

package org.xowl.hime.redist.lexer;

import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.Token;
import org.xowl.hime.redist.TokenizedText;

import java.util.List;

/**
 * Represents a lexer for a text stream
 */
public interface ILexer {
    /**
     * Gets the terminals matched by this lexer
     *
     * @return The terminals matched by this lexer
     */
    List<Symbol> getTerminals();

    /**
     * Gets the lexer's output as a tokenized text
     *
     * @return The lexer's output as a tokenized text
     */
    TokenizedText getOutput();

    /**
     * Sets the handler of lexical errors coming from this parser
     *
     * @param handler The handler
     */
    void setErrorHandler(LexicalErrorHandler handler);

    /**
     * Gets the next token in the input
     *
     * @param contexts The current applicable contexts
     * @return The next token in the input
     */
    Token getNextToken(IContextProvider contexts);

    /**
     * Rewinds this lexer for a specified amount of tokens
     *
     * @param count The number of tokens to rewind
     */
    void rewindTokens(int count);
}
