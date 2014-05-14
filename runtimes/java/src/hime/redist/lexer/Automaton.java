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

import java.io.*;
import java.nio.IntBuffer;

public class Automaton {
    private int[] table;
    private char[] states;
    private int statesCount;

    public int getStatesCount() { return statesCount; }

    public Automaton(DataInput reader, int length) throws IOException {
        this.statesCount = reader.readInt();
        this.table = new int[this.statesCount];
        for (int i=0; i!=this.statesCount; i++)
            this.table[i] = reader.readInt();


    }

    public static Automaton Find(Class lexerType, String name) {
        InputStream stream = lexerType.getResourceAsStream(name);
        DataInput input = new DataInputStream(stream);

        return  null;
    }
}
