using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represents a sub-tree in an AST
    /// </summary>
    /// <remarks>
    /// A sub-tree is composed of a root with its children.
    /// The children may also have children.
    /// The maximum depth of a sub-tree is 2 (root, children and children's children), in which case the root is always a replaceable node.
    /// The internal representation of a sub-tree is an array of Parse Tree Cells.
    /// The organization is that a node's children are immediately following it in the array.
    /// For example, the tree A(B(CD)E(FG)) is represented as [ABCDEFG].
    /// </remarks>
    class SubTree
    {
        private Pool<SubTree> pool;
        private ParseTree.Cell[] items;
        private TreeAction[] actions;

        /// <summary>
        /// Gets ot sets the number of children of the root
        /// </summary>
        public int ChildrenCount
        {
            get { return items[0].count; }
            set { items[0].count = value; }
        }

        /// <summary>
        /// Gets or sets the action applied to the root
        /// </summary>
        public TreeAction Action
        {
            get { return actions[0]; }
            set { actions[0] = value; }
        }

        /// <summary>
        /// Gets the i-th item in this sub-tree's buffer
        /// </summary>
        /// <param name="index">The index within the buffer</param>
        /// <returns>The i-th item in the buffer</returns>
        public ParseTree.Cell GetItem(int index) { return items[index]; }
        /// <summary>
        /// Gets tree action applied to the i-th item in this sub-tree's buffer
        /// </summary>
        /// <param name="index">The index within the buffer</param>
        /// <returns>The tree action applied to the i-th item</returns>
        public TreeAction GetAction(int index) { return actions[index]; }

        /// <summary>
        /// Instantiates a new sub-tree attached to the given pool, with the given capacity
        /// </summary>
        /// <param name="pool">The pool to which this sub-tree is attached</param>
        /// <param name="capacity">The capacity of the internal buffer of this sub-tree</param>
        public SubTree(Pool<SubTree> pool, int capacity)
        {
            this.pool = pool;
            this.items = new ParseTree.Cell[capacity];
            this.actions = new TreeAction[capacity];
        }

        /// <summary>
        /// Initializes the content of this sub-tree
        /// </summary>
        /// <param name="symbol">The root's symbol</param>
        /// <param name="childrenCount">The root's number of children</param>
        /// <param name="action">The tree action applied on the root</param>
        public void Initialize(Symbols.Symbol symbol, int childrenCount, TreeAction action)
        {
            items[0].symbol = symbol;
            items[0].count = childrenCount;
            actions[0] = action;
        }

        /// <summary>
        /// Gets the total number of nodes in this sub-tree
        /// </summary>
        /// <returns>The total number of nodes in this sub-tree</returns>
        public int GetSize()
        {
            int size = 1;
            for (int i = 0; i != items[0].count; i++)
                size += items[size].count + 1;
            return size;
        }

        /// <summary>
        /// Copy the content of this sub-tree to the given sub-tree's buffer beginning at the given index
        /// </summary>
        /// <param name="destination">The sub-tree to copy to</param>
        /// <param name="index">The starting index in the destination's buffer</param>
        /// <remarks>
        /// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
        /// The results of this method in the case of a depth 2 sub-tree is undetermined.
        /// </remarks>
        public void CopyTo(SubTree destination, int index)
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

        /// <summary>
        /// Copy the root's children of this sub-tree to the given sub-tree's buffer beginning at the given index
        /// </summary>
        /// <param name="destination">The sub-tree to copy to</param>
        /// <param name="index">The starting index in the destination's buffer</param>
        /// <remarks>
        /// This methods only applies in the case of a depth 1 sub-tree (only a root and its children).
        /// The results of this method in the case of a depth 2 sub-tree is undetermined.
        /// </remarks>
        public void CopyChildrenTo(SubTree destination, int index)
        {
            if (this.items[0].count == 0)
                return;
            int size = GetSize() - 1;
            Array.Copy(this.items, 1, destination.items, index, size);
            Array.Copy(this.actions, 1, destination.actions, index, size);
        }

        /// <summary>
        /// Commits this sub-tree starting at the given index to the final parse tree
        /// </summary>
        /// <param name="index">The starting index of the sub-tree</param>
        /// <param name="tree">The parse tree to commit to</param>
        /// <remarks>
        /// If the index is 0, the root's children are commited.
        /// If not, the children of the child at the given index are commited.
        /// </remarks>
        public void CommitTo(int index, ParseTree tree)
        {
            if (this.items[index].count != 0)
                this.items[index].first = tree.Store(this.items, index + 1, this.items[index].count);
        }

        /// <summary>
        /// Sets the content of the i-th item
        /// </summary>
        /// <param name="index">The index of the item to set</param>
        /// <param name="symbol">The new item's symbol</param>
        /// <param name="action">The tree action to apply</param>
        public void SetAt(int index, Symbols.Symbol symbol, TreeAction action)
        {
            this.items[index].symbol = symbol;
            this.items[index].count = 0;
            this.actions[index] = action;
        }

        /// <summary>
        /// Moves an item within the buffer
        /// </summary>
        /// <param name="from">The index of the item to move</param>
        /// <param name="to">The destination index for the item</param>
        public void Move(int from, int to)
        {
            this.items[to] = this.items[from];
        }

        /// <summary>
        /// Moves a range of items within the buffer
        /// </summary>
        /// <param name="from">The starting index of the items to move</param>
        /// <param name="to">The destination index for the items</param>
        /// <param name="length">The number of items to move</param>
        public void MoveRange(int from, int to, int length)
        {
            if (length != 0)
            {
                Array.Copy(items, from, items, to, length);
                Array.Copy(actions, from, actions, to, length);
            }
        }

        /// <summary>
        /// Releases this sub-tree to the pool
        /// </summary>
        public void Free()
        {
            if (pool != null)
                pool.Returns(this);
        }
    }
}
