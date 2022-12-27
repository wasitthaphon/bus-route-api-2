using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using BusRouteApi.Helpers;
using BusRouteApi.RequestModels;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class SummaryRepository
    {
        private readonly BusRouteDbContext _context;
        public SummaryRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<Queue<BusRoute>> GetSummaryByBus(BusBody bus, DateOnly dateFrom, DateOnly dateTo)
        {
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();
            Exception e;

            try
            {

                foreach (BusRoute busRoute in await _context.BusRoutes
                                            .Where(busRoutes =>
                                            busRoutes.BusRouteDate >= dateFrom && busRoutes.BusRouteDate <= dateTo && bus.Id == busRoutes.BusId)
                                            .Include(busRoutes => busRoutes.Bus)
                                            .Include(busRoutes => busRoutes.Route)
                                            .Include(busRoutes => busRoutes.OilPrice)
                                            .OrderBy(busRoutes => busRoutes.BusRouteDate)
                                            .ToListAsync())
                {
                    busRoutes.Enqueue(busRoute);
                }

                return busRoutes;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Queue<BusRoute>> GetSummaryByRoute(DateOnly fromDate, DateOnly toDate)
        {
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();
            Exception e;

            try
            {
                foreach (BusRoute busRoute in await _context.BusRoutes
                                            .Where(busRoutes =>
                                            busRoutes.BusRouteDate >= fromDate && busRoutes.BusRouteDate <= toDate)
                                            .Include(busRoutes => busRoutes.Route)
                                            .Include(busRoutes => busRoutes.OilPrice)
                                            .OrderBy(busRoutes => busRoutes.BusRouteDate).ToListAsync())
                {
                    busRoutes.Enqueue(busRoute);
                }

                return busRoutes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Queue<BusRoute>> GetSummaryByPayee(DateOnly dateFrom, DateOnly dateTo, int vendorId)
        {

            try
            {
                Queue<BusRoute> busRoutes = new Queue<BusRoute>();
                Exception e;

                foreach (BusRoute busRoute in await _context.BusRoutes
                                            .Include(busRoutes => busRoutes.Bus)
                                            .ThenInclude(busRoutes => busRoutes.Payee)
                                            .Where(busRoutes =>
                                            busRoutes.BusRouteDate >= dateFrom &&
                                            busRoutes.BusRouteDate <= dateTo && busRoutes.VendorId == vendorId)
                                            .Include(busRoutes => busRoutes.Route)
                                            .Include(busRoutes => busRoutes.OilPrice)
                                            .OrderBy(busRoutes => busRoutes.BusRouteDate)
                                            .ToListAsync())
                {
                    busRoutes.Enqueue(busRoute);
                }

                return busRoutes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}