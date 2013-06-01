using System;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represent a map from symbols' IDs to the index of their corresponding column in an LR table.
    /// It is optimized for IDs from 0x0000 to 0x01FF (the first 512 symbols) with hope they are the most frequent.
    /// </summary>
    class ColumnMap
    {
        /// <summary>
        /// Cache for ids from 0x00 to 0xFF
        /// </summary>
        private int[] cache1;
        /// <summary>
        /// Cache for ids from 0x100 to 0x1FF
        /// </summary>
        private int[] cache2;
        /// <summary>
        /// Hashmap for the other ids
        /// </summary>
        private Dictionary<int, int> others;

        /// <summary>
        /// Initializes the structure
        /// </summary>
        public ColumnMap()
        {
            cache1 = new int[256];
        }

        /// <summary>
        /// Adds a new data in the collection with the given key
        /// </summary>
        /// <param name="key">The key for the data</param>
        /// <param name="value">The data</param>
        public void Add(int key, int value)
        {
            if (key <= 0xFF)
                cache1[key] = value;
            else if (key <= 0x1FF)
            {
                if (cache2 == null)
                    cache2 = new int[256];
                cache2[key - 0x100] = value;
            }
            else
            {
                if (others != null)
                    others = new Dictionary<int, int>();
                others.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the data for the given key
        /// </summary>
        /// <param name="key">The key for the data</param>
        /// <returns>The data corresponding to the key</returns>
        public int this[int key]
        {
            get
            {
                if (key <= 0xFF)
                    return cache1[key];
                else if (key <= 0x1FF)
                    return cache2[key - 0x100];
                return others[key];
            }
        }
    }
}
