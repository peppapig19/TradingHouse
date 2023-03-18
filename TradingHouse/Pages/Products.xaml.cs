using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingHouse.Editing;

namespace TradingHouse.Pages
{
	public partial class Products : Page
	{
		public Products()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		int page;
		List<ProductView> searchResults;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataGridView();
		}

		void LoadDataGridView()
		{
			List<ProductView> list = searchResults;

			if (list == null)
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					list = db.ProductView.ToList();
					if (Id_dept != null) list = list.Where(p => p.id_dept == Id_dept).ToList();
				}
			}
			else page = 0;

			List<ProductView> range;
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
				searchResults = new List<ProductView>();

				if (dept_checkBox.IsChecked == true)
				{
					List<ProductView> matches = db.ProductView.Where(p => p.name_dept.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (name_checkBox.IsChecked == true)
				{
					List<ProductView> matches = db.ProductView.Where(p => p.name.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (unit_checkBox.IsChecked == true)
				{
					List<ProductView> matches = db.ProductView.Where(p => p.unit.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (description_checkBox.IsChecked == true)
				{
					List<ProductView> matches = db.ProductView.Where(p => p.description.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (Id_dept != null) searchResults = searchResults.Where(s => s.id_dept == Id_dept).ToList();

				LoadDataGridView();
				if (searchResults.Count == 0) MessageBox.Show("Записи не найдены.");
			}
		}

		private void create_button_Click(object sender, RoutedEventArgs e)
		{
			EditProduct window = new EditProduct() { Id_dept = Id_dept };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void update_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			ProductView productView = (ProductView)dataGridView.SelectedItem;
			Product product;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				product = db.Product.Find(productView.id);
			}

			EditProduct window = new EditProduct() { Id_dept = Id_dept, Product = product };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void delete_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			ProductView productView = (ProductView)dataGridView.SelectedItem;

			MessageBoxResult res = MessageBox.Show($"Удалить запись с id {productView.id}?", "Удаление", MessageBoxButton.OKCancel);
			if (res == MessageBoxResult.Cancel) return;

			try
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					Product product = db.Product.Find(productView.id);
					db.Product.Remove(product);
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