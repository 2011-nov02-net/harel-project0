exec sp_rename 'Store.Sorder', 'Store.Order'

alter table Store.OrderItem
foreign key (OrderId) references Store.Order(Id)