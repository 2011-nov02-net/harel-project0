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
            Sorders = new HashSet<Sorder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationItem> LocationItems { get; set; }
        public virtual ICollection<Sorder> Sorders { get; set; }
    }
}
