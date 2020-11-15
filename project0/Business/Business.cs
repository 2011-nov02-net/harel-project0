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
        IEnumerable<Sorder> orderHistoryByLocationId(int locationId);
        IEnumerable<Sorder> orderHistoryByCustomerId(int customerId);
        void save();
    }
    /// documentation with <summary> XML comments on all public types and members (optional: <params> and <return>)
    public class Store : IStore
    {
      const string connectionStringPath = "../../project0-connection-string.txt";
      private static project0Context context;
      public Store() {
        string connectionString;
        try {
          connectionString = File.ReadAllText(connectionStringPath); // strip text
        } catch (IOException) {
          Console.WriteLine($"required file {connectionStringPath} not found.");
        }
        var optionsBuilder = new DbContextOptionsBuilder<project0Context>().UseSqlServer(connectionString);
        //optionsBuilder.LogTo(logStream.WriteLine, LogLevel.Information);
        context = project0Context(optionsBuilder.Options);
      }
      bool doesExistCustomerById(int customerId) {
        return context.Customer.Where(customer => customer.Id == customerId).toList().Length() != 0;
      }
      bool doesExistLocationById(int locationId) {
        return context.Location.Where(location => location.Id == locationId).toList().Length() != 0;
      }
      bool doesExistOrderById(int orderId) {
        return context.Sorder.Where(order => order.Id == orderId).toList().Length() != 0;
      }
      void placeOrder(int customerId, int locationId, IEnumerable<int> itemIds){
        var myOrder = new Order{ LocationId = locationId, CustomerId = customerId };
        // OrderId should be set automatically by the context? look up Identity handling in EF
        context.Sorder.add(myOrder);
        context.SaveChanges();
        foreach (var kv in itemIds.GroupBy(x => x).Count()) {
          var myOrderItem = new OrderItem { OrderId = myOrder.OrderId, ItemId = kv.Key, ItemCount = kv.Value };
          context.Add(myOrderItem);
        }
      }
      void addCustomerByName(string name) {
        var myCustomer = new Customer { Name = name};
        context.Customer.add(myCustomer);
        context.SaveChanges();
      }
      void save() {
        context.SaveChanges();
      }
      IEnumerable<Sorder> orderHistoryByLocationId(int locationId) {
        throw new NotImplementedException();
        /*
        return from order in context.Sorder where order.LocationId == locationId select order
        */
      }
      IEnumerable<Sorder> orderHistoryByCustomerId(int customerId) {
        throw new NotImplementedException();
        /*
        return from order in context.Sorder where order.CustomerId == customerId select order
        */
      }
    }
    public static class DisplayEntity { // implement toString extension method for SOrder class
      public static override string ToString(this Sorder order)
      { // iterate through orderItems and build string
        throw new NotImplementedException(); 
        // $"{order.Id}, {order.TimePlaced}"
        /* from kv in OrderItem where kv.Order == order 
        select $"{kv.Item}: {kv.itemCount}"
        */
      }
      public static override string ToString(this Item item)
      {
        throw new NotImplementedException();
        // return item.Name;
      }
    }
}
