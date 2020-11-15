using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class Item
    {
        public Item()
        {
            LocationItems = new HashSet<LocationItem>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationItem> LocationItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
