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

package hime.redist.utils;

import java.io.ByteArrayInputStream;
import java.io.DataInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.List;

public class BinHelper {
    private static final int bufferSize = 1024;

    private List<byte[]> content;
    private int size;
    private ByteBuffer buffer;

    public int length() {
        return size;
    }

    public BinHelper(Class type, String name) {
        InputStream stream = type.getResourceAsStream(name);
        this.content = new ArrayList<byte[]>();
        this.size = 0;
        try {
            load(stream);
        } catch (IOException ex) {
        }
        try {
            stream.close();
        } catch (IOException ex) {
        }
        this.buffer = ByteBuffer.wrap(buildFullBuffer());
        this.buffer.order(ByteOrder.LITTLE_ENDIAN);
        this.content.clear();
        this.content = null;
    }

    private void load(InputStream stream) throws IOException {
        byte[] buffer = new byte[bufferSize];
        int length = 0;
        int read = 0;
        while (read != -1) {
            read = stream.read(buffer, length, bufferSize - length);
            if (read == -1) {
                if (length != 0) {
                    content.add(buffer);
                    size += length;
                    return;
                }
            }
            length += read;
            if (length == bufferSize) {
                content.add(buffer);
                size += bufferSize;
                buffer = new byte[bufferSize];
                length = 0;
                read = 0;
            }
        }
    }

    private byte[] buildFullBuffer() {
        byte[] buffer = new byte[size];
        int current = 0;
        for (int i = 0; i != content.size(); i++) {
            if (i == content.size() - 1) {
                // the last buffer
                System.arraycopy(content.get(i), 0, buffer, current, size - current);
            } else {
                System.arraycopy(content.get(i), 0, buffer, current, bufferSize);
                current += bufferSize;
            }
        }
        return buffer;
    }

    public char readChar() { return buffer.getChar(); }

    public int readInt() { return buffer.getInt(); }
}