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

package fr.cenotelie.hime.redist.lexer;


import fr.cenotelie.hime.redist.Text;
import fr.cenotelie.hime.redist.TextContext;
import fr.cenotelie.hime.redist.TextPosition;
import fr.cenotelie.hime.redist.TextSpan;

import java.util.Arrays;

/**
 * Represents the base implementation of Text
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 *
 * @author Laurent Wouters
 */
public abstract class BaseText implements Text {
    /**
     * The initial size of the cache of line start indices
     */
    static final int INIT_LINE_COUNT_CACHE_SIZE = 10000;
    /**
     * Cache of the starting indices of each line within the text
     */
    int[] lines;
    /**
     * Index of the next line
     */
    int line;


    /**
     * Gets the character at the specified index
     *
     * @param index Index from the start
     * @return The character at the specified index
     */
    public abstract char getValue(int index);

    /**
     * Gets whether the specified index is after the end of the text represented by this object
     *
     * @param index Index from the start
     * @return <code>true</code> if the index is after the end of the text
     */
    public abstract boolean isEnd(int index);

    /**
     * Finds all the lines in this content
     */
    protected abstract void findLines();

    /**
     * Determines whether [c1, c2] form a line ending sequence
     * Recognized sequences are:
     * [U+000D, U+000A] (this is Windows-style \r \n)
     * [U+????, U+000A] (this is unix style \n)
     * [U+000D, U+????] (this is MacOS style \r, without \n after)
     * Others:
     * [?, U+000B], [?, U+000C], [?, U+0085], [?, U+2028], [?, U+2029]
     *
     * @param c1 First character
     * @param c2 Second character
     * @return true  if this is a line ending sequence
     */
    boolean isLineEnding(char c1, char c2) {
        // other characters
        if (c2 == '\u000B' || c2 == '\u000C' || c2 == '\u0085' || c2 == '\u2028' || c2 == '\u2029')
            return true;
        // matches [\r, \n] [\r, ??] and  [??, \n]
        return c1 == '\r' || c2 == '\n';
    }

    /**
     * Adds a line starting at the specified index
     *
     * @param index An index in the content
     */
    void addLine(int index) {
        if (line >= lines.length)
            lines = Arrays.copyOf(lines, lines.length + INIT_LINE_COUNT_CACHE_SIZE);
        lines[line] = index;
        line++;
    }

    /**
     * Finds the index in the cache of the line at the given input index in the content
     *
     * @param index The index within this content
     * @return The index of the corresponding line in the cache
     */
    private int findLineAt(int index) {
        if (lines == null)
            findLines();
        for (int i = 1; i != line; i++) {
            if (index < lines[i])
                return i - 1;
        }
        return line - 1;
    }

    @Override
    public int getLineCount() {
        if (lines == null)
            findLines();
        return line;
    }

    @Override
    public String getValue(TextSpan span) {
        return getValue(span.getIndex(), span.getLength());
    }

    @Override
    public int getLineIndex(int line) {
        if (lines == null)
            findLines();
        return lines[line - 1];
    }

    @Override
    public String getLineContent(int line) {
        return getValue(getLineIndex(line), getLineLength(line));
    }

    @Override
    public TextPosition getPositionAt(int index) {
        int l = findLineAt(index);
        return new TextPosition(l + 1, index - lines[l] + 1);
    }

    @Override
    public TextContext getContext(TextPosition position) {
        return getContext(position, 1);
    }

    @Override
    public TextContext getContext(TextPosition position, int length) {
        String content = getLineContent(position.getLine());
        if (content.length() == 0)
            return new TextContext("", "^");
        int end = content.length() - 1;
        while (end != -1 && (content.charAt(end) == '\r' || content.charAt(end) == '\n' || content.charAt(end) == '\u000B' || content.charAt(end) == '\u000C' || content.charAt(end) == '\u0085' || content.charAt(end) == '\u2028' || content.charAt(end) == '\u2029'))
            end--;
        int start = 0;
        while (start < end && Character.isWhitespace(content.charAt(start)))
            start++;
        if (position.getColumn() - 1 < start) {
            // the position is in the whitespace prefix ...
            start = 0;
        }
        if (position.getColumn() - 1 > end) {
            // the position is in the trailing line endings ...
            end = content.length() - 1;
        }
        StringBuilder builder = new StringBuilder();
        for (int i = start; i != position.getColumn() - 1; i++)
            builder.append(content.charAt(i) == '\t' ? '\t' : ' ');
        builder.append("^");
        for (int i = 1; i < length; i++)
            builder.append("^");
        return new TextContext(content.substring(start, end + 1), builder.toString());
    }

    @Override
    public TextContext getContext(TextSpan span) {
        TextPosition position = getPositionAt(span.getIndex());
        return getContext(position, span.getLength());
    }
}
