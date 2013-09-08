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
