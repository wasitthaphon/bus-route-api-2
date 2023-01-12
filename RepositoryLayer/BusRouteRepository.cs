using BusRouteApi.DatabaseLayer;
using BusRouteApi.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusRouteApi.RepositoryLayer
{
    public class BusRouteRepository
    {

        private readonly BusRouteDbContext _context;
        public BusRouteRepository(BusRouteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateBusRoute(BusRoute busRoute)
        {
            try
            {
                await _context.BusRoutes.AddAsync(busRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<bool> CreateBusRoutes(DateOnly busRouteDate, int vendorId, List<BusRoute> busRoutes)
        {
            try
            {
                Queue<BusRoute> busRoutesOld = new Queue<BusRoute>();

                foreach (BusRoute busRoute in await _context.BusRoutes.Where(busRoutes =>
                                        busRoutes.BusRouteDate == busRouteDate &&
                                        busRoutes.VendorId == vendorId).ToListAsync())
                {
                    busRoutesOld.Enqueue(busRoute);
                }


                await _context.Database.BeginTransactionAsync();

                if (busRoutesOld.Count > 0)
                {
                    _context.BusRoutes.RemoveRange(busRoutesOld);
                    await _context.SaveChangesAsync();
                }

                await _context.BusRoutes.AddRangeAsync(busRoutes);
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }

        public async Task<BusRoute> GetBusRoute(int id)
        {
            try
            {
                BusRoute busRoute = await _context.BusRoutes.FirstOrDefaultAsync(busRoute => busRoute.Id == id);

                if (busRoute == null)
                {
                    throw new Exception("Bus route not found");
                }

                return busRoute;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Queue<BusRoute>> GetBusRoutes(int routeId, DateOnly dateFrom, DateOnly dateTo)
        {
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();

            foreach (BusRoute busRoute in await _context.BusRoutes.
                                                Include(busRoutes => busRoutes.Route).
                                                Include(busRoutes => busRoutes.Bus).
                                                Include(busRoutes => busRoutes.Shift).
                                        Where(busRoutes => busRoutes.RouteId == routeId &&
                                        busRoutes.BusRouteDate >= dateFrom && busRoutes.BusRouteDate <= dateTo)
                                        .OrderBy(busRoutes => busRoutes.BusRouteDate)
                                        .ToListAsync())
            {
                busRoutes.Enqueue(busRoute);
            }

            return busRoutes;
        }

        public async Task<Queue<BusRoute>> GetBusRoutes(DateOnly busRouteDate, int vendorId)
        {
            Queue<BusRoute> busRoutes = new Queue<BusRoute>();

            foreach (BusRoute busRoute in await _context.BusRoutes.
                                    Include(busRoutes => busRoutes.Route).
                                    Include(busRoutes => busRoutes.Bus).
                                    Include(busRoutes => busRoutes.Shift).
            Where(busRoutes => busRoutes.BusRouteDate == busRouteDate &&
                              busRoutes.VendorId == vendorId)
            .OrderBy(busRoutes => busRoutes.Route.Name)
            .ToListAsync())
            {
                busRoutes.Enqueue(busRoute);
            }

            return busRoutes;
        }

        public async Task<BusRoute> GetLatestDate(int vendorId)
        {
            BusRoute busRoute = await _context.BusRoutes.Where(busRoute => busRoute.VendorId == vendorId).OrderByDescending(busRoute => busRoute.BusRouteDate).FirstAsync();

            return busRoute;
        }

        public async Task<bool> UpdateBusRoute(BusRoute busRoute)
        {
            try
            {
                _context.BusRoutes.Update(busRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteBusRoute(BusRoute busRoute)
        {
            try
            {
                _context.BusRoutes.Remove(busRoute);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

                throw e;
            }

            return true;
        }

        public async Task<bool> DeleteBusRoutes(List<BusRoute> busRoutes)
        {
            try
            {
                _context.BusRoutes.RemoveRange(busRoutes);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            return true;
        }
    }
}