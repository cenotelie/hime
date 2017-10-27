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

import java.util.HashMap;
import java.util.Map;

/**
 * Represent a map from symbols' IDs to the index of their corresponding column in an LR table.
 * It is optimized for IDs from 0x0000 to 0x01FF (the first 512 symbols) with hope they are the most frequent.
 *
 * @author Laurent Wouters
 */
class ColumnMap {
    /**
     * Cache for ids from 0x00 to 0xFF
     */
    private final int[] cache1;
    /**
     * Cache for ids from 0x100 to 0x1FF
     */
    private int[] cache2;
    /**
     * Hashmap for the other ids
     */
    private Map<Integer, Integer> others;

    /**
     * Initializes the structure
     */
    public ColumnMap() {
        cache1 = new int[256];
    }

    /**
     * Adds a new data in the collection with the given key
     *
     * @param key   The key for the data
     * @param value The data
     */
    public void add(int key, int value) {
        if (key <= 0xFF)
            cache1[key] = value;
        else if (key <= 0x1FF) {
            if (cache2 == null)
                cache2 = new int[256];
            cache2[key - 0x100] = value;
        } else {
            if (others == null)
                others = new HashMap<>();
            others.put(key, value);
        }
    }

    /**
     * Gets the data for the given key
     *
     * @param key The key for the data
     * @return The data corresponding to the key
     */
    public int get(int key) {
        if (key <= 0xFF)
            return cache1[key];
        else if (key <= 0x1FF)
            return cache2[key - 0x100];
        return others.get(key);
    }
}
