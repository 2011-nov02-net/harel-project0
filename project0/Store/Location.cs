using System.Collections.Generic;

namespace Store
{
    public class Location
    {
        public readonly uint locationId;
        Dictionary<uint, uint> inventory;
        public uint LocationId => locationId;
        internal Dictionary<uint, uint> Inventory { get => inventory; set => inventory = value; }
    }
}