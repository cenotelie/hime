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

package hime.redist.lexer;

import hime.redist.utils.BinHelper;

import java.io.DataInputStream;
import java.io.IOException;

public class Automaton {
    private int[] table;
    private char[] states;
    private int statesCount;

    public int getStatesCount() {
        return statesCount;
    }

    public Automaton(BinHelper input) {
        this.statesCount = input.readInt();
        this.table = new int[this.statesCount];
        for (int i = 0; i != this.statesCount; i++)
            this.table[i] = input.readInt();
        this.states = new char[(input.length() - 4 - table.length * 4) / 2];
        for (int i = 0; i != this.states.length; i++)
            this.states[i] = input.readChar();
    }

    public static Automaton find(Class lexerType, String name) {
        BinHelper input = new BinHelper(lexerType, name);
        return new Automaton(input);
    }

    public int getOffsetOf(int state) {
        return table[state];
    }

    public int getStateRecognizedTerminal(int offset) {
        return states[offset];
    }

    public boolean isStateDeadEnd(int offset) {
        return (states[offset + 1] == 0);
    }

    public int getStateBulkTransitionsCount(int offset) {
        return states[offset + 2];
    }

    public int getStateCachedTransition(int offset, int value) {
        return states[offset + 3 + value];
    }

    public int getStateBulkTransition(int offset, int value) {
        int count = states[offset + 2];
        offset += 259;
        for (int i = 0; i != count; i++) {
            if (value >= states[offset] && value <= states[offset + 1])
                return states[offset + 2];
            offset += 3;
        }
        return 0xFFFF;
    }
}
