namespace BusRouteApi.RequestModels
{
    public class SummaryByBusRequest
    {
        public string BusNumber { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}