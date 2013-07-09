using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a node and its children families in a Shared-Packed Parse Forest
	/// </summary>
    class SubSPPF : SubTree
    {
        private Pool<SubSPPF> pool;
        private int originalSID;

        /// <summary>
        /// Gets the ID of the original root symbol, even if it has been replaced
        /// </summary>
        public int OriginalSID { get { return originalSID; } }

        /// <summary>
        /// Instantiates a new sub-tree attached to the given pool, with the given capacity
        /// </summary>
        /// <param name="pool">The pool to which this sub-tree is attached</param>
        /// <param name="capacity">The capacity of the internal buffer of this sub-tree</param>
        public SubSPPF(Pool<SubSPPF> pool, int capacity)
            : base(null, capacity)
        {
            this.pool = pool;
        }

        public SubSPPF(Pool<SubSPPF> pool, Symbols.Symbol symbol)
            : base(null, 1)
        {
            this.pool = pool;
            this.Initialize(symbol, 0, TreeAction.None);
        }

        /// <summary>
        /// Initializes the content of this sub-tree
        /// </summary>
        /// <param name="symbol">The root's symbol</param>
        /// <param name="childrenCount">The root's number of children</param>
        /// <param name="action">The tree action applied on the root</param>
        public new void Initialize(Symbols.Symbol symbol, int childrenCount, TreeAction action)
        {
            this.originalSID = symbol.SymbolID;
            base.Initialize(symbol, childrenCount, action);
        }

        /// <summary>
        /// Releases this sub-tree to the pool
        /// </summary>
        public new void Free()
        {
            if (pool != null)
                pool.Return(this);
        }
    }
}
