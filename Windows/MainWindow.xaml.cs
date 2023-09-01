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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PishiStiray_Demo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrameMain.Navigate(new Pages.LoginPage());
        }
        bool flag = true;
        object cont = null;

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameMain.CanGoBack && MessageBox.Show($"Вы уверены, что хотите вернуться?\nНесохраненные данные могут быть утеряны",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                FrameMain.GoBack();
        }

        private void FrameMain_ContentRendered(object sender, EventArgs e)
        {

            if (flag)
            {
                cont = FrameMain.Content;
                flag = false;
            }


            if (App.CurrentUser != null)
                TBoxUsername.Text = App.CurrentUser.UserSurname + "\n" + App.CurrentUser.UserName + "\n" +
                    App.CurrentUser.UserPatronymic;
            else TBoxUsername.Text = "Гость"; 


          

        }
    }
}
