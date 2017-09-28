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
 * Represents a span of text in an input as a starting index and length
 *
 * @author Laurent Wouters
 */
public class TextSpan {
    /**
     * The starting index
     */
    private final int index;
    /**
     * The length
     */
    private final int length;

    /**
     * Gets the starting index of this span
     *
     * @return The starting index of this span
     */
    public int getIndex() {
        return index;
    }

    /**
     * Gets the length of this span
     *
     * @return The length of this span
     */
    public int getLength() {
        return length;
    }

    /**
     * Initializes this span
     *
     * @param index  The span's index
     * @param length The span's length
     */
    public TextSpan(int index, int length) {
        this.index = index;
        this.length = length;
    }

    @Override
    public String toString() {
        return "@" + index + "+" + length;
    }
}
