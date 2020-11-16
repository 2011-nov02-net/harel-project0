using System;
using System.Collections.Generic;
using Business;
using System.Linq;


namespace Business.ConsoleUI
{
    /// <summary>
    /// </summary>
    class Program
    {
        internal readonly static List<string> activities = new List<string> {
            "add a new customer",
            "search customers by name",
            "display details of an order",
            "display all order history of a store location",
            "display all order history of a customer",
            "place orders to store locations for customers",
        };
        
        private static IStore store;
        
        static void Main(string[] args)
        {
            store = (IStore) new Store();
            Menu();
        }
        static void Menu()
        {
            while (true)
            {
                System.Console.WriteLine("Select An Activity?");
                for (int i = 0; i < activities.Count; i++) Console.WriteLine(activities[i]+$"({i})");
                var activityCode = Convert.ToInt32(Console.ReadLine());
                if (activityCode == -1) return;
                if (activityCode >= 0 && activityCode < activities.Count) {
                    exectuteCode(activityCode);
                } else Console.WriteLine("That is not a valid option.");
            }
        }
        static void exectuteCode(int i)
        {
            switch (i)
            {
                case 0:
                    addCustomer();
                    break;
                case 1:
                    displayCustomersByName();
                    break;
                case 2:
                    displayOrderDetails();
                    break;
                case 3:
                    displayLocationOrderHistory();
                    break;
                case 4:
                    displayCustomerOrderHistory();
                    break;
                case 5:
                    consolePlaceOrder();
                    break;
                default:
                    throw new ArgumentException($"i:{i}");
            }
        }
        static void addCustomer()
        {
          Console.WriteLine("Enter customer name.");
          store.addCustomerByName(Console.ReadLine());
          store.save();
        }
        static void displayCustomersByName()
        {
          Console.WriteLine("Enter customer name.");
          var customers = store.getCustomersByName(Console.ReadLine());
          foreach (var customer in customers) Console.WriteLine(customer);
          if (customers.Count == 0) Console.WriteLine("No customers with given name.");
        }
        static void displayOrderDetails()
        {
          try {
              Console.WriteLine("Enter order id.");
              var myOrderId = Convert.ToInt32(Console.ReadLine());
              var myOrder = store.findOrderById(myOrderId);
              Console.WriteLine($"Order Id: {myOrderId}");
              Console.WriteLine("Contents");
              Console.WriteLine(myOrder);
          } catch (FormatException) {
              Console.WriteLine("Invalid formalt.");
          } catch (InvalidOperationException) {
              Console.WriteLine("No such order on record.");
          }
        }
        static void displayLocationOrderHistory()
        {
          try {
              Console.WriteLine("Enter store location id.");
              var myLocationId = Convert.ToInt32(Console.ReadLine());
              var myOrders = store.orderHistoryByLocationId(myLocationId).ToList();
              foreach (var order in myOrders) Console.WriteLine(order);
          } catch (ArgumentNullException) {
              Console.WriteLine("No such location on record.");
          } catch (FormatException) {
              Console.WriteLine("Invalid input.");
          }
        }
        static void displayCustomerOrderHistory()
        {
          try {
              Console.WriteLine("Enter customer id.");
              var myCustomerId = Convert.ToInt32(Console.ReadLine());
              var myOrders = store.orderHistoryByCustomerId(myCustomerId).ToList();
              foreach (var order in myOrders) Console.WriteLine(order);
          } catch (ArgumentNullException) {
              Console.WriteLine("No such location on record.");
          }
        }
        static void consolePlaceOrder() // fix to not expose object model only interface
        {
          try
          {
              Console.WriteLine("Enter customer id.");
              var myCustomerId = Convert.ToInt32(Console.ReadLine());
              Console.WriteLine("Enter location id.");
              var myLocationId = Convert.ToInt32(Console.ReadLine());
              if (!store.doesExistLocationById(myLocationId)) {
                  Console.WriteLine("Location Id not found.");
                  return;
              }
              Console.WriteLine("Enter items by id on each line (stop to stop).");
              var input = Console.ReadLine();
              var myItemIds = new List<int>();
              while (input != "stop")
              {
                  try {
                    var myItemId = Convert.ToInt32(input);
                    if (store.doesExistItemById(myItemId)) myItemIds.Add(myItemId);
                    else Console.WriteLine("Item Id not found.");
                  } catch (Exception) {
                    Console.WriteLine("Invalid input, is that an item id?");
                  }
                  input = Console.ReadLine();
              }
              store.placeOrder(myCustomerId, myLocationId, (IEnumerable<int>) myItemIds);
          }
          catch (Exception) {
              Console.WriteLine("Invalid Input.");
          }
        }
    }
}
