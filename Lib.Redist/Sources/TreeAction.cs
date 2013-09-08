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
    /// Represents a tree action
    /// </summary>
    public enum TreeAction : byte
    {
        /// <summary>
        /// Keep the node as is
        /// </summary>
        None = 0,
        /// <summary>
        /// Replace the node with its children
        /// </summary>
        Replace = 1,
        /// <summary>
        /// Drop the node and all its descendants
        /// </summary>
        Drop = 2,
        /// <summary>
        /// Promote the node, i.e. replace its parent with it and insert its children where it was
        /// </summary>
        Promote = 3,
        /// <summary>
        /// Execute a semantic action
        /// </summary>
        Semantic = 4
    }
}
