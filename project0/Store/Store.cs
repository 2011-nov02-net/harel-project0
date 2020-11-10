using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace Store
{
    interface IStore
    {
        void placeOrder(Order order);
        void addCustomer(Customer customer);
        List<Order> orderHistory(Location location);
        List<Order> orderHistory(Customer customer);
    }

    public class StoreFront : IStore
    {
        private List<Location> locations;
        private List<Order> orders;
        List<Customer> customers;
        List<Item> items;

        public List<Order> Orders { get => orders; set => orders = value; }
        public List<Location> Locations { get => locations; set => locations = value; }
        public List<Customer> Customers { get => customers; set => customers = value; }
        public List<Item> Items { get => items; set => items = value; }
        private const string ordersDataPath = "orders.json";
        private const string customersDataPath = "customers.json";
        private const string locationsDataPath = "locations.json";
        private const string itemsDataPath = "items.json";

        public StoreFront () {
            loadData();
            //items.Add(new Item("apple"));
            //items.Add(new Item("orange"));
            //items.Add(new Item("banana"));
            Order.Items = this.items;
            var loc = new Location();
            foreach (var item in items) {
                loc.Inventory.Add(item.ItemId, 300);
            }
            locations.Add( loc );
            storeData();
        }
        private void loadData() 
        {
            try
            {
                locations = JsonSerializer.Deserialize<List<Location>>(File.ReadAllText(locationsDataPath));
                orders = JsonSerializer.Deserialize<List<Order>>(File.ReadAllText(ordersDataPath));
                customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(customersDataPath));
                items = JsonSerializer.Deserialize<List<Item>>(File.ReadAllText(itemsDataPath));
            }
            catch (Exception)
            {
                Console.WriteLine("Data Loading Failed.");
                locations = new List<Location>();
                orders = new List<Order>();
                customers = new List<Customer>();
                items = new List<Item>();
            }
        }
        private void storeData() 
        {
            File.WriteAllText(ordersDataPath, JsonSerializer.Serialize(this.orders)); 
            File.WriteAllText(locationsDataPath, JsonSerializer.Serialize(this.locations)); 
            File.WriteAllText(customersDataPath, JsonSerializer.Serialize(this.customers));
            File.WriteAllText(itemsDataPath, JsonSerializer.Serialize(this.items));
        }
        public void save() {
            storeData();
        }
         public void addCustomer(Customer customer)
        {
            this.Customers.Add(customer);
        }
        public List<Customer> getCustomersByName(string name) {
            return (from customer in this.Customers
                where customer.CustomerName == name
                select customer).ToList();
        }

        public List<Order> orderHistory(Customer customer)
        {
            return (from order in this.Orders
                where order.OrderCustomer == customer.CustomerId
                select order).ToList();
        }
        public List<Order> orderHistory(Location location) 
        {
            return (from order in this.Orders
                where order.OrderLocation == location.LocationId
                select order).ToList();
        }
        public void placeOrder(Order order)
        {
            var location = (from loc in Locations
                where loc.LocationId == order.OrderLocation select loc).First();
            foreach (var itemCount in order.Contents) 
            {
                if (location.Inventory[itemCount.Key] < itemCount.Value) 
                {
                    throw new ArgumentException("No inventory to fill order");
                }
                else location.Inventory[itemCount.Key] -= itemCount.Value;
            }
            this.Orders.Add(order);
        }
    }
}
