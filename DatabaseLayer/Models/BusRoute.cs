using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class BusRoute
    {
        public int Id { get; set; }
        public DateOnly BusRouteDate { get; set; }
        public int BusId { get; set; }
        public int ShiftId { get; set; }
        public int RoutePriceId { get; set; }
        public int OilPriceId { get; set; }
        public int VendorId { get; set; }

        public virtual Bus Bus { get; set; } = null!;
        public virtual OilPrice OilPrice { get; set; } = null!;
        public virtual RoutePrice RoutePrice { get; set; } = null!;
        public virtual Shift Shift { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;

        public BusRoute(DateOnly busRouteDate, int busId, int shiftId, int routePriceId, int oilPriceId, int vendorId)
        {
            BusRouteDate = busRouteDate;
            BusId = busId;
            ShiftId = shiftId;
            RoutePriceId = routePriceId;
            OilPriceId = oilPriceId;
            VendorId = vendorId;
        }
    }
}
