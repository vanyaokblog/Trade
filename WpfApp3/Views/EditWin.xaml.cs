using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using WpfApp3.DB;

namespace WpfApp3.Views
{
    public partial class EditWin : Window
    {
        Products Product;
        TradeEntities _tradeEntities = new TradeEntities();

        public EditWin(Products product)
        {
            InitializeComponent();
            Product = product;
            LoadProductData();
        }

        private void LoadProductData()
        {
            ArticleProd.Text = Product.ProductArticleNumber;
            NameProd.Text = Product.ProductName;
            PriceProd.Text = Product.ProductCost.ToString();
            CountProd.Text = Product.ProductQuantityInStock.ToString();
            Disc.Text = Product.ProductDiscountAmount.ToString();
            MaxDisc.Text = Product.ProductMaxDiscount.ToString();
            Provider.Text = Product.Providers.ProviderName;
            Manufact.Text = Product.Manufacturers.ManufacturerName;
            Category.Text = Product.Categories.CategoryName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product.ProductArticleNumber = ArticleProd.Text;
                Product.ProductName = NameProd.Text;
                Product.ProductCost = (int?)double.Parse(PriceProd.Text);
                Product.ProductQuantityInStock = int.Parse(CountProd.Text);
                Product.ProductDiscountAmount = (int?)double.Parse(Disc.Text);
                Product.ProductMaxDiscount = (int?)double.Parse(MaxDisc.Text);

                var provider = _tradeEntities.Providers.SingleOrDefault(p => p.ProviderName == Provider.Text);
                if (provider != null) Product.ProviderID = provider.ProviderID;

                var manufact = _tradeEntities.Manufacturers.SingleOrDefault(m => m.ManufacturerName == Manufact.Text);
                if (manufact != null) Product.ManufacturerID = manufact.ManufacturerID;

                var category = _tradeEntities.Categories.SingleOrDefault(c => c.CategoryName == Category.Text);
                if (category != null) Product.CategoryID = category.CategoryID;

                _tradeEntities.SaveChanges();
                MessageBox.Show("Товар обновлён");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при обновлении товара");
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
