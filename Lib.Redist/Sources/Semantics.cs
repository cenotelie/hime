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

namespace Hime.Redist
{
    /// <summary>
    /// Represents the semantic body of a rule being reduced
    /// </summary>
    public interface SemanticBody
    {
        /// <summary>
        /// Gets the symbol at the i-th index
        /// </summary>
        /// <param name="index">Index of the symbol</param>
        /// <returns>The symbol at the given index</returns>
        Symbol this[int index] { get; }

        /// <summary>
        /// Gets the length of this body
        /// </summary>
        int Length { get; }
    }

    /// <summary>
    /// Delegate for a user-defined semantic action on the given body and with the given parent
    /// </summary>
    /// <param name="head">The semantic object for the head</param>
    /// <param name="body">The current body at the time of the action</param>
    public delegate void UserAction(Symbol head, SemanticBody body);
}