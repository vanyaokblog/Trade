using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.DB;

namespace WpfApp3.Views
{
    public partial class ProductWin : Window
    {
        TradeEntities tradeEntities = new TradeEntities();
        public int RoleID { get; set; }
        public int IdUser { get; set; }
        public string UserFullName { get; set; }


        public ProductWin(int roleID, int idUser, string userFullName)
        {
            InitializeComponent();
            RoleID = roleID;
            IdUser = idUser; 
            UserFullName = userFullName;
            UserFio.Text = userFullName;

            ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
            ProductDataGrid.IsReadOnly = true;

            if (RoleID == 0) // Гость
            {
                basketButton.Visibility = Visibility.Collapsed;
                ProductDataGrid.ContextMenu = null; // Отключаем контекстное меню для гостей
            }
            else if (RoleID == 2) // Менеджер
            {
                basketButton.Visibility = Visibility.Collapsed;
                ProductDataGrid.ContextMenu = null; // Отключаем контекстное меню для менеджеров
            }
            else if (RoleID == 3) // Покупатель
            {
                basketButton.Visibility = Visibility.Visible;
            }
        }

        private void ComboBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sortComboBox = sender as ComboBox;
            var selectedIndex = sortComboBox.SelectedIndex;

            switch (selectedIndex)
            {
                case 0: // По убыванию цены
                    ProductDataGrid.ItemsSource = tradeEntities.Products
                        .OrderByDescending(p => p.ProductCost).ToList();
                    break;
                case 1: // По возрастанию цены
                    ProductDataGrid.ItemsSource = tradeEntities.Products
                        .OrderBy(p => p.ProductCost).ToList();
                    break;
                case 2: // Убрать сортировку
                    ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
                    break;
            }
        }

        private void ComboBoxSortProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sortComboBox = sender as ComboBox;
            var selectedItem = sortComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string providerName = selectedItem.Content.ToString();

                if (providerName == "Все производители")
                {
                    ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
                }
                else
                {
                    ProductDataGrid.ItemsSource = tradeEntities.Products
                        .Where(p => p.Manufacturers.ManufacturerName == providerName).ToList();
                }
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

    }
}
