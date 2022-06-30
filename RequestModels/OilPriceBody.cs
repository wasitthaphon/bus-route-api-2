namespace BusRouteApi.RequestModels
{
    public class OilPriceBody
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public Double Price { get; set; }

    }
}