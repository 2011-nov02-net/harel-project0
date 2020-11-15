using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class LocationItem
    {
        public int LocationId { get; set; }
        public int ItemId { get; set; }
        public int ItemCount { get; set; }

        public virtual Item Item { get; set; }
        public virtual Location Location { get; set; }
    }
}
