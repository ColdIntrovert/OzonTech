using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OzonTech.DB;

namespace OzonTech
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        public static ObservableCollection<Users> users = new ObservableCollection<Users>();
        public static bool activateAdd = false;
        public static bool activateDepartmentAdd = false;
        public static Users authUser;
        public static bool aplicationEdit = false;
    }
}
