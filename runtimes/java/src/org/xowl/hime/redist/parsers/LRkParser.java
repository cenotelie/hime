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

import org.xowl.hime.redist.*;
import org.xowl.hime.redist.lexer.ILexer;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Represents a base for all LR(k) parsers
 */
public abstract class LRkParser extends BaseLRParser {
    /**
     * Initial size of the stack
     */
    protected static final int INIT_STACK_SIZE = 128;

    /**
     * The simulator for LR(k) parsers used for error recovery
     */
    private class Simulator extends LRkSimulator {
        /**
         * Initializes a new simulator based on the given LR(k) parser
         */
        public Simulator() {
            this.parserAutomaton = LRkParser.this.parserAutomaton;
            this.parserVariables = LRkParser.this.parserVariables;
            this.input = LRkParser.this.input;
            this.stack = Arrays.copyOf(LRkParser.this.stack, INIT_STACK_SIZE);
            this.stack = new int[INIT_STACK_SIZE];
            this.head = LRkParser.this.head;
        }
    }

    /**
     * The parser's automaton
     */
    private LRkAutomaton parserAutomaton;
    /**
     * The parser's input as a stream of tokens
     */
    private RewindableTokenStream input;
    /**
     * The AST builder
     */
    private LRkASTBuilder builder;
    /**
     * The parser's stack
     */
    private int[] stack;
    /**
     * Index of the stack's head
     */
    private int head;

    /**
     * Initializes a new instance of the LRkParser class with the given lexer
     *
     * @param automaton The parser's automaton
     * @param variables The parser's variables
     * @param virtuals  The parser's virtuals
     * @param actions   The parser's actions
     * @param lexer     The input lexer
     */
    protected LRkParser(LRkAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, ILexer lexer) {
        super(variables, virtuals, actions, lexer);
        this.parserAutomaton = automaton;
        this.input = new RewindableTokenStream(lexer);
        this.builder = new LRkASTBuilder(lexer.getOutput(), parserVariables, parserVirtuals);
    }

    /**
     * Raises an error on an unexepcted token
     *
     * @param token The unexpected token
     * @return The next token in the case the error is recovered
     */
    private Token onUnexpectedToken(Token token) {
        LRExpected expectedOnHead = parserAutomaton.getExpected(stack[head], lexer.getTerminals());
        // the terminals for shifts are always expected
        List<Symbol> expected = new ArrayList<Symbol>(expectedOnHead.getShifts());
        // check the terminals for reductions
        for (Symbol terminal : expectedOnHead.getReductions())
            if (checkIsExpected(terminal))
                expected.add(terminal);
        // register the error
        allErrors.add(new UnexpectedTokenError(lexer.getOutput().at(token.getIndex()), lexer.getOutput().getPositionOf(token.getIndex()), expected));
        if (!recover)
            return new Token(0, 0);
        if (tryDrop1Unexpected())
            return input.getNextToken();
        if (tryDrop2Unexpected())
            return input.getNextToken();
        for (Symbol terminal : expected) {
            Token dummy = new Token(terminal.getID(), 0);
            if (tryInsertExpected(dummy))
                return dummy;
        }
        return new Token(0, 0);
    }

    /**
     * Checks whether the specified terminal is indeed expected for a reduction.
     * This check is required because in the case of a base LALR graph,
     * some terminals expected for reduction in the automaton are coming from other paths.
     *
     * @param terminal The terminal to check
     * @return true if the terminal is really expected
     */
    private boolean checkIsExpected(Symbol terminal) {
        // copy the stack to use for the simulation
        int[] myStack = Arrays.copyOf(stack, stack.length);
        int myHead = head;
        // get the action for the stack's head
        LRAction action = parserAutomaton.getAction(myStack[myHead], terminal.getID());
        while (action.getCode() != LRAction.CODE_NONE) {
            if (action.getCode() == LRAction.CODE_SHIFT)
                // yep, the terminal was expected
                return true;
            if (action.getCode() == LRAction.CODE_REDUCE) {
                // execute the reduction
                LRProduction production = parserAutomaton.getProduction(action.getData());
                myHead -= production.getReductionLength();
                // this must be a shift
                action = parserAutomaton.getAction(myStack[myHead], parserVariables.get(production.getHead()).getID());
                myHead++;
                if (myHead == myStack.length)
                    myStack = Arrays.copyOf(myStack, myStack.length + INIT_STACK_SIZE);
                myStack[myHead] = action.getData();
                // now, get the new action for the terminal
                action = parserAutomaton.getAction(action.getData(), terminal.getID());
            }
        }
        // nope, that was a pathological case in a LALR graph
        return false;
    }

