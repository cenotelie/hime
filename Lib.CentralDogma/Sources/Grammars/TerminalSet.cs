using System;
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
	sealed class TerminalSet : IList<Terminal>
    {
        private SortedList<ushort, Terminal> content;

        public TerminalSet()
        {
            content = new SortedList<ushort, Terminal>();
        }
        
        public TerminalSet(TerminalSet copied)
        {
            content = new SortedList<ushort, Terminal>(copied.content);
        }

        /**
         * Assumes item is not null.
         * */
        public bool Add(Terminal item)
        {
            if (content.ContainsKey(item.SID))
                return false;
            content.Add(item.SID, item);
            return true;
        }

        public bool AddRange(IEnumerable<Terminal> collection)
        {
            bool mod = false;
            foreach (Terminal item in collection)
            {
                if (!content.ContainsKey(item.SID))
                {
                    mod = true;
                    content.Add(item.SID, item);
                }
            }
            return mod;
        }

        public int IndexOf(Terminal item) { return content.IndexOfKey(item.SID); }
        
        public void Insert(int index, Terminal item) { throw new System.NotImplementedException(); }
        
        public void RemoveAt(int index) { content.RemoveAt(index); }

        public Terminal this[int index]
        {
            get { return content.Values[index]; }
            set { throw new System.NotImplementedException(); }
        }

        void ICollection<Terminal>.Add(Terminal item)
        {
            if (content.ContainsKey(item.SID))
                return;
            content.Add(item.SID, item);
        }

        public void Clear() { content.Clear(); }

        public bool Contains(Terminal item) { return content.ContainsKey(item.SID); }

        public void CopyTo(Terminal[] array, int arrayIndex) { throw new System.NotImplementedException(); }

        public int Count { get { return content.Count; } }

        public bool IsReadOnly {  get { return false; } }

        public bool Remove(Terminal item) { return content.Remove(item.SID); }

        public IEnumerator<Terminal> GetEnumerator() { return content.Values.GetEnumerator(); }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return content.Values.GetEnumerator(); }

        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder("{");
            for (int i = 0; i != Count; i++)
            {
                if (i != 0)
                    builder.Append(", ");
                builder.Append(this[i].Name);
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
