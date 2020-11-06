using System.Collections.Generic;

namespace Store
{
    public class Order
    {
        Dictionary<Item, uint> contents;

        private class Item
        {
            
            private static ulong itemTally;
            readonly ulong itemId;
            public readonly string itemName;

            public Item(string itemName)
            {
                this.itemName = itemName;
                this.itemId = itemTally;
                itemTally++;
            }
        }

        public override string ToString() {
            var result = "";
            foreach(KeyValuePair<Item, uint> entry in this.contents) 
            {
                if (entry.Value == 0) continue;
                result += $"{entry.Key.itemName}: {entry.Value},";
            }
            return result;
        }
    }
    public class Customer
    {
        private static ulong customerTally;
        readonly ulong customerId;
        readonly string customerName;
        // public string orderHistory(List<IStore> stores) 
    }
}