using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Hime.Redist;
using Hime.Redist.Symbols;

namespace Hime.Demo
{
    partial class WinTreeView : Form
    {
        public WinTreeView(ParseTree ast)
        {
            InitializeComponent();
            TreeNode vroot = View.Nodes.Add(GetString(ast.Root));
            AddSubTree(vroot, ast.Root);
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
            if (node.Symbol == null)
                return "<null>";
            string value = node.Symbol.Name;
            if (node.Symbol is Token)
                value += ": \"" + (node.Symbol as Token).Value + "\"";
            return value;
        }
    }
}