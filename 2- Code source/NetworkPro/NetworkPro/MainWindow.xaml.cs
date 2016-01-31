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

        private void btParcourAppli_Click(object sender, RoutedEventArgs e)
        {
            string appli;
            OpenFileDialog oFDAppli = new OpenFileDialog();
            oFDAppli.Filter = "Application|*.exe;*.msi";
            oFDAppli.ShowDialog();
            appli=oFDAppli.FileName;

            TabAppli.SelectedIndex = 0;

        }

        private void btAjoutAppli_Click(object sender, RoutedEventArgs e)
        {
            TabAppli.SelectedIndex = 1;
        }

        private void btWakeOnLane_Click(object sender, RoutedEventArgs e)
        {
            TabPlanif.SelectedIndex = 1;
        }

        private void btPlannifierDeploiment_Click(object sender, RoutedEventArgs e)
        {
            TabPlanif.SelectedIndex = 3;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var suppPlan = MessageBox.Show("Etes-vous sûr de vouloir supprimer cette planification ?", "Supression de planification", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (suppPlan == MessageBoxResult.Yes)
            {
                
            }
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            TabPlanif.SelectedIndex = 2;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TabPlanif.SelectedIndex = 1;
        }

       

        private void TabNetworkPro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*switch (TabNetworkPro.SelectedIndex)
            {
                case 1:
                    TabAppli.SelectedIndex = 0;
                    break;
                case 2:
                    TabPlanif.SelectedIndex = 0;
                    break;
            }*/
        }

        private void Retour_Menu_Click(object sender, RoutedEventArgs e)
        {
            TabPlanif.SelectedIndex = 0;
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            TabAppli.SelectedIndex = 0;
        }
    }
}
