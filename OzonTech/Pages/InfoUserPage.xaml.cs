using OzonTech.DB;
using OzonTech.MyWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для InfoUserPage.xaml
    /// </summary>
    public partial class InfoUserPage : Page
    {
        public static ObservableCollection<Users> listUser {  get; set; }
        public InfoUserPage()
        {
            InitializeComponent();
            listUser = new ObservableCollection<Users>(DbConnections.supportEntities.Users.ToList());
            UsersLv.ItemsSource = listUser;
            this.DataContext = this;
            foreach(Users item in listUser)
            {
                item.FIO = item.Surname + " " + item.Name;
            }

            CountTb.Text = "Кол-во записей:" + " " + UsersLv.Items.Count;



        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchFunc(SearchTb.Text.ToLower());


        }



        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        private void UsersLv_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Получаем элемент, на который нажали
            var clickedItem = e.OriginalSource as FrameworkElement;

            // Проверяем, не нажали ли мы на CheckBox
            if (clickedItem != null)
            {
                // Находим ListViewItem по клику
                var listViewItem = FindAncestor<ListViewItem>(clickedItem);
                if (listViewItem != null)
                {
                    // Получаем объект Users, связанный с этим элементом
                    var selectedUser = (Users)UsersLv.ItemContainerGenerator.ItemFromContainer(listViewItem);

                    if (selectedUser != null)
                    {
                        // Ищем CheckBox внутри элемента
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        bool isChecked = checkBox.IsChecked ?? false; // Состояние CheckBox

                        if (UsersLv.SelectedItem != null)
                        {
                            // Если есть уже выбранный элемент, снимаем отметку с него
                            var previouslySelectedItem = UsersLv.SelectedItem as Users;
                            var previousListViewItem = UsersLv.ItemContainerGenerator.ContainerFromItem(previouslySelectedItem) as ListViewItem;

                            if (previousListViewItem != null)
                            {
                                var previousCheckBox = FindVisualChild<CheckBox>(previousListViewItem);
                                if (previousCheckBox != null)
                                {
                                    previousCheckBox.IsChecked = false; // Снимаем отметку с предыдущего CheckBox
                                }
                            }
                        }

                        // Устанавливаем новое состояние
                        if (isChecked)
                        {
                            // Если CheckBox уже отмечен, снимаем отметку и сбрасываем выбор элемента
                            checkBox.IsChecked = false;
                            UsersLv.SelectedItem = null; // Сбрасываем выбранный элемент
                        }
                        else
                        {
                            // Устанавливаем выбор и отмечаем CheckBox
                            checkBox.IsChecked = true;
                            UsersLv.SelectedItem = selectedUser;

                            // Здесь можно обновить данные, связанные с `selectedUser`, если необходимо
                            //UpdateSelectedUserData(selectedUser);
                        }

                        // Устанавливаем событие как обработанное
                        e.Handled = true;
                    }
                }
            }
        }

        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T parent)
                {
                    return parent;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if(UsersLv.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя!");
            }
            else
            {
                DeleteUsers(UsersLv.SelectedItem as Users);
                UsersLv.SelectedItem = null;
                if(string.IsNullOrEmpty(SearchTb.Text))
                {
                    listUser = new ObservableCollection<Users>(DbConnections.supportEntities.Users.ToList());
                    UsersLv.ItemsSource = listUser;
                    CountTb.Text += " " + UsersLv.Items.Count;

                }
                else
                {

                    SearchFunc(SearchTb.Text.ToLower());
                }
                
            }
        }

        public void SearchFunc( string getText)
        {
            var allUsers = DbConnections.supportEntities.Users.ToList(); // или используйте .AsEnumerable() чтобы избежать задержек

            // Фильтруем пользователей
            listUser = new ObservableCollection<Users>(
                allUsers.Where(i =>
                    i.Name.ToLower().Contains(getText) ||
                    i.Surname.ToLower().Contains(getText) ||
                    i.Phone.Contains(getText) || // Возможно, стоит сделать ToLower()
                    i.Email.ToLower().Contains(getText) ||
                    i.UserName.ToLower().Contains(getText) ||
                    i.Birtday.ToString().Contains(getText)) // Используем нужный формат даты
            );

            // Устанавливаем ItemsSource для ListView
            UsersLv.ItemsSource = listUser;
            CountTb.Text = "Кол-во записей:" + " " + UsersLv.Items.Count;
        }
        public void DeleteUsers(Users delUser)
        {
            DbConnections.supportEntities.Users.Remove(delUser);
            DbConnections.supportEntities.SaveChanges();
        }

        private void EditeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersLv.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя!");
            }
            else
            {
                App.activateAdd = false;
                AddEditUserWindow addEditUserWindow = new AddEditUserWindow(UsersLv.SelectedItem as Users);
                addEditUserWindow.UsersUpdated += LoadUsersData;
                addEditUserWindow.ShowDialog();
            }
            
        }



        private void LoadUsersData()
        {
            // Пример получения обновленных данных из базы данных
            var upUsers = DbConnections.supportEntities.Users.ToList(); // или другой метод получения данных
            UsersLv.ItemsSource = upUsers; // Обновляем источник данных ListView
        }// Конец обновления данных

        private void SddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.activateAdd = true;
            AddEditUserWindow addEditUserWindow = new AddEditUserWindow(UsersLv.SelectedItem as Users);
            addEditUserWindow.UsersUpdated += LoadUsersData;
            addEditUserWindow.ShowDialog();
        }


    }
}
