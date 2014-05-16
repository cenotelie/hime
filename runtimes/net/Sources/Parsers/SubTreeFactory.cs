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
using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents factories of SubTrees
	/// </summary>
	class SubTreeFactory : Factory<SubTree>
	{
		/// <summary>
		/// The capacity of the SubTrees produced by this factory
		/// </summary>
		private int capacity;

		/// <summary>
		/// Initializes this SubTree factory
		/// </summary>
		/// <param name="capacity">The capacity of the produced SubTrees</param>
		public SubTreeFactory(int capacity)
		{
			this.capacity = capacity;
		}

		/// <summary>
		///  Creates a new object
		/// </summary>
		/// <param name="pool">The enclosing pool</param>
		/// <returns>The created object</returns>
		public SubTree CreateNew(Pool<SubTree> pool)
		{
			return new SubTree(pool, capacity);
		}
	}
}