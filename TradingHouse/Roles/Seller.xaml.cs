using System.Windows;
using TradingHouse.Pages;

namespace TradingHouse.Roles
{
	public partial class Seller : Window
	{
		public Seller()
		{
			InitializeComponent();
		}

		public int Id_dept { get; set; }

		private void selling_button_Click(object sender, RoutedEventArgs e)
		{
			Sellings page = new Sellings() { Id_dept = Id_dept };
			frame.Content = page;
			Title = $"Продавец | {page.Title}";
		}

		private void product_button_Click(object sender, RoutedEventArgs e)
		{
			Products page = new Products() { Id_dept = Id_dept };
			frame.Content = page;
			Title = $"Продавец | {page.Title}";
		}

		private void logout_button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow window = new MainWindow();
			window.Show();
			Close();
        }
    }
}