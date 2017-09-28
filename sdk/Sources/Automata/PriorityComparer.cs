/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System.Collections.Generic;

namespace Hime.SDK.Automata
{
	/// <summary>
	/// Represents a comparer of the priority of the final items
	/// This comparer is used to sort final items on a state by priority (greater has more priority)
	/// </summary>
	public class PriorityComparer : IComparer<int>
	{
		/// <summary>
		/// Compares the specified piorities
		/// </summary>
		/// <param name="x">A priority</param>
		/// <param name="y">Another priority</param>
		/// <returns>The comparison result</returns>
		public int Compare(int x, int y)
		{
			return (y - x);
		}
	}
}
