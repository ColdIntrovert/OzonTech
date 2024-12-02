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

namespace OzonTech.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPages.xaml
    /// </summary>
    public partial class MainMenuPages : Page
    {
        public MainMenuPages()
        {
            InitializeComponent();
            if(App.authUser.TypeUsers.Name == "Работник")
            {
                ManageUserBtn.Visibility = Visibility.Collapsed;
                CategoryBtn.Visibility = Visibility.Collapsed;
                DepartamentBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void ManageUserBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InfoUserPage());
        }

        private void CategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoryManagementPage());
        }

        private void DepartamentBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManagmentDepartamentPage());
        }

        private void ApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            OzonTech.DB.Application application = null;
            App.aplicationEdit = false;
            NavigationService.Navigate(new ApplicationPage( application));
        }

        private void LookApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LookApplicationPage());
        }
    }
}
