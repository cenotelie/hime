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

namespace Hime.Redist
{
	/// <summary>
	/// Represents a compact reference to a symbol in a table
	/// </summary>
	struct SymbolRef
	{
		/// <summary>
		/// The backend data
		/// </summary>
		private uint data;

		/// <summary>
		/// Gets the symbol's type
		/// </summary>
		public SymbolType Type { get { return (SymbolType)(data >> 30); } }

		/// <summary>
		/// Gets the symbol's index in its respective table
		/// </summary>
		public int Index { get { return (int)(data & 0x3FFFFFFF); } }

		/// <summary>
		/// Initializes this reference
		/// </summary>
		/// <param name="type">The symbol's type</param>
		/// <param name="index">The symbol's index in its table</param>
		public SymbolRef(SymbolType type, int index)
		{
			this.data = (((uint)type) << 30) | (uint)index;
		}
	}
}