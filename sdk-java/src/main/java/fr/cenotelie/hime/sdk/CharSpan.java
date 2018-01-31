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
 ******************************************************************************/

package fr.cenotelie.hime.sdk;

/**
 * Represents a range of characters
 *
 * @author Laurent Wouters
 */
public class CharSpan implements Comparable<CharSpan> {
    /**
     * Constant value for an invalid value
     */
    public static final CharSpan NULL = new CharSpan((char) 1, (char) 0);

    /**
     * Beginning of the range (included)
     */
    private final char spanBegin;
    /**
     * End of the range (included)
     */
    private final char spanEnd;

    /**
     * Gets the first (included) character of the range
     *
     * @return The first (included) character of the range
     */
    public char getSpanBegin() {
        return spanBegin;
    }

    /**
     * Gets the last (included) character of the range
     *
     * @return The last (included) character of the range
     */
    public char getSpanEnd() {
        return spanEnd;
    }

    /**
     * Gets the range's length in number of characters
     *
     * @return The range's length in number of characters
     */
    public int getLength() {
        return spanEnd - spanBegin + 1;
    }

    /**
     * Initializes this character span
     *
     * @param begin The first (included) character
     * @param end   The last (included) character
     */
    public CharSpan(char begin, char end) {
        this.spanBegin = begin;
        this.spanEnd = end;
    }

    /**
     * Gets the intersection between two spans
     *
     * @param left  The left span
     * @param right The right span
     * @return The intersection
     */
    public static CharSpan intersect(CharSpan left, CharSpan right) {
        if (left.spanBegin < right.spanBegin) {
            if (left.spanEnd < right.spanBegin)
                return NULL;
            if (left.spanEnd < right.spanEnd)
                return new CharSpan(right.spanBegin, left.spanEnd);
            return new CharSpan(right.spanBegin, right.spanEnd);
        } else {
            if (right.spanEnd < left.spanBegin)
                return NULL;
            if (right.spanEnd < left.spanEnd)
                return new CharSpan(left.spanBegin, right.spanEnd);
            return new CharSpan(left.spanBegin, left.spanEnd);
        }
    }

    @Override
    public boolean equals(Object o) {
        if (o instanceof CharSpan) {
            CharSpan span = (CharSpan) o;
            return this.spanBegin == span.spanBegin && this.spanEnd == span.spanEnd;
        }
        return false;
    }

    @Override
    public int compareTo(CharSpan right) {
        return compare(this, right);
    }

    /**
     * The result of a split operation
     */
    public static class SplitResult {
        /**
         * The first part of the resulting split
         */
        public final CharSpan first;
        /**
         * The second part of the resulting split
         */
        public final CharSpan rest;

        /**
         * Initializes this result
         *
         * @param first The first part of the resulting split
         * @param rest  The second part of the resulting split
         */
        public SplitResult(CharSpan first, CharSpan rest) {
            this.first = first;
            this.rest = rest;
        }
    }

    /**
     * Splits the original span with the given splitter
     *
     * @param original The span to be split
     * @param splitter The splitter
     * @return The result
     */
    public static SplitResult split(CharSpan original, CharSpan splitter) {
        if (original.spanBegin == splitter.spanBegin) {
            if (original.spanEnd == splitter.spanEnd)
                return new SplitResult(NULL, NULL);
            return new SplitResult(new CharSpan((char) (splitter.spanEnd + 1), original.spanEnd), NULL);
        }
        if (original.spanEnd == splitter.spanEnd) {
            return new SplitResult(new CharSpan(original.spanBegin, (char) (splitter.spanBegin - 1)), NULL);
        }
        return new SplitResult(
                new CharSpan(original.spanBegin, (char) (splitter.spanBegin - 1)),
                new CharSpan((char) (splitter.spanEnd + 1), original.spanEnd)
        );
    }

    /**
     * Compares the left and right spans for an increasing order sort
     *
     * @param left  The left span
     * @param right The right span
     * @return The order between left and right
     */
    public static int compare(CharSpan left, CharSpan right) {
        return Integer.compare(left.spanBegin, right.spanBegin);
    }

    /**
     * Compares the left and right spans for an decreasing order sort
     *
     * @param left  The left span
     * @param right The right span
     * @return The order between left and right
     */
    public static int compareReverse(CharSpan left, CharSpan right) {
        return Integer.compare(right.spanBegin, left.spanBegin);
    }
}
