using System.Collections.Generic;

namespace Store
{
    public class Location
    {
        public readonly uint locationId;
        private Dictionary<Item, uint> inventory;
        public uint LocationId => locationId;
        internal Dictionary<Item, uint> Inventory { get => inventory; set => inventory = value; }
    }
}