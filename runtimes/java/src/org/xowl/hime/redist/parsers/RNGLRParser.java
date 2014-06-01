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

import java.util.ArrayDeque;
import java.util.ArrayList;
import java.util.List;
import java.util.Queue;

/**
 * Represents a base for all RNGLR parsers
 */
public class RNGLRParser extends BaseLRParser {
    /**
     * Represents a reduction operation to be performed
     * <p/>
     * For reduction of length 0, the node is the GSS node on which it is applied, the first label then is epsilon
     * For others, the node is the SECOND GSS node on the path, not the head. The first label is then the label on the transition from the head
     */
    private static class Reduction {
        /**
         * The GSS node to reduce from
         */
        public int node;
        /**
         * The LR production for the reduction
         */
        public LRProduction prod;
        /**
         * The first label in the GSS
         */
        public GSSLabel first;

        /**
         * Initializes this operation
         *
         * @param node  The GSS node to reduce from
         * @param prod  The LR production for the reduction
         * @param first The first label in the GSS
         */
        public Reduction(int node, LRProduction prod, GSSLabel first) {
            this.node = node;
            this.prod = prod;
            this.first = first;
        }
    }

    /**
     * Represents a shift operation to be performed
     */
    private static class Shift {
        /**
         * GSS node to shift from
         */
        public int from;
        /**
         * The target RNGLR state
         */
        public int to;

        /**
         * Initializes this operation
         *
         * @param from The GSS node to shift from
         * @param to   The target RNGLR state
         */
        public Shift(int from, int to) {
            this.from = from;
            this.to = to;
        }
    }

    /**
     * The parser automaton
     */
    private RNGLRAutomaton parserAutomaton;
    /**
     * The GSS for this parser
     */
    private GSS gss;
    /**
     * The SPPF being built
     */
    private SPPFBuilder sppf;
    /**
     * The sub-trees for the constant nullable variables
     */
    private GSSLabel[] nullables;
    /**
     * The next token
     */
    private Token nextToken;
    /**
     * The queue of reduction operations
     */
    private Queue<Reduction> reductions;
    /**
     * The queue of shift operations
     */
    private Queue<Shift> shifts;

    /**
     * Initializes a new instance of the LRkParser class with the given lexer
     *
     * @param automaton The parser's automaton
     * @param variables The parser's variables
     * @param virtuals  The parser's virtuals
     * @param actions   The parser's actions
     * @param lexer     The input lexer
     * @throws InitializationException
     */
    public RNGLRParser(RNGLRAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, ILexer lexer) throws InitializationException {
        super(variables, virtuals, actions, lexer);
        this.parserAutomaton = automaton;
        this.gss = new GSS();
        this.sppf = new SPPFBuilder(lexer.getOutput(), parserVariables, parserVirtuals);
        buildNullables(variables.length);
        this.sppf.clearHistory();
    }

    /**
     * Builds the constant sub-trees of nullable variables
     *
     * @param varCount The total number of variables
     * @throws InitializationException
     */
    private void buildNullables(int varCount) throws InitializationException {
        this.nullables = new GSSLabel[varCount];
        // Get the dependency table
        List<Integer>[] dependencies = buildNullableDependencies(varCount);
        // Solve and build
        int remaining = 1;
        while (remaining > 0) {
            remaining = 0;
            int solved = 0;
            for (int i = 0; i != varCount; i++) {
                List<Integer> dep = dependencies[i];
                if (dep != null) {
                    boolean ok = true;
                    for (Integer r : dep)
                        ok = ok && (dependencies[r] == null);
                    if (ok) {
                        LRProduction prod = parserAutomaton.getNullableProduction(i);
                        this.nullables[i] = buildSPPF(0, prod, sppf.EPSILON, null);
                        dependencies[i] = null;
                        solved++;
                    } else {
                        remaining++;
                    }
                }
            }
            if (solved == 0 && remaining > 0) {
                // There is dependency cycle ...
                // That should not be possible ...
                throw new InitializationException("Failed to initialize the parser, found a cycle in the nullable variables");
            }
        }
    }

