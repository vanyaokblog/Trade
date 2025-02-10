using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.DB;

namespace WpfApp3.Views
{
    public partial class AdminWin : Window
    {
        public int IdUser { get; set; }
        TradeEntities tradeEntities = new TradeEntities();

        public AdminWin(int idUser)
        {
            InitializeComponent();
            IdUser = idUser;
            ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
            ProductDataGrid.IsReadOnly = true;
            UserFio.Text = tradeEntities.Users.Where(p => p.UserID == idUser).Select(p => p.UserFullName).Single();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Products selectedProduct)
            {
                EditWin editWin = new EditWin(selectedProduct);
                editWin.ShowDialog();
                ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования.");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Products selectedProduct)
            {
                tradeEntities.Products.Remove(selectedProduct);
                tradeEntities.SaveChanges();
                ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddWin addProductWin = new AddWin(IdUser);
            addProductWin.ShowDialog();
            ProductDataGrid.ItemsSource = tradeEntities.Products.ToList();
        }
    }
}
