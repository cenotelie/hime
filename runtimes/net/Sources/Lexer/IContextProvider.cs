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
		/// Gets the priority of the specified context required by the specified terminal
		/// The priority is a positive integer. The lesser the value the higher the priority.
		/// A negative value represents the unavailability of the required context.
		/// </summary>
		/// <param name="context">A context</param>
		/// <param name="onTerminalID">The identifier of the terminal requiring the context</param>
		/// <returns>The context priority, or a negative value if the context is unavailable</returns>
		int GetContextPriority(int context, int onTerminalID);
	}
}
