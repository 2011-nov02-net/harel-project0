INSERT INTO Store.Item
( [Name] )
VALUES
( 'Apple' ),
( 'Orange' ),
( 'Banana' )
GO

INSERT INTO Store.Customer
( [Name] )
VALUES
( 'Mike' );
GO

INSERT INTO Store.Location
( [Name] )
VALUES
( 'America' );
GO

INSERT INTO Store.LocationItem
( LocationId, ItemId, ItemCount )
VALUES
( (SELECT TOP (1) Id FROM Store.Location),
    (SELECT TOP (1) Id FROM Store.Item WHERE [Name] = 'Apple'),
    1000000000 );
GO


INSERT INTO Store.LocationItem
( LocationId, ItemId, ItemCount )
VALUES
( (SELECT TOP (1) Id FROM Store.Location),
    (SELECT TOP (1) Id FROM Store.Item WHERE [Name] = 'Orange'),
    1000000000);
GO


INSERT INTO Store.LocationItem
( LocationId, ItemId, ItemCount )
VALUES
( (SELECT TOP (1) Id FROM Store.Location),
    (SELECT TOP (1) Id FROM Store.Item WHERE [Name] = 'Banana'),
    1000000000);
GO
