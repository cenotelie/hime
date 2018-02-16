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
package fr.cenotelie.hime.redist.parsers;

import fr.cenotelie.hime.redist.*;
import fr.cenotelie.hime.redist.lexer.Automaton;
import fr.cenotelie.hime.redist.lexer.BaseLexer;
import fr.cenotelie.hime.redist.lexer.IContextProvider;
import fr.cenotelie.hime.redist.lexer.TokenKernel;
import fr.cenotelie.hime.redist.utils.IntList;

import java.util.*;

/**
 * Represents a base for all RNGLR parsers
 *
 * @author Laurent Wouters
 */
public class RNGLRParser extends BaseLRParser implements IContextProvider {
    /**
     * Represents a reduction operation to be performed
     * For reduction of length 0, the node is the GSS node on which it is applied, the first label then is epsilon
     * For others, the node is the SECOND GSS node on the path, not the head. The first label is then the label on the transition from the head
     */
    private static class Reduction {
        /**
         * The GSS node to reduce from
         */
        public final int node;
        /**
         * The LR production for the reduction
         */
        public final LRProduction prod;
        /**
         * The first label in the GSS
         */
        public final int first;

        /**
         * Initializes this operation
         *
         * @param node  The GSS node to reduce from
         * @param prod  The LR production for the reduction
         * @param first The first label in the GSS
         */
        public Reduction(int node, LRProduction prod, int first) {
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
        public final int from;
        /**
         * The target RNGLR state
         */
        public final int to;

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
    private final RNGLRAutomaton parserAutomaton;
    /**
     * The GSS for this parser
     */
    private final GSS gss;
    /**
     * The SPPF being built
     */
    private final SPPFBuilder sppf;
    /**
     * The sub-trees for the constant nullable variables
     */
    private final int[] nullables;
    /**
     * The next token
     */
    private TokenKernel nextToken;
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
     * @throws InitializationException if the parser failed to initialize the nullable rules
     */
    public RNGLRParser(RNGLRAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, BaseLexer lexer) throws InitializationException {
        super(variables, virtuals, actions, lexer);
        this.parserAutomaton = automaton;
        this.gss = new GSS();
        this.sppf = new SPPFBuilder(lexer.getTokens(), symVariables, symVirtuals);
        nullables = new int[variables.length];
        buildNullables(variables.length);
        this.sppf.clearHistory();
    }

    @Override
    public int getContextPriority(int context, int onTerminalID) {
        // the default context is always active
        if (context == Automaton.DEFAULT_CONTEXT)
            return Integer.MAX_VALUE;
        if (lexer.getTokens().size() == 0) {
            // this is the first token, does it open the context?
            return parserAutomaton.getContexts(0).opens(onTerminalID, context) ? 0 : -1;
        }
        // try to only look at stack heads that expect the terminal
        IntList queue = new IntList(LRkParser.INIT_STACK_SIZE);
        List<LRProduction> productions = new ArrayList<>();
        IntList distances = new IntList(LRkParser.INIT_STACK_SIZE);
        boolean foundOnPreviousShift = false;
        for (Shift shift : shifts) {
            int count = parserAutomaton.getActionsCount(shift.to, onTerminalID);
            if (count == 0)
                continue;
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(shift.to, onTerminalID, i);
                if (action.getCode() == LRAction.CODE_SHIFT) {
                    // does the context opens with the terminal?
                    if (parserAutomaton.getContexts(shift.to).opens(onTerminalID, context))
                        return 0;
                    // looking at the immediate history, does the context opens from the shift just before?
                    if (parserAutomaton.getContexts(gss.getRepresentedState(shift.from)).opens(nextToken.getTerminalID(), context)) {
                        foundOnPreviousShift = true;
                        break;
                    }
                    // no, enqueue
                    if (!queue.contains(shift.from)) {
                        queue.add(shift.from);
                        productions.add(null);
                        distances.add(2);
                    }
                } else {
                    // this is reduction
                    LRProduction production = parserAutomaton.getProduction(action.getData());
                    // looking at the immediate history, does the context opens from the shift just before?
                    if (parserAutomaton.getContexts(gss.getRepresentedState(shift.from)).opens(nextToken.getTerminalID(), context)) {
                        if (production.getReductionLength() < 1) {
                            // the reduction does not close the context
                            foundOnPreviousShift = true;
                            break;
                        }
                    }
                    // no, enqueue
                    if (!queue.contains(shift.from)) {
                        queue.add(shift.from);
                        productions.add(production);
                        distances.add(2);
                    }
                }
            }
        }
        if (foundOnPreviousShift)
            // found the context opening on the previous shift (and was not immediately closed by a reduction)
            return 1;
        if (queue.size() == 0) {
            // the track is empty, the terminal is unexpected
            return -1;
        }
        // explore the GSS to find the specified context
        for (int i = 0; i != queue.size(); i++) {
            GSS.PathSet paths = gss.getPaths(queue.get(i), 1);
            for (int p = 0; p != paths.count; p++) {
                int from = paths.content[p].getLast();
                int symbolID = sppf.getSymbolOn(paths.content[p].get(0)).getID();
                int distance = distances.get(i);
                LRProduction production = productions.get(i);
                // was the context open on this transition?
                if (parserAutomaton.getContexts(gss.getRepresentedState(from)).opens(symbolID, context)) {
                    if (production == null || production.getReductionLength() < distance)
                        return distance;
                }
                // no, enqueue
                if (!queue.contains(from)) {
                    queue.add(from);
                    productions.add(production);
                    distances.add(distance + 1);
                }
            }
        }
        // at this point, the requested context is not yet open
        // can it be open by a token with the specified terminal ID?
        // queue of GLR states to inspect:
        IntList queueGSSHead = new IntList(LRkParser.INIT_STACK_SIZE);   // the related GSS head
        List<int[]> queueVStack = new ArrayList<>(); // the virtual stack
        // first reduction
        for (Shift shift : shifts) {
            int count = parserAutomaton.getActionsCount(shift.to, onTerminalID);
            if (count > 0) {
                queueGSSHead.add(shift.from);
                queueVStack.add(new int[]{shift.to});
            }
        }
        // now, close the queue
        for (int i = 0; i != queueGSSHead.size(); i++) {
            int head = queueVStack.get(i)[queueVStack.get(i).length - 1];
            int count = parserAutomaton.getActionsCount(head, onTerminalID);
            if (count == 0)
                continue;
            for (int j = 0; j != count; j++) {
                LRAction action = parserAutomaton.getAction(head, onTerminalID, j);
                if (action.getCode() != LRAction.CODE_REDUCE)
                    continue;
                LRProduction production = parserAutomaton.getProduction(action.getData());
                if (production.getReductionLength() == 0) {
                    // 0-length reduction => start from the current head
                    int[] virtualStack = Arrays.copyOf(queueVStack.get(i), queueVStack.get(i).length + 1);
                    int next = getNextByVar(head, symVariables.get(production.getHead()).getID());
                    virtualStack[virtualStack.length - 1] = next;
                    // enqueue
                    queueGSSHead.add(queueGSSHead.get(i));
                    queueVStack.add(virtualStack);
                } else if (production.getReductionLength() < queueVStack.get(i).length) {
                    // we are still the virtual stack
                    int[] virtualStack = Arrays.copyOf(queueVStack.get(i), queueVStack.get(i).length - production.getReductionLength() + 1);
                    int next = getNextByVar(virtualStack[virtualStack.length - 2], symVariables.get(production.getHead()).getID());
                    virtualStack[virtualStack.length - 1] = next;
                    // enqueue
                    queueGSSHead.add(queueGSSHead.get(i));
                    queueVStack.add(virtualStack);
                } else {
                    // we reach the GSS
                    GSS.PathSet paths = gss.getPaths(queueGSSHead.get(i), production.getReductionLength() - queueVStack.get(i).length);
                    for (int k = 0; k != paths.count; k++) {
                        GSSPath path = paths.content[k];
                        // get the target GLR state
                        int next = getNextByVar(gss.getRepresentedState(path.getLast()), symVariables.get(production.getHead()).getID());
                        // enqueue the info, top GSS stack node and target GLR state
                        queueGSSHead.add(path.getLast());
                        queueVStack.add(new int[]{next});
                    }
                }
            }
        }
        for (int[] vstack : queueVStack) {
            int state = vstack[vstack.length - 1];
            int count = parserAutomaton.getActionsCount(state, onTerminalID);
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(state, onTerminalID, i);
                if (action.getCode() == LRAction.CODE_SHIFT && parserAutomaton.getContexts(state).opens(onTerminalID, context))
                    // the context opens here
                    return 0;
            }
        }
        // the context is still unavailable
        return -1;
    }

