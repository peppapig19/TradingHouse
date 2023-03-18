using System.Windows;
using TradingHouse.Pages;

namespace TradingHouse.Roles
{
	public partial class Manager : Window
	{
		public Manager()
		{
			InitializeComponent();
		}

		public int Id_dept { get; set; }

		private void selling_button_Click(object sender, RoutedEventArgs e)
		{
			Sellings page = new Sellings() { Id_dept = Id_dept };
			frame.Content = page;
			Title = $"Заведующий отделом | {page.Title}";
		}

		private void product_button_Click(object sender, RoutedEventArgs e)
		{
			Products page = new Products() { Id_dept = Id_dept };
			frame.Content = page;
			Title = $"Заведующий отделом | {page.Title}";
		}

		private void worker_button_Click(object sender, RoutedEventArgs e)
		{
			Workers page = new Workers() { Id_dept = Id_dept };
			frame.Content = page;
			Title = $"Заведующий отделом | {page.Title}";
		}

		private void logout_button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			Close();
		}
	}
}