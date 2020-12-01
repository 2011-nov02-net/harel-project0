using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class Location
    {
        public Location()
        {
            LocationItems = new HashSet<LocationItem>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationItem> LocationItems { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
