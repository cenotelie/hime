using System;
using System.Collections.Generic;

namespace Hime.Redist.Utils
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
        /// <param name="index"></param>
        /// <returns></returns>
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

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() { return impl.GetEnumerator(); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return impl.GetEnumerator(); }
        public bool TryGetValue(string key, out T value) { return impl.TryGetValue(key, out value); }

        public bool ContainsKey(string key) { return impl.ContainsKey(key); }
        public bool Contains(KeyValuePair<string, T> item) { return (impl.ContainsKey(item.Key) && impl[item.Key] == item.Value); }

        public void Add(string key, T value) { throw new NotImplementedException(); }
        public void Add(KeyValuePair<string, T> item) { throw new NotImplementedException(); }
        public bool Remove(string key) { throw new NotImplementedException(); }
        public bool Remove(KeyValuePair<string, T> item) { throw new NotImplementedException(); }
        public void Clear() { throw new NotImplementedException(); }
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) { throw new NotImplementedException(); }
    }
}
