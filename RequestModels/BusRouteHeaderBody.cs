namespace BusRouteApi.RequestModels
{
    public class BusRouteHeaderBody
    {
        public RouteBody[] routes { get; set; }
        public ShiftBody[] shifts { get; set; }
    }
}