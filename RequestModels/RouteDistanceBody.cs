namespace BusRouteApi.RequestModels
{
    public class RouteDistanceBody
    {
        public int Id { get; set; }
        public string RouteDate { get; set; }
        public int Distance { get; set; }
        public int RouteId { get; set; }
    }
}