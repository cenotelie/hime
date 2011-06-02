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
                string action = "◌";
                if (Child.Action == Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote)
                    action = "↑";
                else if (Child.Action == Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop)
                    action = "↓";
                else if (Child.Action == Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace)
                    action = "≡";

                if (Child.Symbol != null)
                {
                    string name = Child.Symbol.Name;
                    string value = "";
                    if (Child.Symbol is Hime.Redist.Parsers.SymbolToken)
                        value = "\": " + ((Hime.Redist.Parsers.SymbolToken)Child.Symbol).Value.ToString() + "\"";

                    string header = action + " " + name + value;

                    VChild = VNode.Nodes.Add(header);
                }
                else
                {
                    VChild = VNode.Nodes.Add(action + " /!\\ null");
                }
                AddSubTree(VChild, Child);
            }
        }
    }
}