    /**
     * Builds the dependency table between nullable variables
     *
     * @param varCount The total number of variables
     * @return The dependency table
     */
    private List<Integer>[] buildNullableDependencies(int varCount) {
        List<Integer>[] result = new List[varCount];
        for (int i = 0; i != varCount; i++) {
            LRProduction prod = parserAutomaton.getNullableProduction(i);
            if (prod != null)
                result[i] = getNullableDependencies(prod);
        }
        return result;
    }

    /**
     * Gets the dependencies on nullable variables
     *
     * @param production The production of a nullable variable
     * @return The list of the nullable variables' indices that this production depends on
     */
    private List<Integer> getNullableDependencies(LRProduction production) {
        List<Integer> result = new ArrayList<Integer>();
        for (int i = 0; i != production.getBytecodeLength(); i++) {
            char op = production.getOpcode(i);
            switch (LROpCode.getBase(op)) {
                case LROpCode.BASE_SEMANTIC_ACTION: {
                    i++;
                    break;
                }
                case LROpCode.BASE_ADD_VIRTUAL: {
                    i++;
                    break;
                }
                case LROpCode.BASE_ADD_NULL_VARIABLE: {
                    result.add((int) production.getOpcode(i + 1));
                    i++;
                    break;
                }
            }
        }
        return result;
    }

    /**
     * Raises an error on an unexpected token
     *
     * @param gen   The current GSS generation
     * @param token The unexpected token
     */
    private void onUnexpectedToken(int gen, Token token) {
        List<Integer> indices = new ArrayList<Integer>();
        List<Symbol> expected = new ArrayList<Symbol>();
        GSS.Gen generation = gss.getGeneration(gen);
        for (int i = generation.start; i != generation.start + generation.count; i++) {
            List<Integer> temp = parserAutomaton.getExpected(gss.getRepresentedState(i), lexer.getTerminals().size());
            for (int index : temp) {
                if (!indices.contains(index)) {
                    indices.add(index);
                    expected.add(lexer.getTerminals().get(index));
                }
            }
        }
        UnexpectedTokenError error = new UnexpectedTokenError(lexer.getOutput().at(token.getIndex()), lexer.getOutput().getPositionOf(token.getIndex()), expected);
        allErrors.add(error);
        if (debug) {
            System.out.println("==== RNGLR parsing error:");
            System.out.print("\t");
            System.out.println(error.toString());
            String[] context = lexer.getOutput().getContext(error.getPosition());
            System.out.print("\t");
            System.out.println(context[0]);
            System.out.print("\t");
            System.out.println(context[1]);
            gss.print();
        }
    }

    /**
     * Builds the SPPF
     *
     * @param generation The current GSS generation
     * @param production The LR production
     * @param first      The first label of the path
     * @param path       The reduction path
     * @return The corresponding SPPF part
     */
    private GSSLabel buildSPPF(int generation, LRProduction production, GSSLabel first, GSSPath path) {
        Symbol variable = parserVariables.get(production.getHead());
        sppf.reductionPrepare(first, path, production.getReductionLength());
        for (int i = 0; i != production.getBytecodeLength(); i++) {
            char op = production.getOpcode(i);
            switch (LROpCode.getBase(op)) {
                case LROpCode.BASE_SEMANTIC_ACTION: {
                    SemanticAction action = parserActions[production.getOpcode(i + 1)];
                    i++;
                    action.execute(variable, sppf);
                    break;
                }
                case LROpCode.BASE_ADD_VIRTUAL: {
                    int index = production.getOpcode(i + 1);
                    sppf.reductionAddVirtual(index, LROpCode.getTreeAction(op));
                    i++;
                    break;
                }
                case LROpCode.BASE_ADD_NULL_VARIABLE: {
                    int index = production.getOpcode(i + 1);
                    sppf.reductionAddNullable(nullables[index], LROpCode.getTreeAction(op));
                    i++;
                    break;
                }
                default:
                    sppf.reductionPop(LROpCode.getTreeAction(op));
                    break;
            }
        }
        return sppf.reduce(generation, production.getHead(), production.getHeadAction() == LROpCode.TREE_ACTION_REPLACE);
    }

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    public ParseResult parse() {
        reductions = new ArrayDeque<Reduction>();
        shifts = new ArrayDeque<Shift>();
        int Ui = gss.createGeneration();
        int v0 = gss.createNode(0);
        nextToken = lexer.getNextToken();

        int count = parserAutomaton.getActionsCount(0, nextToken.getSymbolID());
        for (int i = 0; i != count; i++) {
            LRAction action = parserAutomaton.getAction(0, nextToken.getSymbolID(), i);
            if (action.getCode() == LRAction.CODE_SHIFT)
                shifts.add(new Shift(v0, action.getData()));
            else if (action.getCode() == LRAction.CODE_REDUCE)
                reductions.add(new Reduction(v0, parserAutomaton.getProduction(action.getData()), sppf.EPSILON));
        }

        while (nextToken.getSymbolID() != Symbol.sidEpsilon) // Wait for ε token
        {
            reducer(Ui);
            Token oldtoken = nextToken;
            nextToken = lexer.getNextToken();
            int Uj = shifter(oldtoken);
            GSS.Gen g = gss.getGeneration(Uj);
            if (g.count == 0) {
                // Generation is empty !
                onUnexpectedToken(Ui, oldtoken);
                return new ParseResult(allErrors, lexer.getOutput());
            }
            Ui = Uj;
        }

        GSS.Gen g = gss.getGeneration(Ui);
        for (int i = g.start; i != g.start + g.count; i++) {
            int state = gss.getRepresentedState(i);
            if (parserAutomaton.isAcceptingState(state)) {
                // Has reduction _Axiom_ -> axiom $ . on ε
                GSS.PathSet set = gss.getPaths(i, 2);
                return new ParseResult(allErrors, lexer.getOutput(), sppf.getTree(set.paths[0].get(1)));
            }
        }
        // At end of input but was still waiting for tokens
        return new ParseResult(allErrors, lexer.getOutput());
    }

