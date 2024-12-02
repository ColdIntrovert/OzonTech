using Microsoft.Win32;
using OzonTech.Classes;
using OzonTech.DB;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
using System.Windows.Shapes;

namespace OzonTech.MyWindows
{
    /// <summary>
    /// Логика взаимодействия для AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public event Action CategoryUpdated;

        private string wallPhoto;
        public static DB.CategoryRequest addRequest = new DB.CategoryRequest();
        public AddCategoryWindow(string title)
        {
            InitializeComponent();
            NameTb.Text = title;
            
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
                CategoryImg.Source = new BitmapImage(new Uri(photoPath, UriKind.Absolute));
            }
            else
            {
                CategoryImg.Source = null; // Заглушка
            }
        }

        private void CategoryImg_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string selectedPath = OpenImageDialog();

            // Обновляем свойство MainImagePath для editService
            if (selectedPath != null)
            {
                SetServiceImage(selectedPath);
                wallPhoto = selectedPath;
            }
        }

        private void AddCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(wallPhoto) || string.IsNullOrEmpty(NameTb.Text) || string.IsNullOrEmpty(DescriptionTb.Text))
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                addRequest.Title = NameTb.Text;
                addRequest.Description = DescriptionTb.Text;
                addRequest.ImageData = wallPhoto;
                DbConnections.supportEntities.CategoryRequest.Add(addRequest);
                DbConnections.supportEntities.SaveChanges();
                CategoryUpdated?.Invoke();
                MessageBox.Show("Категория успешно добавлена!");

            }
        }

        private void CloseCategoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NameTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли введенный символ числом
            CheckClass checkClass = new CheckClass();
            checkClass.CheckString(e);
        }
    }
}
