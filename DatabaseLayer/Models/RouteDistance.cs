using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class RouteDistance
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public DateOnly RouteDate { get; set; }
        public int RouteId { get; set; }

        public RouteDistance(DateOnly routeDate, int distance, int routeId)
        {
            RouteDate = routeDate;
            Distance = distance;
            RouteId = routeId;
        }

        public virtual Route Route { get; set; } = null!;
    }
}
