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
        public WinTreeView(Hime.Kernel.Parsers.SyntaxTreeNode Root)
        {
            InitializeComponent();

            TreeNode VRoot = View.Nodes.Add(Root.Symbol.Name);
            AddSubTree(VRoot, Root);
        }
        public WinTreeView(Hime.Kernel.Symbol Root)
        {
            InitializeComponent();

            TreeNode VRoot = View.Nodes.Add(Root.LocalName);
            AddSubTree(VRoot, Root);
        }

        private void AddSubTree(TreeNode VNode, Hime.Kernel.Parsers.SyntaxTreeNode SNode)
        {
            foreach (Hime.Kernel.Parsers.SyntaxTreeNode Child in SNode.Children)
            {
                TreeNode VChild = null;
                if (Child.Symbol is Hime.Kernel.Parsers.SymbolToken)
                    VChild = VNode.Nodes.Add(Child.Symbol.Name + " : \"" + ((Hime.Kernel.Parsers.SymbolToken)Child.Symbol).Value.ToString() + "\"");
                else
                    VChild = VNode.Nodes.Add(Child.Symbol.Name);
                AddSubTree(VChild, Child);
            }
        }

        private void AddSubTree(TreeNode VNode, Hime.Kernel.Symbol Symbol)
        {
            foreach (Hime.Kernel.Symbol Child in Symbol.Children)
            {
                string Type = string.Empty;
                if (!(Child is Hime.Kernel.Namespace))
                    Type = " [" + Child.GetType().Name + "]";
                TreeNode VChild = VNode.Nodes.Add(Child.LocalName + Type);
                AddSubTree(VChild, Child);
            }
        }
    }
}