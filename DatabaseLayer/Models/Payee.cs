using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Payee
    {
        public Payee(string name, int vendorId)
        {
            Bus = new HashSet<Bus>();
            VendorPayees = new HashSet<VendorPayee>();

            Name = name;
            VendorId = vendorId;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; } = null!;
        public virtual ICollection<Bus> Bus { get; set; }
        public virtual ICollection<VendorPayee> VendorPayees { get; set; }
    }
}
