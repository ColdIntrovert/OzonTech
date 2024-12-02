using OzonTech.Classes;
using OzonTech.DB;
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
    /// Логика взаимодействия для ApplicationPage.xaml
    /// </summary>
    public partial class ApplicationPage : Page
    {
        CheckClass checkClass = new CheckClass();
        public static ObservableCollection<CategoryRequest> categoryRequests = new ObservableCollection<CategoryRequest>();
        public static ObservableCollection<Departments> departments = new ObservableCollection<Departments>();

        public static DB.Application addApplication = new DB.Application();
        public static DB.Application editApplication = new DB.Application();

        public ApplicationPage(OzonTech.DB.Application infoApplication)
        {
            InitializeComponent();
            categoryRequests = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
            CategoriesLv.ItemsSource = categoryRequests;
            if (App.aplicationEdit == false)
            {
                NameTb.Text = string.Empty;
                ComputerTb.Text = string.Empty;
                DescriptionTb.Text = string.Empty;
                CategoriesLv.SelectedItem = null;
                PlaceCb.SelectedItem = null;
                Border1.Visibility = Visibility.Visible;
                Border2.Visibility = Visibility.Collapsed;
                Border3.Visibility = Visibility.Visible;
                Border4.Visibility = Visibility.Collapsed;

                departments = new ObservableCollection<Departments>(DbConnections.supportEntities.Departments.ToList());
                PlaceCb.ItemsSource = departments;
            }
            else
            {

                departments = new ObservableCollection<Departments>(DbConnections.supportEntities.Departments.ToList());
                PlaceCb.ItemsSource = departments;

                editApplication = infoApplication;
                NameTb.Text = infoApplication.Name;
                ComputerTb.Text = infoApplication.Name_Computer;
                DescriptionTb.Text = infoApplication.Description;
                // Устанавливаем выбранный элемент в CategoriesLv
                CategoriesLv.SelectedItem = categoryRequests.FirstOrDefault(c => c.Id_Category == infoApplication.CategoryRequest.Id_Category);

                // Устанавливаем выбранный элемент в PlaceCb
                PlaceCb.SelectedItem = departments.FirstOrDefault(d => d.Id_Depart == infoApplication.Departments.Id_Depart);

                Border1.Visibility = Visibility.Collapsed;
                Border2.Visibility = Visibility.Visible;
                Border3.Visibility = Visibility.Collapsed;
                Border4.Visibility = Visibility.Visible;

            }


        }

        private void SurnameTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            checkClass.CheckString(e);
        }

        private void AddApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox[] {NameTb, ComputerTb, DescriptionTb};
            if (textBox.Any(i => string.IsNullOrEmpty(i.Text)) || CategoriesLv.SelectedItem == null || PlaceCb.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля и выберите категорию!");
            }
            else
            {
                var infCategory = CategoriesLv.SelectedItem as CategoryRequest;
                var infDepart = PlaceCb.SelectedItem as Departments;
                addApplication.Name = NameTb.Text;
                addApplication.Name_Computer = ComputerTb.Text;
                addApplication.Description = DescriptionTb.Text;
                addApplication.DepartamentId = Convert.ToInt32(infDepart.Id_Depart);
                addApplication.Status = "Создано";
                addApplication.CreateOn = DateTime.Now;
                addApplication.UserId = App.authUser.Id_Users;
                addApplication.RequestCategoryId = infCategory.Id_Category;
                DbConnections.supportEntities.Application.Add(addApplication);
                DbConnections.supportEntities.SaveChanges();
                MessageBox.Show("Заявка успешно создана!");
            }
        }

        private void CategoriesLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCategory = CategoriesLv.SelectedItem as CategoryRequest; // Замените YourCategoryType на ваш тип данных

           
                    // Первое нажатие, отображаем данные
                    CategoryTb.Text = selectedCategory.Title; // Предполагается, что у вашего типа есть свойство Title

                    // Устанавливаем изображение
                    if (!string.IsNullOrEmpty(selectedCategory.ImageData))
                    {
                        CategoryImage.Source = new BitmapImage(new Uri(selectedCategory.ImageData, UriKind.Absolute));
                    }


        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            
                NavigationService.Navigate(new MainMenuPages());



            
        }

        private void EditApplicationBtn_Click(object sender, RoutedEventArgs e)
        {
            var textBox = new TextBox[] { NameTb, ComputerTb, DescriptionTb };
            if (textBox.Any(i => string.IsNullOrEmpty(i.Text)) || CategoriesLv.SelectedItem == null || PlaceCb.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля и выберите категорию!");
            }
            else
            {
                var infCategory = CategoriesLv.SelectedItem as CategoryRequest;
                var infDepart = PlaceCb.SelectedItem as Departments;
                editApplication.Name = NameTb.Text;
                editApplication.Name_Computer = ComputerTb.Text;
                editApplication.Description = DescriptionTb.Text;
                editApplication.DepartamentId = Convert.ToInt32(infDepart.Id_Depart);
                editApplication.Status = "Создано";
                editApplication.CreateOn = DateTime.Now;
                editApplication.UserId = App.authUser.Id_Users;
                editApplication.RequestCategoryId = infCategory.Id_Category;
                editApplication.StatusUpdatedOn = DateTime.Now;
                DbConnections.supportEntities.SaveChanges();
                MessageBox.Show("Заявка успешно изменена!");
            }
        }

        private void Back2Btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LookApplicationPage());

        }
    }
}
