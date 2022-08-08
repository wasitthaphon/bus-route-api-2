namespace BusRouteApi.RequestModels
{
    public class BusRouteBody
    {
        public BusRouteHeaderBody headers { get; set; }
        public BusRouteDetailBody[] details { get; set; }
        public string BusRouteDate { get; set; }
        public double OilPrice { get; set; }
        public string PageMode { get; set; }
    }
}