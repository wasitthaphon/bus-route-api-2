using System;
using System.Collections.Generic;
using BusRouteApi.Misc;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Passsword { get; set; } = null!;
        public string Name { get; set; } = null!;

        public Role Role { get; set; }

        public User(string username, string password, string name, Role role)
        {
            Username = username;
            Passsword = password;
            Name = name;
            Role = role;
        }
    }
}
