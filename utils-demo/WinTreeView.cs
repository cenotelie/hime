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

using System.Windows.Forms;
using Hime.Redist;

namespace Hime.Demo
{
	/// <summary>
	/// Simple window that displays an AST
	/// </summary>
	partial class WinTreeView : Form
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Hime.Demo.WinTreeView"/> class.
		/// </summary>
		/// <param name="root">The root of the AST to display</param>
		public WinTreeView(ASTNode root)
		{
			InitializeComponent();
			TreeNode vroot = View.Nodes.Add(root.ToString());
			AddSubTree(vroot, root);
		}

		/// <summary>
		/// Recursively adds the given AST node and its children to the tree viewer
		/// </summary>
		/// <param name="vnode">The current tree viewer node</param>
		/// <param name="snode">An AST node</param>
		private static void AddSubTree(TreeNode vnode, ASTNode snode)
		{
			foreach (ASTNode child in snode.Children)
			{
				TreeNode vchild = vnode.Nodes.Add(child.ToString());
				AddSubTree(vchild, child);
			}
		}
	}
}