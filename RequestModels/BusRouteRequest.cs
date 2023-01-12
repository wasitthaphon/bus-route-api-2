namespace BusRouteApi.RequestModels
{
    public class BusRouteRequest
    {
        public int RouteId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int VendorId { get; set; }
    }
}