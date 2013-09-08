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
    /// Represents a LRBytecode stored as a binary blob
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class LRBytecode
    {
        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private LROpCode[] data;
        [FieldOffset(8)]
        private int length;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the length of the bytecode
        /// </summary>
        public int Length { get { return length; } }
        /// <summary>
        /// Gets the opcode at the given index
        /// </summary>
        /// <param name="index">Index of the opcode</param>
        /// <returns>The opcode at the given index</returns>
        public LROpCode this[int index]
        {
            get { return data[index]; }
        }

        /// <summary>
        /// Initializes a new blob of the given length
        /// </summary>
        /// <param name="length">The length of the bytecode</param>
        public LRBytecode(int length)
        {
            this.blob = new byte[length * 2];
            this.length = length;
        }
    }
}
