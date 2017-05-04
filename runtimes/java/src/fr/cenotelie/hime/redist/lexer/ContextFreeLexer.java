/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

package fr.cenotelie.hime.redist.lexer;

import fr.cenotelie.hime.redist.Symbol;

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
            TokenMatch match = runDFA(inputIndex);
            if (!match.isSuccess()) {
                // failed to match, retry with error handling
                match = runDFAOnError(inputIndex);
            }
            if (match.isSuccess()) {
                if (match.state == 0) {
                    // this is the dollar terminal, at the end of the input
                    // the index of the $ symbol is always 1
                    tokens.add(1, inputIndex, 0);
                    return;
                } else {
                    // matched something
                    automaton.retrieveState(match.state, stateCache);
                    int tIndex = stateCache.getTerminal();
                    if (symTerminals.get(tIndex).getID() != separatorID)
                        tokens.add(tIndex, inputIndex, match.length);
                    inputIndex += match.length;
                }
            } else {
                inputIndex += match.length;
            }
        }
    }
}
