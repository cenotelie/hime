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
	/// Represents a node in a Shared-Packed Parse Forest that can be replaced by its children
	/// </summary>
	class SPPFNodeReplaceable : SPPFNode
	{
		/// <summary>
		/// The label of this node
		/// </summary>
		private readonly TableElemRef label;
		/// <summary>
		/// The children of this node
		/// </summary>
		private readonly SPPFNodeRef[] children;
		/// <summary>
		/// The tree actions on the children of this node
		/// </summary>
		private readonly TreeAction[] actions;

		/// <summary>
		/// Gets whether this node must be replaced by its children
		/// </summary>
		public override bool IsReplaceable { get { return true; } }

		/// <summary>
		/// Gets the original symbol for this node
		/// </summary>
		public override TableElemRef OriginalSymbol { get { return label; } }

		/// <summary>
		/// Gets the number of children of this node
		/// </summary>
		public int ChildrenCount { get { return children != null ? children.Length : 0; } }

		/// <summary>
		/// Gets the children of this node
		/// </summary>
		public SPPFNodeRef[] Children { get { return children; } }

		/// <summary>
		/// Gets the tree actions on the children of this node
		/// </summary>
		public TreeAction[] Actions { get { return actions; } }

		/// <summary>
		/// Initializes this node
		/// </summary>
		/// <param name="identifier">The identifier of this node</param>
		/// <param name="label">The label of this node</param>
		/// <param name="childrenBuffer">A buffer for the children</param>
		/// <param name="actionsBuffer">A buffer for the actions on the children</param>
		/// <param name="childrenCount">The number of children</param>
		public SPPFNodeReplaceable(int identifier, TableElemRef label, SPPFNodeRef[] childrenBuffer, TreeAction[] actionsBuffer, int childrenCount) : base(identifier)
		{
			this.label = label;
			if (childrenCount > 0)
			{
				this.children = new SPPFNodeRef[childrenCount];
				this.actions = new TreeAction[childrenCount];
				Array.Copy(childrenBuffer, children, childrenCount);
				Array.Copy(actionsBuffer, actions, childrenCount);
			}
			else
			{
				this.children = null;
				this.actions = null;
			}
		}
	}
}