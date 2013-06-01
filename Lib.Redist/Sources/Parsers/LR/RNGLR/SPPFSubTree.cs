using System;

namespace Hime.Redist.Parsers
{
    class SPPFSubTree
    {
        private SPPFPool pool;
        private ParseTree.Cell[] items;
        private TreeAction[] actions;
        private int originalSID;

        public int OriginalSID
        {
            get { return originalSID; }
            set { originalSID = value; }
        }

        public Symbols.Symbol Head
        {
            get { return items[0].symbol; }
            set { items[0].symbol = value; }
        }
        public int ChildrenCount
        {
            get { return items[0].count; }
            set { items[0].count = value; }
        }
        public TreeAction Action
        {
            get { return actions[0]; }
            set { actions[0] = value; }
        }
        public ParseTree.Cell this[int index]
        {
            get { return items[index]; }
            set { items[index] = value; }
        }

        public TreeAction GetAction(int index) { return actions[index]; }

        public SPPFSubTree(SPPFPool pool, int capacity)
        {
            this.pool = pool;
            this.items = new ParseTree.Cell[capacity];
            this.actions = new TreeAction[capacity];
        }

        public int GetSize()
        {
            int size = 1;
            for (int i = 0; i != items[0].count; i++)
                size += items[size].count + 1;
            return size;
        }

        public void CopyTo(SPPFSubTree destination, int index)
        {
            if (this.items[0].count == 0)
            {
                destination.items[index] = this.items[0];
                destination.actions[index] = this.actions[0];
            }
            else
            {
                Array.Copy(this.items, 0, destination.items, index, this.items[0].count + 1);
                Array.Copy(this.actions, 0, destination.actions, index, this.items[0].count + 1);
            }
        }

        public void CopyChildrenTo(SPPFSubTree destination, int index)
        {
            if (this.items[0].count == 0)
                return;
            int size = GetSize() - 1;
            Array.Copy(this.items, 1, destination.items, index, size);
            Array.Copy(this.actions, 1, destination.actions, index, size);
        }

        public void CommitTo(int index, ParseTree tree)
        {
            if (this.items[index].count != 0)
                this.items[index].first = tree.Store(this.items, index + 1, this.items[index].count);
        }

        public void SetAt(int index, Symbols.Symbol symbol, TreeAction action)
        {
            this.items[index].symbol = symbol;
            this.items[index].count = 0;
            this.actions[index] = action;
        }

        public void Move(int from, int to)
        {
            this.items[to] = this.items[from];
        }

        public void MoveRange(int from, int to, int length)
        {
            if (length != 0)
            {
                Array.Copy(items, from, items, to, length);
                Array.Copy(actions, from, actions, to, length);
            }
        }

        public void Free()
        {
            if (pool != null)
                pool.Free(this);
        }
    }
}
