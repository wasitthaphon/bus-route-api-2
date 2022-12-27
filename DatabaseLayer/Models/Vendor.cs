using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Vendor
    {
        public Vendor(string name)
        {
            Bus = new HashSet<Bus>();
            BusRoutes = new HashSet<BusRoute>();
            OilPrices = new HashSet<OilPrice>();
            Payees = new HashSet<Payee>();
            Routes = new HashSet<Route>();
            Users = new HashSet<User>();
            VendorPayees = new HashSet<VendorPayee>();

            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Bus> Bus { get; set; }
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
        public virtual ICollection<OilPrice> OilPrices { get; set; }
        public virtual ICollection<Payee> Payees { get; set; }
        public virtual ICollection<Route> Routes { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<VendorPayee> VendorPayees { get; set; }
    }
}
