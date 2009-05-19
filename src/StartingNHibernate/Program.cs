using System;
using System.Data.SQLite;
using NHibernate;
using NHibernate.Cfg;

namespace StartingNHibernate
{
	internal class Program
	{
		private static Configuration _cfg;
		private static ISessionFactory _sessionFactory;
		private static ISession _session;


		private static void Main(string[] args)
		{
			ConfigureNH();
			EmptyDB();
			using (_session = _sessionFactory.OpenSession())
			{
				CreateProducts();
				CreateCategories();
				CreateOrders();
				AssociateCategories();
			}

			using (_session = _sessionFactory.OpenSession())
			{
				ListAllData();
			}

			Console.WriteLine("Done. Press enter.");
			Console.ReadLine();
		}

		private static void AssociateCategories()
		{
			var products = _session.CreateCriteria(typeof (Product)).List<Product>();
			var categories = _session.CreateCriteria(typeof (Category)).List<Category>();

			int i = 0;
			foreach (var p in products)
			{
				int cat1 = i++%10, cat2 = i++%10, cat3 = i++%10;
				p.Categories.Add(categories[cat1]);
				p.Categories.Add(categories[cat2]);
				p.Categories.Add(categories[cat3]);
				_session.Save(p);
			}
			_session.Flush();
		}

		private static void CreateOrders()
		{
			Console.WriteLine("Creating Orders");
			var products = _session.CreateCriteria(typeof (Product)).List<Product>();

			for (int i = 1; i <= 3; i++)
			{
				var order = new Order
				            	{
				            		Date = new DateTime(2000 + i, i, i),
				            		CustomerName = "Customer #" + i
				            	};
				order.Items.Add(new OrderLine {Order = order, Product = products[(i - 1)*3], Quantity = i});
				order.Items.Add(new OrderLine {Order = order, Product = products[1 + (i - 1)*3], Quantity = i + 1});

				_session.Save(order);
			}

			_session.Flush();
		}

		private static void ListAllData()
		{
			var products = _session.CreateCriteria(typeof (Product)).List<Product>();
			foreach (Product p in products)
			{
				Console.WriteLine("Product: id: {0}, Name: {1}, Price {2:C}", p.Id, p.Name, p.Price);
				Console.WriteLine("\t Categories: {0}", p.Categories.Count);
				foreach (Category c in p.Categories)
				{
					Console.WriteLine("\t  * Category: {0}", c.Name);
				}
			}


			var categories = _session.CreateCriteria(typeof (Category)).List<Category>();
			foreach (Category c in categories)
			{
				Console.WriteLine("Category: id: {0}, Name: {1}", c.Id, c.Name);
				Console.WriteLine("\t Products: {0}", c.Products.Count);
				foreach (Product p in c.Products)
				{
					Console.WriteLine("\t  * Product: {0}", p.Name);
				}
			}

			var orders = _session.CreateCriteria(typeof (Order)).List<Order>();
			foreach (var order in orders)
			{
				Console.WriteLine("Order: id: {0}, Cust: {1}", order.Id, order.CustomerName);
				Console.WriteLine("\t Items: {0}", order.Items.Count);
				foreach (var item in order.Items)
				{
					Console.WriteLine("\t  * id: {0}, item: {1}, quantity: {2}",
					                  item.Id, item.Product.Name, item.Quantity);
				}
			}
		}

		private static void CreateProducts()
		{
			Console.WriteLine("Creating Products");
			for (int i = 1; i <= 10; i++)
			{
				var p = new Product
				        	{
				        		Name = "Product " + i,
				        		Price = 9*i
				        	};
				_session.Save(p);
			}
			_session.Flush();
		}

		private static void CreateCategories()
		{
			Console.WriteLine("Creating Categories");
			for (int i = 1; i <= 10; i++)
			{
				var c = new Category
				        	{
				        		Name = "Category " + i
				        	};
				_session.Save(c);
			}
			_session.Flush();
		}

		private static void ConfigureNH()
		{
			//here we read the configuration from the application's
			// config file. See the <hibernate-configuration > section in app.config.
			_cfg = new Configuration();
			_cfg.Configure();
			_cfg.AddAssembly(typeof (Product).Assembly);
			_sessionFactory = _cfg.BuildSessionFactory();
		}

		private static void EmptyDB()
		{
			string cnStr = _cfg.GetProperty("connection.connection_string");
			using (var cn = new SQLiteConnection(cnStr))
			{
				cn.Open();
				var command =
					new SQLiteCommand(
						@"
					delete from order_lines;
					delete from categories_products;
					delete from orders;
					delete from products;
					delete from categories;
					delete from hibernate_unique_key;
				",
						cn);

				command.ExecuteNonQuery();
			}
		}
	}
}