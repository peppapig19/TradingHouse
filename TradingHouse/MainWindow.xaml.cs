using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TradingHouse
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		readonly Random ran = new Random();
		string captcha;

		private BitmapImage GenerateCaptcha(int width = 150, int height = 75)
		{
			Bitmap bitmap = new Bitmap(width, height);
			BitmapImage captchaImage = new BitmapImage();
			Brush[] colors = { Brushes.Black, Brushes.Red, Brushes.RoyalBlue, Brushes.Green };
			string chars = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
			Font font = new Font("Arial", 20, System.Drawing.FontStyle.Strikeout);

			Graphics g = Graphics.FromImage(bitmap);
			int charWidth = (int)g.MeasureString("X", font).Width;
			int Xpos = ran.Next(0, width - charWidth * 4);
			g.Clear(Color.Gray);
			captcha = string.Empty;

			for (int i = 0; i < 4; i++)
			{
				int Ypos = ran.Next(0, height - 30);

				captcha += chars[ran.Next(chars.Length)];
				g.DrawString(captcha[i].ToString(), font, colors[ran.Next(colors.Length)], new PointF(Xpos, Ypos));
				Xpos += charWidth;
			}

			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if (ran.Next() % 100 == 0) bitmap.SetPixel(i, j, Color.White);
				}
			}

			using (MemoryStream memory = new MemoryStream())
			{
				bitmap.Save(memory, ImageFormat.Bmp);

				captchaImage.BeginInit();
				captchaImage.StreamSource = memory;
				captchaImage.CacheOption = BitmapCacheOption.OnLoad;
				captchaImage.EndInit();
			}

			return captchaImage;
		}

		private void password_image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (password_textBox.Visibility == Visibility.Visible)
			{
				passwordBox.Password = password_textBox.Text;
				passwordBox.Visibility = Visibility.Visible;
				password_textBox.Visibility = Visibility.Hidden;
				password_image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/view.png"));
			}
			else
			{
				password_textBox.Text = passwordBox.Password;
				passwordBox.Visibility = Visibility.Hidden;
				password_textBox.Visibility = Visibility.Visible;
				password_image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/hide.png"));
			}
		}

		private void captcha_image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			captcha_image.Source = GenerateCaptcha();
		}

		private void login_button_Click(object sender, RoutedEventArgs e)
		{
			if (captcha != null && captcha_textBox.Text != captcha)
			{
				MessageBox.Show("Неправильно введена капча.");
				captcha_image.Source = GenerateCaptcha();
				login_button.IsEnabled = false;

				DispatcherTimer timer = new DispatcherTimer();
				timer.Tick += Timer_Tick;
				timer.Interval = TimeSpan.FromSeconds(10);
				timer.Start();

				return;
			}

			string login = login_textBox.Text;
			string password = password_textBox.Visibility == Visibility.Visible ? password_textBox.Text : passwordBox.Password;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				Account account = db.Account.Where(a => a.login == login && a.password == password).SingleOrDefault();

				if (account == null)
				{
					MessageBox.Show("Неправильный логин или пароль.");
					captcha_image.Source = GenerateCaptcha();
					captcha_image.Visibility = Visibility.Visible;
					captcha_textBox.Visibility = Visibility.Visible;

					return;
				}

				Worker worker = db.Worker.Find(account.id_worker);

				if (worker == null)
				{
					MessageBox.Show("Аккаунт не присвоен работнику.");
					return;
				}

				Welcome window = new Welcome() { Worker = worker };
				window.ShowDialog();
				Close();
			}
		}

		private void Timer_Tick(object sender, EventArgs e)
		{
			DispatcherTimer timer = (DispatcherTimer)sender;
			timer.Stop();

			login_button.IsEnabled = true;
		}
	}
}