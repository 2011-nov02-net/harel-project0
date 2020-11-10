using System.Collections.Generic;

namespace Store
{
    public class Location
    {
        private static uint locationTally;
        public readonly uint locationId;
        private Dictionary<Item, uint> inventory;
        public uint LocationId => locationId;
        internal Dictionary<Item, uint> Inventory { get => inventory; set => inventory = value; }
        internal Location() {
            this.locationId = locationTally++;
            this.inventory = new Dictionary<Item, uint>();
        }
    }
}