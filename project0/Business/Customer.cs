using System;
using System.Collections.Generic;

#nullable disable

namespace Business
{
    public partial class Customer
    {
        public Customer()
        {
            Sorders = new HashSet<Sorder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Sorder> Sorders { get; set; }
    }
}
