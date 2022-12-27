using System;
using System.Collections.Generic;

namespace BusRouteApi.DatabaseLayer.Models
{
    public partial class Shift
    {
        public Shift(int sequence, string name)
        {
            BusRoutes = new HashSet<BusRoute>();

            Sequence = sequence;
            Name = name;
        }

        public int Id { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<BusRoute> BusRoutes { get; set; }
    }
}
