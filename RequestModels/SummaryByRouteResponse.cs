namespace BusRouteApi.RequestModels
{
    public class SummaryByRouteResponse
    {
        public string RouteName { get; set; }
        public int LapCount { get; set; }
        public double TotalPrice { get; set; }
    }
}