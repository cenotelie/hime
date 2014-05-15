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

import hime.redist.Symbol;
import hime.redist.TextPosition;
import hime.redist.Token;
import hime.redist.TokenizedText;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class PrefetchedText implements TokenizedText {
    private static final int initLineCount = 10000;

    private static class Cell {
        public int terminal;
        public int start;
        public int length;

        public Cell(int terminal, int start, int length) {
            this.terminal = terminal;
            this.start = start;
            this.length = length;
        }
    }

    private String content;
    private int[] lines;
    private int line;
    private List<Symbol> terminals;
    private List<Cell> cells;

    public PrefetchedText(List<Symbol> terminals, String content) {
        this.content = content;
        this.terminals = terminals;
        this.cells = new ArrayList<Cell>();
    }

    public void findLines() {
        this.lines = new int[initLineCount];
        this.lines[0] = 0;
        this.line = 1;
        char c1 = '\0';
        char c2 = '\0';
        for (int i = 0; i != content.length(); i++) {
            // is c1 c2 a line ending sequence?
            if (isLineEnding(c1, c2)) {
                // are we late to detect MacOS style?
                if (c1 == '\r' && c2 != '\n')
                    addLine(i - 1);
                else
                    addLine(i);
            }
            c1 = c2;
            c2 = content.charAt(i);
        }
        lines[line] = content.length();
    }

    private boolean isLineEnding(char c1, char c2) {
        // other characters
        if (c2 == '\u000B' || c2 == '\u000C' || c2 == '\u0085' || c2 == '\u2028' || c2 == '\u2029')
            return true;
        // matches [\r, \n] [\r, ??] and  [??, \n]
        if (c1 == '\r' || c2 == '\n')
            return true;
        return false;
    }

    private void addLine(int index) {
        if (line + 1 >= lines.length)
            lines = Arrays.copyOf(lines, lines.length + initLineCount);
        lines[line++] = index;
    }

    public void addToken(int terminal, int start, int length) {
        cells.add(new Cell(terminal, start, length));
    }

    public Token getTokenAt(int index) {
        Cell cell = cells.get(index);
        return new Token(terminals.get(cell.terminal).getID(), index);
    }


    public int getLineCount() {
        if (lines == null)
            findLines();
        return line;
    }

    public int size() {
        return content.length();
    }

    public String getValue(int index, int length) {
        if (length == 0)
            return "";
        return content.substring(index, index + length);
    }

    public int getLineIndex(int line) {
        if (lines == null)
            findLines();
        return lines[line - 1];
    }

    public int getLineLength(int line) {
        if (lines == null)
            findLines();
        return (lines[line] - lines[line - 1]);
    }

    public String getLineContent(int line) {
        return getValue(getLineIndex(line), getLineLength(line));
    }

    public TextPosition getPositionAt(int index) {
        int l = findLineAt(index);
        return new TextPosition(l + 1, index - lines[l]);
    }

    private int findLineAt(int index) {
        if (lines == null)
            findLines();
        int start = 0;
        int end = line - 1;
        while (true) {
            if (end == start || end == start + 1)
                return start;
            int m = (start + end) / 2;
            int v = lines[m];
            if (index == v)
                return m;
            if (index < v)
                end = m;
            else
                start = m;
        }
    }

    public int getTokenCount() {
        return cells.size();
    }

    public Symbol at(int index) {
        Cell cell = cells.get(index);
        Symbol terminal = terminals.get(cell.terminal);
        String value = getValue(cell.start, cell.length);
        return new Symbol(terminal.getID(), terminal.getName(), value);
    }

    public TextPosition getPositionOf(int tokenIndex) {
        Cell cell = cells.get(tokenIndex);
        return getPositionAt(cell.start);
    }
}
