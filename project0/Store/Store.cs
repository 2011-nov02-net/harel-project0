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
                Locations = new List<Location>();
                Orders = new List<Order>();
                Customers = new List<Customer>();
            }
        }
        private void storeData(string pathOrders) 
        {
            File.WriteAllText(ordersDataPath, JsonSerializer.Serialize(this.orders)); 
            File.WriteAllText(locationsDataPath, JsonSerializer.Serialize(this.locations)); 
            File.WriteAllText(customersDataPath, JsonSerializer.Serialize(this.customers));
            File.WriteAllText(itemsDataPath, JsonSerializer.Serialize(this.items));
        }
         public void addCustomer(Customer customer)
        {
            this.Customers.Add(customer);
        }
        public List<Customer> getCustomersByName(string name) {
            return (List<Customer>) from customer in this.Customers
                where customer.CustomerName == name
                select customer;
        }

        public List<Order> orderHistory(Customer customer)
        {
            return (List<Order>) from order in this.Orders
                where order.OrderCustomer == customer.CustomerId
                select order;
        }
        public List<Order> orderHistory(Location location) 
        {
            return (List<Order>) from order in this.Orders
                where order.OrderLocation == location.LocationId
                select order;
        }
        public void placeOrder(Order order)
        {
            var location = (from loc in Locations
                where loc.LocationId == order.OrderLocation select loc).First();
            foreach (KeyValuePair<Item, uint> itemCount in order.Contents) 
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
