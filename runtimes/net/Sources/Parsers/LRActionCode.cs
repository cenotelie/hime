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

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents an action in a LR parser
	/// </summary>
	public enum LRActionCode
	{
		/// <summary>
		/// No possible action => Error
		/// </summary>
		None = 0,
		/// <summary>
		/// Apply a reduction
		/// </summary>
		Reduce = 1,
		/// <summary>
		/// Shift to another state
		/// </summary>
		Shift = 2,
		/// <summary>
		/// Accept the input
		/// </summary>
		Accept = 3
	}
}
