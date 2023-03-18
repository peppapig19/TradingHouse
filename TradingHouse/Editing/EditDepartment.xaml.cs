using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TradingHouse.Editing
{
	public partial class EditDepartment : Window
	{
		public EditDepartment()
		{
			InitializeComponent();
		}

		public Department Department { get; set; }

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Department == null)
			{
				Department = new Department();

				id_label_1.Visibility = id_label_2.Visibility = Visibility.Collapsed;
			}
			else id_label_2.Content = Department.id;

			DataContext = null;
			DataContext = this;
		}

		public void save_button_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(Department.name))
			{
				MessageBox.Show("Заполните обязательные поля.");
				return;
			}

			if (Department.day_sales_volume < 0)
			{
				MessageBox.Show("Поля типа денежный не могут быть отрицательными.");
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Department.id == 0) db.Department.Add(Department);
				else db.Entry(Department).State = EntityState.Modified;

				db.SaveChanges();
			}

			string testAssemblyName = "Microsoft.VisualStudio.TestPlatform.TestFramework";
			bool isInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(testAssemblyName));

			if (!isInUnitTest) DialogResult = true;
			Close();
		}
	}
}