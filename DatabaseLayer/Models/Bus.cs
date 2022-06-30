using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Bus
    {
        public Bus(string busNumber, int payeeId)
        {
            BusRoutes = new HashSet<BusRoute>();
            BusNumber = busNumber;
            PayeeId = payeeId;
        }

        public int Id { get; set; }
        public string BusNumber { get; set; } = null!;
        public int PayeeId { get; set; }

        public virtual Payee Payee { get; set; } = null!;
        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
