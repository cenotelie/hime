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

package fr.cenotelie.hime.redist.utils;

import java.io.IOException;
import java.io.InputStream;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.util.ArrayList;
import java.util.List;
import java.util.MissingResourceException;

/**
 * Represents a binary input for the automata
 * This class is required because we are forced to read the resource from InputStream without previous knowledge
 * of the stream size. That, plus .Net uses little endian whereas Java uses big endian ...
 * This class provides a set of methods to read as a stream of little endian data.
 *
 * @author Laurent Wouters
 */
public class BinaryInput {
    /**
     * The size of the intermediate buffers
     */
    private static final int bufferSize = 1024;

    /**
     * The list of the intermediate buffers for loading the content
     */
    private List<byte[]> content;
    /**
     * The total size of the input
     */
    private int size;
    /**
     * The in-memory buffer to read from in little endian style
     */
    private ByteBuffer buffer;

    /**
     * Gets the length of this input
     *
     * @return The length of this input
     */
    public int length() {
        return size;
    }

    /**
     * Initializes this input
     *
     * @param type The type used to retrieve the resource
     * @param name The name of the resource to load
     */
    public BinaryInput(Class type, String name) {
        try (InputStream stream = type.getResourceAsStream(name)) {
            if (stream == null) {
                String message = String.format("The resource %s cannot be found in the same assembly as %s", name, type.getName());
                throw new MissingResourceException(message, type.getName(), name);
            }
            this.content = new ArrayList<>();
            this.size = 0;
            load(stream);
            this.buffer = ByteBuffer.wrap(buildFullBuffer());
            this.buffer.order(ByteOrder.LITTLE_ENDIAN);
            this.content.clear();
            this.content = null;
        } catch (IOException exception) {
            this.size = 0;
            this.buffer = null;
        }
    }

    /**
     * Loads all the content from the specified input stream
     *
     * @param stream The stream to load from
     * @throws IOException When the reading the stream fails
     */
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

    /**
     * Builds the full (consecutive in memory) buffer
     *
     * @return The full buffer
     */
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

    /**
     * Reads a single byte
     *
     * @return The next data as a byte
     */
    public byte readByte() {
        return buffer.get();
    }

    /**
     * Reads a single char (an unsigned 16 bits integer)
     *
     * @return The next data as a char
     */
    public char readChar() {
        return buffer.getChar();
    }

    /**
     * Reads a single int (signed 32 bits integer)
     *
     * @return The next data as an int
     */
    public int readInt() {
        return buffer.getInt();
    }
}