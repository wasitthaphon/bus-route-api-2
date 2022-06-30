using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class OilPrice
    {
        public OilPrice(DateOnly oilPriceDate, double price)
        {
            BusRoutes = new HashSet<BusRoute>();
            OilPriceDate = oilPriceDate;
            Price = price;
        }

        public int Id { get; set; }
        public DateOnly OilPriceDate { get; set; }
        public double Price { get; set; }

        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
