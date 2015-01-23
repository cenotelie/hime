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
	/// Base values of LR op-code instructions
	/// </summary>
	public enum LROpCodeBase
	{
		/// <summary>
		/// Pop an AST from the stack
		/// </summary>
		PopStack = 0,

		/// <summary>
		/// Add a virtual symbol
		/// </summary>
		AddVirtual = 4,

		/// <summary>
		/// Execute a semantic action
		/// </summary>
		SemanticAction = 8,

		/// <summary>
		/// Add a null variable
		/// </summary>
		/// <remarks>
		/// This can be found only in RNGLR productions
		/// </remarks>
		AddNullVariable = 16,
	}
}