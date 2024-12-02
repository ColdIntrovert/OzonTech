using Microsoft.Win32;
using OzonTech.Classes;
using OzonTech.DB;
using OzonTech.MyWindows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Логика взаимодействия для CategoryManagementPage.xaml
    /// </summary>
    public partial class CategoryManagementPage : Page
    {
        public static ObservableCollection<CategoryRequest> categories { get; set; }
        public static DB.CategoryRequest addCategory = new DB.CategoryRequest();
        public static DB.CategoryRequest editCategory = new DB.CategoryRequest();
        private string wallPhoto;
        private string newUri2;
        public CategoryManagementPage()
        {
            InitializeComponent();
            categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
            CategoriesLv.ItemsSource = categories;
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTb.Text))
            {
                AddCategoryWindow addCategoryWindow = new AddCategoryWindow("");
                addCategoryWindow.CategoryUpdated += LoadCategoryData;
                addCategoryWindow.ShowDialog();
            }
            else
            {
                AddCategoryWindow addCategoryWindow = new AddCategoryWindow(SearchTb.Text);
                addCategoryWindow.CategoryUpdated += LoadCategoryData;
                addCategoryWindow.ShowDialog();
            }
        }
        private void LoadCategoryData()
        {
            // Пример получения обновленных данных из базы данных
            var upCategory = DbConnections.supportEntities.CategoryRequest.ToList(); // или другой метод получения данных
            CategoriesLv.ItemsSource = upCategory; // Обновляем источник данных ListView
        }// Конец обновления данных


        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var allCategories = DbConnections.supportEntities.CategoryRequest.ToList(); // или используйте .AsEnumerable() чтобы избежать задержек
            string textSr = SearchTb.Text.ToLower();
            categories = new ObservableCollection<CategoryRequest>(allCategories.Where(i =>
     (i.Title != null && i.Title.ToLower().Contains(textSr)) ||
     (i.Description != null && i.Description.ToLower().Contains(textSr))
 ));

            // Устанавливаем ItemsSource для ListView
            CategoriesLv.ItemsSource = categories;
        }

        public void DeleteCategory(CategoryRequest delCategory)
        {
            DbConnections.supportEntities.CategoryRequest.Remove(delCategory);
            DbConnections.supportEntities.SaveChanges();

        }

        private void DelCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesLv.SelectedItem == null)
            {
                MessageBox.Show("Выберите категорию для удаления!");
            }
            else
            {
                DeleteCategory(CategoriesLv.SelectedItem as CategoryRequest);
                DbConnections.supportEntities.SaveChanges();
                CategoriesLv.SelectedItem = null;
                categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
                CategoriesLv.ItemsSource = categories;
                NameCategoryTb.Text = string.Empty;
                DescriptionTb.Text = string.Empty;
                MainImg.Source = new BitmapImage(new Uri(@"C:\Users\ramil\source\repos\OzonTech\OzonTech\Photos\NotPhoto.jpg"));
                MessageBox.Show("Категория успешно удалена!");
            }
        }

        private void ClearImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CategoriesLv.SelectedItem = null;
            NameCategoryTb.Text = string.Empty;
            DescriptionTb.Text = string.Empty;
            MainImg.Source = new BitmapImage(new Uri(@"C:\Users\ramil\source\repos\OzonTech\OzonTech\Photos\NotPhoto.jpg"));
        }

        private void addImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {


            var uniqueCategory = DbConnections.supportEntities.CategoryRequest.FirstOrDefault(i => i.Title == NameCategoryTb.Text);
            if (uniqueCategory != null)
            {
                MessageBox.Show("Категория с таким название уже есть!");

            }
            else
            {

                if (string.IsNullOrEmpty(NameCategoryTb.Text) || string.IsNullOrEmpty(DescriptionTb.Text) || string.IsNullOrEmpty(wallPhoto))
                {

                    MessageBox.Show("Заполните все поля или выберите другое фото!");

                }
                else if (wallPhoto != null)
                {
                    if (MainImg.Source is BitmapImage bitmapImage)
                    {
                        // Получаем UriSource из BitmapImage
                        string imageUri = bitmapImage.UriSource?.ToString();
                        // Теперь imageUri содержит строковое представление пути к изображению
                        string newUri = imageUri.Substring(8).Replace("/", "\\");
                        var uniquePhoto = DbConnections.supportEntities.CategoryRequest.FirstOrDefault(i => i.ImageData == newUri);

                        if (uniquePhoto != null)
                        {
                            MessageBox.Show("Выберите уникальное фото!");

                        }
                        else
                        {
                            addCategory.Title = NameCategoryTb.Text;
                            addCategory.Description = NameCategoryTb.Text;
                            addCategory.ImageData = wallPhoto;
                            DbConnections.supportEntities.CategoryRequest.Add(addCategory);
                            DbConnections.supportEntities.SaveChanges();
                            categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
                            CategoriesLv.ItemsSource = categories;
                            MessageBox.Show("Новая категория успешна добавлена!");
                        }
                    }


                }
                else
                {

                    addCategory.Title = NameCategoryTb.Text;
                    addCategory.Description = NameCategoryTb.Text;
                    addCategory.ImageData = wallPhoto;
                    DbConnections.supportEntities.CategoryRequest.Add(addCategory);
                    DbConnections.supportEntities.SaveChanges();
                    categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
                    CategoriesLv.ItemsSource = categories;
                    MessageBox.Show("Новая категория успешна добавлена!");

                }




            }
        }

        private string OpenImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                Title = "Выберите изображение"
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        private void SetServiceImage(string photoPath)
        {
            if (File.Exists(photoPath))
            {
                MainImg.Source = new BitmapImage(new Uri(photoPath, UriKind.Absolute));
            }
            else
            {
                MainImg.Source = null; // Заглушка
            }
        }

        private void CategoriesLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesLv.SelectedItem is CategoryRequest selectedCategory)
            {
                var listViewItem = (ListViewItem)CategoriesLv.ItemContainerGenerator.ContainerFromItem(selectedCategory);
                if (listViewItem != null)
                {
                    CategoriesLv.SelectedItem = selectedCategory;
                    NameCategoryTb.Text = selectedCategory.Title;
                    DescriptionTb.Text = selectedCategory.Description;
                    MainImg.Source = new BitmapImage(new Uri(selectedCategory.ImageData, UriKind.Absolute));
                    editCategory = selectedCategory; if (MainImg.Source is BitmapImage bitmapImage)
                    {
                        // Получаем UriSource из BitmapImage
                        string imageUri = bitmapImage.UriSource?.ToString();
                        // Теперь imageUri содержит строковое представление пути к изображению
                        newUri2 = imageUri.Substring(8).Replace("/", "\\");

                        
                    }

                }
                else
                {
                    CategoriesLv.SelectedItem = null;
                    NameCategoryTb.Text = string.Empty;
                    DescriptionTb.Text = string.Empty;
                    MainImg.Source = new BitmapImage(new Uri(@"C:\Users\ramil\source\repos\OzonTech\OzonTech\Photos\NotPhoto.jpg"));
                    editCategory = null;
                    newUri2 = null;
                }
            }
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string selectedPath = OpenImageDialog();

            // Обновляем свойство MainImagePath для editService
            if (selectedPath != null)
            {
                SetServiceImage(selectedPath);
                wallPhoto = selectedPath;
            }
        }

        private void SearchTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            CheckClass checkClass = new CheckClass();
            checkClass.CheckString(e);
        }

        private void CategoriesLv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CategoriesLv.SelectedItem = null;
            NameCategoryTb.Text = string.Empty;
            DescriptionTb.Text = string.Empty;
            MainImg.Source = new BitmapImage(new Uri(@"C:\Users\ramil\source\repos\OzonTech\OzonTech\Photos\NotPhoto.jpg"));
            editCategory = null;
            newUri2 = null;
        }

        private void EditCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CategoriesLv.SelectedItem != null)
            {
                
                if (MainImg.Source is BitmapImage bitmapImage)
                {
                    // Получаем UriSource из BitmapImage
                    string imageUri = bitmapImage.UriSource?.ToString();
                    // Теперь imageUri содержит строковое представление пути к изображению
                    string newUri = imageUri.Substring(8).Replace("/", "\\");
                    var uniquePhoto = DbConnections.supportEntities.CategoryRequest.FirstOrDefault(i => i.ImageData == newUri);

                    if (uniquePhoto != null)
                    {
                        if(uniquePhoto.ImageData == newUri2)
                        {
                            editCategory.Title = NameCategoryTb.Text;
                            editCategory.Description = DescriptionTb.Text;
                            editCategory.ImageData = newUri;
                            DbConnections.supportEntities.SaveChanges();
                            categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
                            CategoriesLv.ItemsSource = categories;
                            MessageBox.Show("Категория успешно изменена!");
                        }
                        else
                        {
                            MessageBox.Show("Выберите уникальное фото или предыдущую!");

                        }

                    }
                    else
                    {
                        editCategory.Title = NameCategoryTb.Text;
                        editCategory.Description = DescriptionTb.Text;
                        editCategory.ImageData = newUri;
                        DbConnections.supportEntities.SaveChanges();
                        categories = new ObservableCollection<CategoryRequest>(DbConnections.supportEntities.CategoryRequest.ToList());
                        CategoriesLv.ItemsSource = categories;
                        MessageBox.Show("Категория успешно изменена!");
                    }
                }


                   
                }
                else
                {
                    MessageBox.Show("Выберите категорию!");
                }
            }
        }
    }
