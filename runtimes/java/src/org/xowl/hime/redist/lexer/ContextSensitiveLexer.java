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
public abstract class ContextSensitiveLexer extends BaseLexer {
    /**
     * The current index in the input
     */
    protected int inputIndex;
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
    protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, String input) {
        super(automaton, terminals, separator, input);
        this.inputIndex = 0;
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
    protected ContextSensitiveLexer(Automaton automaton, Symbol[] terminals, int separator, InputStreamReader input) {
        super(automaton, terminals, separator, input);
        this.inputIndex = 0;
        this.stateData = new State();
        this.matchedTerminal = new MatchedTerminal();
    }

    /**
     * Gets the next token in the input
     *
     * @param contexts The current applicable contexts
     * @return The next token in the input
     */
    public Token getNextToken(IContextProvider contexts) {
        while (true) {
            Match match = runDFA(contexts);
            if (match.length != 0) {
                int id = recognizedTerminals.get(match.terminal).getID();
                if (id == separatorID)
                    continue;
                Token token = new Token(id, text.addToken(match.terminal, inputIndex, match.length));
                inputIndex += match.length;
                return token;
            }
            if (match.terminal == Symbol.SID_NOTHING) {
                // This is the EPSILON terminal, failed to match anything
                TextPosition position = text.getPositionAt(inputIndex);
                String unexpected = null;
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
            return new Token(Symbol.SID_DOLLAR, text.addToken(match.terminal, inputIndex, match.length));
        }
    }

    /**
     * Rewinds this lexer for a specified amount of tokens
     *
     * @param count The number of tokens to rewind
     */
    public void rewindTokens(int count) {
        inputIndex = text.dropTokens(count);
    }

    /**
     * Runs the lexer's DFA to match a terminal in the input ahead
     *
     * @param contexts The current applicable contexts
     * @return The matched terminal and length
     */
    private Match runDFA(IContextProvider contexts) {
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
            for (int j = 0; j != stateData.getTerminalsCount(); j++) {
                stateData.retrieveTerminal(j, matchedTerminal);
                if (contexts.isWithin(matchedTerminal.getContext())) {
                    result.terminal = matchedTerminal.getIndex();
                    result.length = (i - inputIndex);
                    break;
                }
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
