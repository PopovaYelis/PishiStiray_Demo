using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace PishiStiray_Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateOrderPage.xaml
    /// </summary>
    public partial class CreateOrderPage : Page
    {
        string code = String.Empty;
        DateTime deliveryDate;
        List<Entities.Product> orderProduct = new List<Entities.Product>();
        int newOrderId = App.Context.Order.Max(p => p.OrderID) + 1;
        public CreateOrderPage()
        {
            InitializeComponent();
        }
        public CreateOrderPage(List<Entities.Product> OrderList, decimal allCost, decimal discCost, decimal finalCost)
        {
            InitializeComponent();
            orderProduct = OrderList;
            TBlockName.Text = App.CurrentUser.UserSurname + " "
                + App.CurrentUser.UserName + " " + App.CurrentUser.UserPatronymic;
            foreach (Entities.Product itemOrder in orderProduct)
            {
                string info = $"{itemOrder.ProductArticleNumber}, {itemOrder.ProductName}, кол-во: {itemOrder.ItemCounter}";
                LBItems.Items.Add(info);
            }
            TBlockDateOrder.Text = DateTime.Now.ToShortDateString();
            bool allOver = true;
            foreach (Entities.Product itemCheck in orderProduct)
                if (itemCheck.ProductQuantityInStock < 3)
                    allOver = false;
            if (allOver)
                deliveryDate = DateTime.Now + TimeSpan.FromDays(3);
            else
                deliveryDate = DateTime.Now + TimeSpan.FromDays(6);
            TBlockDateDelivery.Text = deliveryDate.ToShortDateString();
            TBlockOrderNum.Text = newOrderId.ToString();
            TBlockAllCost.Text = allCost.ToString("N2");
            TBlockAllDisc.Text = discCost.ToString("N2");
            TBlockFinPrice.Text = finalCost.ToString("N2");
            var pickupPoint = App.Context.PickupPoint.Select(p => p.PointAddress).ToArray();
            for (int i = 0; i < pickupPoint.Length; i++)
                CBPickup.Items.Add(pickupPoint[i]);
            Random a = new Random();
            code = a.Next(0, 10).ToString();
            code += a.Next(0, 10).ToString();
            code += a.Next(0, 10).ToString();
            TBlockCode.Text = code;
        }

        private void BtnCrtOrd_Click(object sender, RoutedEventArgs e)
        {
            if (CBPickup.SelectedItem != null)
            {
                var order = new Entities.Order
                {
                    OrderID = newOrderId,
                    OrderStatus = 1,
                    OrderCreateDate = DateTime.Now,
                    OrderDeliveryDate = deliveryDate,
                    OrderPickupPoint = App.Context.PickupPoint.Where(p => p.PointAddress == CBPickup.SelectedItem.ToString())
                    .Select(p => p.PointID).FirstOrDefault(),
                    OrderCode = Convert.ToInt32(code),
                    IDUser = App.CurrentUser.UserID
                };
                App.Context.Order.Add(order);
                foreach (Entities.Product itemOrder in orderProduct)
                {
                    var orderProduct = new Entities.OrderProduct
                    {
                        OrderID = newOrderId,
                        ProductArticleNumber = itemOrder.ProductArticleNumber,
                        ProductCount = itemOrder.ItemCounter
                    };
                    App.Context.OrderProduct.Add(orderProduct);
                }
                App.Context.SaveChanges();
                BtnSaveToFile.Visibility = Visibility.Visible;
                BtnCrtOrd.Visibility = Visibility.Collapsed;
                CBPickup.IsEnabled = false;
                if (MessageBox.Show($"Ваш заказ успешно создан\nХотите сохранить талон в формате PDF?",
                "Успех", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    SaveToPdf();
                }
            }
            else
                MessageBox.Show("Выберите пункт выдачи", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveToPdf();
        }
        private void SaveToPdf()
        {

        }
    }
}