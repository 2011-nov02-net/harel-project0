using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Store
{
    public class Order
    {
        private static List<Item> items;
        private static uint orderTally;
        private Dictionary<uint, uint> contents;
        private readonly uint orderId;

        private readonly uint orderCustomer;
        private readonly uint orderLocation;

        [JsonIgnore]
        public Dictionary<uint, uint> Contents { get => contents; set => contents = value; }
        public Dictionary<string, uint> ContentsS 
        { 
            get 
            {
                var result = new Dictionary<string, uint>();
                foreach (var kv in contents)
                {
                    result.Add(kv.Key.ToString(), kv.Value);
                }
                return result;
            }
            set 
            {
                var result = new Dictionary<uint, uint>();
                foreach (var kv in value) 
                {
                    result.Add(Convert.ToUInt32(kv.Key), kv.Value);
                }
                contents = result;
            } 
        }

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