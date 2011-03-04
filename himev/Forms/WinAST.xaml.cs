using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hime.HimeV.Forms
{
    /// <summary>
    /// Interaction logic for WinAST.xaml
    /// </summary>
    public partial class WinAST : Window
    {
        public WinAST(Hime.Redist.Parsers.SyntaxTreeNode Root)
        {
            InitializeComponent();
            TreeViewItem VRoot = new TreeViewItem();
            VRoot.Header = (Root.Symbol.Name);
            AddSubTree(VRoot, Root);
            Tree.Items.Add(VRoot);
        }

        private void AddSubTree(TreeViewItem VNode, Hime.Redist.Parsers.SyntaxTreeNode SNode)
        {
            foreach (Hime.Redist.Parsers.SyntaxTreeNode Child in SNode.Children)
            {
                TreeViewItem VChild = null;
                if (Child.Symbol != null)
                {
                    VChild = new TreeViewItem();
                    VNode.Items.Add(VChild);
                    if (Child.Symbol is Hime.Redist.Parsers.SymbolToken)
                        VChild.Header = (Child.Symbol.Name + " : \"" + ((Hime.Redist.Parsers.SymbolToken)Child.Symbol).Value.ToString() + "\"");
                    else
                        VChild.Header = (Child.Symbol.Name);
                }
                else
                {
                    VChild = new TreeViewItem();
                    VNode.Items.Add(VChild);
                    VChild.Header = ("/!\\ null");
                }
                AddSubTree(VChild, Child);
            }
        }
    }
}
