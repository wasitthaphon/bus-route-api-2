using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Bus
    {
        public Bus(string busNumber, int payeeId, int vendorId)
        {
            BusRoutes = new HashSet<BusRoute>();

            BusNumber = busNumber;
            PayeeId = payeeId;
            VendorId = vendorId;
        }

        public int Id { get; set; }
        public string BusNumber { get; set; } = null!;
        public int PayeeId { get; set; }
        public int VendorId { get; set; }

        public virtual Payee Payee { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
