using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class RoutePrice
    {
        public RoutePrice(DateOnly routeDate, double price, int routeId)
        {
            BusRoutes = new HashSet<BusRoute>();
            RouteDate = routeDate;
            Price = price;
            RouteId = routeId;
        }

        public int Id { get; set; }
        public DateOnly RouteDate { get; set; }
        public double Price { get; set; }
        public int RouteId { get; set; }

        public virtual Route Route { get; set; } = null!;
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
