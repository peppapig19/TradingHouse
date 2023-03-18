using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TradingHouse.Editing
{
	public partial class EditAccount : Window
	{
		public EditAccount()
		{
			InitializeComponent();
		}

		public Account Account { get; set; }
		public List<Worker> Workers { get; set; } = new List<Worker>();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Account == null)
			{
				Account = new Account();

				id_label_1.Visibility = id_label_2.Visibility = Visibility.Collapsed;
			}
			else id_label_2.Content = Account.id;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				Workers = db.Worker.ToList();
			}

			DataContext = null;
			DataContext = this;
		}

		private void save_button_Click(object sender, RoutedEventArgs e)
		{
			if (Account.id_worker == 0 || string.IsNullOrEmpty(Account.login) || string.IsNullOrEmpty(Account.password))
			{
				MessageBox.Show("Заполните обязательные поля.");
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Account.id == 0) db.Account.Add(Account);
				else db.Entry(Account).State = EntityState.Modified;

				db.SaveChanges();
			}

			DialogResult = true;
			Close();
		}
	}
}