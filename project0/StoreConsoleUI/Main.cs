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
            "place orders to store locations for customers",
            "add a new customer",
            "search customers by name",
            "display details of an order",
            "display all order history of a store location",
            "display all order history of a customer"
        };
        static void Main(string[] args)
        {
            var store = new StoreFront();
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
    }
}
