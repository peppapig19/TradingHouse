using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingHouse.Editing;

namespace TradingHouse.Pages
{
	public partial class Accounts : Page
	{
		public Accounts()
		{
			InitializeComponent();
		}

		int page;
		List<AccountView> searchResults;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataGridView();
		}

		void LoadDataGridView()
		{
			List<AccountView> list = searchResults;

			if (list == null)
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					list = db.AccountView.ToList();
				}
			}
			else page = 0;

			List<AccountView> range;
			int fullPages = list.Count / 15;

			if (list.Count != 0)
			{
				int mod = fullPages > 0 ? list.Count % (fullPages * 15) : list.Count;
				range = page == fullPages ? list.GetRange(page * 15, mod) : list.GetRange(page * 15, 15);
				dataGridView.ItemsSource = range;
			}
			else dataGridView.ItemsSource = range = list;

			count_label.Content = $"{range.Count} из {list.Count}";

			previous_button.IsEnabled = page != 0;
			next_button.IsEnabled = page != fullPages;
		}

		private void previous_button_Click(object sender, RoutedEventArgs e)
		{
			page--;
			LoadDataGridView();

			if (!next_button.IsEnabled) next_button.IsEnabled = true;
		}

		private void next_button_Click(object sender, RoutedEventArgs e)
		{
			page++;
			LoadDataGridView();

			if (!previous_button.IsEnabled) previous_button.IsEnabled = true;
		}

		private void CheckBox_Click(object sender, RoutedEventArgs e)
		{
			Search();
		}

		private void query_textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			Search();
		}

		void Search()
		{
			string query = query_textBox.Text.ToLower();

			if (query == string.Empty)
			{
				searchResults = null;
				LoadDataGridView();
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				searchResults = new List<AccountView>();

				if (name_checkBox.IsChecked == true)
				{
					List<AccountView> matches = db.AccountView.Where(a => a.name_worker.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (login_checkBox.IsChecked == true)
				{
					List<AccountView> matches = db.AccountView.Where(a => a.login.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				LoadDataGridView();
				if (searchResults.Count == 0) MessageBox.Show("Записи не найдены.");
			}
		}

		private void create_button_Click(object sender, RoutedEventArgs e)
		{
			EditAccount window = new EditAccount();
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void update_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			AccountView accountView = (AccountView)dataGridView.SelectedItem;
			Account account;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				account = db.Account.Find(accountView.id);
			}

			EditAccount window = new EditAccount() { Account = account };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void delete_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			AccountView accountView = (AccountView)dataGridView.SelectedItem;

			MessageBoxResult res = MessageBox.Show($"Удалить запись с id {accountView.id}?", "Удаление", MessageBoxButton.OKCancel);
			if (res == MessageBoxResult.Cancel) return;

			try
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					Account account = db.Account.Find(accountView.id);
					db.Account.Remove(account);
					db.SaveChanges();
				}

				LoadDataGridView();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}