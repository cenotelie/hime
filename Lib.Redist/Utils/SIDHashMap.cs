using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Utils
{
    class SIDHashMap<T>
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
    }
}
