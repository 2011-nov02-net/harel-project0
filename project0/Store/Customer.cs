using System.Text;
using System.Text.Json.Serialization;

namespace Store
{
    public class Customer
    {
        [JsonIgnore]
        private static uint customerTally;
        [JsonPropertyName("customerId")]
        private uint customerId;
        [JsonPropertyName("customerId")]
        private string customerName;
        public string CustomerName { get => customerName; }
        public uint CustomerId { get => customerId; }
        public Customer(string name) {
            this.customerId = customerTally++;
            this.customerName = name;
        }
    }
}