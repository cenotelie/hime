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
 * Represents a Unicode code point
 *
 * @author Laurent Wouters
 */
public class UnicodeCodePoint {
    /**
     * The code point value
     */
    private final int value;

    /**
     * Gets the code point value
     *
     * @return The code point value
     */
    public int getValue() {
        return value;
    }

    /**
     * Gets a value indicating whether this codepoint is in Unicode plane 0
     *
     * @return Whether this codepoint is in Unicode plane 0
     */
    public boolean isPlan0() {
        return value <= 0xFFFF;
    }

    /**
     * Initializes the code point
     * The valid Unicode character code points are in the follwing intervals:
     * U+0000 .. U+D7FF
     * U+E000 .. U+FFFF
     * U+10000 .. U+10FFFF
     *
     * @param value The code point value
     */
    public UnicodeCodePoint(int value) {
        if (value < 0 || (value >= 0xD800 && value <= 0xDFFF) || value >= 0x110000)
            throw new IllegalArgumentException("The value is not a valid Unicode character code point");
        this.value = value;
    }

    /**
     * Gets the UTF-16 encoding of this code point
     * No check is done in this method because the the value is assumed valid after construction
     *
     * @return The UTF-16 encoding of this code point
     */
    public char[] getUTF16() {
        if (value <= 0xFFFF) {
            // plane 0
            return new char[]{(char) value};
        }
        int temp = value - 0x10000;
        int lead = (temp >> 10) + 0xD800;
        int trail = (temp & 0x03FF) + 0xDC00;
        return new char[]{(char) lead, (char) trail};
    }

    @Override
    public boolean equals(Object o) {
        return (o instanceof UnicodeCodePoint) && ((UnicodeCodePoint) o).value == this.value;
    }

    /**
     * Returns the sort order of the current instance compared to the specified object
     *
     * @param left  The left code point to compare
     * @param right The right code point to compare
     * @return The sort order of the current instance compared to the specified object
     */
    public static int compare(UnicodeCodePoint left, UnicodeCodePoint right) {
        return Integer.compare(left.value, right.value);
    }
}
