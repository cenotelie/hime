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

package org.xowl.hime.redist;

/**
 * Represents the input of parser with some metadata for line endings
 * <p/>
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 */
public interface Text {
    /**
     * Gets the number of lines
     *
     * @return The number of lines
     */
    int getLineCount();

    /**
     * Gets the size in number of characters
     *
     * @return The size in number of characters
     */
    int size();

    /**
     * Gets the substring beginning at the given index with the given length
     *
     * @param index  Index of the substring from the start
     * @param length Length of the substring
     * @return The substring
     */
    String getValue(int index, int length);

    /**
     * Gets the starting index of the i-th line
     * <p/>
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The starting index of the line
     */
    int getLineIndex(int line);

    /**
     * Gets the length of the i-th line
     * <p/>
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The length of the line
     */
    int getLineLength(int line);

    /**
     * Gets the string content of the i-th line
     * <p/>
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The string content of the line
     */
    String getLineContent(int line);

    /**
     * Gets the position at the given index
     *
     * @param index Index from the start
     * @return The position (line and column) at the index
     */
    TextPosition getPositionAt(int index);
}
