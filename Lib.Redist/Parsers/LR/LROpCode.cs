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

using System.Runtime.InteropServices;

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Represent an opcode for a LR production
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    struct LROpCode
    {
        private const ushort MaskAction = 3;
        private const ushort Virtual = 4;
        private const ushort SemanticAction = 8;
        private const ushort NullVariable = 16;
        
        [FieldOffset(0)]
        private ushort code;

        /// <summary>
        /// Gets the code's value
        /// </summary>
        public int Value { get { return code; } }

        /// <summary>
        /// Gets the tree action included in this code
        /// </summary>
        public LRTreeAction TreeAction { get { return (LRTreeAction)(code & MaskAction); } }

        /// <summary>
        /// Gets whether this is a Pop action
        /// </summary>
        public bool IsPop { get { return (code < Virtual); } }

        /// <summary>
        /// Gets whether this is a Add Virtual action
        /// </summary>
        public bool IsAddVirtual { get { return ((code & Virtual) == Virtual); } }

        /// <summary>
        /// Gets whether this is a Semantic Action
        /// </summary>
        public bool IsSemAction { get { return (code == SemanticAction); } }

        /// <summary>
        /// Gets whether this is a Add Null Variable action
        /// </summary>
        public bool IsAddNullVar { get { return ((code & NullVariable) == NullVariable); } }
    }
}
