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
    /// Represents a collection of LRAction stored as a binary blob
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class LRActions
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private LRAction[] data;
        [FieldOffset(8)]
        private int count;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the number of actions in this collection
        /// </summary>
        public int Count { get { return count; } }
        /// <summary>
        /// Gets the action at the given index
        /// </summary>
        /// <param name="index">Index of the action</param>
        /// <returns>The action at the given index</returns>
        public LRAction this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob for the given number of items
        /// </summary>
        /// <param name="count">The number of items</param>
        public LRActions(int count)
        {
            this.blob = new byte[count * 4];
            this.count = count;
        }
    }
}
