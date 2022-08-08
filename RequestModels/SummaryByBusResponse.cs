namespace BusRouteApi.RequestModels
{
    public class SummaryByBusResponse
    {
        public string[] Shifts { get; set; }
        public SummaryOfDateRoute[] SummaryOfDateRoutes { get; set; }
        public double TotalPrice { get; set; }
        public bool IsQueryFound { get; set; }
    }

    public class SummaryOfDateRoute
    {
        public string Date { get; set; }
        public string[] Routes { get; set; }
    }
}