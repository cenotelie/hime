/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters
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
 ******************************************************************************/

package org.xowl.hime.redist.lexer;

import java.io.IOException;
import java.io.InputStreamReader;
import java.util.Arrays;

/**
 * Text provider that uses a stream as a backend
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 *
 * @author Laurent Wouters
 */
public class StreamingText extends BaseText {
    /**
     * The size of text block
     */
    private static final int BLOCK_SIZE = 256;
    /**
     * Shift for the content upper index
     */
    private static final int UPPER_SHIFT = 8;
    /**
     * Mask for the content lower index
     */
    private static final int LOWER_MASK = 0xFF;

    /**
     * The input to use
     */
    private final InputStreamReader input;
    /**
     * The content read so far
     */
    private char[][] content;
    /**
     * Block index of the next available character slot in the content
     */
    private int contentUpperIndex;
    /**
     * Index within a block of the next available character slot in the content
     */
    private int contentLowerIndex;
    /**
     * A buffer for reading text
     */
    private char[] buffer;
    /**
     * Whether the complete input has been read
     */
    private boolean atEnd;

    /**
     * Initializes this text
     *
     * @param input The input text
     */
    public StreamingText(InputStreamReader input) {
        this.input = input;
        this.content = new char[BLOCK_SIZE][];
        this.contentUpperIndex = 0;
        this.contentLowerIndex = 0;
        this.buffer = new char[BLOCK_SIZE];
        this.atEnd = false;
    }

    /**
     * Reads the input so as to make the specified index available
     *
     * @param index An index from the start
     */
    private void makeAvailable(int index) {
        if (atEnd)
            return;
        while (index > ((contentUpperIndex << UPPER_SHIFT) + contentLowerIndex)) {
            try {
                int read = input.read(buffer, 0, BLOCK_SIZE);
                if (read <= 0) {
                    atEnd = true;
                    return;
                }
                addContent(read);
            } catch (IOException ex) {
                atEnd = true;
                return;
            }
        }
    }

    /**
     * Adds the specified number of characters from the buffer to the content
     *
     * @param size The number of characters to add
     */
    private void addContent(int size) {
        // assert that size <= BLOCK_SIZE
        if (contentLowerIndex + size <= BLOCK_SIZE) {
            // everything fits into the current content block
            addContent(0, size);
        } else {
            // we need to split ...
            int count = size - contentLowerIndex;
            addContent(0, count);
            addContent(count, size - count);
        }
    }

    /**
     * Adds the specified number of characters from the buffer to the content
     *
     * @param start The starting index in the buffer
     * @param size  The number of characters to add
     */
    private void addContent(int start, int size) {
        if (contentUpperIndex == content.length)
            content = Arrays.copyOf(content, content.length + BLOCK_SIZE);
        char[] target = content[contentUpperIndex];
        if (target == null) {
            target = new char[BLOCK_SIZE];
            content[contentUpperIndex] = target;
        }
        System.arraycopy(buffer, start, target, contentLowerIndex, size);
        contentLowerIndex += size;
        if (contentLowerIndex >= BLOCK_SIZE) {
            contentUpperIndex++;
            contentLowerIndex -= BLOCK_SIZE;
        }
    }

    /**
     * Gets the character at the specified index
     *
     * @param index Index from the start
     * @return The character at the specified index
     */
    @Override
    public char getValue(int index) {
        makeAvailable(index);
        return content[index >> UPPER_SHIFT][index & LOWER_MASK];
    }

    /**
     * Gets whether the specified index is after the end of the text represented by this object
     *
     * @param index Index from the start
     * @return <code>true</code> if the index is after the end of the text
     */
    @Override
    public boolean isEnd(int index) {
        makeAvailable(index + 1);
        return (index >= size());
    }

    /**
     * Finds all the lines in this content
     */
    @Override
    protected void findLines() {
        if (!atEnd)
            makeAvailable(Integer.MAX_VALUE);
        this.lines = new int[INIT_LINE_COUNT_CACHE_SIZE];
        this.lines[0] = 0;
        this.line = 1;
        char c1;
        char c2 = '\0';
        for (int i = 0; i != size(); i++) {
            c1 = c2;
            c2 = content[i >> UPPER_SHIFT][i & LOWER_MASK];
            // is c1 c2 a line ending sequence?
            if (isLineEnding(c1, c2)) {
                // are we late to detect MacOS style?
                if (c1 == '\r' && c2 != '\n')
                    addLine(i);
                else
                    addLine(i + 1);
            }
        }
    }

    /**
     * Gets the size in number of characters
     *
     * @return The size in number of characters
     */
    public int size() {
        return (contentUpperIndex << UPPER_SHIFT) + contentLowerIndex;
    }

    /**
     * Gets the substring beginning at the given index with the given length
     *
     * @param index  Index of the substring from the start
     * @param length Length of the substring
     * @return The substring
     */
    public String getValue(int index, int length) {
        if (length == 0)
            return "";
        if (buffer.length < length)
            buffer = new char[length];
        int start = 0;
        int count = length;
        int indexUpper = index >> UPPER_SHIFT;
        int indexLower = index & LOWER_MASK;
        while (indexLower + count >= BLOCK_SIZE) {
            // while we can copy chunks
            int copyLength = BLOCK_SIZE - indexLower;
            System.arraycopy(content[indexUpper], indexLower, buffer, start, copyLength);
            count -= copyLength;
            start += copyLength;
            indexUpper++;
            indexLower = 0;
        }
        if (count > 0)
            System.arraycopy(content[indexUpper], indexLower, buffer, start, count);
        return new String(buffer, 0, length);
    }

    /**
     * Gets the length of the i-th line
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The length of the line
     */
    public int getLineLength(int line) {
        if (lines == null)
            findLines();
        if (line == this.line)
            return (size() - lines[this.line - 1]);
        return (lines[line] - lines[line - 1]);
    }
}
