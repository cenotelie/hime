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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Hime.Redist.AST;
using Hime.Redist.Symbols;

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
            TreeNode vroot = View.Nodes.Add(GetString(root));
            AddSubTree(vroot, root);
        }

		/// <summary>
		/// Recursively adds the given AST node and its children to the tree viewer
		/// </summary>
		/// <param name="vnode">The current tree viewer node</param>
		/// <param name="snode">An AST node</param>
        private void AddSubTree(TreeNode vnode, ASTNode snode)
        {
            foreach (ASTNode child in snode.Children)
            {
                TreeNode vchild = null;
                vchild = vnode.Nodes.Add(GetString(child));
                AddSubTree(vchild, child);
            }
        }

		/// <summary>
		/// Gets the string representation for display of the given node
		/// </summary>
		/// <param name="node">An AST node</param>
		/// <returns>The string representation of the node</returns>
        private string GetString(ASTNode node)
        {
            if (node.Symbol == null)
                return "<null>";
            string value = node.Symbol.Name;
            if (node.Symbol is Token)
                value += ": \"" + (node.Symbol as Token).Value + "\"";
            return value;
        }
    }
}