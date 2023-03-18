using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using TradingHouse.Roles;

namespace TradingHouse
{
	public partial class Welcome : Window
	{
		public Welcome()
		{
			InitializeComponent();
		}

		public Worker Worker { get; set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			name_label.Content += Worker.name;
			type_label.Content += Worker.type;

			if (!string.IsNullOrEmpty(Worker.image))
			{
				string path = Path.Combine(Environment.CurrentDirectory, Worker.image);

				if (File.Exists(path)) image.Source = new BitmapImage(new Uri(path));
				else MessageBox.Show("Изображение недоступно.");
			}
		}

		private void continue_button_Click(object sender, RoutedEventArgs e)
		{
			switch (Worker.type)
			{
				case "продавец":
					Seller seller = new Seller() { Id_dept = (int)Worker.id_dept };
					seller.Show();
					break;
				case "заведующий отделом":
					Manager manager = new Manager() { Id_dept = (int)Worker.id_dept };
					manager.Show();
					break;
				case "директор":
					Director director = new Director();
					director.Show();
					break;
				case "системный администратор":
					Admin admin = new Admin();
					admin.Show();
					break;
			}

			Close();
		}
	}
}