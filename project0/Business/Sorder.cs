using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class Sorder
    {
        public Sorder()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int LocationId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
