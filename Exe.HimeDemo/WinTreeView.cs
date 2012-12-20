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
        public WinTreeView(CSTNode Root)
        {
            InitializeComponent();

            TreeNode VRoot = View.Nodes.Add(Root.Symbol.Name);
            AddSubTree(VRoot, Root);
        }

        private void AddSubTree(TreeNode VNode, CSTNode SNode)
        {
            foreach (CSTNode Child in SNode.Children)
            {
                TreeNode VChild = null;
                if (Child.Symbol != null)
                {
                    string name = Child.Symbol.Name;
                    string value = "";
                    if (Child.Symbol is Token)
                        value = ": \"" + (Child.Symbol as Token).Value.ToString() + "\"";
                    string header = name + value;
                    VChild = VNode.Nodes.Add(header);
                }
                else
                {
                    VChild = VNode.Nodes.Add("<null>");
                }
                AddSubTree(VChild, Child);
            }
        }
    }
}