    /**
     * Executes the reduction operations from the given GSS generation
     *
     * @param generation The current GSS generation
     */
    private void reducer(int generation) {
        sppf.clearHistory();
        while (!reductions.isEmpty())
            executeReduction(generation, reductions.remove());
        sppf.collect();
    }

    /**
     * Executes a reduction operation for all found path
     *
     * @param generation The current GSS generation
     * @param reduction  The reduction operation
     */
    private void executeReduction(int generation, Reduction reduction) {
        // Get all path from the reduction node
        GSS.PathSet set = null;
        if (reduction.prod.getReductionLength() == 0)
            set = gss.getPaths(reduction.node, 0);
        else
            // The given GSS node is the second on the path, so start from it with length - 1
            set = gss.getPaths(reduction.node, reduction.prod.getReductionLength() - 1);

        // Execute the reduction on all paths
        for (int i = 0; i != set.count; i++)
            executeReduction(generation, reduction, set.paths[i]);
    }

    /**
     * Executes a reduction operation for a given path
     *
     * @param generation The current GSS generation
     * @param reduction  The reduction operation
     * @param path       The GSS path to use for the reduction
     */
    private void executeReduction(int generation, Reduction reduction, GSSPath path) {
        // Get the rule's head
        Symbol head = parserVariables.get(reduction.prod.getHead());
        // Resolve the sub-root
        GSSLabel label = sppf.getLabelFor(generation, SymbolRef.encode(SymbolType.VARIABLE, reduction.prod.getHead()));
        if (label.isEpsilon()) {
            // not in history, build the SPPF here
            label = buildSPPF(generation, reduction.prod, reduction.first, path);
        }

        // Get the target state by transition on the rule's head
        int to = getNextByVar(gss.getRepresentedState(path.getLast()), head.getID());
        // Find a node for the target state in the GSS
        int w = gss.findNode(generation, to);
        if (w != -1) {
            // A node for the target state is already in the GSS
            if (!gss.hasEdge(generation, w, path.getLast())) {
                // But the new edge does not exist
                gss.createEdge(w, path.getLast(), label);
                // Look for the new reductions at this state
                if (reduction.prod.getReductionLength() != 0) {
                    int count = parserAutomaton.getActionsCount(to, nextToken.getSymbolID());
                    for (int i = 0; i != count; i++) {
                        LRAction action = parserAutomaton.getAction(to, nextToken.getSymbolID(), i);
                        if (action.getCode() == LRAction.CODE_REDUCE) {
                            LRProduction prod = parserAutomaton.getProduction(action.getData());
                            // length 0 reduction are not considered here because they already exist at this point
                            if (prod.getReductionLength() != 0)
                                reductions.add(new Reduction(path.getLast(), prod, label));
                        }
                    }
                }
            }
        } else {
            // Create the new corresponding node in the GSS
            w = gss.createNode(to);
            gss.createEdge(w, path.getLast(), label);
            // Look for all the reductions and shifts at this state
            int count = parserAutomaton.getActionsCount(to, nextToken.getSymbolID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(to, nextToken.getSymbolID(), i);
                if (action.getCode() == LRAction.CODE_SHIFT) {
                    shifts.add(new Shift(w, action.getData()));
                } else if (action.getCode() == LRAction.CODE_REDUCE) {
                    LRProduction prod = parserAutomaton.getProduction(action.getData());
                    if (prod.getReductionLength() == 0)
                        reductions.add(new Reduction(w, prod, SPPFBuilder.EPSILON));
                    else if (reduction.prod.getReductionLength() != 0)
                        reductions.add(new Reduction(path.getLast(), prod, label));
                }
            }
        }
    }

