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
    partial class WinTreeView : Form
    {
        public WinTreeView(CSTNode root)
        {
            InitializeComponent();
            TreeNode vroot = View.Nodes.Add(root.Symbol.Name);
            AddSubTree(vroot, root);
        }

        private void AddSubTree(TreeNode vnode, CSTNode snode)
        {
            foreach (CSTNode child in snode.Children)
            {
                TreeNode vchild = null;
                if (child.Symbol != null)
                {
                    string name = child.Symbol.Name;
                    string value = "";
                    if (child.Symbol is Token)
                        value = ": \"" + (child.Symbol as Token).Value.ToString() + "\"";
                    string header = name + value;
                    vchild = vnode.Nodes.Add(header);
                }
                else
                {
                    vchild = vnode.Nodes.Add("<null>");
                }
                AddSubTree(vchild, child);
            }
        }
    }
}