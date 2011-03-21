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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hime.HimeV.Forms
{
    /// <summary>
    /// Interaction logic for GrammarControl.xaml
    /// </summary>
    public partial class GrammarControl : UserControl
    {
        private string p_File;
        private Model.Grammar p_Grammar;

        public GrammarControl(string file)
        {
            p_File = file;
            InitializeComponent();
            Content.Text = System.IO.File.ReadAllText(p_File);
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (p_Grammar == null)
                return;
            Hime.Redist.Parsers.SyntaxTreeNode result = p_Grammar.Parse(TestData.Text);
            if (result == null)
                return;
            Forms.WinAST win = new Forms.WinAST(result);
            win.Show();
        }

        private void GrammarSave_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.WriteAllText(p_File, Content.Text);
        }

        private void GrammarCompile_Click(object sender, RoutedEventArgs e)
        {
            p_Grammar = new Model.Grammar(Content.Text);
            bool result = p_Grammar.GenerateParser();
            if (!result)
            {
                p_Grammar = null;
                GrammarStatus.Content = "Error";
                GrammarStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 50, 50));
            }
            else
            {
                GrammarStatus.Content = "Parser Generated";
                GrammarStatus.Foreground = new SolidColorBrush(Color.FromRgb(50, 255, 50));
            }
        }

        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {
            p_Grammar = null;
            GrammarStatus.Content = "Modified";
            GrammarStatus.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }
    }
}
