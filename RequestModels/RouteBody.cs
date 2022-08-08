namespace BusRouteApi.RequestModels
{
    public class RouteBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Distance { get; set; }
        public string RouteType { get; set; }
    }
}