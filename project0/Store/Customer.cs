using System.Text;
using System.Text.Json.Serialization;

namespace Store
{
    public class Customer
    {
        private static uint customerTally;
        private uint customerId;
        private string customerName;
        public string CustomerName { get => customerName; }
        public uint CustomerId { get => customerId; }
        public Customer(string name) {
            this.customerId = customerTally++;
            this.customerName = name;
        }
    }
}