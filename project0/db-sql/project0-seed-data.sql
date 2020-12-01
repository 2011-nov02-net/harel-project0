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
