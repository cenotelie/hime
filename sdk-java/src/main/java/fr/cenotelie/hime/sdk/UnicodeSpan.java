/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

package fr.cenotelie.hime.sdk;

/**
 * Represents a range of Unicode characters
 *
 * @author Laurent Wouters
 */
public class UnicodeSpan {
    /**
     * Beginning of the range (included)
     */
    private final int begin;
    /**
     * End of the range (included)
     */
    private final int end;

    /**
     * Gets the first (included) character of the range
     *
     * @return The first (included) character of the range
     */
    public int getBegin() {
        return begin;
    }

    /**
     * Gets the last (included) character of the range
     *
     * @return The last (included) character of the range
     */
    public int getEnd() {
        return end;
    }

    /**
     * Gets the range's length in number of characters
     *
     * @return The range's length in number of characters
     */
    public int getLength() {
        return end - begin + 1;
    }

    /**
     * Gets a value indicating whether this span is entirely in Unicode plane 0
     *
     * @return Whether this span is entirely in Unicode plane 0
     */
    public boolean isPlane0() {
        return begin <= 0xFFFF && end <= 0xFFFF;
    }

    /**
     * Initializes this character span
     *
     * @param begin The first (included) character
     * @param end   The last (included) character
     */
    public UnicodeSpan(int begin, int end) {
        this.begin = begin;
        this.end = end;
    }
}
