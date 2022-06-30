using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Vendor
    {
        public Vendor(string name)
        {
            BusRoutes = new HashSet<BusRoute>();
            VendorPayees = new HashSet<VendorPayee>();
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<BusRoute> BusRoutes { get; set; }
        public virtual ICollection<VendorPayee> VendorPayees { get; set; }
    }
}
