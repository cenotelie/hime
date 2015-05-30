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
import org.xowl.hime.redist.TextPosition;
import org.xowl.hime.redist.UnexpectedCharError;

import java.io.InputStreamReader;

/**
 * Represents a context-free lexer (lexing rules do not depend on the context)
 *
 * @author Laurent Wouters
 */
public abstract class ContextFreeLexer extends BaseLexer {
    /**
     * The index of the next token
     */
    private int tokenIndex;
    /**
     * The cached automaton state
     */
    private final AutomatonState stateCache;
    /**
     * Length of the matched input
     */
    private int matchedLength;

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
        this.stateCache = new AutomatonState();
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
        this.stateCache = new AutomatonState();
    }

    @Override
    public TokenKernel getNextToken(IContextProvider contexts) {
        if (tokens.size() == 0) {
            // this is the first call to this method, prefetch the tokens
            findTokens();
            tokenIndex = 0;
        }
        // no more tokens? return epsilon
        if (tokenIndex >= tokens.size())
            return new TokenKernel(Symbol.SID_EPSILON, 0);
        TokenKernel result = new TokenKernel(tokens.getSymbol(tokenIndex).getID(), tokenIndex);
        tokenIndex++;
        return result;
    }

    /**
     * Finds all the tokens in the lexer's input
     */
    private void findTokens() {
        int inputIndex = 0;
        while (true) {
            int matchedIndex = runDFA(inputIndex);
            if (matchedLength != 0) {
                if (symTerminals.get(matchedIndex).getID() != separatorID)
                    tokens.add(matchedIndex, inputIndex, matchedLength);
                inputIndex += matchedLength;
                continue;
            }
            if (matchedIndex == 0) {
                // This is the EPSILON terminal, failed to match anything
                TextPosition position = text.getPositionAt(inputIndex);
                String unexpected;
                int c = text.getValue(inputIndex);
                if (c >= 0xD800 && c <= 0xDFFF) {
                    // this is a surrogate encoding point
                    unexpected = text.getValue(inputIndex, inputIndex + 2);
                    inputIndex += 2;
                } else {
                    unexpected = Character.toString(text.getValue(inputIndex));
                    inputIndex++;
                }
                handler.handle(new UnexpectedCharError(unexpected, position));
                continue;
            }
            // This is the dollar terminal, at the end of the input
            tokens.add(matchedIndex, inputIndex, matchedLength);
            return;
        }
    }

    /**
     * Runs the lexer's DFA to match a terminal in the input ahead
     *
     * @param inputIndex The current start index in the input text
     * @return The index of the matched terminal
     */
    private int runDFA(int inputIndex) {
        matchedLength = 0;
        if (text.isEnd(inputIndex)) {
            // At the end of input
            return 1; // 1 is always the index of the $ terminal
        }

        int result = 0;
        int state = 0;
        int i = inputIndex;

        while (state != Automaton.DEAD_STATE) {
            automaton.retrieveState(state, stateCache);
            // Is this state a matching state ?
            if (stateCache.getTerminalCount() != 0) {
                result = stateCache.getTerminal();
                matchedLength = (i - inputIndex);
            }
            // No further transition => exit
            if (stateCache.isDeadEnd())
                break;
            // At the end of the buffer
            if (text.isEnd(i))
                break;
            char current = text.getValue(i);
            i++;
            // Try to find a transition from this state with the read character
            state = stateCache.getTargetBy(current);
        }
        return result;
    }
}
