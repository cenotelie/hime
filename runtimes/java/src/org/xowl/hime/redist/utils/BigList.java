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
package org.xowl.hime.redist.utils;

import java.lang.reflect.Array;
import java.util.Arrays;

/**
 * Represents a list of items that is efficient in storage and addition.
 * Items cannot be removed or inserted.
 *
 * @param <T> The type of the stored items
 */
public class BigList<T> {
    /**
     * The number of bits allocated to the lowest part of the index (within a chunk)
     */
    private static final int UPPER_SHIFT = 8;
    /**
     * The size of the chunks
     */
    private static final int CHUNKS_SIZE = 1 << UPPER_SHIFT;
    /**
     * Bit mask for the lowest part of the index (within a chunk)
     */
    private static final int LOWER_MASK = CHUNKS_SIZE - 1;
    /**
     * Initial size of the higer array (pointers to the chunks)
     */
    private static final int INIT_CHUNK_COUNT = CHUNKS_SIZE;

    /**
     * The data
     */
    private T[][] chunks;
    /**
     * The index of the current chunk
     */
    private int chunkIndex;
    /**
     * The index of the next available cell within the current chunk
     */
    private int cellIndex;
    /**
     * The type of the stored elements (T)
     */
    private Class typeElement;

    /**
     * Initializes this list
     *
     * @param type The type of stored elements
     * @param typeArray The type of an array of stored elements
     */
    public BigList(Class<T> type, Class<T[]> typeArray) {
        this.typeElement = type;
        this.chunks = (T[][]) Array.newInstance(typeArray, INIT_CHUNK_COUNT);
        this.chunks[0] = (T[]) Array.newInstance(typeElement, CHUNKS_SIZE);
        this.chunkIndex = 0;
        this.cellIndex = 0;
    }

    /**
     * Gets the size of this list
     *
     * @return The size of this list
     */
    public int size() {
        return (chunkIndex * CHUNKS_SIZE) + cellIndex;
    }

    /**
     * Gets the value of the item at the given index
     *
     * @param index Index of an item
     * @return The value of the item at the given index
     */
    public T get(int index) {
        return chunks[index >> UPPER_SHIFT][index & LOWER_MASK];
    }

    /**
     * Sets the value of the item at the given index
     *
     * @param index Index of an item
     * @param value The value of the item at the given index
     */
    public void set(int index, T value) {
        chunks[index >> UPPER_SHIFT][index & LOWER_MASK] = value;
    }

    /**
     * Adds the given value at the end of this list
     *
     * @param value The value to add
     * @return The index of the value in this list
     */
    public int add(T value) {
        if (cellIndex == CHUNKS_SIZE)
            addChunk();
        chunks[chunkIndex][cellIndex] = value;
        int index = (chunkIndex << UPPER_SHIFT | cellIndex);
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
    public int add(T[] values, int index, int length) {
        int start = size();
        if (length > 0)
            docopy(values, index, length);
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
        int chunk = from >> UPPER_SHIFT;     // The current chunk to copy from
        int cell = from & LOWER_MASK;        // The current starting index in the chunk
        while (cell + count > CHUNKS_SIZE) {
            docopy(chunks[chunk], cell, CHUNKS_SIZE - cell);
            count -= CHUNKS_SIZE - cell;
            chunk++;
            cell = 0;
        }
        docopy(chunks[chunk], cell, count);
        return start;
    }

    /**
     * Copies the given values at the end of this list
     *
     * @param values The values to add
     * @param index  The starting index of the values to store
     * @param length The number of values to store
     */
    private void docopy(T[] values, int index, int length) {
        while (cellIndex + length > CHUNKS_SIZE) {
            int count = CHUNKS_SIZE - cellIndex;
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
        T[] t = (T[]) Array.newInstance(typeElement, CHUNKS_SIZE);
        if (chunkIndex == chunks.length - 1)
            chunks = Arrays.copyOf(chunks, chunks.length + INIT_CHUNK_COUNT);
        chunks[++chunkIndex] = t;
        cellIndex = 0;
    }
}
