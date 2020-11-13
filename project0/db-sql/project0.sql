CREATE SCHEMA Store;
GO

CREATE TABLE Store.Item (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE Store.Customer (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150) NOT NULL
);
GO

CREATE TABLE Store.Location (
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Name NVARCHAR(150) Null
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
    OrderId INT NOT NULL PRIMARY KEY IDENTITY,
    LocationId INT NOT NULL FOREIGN KEY REFERENCES Store.Location(Id),
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Store.Customer(Id)
);
GO 

CREATE TABLE Store.OrderItem (
    OrderId INT NOT NULL FOREIGN KEY REFERENCES Store.SOrder(OrderId),
    ItemId INT NOT NULL FOREIGN KEY REFERENCES Store.Item(Id),
    ItemCount INT NOT NULL DEFAULT 0 CHECK (ItemCount >= 0 AND ItemCount < 100),
    PRIMARY KEY (OrderId, ItemId)
);
GO


INSERT INTO Store.Item
( Name )
VALUES
( 'Apple'),
('Orange' ),
('Banana')
GO

INSERT INTO Store.Customer
( Name )
VALUES
( 'Mike');
GO

INSERT INTO Store.[Location]
( Name )
VALUES
( 'America');
GO

INSERT INTO Store.[LocationItem]
( LocationId, ItemId, ItemCount )
VALUES
( (Select top (1) Id from Store.[Location]),
(select top (1) id from store.Item where name = 'Apple'),
1000000000);
GO


INSERT INTO Store.[LocationItem]
( LocationId, ItemId, ItemCount )
VALUES
( (Select top (1) Id from Store.[Location]),
(select top (1) id from store.Item where name = 'Orange'),
1000000000);
GO


INSERT INTO Store.[LocationItem]
( LocationId, ItemId, ItemCount )
VALUES
( (Select top (1) Id from Store.[Location]),
(select top (1) id from store.Item where name = 'Banana'),
1000000000);
GO