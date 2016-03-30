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
     * The cached matched terminal
     */
    private final MatchedTerminal terminalCache;
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
        this.terminalCache = new MatchedTerminal();
        this.isDollarEmitted = false;
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
        this.terminalCache = new MatchedTerminal();
        this.isDollarEmitted = false;
    }

    @Override
    public TokenKernel getNextToken(IContextProvider contexts) {
        if (isDollarEmitted)
            return new TokenKernel(Symbol.SID_EPSILON, -1);

        while (true) {
            TokenMatch match = runDFA(inputIndex);
            if (!match.isSuccess()) {
                // failed to match, retry with error handling
                match = runDFAOnError(inputIndex);
            }
            if (match.isSuccess()) {
                if (match.state == 0) {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    isDollarEmitted = true;
                    return new TokenKernel(Symbol.SID_DOLLAR, tokens.add(1, inputIndex, 0));
                } else {
                    // matched something
                    int tIndex = getTerminalFor(match.state, contexts);
                    int tID = symTerminals.get(tIndex).getID();
                    if (tID == separatorID) {
                        inputIndex += match.length;
                    } else {
                        TokenKernel token = new TokenKernel(tID, tokens.add(tIndex, inputIndex, match.length));
                        inputIndex += match.length;
                        return token;
                    }
                }
            } else {
                inputIndex += match.length;
            }
        }
    }

    /**
     * Gets the index of the terminal with the highest priority that is possible in the contexts
     *
     * @param state    The DFA state
     * @param provider The current applicable contexts
     * @return The index of the terminal
     */
    private int getTerminalFor(int state, IContextProvider provider) {
        // return the match with the highest priority that is possible in the contexts
        automaton.retrieveState(state, stateCache);
        stateCache.getTerminal(0, terminalCache);
        int id = symTerminals.get(terminalCache.getIndex()).getID();
        int currentResult = terminalCache.getIndex();
        if (id == separatorID)
            // the separator trumps all
            return currentResult;
        int currentPriority = provider.getContextPriority(terminalCache.getContext(), id);
        for (int j = 1; j != stateCache.getTerminalCount(); j++) {
            stateCache.getTerminal(j, terminalCache);
            id = symTerminals.get(terminalCache.getIndex()).getID();
            if (id == separatorID)
                // the separator trumps all
                return terminalCache.getIndex();
            int priority = provider.getContextPriority(terminalCache.getContext(), id);
            if (currentPriority < 0 || (priority >= 0 && priority < currentPriority)) {
                currentResult = terminalCache.getIndex();
                currentPriority = priority;
            }
        }
        return currentResult;
    }
}
