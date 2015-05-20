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

namespace Hime.SDK
{
	/// <summary>
	/// Represents a Unicode block of characters
	/// </summary>
	public class UnicodeBlock
	{
		/// <summary>
		/// The block's name
		/// </summary>
		private readonly string name;
		/// <summary>
		/// The block's character span
		/// </summary>
		private readonly UnicodeSpan span;

		/// <summary>
		/// Get this block's name
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// Gets the span of this block
		/// </summary>
		public UnicodeSpan Span { get { return span; } }

		/// <summary>
		/// Initializes this Unicode block
		/// </summary>
		/// <param name="name">Block's name</param>
		/// <param name="begin">Beginning character (included)</param>
		/// <param name="end">End character (included)</param>
		public UnicodeBlock(string name, int begin, int end)
		{
			this.name = name;
			span = new UnicodeSpan(begin, end);
		}

		/// <summary>
		/// Gets the string representation of this block
		/// </summary>
		/// <returns>The string representation</returns>
		public override string ToString()
		{
			return name + " " + span;
		}
	}
}