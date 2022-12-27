using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Route
    {
        public Route(string name, string routeType, int vendorId)
        {
            BusRoutes = new HashSet<BusRoute>();
            RouteDistances = new HashSet<RouteDistance>();
            RoutePrices = new HashSet<RoutePrice>();

            Name = name;
            RouteType = routeType;
            VendorId = vendorId;
            Status = "Active";
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string RouteType { get; set; } = null!;
        public int VendorId { get; set; }
        public string Status { get; set; } = null!;

        public virtual Vendor Vendor { get; set; } = null!;
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
        public virtual ICollection<RouteDistance> RouteDistances { get; set; }
        public virtual ICollection<RoutePrice> RoutePrices { get; set; }
    }
}
