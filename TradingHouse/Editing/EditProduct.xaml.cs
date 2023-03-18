using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TradingHouse.Editing
{
	public partial class EditProduct : Window
	{
		public EditProduct()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		public Product Product { get; set; }
		public List<Department> Departments { get; set; } = new List<Department>();

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Product == null)
			{
				Product = new Product();

				id_label_1.Visibility = id_label_2.Visibility = Visibility.Collapsed;
			}
			else id_label_2.Content = Product.id;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Id_dept == null) Departments = db.Department.ToList();
				else Departments = new List<Department>() { db.Department.Find(Id_dept) };
			}

			DataContext = null;
			DataContext = this;
		}

		public void save_button_Click(object sender, RoutedEventArgs e)
		{
			if (Product.id_dept == 0 || string.IsNullOrEmpty(Product.name) || string.IsNullOrEmpty(Product.unit))
			{
				MessageBox.Show("Заполните обязательные поля.");
				return;
			}

			if (Product.price < 0)
			{
				MessageBox.Show("Поля типа денежный не могут быть отрицательными.");
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Product.id == 0) db.Product.Add(Product);
				else db.Entry(Product).State = EntityState.Modified;

				db.SaveChanges();
			}

			string testAssemblyName = "Microsoft.VisualStudio.TestPlatform.TestFramework";
			bool isInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(testAssemblyName));

			if (!isInUnitTest) DialogResult = true;
			Close();
		}
	}
}