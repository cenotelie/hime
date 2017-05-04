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
package fr.cenotelie.hime.redist.parsers;

/**
 * Represents a path in a GSS
 *
 * @author Laurent Wouters
 */
class GSSPath {
    /**
     * The initial size of the label buffer
     */
    private static final int INIT_BUFFER_SIZE = 64;

    /**
     * The last GSS node in this path
     */
    private int last;
    /**
     * The generation containing the last GSS node in this path
     */
    private int generation;
    /**
     * The labels on this GSS path
     */
    private int[] labels;

    /**
     * Gets the final target of this path
     *
     * @return The final target of this path
     */
    public int getLast() {
        return last;
    }

    /**
     * Sets the final target of this path
     *
     * @param state The final target of this path
     */
    public void setLast(int state) {
        last = state;
    }

    /**
     * Gets the generation containing the final target of this path
     *
     * @return The generation containing the final target of this path
     */
    public int getGeneration() {
        return generation;
    }

    /**
     * Sets the generation containing the final target of this path
     *
     * @param generation The generation containing the final target of this path
     */
    public void setGeneration(int generation) {
        this.generation = generation;
    }

    /**
     * Gets the i-th label of the edges traversed by this path
     *
     * @param index Index of the label of the edges traversed by this path
     * @return The i-th label of the edges traversed by this path
     */
    public int get(int index) {
        return labels[index];
    }

    /**
     * Sets the i-th label of the edges traversed by this path
     *
     * @param index Index of the label of the edges traversed by this path
     * @param label The i-th label of the edges traversed by this path
     */
    public void set(int index, int label) {
        labels[index] = label;
    }

    /**
     * Initializes this path
     *
     * @param length The number of labels required for this path
     */
    public GSSPath(int length) {
        this.last = 0;
        this.labels = new int[length < INIT_BUFFER_SIZE ? INIT_BUFFER_SIZE : length];
    }

    /**
     * Ensure the specified length of the label buffer
     *
     * @param length The required length
     */
    public void ensure(int length) {
        if (length > labels.length)
            labels = new int[length];
    }

    /**
     * Copy the content of another path to this one
     *
     * @param path   The path to copy
     * @param length The path's length
     */
    public void copyLabelsFrom(GSSPath path, int length) {
        System.arraycopy(path.labels, 0, this.labels, 0, length);
    }
}
