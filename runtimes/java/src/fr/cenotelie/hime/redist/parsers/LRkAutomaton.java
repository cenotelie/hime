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
 * Represents the LR(k) parsing table and productions
 *
 * @author Laurent Wouters
 */
public class LRkAutomaton {
    /**
     * The number of columns in the LR table
     */
    private final char ncols;
    /**
     * The number of states in the LR table
     */
    private final char nstates;
    /**
     * Map of symbol ID to column index in the LR table
     */
    private final ColumnMap columns;
    /**
     * The contexts information
     */
    private final LRContexts[] contexts;
    /**
     * The LR table
     */
    private final LRAction[] table;
    /**
     * The table of LR productions
     */
    private final LRProduction[] productions;

    /**
     * Gets the number of states in this automaton
     *
     * @return The number of states in this automaton
     */
    public int getStatesCount() {
        return nstates;
    }

    /**
     * Initializes a new automaton from the given binary stream
     *
     * @param input A binary stream
     */
    private LRkAutomaton(BinaryInput input) {
        this.ncols = input.readChar();
        this.nstates = input.readChar();
        int nprod = input.readChar();
        char[] columnsID = new char[ncols];
        for (int i = 0; i != ncols; i++)
            columnsID[i] = input.readChar();
        this.columns = new ColumnMap();
        for (int i = 0; i != ncols; i++)
            this.columns.add(columnsID[i], i);
        this.contexts = new LRContexts[nstates];
        for (int i = 0; i != nstates; i++)
            this.contexts[i] = new LRContexts(input);
        this.table = new LRAction[nstates * ncols];
        for (int i = 0; i != table.length; i++)
            this.table[i] = new LRAction(input);
        this.productions = new LRProduction[nprod];
        for (int i = 0; i != nprod; i++)
            this.productions[i] = new LRProduction(input);
    }

    /**
     * Loads an automaton from a resource
     *
     * @param parserType The type of the parser to load an automaton for
     * @param name       The full name of the resource to load from
     * @return The automaton
     */
    public static LRkAutomaton find(Class parserType, String name) {
        BinaryInput input = new BinaryInput(parserType, name);
        return new LRkAutomaton(input);
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
     * Gets the LR(k) action for the given state and sid
     *
     * @param state State in the LR(k) automaton
     * @param sid   Symbol's ID
     * @return The LR(k) action for the state and sid
     */
    public LRAction getAction(int state, int sid) {
        return table[state * ncols + columns.get(sid)];
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
     * Gets the expected terminals for the specified state
     *
     * @param state     The DFA state
     * @param terminals The possible terminals
     * @return The expected terminals
     */
    public LRExpected getExpected(int state, List<Symbol> terminals) {
        LRExpected result = new LRExpected();
        int offset = ncols * state;
        for (int i = 0; i != terminals.size(); i++) {
            LRAction action = table[offset];
            if (action.getCode() == LRAction.CODE_SHIFT)
                result.getShifts().add(terminals.get(i));
            else if (action.getCode() == LRAction.CODE_REDUCE)
                result.getReductions().add(terminals.get(i));
            offset++;
        }
        return result;
    }
}
