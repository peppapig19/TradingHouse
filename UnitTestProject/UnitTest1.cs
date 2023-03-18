using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows;
using TradingHouse;
using TradingHouse.Editing;

namespace UnitTestProject
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Authorization_TestMethod()
		{
			string login = "ken";
			string password = "zav";
			Account account;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				account = db.Account.Where(a => a.login == login && a.password == password).FirstOrDefault();
			}

			Assert.IsNotNull(account);
		}

		[TestMethod]
		public void CreateDepartment_TestMethod()
		{
			EditDepartment window = new EditDepartment();
			window.Window_Loaded(this, new RoutedEventArgs());
			window.Department.name = "Test";

			window.save_button_Click(this, new RoutedEventArgs());

			Assert.AreNotEqual(0, window.Department.id);
		}

		[TestMethod]
		public void CreateProduct_TestMethod()
		{
			EditProduct window = new EditProduct();
			window.Window_Loaded(this, new RoutedEventArgs());
			window.Product.id_dept = 3;
			window.Product.name = "Test";
			window.Product.unit = "Test";
			window.Product.price = 10;

			window.save_button_Click(this, new RoutedEventArgs());

			Assert.AreNotEqual(0, window.Product.id);
		}

		[TestMethod]
		public void CreateSelling_TestMethod()
		{
			EditSelling window = new EditSelling();
			window.Window_Loaded(this, new RoutedEventArgs());
			window.Selling.id_product = 3;
			window.Selling.date = DateTime.Now;
			window.Selling.amount = 10;

			window.save_button_Click(this, new RoutedEventArgs());

			Assert.AreNotEqual(0, window.Selling.id);
		}

		[TestMethod]
		public void CreateWorker_TestMethod()
		{
			EditWorker window = new EditWorker();
			window.Window_Loaded(this, new RoutedEventArgs());
			window.Worker.id_dept = 3;
			window.Worker.type = "продавец";
			window.Worker.name = "Test";

			window.save_button_Click(this, new RoutedEventArgs());

			Assert.AreNotEqual(0, window.Worker.id);
		}
	}
}