    private boolean tryDrop1Unexpected() {
        Simulator sim = new Simulator();
        boolean success = sim.testForLength(3, new Token(0, 0));
        input.rewind(sim.getAdvance());
        return (success);
    }

    private boolean tryDrop2Unexpected() {
        input.getNextToken();
        Simulator sim = new Simulator();
        boolean success = sim.testForLength(3, new Token(0, 0));
        input.rewind(sim.getAdvance());
        if (!success)
            input.rewind(1);
        return success;
    }

    private boolean tryInsertExpected(Token terminal) {
        Simulator sim = new Simulator();
        boolean success = sim.testForLength(3, terminal);
        input.rewind(sim.getAdvance());
        return (success);
    }

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    public ParseResult parse() {
        this.stack = new int[INIT_STACK_SIZE];
        Token nextToken = input.getNextToken();
        while (true) {
            char action = ParseOnToken(nextToken);
            if (action == LRAction.CODE_SHIFT) {
                nextToken = input.getNextToken();
                continue;
            }
            if (action == LRAction.CODE_ACCEPT)
                return new ParseResult(allErrors, lexer.getOutput(), builder.GetTree());
            nextToken = onUnexpectedToken(nextToken);
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
    private char ParseOnToken(Token token) {
        while (true) {
            LRAction action = parserAutomaton.getAction(stack[head], token.getSymbolID());
            if (action.getCode() == LRAction.CODE_SHIFT) {
                head++;
                if (head == stack.length)
                    stack = Arrays.copyOf(stack, stack.length + INIT_STACK_SIZE);
                stack[head] = action.getData();
                builder.stackPushToken(token.getIndex());
                return action.getCode();
            } else if (action.getCode() == LRAction.CODE_REDUCE) {
                LRProduction production = parserAutomaton.getProduction(action.getData());
                head -= production.getReductionLength();
                reduce(production);
                action = parserAutomaton.getAction(stack[head], parserVariables.get(production.getHead()).getID());
                head++;
                if (head == stack.length)
                    stack = Arrays.copyOf(stack, stack.length + INIT_STACK_SIZE);
                stack[head] = action.getData();
                continue;
            }
            return action.getCode();
        }
    }

    /**
     * Executes the given LR reduction
     *
     * @param production A LR reduction
     */
    private void reduce(LRProduction production) {
        Symbol variable = parserVariables.get(production.getHead());
        builder.reductionPrepare(production.getHead(), production.getReductionLength(), production.getHeadAction());
        for (int i = 0; i != production.getBytecodeLength(); i++) {
            char op = production.getOpcode(i);
            switch (LROpCode.getBase(op)) {
                case LROpCode.BASE_SEMANTIC_ACTION: {
                    SemanticAction action = parserActions[production.getOpcode(i + 1)];
                    i++;
                    action.execute(variable, builder);
                    break;
                }
                case LROpCode.BASE_ADD_VIRTUAL: {
                    int index = production.getOpcode(i + 1);
                    builder.reductionAddVirtual(index, LROpCode.getTreeAction(op));
                    i++;
                    break;
                }
                default:
                    builder.reductionPop(LROpCode.getTreeAction(op));
                    break;
            }
        }
        builder.reduce();
    }
}
