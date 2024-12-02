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
    /// Логика взаимодействия для ManagmentDepartamentPage.xaml
    /// </summary>
    public partial class ManagmentDepartamentPage : Page
    {
        public static ObservableCollection<Departments> departments {  get; set; }
        public static Departments notInfo;
        public ManagmentDepartamentPage()
        {
            InitializeComponent();
            LoadDepartmentsUpdate();
            CountTb.Text = "Кол-во записей:" + " " + DepartamentLv.Items.Count;
            
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

        private void DepartamentLv_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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
                    var selectedUser = (Departments)DepartamentLv.ItemContainerGenerator.ItemFromContainer(listViewItem);

                    if (selectedUser != null)
                    {
                        // Ищем CheckBox внутри элемента
                        var checkBox = FindVisualChild<CheckBox>(listViewItem);
                        bool isChecked = checkBox.IsChecked ?? false; // Состояние CheckBox

                        if (DepartamentLv.SelectedItem != null)
                        {
                            // Если есть уже выбранный элемент, снимаем отметку с него
                            var previouslySelectedItem = DepartamentLv.SelectedItem as Departments;
                            var previousListViewItem = DepartamentLv.ItemContainerGenerator.ContainerFromItem(previouslySelectedItem) as ListViewItem;

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
                            DepartamentLv.SelectedItem = null; // Сбрасываем выбранный элемент
                        }
                        else
                        {
                            // Устанавливаем выбор и отмечаем CheckBox
                            checkBox.IsChecked = true;
                            DepartamentLv.SelectedItem = selectedUser;

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
            if(DepartamentLv.SelectedItem != null)
            {
                DeleteDepartaments(DepartamentLv.SelectedItem as Departments);
                DepartamentLv.SelectedItem = null;
                departments = new ObservableCollection<Departments>(DbConnections.supportEntities.Departments.ToList());
                DepartamentLv.ItemsSource = departments;
                CountTb.Text = "Кол-во записей:" + " " + DepartamentLv.Items.Count;

            }
            else
            {
                MessageBox.Show("Выберите отдел для удаления!");
            }
        }

        private void EditeBtn_Click(object sender, RoutedEventArgs e)
        {
            if(DepartamentLv.SelectedItem != null)
            {
                App.activateDepartmentAdd = false;
                AddDepartmentWindow addDepartmentWindow = new AddDepartmentWindow(DepartamentLv.SelectedItem as Departments);
                addDepartmentWindow.DepartmensUpdated += LoadDepartmentsUpdate;
                addDepartmentWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите отдел для изменения!");
            }
        }

        private void SddBtn_Click(object sender, RoutedEventArgs e)
        {
            App.activateDepartmentAdd = true;
            AddDepartmentWindow addDepartmentWindow = new AddDepartmentWindow(notInfo);
            addDepartmentWindow.DepartmensUpdated += LoadDepartmentsUpdate;
            addDepartmentWindow.ShowDialog();
        }
        
        public void LoadDepartmentsUpdate()
        {
            departments = new ObservableCollection<Departments>(DbConnections.supportEntities.Departments.ToList());
            DepartamentLv.ItemsSource = departments;
            DepartamentLv.SelectedItem = null;
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchFunc(SearchTb.Text.ToLower());
        }

        public void SearchFunc(string getText)
        {
            var filterDepartament = DbConnections.supportEntities.Departments.ToList(); // или используйте .AsEnumerable() чтобы избежать задержек

            // Фильтруем пользователей
            departments = new ObservableCollection<Departments>(
                filterDepartament.Where(i =>
                    i.Title.ToLower().Contains(getText) ||
                    i.Room.ToLower().Contains(getText) ||
                    i.Place.Contains(getText))
            );

            // Устанавливаем ItemsSource для ListView
            DepartamentLv.ItemsSource = departments;
            CountTb.Text = "Кол-во записей:" + " " + DepartamentLv.Items.Count;
        }

        public void DeleteDepartaments(Departments delDepar)
        {
            DbConnections.supportEntities.Departments.Remove(delDepar);
            DbConnections.supportEntities.SaveChanges();
        }

    }
}
