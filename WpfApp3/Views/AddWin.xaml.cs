using System;
using System.Linq;
using System.Windows;
using WpfApp3.DB;
using System.Data.Entity;

namespace WpfApp3.Views
{
    public partial class AddWin : Window
    {
        public int IdUser { get; set; }
        private TradeEntities tradeEntities = new TradeEntities();

        public AddWin(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int idM = 0, idP = 0, idC = 0;

            using (var transaction = tradeEntities.Database.BeginTransaction())
            {
                try
                {
                    // Проверка и добавление производителя
                    var manufacturer = tradeEntities.Manufacturers.SingleOrDefault(p => p.ManufacturerName == Manufact.Text);
                    if (manufacturer == null)
                    {
                        var newManufacturer = new Manufacturers { ManufacturerName = Manufact.Text };
                        tradeEntities.Manufacturers.Add(newManufacturer);
                        tradeEntities.SaveChanges();
                        idM = newManufacturer.ManufacturerID;
                    }
                    else
                    {
                        idM = manufacturer.ManufacturerID;
                    }

                    // Проверка и добавление поставщика
                    var provider = tradeEntities.Providers.SingleOrDefault(p => p.ProviderName == Provider.Text);
                    if (provider == null)
                    {
                        var newProvider = new Providers { ProviderName = Provider.Text };
                        tradeEntities.Providers.Add(newProvider);
                        tradeEntities.SaveChanges();
                        idP = newProvider.ProviderID;
                    }
                    else
                    {
                        idP = provider.ProviderID;
                    }

                    // Проверка и добавление категории
                    var category = tradeEntities.Categories.SingleOrDefault(p => p.CategoryName == Category.Text);
                    if (category == null)
                    {
                        var newCategory = new Categories { CategoryName = Category.Text };
                        tradeEntities.Categories.Add(newCategory);
                        tradeEntities.SaveChanges();
                        idC = newCategory.CategoryID;
                    }
                    else
                    {
                        idC = category.CategoryID;
                    }

                    var newProduct = new Products
                    {
                        ProductArticleNumber = ArticleProd.Text,
                        ProductName = NameProd.Text,
                        ProductDescription = DescriptionProd.Text,
                        ProductUnit = "шт.",
                        ProductCost = (int?)double.Parse(PriceProd.Text),
                        ProductMaxDiscount = (int?)double.Parse(MaxDisc.Text),
                        ProductQuantityInStock = int.Parse(CountProd.Text),
                        ProductDiscountAmount = (int?)double.Parse(Disc.Text),
                        ProductPhoto = "/Images/picture.png",
                        ManufacturerID = idM,
                        ProviderID = idP,
                        CategoryID = idC
                    };

                    tradeEntities.Products.Add(newProduct);
                    tradeEntities.SaveChanges();
                    transaction.Commit();
                    MessageBox.Show("Товар добавлен");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Ошибка: {GetInnermostExceptionMessage(ex)}");
                }
            }
        }

        private string GetInnermostExceptionMessage(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
