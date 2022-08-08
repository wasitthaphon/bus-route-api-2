using BusRouteApi.DatabaseLayer.Models;

namespace BusRouteApi.RequestModels
{
    public class BusBody
    {
        public int Id { get; set; }
        public string BusNumber { get; set; }
        public string PayeeName { get; set; }

        public BusBody() { }

        public BusBody(Bus bus)
        {
            Id = bus.Id;
            BusNumber = bus.BusNumber;
            PayeeName = bus.Payee.Name;
        }
    }
}