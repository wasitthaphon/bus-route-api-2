namespace BusRouteApi.RequestModels
{
    public class RouteBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RouteType { get; set; }
        public string Status { get; set; }
        public int VendorId { get; set; }
    }
}