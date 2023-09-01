using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PishiStiray_Demo
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.Trade_demoEntities1 Context
        { get; } = new Entities.Trade_demoEntities1();
        public static Entities.User CurrentUser = null;
    }
}
