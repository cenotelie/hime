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

using System.Collections.Generic;

namespace Hime.Redist.AST
{
    /// <summary>
    /// Represents a node in the raw SPPF (Shared Packed Parse Forest) created by a RNGLR parser
    /// Tree actions are directly executed on this structure, producing the final AST
    /// </summary>
    /// <remarks>
    /// The full SPPF is never produced because the only the first family of each SPPf node is ever treated.
    /// This is an optimization based on the fact that user only want an AST and the final AST is arbitrarily produced by the first families.
    /// </remarks>
    class SPPFNode : BuildNode
    {
        /// <summary>
        /// The original SID retained by this SPPF node (even though the final AST node may change due to tree actions)
        /// </summary>
        private int originalSID;
        /// <summary>
        /// Gets the original SID of this SPPF node
        /// </summary>
        public int SymbolID { get { return originalSID; } }
        
        /// <summary>
        /// Initializes this node with the given symbol
        /// </summary>
        /// <param name="symbol">The symbol for this node</param>
        public SPPFNode(Symbols.Symbol symbol)
            : base(symbol)
        {
            this.originalSID = symbol.SymbolID;
        }
    }
}
