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
        private string p_File;

        public MainWindow()
        {
            InitializeComponent();

            //System.Diagnostics.Process.GetCurrentProcess().StandardOutput.BaseStream;
        }

        private void Grammar_PreviewDragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
            e.Handled = true;
        }

        private void Grammar_Drop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            p_File = ((string[])text)[0];
            Grammar.Text = System.IO.File.ReadAllText(p_File);
            this.Title = "HimeV - " + p_File;
            e.Handled = true;
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            System.IO.File.WriteAllText(p_File, Grammar.Text);
            string selection = (string)ComboMethod.SelectedValue;
            selection = selection.Replace("(", "").Replace(")", "");
            Hime.Parsers.ParsingMethod method = (Hime.Parsers.ParsingMethod)System.Enum.Parse(typeof(Hime.Parsers.ParsingMethod), selection);
            Model.Project Project = new Model.Project(p_File, GrammarName.Text, method);
            Project.RegenerateParser();
            Project.Parse(TestData.Text);
        }
    }
}
