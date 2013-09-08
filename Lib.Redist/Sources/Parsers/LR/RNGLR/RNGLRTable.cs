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
    /// Represents a parse table for the RNGLR algorithm
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    class RNGLRTable
    {
        /// <summary>
        /// Represents a cell in a RNGLR parse table
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = 6)]
        public struct Cell
        {
            [FieldOffset(0)]
            private ushort count;
            [FieldOffset(2)]
            private uint index;

            /// <summary>
            /// Gets the number of actions in the cell
            /// </summary>
            public int ActionsCount { get { return count; } }
            /// <summary>
            /// Gets the index of the first action in the Actions table
            /// </summary>
            public int ActionsIndex { get { return (int)index; } }
        }

        [FieldOffset(0)]
        private byte[] blob;
        [FieldOffset(0)]
        private Cell[] data;
        [FieldOffset(8)]
        private int ncols;

        /// <summary>
        /// Gets the raw array of bytes
        /// </summary>
        public byte[] Raw { get { return blob; } }
        /// <summary>
        /// Gets the cell for the given row and column
        /// </summary>
        /// <param name="row">The row in the table</param>
        /// <param name="col">The column in the table</param>
        /// <returns>The Cell for the given row and column</returns>
        public Cell this[int row, int col]
        {
            get { return data[row * ncols + col]; }
        }

        /// <summary>
        /// Initializes a new table with the given size
        /// </summary>
        /// <param name="nbstates">Number of states (rows)</param>
        /// <param name="ncols">Number of columns</param>
        public RNGLRTable(int nbstates, int ncols)
        {
            this.blob = new byte[nbstates * ncols * 6];
            this.ncols = ncols;
        }
    }
}
