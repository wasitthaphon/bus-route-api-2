using BusRouteApi.DatabaseLayer.Models;

namespace BusRouteApi.RequestModels
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Role = user.Role.ToString();
            Token = token;
        }
    }
}