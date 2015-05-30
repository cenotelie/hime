/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters
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

package org.xowl.hime.redist.lexer;

import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.Text;
import org.xowl.hime.redist.Token;
import org.xowl.hime.redist.TokenRepository;

import java.io.InputStreamReader;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Represents a base lexer
 *
 * @author Laurent Wouters
 */
public abstract class BaseLexer {
    /**
     * The handler of lexical error for this lexer
     */
    LexicalErrorHandler handler;
    /**
     * This lexer's automaton
     */
    final Automaton automaton;
    /**
     * The terminals matched by this lexer
     */
    final List<Symbol> symTerminals;
    /**
     * Symbol ID of the SEPARATOR terminal
     */
    final int separatorID;
    /**
     * The input text
     */
    final BaseText text;
    /**
     * The token repository
     */
    final TokenRepository tokens;

    /**
     * Gets the terminals matched by this lexer
     *
     * @return The terminals matched by this lexer
     */
    public List<Symbol> getTerminals() {
        return symTerminals;
    }

    /**
     * Gets the lexer's input text
     *
     * @return The lexer's input text
     */
    public Text getInput() {
        return text;
    }

    /**
     * Gets the lexer's output stream of tokens
     *
     * @return The lexer's output stream of tokens
     */
    public Iterable<Token> getOutput() {
        return tokens;
    }

    /**
     * Gets the token repository for this lexer
     *
     * @return The token repository for this lexer
     */
    public TokenRepository getTokens() {
        return tokens;
    }

    /**
     * Sets the handler of lexical errors coming from this parser
     *
     * @param handler The handler
     */
    public void setErrorHandler(LexicalErrorHandler handler) {
        this.handler = handler;
    }

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected BaseLexer(Automaton automaton, Symbol[] terminals, int separator, String input) {
        this.automaton = automaton;
        this.symTerminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.separatorID = separator;
        this.text = new PrefetchedText(input);
        this.tokens = new TokenRepository(symTerminals, text);
    }

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected BaseLexer(Automaton automaton, Symbol[] terminals, int separator, InputStreamReader input) {
        this.automaton = automaton;
        this.symTerminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.separatorID = separator;
        this.text = new StreamingText(input);
        this.tokens = new TokenRepository(symTerminals, text);
    }

    /**
     * Gets the next token in the input
     *
     * @return The next token in the input
     */
    public Token getNextToken() {
        return tokens.at(getNextToken(null).getIndex());
    }

    /**
     * Gets the next token in the input
     *
     * @param contexts The current applicable contexts
     * @return The next token in the input
     */
    public abstract TokenKernel getNextToken(IContextProvider contexts);
}
