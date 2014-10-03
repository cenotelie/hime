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

import org.xowl.hime.redist.ParseResult;
import org.xowl.hime.redist.SemanticAction;
import org.xowl.hime.redist.Symbol;
import org.xowl.hime.redist.Token;
import org.xowl.hime.redist.lexer.ILexer;

/**
 * Represents a base for all context-free LR(k) parsers
 */
public abstract class LRkContextSensitiveParser extends LRkParser {
    /**
     * The parser's stack
     */
    private LRkStack stack;

    /**
     * Initializes a new instance of the parser
     *
     * @param automaton The parser's automaton
     * @param variables The parser's variables
     * @param virtuals  The parser's virtuals
     * @param actions   The parser's actions
     * @param lexer     The input lexer
     */
    protected LRkContextSensitiveParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, ILexer lexer) {
        super(automaton, variables, virtuals, actions, lexer);
    }

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    public ParseResult parse() {
        stack = new LRkStack();
        Token nextToken = lexer.getNextToken(stack);
        while (true) {
            char action = parseOnToken(nextToken);
            if (action == LRAction.CODE_SHIFT) {
                nextToken = lexer.getNextToken(stack);
                continue;
            }
            if (action == LRAction.CODE_ACCEPT)
                return new ParseResult(allErrors, lexer.getOutput(), builder.GetTree());
            nextToken = onUnexpectedToken(stack.getHeadState(), nextToken);
            if (nextToken.getSymbolID() == 0 || allErrors.size() >= MAX_ERROR_COUNT)
                return new ParseResult(allErrors, lexer.getOutput());
        }
    }

    /**
     * Parses the given token
     *
     * @param token The token to parse
     * @return The LR action on the token
     */
    private char parseOnToken(Token token) {
        while (true) {
            LRAction action = automaton.getAction(stack.getHeadState(), token.getSymbolID());
            if (action.getCode() == LRAction.CODE_SHIFT) {
                stack.pushBlock(action.getData(), 1);
                builder.stackPushToken(token.getIndex());
                return action.getCode();
            } else if (action.getCode() == LRAction.CODE_REDUCE) {
                LRProduction production = automaton.getProduction(action.getData());
                stack.popBlocks(production.getReductionLength());
                reduce(production);
                action = automaton.getAction(stack.getHeadState(), parserVariables.get(production.getHead()).getID());
                stack.pushBlock(action.getData(), 1);
                continue;
            }
            return action.getCode();
        }
    }
}
