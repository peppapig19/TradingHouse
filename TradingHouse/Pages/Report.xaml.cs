using System;
using System.Data;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;

namespace TradingHouse.Pages
{
	public partial class Report : Page
	{
		public Report()
		{
			InitializeComponent();
		}

		private void report_button_Click(object sender, RoutedEventArgs e)
		{
			if (from_datePicker.SelectedDate == null || to_datePicker.SelectedDate == null) return;

			string minDate = ((DateTime)from_datePicker.SelectedDate).ToString("yyyy-MM-dd");
			string maxDate = ((DateTime)to_datePicker.SelectedDate).ToString("yyyy-MM-dd");

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				DbCommand cmd = db.Database.Connection.CreateCommand();
				cmd.CommandText = $"SELECT D.id, " +
					$"D.name, " +
					$"D.day_sales_volume, " +
					$"(D.day_sales_volume * DATEDIFF(DAY, '{minDate}', '{maxDate}')) AS planned_volume, " +
					$"SUM(X.cost) AS actual_volume, " +
					$"(SUM(X.cost) - (D.day_sales_volume * DATEDIFF(DAY, '{minDate}', '{maxDate}'))) AS profit " +
					$"FROM (" +
					$"SELECT S.*, D.id AS id_dept FROM Selling S " +
					$"INNER JOIN Product P ON S.id_product = P.id " +
					$"INNER JOIN Department D ON P.id_dept = D.id " +
					$"WHERE S.date BETWEEN '{minDate}' AND '{maxDate}'" +
					$") X " +
					$"RIGHT JOIN Department D ON X.id_dept = D.id " +
					$"GROUP BY D.id, D.name, D.day_sales_volume";
				cmd.Connection.Open();

				DataTable dt = new DataTable();
				dt.Load(cmd.ExecuteReader());
				dataGridView.ItemsSource = dt.DefaultView;
			}
        }
    }
}