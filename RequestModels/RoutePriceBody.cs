namespace BusRouteApi.RequestModels
{
    public class RoutePriceBody
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int RouteId { get; set; }
        public int OilPriceReference { get; set; }

    }
}