    /**
     * Builds the constant sub-trees of nullable variables
     *
     * @param varCount The total number of variables
     * @throws InitializationException
     */
    private void buildNullables(int varCount) throws InitializationException {
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
                        this.nullables[i] = buildSPPF(0, prod, SPPF.EPSILON, null);
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
        List<Integer> result = new ArrayList<>();
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
                default:
                    break;
            }
        }
        return result;
    }

    /**
     * Raises an error on an unexpected token
     *
     * @param stem The size of the generation's stem
     */
    private void onUnexpectedToken(int stem) {
        // build the list of expected terminals
        List<Symbol> expected = new ArrayList<>();
        GSSGeneration genData = gss.getGeneration();
        for (int i = 0; i != genData.getCount(); i++) {
            LRExpected expectedOnHead = parserAutomaton.getExpected(gss.getRepresentedState(i + genData.getStart()), lexer.getTerminals());
            // register the terminals for shift actions
            for (Symbol terminal : expectedOnHead.getShifts())
                if (!expected.contains(terminal))
                    expected.add(terminal);
            if (i < stem) {
                // the state was in the stem, also look for reductions
                for (Symbol terminal : expectedOnHead.getReductions())
                    if (!expected.contains(terminal) && checkIsExpected(i + genData.getStart(), terminal))
                        expected.add(terminal);
            }
        }
        // register the error
        UnexpectedTokenError error = new UnexpectedTokenError(lexer.getTokens().at(nextToken.getIndex()), expected);
        allErrors.add(error);
        if (isModeDebug()) {
            System.out.println("==== RNGLR parsing error:");
            System.out.print("\t");
            System.out.println(error.toString());
            TextContext context = lexer.getInput().getContext(error.getPosition());
            System.out.print("\t");
            System.out.println(context.getContent());
            System.out.print("\t");
            System.out.println(context.getPointer());
            gss.print();
        }
    }

    /**
     * Checks whether the specified terminal is indeed expected for a reduction.
     * This check is required because in the case of a base LALR graph,
     * some terminals expected for reduction in the automaton are coming from other paths.
     *
     * @param gssNode  The GSS node from which to reduce
     * @param terminal The terminal to check
     * @return true if the terminal is really expected
     */
    private boolean checkIsExpected(int gssNode, Symbol terminal) {
        // queue of GLR states to inspect:
        IntList queueGSSHead = new IntList(LRkParser.INIT_STACK_SIZE);   // the related GSS head
        List<int[]> queueVStack = new ArrayList<>(); // the virtual stack

        // first reduction
        {
            int count = parserAutomaton.getActionsCount(gss.getRepresentedState(gssNode), terminal.getID());
            for (int j = 0; j != count; j++) {
                LRAction action = parserAutomaton.getAction(gss.getRepresentedState(gssNode), terminal.getID(), j);
                if (action.getCode() == LRAction.CODE_REDUCE) {
                    // execute the reduction
                    LRProduction production = parserAutomaton.getProduction(action.getData());
                    GSS.PathSet paths = gss.getPaths(gssNode, production.getReductionLength());
                    for (int k = 0; k != paths.count; k++) {
                        GSSPath path = paths.content[k];
                        // get the target GLR state
                        int next = getNextByVar(gss.getRepresentedState(path.getLast()), symVariables.get(production.getHead()).getID());
                        // enqueue the info, top GSS stack node and target GLR state
                        queueGSSHead.add(path.getLast());
                        queueVStack.add(new int[]{next});
                    }
                }
            }
        }

        // now, close the queue
        for (int i = 0; i != queueGSSHead.size(); i++) {
            int head = queueVStack.get(i)[queueVStack.get(i).length - 1];
            int count = parserAutomaton.getActionsCount(head, terminal.getID());
            if (count == 0)
                continue;
            for (int j = 0; j != count; j++) {
                LRAction action = parserAutomaton.getAction(head, terminal.getID(), j);
                if (action.getCode() == LRAction.CODE_SHIFT)
                    // yep, the terminal was expected
                    return true;
                if (action.getCode() == LRAction.CODE_REDUCE) {
                    // execute the reduction
                    LRProduction production = parserAutomaton.getProduction(action.getData());
                    if (production.getReductionLength() == 0) {
                        // 0-length reduction => start from the current head
                        int[] virtualStack = Arrays.copyOf(queueVStack.get(i), queueVStack.get(i).length + 1);
                        virtualStack[virtualStack.length - 1] = getNextByVar(head, symVariables.get(production.getHead()).getID());
                        // enqueue
                        queueGSSHead.add(queueGSSHead.get(i));
                        queueVStack.add(virtualStack);
                    } else if (production.getReductionLength() < queueVStack.get(i).length) {
                        // we are still the virtual stack
                        int[] virtualStack = Arrays.copyOf(queueVStack.get(i), queueVStack.get(i).length - production.getReductionLength() + 1);
                        virtualStack[virtualStack.length - 1] = getNextByVar(virtualStack[virtualStack.length - 2], symVariables.get(production.getHead()).getID());
                        // enqueue
                        queueGSSHead.add(queueGSSHead.get(i));
                        queueVStack.add(virtualStack);
                    } else {
                        // we reach the GSS
                        GSS.PathSet paths = gss.getPaths(queueGSSHead.get(i), production.getReductionLength() - queueVStack.get(i).length);
                        for (int k = 0; k != paths.count; k++) {
                            GSSPath path = paths.content[k];
                            // get the target GLR state
                            int next = getNextByVar(gss.getRepresentedState(path.getLast()), symVariables.get(production.getHead()).getID());
                            // enqueue the info, top GSS stack node and target GLR state
                            queueGSSHead.add(path.getLast());
                            queueVStack.add(new int[]{next});
                        }
                    }
                }
            }
        }

        // nope, that was a pathological case in a LALR graph
        return false;
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
    private int buildSPPF(int generation, LRProduction production, int first, GSSPath path) {
        Symbol variable = symVariables.get(production.getHead());
        sppf.reductionPrepare(first, path, production.getReductionLength());
        for (int i = 0; i != production.getBytecodeLength(); i++) {
            char op = production.getOpcode(i);
            switch (LROpCode.getBase(op)) {
                case LROpCode.BASE_SEMANTIC_ACTION: {
                    SemanticAction action = symActions.get(production.getOpcode(i + 1));
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
        return sppf.reduce(generation, production.getHead(), production.getHeadAction());
    }

    /**
     * Parses the input and returns the result
     *
     * @return A ParseResult object containing the data about the result
     */
    public ParseResult parse() {
        reductions = new ArrayDeque<>();
        shifts = new ArrayDeque<>();
        int Ui = gss.createGeneration();
        int v0 = gss.createNode(0);
        nextToken = lexer.getNextToken(this);

        // bootstrap the shifts and reductions queues
        int count = parserAutomaton.getActionsCount(0, nextToken.getTerminalID());
        for (int i = 0; i != count; i++) {
            LRAction action = parserAutomaton.getAction(0, nextToken.getTerminalID(), i);
            if (action.getCode() == LRAction.CODE_SHIFT)
                shifts.add(new Shift(v0, action.getData()));
            else if (action.getCode() == LRAction.CODE_REDUCE)
                reductions.add(new Reduction(v0, parserAutomaton.getProduction(action.getData()), SPPF.EPSILON));
        }

        while (nextToken.getTerminalID() != Symbol.SID_EPSILON) {
            // Wait for ε token
            // the stem length (initial number of nodes in the generation before reductions)
            int stem = gss.getGeneration(Ui).getCount();
            // apply all reduction actions
            reducer(Ui);
            // no scheduled shift actions?
            if (shifts.isEmpty()) {
                // the next token was not expected
                onUnexpectedToken(stem);
                return new ParseResult(allErrors, lexer.getInput());
            }
            // look for the next next-token
            TokenKernel oldtoken = nextToken;
            nextToken = lexer.getNextToken(this);
            // apply the scheduled shift actions
            Ui = shifter(oldtoken);
        }

        GSSGeneration genData = gss.getGeneration(Ui);
        for (int i = genData.getStart(); i != genData.getStart() + genData.getCount(); i++) {
            int state = gss.getRepresentedState(i);
            if (parserAutomaton.isAcceptingState(state)) {
                // Has reduction _Axiom_ -> axiom $ . on ε
                GSS.PathSet paths = gss.getPaths(i, 2);
                return new ParseResult(allErrors, lexer.getInput(), sppf.getTree(paths.content[0].get(1)));
            }
        }
        // At end of input but was still waiting for tokens
        return new ParseResult(allErrors, lexer.getInput());
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
    }

    /**
     * Executes a reduction operation for all found path
     *
     * @param generation The current GSS generation
     * @param reduction  The reduction operation
     */
    private void executeReduction(int generation, Reduction reduction) {
        // Get all path from the reduction node
        GSS.PathSet paths;
        if (reduction.prod.getReductionLength() == 0)
            paths = gss.getPaths(reduction.node, 0);
        else
            // The given GSS node is the second on the path, so start from it with length - 1
            paths = gss.getPaths(reduction.node, reduction.prod.getReductionLength() - 1);

        // Execute the reduction on all paths
        for (int i = 0; i != paths.count; i++)
            executeReduction(generation, reduction, paths.content[i]);
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
        Symbol head = symVariables.get(reduction.prod.getHead());
        // Resolve the sub-root
        int label = sppf.getLabelFor(path.getGeneration(), TableElemRef.encode(TableElemRef.TABLE_VARIABLE, reduction.prod.getHead()));
        if (label == SPPF.EPSILON) {
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
                    int count = parserAutomaton.getActionsCount(to, nextToken.getTerminalID());
                    for (int i = 0; i != count; i++) {
                        LRAction action = parserAutomaton.getAction(to, nextToken.getTerminalID(), i);
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
            int count = parserAutomaton.getActionsCount(to, nextToken.getTerminalID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(to, nextToken.getTerminalID(), i);
                if (action.getCode() == LRAction.CODE_SHIFT) {
                    shifts.add(new Shift(w, action.getData()));
                } else if (action.getCode() == LRAction.CODE_REDUCE) {
                    LRProduction prod = parserAutomaton.getProduction(action.getData());
                    if (prod.getReductionLength() == 0)
                        reductions.add(new Reduction(w, prod, SPPF.EPSILON));
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
    private int shifter(TokenKernel oldtoken) {
        // Create next generation
        int gen = gss.createGeneration();

        // Create the GSS label to be used for the transitions
        int sym = TableElemRef.encode(TableElemRef.TABLE_TOKEN, oldtoken.getIndex());
        int label = sppf.getSingleNode(sym);

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
    private void executeShift(int gen, int label, Shift shift) {
        int w = gss.findNode(gen, shift.to);
        if (w != -1) {
            // A node for the target state is already in the GSS
            gss.createEdge(w, shift.from, label);
            // Look for the new reductions at this state
            int count = parserAutomaton.getActionsCount(shift.to, nextToken.getTerminalID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(shift.to, nextToken.getTerminalID(), i);
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
            int count = parserAutomaton.getActionsCount(shift.to, nextToken.getTerminalID());
            for (int i = 0; i != count; i++) {
                LRAction action = parserAutomaton.getAction(shift.to, nextToken.getTerminalID(), i);
                if (action.getCode() == LRAction.CODE_SHIFT)
                    shifts.add(new Shift(w, action.getData()));
                else if (action.getCode() == LRAction.CODE_REDUCE) {
                    LRProduction prod = parserAutomaton.getProduction(action.getData());
                    if (prod.getReductionLength() == 0) // Length 0 => reduce from the head
                        reductions.add(new Reduction(w, prod, SPPF.EPSILON));
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
