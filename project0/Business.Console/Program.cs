using System;
using System.Collections.Generic;
using Business;

namespace Business.Console
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
            Console.WriteLine("Hello World!");
            store = (IStore) new Store();
            Menu();
        }
        static void Menu()
        {
            while (true)
            {
                Console.WriteLine("Select An Activity?");
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
                    throw new ArgumentException();
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
              Console.WriteLine($"Order Id: {myOrderId}");
              Console.WriteLine("Contents");
              Console.WriteLine(store.findOrderById(myOrderId));
          } catch (ArgumentNullException) {
              Console.WriteLine("No such order on record.");
          }
        }
        static void displayLocationOrderHistory()
        {
          try {
              Console.WriteLine("Enter store location id.");
              var myLocationId = Convert.ToInt32(Console.ReadLine());
              foreach (var order in store.orderHistoryByLocationId(myLocationId)) Console.WriteLine(order);
          } catch (ArgumentNullException) {
              Console.WriteLine("No such location on record.");
          }
        }
        static void displayCustomerOrderHistory()
        {
          try {
              Console.WriteLine("Enter customer id.");
              var myCustomerId = Convert.ToInt32(Console.ReadLine());
              foreach (var order in store.orderHistoryByCustomerId(myCustomerId)) Console.WriteLine(order);
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
              Console.WriteLine("Enter items by id on each line (stop to stop).");
              var input = "";
              var myItemIds = new List<int>();
              while (input != "stop")
              {
                  input = Console.ReadLine();
                  if (input != "stop") myItemIds.Add(Convert.ToInt32(input));
              }
              store.placeOrder(myCustomerId, myLocationId, (IEnumerable<int>) myItemIds);
          }
          catch (Exception) {
              Console.WriteLine("Invalid Input");
          }
        }
    }
}
