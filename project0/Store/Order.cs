using System.Collections.Generic;

namespace Store
{
    class Item
    { 
        private static ulong itemTally;
        readonly ulong itemId;
        private readonly string itemName;
        public Item(string itemName)
        {
            this.itemName = itemName;
            this.itemId = itemTally++;
            itemTally++;
        }
        public string ItemName => itemName;

        public ulong ItemId => itemId;
    }
    public class Order
    {
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
                result += $"{entry.Key.ItemName}: {entry.Value},";
            }
            return result;
        }
    }
}