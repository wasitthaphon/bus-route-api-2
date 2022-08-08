namespace BusRouteApi.RequestModels
{
    public class VendorPayeeBody
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public int PayeeId { get; set; }
    }
}