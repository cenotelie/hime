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
		PopStackNoAction = LROpCodeBase.PopStack + TreeAction.None,
		/// <summary>
		/// Pop an AST from the stack and apply the drop tree action
		/// </summary>
		PopStackDrop = LROpCodeBase.PopStack + TreeAction.Drop,
		/// <summary>
		/// Pop an AST from the stack and apply the promote tree action
		/// </summary>
		PopStackPromote = LROpCodeBase.PopStack + TreeAction.Promote,

		/// <summary>
		/// Add a virtual symbol without tree action
		/// </summary>
		AddVirtualNoAction = LROpCodeBase.AddVirtual + TreeAction.None,
		/// <summary>
		/// Add a virtual symbol and apply the drop tree action
		/// </summary>
		/// <remarks>
		/// This doesn't make any sense, but it is possible!
		/// </remarks>
		AddVirtualDrop = LROpCodeBase.AddVirtual + TreeAction.Drop,
		/// <summary>
		/// Add a virtual symbol and apply the promote tree action
		/// </summary>
		AddVirtualPromote = LROpCodeBase.AddVirtual + TreeAction.Promote,

		/// <summary>
		/// Execute a semantic action
		/// </summary>
		SemanticAction = LROpCodeBase.SemanticAction,

		/// <summary>
		/// Add a null variable without any tree action
		/// </summary>
		/// <remarks>
		/// This can be found only in RNGLR productions
		/// </remarks>
		AddNullVariableNoAction = LROpCodeBase.AddNullVariable + TreeAction.None,
		/// <summary>
		/// Add a null variable and apply the drop tree action
		/// </summary>
		/// <remarks>
		/// This can be found only in RNGLR productions
		/// </remarks>
		AddNullVariableDrop = LROpCodeBase.AddNullVariable + TreeAction.Drop,
		/// <summary>
		/// Add a null variable and apply the promote action
		/// </summary>
		/// <remarks>
		/// This can be found only in RNGLR productions
		/// </remarks>
		AddNullVariablePromote = LROpCodeBase.AddNullVariable + TreeAction.Promote
	}
}
