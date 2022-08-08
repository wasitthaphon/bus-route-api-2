using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;

        public User(string username, string password, string name, string role)
        {
            Username = username;
            Password = password;
            Name = name;
            Role = role;
        }
    }
}
