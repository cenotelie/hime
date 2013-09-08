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

namespace Hime.Redist
{
    /// <summary>
    /// Represents an immutable dictionary of symbols.
    /// The symbols can be accessed by name or index.
    /// </summary>
    /// <typeparam name="T">The type of the symbols</typeparam>
    public sealed class SymbolDictionary<T> : IDictionary<string, T> where T : Symbols.Symbol
    {
        private Dictionary<string, T> impl;
        private T[] raw;

        /// <summary>
        /// Determines whether the collection is read-only. Always true.
        /// </summary>
        public bool IsReadOnly { get { return true; } }
        /// <summary>
        /// Gets the number of symbols in the collection
        /// </summary>
        public int Count { get { return raw.Length; } }
        /// <summary>
        /// Gets the collection of symbols' names
        /// </summary>
        public ICollection<string> Keys { get { return impl.Keys; } }
        /// <summary>
        /// Gets a collection of symbols
        /// </summary>
        public ICollection<T> Values { get { return impl.Values; } }
        /// <summary>
        /// Gets the symbol corresponding to the given name
        /// </summary>
        /// <param name="key">A symbol's name</param>
        /// <returns>The symbol with the given name</returns>
        public T this[string key]
        {
            get { return impl[key]; }
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Gets the symbol corresponding to the given index
        /// </summary>
        /// <param name="index">A symbol's index</param>
        /// <returns>The symbol at the given index</returns>
        public T this[int index] { get { return raw[index]; } }

        /// <summary>
        /// Initializes this dictionary from the given array of symbols
        /// </summary>
        /// <param name="data">The symbols serving as data for this dictionary</param>
        public SymbolDictionary(T[] data)
        {
            this.raw = data;
            this.impl = new Dictionary<string, T>();
            foreach (T item in data)
                this.impl.Add(item.Name, item);
        }

        /// <summary>
        /// Gets an enumerator of key-value pairs of symbols with their name
        /// </summary>
        /// <returns>An enumerator of key-value pairs of symbols with their name</returns>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() { return impl.GetEnumerator(); }
        /// <summary>
        /// Gets an enumerator of key-value pairs of symbols with their name
        /// </summary>
        /// <returns>An enumerator of key-value pairs of symbols with their name</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return impl.GetEnumerator(); }
        /// <summary>
        /// Tries to get a symbol for the given name
        /// </summary>
        /// <param name="key">The name to search for</param>
        /// <param name="value">The symbol with the given name, or null if none is found</param>
        /// <returns>True if a symbol is found</returns>
        public bool TryGetValue(string key, out T value) { return impl.TryGetValue(key, out value); }

        /// <summary>
        /// Determines whether a symbol with the given name exists
        /// </summary>
        /// <param name="key">A symbol's name</param>
        /// <returns>True if a symbol with the given name exists</returns>
        public bool ContainsKey(string key) { return impl.ContainsKey(key); }
        /// <summary>
        /// Determines whether a pair of name and symbol is in the collection
        /// </summary>
        /// <param name="item">A pair of name and symbol</param>
        /// <returns>True if the pair is in the collection</returns>
        public bool Contains(KeyValuePair<string, T> item) { return (impl.ContainsKey(item.Key) && impl[item.Key] == item.Value); }

        /// <summary>
        /// Adds a symbol with its name in the collection.
        /// This method is not implemented.
        /// </summary>
        /// <param name="key">The name of the symbol</param>
        /// <param name="value">The symbol</param>
        public void Add(string key, T value) { throw new NotImplementedException(); }
        /// <summary>
        /// Adds a pair of name and symbol in the collection
        /// This method is not implemented
        /// </summary>
        /// <param name="item">The pair to add to the collection</param>
        public void Add(KeyValuePair<string, T> item) { throw new NotImplementedException(); }
        /// <summary>
        /// Removes the symbol with the given name from the collection
        /// This method is not implemented
        /// </summary>
        /// <param name="key">The name if the symbol to remove from the collection</param>
        /// <returns>True if the corresponding symbol was found and removed</returns>
        public bool Remove(string key) { throw new NotImplementedException(); }
        /// <summary>
        /// Removes a pair of name and symbol from the collection
        /// This method is not implemented
        /// </summary>
        /// <param name="item">The pair of name and symbol to remove</param>
        /// <returns>True if the corresponding pair was removed</returns>
        public bool Remove(KeyValuePair<string, T> item) { throw new NotImplementedException(); }
        /// <summary>
        /// Removes all data in this collection
        /// This method is not implemented
        /// </summary>
        public void Clear() { throw new NotImplementedException(); }
        /// <summary>
        /// Copies the content of this collection to the given array, starting at the given index
        /// This method is not implemented
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="arrayIndex">The starting index in the array</param>
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) { throw new NotImplementedException(); }
    }
}
