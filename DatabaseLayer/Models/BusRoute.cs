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
        public double RoutePrice { get; set; }
        public int OilPriceId { get; set; }
        public int VendorId { get; set; }
        public int RouteDistance { get; set; }
        public int RouteId { get; set; }

        public BusRoute(DateOnly busRouteDate, int busId, int shiftId, int routeId,
                        int oilPriceId, int vendorId, double routePrice, int routeDistance)
        {
            BusRouteDate = busRouteDate;
            BusId = busId;
            ShiftId = shiftId;
            RoutePrice = routePrice;
            RouteId = routeId;
            OilPriceId = oilPriceId;
            VendorId = vendorId;
            RouteDistance = routeDistance;
        }

        public virtual Bus Bus { get; set; } = null!;
        public virtual OilPrice OilPrice { get; set; } = null!;
        public virtual Route Route { get; set; } = null!;
        public virtual Shift Shift { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
    }
}
