using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class OrderItem
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int ItemCount { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
