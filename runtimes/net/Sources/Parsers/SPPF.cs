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
	/// Represents a Shared-Packed Parse Forest
	/// </summary>
	class SPPF
	{
		/// <summary>
		/// Represents the epsilon node
		/// </summary>
		public const int EPSILON = -1;


		/// <summary>
		/// The nodes in the SPPF
		/// </summary>
		private readonly BigList<SPPFNode> nodes;

		/// <summary>
		/// Initializes this SPPF
		/// </summary>
		public SPPF()
		{
			this.nodes = new BigList<SPPFNode>();
		}

		/// <summary>
		/// Gets the SPPF node for the specified identifier
		/// </summary>
		/// <param name="identifier">The identifier of an SPPF node</param>
		/// <returns>The SPPF node</returns>
		public SPPFNode GetNode(int identifier)
		{
			return nodes[identifier];
		}

		/// <summary>
		/// Creates a new single node in the SPPF
		/// </summary>
		/// <param name="label">The original label for this node</param>
		/// <returns>The identifier of the new node</returns>
		public int NewNode(TableElemRef label)
		{
			return nodes.Add(new SPPFNodeNormal(nodes.Size, label));
		}

		/// <summary>
		/// Creates a new single node in the SPPF
		/// </summary>
		/// <param name="original">The original symbol of this node</param>
		/// <param name="label">The label on the first version of this node</param>
		/// <param name="childrenBuffer">A buffer for the children</param>
		/// <param name="childrenCount">The number of children</param>
		/// <returns>The identifier of the new node</returns>
		public int NewNode(TableElemRef original, TableElemRef label, SPPFNodeRef[] childrenBuffer, int childrenCount)
		{
			return nodes.Add(new SPPFNodeNormal(nodes.Size, original, label, childrenBuffer, childrenCount));
		}

		/// <summary>
		/// Creates a new replaceable node in the SPPF
		/// </summary>
		/// <param name="label">The label of this node</param>
		/// <param name="childrenBuffer">A buffer for the children</param>
		/// <param name="actionsBuffer">A buffer for the actions on the children</param>
		/// <param name="childrenCount">The number of children</param>
		/// <returns>The identifier of the new node</returns>
		public int NewReplaceableNode(TableElemRef label, SPPFNodeRef[] childrenBuffer, TreeAction[] actionsBuffer, int childrenCount)
		{
			return nodes.Add(new SPPFNodeReplaceable(nodes.Size, label, childrenBuffer, actionsBuffer, childrenCount));
		}
	}
}