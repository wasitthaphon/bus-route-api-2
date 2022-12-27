using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class OilPrice
    {
        public OilPrice(DateOnly oilPriceDate, double price, int vendorId)
        {
            BusRoutes = new HashSet<BusRoute>();
            OilPriceDate = oilPriceDate;
            Price = price;
            VendorId = vendorId;
        }

        public int Id { get; set; }
        public DateOnly OilPriceDate { get; set; }
        public double Price { get; set; }
        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; } = null!;
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
