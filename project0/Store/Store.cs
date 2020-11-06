using System;

namespace Store
{
    interface IStore
    {
        void LoadInventoryCSV(string path);
        void WriteInventoryCSV(string path);
        void LoadOrdersCSV(string path);
        void WriteOrdersCSV(string path);
        void placeOrders(Order order);
        void addCustomer(Customer customer);
        string orderHistory();
    }
    public class Business : IStore
    {
        public void addCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void LoadInventoryCSV(string path)
        {
            throw new NotImplementedException();
        }

        public void LoadOrdersCSV(string path)
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

        public void WriteInventoryCSV(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteOrdersCSV(string path)
        {
            throw new NotImplementedException();
        }
    }
}
