using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradingHouse.Editing;

namespace TradingHouse.Pages
{
	public partial class Workers : Page
	{
		public Workers()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		int page;
		List<WorkerView> searchResults;

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataGridView();
		}

		void LoadDataGridView()
		{
			List<WorkerView> list = searchResults;

			if (list == null)
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					list = db.WorkerView.ToList();
					if (Id_dept != null) list = list.Where(w => w.id_dept == Id_dept).ToList();
				}
			}
			else page = 0;

			List<WorkerView> range;
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
				searchResults = new List<WorkerView>();

				if (dept_checkBox.IsChecked == true)
				{
					List<WorkerView> matches = db.WorkerView.Where(w => w.name_dept.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (type_checkBox.IsChecked == true)
				{
					List<WorkerView> matches = db.WorkerView.Where(w => w.type.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (name_checkBox.IsChecked == true)
				{
					List<WorkerView> matches = db.WorkerView.Where(w => w.name.ToLower().Contains(query)).ToList();
					searchResults = searchResults.Union(matches).ToList();
				}

				if (Id_dept != null) searchResults = searchResults.Where(s => s.id_dept == Id_dept).ToList();

				LoadDataGridView();
				if (searchResults.Count == 0) MessageBox.Show("Записи не найдены.");
			}
		}

		private void create_button_Click(object sender, RoutedEventArgs e)
		{
			EditWorker window = new EditWorker() { Id_dept = Id_dept };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void update_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			WorkerView workerView = (WorkerView)dataGridView.SelectedItem;
			Worker worker;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				worker = db.Worker.Find(workerView.id);
			}

			EditWorker window = new EditWorker() { Id_dept = Id_dept, Worker = worker };
			if (window.ShowDialog() == true) LoadDataGridView();
		}

		private void delete_button_Click(object sender, RoutedEventArgs e)
		{
			if (dataGridView.SelectedItem == null) return;

			WorkerView workerView = (WorkerView)dataGridView.SelectedItem;

			MessageBoxResult res = MessageBox.Show($"Удалить запись с id {workerView.id}?", "Удаление", MessageBoxButton.OKCancel);
			if (res == MessageBoxResult.Cancel) return;

			try
			{
				using (TradingHouseEntities db = new TradingHouseEntities())
				{
					Worker worker = db.Worker.Find(workerView.id);
					db.Worker.Remove(worker);
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