using OzonTech.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace OzonTech.MyWindows
{
    /// <summary>
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        public event Action UsersUpdated;

        public static ObservableCollection<TypeUsers> typeUsers = new ObservableCollection<TypeUsers>();    
        public static DB.Users addUsers = new DB.Users();
        public static DB.Users editUsers = new DB.Users();

        public AddEditUserWindow(Users infoUser)
        {
            InitializeComponent();
            editUsers = infoUser;
            typeUsers = new ObservableCollection<TypeUsers>(DbConnections.supportEntities.TypeUsers.ToList());
            if(App.activateAdd == false)
            {
                EditBtn.Visibility = Visibility.Visible;
                AddBtn.Visibility = Visibility.Collapsed;
                NameTb.Text = infoUser.Name;
                SurnameTb.Text = infoUser.Surname;
                PasswordPb.Password = infoUser.Password;
                USTb.Text = infoUser.UserName;
                PhoneTb.Text = infoUser.Phone;
                EmailTb.Text = infoUser.Email;
                DateDp.SelectedDate = infoUser.Birtday;
                if (infoUser.Type != null)
                {
                    TypeUserCb.SelectedItem = typeUsers.FirstOrDefault(t => t.Id_Type == infoUser.Type);
                }
            }
            else
            {
                EditBtn.Visibility = Visibility.Collapsed;
                AddBtn.Visibility = Visibility.Visible;
            }
            TypeUserCb.ItemsSource = typeUsers;
            TypeUserCb.DisplayMemberPath = "Name";

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

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox[] {NameTb, SurnameTb, PhoneTb, USTb, EmailTb};
            if(textBox.Any(x => string.IsNullOrEmpty(x.Text)) || DateDp.SelectedDate == null || TypeUserCb.SelectedItem == null || string.IsNullOrEmpty(PasswordPb.Password))
            {
                MessageBox.Show("Заполните всес поля!");
            }
            else
            {
                editUsers.Name = NameTb.Text;
                editUsers.Surname = SurnameTb.Text;
                editUsers.Phone = PhoneTb.Text;
                editUsers.Email = EmailTb.Text;
                editUsers.UserName = USTb.Text;
                editUsers.FIO = SurnameTb.Text + " " + NameTb.Text;
                editUsers.Password = PasswordPb.Password;
                editUsers.Birtday = DateDp.SelectedDate;
                editUsers.UpdatedOn = DateTime.Now;
                editUsers.Type = (TypeUserCb.SelectedItem as TypeUsers).Id_Type;
                DbConnections.supportEntities.SaveChanges();
                UsersUpdated?.Invoke();

                MessageBox.Show("Пользователь успешно изменён!");

            }

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox[] { NameTb, SurnameTb, PhoneTb, USTb, EmailTb };
            if (textBox.Any(x => string.IsNullOrEmpty(x.Text)) || DateDp.SelectedDate == null || TypeUserCb.SelectedItem == null || string.IsNullOrEmpty(PasswordPb.Password))
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                addUsers.Name = NameTb.Text;
                addUsers.Surname = SurnameTb.Text;
                addUsers.Phone = PhoneTb.Text;
                addUsers.Email = EmailTb.Text;
                editUsers.FIO = SurnameTb.Text + " " + NameTb.Text;
                addUsers.UserName = USTb.Text;
                addUsers.Password = PasswordPb.Password;
                addUsers.Birtday = DateDp.SelectedDate;
                editUsers.CreatedOn = DateTime.Now;
                addUsers.Type = (TypeUserCb.SelectedItem as TypeUsers).Id_Type;
                DbConnections.supportEntities.Users.Add(addUsers);
                DbConnections.supportEntities.SaveChanges();
                UsersUpdated?.Invoke();

                MessageBox.Show("Пользователь успешно добавлен!");
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SurnameTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            if (char.IsDigit(e.Text[0]))
            {
                // Если это число, отменяем ввод
                e.Handled = true;
            }
        }

        private void PhoneTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+()-]");
            e.Handled = regex.IsMatch(e.Text);
        }


    }
}
