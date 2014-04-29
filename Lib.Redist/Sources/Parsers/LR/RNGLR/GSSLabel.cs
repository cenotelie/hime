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
	struct GSSLabel
	{
		/// <summary>
		/// The associated sub-tree
		/// </summary>
		private SubTree tree;
		/// <summary>
		/// The original symbol of the sub-tree's root
		/// </summary>
		private SymbolRef original;

		/// <summary>
		/// Gets a value indicating whether this instance is epsilon
		/// </summary>
		public bool IsEpsilon { get { return (tree == null); } }

		/// <summary>
		/// Gets the tree associated with this label
		/// </summary>
		public SubTree Tree { get { return tree; } }

		/// <summary>
		/// Gets the original symbol of the sub-tree's root
		/// </summary>
		public SymbolRef Original { get { return original; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Hime.Redist.Parsers.GSSLabel"/> struct.
		/// </summary>
		/// <param name="st">The associated sub-tree</param>
		public GSSLabel(SubTree st)
		{
			this.tree = st;
			this.original = st.GetLabelAt(0);
		}
	}
}