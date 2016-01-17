using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetworkPro
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ParcourAppli_Click(object sender, RoutedEventArgs e)
        {
            string appli;
            OpenFileDialog oFDAppli = new OpenFileDialog();
            oFDAppli.Filter = "Application|*.exe;*.msi";
            oFDAppli.ShowDialog();
            appli=oFDAppli.FileName;

            TabAppli.SelectedIndex = 0;

        }

        private void AjoutAppli_Click(object sender, RoutedEventArgs e)
        {
            TabAppli.SelectedIndex = 1;
        }

        

        

      

        

       
    }
}
