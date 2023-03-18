using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingHouse.Editing;

namespace TradingHouse.Pages
{
	public partial class Sellings : Page
	{
		public Sellings()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		int page;
		List<SellingView> searchResults;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataGridView();
		}

		void LoadDataGridView()
		{
			List<SellingView> list = searchResults;

			if (list == null)
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					list = db.SellingView.ToList();
					if (Id_dept != null) list = list.Where(s => s.id_dept == Id_dept).ToList();
				}
			}
			else page = 0;

			List<SellingView> range;
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

		private void query_textBox_TextChanged(object sender, TextChangedEventArgs e)
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
				searchResults = db.SellingView.Where(s => s.name_product.ToLower().Contains(query)).ToList();
				if (Id_dept != null) searchResults = searchResults.Where(s => s.id_dept == Id_dept).ToList();

				LoadDataGridView();
				if (searchResults.Count == 0) MessageBox.Show("Записи не найдены.");
			}
		}

		private void create_button_Click(object sender, RoutedEventArgs e)
		{
			EditSelling window = new EditSelling() { Id_dept = Id_dept };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void update_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			SellingView sellingView = (SellingView)dataGridView.SelectedItem;
			Selling selling;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				selling = db.Selling.Find(sellingView.id);
			}

			EditSelling window = new EditSelling() { Id_dept = Id_dept, Selling = selling };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void delete_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			SellingView sellingView = (SellingView)dataGridView.SelectedItem;

			MessageBoxResult res = MessageBox.Show($"Удалить запись с id {sellingView.id}?", "Удаление", MessageBoxButton.OKCancel);
			if (res == MessageBoxResult.Cancel) return;

			try
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					Selling selling = db.Selling.Find(sellingView.id);
					db.Selling.Remove(selling);
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