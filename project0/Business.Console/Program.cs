using System;
using System.Collections.Generic;
using Business;
using System.Linq;


namespace Business.ConsoleUI
{
    /// <summary>
    /// Main console UI to IStore interface
    // interacts by sending strings and ints
    /// recieves Sorders, strings and ints
    /// in the future replace EF framework generated Sorder
    /// With a class inheriting from it that implements an interface.
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
                Console.Clear();
                Console.WriteLine("Select An Activity?");
                for (int i = 0; i < activities.Count; i++) Console.WriteLine(activities[i]+$" ({i})");
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
            Console.ReadLine();
        }
        static void addCustomer()
        {
          Console.WriteLine("Enter customer name.");
          var nName = Console.ReadLine();
          store.addCustomerByName(nName);
          store.save();
          Console.WriteLine($"Success, new customer added");
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
              Console.WriteLine(myOrder);
          } catch (InvalidOperationException) {
              Console.WriteLine("No such order on record.");
          } catch (FormatException) {
              Console.WriteLine("Invalid format.");
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
          } catch (ArgumentException) {
              Console.WriteLine("No such location on record.");
          }
        }
        static void displayCustomerOrderHistory()
        {
          try {
              Console.WriteLine("Enter customer id.");
              var myCustomerId = Convert.ToInt32(Console.ReadLine());
              var myOrders = store.orderHistoryByCustomerId(myCustomerId).ToList();
              foreach (var order in myOrders) Console.WriteLine(order);
          } catch (ArgumentException) {
              Console.WriteLine("No such customer on record.");
          } catch (FormatException) {
              Console.WriteLine("Invalid input.");
          }
        }
        static void consolePlaceOrder()
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
              if (!store.doesExistCustomerById(myCustomerId)) {
                  Console.WriteLine("Customer Id not found.");
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
              Console.WriteLine("Success, order placed.");
          }
          catch (Exception) {
              Console.WriteLine("Invalid Input.");
          }
        }
    }
}
