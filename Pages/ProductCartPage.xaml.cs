using PishiStiray_Demo.Windows;
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

namespace PishiStiray_Demo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProductCartPage.xaml
    /// </summary>
    public partial class ProductCartPage : Page
    {
        List<Entities.Product> productInCart = new List<Entities.Product>();
        decimal allCost = 0;
        decimal discCost;
        decimal finalCost = 0;
        public ProductCartPage()
        {
            InitializeComponent();
        }
        public ProductCartPage(List<Entities.Product> ProductCart)
        {
            InitializeComponent();
            productInCart = ProductCart;
            UpdateCart();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Entities.Product;
            if (MessageBox.Show($"Вы уверены, что хотите удалить товар: {currentProduct.ProductName}\nАртикль: " +
                $"{currentProduct.ProductArticleNumber}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeleteItem(currentProduct);
                UpdateCart();
            }
        }
        private void UpdateCart()
        {
            LViewOrder.Items.Clear();
            foreach (Entities.Product productItem in productInCart)
            {
                if (!LViewOrder.Items.Contains(productItem) && productItem.ItemCounter != 0)
                {
                    LViewOrder.Items.Add(productItem);
                }
            }
            if (productInCart.Count == 0)
            {
                TBlockItemCounter.Text = "Ваша корзина пуста";
                TBlockAllCost.Visibility = Visibility.Collapsed;
                TBlockDiscCost.Visibility = Visibility.Collapsed;
                TBlockFinalCost.Visibility = Visibility.Collapsed;
                BtnCreateOrd.Visibility = Visibility.Collapsed;
                BtnClearCart.Visibility = Visibility.Collapsed;
                MessageBox.Show("Вы удалили все товары.\nВаша корзина пуста", "Корзина",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                CountCost();
        }
        private void DeleteItem(Entities.Product changingProduct)
        {
            var itemToClear = App.Context.Product.Where(p => p.ProductArticleNumber == changingProduct.ProductArticleNumber)
                    .FirstOrDefault();
            itemToClear.ItemCounter = 0;
            productInCart.RemoveAll(p => p.ProductArticleNumber == changingProduct.ProductArticleNumber);
        }
        private void TBoxCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ItemTXB = sender as TextBox;
            var currentProduct = ItemTXB.DataContext as Entities.Product;
            if (!Int32.TryParse(ItemTXB.Text, out int itemCount) || itemCount < 0)
            {
                itemCount = 1;
                ItemTXB.Text = "1";
            }
            App.Context.Product.Where(p => p.ProductArticleNumber == currentProduct.ProductArticleNumber)
                .FirstOrDefault().ItemCounter = itemCount;
            if (ItemTXB.Text == "0" && MessageBox.Show($"Вы уверены, что хотите удалить товар: {currentProduct.ProductName}\nАртикль: " +
                $"{currentProduct.ProductArticleNumber}?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeleteItem(currentProduct);
                UpdateCart();
            }
            else if (ItemTXB.Text == "0")
            {
                ItemTXB.Text = "1";
            }
            CountCost();
        }
        private void CountCost()
        {
            TBlockAllCost.Visibility = Visibility.Visible;
            TBlockDiscCost.Visibility = Visibility.Visible;
            TBlockFinalCost.Visibility = Visibility.Visible;
            allCost = 0;
            discCost = 0;
            finalCost = 0;
            foreach (Entities.Product productItem in LViewOrder.Items)
            {
                allCost += productItem.ProductCost * productItem.ItemCounter;
                finalCost += (decimal)(productItem.CostWithDiscount * productItem.ItemCounter);
            }
            discCost = allCost - finalCost;
            TBlockAllCost.Text = "Общая сумма: " + allCost.ToString("N2");
            TBlockDiscCost.Text = "Сумма скидки: " + discCost.ToString("N2");
            TBlockFinalCost.Text = "Итого: " + finalCost.ToString("N2");
        }

        private void BtnCreateOrd_Click(object sender, RoutedEventArgs e)
        {
            List<Entities.Product> orderProducts = new List<Entities.Product>();
            foreach (Entities.Product orderItem in LViewOrder.Items)
                orderProducts.Add(orderItem);
            NavigationService.Navigate(new CreateOrderPage(orderProducts, allCost, discCost, finalCost));
        }

        private void BtnClearCart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите очистить корзину?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                foreach (Entities.Product itemInCart in LViewOrder.Items)
                {
                    DeleteItem(itemInCart);
                }
                UpdateCart();
            }
        }
    }
}
