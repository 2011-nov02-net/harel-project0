using System;
using System.Collections.Generic;
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

        public StoreFront () {
            Locations = new List<Location>();
            Orders = new List<Order>();
            Customers = new List<Customer>();
        }
        private List<Location> readLocations(string path) 
        {
            throw new NotImplementedException();
        }
        private List<Location> writeLocations(string path) 
        {
            throw new NotImplementedException();
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
