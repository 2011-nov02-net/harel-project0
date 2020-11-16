using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Business
{
  public interface IStore
  {
    void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds);
    void addCustomerByName(string name);
    bool doesExistCustomerById(int customerId);
    bool doesExistLocationById(int locationId);
    bool doesExistOrderById(int orderId);
    bool doesExistItemById(int itemId);
    Sorder findOrderById(int orderId);
    List<Customer> getCustomersByName(string name);
    IEnumerable<Sorder> orderHistoryByLocationId(int locationId);
    IEnumerable<Sorder> orderHistoryByCustomerId(int customerId);
    void save();
  }
  /// documentation with <summary> XML comments on all public types and members
  /// optional: documentation with <params> and <return> on public methods
  public class Store : IStore
  {
    const string connectionStringPath = "../project0-connection-string.txt";
    private static project0Context context;
    private IEnumerable<Sorder> Orders {
      get => (IEnumerable<Sorder>) context.Sorders;
      set => context.Sorders = (DbSet<Sorder>) value;
    }
    private IEnumerable<Location> Locations {
      get => (IEnumerable<Location>) context.Locations;
      set => context.Locations = (DbSet<Location>) value;
    }
    private IEnumerable<Customer> Customers {
      get => (IEnumerable<Customer>) context.Customers;
      set => context.Customers = (DbSet<Customer>) value;
    }
    private IEnumerable<Item> Items {
      get => (IEnumerable<Item>) context.Items;
      set => context.Items = (DbSet<Item>) value;
    }
    public Store() {
      var optionsBuilder = new DbContextOptionsBuilder<project0Context>();
      string connectionString;
      try {
        connectionString = File.ReadAllText(connectionStringPath); // strip text
      } catch (IOException) {
        Console.WriteLine($"required file {connectionStringPath} not found.");
        throw new Exception();
      }
      optionsBuilder.UseSqlServer(connectionString);
      //using var logStream = new StreamWriter("ef-logs.txt");
      //optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
      context = new project0Context(optionsBuilder.Options);
    }
    public bool doesExistCustomerById(int customerId) {
      return ((DbSet<Customer>)Customers).Find(customerId) != null;
    }
    public bool doesExistLocationById(int locationId) {
      return ((DbSet<Location>)Locations).Find(locationId) != null;
    }
    public bool doesExistOrderById(int orderId) {
      return ((DbSet<Sorder>)Orders).Find(orderId) != null;
    }
    public bool doesExistItemById(int itemId) {
      return ((DbSet<Item>)Items).Find(itemId) != null;
    }
    public void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds){
      var myOrder = new Sorder{ LocationId = locationId, CustomerId = customerId };
      // OrderId should be set automatically by the context? look up Identity handling in EF
      ((DbSet<Sorder>)Orders).Add(myOrder);
      context.SaveChanges();
      foreach (var grouping in itemIds.GroupBy(x => x)) {
        var myOrderItem = new OrderItem {
          OrderId = myOrder.Id,
          ItemId = grouping.Key,
          ItemCount = grouping.Count()
        };
        context.Add(myOrderItem);
      }
      context.SaveChanges();
    }
    public List<Customer> getCustomersByName(string name)
    {
      return ((DbSet<Customer>)Customers).Where(x => x.Name == name).ToList();
    }
    public Sorder findOrderById(int orderId) 
    {
      return ((DbSet<Sorder>)Orders).Find(orderId);   
    }
    public void addCustomerByName(string name) {
      var myCustomer = new Customer { Name = name};
      ((DbSet<Customer>)Customers).Add(myCustomer);
      context.SaveChanges();
    }
    public void save() {
      context.SaveChanges();
    }
    public IEnumerable<Sorder> orderHistoryByLocationId(int locationId) {
      if (doesExistLocationById(locationId)) {
        return (IEnumerable<Sorder>) Locations.Where(x => x.Id == locationId).First().Sorders;
      } else {
        throw new ArgumentException("Location Id not found.");
      }
      //return from order in context.Sorder where order.LocationId == locationId select order;
    }
    public IEnumerable<Sorder> orderHistoryByCustomerId(int customerId) {
      if (doesExistCustomerById(customerId)) {
        return (IEnumerable<Sorder>) ((DbSet<Customer>)Customers).Find(customerId).Sorders;
      }
      else {
        throw new ArgumentException("Customer Id not found.");
      }
      //return from order in context.Sorder where order.CustomerId == customerId select order;
    }
  }
  public static class DisplayEntity { // implement toString extension method for SOrder class
    public static string ToString(this Sorder order)
    {
      //throw new NotImplementedException();
      return $"Id: {order.Id}, {order.TimePlaced}" + String.Join(", ",
        order.OrderItems.Select(x => $"{x.Item}: {x.ItemCount}") );
      //(from kv in OrderItem where kv.Order == order
      //select $"{kv.Item}: {kv.itemCount}"
      // iterate through orderItems and build string
      
    }
    public static string ToString(this Item item)
    {
      //throw new NotImplementedException();
      return item.Name;
    }
  }
}
