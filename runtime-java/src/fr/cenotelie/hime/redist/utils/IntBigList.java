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
package fr.cenotelie.hime.redist.utils;

import java.util.Arrays;

/**
 * Represents a list of integers that is efficient in storage and addition.
 * Items cannot be removed or inserted.
 *
 * @author Laurent Wouters
 */
public class IntBigList {
    /**
     * The number of bits allocated to the lowest part of the index (within a chunk)
     */
    private static final int upperShift = 8;
    /**
     * The size of the chunks
     */
    private static final int chunksSize = 1 << upperShift;
    /**
     * Bit mask for the lowest part of the index (within a chunk)
     */
    private static final int lowerMask = chunksSize - 1;
    /**
     * Initial size of the higer array (pointers to the chunks)
     */
    private static final int initChunkCount = chunksSize;

    /**
     * The data
     */
    private int[][] chunks;
    /**
     * The index of the current chunk
     */
    private int chunkIndex;
    /**
     * The index of the next available cell within the current chunk
     */
    private int cellIndex;

    /**
     * Initializes this list
     */
    public IntBigList() {
        this.chunks = new int[initChunkCount][];
        this.chunks[0] = new int[chunksSize];
        this.chunkIndex = 0;
        this.cellIndex = 0;
    }

    /**
     * Gets the size of this list
     *
     * @return The size of this list
     */
    public int size() {
        return (chunkIndex << upperShift) + cellIndex;
    }

    /**
     * Gets the value of the item at the given index
     *
     * @param index Index of an item
     * @return The value of the item at the given index
     */
    public int get(int index) {
        return chunks[index >> upperShift][index & lowerMask];
    }

    /**
     * Sets the value of the item at the given index
     *
     * @param index Index of an item
     * @param value The value of the item at the given index
     */
    public void set(int index, int value) {
        chunks[index >> upperShift][index & lowerMask] = value;
    }

    /**
     * Adds the given value at the end of this list
     *
     * @param value The value to add
     * @return The index of the value in this list
     */
    public int add(int value) {
        if (cellIndex == chunksSize)
            addChunk();
        chunks[chunkIndex][cellIndex] = value;
        int index = (chunkIndex << upperShift | cellIndex);
        cellIndex++;
        return index;
    }

    /**
     * Copies the given values at the end of this list
     *
     * @param values The values to add
     * @param index  The starting index of the values to store
     * @param length The number of values to store
     * @return The index within this list at which the values have been added
     */
    public int add(int[] values, int index, int length) {
        int start = size();
        if (length > 0)
            doCopy(values, index, length);
        return start;
    }

    /**
     * Copies the values from the given index at the end of the list
     *
     * @param from  The index to start copy from
     * @param count The number of items to copy
     * @return The index within this list at which the values have been copied to
     */
    public int duplicate(int from, int count) {
        int start = size();
        if (count <= 0)
            return start;
        int chunk = from >> upperShift;     // The current chunk to copy from
        int cell = from & lowerMask;        // The current starting index in the chunk
        while (cell + count > chunksSize) {
            doCopy(chunks[chunk], cell, chunksSize - cell);
            count -= chunksSize - cell;
            chunk++;
            cell = 0;
        }
        doCopy(chunks[chunk], cell, count);
        return start;
    }

    /**
     * Copies the given values at the end of this list
     *
     * @param values The values to add
     * @param index  The starting index of the values to store
     * @param length The number of values to store
     */
    private void doCopy(int[] values, int index, int length) {
        while (cellIndex + length > chunksSize) {
            int count = chunksSize - cellIndex;
            if (count == 0) {
                addChunk();
                continue;
            }
            System.arraycopy(values, index, chunks[chunkIndex], cellIndex, count);
            index += count;
            length -= count;
            addChunk();
        }
        System.arraycopy(values, index, chunks[chunkIndex], cellIndex, length);
        cellIndex += length;
    }

    /**
     * Adds a new (empty) chunk of cells
     */
    private void addChunk() {
        int[] t = new int[chunksSize];
        if (chunkIndex == chunks.length - 1)
            chunks = Arrays.copyOf(chunks, chunks.length + initChunkCount);
        chunks[++chunkIndex] = t;
        cellIndex = 0;
    }
}
