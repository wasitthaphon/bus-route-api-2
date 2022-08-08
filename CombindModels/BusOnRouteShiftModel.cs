using BusRouteApi.DatabaseLayer.Models;

namespace BusRouteApi.CombindModel
{
    public class BusOnRouteShiftModel
    {
        public DatabaseLayer.Models.Route Route { get; set; }
        public List<BusShiftModel> busRoutes { get; set; }
    }

    public class BusShiftModel
    {
        public Shift Shift { get; set; }
        public Bus Bus { get; set; }
    }
}