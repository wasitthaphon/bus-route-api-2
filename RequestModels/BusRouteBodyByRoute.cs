namespace BusRouteApi.RequestModels
{
    public class BusRouteBodyByRoute
    {
        public string BusRouteDate { get; set; }
        public string Route { get; set; }
        public BusOnShift[] BusOnShifts { get; set; }
    }
}