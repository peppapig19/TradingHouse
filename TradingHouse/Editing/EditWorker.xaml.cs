using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TradingHouse.Editing
{
	public partial class EditWorker : Window
	{
		public EditWorker()
		{
			InitializeComponent();
		}

		public int? Id_dept { get; set; }
		public Worker Worker { get; set; }
		public List<Department> Departments { get; set; }

		public void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (Worker == null)
			{
				Worker = new Worker();

				id_label_1.Visibility = id_label_2.Visibility = Visibility.Collapsed;
			}
			else id_label_2.Content = Worker.id;

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Id_dept == null) Departments = db.Department.ToList();
				else Departments = new List<Department>() { db.Department.Find(Id_dept) };
			}

			DataContext = null;
			DataContext = this;

			if (!string.IsNullOrEmpty(Worker.image))
			{
				string path = Path.Combine(Environment.CurrentDirectory, Worker.image);

				if (File.Exists(path))
				{
					BitmapImage bi = new BitmapImage();
					bi.BeginInit();
					bi.CacheOption = BitmapCacheOption.OnLoad;
					bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
					bi.UriSource = new Uri(path);
					bi.EndInit();
					image.Source = bi;
				}
				else MessageBox.Show("Изображение недоступно.");
			}
		}

		private void upload_button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "Изображения|*.jpg;*.jpeg;*.png";

			if (dialog.ShowDialog() == false) return;

			image.Source = null;

			string fileName = Path.GetFileName(dialog.FileName);
			string path = Path.Combine(Environment.CurrentDirectory, $"Profiles/{fileName}");
			string oldPath = !string.IsNullOrEmpty(Worker.image) ? Path.Combine(Environment.CurrentDirectory, Worker.image) : string.Empty;

			Directory.CreateDirectory(Path.GetDirectoryName(path));
			if (File.Exists(oldPath)) File.Delete(oldPath);
			File.Copy(dialog.FileName, path, true);

			Worker.image = $"Profiles/{fileName}";
			image.Source = new BitmapImage(new Uri(path));
		}

		public void save_button_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(Worker.type) || string.IsNullOrEmpty(Worker.name))
			{
				MessageBox.Show("Заполните обязательные поля.");
				return;
			}

			if (Worker.id_dept == null && (Worker.type == "продавец" || Worker.type == "заведующий отделом"))
			{
				MessageBox.Show("Работник данной должности должен принадлежать к отделу.");
				return;
			}
			else if (Worker.id_dept != null && (Worker.type == "директор" || Worker.type == "системный администратор"))
			{
				MessageBox.Show("Работник данной должности не может принадлежать к отделу.");
				return;
			}

			using (TradingHouseEntities db = new TradingHouseEntities())
			{
				if (Worker.id == 0) db.Worker.Add(Worker);
				else db.Entry(Worker).State = EntityState.Modified;

				db.SaveChanges();
			}

			string testAssemblyName = "Microsoft.VisualStudio.TestPlatform.TestFramework";
			bool isInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(testAssemblyName));

			if (!isInUnitTest) DialogResult = true;
			Close();
		}
	}
}