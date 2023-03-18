using System.Windows;
using TradingHouse.Pages;

namespace TradingHouse.Roles
{
	public partial class Director : Window
	{
		public Director()
		{
			InitializeComponent();
		}

		private void selling_button_Click(object sender, RoutedEventArgs e)
		{
			Sellings page = new Sellings();
			frame.Content = page;
			Title = $"Директор | {page.Title}";
		}

		private void product_button_Click(object sender, RoutedEventArgs e)
		{
			Products page = new Products();
			frame.Content = page;
			Title = $"Директор | {page.Title}";
		}

		private void worker_button_Click(object sender, RoutedEventArgs e)
		{
			Workers page = new Workers();
			frame.Content = page;
			Title = $"Директор | {page.Title}";
		}

		private void dept_button_Click(object sender, RoutedEventArgs e)
		{
			Departments page = new Departments();
			frame.Content = page;
			Title = $"Директор | {page.Title}";
		}

		private void report_button_Click(object sender, RoutedEventArgs e)
		{
			Report page = new Report();
			frame.Content = page;
			Title = $"Директор | {page.Title}";
		}

		private void logout_button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			Close();
		}
	}
}