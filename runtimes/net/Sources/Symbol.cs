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
	/// Represents a grammar symbol (terminal, variable or virtual)
	/// </summary>
	public struct Symbol
	{
		/// <summary>
		/// Symbol ID for inexistant symbol
		/// </summary>
		public const int SID_NOTHING = 0;

		/// <summary>
		/// Symbol ID of the Epsilon terminal
		/// </summary>
		public const int SID_EPSILON = 1;

		/// <summary>
		/// Symbol ID of the Dollar terminal
		/// </summary>
		public const int SID_DOLLAR = 2;

		/// <summary>
		/// The symbol's unique identifier
		/// </summary>
		private readonly int id;
		/// <summary>
		/// The symbol's name
		/// </summary>
		private readonly string name;

		/// <summary>
		/// Gets the symbol's unique identifier
		/// </summary>
		public int ID { get { return id; } }

		/// <summary>
		/// Gets the symbol's name
		/// </summary>
		public string Name { get { return name; } }

		/// <summary>
		/// Initializes this symbol
		/// </summary>
		/// <param name="id">The id</param>
		/// <param name="name">The symbol's name</param>
		public Symbol(int id, string name)
		{
			this.id = id;
			this.name = name;
		}

		/// <summary>
		/// Gets a string representation of this symbol
		/// </summary>
		/// <returns>The value of this symbol</returns>
		public override string ToString()
		{
			return name;
		}
	}
}