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
using System.Windows.Threading;

namespace PishiStiray_Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public bool isCapt = false;
        public string[] captIm = new string[] { "G:\\Demo\\Captcha\\1.png", "G:\\Demo\\Captcha\\2.png",
            "G:\\Demo\\Captcha\\3.png", "G:\\Demo\\Captcha\\4.png" };
        public string[] captNum = new string[] { "2379", "4851", "9142", "3198" };
        int curCapt = 0;
        int counter = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1),
        };
        public LoginPage()
        {
            InitializeComponent();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
        }
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter >= 10)
            {
                BtnLogin.IsEnabled = true;
                counter = 0;
                dispatcherTimer.Stop();
            }
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.Context.User
                .FirstOrDefault(p => p.UserLogin == TBoxLogin.Text && p.UserPassword == PBoxPassword.Password);
            if (!isCapt)
            {
                if (currentUser != null)
                {
                    App.CurrentUser = currentUser;
                    NavigationService.Navigate(new ProductPage());
                }
                else
                {
                    isCapt = true;
                    ImCapt.Visibility = Visibility.Visible;
                    TBCapt.Visibility = Visibility.Visible;
                    curCapt = (curCapt + 1) % 4;
                    ImCapt.Source = new BitmapImage(new Uri(captIm[curCapt]));
                    MessageBox.Show("Пользователь не найден.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (currentUser != null && TBCapt.Text == captNum[curCapt])
                {
                    App.CurrentUser = currentUser;
                    NavigationService.Navigate(new ProductPage());
                }
                else if (TBCapt.Text != captNum[curCapt])
                {
                    curCapt = (curCapt + 1) % 4;
                    ImCapt.Source = new BitmapImage(new Uri(captIm[curCapt]));
                    MessageBox.Show("Неправильно введена капча.\nПопробуйте ещё раз через 10 секунд", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    dispatcherTimer.Start();
                    BtnLogin.IsEnabled = false;
                }
                else
                {
                    curCapt = (curCapt + 1) % 4;
                    ImCapt.Source = new BitmapImage(new Uri(captIm[curCapt]));
                    MessageBox.Show("Пользователь не найден.\nПопробуйте ещё раз через 10 секунд", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    dispatcherTimer.Start();
                    BtnLogin.IsEnabled = false;
                }
                TBCapt.Text = String.Empty;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isCapt = false;
            ImCapt.Visibility = Visibility.Collapsed;
            TBCapt.Visibility = Visibility.Collapsed;
            TBoxLogin.Text = String.Empty;
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage());
        }
    }
}
