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

import fr.cenotelie.hime.redist.Symbol;
import fr.cenotelie.hime.redist.utils.BinaryInput;

import java.util.List;

/**
 * Represents the RNGLR parsing table and productions
 *
 * @author Laurent Wouters
 */
public class RNGLRAutomaton {
    /**
     * Represents a cell in a RNGLR parse table
     */
    private static class Cell {
        /**
         * The number of actions in this cell
         */
        public final char count;
        /**
         * Index of the cell's data
         */
        public final int index;

        /**
         * Loads this cell from the specified input
         *
         * @param input The input to load from
         */
        public Cell(BinaryInput input) {
            this.count = input.readChar();
            this.index = input.readInt();
        }
    }

    /**
     * Index of the axiom variable
     */
    private final int axiom;
    /**
     * The number of columns in the LR table
     */
    private final char ncols;
    /**
     * The number of states in the automaton
     */
    private final int nstates;
    /**
     * Map of symbol ID to column index in the LR table
     */
    private final ColumnMap columns;
    /**
     * The contexts information
     */
    private final LRContexts[] contexts;
    /**
     * The RNGLR table
     */
    private final Cell[] table;
    /**
     * The action table
     */
    private final LRAction[] actions;
    /**
     * The table of LR productions
     */
    private final LRProduction[] productions;
    /**
     * The table of nullable variables
     */
    private final char[] nullables;

    /**
     * Gets the index of the axiom
     *
     * @return The index of the axiom
     */
    public int getAxiom() {
        return axiom;
    }

    /**
     * Gets the number of states in the RNGLR table
     *
     * @return The number of states in the RNGLR table
     */
    public int getStatesCount() {
        return nstates;
    }

    /**
     * Initializes a new automaton from the given binary stream
     *
     * @param input The binary stream to load from
     */
    private RNGLRAutomaton(BinaryInput input) {
        this.axiom = input.readChar();
        this.ncols = input.readChar();
        this.nstates = input.readChar();
        int nactions = input.readInt();
        int nprod = input.readChar();
        int nnprod = input.readChar();
        char[] columnsID = new char[ncols];
        for (int i = 0; i != ncols; i++)
            columnsID[i] = input.readChar();
        this.columns = new ColumnMap();
        for (int i = 0; i != ncols; i++)
            this.columns.add(columnsID[i], i);
        contexts = new LRContexts[nstates];
        for (int i = 0; i != nstates; i++)
            contexts[i] = new LRContexts(input);
        this.table = new Cell[nstates * ncols];
        for (int i = 0; i != table.length; i++)
            this.table[i] = new Cell(input);
        this.actions = new LRAction[nactions];
        for (int i = 0; i != actions.length; i++)
            this.actions[i] = new LRAction(input);
        this.productions = new LRProduction[nprod];
        for (int i = 0; i != nprod; i++)
            this.productions[i] = new LRProduction(input);
        this.nullables = new char[nnprod];
        for (int i = 0; i != nullables.length; i++)
            this.nullables[i] = input.readChar();
    }

    /**
     * Loads an automaton from a resource
     *
     * @param parserType The type of the parser to load an automaton for
     * @param name       The full name of the resource to load from
     * @return The automaton
     */
    public static RNGLRAutomaton find(Class parserType, String name) {
        BinaryInput input = new BinaryInput(parserType, name);
        return new RNGLRAutomaton(input);
    }

    /**
     * Gets the contexts opened by the specified state
     *
     * @param state State in the LR(k) automaton
     * @return The opened contexts
     */
    public LRContexts getContexts(int state) {
        return contexts[state];
    }

    /**
     * Gets the number of GLR actions for the given state and sid
     *
     * @param state An automaton's state
     * @param sid   A symbol ID
     * @return The number of GLR actions
     */
    public int getActionsCount(int state, int sid) {
        return table[state * ncols + columns.get(sid)].count;
    }

    /**
     * Gets the i-th GLR action for the given state and sid
     *
     * @param state An automaton's state
     * @param sid   A symbol ID
     * @param index The action index
     * @return The GLR action
     */
    public LRAction getAction(int state, int sid, int index) {
        return actions[table[state * ncols + columns.get(sid)].index + index];
    }

    /**
     * Gets the production at the given index
     *
     * @param index Production's index
     * @return The production a the given index
     */
    public LRProduction getProduction(int index) {
        return productions[index];
    }

    /**
     * Gets the production for the nullable variable with the given index
     *
     * @param index Index of a nullable variable
     * @return The production, or null if the variable is not nullable
     */
    public LRProduction getNullableProduction(int index) {
        int temp = nullables[index];
        if (temp == 0xFFFF)
            return null;
        return productions[temp];
    }

    /**
     * Determine whether the given state is the accepting state
     *
     * @param state An automaton's state
     * @return True if the state is the accepting state, false otherwise
     */
    public boolean isAcceptingState(int state) {
        return table[state * ncols].count == 1 && (actions[table[state * ncols].index].getCode() == LRAction.CODE_ACCEPT);
    }

    /**
     * Gets the expected terminals for the specified state
     *
     * @param state     The DFA state
     * @param terminals The possible terminals
     * @return The expected terminals
     */
    public LRExpected getExpected(int state, List<Symbol> terminals) {
        LRExpected result = new LRExpected();
        for (int i = 0; i != terminals.size(); i++) {
            Cell cell = table[state * ncols + i];
            for (int j = 0; j != cell.count; j++) {
                LRAction action = actions[cell.index + j];
                if (action.getCode() == LRAction.CODE_SHIFT)
                    result.addUniqueShift(terminals.get(i));
                else if (action.getCode() == LRAction.CODE_REDUCE)
                    result.addUniqueReduction(terminals.get(i));
            }
        }
        return result;
    }
}
