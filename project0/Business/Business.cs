using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
    internal static IEnumerable<Sorder> Orders {
      get => (IEnumerable<Sorder>) context.Sorders;
      set => context.Sorders = (DbSet<Sorder>) value;
    }
    internal static IEnumerable<Location> Locations {
      get => (IEnumerable<Location>) context.Locations;
      set => context.Locations = (DbSet<Location>) value;
    }
    internal static IEnumerable<Customer> Customers {
      get => (IEnumerable<Customer>) context.Customers;
      set => context.Customers = (DbSet<Customer>) value;
    }
    internal static IEnumerable<Item> Items {
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
      var inventory = context.LocationItems
        .Where(x => x.LocationId == locationId)
        .Where(x => itemIds.Contains(x.ItemId)).ToList();
      foreach (var grouping in itemIds.GroupBy(x => x)) {
        var myOrderItem = new OrderItem {
          OrderId = myOrder.Id,
          ItemId = grouping.Key,
          ItemCount = grouping.Count()
        };
        var myLocationItem = inventory.Find(x => x.ItemId == myOrderItem.ItemId);
        myLocationItem.ItemCount -= myOrderItem.ItemCount;
        context.LocationItems.Update(myLocationItem);
        //if (myLocationItem.ItemCount >= myOrderItem.ItemCount)
        context.OrderItems.Add(myOrderItem);
      }
      try {
        context.SaveChanges();
      } catch (Exception e) {
        context.Sorders.Remove(myOrder);
        throw e;
      }
    }
    public List<Customer> getCustomersByName(string name)
    {
      return ((DbSet<Customer>)Customers).Where(x => x.Name == name).ToList();
    }
    public Sorder findOrderById(int orderId) 
    {
      var output = ((DbSet<Sorder>)Orders).Where(o => o.Id == orderId)
      .Include(o => o.OrderItems).ThenInclude(io => io.Item)
      .Include(o => o.Customer).Include(o => o.Location);
      output.Load();
      return output.First(); 
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
        return (IEnumerable<Sorder>) ((DbSet<Location>)Locations)
        .Where(x => x.Id == locationId)
        .Include(l => l.Sorders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Item)
        .First().Sorders.ToList();
      } else {
        throw new ArgumentException("Location Id not found.");
      }
      //return from order in context.Sorder where order.LocationId == locationId select order;
    }
    public IEnumerable<Sorder> orderHistoryByCustomerId(int customerId) {
      if (doesExistCustomerById(customerId)) {
        return (IEnumerable<Sorder>) ((DbSet<Customer>)Customers)
        .Where(x => x.Id == customerId)
        .Include(c => c.Sorders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Item)
        .First().Sorders.ToList();
      }
      else {
        throw new ArgumentException("Customer Id not found.");
      }
      //return from order in context.Sorder where order.CustomerId == customerId select order;
    }
  }
  public partial class Sorder {
    public override string ToString()
    {
        return $"Id: {this.Id}, {this.TimePlaced}\n" + String.Join(", ",
        this.OrderItems.Select(x => $"{x.Item}: {x.ItemCount}").ToList() );
        // Location: {this.Location.Name}, Customer:{this.Customer.Name}
    } 
  }
  public partial class Item {
    public override string ToString() {
        return this.Name+$" ({this.Id})";
    }
  }
  public partial class Customer {
    public override string ToString() {
      return $"id:{this.Id},\t Name:{this.Name}";
    }
  }
  public partial class Location {
    public override string ToString() {
        return $"id:{this.Id},\t Name:{this.Name}";
    }
  }
}
