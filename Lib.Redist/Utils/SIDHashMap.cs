using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Utils
{
    internal class SIDHashMap<T> : IDictionary<ushort, T>
    {
        private T[] cache1;
        private T[] cache2;
        private Dictionary<ushort, T> others;

        public SIDHashMap()
        {
            cache1 = new T[256];
        }

        public void Add(ushort key, T value)
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
                    others = new Dictionary<ushort, T>();
                others.Add(key, value);
            }
        }

        public bool ContainsKey(ushort key)
        {
            throw new NotImplementedException();
        }

        public ICollection<ushort> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(ushort key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(ushort key, out T value)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Values
        {
            get { throw new NotImplementedException(); }
        }

        public T this[ushort key]
        {
            get
            {
                if (key <= 0xFF)
                    return cache1[key];
                else if (key <= 0x1FF)
                    return cache2[key];
                return others[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<ushort, T> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<ushort, T> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<ushort, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<ushort, T> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<ushort, T>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
