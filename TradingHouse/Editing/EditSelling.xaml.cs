using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace TradingHouse.Editing
{
	public partial class EditSelling : Window
	{
		public EditSelling()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		public Selling Selling { get; set; }
		public List<Product> Products { get; set; } = new List<Product>();

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Selling == null)
			{
				Selling = new Selling();

				id_label_1.Visibility = id_label_2.Visibility = Visibility.Collapsed;
				Selling.date = DateTime.Now;
			}
			else id_label_2.Content = Selling.id;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				Products = db.Product.ToList();
				if (Id_dept != null) Products = Products.Where(p => p.id_dept == Id_dept).ToList();
			}

			DataContext = null;
			DataContext = this;
		}

		public void save_button_Click(object sender, RoutedEventArgs e)
		{
			if (Selling.id_product == 0 || Selling.amount == 0)
			{
				MessageBox.Show("Заполните обязательные поля.");
				return;
			}

			if (Selling.amount < 0)
			{
				MessageBox.Show("Количество не может быть отрицательным.");
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Selling.id == 0) db.Selling.Add(Selling);
				else db.Entry(Selling).State = EntityState.Modified;

				db.SaveChanges();
			}

			string testAssemblyName = "Microsoft.VisualStudio.TestPlatform.TestFramework";
			bool isInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(testAssemblyName));

			if (!isInUnitTest) DialogResult = true;
			Close();
		}
	}
}