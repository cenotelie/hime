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
	/// Represents a block in the stack of LR(k) parser
	/// </summary>
	struct LRkStackBlock
	{
		/// <summary>
		/// The LR(k) automaton state associated with this block
		/// </summary>
		private ushort state;
		/// <summary>
		/// The size of this block
		/// </summary>
		private ushort size;

		/// <summary>
		/// Gets the LR(k) automaton state associated with this block
		/// </summary>
		public ushort State { get { return state; } }

		/// <summary>
		/// Gets the size of this block
		/// </summary>
		public ushort Size { get { return size; } }

		/// <summary>
		/// Setups this block
		/// </summary>
		/// <param name="state">The LR(k) automaton state associated with this block</param>
		/// <param name="size">The size of this block</param>
		public void Setup(ushort state, ushort size)
		{
			this.state = state;
			this.size = size;
		}
	}
}
