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

package hime.redist.lexer;

import hime.redist.*;

import java.io.DataInput;
import java.io.IOException;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

/**
 * Represents a lexer for a prefetched piece of text, i.e. the text is already in memory
 */
public abstract class PrefetchedLexer implements ILexer {
    /**
     * The handler of lexical error for this lexer
     */
    private LexicalErrorHandler handler;
    /**
     * This lexer's automaton
     */
    private Automaton lexAutomaton;
    /**
     * The terminals matched by this lexer
     */
    private List<Symbol> terminals;
    /**
     * Symbol ID of the SEPARATOR terminal
     */
    private int lexSeparator;
    /**
     * The lexer's full input
     */
    private String input;
    /**
     * The tokenized text
     */
    private PrefetchedText text;
    /**
     * The current index in the input
     */
    private int inputIndex;
    /**
     * The index of the next token
     */
    private int tokenIndex;

    /**
     * Gets the terminals matched by this lexer
     *
     * @return The terminals matched by this lexer
     */
    public List<Symbol> getTerminals() {
        return terminals;
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
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected PrefetchedLexer(Automaton automaton, Symbol[] terminals, int separator, String input) {
        this.lexAutomaton = automaton;
        this.terminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.lexSeparator = separator;
        this.input = input;
        this.text = new PrefetchedText(this.terminals, this.input);
        this.inputIndex = 0;
        this.tokenIndex = -1;
    }

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected PrefetchedLexer(Automaton automaton, Symbol[] terminals, int separator, DataInput input) throws IOException {
        this(automaton, terminals, separator, input.readUTF());
    }

    /**
     * Gets the next token in the input
     *
     * @return The next token in the input
     */
    public Token getNextToken() {
        if (tokenIndex == -1) {
            // this is the first call to this method, prefetch the tokens
            findTokens();
            tokenIndex = 0;
        }
        // no more tokens? return epsilon
        if (tokenIndex >= text.getTokenCount())
            return new Token(Symbol.sidEpsilon, 0);
        return text.getTokenAt(tokenIndex++);
    }

    /**
     * Represents a match in the input
     */
    private class Match {
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

    /**
     * Finds all the tokens in the lexer's input
     */
    private void findTokens() {
        while (true) {
            Match match = runDFA();
            if (match.length != 0) {
                if (terminals.get(match.terminal).getID() != lexSeparator)
                    text.addToken(match.terminal, inputIndex, match.length);
                inputIndex += match.length;
                continue;
            }
            if (match.terminal == 0) {
                // This is the epsilon terminal, failed to match anything
                TextPosition position = text.getPositionAt(inputIndex);
                String unexpected = null;
                int c = input.charAt(inputIndex);
                if (c >= 0xD800 && c <= 0xDFFF) {
                    // this is a surrogate encoding point
                    unexpected = input.substring(inputIndex, inputIndex + 2);
                    inputIndex += 2;
                } else {
                    unexpected = input.substring(inputIndex, inputIndex + 1);
                    inputIndex++;
                }
                handler.handle(new UnexpectedCharError(unexpected, position));
                continue;
            }
            // This is the dollar terminal, at the end of the input
            text.addToken(match.terminal, inputIndex, match.length);
            return;
        }
    }

    /**
     * Runs the lexer's DFA to match a terminal in the input ahead
     *
     * @return The matched terminal and length
     */
    private Match runDFA() {
        if (inputIndex == input.length()) {
            // At the end of input
            return new Match(1);
        }

        Match result = new Match(0);
        int state = 0;
        int i = inputIndex;

        while (state != 0xFFFF) {
            int offset = lexAutomaton.getOffsetOf(state);
            // Is this state a matching state ?
            int terminal = lexAutomaton.getStateRecognizedTerminal(offset);
            if (terminal != 0xFFFF) {
                result.terminal = terminal;
                result.length = (i - inputIndex);
            }
            // No further transition => exit
            if (lexAutomaton.isStateDeadEnd(offset))
                break;
            // At the end of the buffer
            if (i == input.length())
                break;
            char current = input.charAt(i++);
            // Try to find a transition from this state with the read character
            if (current <= 255)
                state = lexAutomaton.getStateCachedTransition(offset, current);
            else
                state = lexAutomaton.getStateBulkTransition(offset, current);
        }
        return result;
    }
}
