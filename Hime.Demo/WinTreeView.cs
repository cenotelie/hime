using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LangTest
{
    partial class WinTreeView : Form
    {
        public WinTreeView(Hime.Redist.Parsers.SyntaxTreeNode Root)
        {
            InitializeComponent();

            TreeNode VRoot = View.Nodes.Add(Root.Symbol.Name);
            AddSubTree(VRoot, Root);
        }

        private void AddSubTree(TreeNode VNode, Hime.Redist.Parsers.SyntaxTreeNode SNode)
        {
            foreach (Hime.Redist.Parsers.SyntaxTreeNode Child in SNode.Children)
            {
                TreeNode VChild = null;
                if (Child.Symbol is Hime.Redist.Parsers.SymbolToken)
                    VChild = VNode.Nodes.Add(Child.Symbol.Name + " : \"" + ((Hime.Redist.Parsers.SymbolToken)Child.Symbol).Value.ToString() + "\"");
                else
                    VChild = VNode.Nodes.Add(Child.Symbol.Name);
                AddSubTree(VChild, Child);
            }
        }
    }
}