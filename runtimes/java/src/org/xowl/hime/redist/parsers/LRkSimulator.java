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
package org.xowl.hime.redist.parsers;

import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.Token;

import java.util.List;

/**
 * Represents a base simulator for all LR(k) parsers
 */
abstract class LRkSimulator {
    /**
     * Parser's variables
     */
    protected List<Symbol> parserVariables;
    /**
     * LR(k) parsing table and productions
     */
    protected LRkAutomaton parserAutomaton;
    /**
     * Parser's input encapsulating the lexer
     */
    protected RewindableTokenStream input;
    /**
     * Parser's stack
     */
    protected int[] stack;
    /**
     * Current stack's head
     */
    protected int head;
    /**
     * The number of token used from the input
     */
    protected int advance = 0;

    /**
     * Gets the number of token used from the input
     *
     * @return The number of token used from the input
     */
    public int getAdvance() {
        return advance;
    }

    /**
     * Tests the given input against the parser
     *
     * @param length   Length to test
     * @param inserted Token to insert, or null if none should be inserted
     * @return Whether the test succeeded
     */
    public boolean testForLength(int length, Token inserted) {
        int remaining = length;
        Token nextToken = (inserted.getSymbolID() != 0) ? inserted : input.getNextToken();
        advance = (inserted.getSymbolID() != 0) ? 1 : 0;
        while (true) {
            char action = recognizeOnToken(nextToken);
            if (action == LRAction.CODE_SHIFT) {
                remaining--;
                if (remaining == 0)
                    return true;
                nextToken = input.getNextToken();
                advance++;
                continue;
            }
            if (action == LRAction.CODE_ACCEPT)
                return true;
            return false;
        }
    }

    private char recognizeOnToken(Token token) {
        while (true) {
            LRAction action = parserAutomaton.getAction(stack[head], token.getSymbolID());
            if (action.getCode() == LRAction.CODE_SHIFT) {
                stack[++head] = action.getData();
                return action.getCode();
            } else if (action.getCode() == LRAction.CODE_REDUCE) {
                LRProduction production = parserAutomaton.getProduction(action.getData());
                Symbol var = parserVariables.get(production.getHead());
                head -= production.getReductionLength();
                action = parserAutomaton.getAction(stack[head], var.getID());
                stack[++head] = action.getData();
                continue;
            }
            return action.getCode();
        }
    }
}
