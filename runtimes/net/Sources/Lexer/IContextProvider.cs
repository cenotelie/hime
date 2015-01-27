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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// Represents an entity providing information about the current contexts
	/// </summary>
	public interface IContextProvider
	{
		/// <summary>
		/// Gets whether the specified context is in effect
		/// </summary>
		/// <param name="context">A context</param>
		/// <returns><c>true</c> if the specified context is in effect</returns>
		bool IsWithin(int context);

		/// <summary>
		/// Gets whether the terminal at the specified index would be of use
		/// </summary>
		/// <param name="terminalIndex">The index of the terminal</param>
		/// <returns><c>true</c> if the terminal would be of use</returns>
		bool CanUse(int terminalIndex);
	}
}
