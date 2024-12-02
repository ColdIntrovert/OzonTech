using OzonTech.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
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
    /// Логика взаимодействия для LookApplicationPage.xaml
    /// </summary>
    public partial class LookApplicationPage : Page
    {
        public static ObservableCollection<OzonTech.DB.Application> applications = new ObservableCollection<OzonTech.DB.Application>();
        public static ObservableCollection<CategoryRequest> categoryRequests = new ObservableCollection<CategoryRequest>();
        public static ObservableCollection<Departments> departments = new ObservableCollection<Departments>();
        public static ObservableCollection<Users> users = new ObservableCollection<Users>();



        public static DB.Application addApplications = new DB.Application();
        public LookApplicationPage()
        {
            InitializeComponent();
            applications = new ObservableCollection<OzonTech.DB.Application>(DbConnections.supportEntities.Application.Where(x => x.Status == "Создано").ToList());

            ApplicationsLv.ItemsSource = applications;
            
            categoryRequests = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
            CategoryTb.ItemsSource = categoryRequests;
            CategoryTb.DisplayMemberPath = "Title";
            departments = new ObservableCollection<Departments>(DbConnections.supportEntities.Departments.ToList());
            DepartamentTb.ItemsSource = departments;
            DepartamentTb.DisplayMemberPath = "Title";

            users = new ObservableCollection<Users>(DbConnections.supportEntities.Users.ToList());
            UsersTb.ItemsSource = users;

        }

        private void ClearImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SearchTb.Text = string.Empty;
            CategoryTb.SelectedItem = null;
            UsersTb.SelectedItem = null;
            DepartamentTb.SelectedItem = null;
            StatusTb.SelectedItem = null;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }



        private void ApplyFilters()
        {
            var allApplications = DbConnections.supportEntities.Application.ToList();
            string searchText = SearchTb.Text.ToLower();

            // Получаем выбранные значения из комбобоксов
            var selectedCategory = CategoryTb.SelectedItem as CategoryRequest;
            var selectedDepartment = DepartamentTb.SelectedItem as Departments;
            var selectedUser = UsersTb.SelectedItem as Users;

            // Получаем выбранный статус
            string selectedStatus = (StatusTb.SelectedItem as ComboBoxItem)?.Tag.ToString();

            // Применяем фильтрацию
            var filteredApplications = allApplications.Where(a =>
                (string.IsNullOrEmpty(searchText) ||
                 (a.Name != null && a.Name.ToLower().Contains(searchText)) ||
                 (a.Description != null && a.Description.ToLower().Contains(searchText))) &&

                (selectedCategory == null ||
                 selectedCategory.Title == "Пусто" || // Если выбрано "Пусто", пропускаем фильтрацию
                 a.RequestCategoryId == selectedCategory.Id_Category) &&

                (selectedDepartment == null ||
                 selectedDepartment.Title == "Пусто" || // Если выбрано "Пусто", пропускаем фильтрацию
                 a.DepartamentId == selectedDepartment.Id_Depart) &&

                (selectedUser == null ||
                 selectedUser.Name == "Пусто" || // Если выбрано "Пусто", пропускаем фильтрацию
                 a.UserId == selectedUser.Id_Users) &&

        (string.IsNullOrEmpty(selectedStatus) ||
         selectedStatus == "Пусто" || // Если выбрано "Пусто", пропускаем фильтрацию
         a.Status == selectedStatus)); // Фильтрация по статусу            );

            applications = new ObservableCollection<OzonTech.DB.Application>(filteredApplications);

            // Устанавливаем ItemsSource для ListView
            ApplicationsLv.ItemsSource = applications;
            CountTb.Text = "Кол-во записей: " + applications.Count; // Обновляем количество записей
        }



        private void ApplicationsLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CategoryTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();

        }

        private void UsersTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();

        }

        private void StatusTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();

        }

        private void DepartamentTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            // Получаем родительский ListViewItem
            var item = FindAncestor<ListViewItem>(button);
            if (item != null)
            {
                // Получаем данные из DataContext элемента
                var serviceData = item.DataContext as OzonTech.DB.Application; // замените YourServiceModel на ваш класс модели

                DbConnections.supportEntities.Application.Remove(serviceData);
                DbConnections.supportEntities.SaveChanges();

                applications = new ObservableCollection<OzonTech.DB.Application>(DbConnections.supportEntities.Application.Where(x => x.Status == "Создано").ToList());
                ApplicationsLv.ItemsSource = applications;
                CountTb.Text = "Кол-во записей:" + " " + ApplicationsLv.Items.Count;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            App.aplicationEdit = true;
            // Получаем родительский ListViewItem
            var item = FindAncestor<ListViewItem>(button);
            if (item != null)
            {
                // Получаем данные из DataContext элемента
                var serviceData = item.DataContext as OzonTech.DB.Application; // замените YourServiceModel на ваш класс модели
               NavigationService.Navigate(new ApplicationPage(serviceData));
            }
        }

        private void AccertBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            // Получаем родительский ListViewItem
            var item = FindAncestor<ListViewItem>(button);
            if (item != null)
            {
                // Получаем данные из DataContext элемента
                var serviceData = item.DataContext as OzonTech.DB.Application; // замените YourServiceModel на ваш класс модели
                addApplications = serviceData;
                addApplications.Status = "Готово";
                DbConnections.supportEntities.SaveChanges();

                applications = new ObservableCollection<OzonTech.DB.Application>(DbConnections.supportEntities.Application.Where(x => x.Status == "Создано").ToList());
                ApplicationsLv.ItemsSource = applications;
                CountTb.Text = "Кол-во записей:" + " " + ApplicationsLv.Items.Count;
            }
        }


        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T variable)
                {
                    return variable;
                }

                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        private StackPanel FindChildByName(DependencyObject parent, string childName)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is StackPanel panel && panel.Name == childName)
                {
                    return panel;
                }

                var foundChild = FindChildByName(child, childName);
                if (foundChild != null)
                {
                    return foundChild;
                }
            }
            return null;
        }

        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void CleanCategoryImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CategoryTb.SelectedItem = null;
        }

        private void CleanUsersImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UsersTb.SelectedItem = null;
        }

        private void CleanStatusImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StatusTb.SelectedItem = null;
        }

        private void CleanDepartmentImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DepartamentTb.SelectedItem = null;
        }
    }
}
