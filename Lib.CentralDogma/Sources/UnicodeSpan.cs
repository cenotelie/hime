/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.Collections.Generic;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a range of Unicode characters
	/// </summary>
    public struct UnicodeSpan
    {
    	/// <summary>
    	/// Beginning of the range (included)
    	/// </summary>
        private UnicodeCodePoint spanBegin;
        
        /// <summary>
        /// End of the range (included)
        /// </summary>
        private UnicodeCodePoint spanEnd;

        /// <summary>
        /// Gets the first (included) character of the range
        /// </summary>
        public UnicodeCodePoint Begin { get { return spanBegin; } }
        
        /// <summary>
        /// Gets the last (included) character of the range
        /// </summary>
        public UnicodeCodePoint End { get { return spanEnd; } }
        
        /// <summary>
        /// Gets the range's length in number of characters
        /// </summary>
        public int Length { get { return spanEnd.Value - spanBegin.Value + 1; } }

        /// <summary>
        /// Initializes this character span
        /// </summary>
        /// <param name="begin">The first (included) character</param>
        /// <param name="end">The last (included) character</param>
        public UnicodeSpan(UnicodeCodePoint begin, UnicodeCodePoint end)
        {
            spanBegin = begin;
            spanEnd = end;
        }

		/// <summary>
        /// Initializes this character span
        /// </summary>
        /// <param name="begin">The first (included) character</param>
        /// <param name="end">The last (included) character</param>
		public UnicodeSpan(int begin, int end)
        {
            spanBegin = new UnicodeCodePoint(begin);
            spanEnd = new UnicodeCodePoint(end);
        }

        /// <summary>
        /// Gets the string representation of this span
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            if (spanBegin > spanEnd)
                return string.Empty;
            if (spanBegin == spanEnd)
                return spanBegin.ToString();
            return "[" + spanBegin.ToString() + "-" + spanEnd.ToString() + "]";
        }
    }
}