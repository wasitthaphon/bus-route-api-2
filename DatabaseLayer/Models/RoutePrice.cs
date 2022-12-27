﻿using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class RoutePrice
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int RouteId { get; set; }
        public int OilPriceReference { get; set; }

        public RoutePrice(int oilPriceReference, double price, int routeId)
        {
            OilPriceReference = oilPriceReference;
            Price = price;
            RouteId = routeId;
        }

        public virtual Route Route { get; set; } = null!;
    }
}
