drop table if exists [hibernate_unique_key];
drop table if exists [categories_products];
drop table if exists [order_lines];
drop table if exists [products];
drop table if exists [orders];
drop table if exists [categories];


CREATE TABLE [categories] (
	[id] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
	[name] varchar(50)  NOT NULL
);

CREATE TABLE [orders] (
    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [date] datetime NOT NULL,
    [customer_name] nvarchar(50) NOT NULL
);

CREATE TABLE [products] (
    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [name] nvarchar(50) NOT NULL,
    [price] decimal NOT NULL
);

CREATE TABLE [categories_products] (
    [category_id] INTEGER NOT NULL,
    [product_id] INTEGER NOT NULL,
    CONSTRAINT [PK_categories_products] PRIMARY KEY ([category_id], [product_id]),
    CONSTRAINT [FK_categories_products_category_id_categories_id] FOREIGN KEY ([category_id]) REFERENCES [categories] ([id]),
    CONSTRAINT [FK_categories_products_product_id_products_id] FOREIGN KEY ([product_id]) REFERENCES [products] ([id])
);

CREATE TABLE [order_lines] (
    [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [order_id] INTEGER NOT NULL,
    [product_id] INTEGER NOT NULL,
    [quantity] INTEGER NOT NULL,
    CONSTRAINT [FK_order_lines_order_id_orders_date] FOREIGN KEY ([order_id]) REFERENCES [orders] ([date]),
    CONSTRAINT [FK_order_lines_product_id_products_id] FOREIGN KEY ([product_id]) REFERENCES [products] ([id])
);

CREATE TABLE [hibernate_unique_key] (
	[table_name] VARCHAR(120)  PRIMARY KEY NULL,
	[next_hi] INTEGER  NULL
);
