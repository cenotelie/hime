/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a version of a node in a Shared-Packed Parse Forest
	/// </summary>
	class SPPFNodeVersion
	{
		/// <summary>
		/// The label of the node for this version
		/// </summary>
		private readonly TableElemRef label;
		/// <summary>
		/// The children of the node for this version
		/// </summary>
		private readonly SPPFNodeRef[] children;

		/// <summary>
		/// Gets the label of the node for this version
		/// </summary>
		public TableElemRef Label { get { return label; } }

		/// <summary>
		/// Gets the number of children for this version of the node
		/// </summary>
		public int ChildrenCount { get { return children != null ? children.Length : 0; } }

		/// <summary>
		/// Gets the children of the node for this version
		/// </summary>
		public SPPFNodeRef[] Children { get { return children; } }

		/// <summary>
		/// Initializes this node version without children
		/// </summary>
		/// <param name="label">The label for this version of the node</param>
		public SPPFNodeVersion(TableElemRef label)
		{
			this.label = label;
			this.children = null;
		}

		/// <summary>
		/// Initializes this node version
		/// </summary>
		/// <param name="label">The label for this version of the node</param>
		/// <param name="children">A buffer of children for this version of the node</param>
		/// <param name="childrenCount">The number of children</param>
		public SPPFNodeVersion(TableElemRef label, SPPFNodeRef[] children, int childrenCount)
		{
			this.label = label;
			if (children == null || childrenCount == 0)
			{
				this.children = null;
			}
			else
			{
				this.children = new SPPFNodeRef[childrenCount];
				Array.Copy(children, this.children, childrenCount);
			}
		}
	}
}