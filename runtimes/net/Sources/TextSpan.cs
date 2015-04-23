/**********************************************************************
* Copyright (c) 2015 Laurent Wouters and others
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

namespace Hime.Redist
{
	/// <summary>
	/// Represents a span of text in an input as a starting index and length
	/// </summary>
	public struct TextSpan
	{
		/// <summary>
		/// The starting index
		/// </summary>
		private readonly int index;
		/// <summary>
		/// The length
		/// </summary>
		private readonly int length;

		/// <summary>
		/// Gets the starting index of this span
		/// </summary>
		public int Index { get { return index; } }

		/// <summary>
		/// Gets the length of this span
		/// </summary>
		public int Length { get { return length; } }

		/// <summary>
		/// Initializes this span
		/// </summary>
		/// <param name="index">The span's index</param>
		/// <param name="length">The span's length</param>
		public TextSpan(int index, int length)
		{
			this.index = index;
			this.length = length;
		}
	}
}
