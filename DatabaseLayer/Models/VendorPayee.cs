using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class VendorPayee
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int PayeeId { get; set; }
        public VendorPayee(int vendorId, int payeeId)
        {
            VendorId = vendorId;
            PayeeId = payeeId;
        }
        public virtual Payee Payee { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
    }
}
