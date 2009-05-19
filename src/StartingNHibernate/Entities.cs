using System;
using Iesi.Collections.Generic;

namespace StartingNHibernate
{
	public class Product
	{
		public Product()
		{
			Categories = new HashedSet<Category>();
		}

		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual decimal Price { get; set; }

		public virtual ISet<Category> Categories { get; set; }
	}

	public class Category
	{
		public Category()
		{
			Products = new HashedSet<Product>();
		}

		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual ISet<Product> Products { get; set; }
	}

	public class Order
	{
		public Order()
		{
			Items = new HashedSet<OrderLine>();
		}

		public virtual int Id { get; set; }
		public virtual DateTime Date { get; set; }
		public virtual string CustomerName { get; set; }

		public virtual ISet<OrderLine> Items { get; set; }
	}

	public class OrderLine
	{
		public virtual int Id { get; set; }
		public virtual int Quantity { get; set; }
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
	}
}