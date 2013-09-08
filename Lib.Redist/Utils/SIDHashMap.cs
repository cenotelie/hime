/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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

using System;
using System.Collections.Generic;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Optimized hashmap for data accessed by ids from 0x0000 to 0x01FF
    /// </summary>
    /// <typeparam name="T">The type of data to be stored</typeparam>
    class SIDHashMap<T>
    {
        /// <summary>
        /// Cache for ids from 0x00 to 0xFF
        /// </summary>
        private T[] cache1;
        /// <summary>
        /// Cache for ids from 0x100 to 0x1FF
        /// </summary>
        private T[] cache2;
        /// <summary>
        /// Hashmap for the other ids
        /// </summary>
        private Dictionary<int, T> others;

        /// <summary>
        /// Initializes the structure
        /// </summary>
        public SIDHashMap()
        {
            cache1 = new T[256];
        }

        /// <summary>
        /// Adds a new data in the collection with the given key
        /// </summary>
        /// <param name="key">The key for the data</param>
        /// <param name="value">The data</param>
        public void Add(int key, T value)
        {
            if (key <= 0xFF)
                cache1[key] = value;
            else if (key <= 0x1FF)
            {
                if (cache2 == null)
                    cache2 = new T[256];
                cache2[key - 0x100] = value;
            }
            else
            {
                if (others != null)
                    others = new Dictionary<int, T>();
                others.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the data for the given key
        /// </summary>
        /// <param name="key">The key for the data</param>
        /// <returns>The data corresponding to the key</returns>
        public T this[int key]
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
