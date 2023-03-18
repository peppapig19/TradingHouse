using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingHouse.Editing;

namespace TradingHouse.Pages
{
	public partial class Departments : Page
	{
		public Departments()
		{
			InitializeComponent();
		}

		int page;
		List<DepartmentView> searchResults;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataGridView();
		}

		void LoadDataGridView()
		{
			List<DepartmentView> list = searchResults;

			if (list == null)
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					list = db.DepartmentView.ToList();
				}
			}
			else page = 0;

			List<DepartmentView> range;
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
				searchResults = new List<DepartmentView>();

				if (manager_checkBox.IsChecked == true)
				{
					List<DepartmentView> matches = db.DepartmentView.Where(d => d.name_manager.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (name_checkBox.IsChecked == true)
				{
					List<DepartmentView> matches = db.DepartmentView.Where(d => d.name.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				LoadDataGridView();
				if (searchResults.Count == 0) MessageBox.Show("Записи не найдены.");
			}
		}

		private void create_button_Click(object sender, RoutedEventArgs e)
		{
			EditDepartment window = new EditDepartment();
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void update_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			DepartmentView departmentView = (DepartmentView)dataGridView.SelectedItem;
			Department department;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				department = db.Department.Find(departmentView.id);
			}

			EditDepartment window = new EditDepartment() { Department = department };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void delete_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			DepartmentView departmentView = (DepartmentView)dataGridView.SelectedItem;

			MessageBoxResult res = MessageBox.Show($"Удалить запись с id {departmentView.id}?", "Удаление", MessageBoxButton.OKCancel);
			if (res == MessageBoxResult.Cancel) return;

			try
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					Department department = db.Department.Find(departmentView.id);
					db.Department.Remove(department);
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