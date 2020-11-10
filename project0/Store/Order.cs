using System.Collections.Generic;

namespace Store
{
    public class Item
    { 
        private static ulong itemTally;
        readonly ulong itemId;
        private readonly string itemName;
        Item(string itemName)
        {
            this.itemName = itemName;
            this.itemId = itemTally++;
            itemTally++;
        }
        public string ItemName => itemName;

        public ulong ItemId => itemId;
        public override string ToString() {
            return this.itemName.ToString();
        }
    }
    public class Order
    {
        private static ulong orderTally;
        private Dictionary<Item, uint> contents;
        private readonly uint orderId;

        private readonly uint orderCustomer;
        private readonly uint orderLocation;

        internal Dictionary<Item, uint> Contents { get => contents; set => contents = value; }

        public uint OrderCustomer => orderCustomer;

        public uint OrderLocation => orderLocation;

        public uint OrderId => orderId;

        public override string ToString() {
            var result = "";
            foreach(KeyValuePair<Item, uint> entry in this.Contents) 
            {
                if (entry.Value == 0) continue;
                result += entry.Key.ToString()+$": {entry.Value},";
            }
            return result;
        }
        public Order(List<Item> items, Location loc, Customer cust) 
        {
            this.orderId = (uint) orderTally++;
            this.orderLocation = loc.LocationId;
            this.orderCustomer = cust.CustomerId;
            var dct = new Dictionary<Item, uint>();
            foreach (Item item in items) {
                if (dct.ContainsKey(item)) dct[item]++;
                else dct.Add(item, 1);
            }
            this.contents = dct;
        }
    }
}