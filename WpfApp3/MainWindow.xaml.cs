using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WpfApp3.DB;
using WpfApp3.Views;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        TradeEntities tradeEntities = new TradeEntities();

        public MainWindow()
        {
            InitializeComponent();
        }
        private int failedAttempts = 0;
        private string captcha = "";

        private string GenerateCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rand = new Random();
            string captcha = new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
            return captcha;
        }

        private ImageSource GenerateCaptchaImage(string captchaText)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();

            FormattedText formattedText = new FormattedText(captchaText, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Comic Sans MS"), 32, Brushes.Black);

            drawingContext.DrawText(formattedText, new Point(10, 10));

            drawingContext.DrawLine(new Pen(Brushes.Gray, 2), new Point(0, 30), new Point(200, 30));
            drawingContext.DrawLine(new Pen(Brushes.Gray, 2), new Point(0, 35), new Point(200, 35));
            drawingContext.Close();

            RenderTargetBitmap bmp = new RenderTargetBitmap(200, 60, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            return bmp;
        }

        private void logIn_Click(object sender, RoutedEventArgs e)
        {
            if (failedAttempts >= 3)
            {
                if (captchaTextBox.Text != captcha)
                {
                    MessageBox.Show("Неверная CAPTCHA!");
                    return;
                }
            }

            try
            {
                var user = tradeEntities.Users
                    .Where(p => p.UserLogin == loginTextBox.Text && p.UserPassword == passBox.Password)
                    .Select(p => new { p.UserID, p.RoleID, p.UserFullName })
                    .Single();

                int roleID = user.RoleID ?? 0;

                if (roleID == 1)
                {
                    AdminWin adminWin = new AdminWin(user.UserID);
                    adminWin.Show();
                    Close();
                }
                else if (roleID == 2 || roleID == 3)
                {
                    ProductWin productWin = new ProductWin(roleID, user.UserID, user.UserFullName);
                    productWin.Show();
                    Close();
                }
                else
                {
                    ProductWin productWin = new ProductWin(0, user.UserID, "Гость");
                    productWin.Show();
                    Close();
                }
            }
            catch
            {
                MessageBox.Show("Неверный логин или пароль!");
                failedAttempts++;
                if (failedAttempts == 3)
                {
                    captcha = GenerateCaptcha();
                    captchaPanel.Visibility = Visibility.Visible;
                    captchaImage.Source = GenerateCaptchaImage(captcha);
                    this.Height += 120;
                }
            }
        }


        private void logInGuest_Click(object sender, RoutedEventArgs e)
        {
            ProductWin productWin = new ProductWin(0, 0, "Гость");
            productWin.Show();
            Close();
        }

    }
}
