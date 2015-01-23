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
	/// Represents a piece of parsing data
	/// </summary>
	public interface ParseAtom
	{
		/// <summary>
		/// Gets the position in the input text of this element
		/// </summary>
		TextPosition Position { get; }

		/// <summary>
		/// Gets the span in the input text of this element
		/// </summary>
		TextSpan Span { get; }

		/// <summary>
		/// Gets the context of this element in the input
		/// </summary>
		TextContext Context { get; }

		/// <summary>
		/// Gets the grammar symbol associated to this element
		/// </summary>
		Symbol Symbol { get; }

		/// <summary>
		/// Gets the value of this element, if any
		/// </summary>
		string Value { get; }
	}
}
