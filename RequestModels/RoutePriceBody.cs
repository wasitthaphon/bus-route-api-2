namespace BusRouteApi.RequestModels
{
    public class RoutePriceBody
    {
        public int Id { get; set; }
        public string RouteDate { get; set; }
        public double Price { get; set; }
        public int Distance { get; set; }
        public int RouteId { get; set; }
        public string RouteType { get; set; }
        public string RouteName { get; set; }
    }
}