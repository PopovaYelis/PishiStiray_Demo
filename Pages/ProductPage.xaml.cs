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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        List<Entities.Product> ProductCart = new List<Entities.Product>();
        public ProductPage()
        {
            InitializeComponent();
            ComboDiscount.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateProduct();
            foreach (Entities.Product prodItem in LViewProducts.Items)
                prodItem.ItemCounter = 0;
            BtnCreateOrd.Visibility = Visibility.Collapsed;
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var currentProduct = button.DataContext as Entities.Product;
            NavigationService.Navigate(new AddEditProductPage(currentProduct));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentProduct = (sender as Button).DataContext as Entities.Product;
            if (App.Context.OrderProduct.FirstOrDefault(p => p.ProductArticleNumber == currentProduct.ProductArticleNumber) == null)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить товар: {currentProduct.ProductName}\nАртикль: " +
                    $"{currentProduct.ProductArticleNumber}?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    App.Context.Product.Remove(currentProduct);
                    App.Context.SaveChanges();
                    UpdateProduct();
                }
            }
            else
            {
                MessageBox.Show("Нельзя удалить товар\nПричина: на данный товар существует заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProduct()
        {
            var products = App.Context.Product.ToList();
            if (ComboSortBy.SelectedIndex == 0)
            {
                products = products.OrderBy(p => p.CostWithDiscount).ToList();
            }
            else
            {
                products = products.OrderByDescending(p => p.CostWithDiscount).ToList();
            }
            if (ComboDiscount.SelectedIndex == 1)
                products = products.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (ComboDiscount.SelectedIndex == 2)
                products = products.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            if (ComboDiscount.SelectedIndex == 3)
                products = products.Where(p => p.ProductDiscountAmount >= 15).ToList();
            products = products.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewProducts.ItemsSource = null;
            LViewProducts.ItemsSource = products;
            int countFind = LViewProducts.Items.Count;
            TBlockItemCounter.Text = countFind.ToString() + " из " + App.Context.Product.Count().ToString();
            if (countFind < 1)
                TBlockItemCounter.Text += "\nПо вашему запросу ничего не найдено. Измените фильтры.";
        }

        private void AddToOrd_Click(object sender, RoutedEventArgs e)
        {
            ProductCart.Add(LViewProducts.SelectedItem as Entities.Product);
            ProductCart[ProductCart.Count - 1].ItemCounter++;
            BtnCreateOrd.Visibility = Visibility.Visible;
        }


        private void BtnCreateOrd_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow ow = new OrderWindow(ProductCart);
            ow.ShowDialog();
            if (ProductCart.Count == 0)
                BtnCreateOrd.Visibility = Visibility.Collapsed;
        }

    }
}
