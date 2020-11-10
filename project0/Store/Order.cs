using System.Collections.Generic;

namespace Store
{
    public class Item
    { 
        private static uint itemTally;
        readonly uint itemId;
        private readonly string itemName;
        public Item(string itemName)
        {
            this.itemName = itemName;
            this.itemId = itemTally++;
        }
        public string ItemName => itemName;

        public uint ItemId => itemId;
        public override string ToString() {
            return this.itemName.ToString();
        }
    }
    public class Order
    {
        private static List<Item> items;
        // Convert all Dictionaries of items to dictonaries of itemIds
        private static uint orderTally;
        private Dictionary<uint, uint> contents;
        private readonly uint orderId;

        private readonly uint orderCustomer;
        private readonly uint orderLocation;

        public Dictionary<uint, uint> Contents { get => contents; set => contents = value; }

        public uint OrderCustomer => orderCustomer;

        public uint OrderLocation => orderLocation;

        public uint OrderId => orderId;

        public static List<Item> Items { get => items; set => items = value; }

        public override string ToString() {
            var result = "";
            foreach(var entry in this.Contents) 
            {
                if (entry.Value != 0) 
                {
                    result += items.Find(item => item.ItemId == entry.Key).ToString() + $": {entry.Value},";
                }
            }
            return result;
        }
        public Order(List<Item> items, Location loc, Customer cust) 
        {
            this.orderId = (uint) orderTally++;
            this.orderLocation = loc.LocationId;
            this.orderCustomer = cust.CustomerId;
            var dct = new Dictionary<uint, uint>();
            foreach (var item in items) {
                if (dct.ContainsKey(item.ItemId)) dct[item.ItemId]++;
                else dct.Add(item.ItemId, 1);
            }
            this.contents = dct;
        }
    }
}