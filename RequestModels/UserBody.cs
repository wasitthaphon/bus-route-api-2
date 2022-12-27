namespace BusRouteApi.RequestModels
{
    public class UserBody
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int VendorId { get; set; }
    }
}