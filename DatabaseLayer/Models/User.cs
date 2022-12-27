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
        public int VendorId { get; set; }
        public User(string username, string password, string name, string role, int vendorId)
        {
            Username = username;
            Password = password;
            Name = name;
            Role = role;
            VendorId = vendorId;
        }

        public virtual Vendor Vendor { get; set; } = null!;
    }
}
