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

package fr.cenotelie.hime.redist;

/**
 * Represents the input of parser with some metadata for line endings
 * All line numbers and column numbers are 1-based.
 * Indices in the content are 0-based.
 *
 * @author Laurent Wouters
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
     * Get the substring corresponding to the specified span
     *
     * @param span A span in this text
     * @return The substring
     */
    String getValue(TextSpan span);

    /**
     * Gets the starting index of the i-th line
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The starting index of the line
     */
    int getLineIndex(int line);

    /**
     * Gets the length of the i-th line
     * The line numbering is 1-based
     *
     * @param line The line number
     * @return The length of the line
     */
    int getLineLength(int line);

    /**
     * Gets the string content of the i-th line
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

    /**
     * Gets the context description for the current text at the specified position
     *
     * @param position The position in this text
     * @return The context description
     */
    TextContext getContext(TextPosition position);

    /**
     * Gets the context description for the current text at the specified position
     *
     * @param position The position in this text
     * @param length   The length of the element to contextualize
     * @return The context description
     */
    TextContext getContext(TextPosition position, int length);

    /**
     * Gets the context description for the current text at the specified span
     *
     * @param span The span of text to contextualize
     * @return The context description
     */
    TextContext getContext(TextSpan span);
}
