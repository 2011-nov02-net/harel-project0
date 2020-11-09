namespace Store
{
    public class Customer
    {
        private static ulong customerTally;
        private ulong customerId;
        private string customerName;
        public string CustomerName { get => customerName; }
        public ulong CustomerId { get => customerId; }
        public Customer(string name) {
            this.customerId = customerTally++;
            this.customerName = name;
        }
    }
}