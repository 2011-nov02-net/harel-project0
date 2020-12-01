CREATE SCHEMA Store;
GO

CREATE TABLE Store.Item (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE Store.Customer (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150) NOT NULL,
	DefaultLocationId INT NULL FOREIGN KEY REFERENCES Store.Location(Id)
);
GO

CREATE TABLE Store.Location (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150),
);
GO

CREATE TABLE Store.LocationItem (
	LocationId INT NOT NULL FOREIGN KEY REFERENCES Store.Location(Id),
    ItemId INT NOT NULL FOREIGN KEY REFERENCES Store.Item(Id),
    ItemCount INT NOT NULL DEFAULT 0 CHECK (ItemCount >= 0),
    PRIMARY KEY (LocationId, ItemId)
);
GO

CREATE TABLE Store.SOrder (
    Id INT NOT NULL PRIMARY KEY IDENTITY,
    LocationId INT NOT NULL FOREIGN KEY REFERENCES Store.Location(Id),
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Store.Customer(Id),
    TimePlaced DATETIME NOT NULL DEFAULT SYSDATETIME()
);
GO

CREATE TABLE Store.OrderItem (
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Store.SOrder(Id),
    ItemId INT NOT NULL FOREIGN KEY REFERENCES Store.Item(Id),
    ItemCount INT NOT NULL DEFAULT 0 CHECK (ItemCount >= 0 AND ItemCount < 100),
    PRIMARY KEY (OrderId, ItemId)
);
GO

exec sp_rename '[Store].[Sorder]', '[Store].[Order]';

alter table Store.OrderItem
foreign key (OrderId) references [Store].[Order](Id);
GO