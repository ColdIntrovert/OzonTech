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
using System.Collections.ObjectModel;
using OzonTech.DB;
namespace OzonTech.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            App.users = new ObservableCollection<Users>(DbConnections.supportEntities.Users.ToList());
            InitializeComponent();
        }

        private void CloseEyeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PasswordPb.Password = VisPassTb.Text;
            PasswordPb.Visibility = Visibility.Visible;
            VisPassTb.Visibility = Visibility.Collapsed;
            OpenEyeBtn.Visibility = Visibility.Visible;
            CloseEyeBtn.Visibility = Visibility.Collapsed;
        }

        private void OpenEyeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisPassTb.Text = PasswordPb.Password;
            PasswordPb.Visibility = Visibility.Collapsed;
            VisPassTb.Visibility = Visibility.Visible;
            OpenEyeBtn.Visibility = Visibility.Collapsed;
            CloseEyeBtn.Visibility = Visibility.Visible;
        }

        private void VisiblePassTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            PasswordPb.Password = VisPassTb.Text;
        }

        private void UserNameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void PasswordPb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void AuthBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.users.FirstOrDefault(i => i.Email == UserNameTb.Text && i.Password == PasswordPb.Password);
            
            if(currentUser == null)
            {
                MessageBox.Show("Пользователя с такими данными не надено!");
            }
            else
            {
                App.authUser = currentUser;
                NavigationService.Navigate(new MainMenuPages());
            }
        }
    }
}
