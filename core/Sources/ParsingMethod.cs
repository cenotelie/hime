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

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a parsing method
	/// </summary>
	public enum ParsingMethod : byte
	{
		/// <summary>
		/// The LR(0) parsing method
		/// </summary>
		LR0 = 1,
		/// <summary>
		/// The LR(1) parsing method
		/// </summary>
		LR1 = 2,
		/// <summary>
		/// The LALR(1) parsing method
		/// </summary>
		LALR1 = 3,
		/// <summary>
		/// The RNGLR parsing method based on a LR(1) graph
		/// </summary>
		RNGLR1 = 4,
		/// <summary>
		/// The RNGLR parsing method based on a LALR(1) graph
		/// </summary>
		RNGLALR1 = 5
	}
}