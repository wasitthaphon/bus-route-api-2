namespace BusRouteApi.RequestModels
{
    public class SummaryByPayeeRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int VendorId { get; set; }
    }
}