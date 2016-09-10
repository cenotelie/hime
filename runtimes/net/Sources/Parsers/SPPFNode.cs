/**********************************************************************
* Copyright (c) 2016 Laurent Wouters
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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a node in a Shared-Packed Parse Forest
	/// </summary>
	abstract class SPPFNode
	{
		/// <summary>
		/// The identifier of this node
		/// </summary>
		protected readonly int identifier;

		/// <summary>
		/// Gets the identifier of this node
		/// </summary>
		public int Identifier { get { return identifier; } }

		/// <summary>
		/// Gets whether this node must be replaced by its children
		/// </summary>
		public abstract bool IsReplaceable { get; }

		/// <summary>
		/// Gets the original symbol for this node
		/// </summary>
		public abstract TableElemRef OriginalSymbol { get; }

		/// <summary>
		/// Initializes this node
		/// </summary>
		/// <param name="identifier">The identifier of this node</param>
		public SPPFNode(int identifier)
		{
			this.identifier = identifier;
		}
	}
}