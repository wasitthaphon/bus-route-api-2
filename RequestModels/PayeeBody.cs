using BusRouteApi.DatabaseLayer.Models;

namespace BusRouteApi.RequestModels
{
    public class PayeeBody
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VendorId { get; set; }

        public PayeeBody() { }

        public PayeeBody(Payee payee)
        {
            Id = payee.Id;
            Name = payee.Name;
            VendorId = payee.VendorId;
        }
    }
}