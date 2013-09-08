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
    /// Represents a LR action in a LR parse table
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    struct LRAction
    {
        [FieldOffset(0)]
        private LRActionCode code;
        [FieldOffset(2)]
        private ushort data;

        /// <summary>
        /// Gets the action code
        /// </summary>
        public LRActionCode Code { get { return code; } }
        /// <summary>
        /// Gets the data associated with the action
        /// </summary>
        /// <remarks>
        /// If the code is Reduce, it is the index of the LRProduction
        /// If the code is Shift, it is the index of the next state
        /// </remarks>
        public int Data { get { return data; } }
    }
}
