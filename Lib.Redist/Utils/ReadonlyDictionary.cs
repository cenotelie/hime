using System;
using System.Collections.Generic;

namespace Hime.Redist.Utils
{
    /// <summary>
    /// Represents an immutable dictionary of symbols
    /// </summary>
    /// <typeparam name="T">The type of symbol</typeparam>
    public sealed class SymbolDictionary<T> : IDictionary<string, T> where T : Parsers.Symbol
    {
        private Dictionary<string, T> impl;
        internal T[] raw;

        public bool IsReadOnly { get { return true; } }
        public int Count { get { return raw.Length; } }
        public ICollection<string> Keys { get { return impl.Keys; } }
        public ICollection<T> Values { get { return impl.Values; } }
        public T this[string key]
        {
            get { return impl[key]; }
            set { throw new NotImplementedException(); }
        }
        public T this[int index] { get { return raw[index]; } }

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
