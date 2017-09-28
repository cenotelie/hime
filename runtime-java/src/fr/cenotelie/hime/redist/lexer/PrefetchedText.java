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

/**
 * Text provider that fetches and stores the full content of an input lexer
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 *
 * @author Laurent Wouters
 */
public class PrefetchedText extends BaseText {
    /**
     * The full content of the input
     */
    private final String content;

    /**
     * Initializes this text
     *
     * @param content The full lexer's input as a string
     */
    public PrefetchedText(String content) {
        this.content = content;
    }

    /**
     * Gets the character at the specified index
     *
     * @param index Index from the start
     * @return The character at the specified index
     */
    @Override
    public char getValue(int index) {
        return content.charAt(index);
    }

    /**
     * Gets whether the specified index is after the end of the text represented by this object
     *
     * @param index Index from the start
     * @return <code>true</code> if the index is after the end of the text
     */
    @Override
    public boolean isEnd(int index) {
        return (index >= content.length());
    }

    /**
     * Finds all the lines in this content
     */
    @Override
    protected void findLines() {
        this.lines = new int[INIT_LINE_COUNT_CACHE_SIZE];
        this.lines[0] = 0;
        this.line = 1;
        char c1;
        char c2 = '\0';
        for (int i = 0; i != content.length(); i++) {
            c1 = c2;
            c2 = content.charAt(i);
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
        return content.length();
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
        return content.substring(index, index + length);
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
            return (content.length() - lines[this.line - 1]);
        return (lines[line] - lines[line - 1]);
    }
}
