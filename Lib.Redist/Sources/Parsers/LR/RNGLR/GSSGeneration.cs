using System.Collections;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
    class GSSGeneration : IEnumerable<GSSNode>
    {
        private GSS stack;
        private int index;
        private int size;
        private GSSNode[] data;
        private BitArray marks;

        public int Index { get { return index; } }
        public int Size { get { return size; } }

        public GSSGeneration(GSS stack, int index, int nbstates)
        {
            this.stack = stack;
            this.index = index;
            this.data = new GSSNode[nbstates];
            this.marks = new BitArray(nbstates);
        }

        public GSSNode this[int state] { get { return data[state]; } }

        public bool Contains(int state) { return (data[state] == null); }

        public GSSNode CreateNode(int state)
        {
            GSSNode node = stack.Acquire();
            node.Initialize(this, state);
            data[state] = node;
            size++;
            return node;
        }

        public void Mark(GSSNode node)
        {
            marks.Set(node.State, true);
        }

        public void Sweep()
        {
            int found = 0;
            for (int i = 0; i != data.Length; i++)
            {
                GSSNode node = data[i];
                if (node == null)
                    continue;
                found++;
                if (!marks[i])
                {
                    stack.Free(node);
                    data[i] = null;
                }
                if (found == size)
                    return;
            }
        }

        public IEnumerator<GSSNode> GetEnumerator() { return new Iterator(data); }

        IEnumerator IEnumerable.GetEnumerator() { return new Iterator(data); }

        private class Iterator : IEnumerator<GSSNode>
        {
            private GSSNode[] data;
            private int index;

            public Iterator(GSSNode[] data)
            {
                this.data = data;
                this.index = -1;
            }

            public void Dispose() { this.data = null; }
            public void Reset() { this.index = -1; }

            public GSSNode Current { get { return data[index]; } }
            object IEnumerator.Current { get { return data[index]; } }

            public bool MoveNext()
            {
                index++;
                while (index < data.Length && data[index] == null)
                    index++;
                return (index < data.Length);
            }
        }
    }
}
