using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;

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
    String findOrderById(int orderId);
    List<Customer> getCustomersByName(string name);
    IEnumerable<Order> orderHistoryByLocationId(int locationId);
    IEnumerable<Order> orderHistoryByCustomerId(int customerId);
    void save();
  }
  /// documentation with <summary> XML comments on all public types and members
  /// optional: documentation with <params> and <return> on public methods
  
  /// <summary>
  /// The main Business class.
  /// Implements the IStore interface with Effect framework database connection
  /// </summary>
  public class Store : IStore
  {
    const string connectionStringPath = "../project0-connection-string.txt";
    private static project0Context context;
    internal static IEnumerable<Order> Orders {
      get => (IEnumerable<Order>) context.Orders;
      set => context.Orders = (DbSet<Order>) value;
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
    /// <summary>
    /// The empty constructor
    /// opens a connection through a context and stores it in a static member
    /// </summary>
    public Store() {
      string connectionString;
      try {
        connectionString = File.ReadAllText(connectionStringPath); // strip text
      } catch (IOException) {
        Console.WriteLine($"required file {connectionStringPath} not found.");
        throw new Exception();
      }
      Connect(connectionString);
    }
    public Store(string connectionStringPath) {
      string connectionString;
      try {
        connectionString = File.ReadAllText(connectionStringPath);
      } catch (IOException) {
        Console.WriteLine($"required file {connectionStringPath} not found.");
        throw new Exception();
      }
      Connect(connectionString);
    }
    private static void Connect(string connectionString) {
      var optionsBuilder = new DbContextOptionsBuilder<project0Context>();
      optionsBuilder.UseSqlServer(connectionString);
      context = new project0Context(optionsBuilder.Options);
    }
    /// <summary>
    /// check for the existence of customer by id
    /// </summary>
    public bool doesExistCustomerById(int customerId) {
      return ((DbSet<Customer>)Customers).Find(customerId) != null;
    }
    /// <summary>
    /// check for the existence of a location by id
    /// </summary>
    public bool doesExistLocationById(int locationId) {
      return ((DbSet<Location>)Locations).Find(locationId) != null;
    }
    /// <summary>
    /// check for the existence of a Order by id
    /// </summary>
    public bool doesExistOrderById(int orderId) {
      return ((DbSet<Order>)Orders).Find(orderId) != null;
    }
    /// <summary>
    /// check for the existence of a Item by id
    /// </summary>
    public bool doesExistItemById(int itemId) {
      return ((DbSet<Item>)Items).Find(itemId) != null;
    }
    /// <summary>
    /// Place an order
    /// </summary>
    public void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds){
      var myOrder = new Order{ LocationId = locationId, CustomerId = customerId };
      // OrderId should be set automatically by the context? look up Identity handling in EF
      ((DbSet<Order>)Orders).Add(myOrder);
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
      } catch (Exception ex) {
        context.Orders.Remove(myOrder);
        ExceptionDispatchInfo.Capture(ex).Throw();
      }
    }
    /// <summary>
    /// Make a list of customers with a given name
    /// </summary>
    public List<Customer> getCustomersByName(string name) {
      return ((DbSet<Customer>)Customers).Where(x => x.Name == name).ToList();
    }
    /// <summary>
    /// Find an order by Id and
    /// Collect extended information on it
    /// </summary>
    public String findOrderById(int orderId)
    {
      var myOrder = ((DbSet<Order>)Orders).Where(o => o.Id == orderId)
      .Include(o => o.OrderItems).ThenInclude(io => io.Item)
      .Include(o => o.Customer).Include(o => o.Location).First();
      string output = "";
      output += $"Customer: {myOrder.Customer}\n";
      output += $"Location: {myOrder.Location}\n";
      output += myOrder.ToString();
      return output;
    }
    /// <summary>
    /// Create a new customer with all information other than name autogenerated or default
    /// </summary>
    public void addCustomerByName(string name) {
      var myCustomer = new Customer { Name = name};
      ((DbSet<Customer>)Customers).Add(myCustomer);
      context.SaveChanges();
    }
    /// <summary>
    /// Make persisting changes public to seperate data layer from db context implementation.
    /// </summary>
    public void save() {
      context.SaveChanges();
    }
    /// <summary>
    /// Use LINQ to eagerly collect all the Order objects for a given location.
    /// Collect Item info for all orders
    /// </summary>
    public IEnumerable<Order> orderHistoryByLocationId(int locationId) {
      if (doesExistLocationById(locationId)) {
        return (IEnumerable<Order>) ((DbSet<Location>)Locations)
        .Where(x => x.Id == locationId)
        .Include(l => l.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Item)
        .First().Orders.ToList();
      } else {
        throw new ArgumentException("Location Id not found.");
      }
      //return from order in context.Order where order.LocationId == locationId select order;
    }
    /// <summary>
    /// Use LINQ to eagerly collect all the Order objecsts for a given customer.
    /// Collect Item info for all orders
    /// </summary>
    public IEnumerable<Order> orderHistoryByCustomerId(int customerId) {
      if (doesExistCustomerById(customerId)) {
        return (IEnumerable<Order>) ((DbSet<Customer>)Customers)
        .Where(x => x.Id == customerId)
        .Include(c => c.Orders).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Item)
        .First().Orders.ToList();
      }
      else {
        throw new ArgumentException("Customer Id not found.");
      }
      //return from order in context.Order where order.CustomerId == customerId select order;
    }
  }
  /// <summary>
  /// Other files are autogenerated by database first approach so
  // This developer written file implements string formatting methods for generated classes.
  /// </summary>
  public partial class Order {
    /// <summary>
    /// Display an item in the style
    /// Id: {this.Id}, {this.TimePlaced}
    /// {Item}, {Item}, ...
    /// </summary>
    public override string ToString()
    {
        return $"Id: {this.Id}, {this.TimePlaced}\n" + String.Join(", ",
        this.OrderItems.Select(x => $"{x.Item}: {x.ItemCount}").ToList() );
        // Location: {this.Location.Name}, Customer:{this.Customer.Name}
    }
  }
  public partial class Item {
    /// <summary>
    /// Display an item in the style "{Name} ({Id})"
    /// </summary>
    public override string ToString() {
        return this.Name+$" ({this.Id})";
    }
  }
  public partial class Customer {
    /// <summary>
    /// Display an customer in the style "id: {id}, Name: {Name}"
    /// </summary>
    public override string ToString() {
      return $"id: {this.Id}, Name: {this.Name}";
    }
  }
  public partial class Location {
    /// <summary>
    /// Display an location in the style "id: {id}, Name: {Name}"
    /// </summary>
    public override string ToString() {
        return $"id: {this.Id}, Name: {this.Name}";
    }
  }
}
