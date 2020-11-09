using System;
using System.Collections.Generic;

namespace Store
{
    interface IStore
    {
        void LoadInventory(string path);
        void WriteInventory(string path);
        void placeOrders(Order order);
        void addCustomer(Customer customer);
        string orderHistory();
    }

    public class StoreFront : IStore
    {
        public StoreFront () {
            List<Location> locations = new List<Location>();
            List<Order> orders = new List<Order>();
            List<Customer> customers = new List<Customer>();
        }
        private List<Location> readLocations(string path) {
            throw new NotImplementedException();
        }
         public void addCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void LoadInventory(string path)
        {
            throw new NotImplementedException();
        }

        public string orderHistory()
        {
            throw new NotImplementedException();
        }

        public void placeOrders(Order order)
        {
            throw new NotImplementedException();
        }

        public void WriteInventory(string path)
        {
            throw new NotImplementedException();
        }
    }
}
