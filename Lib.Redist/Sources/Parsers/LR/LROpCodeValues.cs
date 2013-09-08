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

namespace Hime.Redist.Parsers
{
    /// <summary>
    /// Enumeration of the LR op codes
    /// </summary>
    [CLSCompliant(false)]
    public enum LROpCodeValues : ushort
    {
        /// <summary>
        /// Pop an AST from the stack without applying any tree action
        /// </summary>
        PopNoAction = 0,
        /// <summary>
        /// Pop an AST from the stack and apply the drop tree action
        /// </summary>
        PopDrop = 2,
        /// <summary>
        /// Pop an AST from the stack and apply the promote tree action
        /// </summary>
        PopPromote = 3,

        /// <summary>
        /// Add a virtual symbol without tree action
        /// </summary>
        VirtualNoAction = 4 + PopNoAction,
        /// <summary>
        /// Add a virtual symbol and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This doesn't make any sense, but it is possible!
        /// </remarks>
        VirtualDrop = 4 + PopDrop,
        /// <summary>
        /// Add a virtual symbol and apply the promote tree action
        /// </summary>
        VirtualPromote = 4 + PopPromote,
        
        /// <summary>
        /// Execute a semantic action
        /// </summary>
        SemanticAction = 8,

        /// <summary>
        /// Add a null variable without any tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariableNoAction = 16,
        /// <summary>
        /// Add a null variable and apply the drop tree action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariableDrop = 16 + PopDrop,
        /// <summary>
        /// Add a null variable and apply the promote action
        /// </summary>
        /// <remarks>
        /// This can be found only in RNGLR productions
        /// </remarks>
        NullVariablePromote = 16 + PopPromote
    }
}
