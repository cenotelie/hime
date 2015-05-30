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
public abstract class ContextSensitiveLexer extends BaseLexer {
    /**
     * The current index in the input
     */
    private int inputIndex;
    /**
     * The cached automaton state
     */
    private final AutomatonState stateCache;
    /**
     * The cached matched terminal
     */
    private final MatchedTerminal terminalCache;
    /**
     * Length of the matched input
     */
    private int matchedLength;
    /**
     * Whether the end-of-input dollar marker has already been emitted
     */
    private boolean isDollarEmitted;

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
        this.stateCache = new AutomatonState();
        this.terminalCache = new MatchedTerminal();
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
        this.stateCache = new AutomatonState();
        this.terminalCache = new MatchedTerminal();
    }

    @Override
    public TokenKernel getNextToken(IContextProvider contexts) {
        if (isDollarEmitted)
            return new TokenKernel(Symbol.SID_EPSILON, -1);

        while (true) {
            int matchedIndex = runDFA(contexts);
            if (matchedLength != 0) {
                // matched something!
                int terminalID = symTerminals.get(matchedIndex).getID();
                if (terminalID == separatorID) {
                    inputIndex += matchedLength;
                    continue;
                }
                TokenKernel token = new TokenKernel(terminalID, tokens.add(matchedIndex, inputIndex, matchedLength));
                inputIndex += matchedLength;
                return token;
            }
            if (matchedIndex == 1) {
                // This is the dollar terminal, at the end of the input
                isDollarEmitted = true;
                return new TokenKernel(Symbol.SID_DOLLAR, tokens.add(1, inputIndex, 0));
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
     * @return The index of the matched terminal
     */
    private int runDFA(IContextProvider contexts) {
        matchedLength = 0;
        if (text.isEnd(inputIndex)) {
            // At the end of input
            return 1; // 1 is always the index of the $ terminal
        }

        int matchingState = -1;
        int state = 0;
        int i = inputIndex;

        while (state != Automaton.DEAD_STATE) {
            automaton.retrieveState(state, stateCache);
            // Is this state a matching state ?
            if (stateCache.getTerminalCount() != 0) {
                matchingState = state;
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

        if (matchingState == -1)
            // no match
            return 0;

        automaton.retrieveState(matchingState, stateCache);
        // look for a perfect match
        for (int j = 0; j != stateCache.getTerminalCount(); j++) {
            stateCache.getTerminal(j, terminalCache);
            int id = symTerminals.get(terminalCache.getIndex()).getID();
            if (id == separatorID || (contexts.isExpected(id) && contexts.isInContext(terminalCache.getContext(), id)))
                return terminalCache.getIndex();
        }
        // look for the correct context
        for (int j = 0; j != stateCache.getTerminalCount(); j++) {
            stateCache.getTerminal(j, terminalCache);
            int id = symTerminals.get(terminalCache.getIndex()).getID();
            if (terminalCache.getContext() == Automaton.DEFAULT_CONTEXT || contexts.isInContext(terminalCache.getContext(), id))
                return terminalCache.getIndex();
        }
        // nope, just return the first match
        return stateCache.getTerminal();
    }
}
