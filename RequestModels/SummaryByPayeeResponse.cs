namespace BusRouteApi.RequestModels
{
    public class SummaryByPayeeResponse
    {
        public string PayeeName { get; set; }
        public double NMBPrice { get; set; }
        public double PayeeTotalPrice { get; set; }
        public double TotalReceived { get; set; }
    }
}