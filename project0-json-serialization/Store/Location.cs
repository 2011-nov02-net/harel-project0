using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Store
{
    public class Location
    {
        private static uint locationTally;
        public readonly uint locationId;
        private Dictionary<uint, uint> inventory;
        public uint LocationId => locationId;
        public Dictionary<string, uint> InventoryS
        {
            get
            {
                var result = new Dictionary<string, uint>();
                foreach (var kv in inventory)
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
                inventory = result;
            }
        }
        [JsonIgnore]
        public Dictionary<uint, uint> Inventory { get => inventory; set => inventory = value; }

        internal Location() {
            this.locationId = locationTally++;
            this.inventory = new Dictionary<uint, uint>();
        }
    }
}