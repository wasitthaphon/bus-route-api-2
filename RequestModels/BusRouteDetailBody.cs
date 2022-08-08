namespace BusRouteApi.RequestModels
{
    public class BusRouteDetailBody
    {
        public BusOnShift[] BusOnShifts { get; set; }
        public string Route { get; set; }
        public bool IsSpecialRoute { get; set; }
    }

    public class BusOnShift
    {
        public string Shift { get; set; }
        public string BusNumber { get; set; }
    }
}