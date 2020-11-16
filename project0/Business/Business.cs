using System;
using System.Linq;
using System.IO;
using System.Text;


namespace Business
{
  interface IStore
  {
    void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds);
    void addCustomerByName(string name);
    bool doesExistCustomerById(int customerId);
    bool doesExistLocationById(int locationId);
    bool doesExistOrderById(int orderId);
    bool doesExistItemById(int itemId);
    IEnumerable<Sorder> orderHistoryByLocationId(int locationId);
    IEnumerable<Sorder> orderHistoryByCustomerId(int customerId);
    void save();
  }
  /// documentation with <summary> XML comments on all public types and members
  /// optional: documentation with <params> and <return> on public methods
  public class Store : IStore
  {
    const string connectionStringPath = "../../project0-connection-string.txt";
    private static project0Context context;
    /* /// Considering accessing tables through properties.
    private IEnumerator<Sorder> Orders {
      get => (IEnumerator<Sorder>) context.Sorders;
      set => context.Sorders = (DbSet<Sorder>) value;
    }*/
    public Store() {
      string connectionString;
      try {
        connectionString = File.ReadAllText(connectionStringPath); // strip text
      } catch (IOException) {
        Console.WriteLine($"required file {connectionStringPath} not found.");
        throw new Exception();
      }
      var optionsBuilder = new DbContextOptionsBuilder<project0Context>().UseSqlServer(connectionString);
      //optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
      context = project0Context(optionsBuilder.Options);
    }
    public bool doesExistCustomerById(int customerId) {
      return context.Customer.Where(customer => customer.Id == customerId).toList().Length() != 0;
    }
    public bool doesExistLocationById(int locationId) {
      return context.Location.Where(location => location.Id == locationId).toList().Length() != 0;
    }
    public bool doesExistOrderById(int orderId) {
      return context.Sorder.Where(order => order.Id == orderId).toList().Length() != 0;
    }
    public bool doesExistItemById(int itemId) {
      return context.Item.Where(item => item.Id == itemId).toList().Length() != 0;
    }
    public void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds){
      var myOrder = new Order{ LocationId = locationId, CustomerId = customerId };
      // OrderId should be set automatically by the context? look up Identity handling in EF
      context.Sorder.add(myOrder);
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
    public void addCustomerByName(string name) {
      var myCustomer = new Customer { Name = name};
      context.Customer.add(myCustomer);
      context.SaveChanges();
    }
    public void save() {
      context.SaveChanges();
    }
    public IEnumerable<Sorder> orderHistoryByLocationId(int locationId) {
      throw new NotImplementedException();
      return from order in context.Sorder where order.LocationId == locationId select order;
    }
    public IEnumerable<Sorder> orderHistoryByCustomerId(int customerId) {
      throw new NotImplementedException();
      return from order in context.Sorder where order.CustomerId == customerId select order;
    }
  }
  public static class DisplayEntity { // implement toString extension method for SOrder class
    public static override string ToString(this Sorder order)
    {
      //throw new NotImplementedException();
      return $"{Id: {order.Id}, {order.TimePlaced}" + String.Join(" ",
        OrderItem.Where(x => x.Order == order).Select(x => $"{x.Item}: {x.itemCount}") );
      //(from kv in OrderItem where kv.Order == order
      //select $"{kv.Item}: {kv.itemCount}"
      // iterate through orderItems and build string
      
    }
    public static override string ToString(this Item item)
    {
      //throw new NotImplementedException();
      return item.Name;
    }
  }
}
