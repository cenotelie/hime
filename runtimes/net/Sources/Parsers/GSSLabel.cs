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
	/// Represents a label on a GSS edge
	/// </summary>
	/// <remarks>
	/// The data in this structure can have two interpretations:
	/// 1) It can represent a sub-tree with a replaceable root.
	/// 2) It can represent a reference to a single node in a SPPF.
	/// </remarks>
	struct GSSLabel
	{
		/// <summary>
		/// A sub-tree with a replaceable root
		/// </summary>
		private readonly SubTree tree;
		/// <summary>
		/// The original symbol of the SPPF node
		/// </summary>
		private readonly TableElemRef original;
		/// <summary>
		/// The index of the SPPF node
		/// </summary>
		private readonly int nodeIndex;

		/// <summary>
		/// Gets the sub-tree with a repleaceable root
		/// </summary>
		public SubTree ReplaceableTree { get { return tree; } }
		/// <summary>
		/// Gets the original symbol of the SPPF node
		/// </summary>
		public TableElemRef Original { get { return original; } }
		/// <summary>
		/// Gets the index of the SPPF node
		/// </summary>
		public int NodeIndex { get { return nodeIndex; } }

		/// <summary>
		/// Wether this label is an epsilon label
		/// </summary>
		public bool IsEpsilon { get { return (tree == null && nodeIndex == -1); } }
		/// <summary>
		/// Whether this label represents a sub-tree with a replaceable root
		/// </summary>
		public bool IsReplaceable { get { return (tree != null); } }

		/// <summary>
		/// Initializes this label as representing a sub-tree with a replaceable root
		/// </summary>
		/// <param name="tree">The sub-tree with a replaceable root</param>
		public GSSLabel(SubTree tree)
		{
			this.tree = tree;
			original = new TableElemRef();
			nodeIndex = -1;
		}

		/// <summary>
		/// Initializes this label as representing a single SPPF node
		/// </summary>
		/// <param name="original">The original symbol of the SPPF node</param>
		/// <param name="index">The index of the SPPF node</param>
		public GSSLabel(TableElemRef original, int index)
		{
			tree = null;
			this.original = original;
			nodeIndex = index;
		}
	}
}