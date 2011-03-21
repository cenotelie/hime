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
    public partial class MainWindow
    {
        private Model.Grammar p_Project;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuFileNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFileLoad_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.DefaultExt = "gram";
            dialog.Multiselect = false;
            dialog.ShowDialog(this);
            if (dialog.FileName != null)
            {
                Forms.GrammarControl control = new Forms.GrammarControl(dialog.FileName);

                StackPanel header = new StackPanel();
                header.Orientation = Orientation.Horizontal;
                Label label = new Label();
                label.Content = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                Button close = new Button();
                close.Height = 20;
                close.Content = "X";
                header.Children.Add(label);
                header.Children.Add(close);

                TabItem item = new TabItem();
                item.Header = header;
                item.Content = control;
                Grammars.Items.Add(item);
                item.Focus();
            }
        }

        private void MenuFileSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFileSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFileQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
