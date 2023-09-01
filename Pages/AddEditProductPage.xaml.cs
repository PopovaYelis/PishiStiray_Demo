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
    /// Логика взаимодействия для AddEditProductPage.xaml
    /// </summary>
    public partial class AddEditProductPage : Page
    {
        private Entities.Product _currentProduct = null;
        private byte[] _mainImageData = null;
        public AddEditProductPage()
        {
            InitializeComponent();
        }
        public AddEditProductPage(Entities.Product product)
        {
            InitializeComponent();
            Title = "Редактировать товар";
            _currentProduct = product;
            TBoxArticle.Text = _currentProduct.ProductArticleNumber;
            TBoxTitle.Text = _currentProduct.ProductName;
            TBoxDescription.Text = _currentProduct.ProductDescription;

            //CBCategory.SelectedItem = _currentProduct.ProductCategory1.CategoryName;
            //CBManufactor.SelectedItem = _currentProduct.Manufacturer.ManufacturerName;
            //CBProvider.SelectedItem = _currentProduct.Provider.ProviderName;
            //CBProdUni.SelectedItem = _currentProduct.ProductUnit1.UnitName;

            CBCategory.SelectedIndex = _currentProduct.ProductCategory - 1;
            CBManufactor.SelectedIndex = _currentProduct.ProductManufacturer - 1;
            CBProvider.SelectedIndex = _currentProduct.ProductProvider - 1;
            CBProdUni.SelectedIndex = _currentProduct.ProductUnit - 1;

            TBoxCost.Text = _currentProduct.ProductCost.ToString("N2");
            TBoxDiscountAm.Text = _currentProduct.ProductDiscountAmount.ToString("N2");
            TBoxDiscountMax.Text = _currentProduct.ProductDiscountMax.ToString("N2");
            TBoxInStock.Text = _currentProduct.ProductQuantityInStock.ToString();
            if (_currentProduct.ProductPhoto != null)
            {
                ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(_currentProduct.ProductPhoto) as ImageSource;
            }
        }

        private void BtnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image | *.png; *.jpg; *.jpeg";
            if (ofd.ShowDialog() == true)
            {
                _mainImageData = File.ReadAllBytes(ofd.FileName);
                ImageService.Source = new ImageSourceConverter()
                    .ConvertFrom(_mainImageData) as ImageSource;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = CheckErrors();

            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (_currentProduct == null)
                {
                    var product = new Entities.Product
                    {
                        ProductArticleNumber = TBoxArticle.Text,
                        ProductName = TBoxTitle.Text,
                        ProductDescription = TBoxDescription.Text,
                        ProductCategory = App.Context.ProductCategory.Where(p => p.CategoryName == CBCategory.SelectedItem.ToString())
                            .Select(p => p.CategoryID).FirstOrDefault(),
                        ProductPhoto = _mainImageData,
                        ProductManufacturer = App.Context.Manufacturer.Where(p => p.ManufacturerName == CBManufactor.SelectedItem.ToString())
                            .Select(p => p.ManufactorerID).FirstOrDefault(),
                        ProductProvider = App.Context.Provider.Where(p => p.ProviderName == CBProvider.SelectedItem.ToString())
                            .Select(p => p.ProviderID).FirstOrDefault(),
                        ProductCost = Convert.ToDecimal(TBoxCost.Text),
                        ProductDiscountAmount = Convert.ToDecimal(TBoxDiscountAm.Text),
                        ProductDiscountMax = Convert.ToDecimal(TBoxDiscountMax.Text),
                        ProductUnit = App.Context.ProductUnit.Where(p => p.UnitName == CBProdUni.SelectedItem.ToString())
                            .Select(p => p.UnitID).FirstOrDefault(),
                        ProductQuantityInStock = Convert.ToInt32(TBoxInStock.Text)
                    };

                    App.Context.Product.Add(product);
                    App.Context.SaveChanges();
                }
                else
                {
                    _currentProduct.ProductArticleNumber = TBoxArticle.Text;
                    _currentProduct.ProductName = TBoxTitle.Text;
                    _currentProduct.ProductDescription = TBoxDescription.Text;
                    _currentProduct.ProductCategory = App.Context.ProductCategory.Where(p => p.CategoryName == CBCategory.SelectedItem.ToString())
                        .Select(p => p.CategoryID).FirstOrDefault();
                    _currentProduct.ProductManufacturer = App.Context.Manufacturer.Where(p => p.ManufacturerName == CBManufactor.SelectedItem.ToString())
                        .Select(p => p.ManufactorerID).FirstOrDefault();
                    _currentProduct.ProductProvider = App.Context.Provider.Where(p => p.ProviderName == CBProvider.SelectedItem.ToString())
                        .Select(p => p.ProviderID).FirstOrDefault();
                    _currentProduct.ProductCost = Convert.ToDecimal(TBoxCost.Text);
                    _currentProduct.ProductDiscountAmount = Convert.ToDecimal(TBoxDiscountAm.Text);
                    _currentProduct.ProductDiscountMax = Convert.ToDecimal(TBoxDiscountMax.Text);
                    _currentProduct.ProductUnit = App.Context.ProductUnit.Where(p => p.UnitName == CBProdUni.SelectedItem.ToString())
                        .Select(p => p.UnitID).FirstOrDefault();
                    _currentProduct.ProductQuantityInStock = Convert.ToInt32(TBoxInStock.Text);
                    if (_mainImageData != null)
                    {
                        _currentProduct.ProductPhoto = _mainImageData;
                    }
                    App.Context.SaveChanges();
                }
                NavigationService.GoBack();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var productCat = App.Context.ProductCategory.OrderBy(p => p.CategoryID).Select(p => p.CategoryName).ToArray();
            var productMan = App.Context.Manufacturer.OrderBy(p => p.ManufactorerID).Select(p => p.ManufacturerName).ToArray();
            var productProv = App.Context.Provider.OrderBy(p => p.ProviderID).Select(p => p.ProviderName).ToArray();
            var productUnit = App.Context.ProductUnit.OrderBy(p => p.UnitID).Select(p => p.UnitName).ToArray();
            for (int i = 0; i < productCat.Length; i++)
                CBCategory.Items.Add(productCat[i]);
            for (int i = 0; i < productMan.Length; i++)
                CBManufactor.Items.Add(productMan[i]);
            for (int i = 0; i < productProv.Length; i++)
                CBProvider.Items.Add(productProv[i]);
            for (int i = 0; i < productUnit.Length; i++)
                CBProdUni.Items.Add(productUnit[i]);
        }
        private string CheckErrors()
        {
            decimal cost = 0;
            decimal discAm = 0;
            decimal discMax = 0;
            int inStock = 0;
            var errorBuilder = new StringBuilder();
            var productFromDB = App.Context.Product.ToList()
                .FirstOrDefault(p => p.ProductArticleNumber.ToLower() == TBoxTitle.Text.ToLower());

            if (string.IsNullOrWhiteSpace(TBoxArticle.Text))
                errorBuilder.AppendLine("Артикль товара обязателен для заполнения;");
            else if (productFromDB != null && productFromDB != _currentProduct)
                errorBuilder.AppendLine("Такой артикль уже есть в базе данных;");
            if (string.IsNullOrWhiteSpace(TBoxTitle.Text))
                errorBuilder.AppendLine("Название товара обязательно для заполнения;");
            if (string.IsNullOrWhiteSpace(TBoxDescription.Text))
                errorBuilder.AppendLine("Описание товара обязательно для заполнения;");
            if (CBCategory.SelectedItem == null)
                errorBuilder.AppendLine("Выберите категорию товара;");
            if (CBManufactor.SelectedItem == null)
                errorBuilder.AppendLine("Выберите производителя;");
            if (CBProdUni.SelectedItem == null)
                errorBuilder.AppendLine("Выберите поставщика;");
            if (string.IsNullOrWhiteSpace(TBoxCost.Text))
                errorBuilder.AppendLine("Цена товара обязательна для заполнения;");
            else if (decimal.TryParse(TBoxCost.Text, out cost) == false || cost <= 0)
                errorBuilder.AppendLine("Цена товара должна быть положительным числом;");
            if (string.IsNullOrWhiteSpace(TBoxDiscountAm.Text))
                errorBuilder.AppendLine("Действительная скидка товара обязательна для заполнения;");
            else if (decimal.TryParse(TBoxDiscountAm.Text, out discAm) == false || discAm < 0 || discAm > 100)
                errorBuilder.AppendLine("Действительная скидка товара должна быть в диапазоне от 0 до 100;");
            if (string.IsNullOrWhiteSpace(TBoxDiscountMax.Text))
                errorBuilder.AppendLine("Максимальная скидка товара обязательна для заполнения;");
            else if (decimal.TryParse(TBoxDiscountMax.Text, out discMax) == false || discMax < 0 || discMax > 100)
                errorBuilder.AppendLine("Максимальная скидка товара должна быть в диапазоне от 0 до 100;");
            if (CBProdUni.SelectedItem == null)
                errorBuilder.AppendLine("Выберите единицы измерения;");
            if (string.IsNullOrWhiteSpace(TBoxInStock.Text))
                errorBuilder.AppendLine("Количество товара на складе обязательно для заполнения;");
            else if (Int32.TryParse(TBoxInStock.Text, out inStock) == false || inStock < 0)
                errorBuilder.AppendLine("Количество товара на складе должно быть неотрицательным числом;");

            if (errorBuilder.Length > 0)
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");

            return errorBuilder.ToString();
        }
    }
}
