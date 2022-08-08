using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Route
    {
        public Route(string name, int distance, string routeType)
        {
            RoutePrices = new HashSet<RoutePrice>();
            Name = name;
            Distance = distance;
            RouteType = routeType;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Distance { get; set; }
        public string? RouteType { get; set; }

        public virtual ICollection<RoutePrice> RoutePrices { get; set; }
    }
}