    /**
     * Executes the shift operations for the given token
     *
     * @param oldtoken A token
     * @return The next generation
     */
    private int shifter(Token oldtoken) {
        // Create next generation
        int gen = gss.createGeneration();

        // Create the GSS label to be used for the transitions
        int sym = SymbolRef.encode(SymbolType.TOKEN, oldtoken.getIndex());
        GSSLabel label = new GSSLabel(sym, sppf.getSingleNode(sym));

        // Execute all shifts in the queue at this point
        int count = shifts.size();
        for (int i = 0; i != count; i++)
            executeShift(gen, label, shifts.remove());
        return gen;
    }

    /**
     * Executes a shift operation
     *
     * @param gen   The GSS generation to start from
     * @param label The GSS label to use for the new GSS edges
     * @param shift The shift operation
     */
    private void executeShift(int gen, GSSLabel label, Shift shift) {
        int w = gss.findNode(gen, shift.to);
        if (w != -1) {
            // A node for the target state is already in the GSS
            gss.createEdge(w, shift.from, label);
            // Look for the new reductions at this state
            int count = parserAutomaton.getActionsCount(shift.to, nextToken.getSymbolID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(shift.to, nextToken.getSymbolID(), i);
                if (action.getCode() == LRAction.CODE_REDUCE) {
                    LRProduction prod = parserAutomaton.getProduction(action.getData());
                    // length 0 reduction are not considered here because they already exist at this point
                    if (prod.getReductionLength() != 0)
                        reductions.add(new Reduction(shift.from, prod, label));
                }
            }
        } else {
            // Create the new corresponding node in the GSS
            w = gss.createNode(shift.to);
            gss.createEdge(w, shift.from, label);
            // Look for all the reductions and shifts at this state
            int count = parserAutomaton.getActionsCount(shift.to, nextToken.getSymbolID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(shift.to, nextToken.getSymbolID(), i);
                if (action.getCode() == LRAction.CODE_SHIFT)
                    shifts.add(new Shift(w, action.getData()));
                else if (action.getCode() == LRAction.CODE_REDUCE) {
                    LRProduction prod = parserAutomaton.getProduction(action.getData());
                    if (prod.getReductionLength() == 0) // Length 0 => reduce from the head
                        reductions.add(new Reduction(w, prod, SPPFBuilder.EPSILON));
                    else // reduce from the second node on the path
                        reductions.add(new Reduction(shift.from, prod, label));
                }
            }
        }
    }

    /**
     * Gets the next RNGLR state by a shift with the given variable ID
     *
     * @param state A RNGLR state
     * @param var   A variable ID
     * @return The next RNGLR state, or 0xFFFF if no transition is found
     */
    private int getNextByVar(int state, int var) {
        int ac = parserAutomaton.getActionsCount(state, var);
        for (int i = 0; i != ac; i++) {
            LRAction action = parserAutomaton.getAction(state, var, i);
            if (action.getCode() == LRAction.CODE_SHIFT)
                return action.getData();
        }
        return 0xFFFF;
    }
}
