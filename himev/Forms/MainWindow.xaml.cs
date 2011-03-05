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

namespace Hime.HimeV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model.Project p_Project;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (p_Project == null)
                return;
            Hime.Redist.Parsers.SyntaxTreeNode result = p_Project.Parse(TestData.Text);
            result = result.ApplyActions();
            if (result == null)
                return;
            Forms.WinAST win = new Forms.WinAST(result);
            win.Show();
        }

        private void Box_DragEnter(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            object text = e.Data.GetData(DataFormats.FileDrop);
            string file = ((string[])text)[0];
            foreach (TabItem tabitem in Grammars.Items)
                if (tabitem.Tag.Equals(file))
                    return;
            
            TextBox box = new TextBox();
            box.AcceptsReturn = true;
            box.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            box.Text = System.IO.File.ReadAllText(file);
            box.PreviewDragEnter += new DragEventHandler(Box_DragEnter);
            box.PreviewDragOver += new DragEventHandler(Box_DragEnter);
            box.Drop += new DragEventHandler(DockPanel_Drop);
            box.TextChanged += new TextChangedEventHandler(TextGrammar_TextChanged);

            TabItem item = new TabItem();
            item.Header = System.IO.Path.GetFileName(file);
            item.Tag = file;
            item.Content = box;
            Grammars.Items.Add(item);
            Grammars.SelectedItem = item;
        }

        private void Compile_Click(object sender, RoutedEventArgs e)
        {
            string selection = ((ComboBoxItem)ComboMethod.SelectedValue).Content.ToString();
            selection = selection.Replace("(", "").Replace(")", "");
            Hime.Parsers.ParsingMethod method = (Hime.Parsers.ParsingMethod)System.Enum.Parse(typeof(Hime.Parsers.ParsingMethod), selection);
            p_Project = new Model.Project(TextGrammarName.Text, method);
            foreach (TabItem tabitem in Grammars.Items)
            {
                string text = ((TextBox)tabitem.Content).Text;
                string file = (string)tabitem.Tag;
                System.IO.File.WriteAllText(file, text);
                p_Project.AddFile(file);
            }
            bool result = p_Project.RegenerateParser();
            if (result)
                TextGrammarName.Background = new SolidColorBrush(Color.FromArgb(50, 0, 255, 0));
            else
                TextGrammarName.Background = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0));
        }

        private void TextGrammar_TextChanged(object sender, TextChangedEventArgs e)
        {
            p_Project = null;
            TextGrammarName.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
