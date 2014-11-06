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
import org.xowl.hime.redist.TokenizedText;

import java.io.InputStreamReader;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Represents a base lexer
 */
public abstract class BaseLexer implements ILexer {
    /**
     * The handler of lexical error for this lexer
     */
    protected LexicalErrorHandler handler;
    /**
     * This lexer's automaton
     */
    protected Automaton automaton;
    /**
     * The terminals matched by this lexer
     */
    protected List<Symbol> recognizedTerminals;
    /**
     * Symbol ID of the SEPARATOR terminal
     */
    protected int separatorID;
    /**
     * The tokenized text
     */
    protected BaseTokenizedText text;

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
        this.recognizedTerminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.separatorID = separator;
        this.text = new PrefetchedText(this.recognizedTerminals, input);
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
        this.recognizedTerminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.separatorID = separator;
        this.text = new StreamingText(this.recognizedTerminals, input);
    }

    /**
     * Gets the terminals matched by this lexer
     *
     * @return The terminals matched by this lexer
     */
    public List<Symbol> getTerminals() {
        return recognizedTerminals;
    }

    /**
     * Gets the lexer's output as a tokenized text
     *
     * @return The lexer's output as a tokenized text
     */
    public TokenizedText getOutput() {
        return text;
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
     * Represents a match in the input
     */
    protected static class Match {
        /**
         * Index of the matched terminal
         */
        public int terminal;
        /**
         * Length of the matched input
         */
        public int length;

        /**
         * Initializes a match
         *
         * @param terminal Index of the matched terminal
         */
        public Match(int terminal) {
            this.terminal = terminal;
            this.length = 0;
        }
    }
}
