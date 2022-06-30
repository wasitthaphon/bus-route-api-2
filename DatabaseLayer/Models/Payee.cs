using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Payee
    {
        public Payee(string name)
        {
            Bus = new HashSet<Bus>();
            VendorPayees = new HashSet<VendorPayee>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Bus> Bus { get; set; }
        public virtual ICollection<VendorPayee> VendorPayees { get; set; }
    }
}
