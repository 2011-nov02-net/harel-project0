using System;
using System.Collections.Generic;
using Store;

namespace StoreConsoleUI
{
    /// <summary>
    /* User interface for store application project
    place orders to store locations for customers
    add a new customer
    search customers by name
    display details of an order
    display all order history of a store location
    display all order history of a customer
    input validation */
    /// </summary>
    /*
    
    */
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
        private static StoreFront store;
        static void Main(string[] args)
        {
            store = new StoreFront();
        }
        static void Menu() {
            while (true)
            {
                Console.WriteLine("Select An Activity?");
                for (int i = 0; i < activities.Count; i++) Console.WriteLine(activities[i]+$"({i})");
                var activityCode = Convert.ToInt32(Console.ReadLine());
                if (activityCode == -1) {
                    return;
                }
                if (activityCode >= 0 && activityCode < activities.Count) {
                    throw new NotImplementedException("Not implemented.");
                } else Console.WriteLine("That is not a valid option.");
            }
        }
        static void exectuteCode(int i) {
            switch (i)
            {
                case 0:
                    Console.WriteLine("Enter customer name.");
                    store.addCustomer(new Customer(Console.ReadLine()));
                    break;
                case 1:
                    Console.WriteLine("Enter customer name.");
                    var customers = store.getCustomersByName(Console.ReadLine());
                    // do stuff to display customers
                    break;
                case 2: 
                    Console.WriteLine("Enter order id.");
                    var myOrderId = Convert.ToInt32(Console.ReadLine());
                    try {
                        var order = store.Orders.Find(order => order.OrderId == myOrderId);
                        Console.WriteLine($"Order Id: {order.OrderId}");
                        Console.WriteLine("Contents");
                        Console.WriteLine(order.ToString());
                    } catch (ArgumentNullException) {
                        Console.WriteLine("No such order on record.");
                    }
                    break;
                case 3:
                    Console.WriteLine("Enter store location id.");
                    var myLocationId = Convert.ToInt32(Console.ReadLine());
                    try {
                        var location = store.Locations.Find(location => location.LocationId == myLocationId);
                        var orders = store.orderHistory(location);
                        // display orders
                    } catch (ArgumentNullException) {
                        Console.WriteLine("No such location on record.");
                    }
                    break;
                case 4:
                    Console.WriteLine("Enter customer id.");
                    var myCustomerId = Convert.ToInt32(Console.ReadLine());
                    try {
                        var customer = store.Customers.Find(customer => customer.CustomerId == myCustomerId);
                        var orders = store.orderHistory(customer);
                        // Display orders
                    } catch (ArgumentNullException) {
                        Console.WriteLine("No such location on record.");
                    }
                    break;
                case 5:
                    // take down order details on console
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
