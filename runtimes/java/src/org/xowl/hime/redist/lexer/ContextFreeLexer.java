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
import org.xowl.hime.redist.TextPosition;
import org.xowl.hime.redist.Token;
import org.xowl.hime.redist.UnexpectedCharError;

import java.io.InputStreamReader;

/**
 * Represents a context-free lexer (lexing rules do not depend on the context)
 */
public abstract class ContextFreeLexer extends BaseLexer {
    /**
     * The index of the next token
     */
    private int tokenIndex;
    /**
     * The cache of DFA state data
     */
    private State stateData;
    /**
     * The cache of matched terminal
     */
    private MatchedTerminal matchedTerminal;

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected ContextFreeLexer(Automaton automaton, Symbol[] terminals, int separator, String input) {
        super(automaton, terminals, separator, input);
        this.tokenIndex = -1;
        this.stateData = new State();
        this.matchedTerminal = new MatchedTerminal();
    }

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected ContextFreeLexer(Automaton automaton, Symbol[] terminals, int separator, InputStreamReader input) {
        super(automaton, terminals, separator, input);
        this.tokenIndex = -1;
        this.stateData = new State();
        this.matchedTerminal = new MatchedTerminal();
    }

    /**
     * Gets the next token in the input
     * This forces the use of the default context
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
            return new Token(Symbol.SID_EPSILON, 0);
        return text.getTokenAt(tokenIndex++);
    }

    /**
     * Gets the next token in the input
     *
     * @param contexts The current applicable contexts
     * @return The next token in the input
     */
    public Token getNextToken(ContextStack contexts) {
        return getNextToken();
    }

    /**
     * Finds all the tokens in the lexer's input
     */
    private void findTokens() {
        while (true) {
            Match match = runDFA();
            if (match.length != 0) {
                if (recognizedTerminals.get(match.terminal).getID() != separatorID)
                    text.addToken(match.terminal, inputIndex, match.length);
                inputIndex += match.length;
                continue;
            }
            if (match.terminal == 0) {
                // This is the EPSILON terminal, failed to match anything
                TextPosition position = text.getPositionAt(inputIndex);
                String unexpected = null;
                int c = text.getValue(inputIndex);
                if (c >= 0xD800 && c <= 0xDFFF) {
                    // this is a surrogate encoding point
                    unexpected = text.getValue(inputIndex, inputIndex + 2);
                    inputIndex += 2;
                } else {
                    unexpected = text.getValue(inputIndex, inputIndex + 1);
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
        if (text.isEnd(inputIndex)) {
            // At the end of input
            return new Match(1); // 1 is always the index of the $ terminal
        }

        Match result = new Match(0);
        int state = 0;
        int i = inputIndex;

        while (state != Automaton.DEAD_STATE) {
            automaton.retrieveState(state, stateData);
            // Is this state a matching state ?
            if (stateData.getTerminalsCount() != 0) {
                stateData.retrieveTerminal(0, matchedTerminal);
                result.terminal = matchedTerminal.getIndex();
                result.length = (i - inputIndex);
            }
            // No further transition => exit
            if (stateData.isDeadEnd())
                break;
            // At the end of the buffer
            if (text.isEnd(i))
                break;
            char current = text.getValue(i);
            i++;
            // Try to find a transition from this state with the read character
            state = stateData.getTargetBy(current);
        }
        return result;
    }
}
