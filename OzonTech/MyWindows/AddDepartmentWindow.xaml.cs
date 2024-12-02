using OzonTech.DB;
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
using System.Windows.Shapes;

namespace OzonTech.MyWindows
{
    /// <summary>
    /// Логика взаимодействия для AddDepartmentWindow.xaml
    /// </summary>
    public partial class AddDepartmentWindow : Window
    {
        public event Action DepartmensUpdated;
        public static DB.Departments addDepartmens = new DB.Departments(); 
        public static DB.Departments editDepartmens = new DB.Departments();
        public AddDepartmentWindow(Departments infoDepartments)
        {
            InitializeComponent();
            if(App.activateDepartmentAdd == true)
            {
                AddBorder.Visibility = Visibility.Visible;
                EditBorder.Visibility = Visibility.Collapsed;
                NameTb.Text = string.Empty;
                PlaceTb.Text = string.Empty;
                RoomTb.Text = string.Empty;
                editDepartmens = infoDepartments;

            }
            else
            {
                AddBorder.Visibility = Visibility.Collapsed;
                EditBorder.Visibility = Visibility.Visible;
                NameTb.Text = infoDepartments.Title; 
                PlaceTb.Text = infoDepartments.Place;
                RoomTb.Text = infoDepartments.Room;
                editDepartmens = infoDepartments;
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var textbox = new TextBox[] { NameTb, PlaceTb, RoomTb };
            if (textbox.Any(i => string.IsNullOrEmpty(i.Text)))
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                addDepartmens.Title = NameTb.Text;
                addDepartmens.Place = PlaceTb.Text;
                addDepartmens.Room = RoomTb.Text;

                DbConnections.supportEntities.Departments.Add(editDepartmens);
                DbConnections.supportEntities.SaveChanges();
                DepartmensUpdated?.Invoke();
                MessageBox.Show("Новый отдел успешно добавлен!");
            }


        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var textbox = new TextBox[] { NameTb, PlaceTb, RoomTb };
            if (textbox.Any(i => string.IsNullOrEmpty(i.Text)))
            {

                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                editDepartmens.Title = NameTb.Text;
                editDepartmens.Place = PlaceTb.Text;
                editDepartmens.Room = RoomTb.Text;
                DbConnections.supportEntities.SaveChanges();
                DepartmensUpdated?.Invoke();
                MessageBox.Show("Отдел успешно изменён!");
            }

        }



    }
}
