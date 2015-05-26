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

package org.xowl.hime.redist.lexer;

import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.TextPosition;
import org.xowl.hime.redist.UnexpectedCharError;

import java.io.InputStreamReader;
import java.util.Arrays;

/**
 * Represents a context-free lexer (lexing rules do not depend on the context)
 *
 * @author Laurent Wouters
 */
public abstract class ContextSensitiveLexer extends BaseLexer {
    /**
     * The current index in the input
     */
    protected int inputIndex;
    /**
     * The buffer for token kernels
     */
    private final TokenKernelBuffer buffer;
    /**
     * The cache of DFA state data
     */
    private final AutomatonState stateCache;
    /**
     * The cached of matched terminal data
     */
    private final MatchedTerminal matchedTerminal;
    /**
     * The buffer of matches
     */
    private int[] matches;
    /**
     * The number of matches
     */
    private int matchesCount;

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, String input) {
        super(automaton, terminals, separator, input);
        this.inputIndex = 0;
        this.buffer = new TokenKernelBuffer(5);
        this.stateCache = new AutomatonState();
        this.matchedTerminal = new MatchedTerminal();
        this.matches = new int[5];
    }

    /**
     * Initializes a new instance of the Lexer class with the given input
     *
     * @param automaton DFA automaton for this lexer
     * @param terminals Terminals recognized by this lexer
     * @param separator SID of the separator token
     * @param input     Input to this lexer
     */
    protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, InputStreamReader input) {
        super(automaton, terminals, separator, input);
        this.inputIndex = 0;
        this.buffer = new TokenKernelBuffer(5);
        this.stateCache = new AutomatonState();
        this.matchedTerminal = new MatchedTerminal();
        this.matches = new int[5];
    }

    @Override
    public TokenKernel getNextToken(IContextProvider contexts) {
        while (true) {
            int length = runDFA(contexts);
            if (length != 0) {
                // matched something!
                int terminalIndex = matches[0];
                int terminalID = symTerminals.get(terminalIndex).getID();
                if (terminalID == separatorID)
                    continue;
                TokenKernel token = new TokenKernel(terminalID, tokens.add(terminalIndex, inputIndex, length));
                inputIndex += length;
                return token;
            }
            if (matchesCount > 0) {
                // This is the dollar terminal, at the end of the input
                return new TokenKernel(Symbol.SID_DOLLAR, tokens.add(matches[0], inputIndex, 0));
            }
            // Failed to match anything
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
        }
    }

    @Override
    public TokenKernelBuffer getNextTokens(IContextProvider contexts) {
        while (true) {
            int length = runDFA(contexts);
            if (length != 0) {
                // matched something!
                buffer.reset();
                for (int i = 0; i != matchesCount; i++) {
                    int terminalIndex = matches[i];
                    int terminalID = symTerminals.get(terminalIndex).getID();
                    if (terminalID == separatorID)
                        // filter out the separators
                        continue;
                    buffer.add(new TokenKernel(terminalID, tokens.add(terminalIndex, inputIndex, length)));
                }
                inputIndex += length;
                if (buffer.size() > 0)
                    return buffer;
                continue;
            }
            if (matchesCount > 0) {
                // This is the dollar terminal, at the end of the input
                buffer.reset();
                buffer.add(new TokenKernel(Symbol.SID_DOLLAR, tokens.add(matches[0], inputIndex, 0)));
                return buffer;
            }
            // Failed to match anything
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
        }
    }


    /**
     * Runs the lexer's DFA to match a terminal in the input ahead
     *
     * @param contexts The current applicable contexts
     * @return The length of the matches
     */
    private int runDFA(IContextProvider contexts) {
        matchesCount = 0;

        if (text.isEnd(inputIndex)) {
            // At the end of input
            matches[0] = 1; // 1 is always the index of the $ terminal
            return 0;
        }

        int state = 0;
        int i = inputIndex;
        int length = 0;

        while (state != Automaton.DEAD_STATE) {
            automaton.retrieveState(state, stateCache);
            // Is this state a matching state ?
            boolean firstMatch = true;
            for (int j = 0; j != stateCache.getTerminalCount(); j++) {
                stateCache.getTerminal(j, matchedTerminal);
                if (contexts.isAcceptable(matchedTerminal.getContext(), matchedTerminal.getIndex())) {
                    if (firstMatch) {
                        // this is the first match in this state, the longest input
                        matchesCount = 0;
                        length = i - inputIndex;
                        firstMatch = false;
                    }
                    if (matchesCount >= matches.length)
                        matches = Arrays.copyOf(matches, matches.length * 2);
                    matches[matchesCount] = matchedTerminal.getIndex();
                    matchesCount++;
                }
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
        return length;
    }
}
