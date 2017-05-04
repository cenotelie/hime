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

import fr.cenotelie.hime.redist.*;

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
     * The default maximum Levenshtein distance to go to for the recovery of a matching failure
     */
    protected static final int DEFAULT_RECOVERY_MATCHING_DISTANCE = 3;

    /**
     * The default context provider
     */
    private static class DefaultContextProvider implements IContextProvider {
        @Override
        public int getContextPriority(int context, int onTerminalID) {
            return context == Automaton.DEFAULT_CONTEXT ? Integer.MAX_VALUE : 0;
        }
    }

    /**
     * The default context provider
     */
    private static final DefaultContextProvider DEFAULT_CONTEXT_PROVIDER = new DefaultContextProvider();

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
     * The maximum Levenshtein distance to go to for the recovery of a matching failure.
     */
    int recoveryDistance;
    /**
     * The cached automaton state
     */
    final AutomatonState stateCache;

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
     * Gets the maximum Levenshtein distance to go to for the recovery of a matching failure.
     * A distance of 0 indicates no recovery.
     *
     * @return The maximum Levenshtein distance to go to for the recovery of a matching failure
     */
    public int getRecoveryDistance() {
        return recoveryDistance;
    }

    /**
     * Sets the maximum Levenshtein distance to go to for the recovery of a matching failure.
     * A distance of 0 indicates no recovery.
     *
     * @param distance The maximum Levenshtein distance to go to for the recovery of a matching failure
     */
    public void setRecoveryDistance(int distance) {
        this.recoveryDistance = distance;
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
        this.recoveryDistance = DEFAULT_RECOVERY_MATCHING_DISTANCE;
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
    protected BaseLexer(Automaton automaton, Symbol[] terminals, int separator, InputStreamReader input) {
        this.automaton = automaton;
        this.symTerminals = Collections.unmodifiableList(Arrays.asList(terminals));
        this.separatorID = separator;
        this.text = new StreamingText(input);
        this.tokens = new TokenRepository(symTerminals, text);
        this.recoveryDistance = DEFAULT_RECOVERY_MATCHING_DISTANCE;
        this.stateCache = new AutomatonState();
    }

    /**
     * Gets the next token in the input
     *
     * @return The next token in the input
     */
    public Token getNextToken() {
        return tokens.at(getNextToken(DEFAULT_CONTEXT_PROVIDER).getIndex());
    }

    /**
     * Gets the next token in the input
     *
     * @param contexts The current applicable contexts
     * @return The next token in the input
     */
    public abstract TokenKernel getNextToken(IContextProvider contexts);

    /**
     * Runs the lexer's DFA to match a terminal in the input ahead
     *
     * @param index The current start index in the input text
     * @return The matching DFA state and length
     */
    protected TokenMatch runDFA(int index) {
        if (text.isEnd(index)) {
            // At the end of input
            // The only terminal matched at state index 0 is $
            return new TokenMatch(0, 0);
        }

        TokenMatch result = new TokenMatch(0);
        AutomatonState stateData = new AutomatonState();
        int state = 0;
        int i = index;

        while (state != Automaton.DEAD_STATE) {
            automaton.retrieveState(state, stateData);
            // Is this state a matching state ?
            if (stateData.getTerminalCount() != 0)
                result = new TokenMatch(state, i - index);
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

    /**
     * When an error was encountered, runs the lexer's DFA to match a terminal in the input ahead
     *
     * @param originIndex The current start index in the input text
     * @return The matching DFA state and length
     */
    protected TokenMatch runDFAOnError(int originIndex) {
        if (recoveryDistance <= 0) {
            handler.handle(new UnexpectedCharError(Character.toString(text.getValue(originIndex)), text.getPositionAt(originIndex)));
            return new TokenMatch(1);
        } else {
            int index = -1;
            for (int i = 0; i != symTerminals.size(); i++) {
                if (symTerminals.get(i).getID() == separatorID) {
                    index = i;
                    break;
                }
            }
            FuzzyMatcher matcher = new FuzzyMatcher(automaton, index, text, handler, recoveryDistance, originIndex);
            return matcher.run();
        }
    }
}
