using System;
using System.Collections.Generic;
using System.Linq;

namespace Store
{
    interface IStore
    {
        void placeOrder(Order order);
        void addCustomer(Customer customer);
        IEnumerable<Order> orderHistory(Location location);
        IEnumerable<Order> orderHistory(Customer customer);
    }

    public class StoreFront : IStore
    {
        private List<Location> locations;
        private List<Order> orders;
        List<Customer> customers;
        List<Item> items;

        public List<Order> Orders
        {
            get => orders;
            set
            {
                List<Order> oldOrders = orders;
                orders = value;
                // location??
            }
        }

        public StoreFront () {
            locations = new List<Location>();
            Orders = new List<Order>();
            customers = new List<Customer>();
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
            this.customers.Add(customer);
        }

        public List<Order> orderHistory(Customer customer)
        {
            return (List<Order>) from order in this.Orders
                where order.OrderCustomer == customer.CustomerId
                select order;
        }
        public void placeOrder(Order order)
        {
            var location = (from loc in locations
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
        public List<Order> orderHistory(Location location) 
        {
            return (List<Order>) from order in this.Orders
                where order.OrderLocation == location.LocationId
                select order;
        }
    }
}
