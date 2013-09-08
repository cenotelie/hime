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

namespace Hime.Redist.Lexer
{
    /// <summary>
    /// Represents a piece of text
    /// </summary>
    struct TextBuffer
    {
        private char[] data;    // The contained data
        private int start;      // The start index in the data
        private int end;        // The end index in the data

        /// <summary>
        /// Gets the character at the given index
        /// </summary>
        /// <param name="index">Index in the data</param>
        /// <returns>The corresponding character</returns>
        public char this[int index] { get { return data[index]; } }
        
        /// <summary>
        /// Gets the start index
        /// </summary>
        public int Start { get { return start; } }
        
        /// <summary>
        /// Gets the end index
        /// </summary>
        public int End { get { return end; } }
        
        /// <summary>
        /// Gets whether this buffer is empty
        /// </summary>
        public bool IsEmpty { get { return (end <= start); } }

        /// <summary>
        /// Initializes this buffer
        /// </summary>
        /// <param name="data">The underlyin data</param>
        /// <param name="start">The start index in the data</param>
        /// <param name="end">The end index in the data</param>
        public TextBuffer(char[] data, int start, int end)
        {
            this.data = data;
            this.start = start;
            this.end = end;
        }
    }
}
