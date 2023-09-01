using PishiStiray_Demo.Pages;
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

namespace PishiStiray_Demo.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public OrderWindow()
        {
            InitializeComponent();
        }
        public OrderWindow(List<Entities.Product> ProductCart)
        {
            InitializeComponent();
            FrameOrder.Navigate(new ProductCartPage(ProductCart));
            if (App.CurrentUser != null)
                TBoxUsername.Text = App.CurrentUser.UserSurname + "\n" + App.CurrentUser.UserName + "\n" +
                    App.CurrentUser.UserPatronymic;
            else TBoxUsername.Text = "Гость";
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrameOrder.CanGoBack && MessageBox.Show($"Вы уверены, что хотите вернуться?\nНесохраненные данные могут быть утеряны",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                FrameOrder.GoBack();
        }
    }
}
