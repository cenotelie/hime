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
using Hime.Redist;

namespace Hime.Demo
{
    partial class WinTreeView : Form
    {
        public WinTreeView(ASTNode root)
        {
            InitializeComponent();
            TreeNode vroot = View.Nodes.Add(GetString(root));
            AddSubTree(vroot, root);
        }

        private void AddSubTree(TreeNode vnode, ASTNode snode)
        {
            foreach (ASTNode child in snode.Children)
            {
                TreeNode vchild = null;
                vchild = vnode.Nodes.Add(GetString(child));
                AddSubTree(vchild, child);
            }
        }

        private string GetString(ASTNode node)
        {
            Symbol symbol = node.Symbol;
            if (symbol.Value != null)
                return symbol.Value;
            return symbol.Name;
        }
    }
}