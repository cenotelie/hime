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

package org.xowl.hime.redist.lexer;

import org.xowl.hime.redist.*;
import org.xowl.hime.redist.utils.BigList;

import java.util.Arrays;
import java.util.List;

/**
 * Stores the full content of an input lexer
 *
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 */
public class PrefetchedText implements TokenizedText {
    /**
     * The initiaal size of the cache of line start indices
     */
    private static final int initLineCount = 10000;

    /**
     * Represents the metadata of a token
     */
    private static class Cell {
        /**
         * The terminal's index
         */
        public int terminal;
        /**
         * Start index of the text
         */
        public int start;
        /**
         * Length of the token
         */
        public int length;

        /**
         * Initializes this cell
         *
         * @param terminal The terminal's index
         * @param start    Start index of the text
         * @param length   Length of the token
         */
        public Cell(int terminal, int start, int length) {
            this.terminal = terminal;
            this.start = start;
            this.length = length;
        }
    }

    /**
     * The full content of the input
     */
    private String content;
    /**
     * Cache of the starting indices of each line within the text
     */
    private int[] lines;
    /**
     * Index of the next line
     */
    private int line;
    /**
     * The terminal symbols matched in this content
     */
    private List<Symbol> terminals;
    /**
     * The token data in this content
     */
    private BigList<Cell> cells;

    /**
     * Initializes this text
     *
     * @param terminals The terminal symbols
     * @param content   The full lexer's input as a string
     */
    public PrefetchedText(List<Symbol> terminals, String content) {
        this.content = content;
        this.terminals = terminals;
        this.cells = new BigList<Cell>(Cell.class, Cell[].class);
    }

    /**
     * Finds all the lines in this content
     */
    public void findLines() {
        this.lines = new int[initLineCount];
        this.lines[0] = 0;
        this.line = 1;
        char c1 = '\0';
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
     * Determines whether [c1, c2] form a line ending sequence
     *
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
    private boolean isLineEnding(char c1, char c2) {
        // other characters
        if (c2 == '\u000B' || c2 == '\u000C' || c2 == '\u0085' || c2 == '\u2028' || c2 == '\u2029')
            return true;
        // matches [\r, \n] [\r, ??] and  [??, \n]
        if (c1 == '\r' || c2 == '\n')
            return true;
        return false;
    }

    /**
     * Adds a line starting at the specified index
     *
     * @param index An index in the content
     */
    private void addLine(int index) {
        if (line >= lines.length)
            lines = Arrays.copyOf(lines, lines.length + initLineCount);
        lines[line] = index;
        line++;
    }

    /**
     * Adds a detected token in this text
     *
     * @param terminal Index of the matched terminal
     * @param start    Start index in the text
     * @param length   Length of the token
     */
    public void addToken(int terminal, int start, int length) {
        cells.add(new Cell(terminal, start, length));
    }

    /**
     * Gets the token at the specified index
     *
     * @param index A token's index
     * @return The token at the specified index
     */
    public Token getTokenAt(int index) {
        Cell cell = cells.get(index);
        return new Token(terminals.get(cell.terminal).getID(), index);
    }


    /**
     * Gets the number of lines
     *
     * @return The number of lines
     */
    public int getLineCount() {
        if (lines == null)
            findLines();
        return line;
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
     * Gets the starting index of the i-th line
     *
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The starting index of the line
     */
    public int getLineIndex(int line) {
        if (lines == null)
            findLines();
        return lines[line - 1];
    }

    /**
     * Gets the length of the i-th line
     *
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

    /**
     * Gets the string content of the i-th line
     *
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The string content of the line
     */
    public String getLineContent(int line) {
        return getValue(getLineIndex(line), getLineLength(line));
    }

    /**
     * Gets the position at the given index
     *
     * @param index Index from the start
     * @return The position (line and column) at the index
     */
    public TextPosition getPositionAt(int index) {
        int l = findLineAt(index);
        return new TextPosition(l + 1, index - lines[l] + 1);
    }

    /**
     * Gets the context description for the current text at the specified position
     *
     * @param position The position in this text
     * @return The context description
     */
    public Context getContext(TextPosition position) {
        String content = getLineContent(position.getLine());
        if (content.length() == 0)
            return new Context("", "^");
        int end = content.length() - 1;
        while (end != -1 && (content.charAt(end) == '\n' || content.charAt(end) == '\r'))
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
        for (int i=start; i!=position.getColumn() - 1; i++)
            builder.append(content.charAt(i) == '\t' ? '\t' : ' ');
        builder.append("^");
        return new Context( content.substring(start, end + 1), builder.toString());
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
        for (int i=1; i!=line; i++) {
            if (index < lines[i]) {
                return i - 1;
            }
        }
        return line - 1;
    }

    /**
     * Gets the number of tokens in this text
     *
     * @return The number of tokens in this text
     */
    public int getTokenCount() {
        return cells.size();
    }

    /**
     * Gets the token at the given index
     *
     * @param index An index
     * @return The token
     */
    public Symbol at(int index) {
        Cell cell = cells.get(index);
        Symbol terminal = terminals.get(cell.terminal);
        String value = getValue(cell.start, cell.length);
        return new Symbol(terminal.getID(), terminal.getName(), value);
    }

    /**
     * Gets the position of the token at the given index
     *
     * @param tokenIndex The index of a token
     * @return The position (line and column) of the token
     */
    public TextPosition getPositionOf(int tokenIndex) {
        Cell cell = cells.get(tokenIndex);
        return getPositionAt(cell.start);
    }
}
