using System;
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
          Console.WriteLine($"required file {path} not found.");
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
        var myOrder = new context.Order{ LocationId = locationId, CustomerId = customerId };
        // OrderId should be set automatically by the context? look up Identity handling in EF
        context.Sorder.add(myOrder);
        //var groups = from s in stuff group s by s into g
        //  select new { Stuff = g.Key, Count = g.Count()
        foreach (var kv in groups) {
          var myOrderItem = new OrderItem { OrderId = myOrder.Id, ItemId = kv.Key, ItemCount = kv.Value };
          context.Add(myOrderItem);
        }
      }
      void addCustomerByName(string name) {
        var myCustomer = new Customer { name = name}
        context.Customer.add(myCustomer);
      }
      void save() {
        context.SaveChanges();
      }
      IEnumerable<Sorder> orderHistoryByLocationId(int locationId) {
        throw new NotImplementedException();
      }
      IEnumerable<Sorder> orderHistoryByCustomerId(int customerId) {
        throw new NotImplementedException();
      }
    }
    public static class DisplayEntity { // implement toString extension method for SOrder class
      public static override string ToString(this Sorder order)
      { // iterate through orderItems and build string
        throw new NotImplementedException();
      }
      public static override string ToString(this Item item)
      {
        throw new NotImplementedException();
      }
    }
}
